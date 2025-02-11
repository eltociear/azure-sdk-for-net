// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.DataBoxEdge.Models
{
    public partial class DataBoxEdgeShippingAddress : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(AddressLine1))
            {
                writer.WritePropertyName("addressLine1"u8);
                writer.WriteStringValue(AddressLine1);
            }
            if (Optional.IsDefined(AddressLine2))
            {
                writer.WritePropertyName("addressLine2"u8);
                writer.WriteStringValue(AddressLine2);
            }
            if (Optional.IsDefined(AddressLine3))
            {
                writer.WritePropertyName("addressLine3"u8);
                writer.WriteStringValue(AddressLine3);
            }
            if (Optional.IsDefined(PostalCode))
            {
                writer.WritePropertyName("postalCode"u8);
                writer.WriteStringValue(PostalCode);
            }
            if (Optional.IsDefined(City))
            {
                writer.WritePropertyName("city"u8);
                writer.WriteStringValue(City);
            }
            if (Optional.IsDefined(State))
            {
                writer.WritePropertyName("state"u8);
                writer.WriteStringValue(State);
            }
            writer.WritePropertyName("country"u8);
            writer.WriteStringValue(Country);
            writer.WriteEndObject();
        }

        internal static DataBoxEdgeShippingAddress DeserializeDataBoxEdgeShippingAddress(JsonElement element)
        {
            Optional<string> addressLine1 = default;
            Optional<string> addressLine2 = default;
            Optional<string> addressLine3 = default;
            Optional<string> postalCode = default;
            Optional<string> city = default;
            Optional<string> state = default;
            string country = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("addressLine1"u8))
                {
                    addressLine1 = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("addressLine2"u8))
                {
                    addressLine2 = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("addressLine3"u8))
                {
                    addressLine3 = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("postalCode"u8))
                {
                    postalCode = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("city"u8))
                {
                    city = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("state"u8))
                {
                    state = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("country"u8))
                {
                    country = property.Value.GetString();
                    continue;
                }
            }
            return new DataBoxEdgeShippingAddress(addressLine1.Value, addressLine2.Value, addressLine3.Value, postalCode.Value, city.Value, state.Value, country);
        }
    }
}
