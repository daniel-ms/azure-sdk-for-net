// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
// 
// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.DataLake.Analytics.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Microsoft.Rest.Azure;

    /// <summary>
    /// Azure Storage blob container information.
    /// </summary>
    public partial class BlobContainer
    {
        /// <summary>
        /// Initializes a new instance of the BlobContainer class.
        /// </summary>
        public BlobContainer() { }

        /// <summary>
        /// Initializes a new instance of the BlobContainer class.
        /// </summary>
        public BlobContainer(string name = default(string), string id = default(string), string type = default(string), BlobContainerProperties properties = default(BlobContainerProperties))
        {
            Name = name;
            Id = id;
            Type = type;
            Properties = properties;
        }

        /// <summary>
        /// Gets or sets the name of the blob container.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the unique identifier of the blob container.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        /// <summary>
        /// Gets or sets the type of the blob container.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }

        /// <summary>
        /// Gets or sets the properties of the blob container.
        /// </summary>
        [JsonProperty(PropertyName = "properties")]
        public BlobContainerProperties Properties { get; private set; }

    }
}
