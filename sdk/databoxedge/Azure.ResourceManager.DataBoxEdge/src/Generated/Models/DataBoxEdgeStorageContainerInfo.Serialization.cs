// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.DataBoxEdge.Models
{
    public partial class DataBoxEdgeStorageContainerInfo : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("storageAccountCredentialId"u8);
            writer.WriteStringValue(StorageAccountCredentialId);
            writer.WritePropertyName("containerName"u8);
            writer.WriteStringValue(ContainerName);
            writer.WritePropertyName("dataFormat"u8);
            writer.WriteStringValue(DataFormat.ToString());
            writer.WriteEndObject();
        }

        internal static DataBoxEdgeStorageContainerInfo DeserializeDataBoxEdgeStorageContainerInfo(JsonElement element)
        {
            ResourceIdentifier storageAccountCredentialId = default;
            string containerName = default;
            DataBoxEdgeStorageContainerDataFormat dataFormat = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("storageAccountCredentialId"u8))
                {
                    storageAccountCredentialId = new ResourceIdentifier(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("containerName"u8))
                {
                    containerName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("dataFormat"u8))
                {
                    dataFormat = new DataBoxEdgeStorageContainerDataFormat(property.Value.GetString());
                    continue;
                }
            }
            return new DataBoxEdgeStorageContainerInfo(storageAccountCredentialId, containerName, dataFormat);
        }
    }
}
