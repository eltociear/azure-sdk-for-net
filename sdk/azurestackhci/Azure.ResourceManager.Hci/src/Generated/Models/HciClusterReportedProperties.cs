// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace Azure.ResourceManager.Hci.Models
{
    /// <summary> Properties reported by cluster agent. </summary>
    public partial class HciClusterReportedProperties
    {
        /// <summary> Initializes a new instance of HciClusterReportedProperties. </summary>
        internal HciClusterReportedProperties()
        {
            Nodes = new ChangeTrackingList<HciClusterNode>();
        }

        /// <summary> Initializes a new instance of HciClusterReportedProperties. </summary>
        /// <param name="clusterName"> Name of the on-prem cluster connected to this resource. </param>
        /// <param name="clusterId"> Unique id generated by the on-prem cluster. </param>
        /// <param name="clusterVersion"> Version of the cluster software. </param>
        /// <param name="nodes"> List of nodes reported by the cluster. </param>
        /// <param name="lastUpdatedOn"> Last time the cluster reported the data. </param>
        /// <param name="imdsAttestation"> IMDS attestation status of the cluster. </param>
        /// <param name="diagnosticLevel"> Level of diagnostic data emitted by the cluster. </param>
        internal HciClusterReportedProperties(string clusterName, Guid? clusterId, string clusterVersion, IReadOnlyList<HciClusterNode> nodes, DateTimeOffset? lastUpdatedOn, ImdsAttestationState? imdsAttestation, HciClusterDiagnosticLevel? diagnosticLevel)
        {
            ClusterName = clusterName;
            ClusterId = clusterId;
            ClusterVersion = clusterVersion;
            Nodes = nodes;
            LastUpdatedOn = lastUpdatedOn;
            ImdsAttestation = imdsAttestation;
            DiagnosticLevel = diagnosticLevel;
        }

        /// <summary> Name of the on-prem cluster connected to this resource. </summary>
        public string ClusterName { get; }
        /// <summary> Unique id generated by the on-prem cluster. </summary>
        public Guid? ClusterId { get; }
        /// <summary> Version of the cluster software. </summary>
        public string ClusterVersion { get; }
        /// <summary> List of nodes reported by the cluster. </summary>
        public IReadOnlyList<HciClusterNode> Nodes { get; }
        /// <summary> Last time the cluster reported the data. </summary>
        public DateTimeOffset? LastUpdatedOn { get; }
        /// <summary> IMDS attestation status of the cluster. </summary>
        public ImdsAttestationState? ImdsAttestation { get; }
        /// <summary> Level of diagnostic data emitted by the cluster. </summary>
        public HciClusterDiagnosticLevel? DiagnosticLevel { get; }
    }
}
