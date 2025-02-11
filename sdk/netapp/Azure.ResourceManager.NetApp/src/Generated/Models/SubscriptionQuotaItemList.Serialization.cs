// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.NetApp.Models
{
    internal partial class SubscriptionQuotaItemList
    {
        internal static SubscriptionQuotaItemList DeserializeSubscriptionQuotaItemList(JsonElement element)
        {
            Optional<IReadOnlyList<NetAppSubscriptionQuotaItem>> value = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("value"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    List<NetAppSubscriptionQuotaItem> array = new List<NetAppSubscriptionQuotaItem>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(NetAppSubscriptionQuotaItem.DeserializeNetAppSubscriptionQuotaItem(item));
                    }
                    value = array;
                    continue;
                }
            }
            return new SubscriptionQuotaItemList(Optional.ToList(value));
        }
    }
}
