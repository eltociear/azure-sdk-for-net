// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.RecoveryServicesSiteRecovery.Models
{
    public partial class NewRecoveryVirtualNetwork : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(RecoveryVirtualNetworkResourceGroupName))
            {
                writer.WritePropertyName("recoveryVirtualNetworkResourceGroupName"u8);
                writer.WriteStringValue(RecoveryVirtualNetworkResourceGroupName);
            }
            if (Optional.IsDefined(RecoveryVirtualNetworkName))
            {
                writer.WritePropertyName("recoveryVirtualNetworkName"u8);
                writer.WriteStringValue(RecoveryVirtualNetworkName);
            }
            writer.WritePropertyName("resourceType"u8);
            writer.WriteStringValue(ResourceType);
            writer.WriteEndObject();
        }

        internal static NewRecoveryVirtualNetwork DeserializeNewRecoveryVirtualNetwork(JsonElement element)
        {
            Optional<string> recoveryVirtualNetworkResourceGroupName = default;
            Optional<string> recoveryVirtualNetworkName = default;
            string resourceType = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("recoveryVirtualNetworkResourceGroupName"u8))
                {
                    recoveryVirtualNetworkResourceGroupName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("recoveryVirtualNetworkName"u8))
                {
                    recoveryVirtualNetworkName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("resourceType"u8))
                {
                    resourceType = property.Value.GetString();
                    continue;
                }
            }
            return new NewRecoveryVirtualNetwork(resourceType, recoveryVirtualNetworkResourceGroupName.Value, recoveryVirtualNetworkName.Value);
        }
    }
}
