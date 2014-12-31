// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.Automation.Models;
using Microsoft.WindowsAzure.Common.Internals;

namespace Microsoft.Azure.Management.Automation.Models
{
    /// <summary>
    /// The response model for the get job output operation.
    /// </summary>
    public partial class JobStreamListStreamItemsResponse : OperationResponseWithSkipToken
    {
        private IList<JobStreamItem> _jobStreamItems;
        
        /// <summary>
        /// Optional. A list of job stream items.
        /// </summary>
        public IList<JobStreamItem> JobStreamItems
        {
            get { return this._jobStreamItems; }
            set { this._jobStreamItems = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the JobStreamListStreamItemsResponse
        /// class.
        /// </summary>
        public JobStreamListStreamItemsResponse()
        {
            this.JobStreamItems = new LazyList<JobStreamItem>();
        }
    }
}