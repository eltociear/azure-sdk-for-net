// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;
using Azure.ResourceManager.Models;

namespace Azure.ResourceManager.FrontDoor.Models
{
    /// <summary> Defines the Timeseries. </summary>
    public partial class FrontDoorTimeSeriesInfo : TrackedResourceData
    {
        /// <summary> Initializes a new instance of FrontDoorTimeSeriesInfo. </summary>
        /// <param name="location"> The location. </param>
        public FrontDoorTimeSeriesInfo(AzureLocation location) : base(location)
        {
            TimeSeriesData = new ChangeTrackingList<FrontDoorTimeSeriesDataPoint>();
        }

        /// <summary> Initializes a new instance of FrontDoorTimeSeriesInfo. </summary>
        /// <param name="id"> The id. </param>
        /// <param name="name"> The name. </param>
        /// <param name="resourceType"> The resourceType. </param>
        /// <param name="systemData"> The systemData. </param>
        /// <param name="tags"> The tags. </param>
        /// <param name="location"> The location. </param>
        /// <param name="endpoint"> The endpoint associated with the Timeseries data point. </param>
        /// <param name="startOn"> The start DateTime of the Timeseries in UTC. </param>
        /// <param name="endOn"> The end DateTime of the Timeseries in UTC. </param>
        /// <param name="aggregationInterval"> The aggregation interval of the Timeseries. </param>
        /// <param name="timeSeriesType"> The type of Timeseries. </param>
        /// <param name="country"> The country associated with the Timeseries. Values are country ISO codes as specified here- https://www.iso.org/iso-3166-country-codes.html. </param>
        /// <param name="timeSeriesData"> The set of data points for the timeseries. </param>
        internal FrontDoorTimeSeriesInfo(ResourceIdentifier id, string name, ResourceType resourceType, SystemData systemData, IDictionary<string, string> tags, AzureLocation location, Uri endpoint, DateTimeOffset? startOn, DateTimeOffset? endOn, FrontDoorTimeSeriesInfoAggregationInterval? aggregationInterval, FrontDoorTimeSeriesType? timeSeriesType, string country, IList<FrontDoorTimeSeriesDataPoint> timeSeriesData) : base(id, name, resourceType, systemData, tags, location)
        {
            Endpoint = endpoint;
            StartOn = startOn;
            EndOn = endOn;
            AggregationInterval = aggregationInterval;
            TimeSeriesType = timeSeriesType;
            Country = country;
            TimeSeriesData = timeSeriesData;
        }

        /// <summary> The endpoint associated with the Timeseries data point. </summary>
        public Uri Endpoint { get; set; }
        /// <summary> The start DateTime of the Timeseries in UTC. </summary>
        public DateTimeOffset? StartOn { get; set; }
        /// <summary> The end DateTime of the Timeseries in UTC. </summary>
        public DateTimeOffset? EndOn { get; set; }
        /// <summary> The aggregation interval of the Timeseries. </summary>
        public FrontDoorTimeSeriesInfoAggregationInterval? AggregationInterval { get; set; }
        /// <summary> The type of Timeseries. </summary>
        public FrontDoorTimeSeriesType? TimeSeriesType { get; set; }
        /// <summary> The country associated with the Timeseries. Values are country ISO codes as specified here- https://www.iso.org/iso-3166-country-codes.html. </summary>
        public string Country { get; set; }
        /// <summary> The set of data points for the timeseries. </summary>
        public IList<FrontDoorTimeSeriesDataPoint> TimeSeriesData { get; }
    }
}
