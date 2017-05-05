// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace ApplicationGateway.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Troubleshooting information gained from specified resource.
    /// </summary>
    public partial class TroubleshootingResult
    {
        /// <summary>
        /// Initializes a new instance of the TroubleshootingResult class.
        /// </summary>
        public TroubleshootingResult()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the TroubleshootingResult class.
        /// </summary>
        /// <param name="startTime">The start time of the
        /// troubleshooting.</param>
        /// <param name="endTime">The end time of the troubleshooting.</param>
        /// <param name="code">The result code of the troubleshooting.</param>
        /// <param name="results">Information from troubleshooting.</param>
        public TroubleshootingResult(System.DateTime? startTime = default(System.DateTime?), System.DateTime? endTime = default(System.DateTime?), string code = default(string), IList<TroubleshootingDetails> results = default(IList<TroubleshootingDetails>))
        {
            StartTime = startTime;
            EndTime = endTime;
            Code = code;
            Results = results;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the start time of the troubleshooting.
        /// </summary>
        [JsonProperty(PropertyName = "startTime")]
        public System.DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the troubleshooting.
        /// </summary>
        [JsonProperty(PropertyName = "endTime")]
        public System.DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the result code of the troubleshooting.
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets information from troubleshooting.
        /// </summary>
        [JsonProperty(PropertyName = "results")]
        public IList<TroubleshootingDetails> Results { get; set; }

    }
}
