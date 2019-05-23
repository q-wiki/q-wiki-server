// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace WikidataGame.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class MiniGame
    {
        /// <summary>
        /// Initializes a new instance of the MiniGame class.
        /// </summary>
        public MiniGame()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the MiniGame class.
        /// </summary>
        public MiniGame(string id = default(string), int? type = default(int?), string taskDescription = default(string), IList<string> answerOptions = default(IList<string>))
        {
            Id = id;
            Type = type;
            TaskDescription = taskDescription;
            AnswerOptions = answerOptions;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public int? Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "taskDescription")]
        public string TaskDescription { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "answerOptions")]
        public IList<string> AnswerOptions { get; set; }

    }
}
