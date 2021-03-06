// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
// 
// Code generated by Microsoft (R) AutoRest Code Generator 0.15.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.Logic.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Microsoft.Rest.Azure;

    /// <summary>
    /// </summary>
    public partial class RunWorkflowParameters
    {
        /// <summary>
        /// Initializes a new instance of the RunWorkflowParameters class.
        /// </summary>
        public RunWorkflowParameters() { }

        /// <summary>
        /// Initializes a new instance of the RunWorkflowParameters class.
        /// </summary>
        public RunWorkflowParameters(string name = default(string), object outputs = default(object))
        {
            Name = name;
            Outputs = outputs;
        }

        /// <summary>
        /// Gets or sets the name of workflow run trigger.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the outputs of workflow run trigger.
        /// </summary>
        [JsonProperty(PropertyName = "outputs")]
        public object Outputs { get; set; }

    }
}
