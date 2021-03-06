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
using System.Linq;

namespace Microsoft.Azure.Management.StreamAnalytics.Models
{
    /// <summary>
    /// The running state of the job.
    /// </summary>
    public static partial class JobRunningState
    {
        /// <summary>
        /// Created.
        /// </summary>
        public const string Created = "Created";
        
        /// <summary>
        /// Starting.
        /// </summary>
        public const string Starting = "Starting";
        
        /// <summary>
        /// Stopping.
        /// </summary>
        public const string Stopping = "Stopping";
        
        /// <summary>
        /// Stopped.
        /// </summary>
        public const string Stopped = "Stopped";
        
        /// <summary>
        /// Deleting.
        /// </summary>
        public const string Deleting = "Deleting";
        
        /// <summary>
        /// Failed.
        /// </summary>
        public const string Failed = "Failed";
        
        /// <summary>
        /// No input bytes have been seen since the job was created or in the
        /// last 2 minutes. If a job is in the Idle state for a long period of
        /// time, it is likely that the input exists but there are no raw
        /// bytes to process.
        /// </summary>
        public const string Idle = "Idle";
        
        /// <summary>
        /// A nonzero amount of filtered input events has been successfully
        /// consumed by the Stream Analytics job. If a job is stuck in the
        /// Processing state without producing output, it is likely that the
        /// processing time window is large or the query logic is complicated.
        /// </summary>
        public const string Processing = "Processing";
        
        /// <summary>
        /// This state indicates that a Stream Analytics job is encountering
        /// one of the following errors: input/output communication errors,
        /// query errors, or retry-able run-time errors. To distinguish what
        /// type of error(s) the job is encountering, view the operation logs.
        /// </summary>
        public const string Degraded = "Degraded";
        
        /// <summary>
        /// Restarting.
        /// </summary>
        public const string Restarting = "Restarting";
    }
}
