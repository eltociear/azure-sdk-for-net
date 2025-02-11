// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;
using Azure.ResourceManager.Storage;

namespace Azure.ResourceManager.Storage.Models
{
    internal partial class ListQueueResource
    {
        internal static ListQueueResource DeserializeListQueueResource(JsonElement element)
        {
            Optional<IReadOnlyList<StorageQueueData>> value = default;
            Optional<string> nextLink = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("value"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    List<StorageQueueData> array = new List<StorageQueueData>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(StorageQueueData.DeserializeStorageQueueData(item));
                    }
                    value = array;
                    continue;
                }
                if (property.NameEquals("nextLink"u8))
                {
                    nextLink = property.Value.GetString();
                    continue;
                }
            }
            return new ListQueueResource(Optional.ToList(value), nextLink.Value);
        }
    }
}
