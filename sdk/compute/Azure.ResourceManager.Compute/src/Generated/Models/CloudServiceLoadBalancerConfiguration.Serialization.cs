// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.Compute.Models
{
    public partial class CloudServiceLoadBalancerConfiguration : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Id))
            {
                writer.WritePropertyName("id"u8);
                writer.WriteStringValue(Id);
            }
            writer.WritePropertyName("name"u8);
            writer.WriteStringValue(Name);
            writer.WritePropertyName("properties"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("frontendIpConfigurations"u8);
            writer.WriteStartArray();
            foreach (var item in FrontendIPConfigurations)
            {
                writer.WriteObjectValue(item);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        internal static CloudServiceLoadBalancerConfiguration DeserializeCloudServiceLoadBalancerConfiguration(JsonElement element)
        {
            Optional<ResourceIdentifier> id = default;
            string name = default;
            IList<LoadBalancerFrontendIPConfiguration> frontendIPConfigurations = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("id"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    id = new ResourceIdentifier(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("name"u8))
                {
                    name = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("properties"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    foreach (var property0 in property.Value.EnumerateObject())
                    {
                        if (property0.NameEquals("frontendIpConfigurations"u8))
                        {
                            List<LoadBalancerFrontendIPConfiguration> array = new List<LoadBalancerFrontendIPConfiguration>();
                            foreach (var item in property0.Value.EnumerateArray())
                            {
                                array.Add(LoadBalancerFrontendIPConfiguration.DeserializeLoadBalancerFrontendIPConfiguration(item));
                            }
                            frontendIPConfigurations = array;
                            continue;
                        }
                    }
                    continue;
                }
            }
            return new CloudServiceLoadBalancerConfiguration(id.Value, name, frontendIPConfigurations);
        }
    }
}
