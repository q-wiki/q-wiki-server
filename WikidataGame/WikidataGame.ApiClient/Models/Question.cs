// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace WikidataGame.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class Question
    {
        /// <summary>
        /// Initializes a new instance of the Question class.
        /// </summary>
        public Question()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Question class.
        /// </summary>
        public Question(string sparqlQuery, string taskDescription, Category category, int miniGameType, string id = default(string), int? status = default(int?), double? rating = default(double?))
        {
            Id = id;
            SparqlQuery = sparqlQuery;
            TaskDescription = taskDescription;
            Category = category;
            MiniGameType = miniGameType;
            Status = status;
            Rating = rating;
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
        [JsonProperty(PropertyName = "sparqlQuery")]
        public string SparqlQuery { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "taskDescription")]
        public string TaskDescription { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "category")]
        public Category Category { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "miniGameType")]
        public int MiniGameType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "rating")]
        public double? Rating { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (SparqlQuery == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "SparqlQuery");
            }
            if (TaskDescription == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "TaskDescription");
            }
            if (Category == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Category");
            }
        }
    }
}