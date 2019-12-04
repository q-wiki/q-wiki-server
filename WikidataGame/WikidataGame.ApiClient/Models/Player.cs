// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace WikidataGame.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class Player
    {
        /// <summary>
        /// Initializes a new instance of the Player class.
        /// </summary>
        public Player()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Player class.
        /// </summary>
        public Player(System.Guid? id = default(System.Guid?), string name = default(string), string profileImage = default(string))
        {
            Id = id;
            Name = name;
            ProfileImage = profileImage;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public System.Guid? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "profileImage")]
        public string ProfileImage { get; set; }

    }
}
