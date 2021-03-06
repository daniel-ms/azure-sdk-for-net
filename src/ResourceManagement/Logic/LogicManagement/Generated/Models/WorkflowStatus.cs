// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
// 
// Code generated by Microsoft (R) AutoRest Code Generator 0.15.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.Logic.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines values for WorkflowStatus.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkflowStatus
    {
        [EnumMember(Value = "NotSpecified")]
        NotSpecified,
        [EnumMember(Value = "Paused")]
        Paused,
        [EnumMember(Value = "Running")]
        Running,
        [EnumMember(Value = "Waiting")]
        Waiting,
        [EnumMember(Value = "Succeeded")]
        Succeeded,
        [EnumMember(Value = "Skipped")]
        Skipped,
        [EnumMember(Value = "Suspended")]
        Suspended,
        [EnumMember(Value = "Cancelled")]
        Cancelled,
        [EnumMember(Value = "Failed")]
        Failed,
        [EnumMember(Value = "Faulted")]
        Faulted,
        [EnumMember(Value = "TimedOut")]
        TimedOut,
        [EnumMember(Value = "Aborted")]
        Aborted
    }
}
