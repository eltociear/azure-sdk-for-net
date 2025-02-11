// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace Azure.ResourceManager.ProviderHub.Models
{
    /// <summary> The DefaultRolloutSpecificationRestOfTheWorldGroupOne. </summary>
    public partial class DefaultRolloutSpecificationRestOfTheWorldGroupOne : TrafficRegionRolloutConfiguration
    {
        /// <summary> Initializes a new instance of DefaultRolloutSpecificationRestOfTheWorldGroupOne. </summary>
        public DefaultRolloutSpecificationRestOfTheWorldGroupOne()
        {
        }

        /// <summary> Initializes a new instance of DefaultRolloutSpecificationRestOfTheWorldGroupOne. </summary>
        /// <param name="regions"></param>
        /// <param name="waitDuration"></param>
        internal DefaultRolloutSpecificationRestOfTheWorldGroupOne(IList<string> regions, TimeSpan? waitDuration) : base(regions, waitDuration)
        {
        }
    }
}
