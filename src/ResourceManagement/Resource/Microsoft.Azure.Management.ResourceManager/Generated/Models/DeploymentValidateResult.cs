// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
// 
// Code generated by Microsoft (R) AutoRest Code Generator 0.14.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.ResourceManager.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Microsoft.Rest.Azure;

    /// <summary>
    /// Information from validate template deployment response.
    /// </summary>
    public partial class DeploymentValidateResult
    {
        /// <summary>
        /// Initializes a new instance of the DeploymentValidateResult class.
        /// </summary>
        public DeploymentValidateResult() { }

        /// <summary>
        /// Initializes a new instance of the DeploymentValidateResult class.
        /// </summary>
        public DeploymentValidateResult(ResourceManagementErrorWithDetails error = default(ResourceManagementErrorWithDetails), DeploymentPropertiesExtended properties = default(DeploymentPropertiesExtended))
        {
            Error = error;
            Properties = properties;
        }

        /// <summary>
        /// Gets or sets validation error.
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public ResourceManagementErrorWithDetails Error { get; set; }

        /// <summary>
        /// Gets or sets the template deployment properties.
        /// </summary>
        [JsonProperty(PropertyName = "properties")]
        public DeploymentPropertiesExtended Properties { get; set; }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (this.Error != null)
            {
                this.Error.Validate();
            }
            if (this.Properties != null)
            {
                this.Properties.Validate();
            }
        }
    }
}
