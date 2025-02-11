// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.RecoveryServicesSiteRecovery.Models
{
    public partial class OSDiskDetails
    {
        internal static OSDiskDetails DeserializeOSDiskDetails(JsonElement element)
        {
            Optional<string> osVhdId = default;
            Optional<string> osType = default;
            Optional<string> vhdName = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("osVhdId"u8))
                {
                    osVhdId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("osType"u8))
                {
                    osType = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("vhdName"u8))
                {
                    vhdName = property.Value.GetString();
                    continue;
                }
            }
            return new OSDiskDetails(osVhdId.Value, osType.Value, vhdName.Value);
        }
    }
}
