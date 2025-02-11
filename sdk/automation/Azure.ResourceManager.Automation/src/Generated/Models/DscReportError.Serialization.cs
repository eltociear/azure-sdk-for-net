// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.Automation.Models
{
    public partial class DscReportError
    {
        internal static DscReportError DeserializeDscReportError(JsonElement element)
        {
            Optional<string> errorSource = default;
            Optional<string> resourceId = default;
            Optional<string> errorCode = default;
            Optional<string> errorMessage = default;
            Optional<string> locale = default;
            Optional<string> errorDetails = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("errorSource"u8))
                {
                    errorSource = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("resourceId"u8))
                {
                    resourceId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("errorCode"u8))
                {
                    errorCode = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("errorMessage"u8))
                {
                    errorMessage = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("locale"u8))
                {
                    locale = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("errorDetails"u8))
                {
                    errorDetails = property.Value.GetString();
                    continue;
                }
            }
            return new DscReportError(errorSource.Value, resourceId.Value, errorCode.Value, errorMessage.Value, locale.Value, errorDetails.Value);
        }
    }
}
