// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.MachineLearning.Models
{
    public partial class MachineLearningJobScheduleAction : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("jobDefinition"u8);
            writer.WriteObjectValue(JobDefinition);
            writer.WritePropertyName("actionType"u8);
            writer.WriteStringValue(ActionType.ToString());
            writer.WriteEndObject();
        }

        internal static MachineLearningJobScheduleAction DeserializeMachineLearningJobScheduleAction(JsonElement element)
        {
            MachineLearningJobProperties jobDefinition = default;
            ScheduleActionType actionType = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("jobDefinition"u8))
                {
                    jobDefinition = MachineLearningJobProperties.DeserializeMachineLearningJobProperties(property.Value);
                    continue;
                }
                if (property.NameEquals("actionType"u8))
                {
                    actionType = new ScheduleActionType(property.Value.GetString());
                    continue;
                }
            }
            return new MachineLearningJobScheduleAction(actionType, jobDefinition);
        }
    }
}
