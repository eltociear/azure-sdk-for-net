// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.AppService.Models
{
    public partial class ApplicationLogsConfig : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(FileSystem))
            {
                writer.WritePropertyName("fileSystem"u8);
                writer.WriteObjectValue(FileSystem);
            }
            if (Optional.IsDefined(AzureTableStorage))
            {
                writer.WritePropertyName("azureTableStorage"u8);
                writer.WriteObjectValue(AzureTableStorage);
            }
            if (Optional.IsDefined(AzureBlobStorage))
            {
                writer.WritePropertyName("azureBlobStorage"u8);
                writer.WriteObjectValue(AzureBlobStorage);
            }
            writer.WriteEndObject();
        }

        internal static ApplicationLogsConfig DeserializeApplicationLogsConfig(JsonElement element)
        {
            Optional<FileSystemApplicationLogsConfig> fileSystem = default;
            Optional<AppServiceTableStorageApplicationLogsConfig> azureTableStorage = default;
            Optional<AppServiceBlobStorageApplicationLogsConfig> azureBlobStorage = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("fileSystem"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    fileSystem = FileSystemApplicationLogsConfig.DeserializeFileSystemApplicationLogsConfig(property.Value);
                    continue;
                }
                if (property.NameEquals("azureTableStorage"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    azureTableStorage = AppServiceTableStorageApplicationLogsConfig.DeserializeAppServiceTableStorageApplicationLogsConfig(property.Value);
                    continue;
                }
                if (property.NameEquals("azureBlobStorage"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    azureBlobStorage = AppServiceBlobStorageApplicationLogsConfig.DeserializeAppServiceBlobStorageApplicationLogsConfig(property.Value);
                    continue;
                }
            }
            return new ApplicationLogsConfig(fileSystem.Value, azureTableStorage.Value, azureBlobStorage.Value);
        }
    }
}
