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

    public partial class DetailedMiniGame
    {
        /// <summary>
        /// Initializes a new instance of the DetailedMiniGame class.
        /// </summary>
        public DetailedMiniGame()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the DetailedMiniGame class.
        /// </summary>
        public DetailedMiniGame(Question question = default(Question), IList<string> correctAnswer = default(IList<string>), string id = default(string), int? type = default(int?), string taskDescription = default(string), IList<string> answerOptions = default(IList<string>))
        {
            Question = question;
            CorrectAnswer = correctAnswer;
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
        [JsonProperty(PropertyName = "question")]
        public Question Question { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "correctAnswer")]
        public IList<string> CorrectAnswer { get; set; }

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

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Question != null)
            {
                Question.Validate();
            }
        }
    }
}