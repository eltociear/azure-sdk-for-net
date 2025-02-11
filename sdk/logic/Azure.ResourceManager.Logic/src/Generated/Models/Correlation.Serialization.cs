// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.Logic.Models
{
    internal partial class Correlation
    {
        internal static Correlation DeserializeCorrelation(JsonElement element)
        {
            Optional<string> clientTrackingId = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("clientTrackingId"u8))
                {
                    clientTrackingId = property.Value.GetString();
                    continue;
                }
            }
            return new Correlation(clientTrackingId.Value);
        }
    }
}
