// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Monitor.Ingestion
{
    /// <summary> The IngestionUsingDataCollectionRules service client. </summary>
    [CodeGenClient("IngestionUsingDataCollectionRulesClient")]
    public partial class LogsIngestionClient
    {
        /// <summary> Initializes a new instance of LogsIngestionClient for mocking. </summary>
        protected LogsIngestionClient()
        {
        }

        // The size we use to determine whether to upload as a single PUT BLOB
        // request or stage as multiple blocks.
        // 1 Mb in byte format
        internal static int SingleUploadThreshold = 1024 * 1024;

        // For test purposes only
        // If Compression wants to be turned off (hard to generate 1 Mb data gzipped) set Compression to gzip
        internal static string Compression;

        internal readonly struct BatchedLogs
        {
            public BatchedLogs(List<object> logs, BinaryData logsData)
            {
                Logs = logs;
                LogsData = logsData;
            }

            public List<object> Logs { get; }
            public BinaryData LogsData { get; }
        }

        internal HttpMessage CreateUploadRequest(string ruleId, string streamName, RequestContent content, string contentEncoding, RequestContext context = null)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier204);
            var request = message.Request;
            request.Method = RequestMethod.Post;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/dataCollectionRules/", false);
            uri.AppendPath(ruleId, true);
            uri.AppendPath("/streams/", false);
            uri.AppendPath(streamName, true);
            uri.AppendQuery("api-version", _apiVersion, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Content-Type", "application/json");
            // If any encoding is specified, avoid gzipping. If contentEncoding == "gzip" that means content is already gzipped, so we shouldn't gzip again
            if (contentEncoding == null)
            {
                // contentEncoding is now "gzip"
                request.Headers.Add("Content-Encoding", "gzip");
                GZipUtf8JsonRequestContent gzContent = new(content);
                request.Content = gzContent;
            }
            else
            {
                request.Headers.Add("Content-Encoding", contentEncoding);
                request.Content = content;
            }
            return message;
        }

        /// <summary>
        /// Hidden method for batching data - serializing into arrays of JSON no more than SingleUploadThreshold each
        /// </summary>
        /// <param name="logEntries"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static IEnumerable<BatchedLogs> Batch<T>(IEnumerable<T> logEntries, UploadLogsOptions options = null)
        {
            // Create an ArrayBufferWriter as backing store for Utf8JsonWriter
            ArrayBufferWriter<byte> arrayBuffer = new ArrayBufferWriter<byte>(SingleUploadThreshold);
            Utf8JsonWriter writer = new Utf8JsonWriter(arrayBuffer);
            writer.WriteStartArray();
            int entryCount = 0;
            List<object> currentLogList = new List<object>();
            var logEntriesList = logEntries.ToList();
            int logEntriesCount = logEntriesList.Count;
            foreach (var log in logEntriesList)
            {
                BinaryData entry;
                bool isLastEntry = (entryCount + 1 == logEntriesCount);
                // If log is already BinaryData, no need to serialize it
                if (log is BinaryData d)
                    entry = d;
                // If log is not BinaryData, serialize it. Default Serializer is System.Text.Json
                else if (options == null || options.Serializer == null)
                    entry = BinaryData.FromObjectAsJson(log);
                // Otherwise use Serializer specified in options
                else
                    entry = options.Serializer.Serialize(log);

                var memory = entry.ToMemory();
                // if single log is > 1 Mb send to be gzipped by itself
                if (memory.Length > SingleUploadThreshold)
                {
                    // Create tempArrayBufferWriter (unsized to store log) and tempWriter for individual log
                    ArrayBufferWriter<byte> tempArrayBuffer = new ArrayBufferWriter<byte>();
                    Utf8JsonWriter tempWriter = new Utf8JsonWriter(tempArrayBuffer);
                    tempWriter.WriteStartArray();
                    WriteMemory(tempWriter, memory);
                    tempWriter.WriteEndArray();
                    tempWriter.Flush();
                    yield return new BatchedLogs(new List<object> { log }, BinaryData.FromBytes(tempArrayBuffer.WrittenMemory));
                }
                // if adding this entry makes stream > 1 Mb send current stream now
                else if ((writer.BytesPending + memory.Length + 1) >= SingleUploadThreshold)
                {
                    writer.WriteEndArray();
                    writer.Flush();
                    // This batch is full so send it now
                    yield return new BatchedLogs(currentLogList, BinaryData.FromBytes(arrayBuffer.WrittenMemory));

                    // Reset arrayBuffer and writer for next batch
                    arrayBuffer = new ArrayBufferWriter<byte>(SingleUploadThreshold);
                    writer.Reset(arrayBuffer);
                    writer.WriteStartArray();
                    // reset log list
                    currentLogList = new List<object>();
                    // add current log to memory and currentLogList
                    WriteMemory(writer, memory);
                    currentLogList.Add(log);

                    // if this is the last log, send batch now
                    if (isLastEntry)
                    {
                        writer.WriteEndArray();
                        writer.Flush();
                        yield return new BatchedLogs(currentLogList, BinaryData.FromBytes(arrayBuffer.WrittenMemory));
                    }
                }
                else
                {
                    // Add entry to existing stream and update logList
                    WriteMemory(writer, memory);
                    currentLogList.Add(log);

                    // if this is the last log, send batch now
                    if (isLastEntry)
                    {
                        writer.WriteEndArray();
                        writer.Flush();
                        yield return new BatchedLogs(currentLogList, BinaryData.FromBytes(arrayBuffer.WrittenMemory));
                    }
                }
                entryCount++;
            }
        }

        private static void WriteMemory(Utf8JsonWriter writer, ReadOnlyMemory<byte> memory)
        {
            using (JsonDocument doc = JsonDocument.Parse(memory))
            {
                // Comma separator added automatically by JsonDocument
                doc.RootElement.WriteTo(writer);
            }
        }

        /// <summary> Ingestion API used to directly ingest data using Data Collection Rules. </summary>
        /// <param name="ruleId"> The immutable Id of the Data Collection Rule resource. </param>
        /// <param name="streamName"> The streamDeclaration name as defined in the Data Collection Rule. </param>
        /// <param name="logs"> The content to send as the body of the request. Details of the request body schema are in the Remarks section below. </param>
        /// <param name="options"> The options model to configure the request to upload logs to Azure Monitor. </param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="ruleId"/>, <paramref name="streamName"/> or <paramref name="logs"/> is null. </exception>
        /// <exception cref="ArgumentException"> <paramref name="ruleId"/> or <paramref name="streamName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="RequestFailedException"> Service returned a non-Success status code. </exception>
        /// <returns> The response returned from the service. </returns>
        /// <example>
        /// This sample shows how to call Upload with required parameters and request content.
        /// <code><![CDATA[
        /// var credential = new DefaultAzureCredential();
        /// var endpoint = new Uri("<https://my-account-name.azure.com>");
        /// var client = new LogsIngestionClient(endpoint, credential);
        ///
        /// var data = new[] {
        ///     new {}
        /// };
        ///
        /// Response response = client.Upload("<ruleId>", "<streamName>", data);
        /// Console.WriteLine(response.Status);
        /// ]]></code>
        /// </example>
        /// <remarks> See error response code and error response message for more detail. </remarks>
        public virtual Response Upload<T>(string ruleId, string streamName, IEnumerable<T> logs, UploadLogsOptions options = null, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(ruleId, nameof(ruleId));
            Argument.AssertNotNullOrEmpty(streamName, nameof(streamName));
            Argument.AssertNotNullOrEmpty(logs, nameof(logs));

            using var scope = ClientDiagnostics.CreateScope("LogsIngestionClient.Upload");
            Response response = null;
            List<Exception> exceptions = null;
            options ??= new UploadLogsOptions();
            scope.Start();

            // Keep track of the number of failed logs across batches
            int logsFailed = 0;
            var shouldAbort = false;
            // Partition the stream into individual blocks
            foreach (BatchedLogs batch in Batch(logs, options))
            {
                //stop queuing new work on abort
                if (shouldAbort)
                    break;
                try
                {
                    // Because we are uploading in sequence, wait for each batch to upload before starting the next batch
                    response = UploadBatchListSyncOrAsync(
                        batch,
                        ruleId,
                        streamName,
                        async: false,
                        cancellationToken).EnsureCompleted();

                    if (response.Status != 204)
                    {
                        // if there is no Handler on options, throw exception otherwise raise Handler
                        if (!options.HasHandler)
                        {
                            throw new RequestFailedException(response);
                        }
                        else
                        {
                            var eventArgs = new UploadFailedEventArgs(batch.Logs, new RequestFailedException(response), isRunningSynchronously: true, ClientDiagnostics, cancellationToken);
#pragma warning disable AZC0106 // Non-public asynchronous method needs 'async' parameter.
                            // sync/async parameter in eventArgs
                            var ex = options.OnUploadFailedAsync(eventArgs).EnsureCompleted();
#pragma warning restore AZC0106 // Non-public asynchronous method needs 'async' parameter.
                            shouldAbort = ex != null;
                            if (shouldAbort)
                                AddException(ref exceptions, ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!options.HasHandler)
                    {
                        logsFailed += batch.Logs.Count;
                        // If we have an error, add Exception from response into exceptions list without throwing
                        AddException(
                            ref exceptions,
                            ex);
                    }
                    else
                    {
                        var eventArgs = new UploadFailedEventArgs(batch.Logs, new RequestFailedException(response), isRunningSynchronously: true, ClientDiagnostics, cancellationToken);
#pragma warning disable AZC0106 // Non-public asynchronous method needs 'async' parameter.
                        var exceptionOnUpload = options.OnUploadFailedAsync(eventArgs).EnsureCompleted();
#pragma warning restore AZC0106 // Non-public asynchronous method needs 'async' parameter.
                        shouldAbort = exceptionOnUpload != null;
                        if (shouldAbort)
                            AddException(ref exceptions, exceptionOnUpload);
                    }
                }
            }
            if (exceptions?.Count > 0)
            {
                var ex = new AggregateException($"{logsFailed} out of the {logs.Count()} logs failed to upload. Please check the InnerExceptions for more details.", exceptions);
                scope.Failed(ex);
                throw ex;
            }

            // If no exceptions return response
            return response; //204 - response of last batch with header
        }

        /// <summary> Ingestion API used to directly ingest data using Data Collection Rules. </summary>
        /// <param name="ruleId"> The immutable Id of the Data Collection Rule resource. </param>
        /// <param name="streamName"> The streamDeclaration name as defined in the Data Collection Rule. </param>
        /// <param name="logs"> The content to send as the body of the request. Details of the request body schema are in the Remarks section below. </param>
        /// <param name="options">  The options model to configure the request to upload logs to Azure Monitor. </param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="ruleId"/>, <paramref name="streamName"/> or <paramref name="logs"/> is null. </exception>
        /// <exception cref="ArgumentException"> <paramref name="ruleId"/> or <paramref name="streamName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="RequestFailedException"> Service returned a non-Success status code. </exception>
        /// <returns> The response returned from the service. </returns>
        /// <example>
        /// This sample shows how to call Upload with required parameters and request content.
        /// <code><![CDATA[
        /// var credential = new DefaultAzureCredential();
        /// var endpoint = new Uri("<https://my-account-name.azure.com>");
        /// var client = new LogsIngestionClient(endpoint, credential);
        ///
        /// var data = new[] {
        ///     new {}
        /// };
        ///
        /// Response response = client.Upload("<ruleId>", "<streamName>", data);
        /// Console.WriteLine(response.Status);
        /// ]]></code>
        /// </example>
        /// <remarks> See error response code and error response message for more detail. </remarks>
        public virtual async Task<Response> UploadAsync<T>(string ruleId, string streamName, IEnumerable<T> logs, UploadLogsOptions options = null, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(ruleId, nameof(ruleId));
            Argument.AssertNotNullOrEmpty(streamName, nameof(streamName));
            Argument.AssertNotNullOrEmpty(logs, nameof(logs));
            options ??= new UploadLogsOptions();

            using var scope = ClientDiagnostics.CreateScope("LogsIngestionClient.Upload");

            List<Exception> exceptions = null;
            var shouldAbort = false;
            scope.Start();

            // A list of tasks that are currently executing which will
            // always be smaller than or equal to MaxWorkerCount
            var runningTasks = new List<(Task<Response> CurrentTask, List<object> Logs)>();
            // Keep track of the number of failed logs across batches
            int logsFailed = 0;

            // Partition the stream into individual blocks
            foreach (BatchedLogs batch in Batch(logs, options))
            {
                if (shouldAbort)
                    break;
                try
                {
                    // Start staging the next batch (but don't await the Task!)
                    Task<Response> task = UploadBatchListSyncOrAsync(
                        batch,
                        ruleId,
                        streamName,
                        async: true,
                        cancellationToken);

                    // Add the block to our task and commit lists
                    runningTasks.Add((task, batch.Logs));

                    // If we run out of workers
                    if (runningTasks.Count >= options.MaxConcurrency)
                    {
                        // Wait for at least one of them to finish
                        await Task.WhenAny(runningTasks.Select(_ => _.CurrentTask)).ConfigureAwait(false);
                        // Clear any completed blocks from the task list
                        for (int i = 0; i < runningTasks.Count; i++)
                        {
                            Task<Response> runningTask = runningTasks[i].CurrentTask;
                            if (!runningTask.IsCompleted)
                            {
                                continue;
                            }
                            // Check completed task for Exception/RequestFailedException and increase logsFailed count
                            if (!options.HasHandler)
                            {
                                ProcessCompletedTask(runningTasks[i], ref exceptions, ref logsFailed);
                            }
                            else
                            {
                                Exception exceptionEventHandler = await ProcessCompletedTaskEventHandlerAsync(runningTask, batch.Logs, options, cancellationToken).ConfigureAwait(false);
                                shouldAbort = exceptionEventHandler != null;
                                if (shouldAbort)
                                    AddException(ref exceptions, exceptionEventHandler);
                            }
                            // Remove completed task from task list
                            runningTasks.RemoveAt(i);
                            i--;
                        }
                    }

                    // Wait for all the remaining blocks to finish uploading
                    await Task.WhenAll(runningTasks.Select(_ => _.CurrentTask)).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    // We do not want to log exceptions here as we will loop through all the tasks later
                }
            }

            // At this point, all tasks have completed. Examine tasks to see if they have exceptions. If Status code != 204, add RequestFailedException to list of exceptions. Increment logsFailed accordingly
            foreach (var task in runningTasks)
            {
                // Check completed task for Exception/RequestFailedException and increase logsFailed count
                if (!options.HasHandler)
                {
                    ProcessCompletedTask(task, ref exceptions, ref logsFailed);
                }
                else
                {
                    Exception exceptionEventHandler = await ProcessCompletedTaskEventHandlerAsync(task.CurrentTask, task.Logs, options, cancellationToken).ConfigureAwait(false);
                    shouldAbort = exceptionEventHandler != null;
                    if (shouldAbort)
                        AddException(ref exceptions, exceptionEventHandler);
                }
            }
            if (exceptions?.Count > 0)
            {
                var ex = new AggregateException($"{logsFailed} out of the {logs.Count()} logs failed to upload. Please check the InnerExceptions for more details.", exceptions);
                scope.Failed(ex);
                throw ex;
            }

            // If no exceptions return response
            return runningTasks.Select(_ => _.CurrentTask).Last().Result; //204 - response of last batch with header
        }

        private static void ProcessCompletedTask((Task<Response> CurrentTask, List<object> Logs) runningTask, ref List<Exception> exceptions, ref int logsFailed)
        {
            int logsCount = runningTask.Logs.Count;
            // If current task has an exception, log the exception and add number of logs in this task to failed logs
            if (runningTask.CurrentTask.Exception != null)
            {
                AddException(
                    ref exceptions,
                    runningTask.CurrentTask.Exception);
                logsFailed += logsCount;
            }
            // If current task returned a response that was not a success, log the exception and add number of logs in this task to failed logs
            else if (runningTask.CurrentTask.Result.Status != 204)
            {
                AddException(
                    ref exceptions,
                    new RequestFailedException(runningTask.CurrentTask.Result));
                logsFailed += logsCount;
            }
        }

        internal async Task<Exception> ProcessCompletedTaskEventHandlerAsync(Task<Response> completedTask, List<object> logs, UploadLogsOptions options, CancellationToken cancellationToken)
        {
            UploadFailedEventArgs eventArgs;
            if (completedTask.Exception != null)
            {
                eventArgs = new UploadFailedEventArgs(logs, completedTask.Exception, isRunningSynchronously: false, ClientDiagnostics, cancellationToken);
                return await options.OnUploadFailedAsync(eventArgs).ConfigureAwait(false);
            }
            else if (completedTask.Result.Status != 204)
            {
                eventArgs = new UploadFailedEventArgs(logs, new RequestFailedException(completedTask.Result), isRunningSynchronously: false, ClientDiagnostics, cancellationToken);
                return await options.OnUploadFailedAsync(eventArgs).ConfigureAwait(false);
            }
            else
            {
                return null;
            }
        }

        private async Task<Response> UploadBatchListSyncOrAsync(BatchedLogs batch, string ruleId, string streamName, bool async, CancellationToken cancellationToken)
        {
            using HttpMessage message = CreateUploadRequest(ruleId, streamName, batch.LogsData, Compression, null);

            if (async)
            {
                await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                _pipeline.Send(message, cancellationToken);
            }

            return message.Response;
        }

        private static void AddException(ref List<Exception> exceptions, Exception ex)
        {
            exceptions ??= new List<Exception>();
            exceptions.Add(ex);
        }
    }
}
