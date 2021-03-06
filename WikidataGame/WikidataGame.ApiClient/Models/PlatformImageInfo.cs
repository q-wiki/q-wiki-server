// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace WikidataGame.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class PlatformImageInfo
    {
        /// <summary>
        /// Initializes a new instance of the PlatformImageInfo class.
        /// </summary>
        public PlatformImageInfo()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the PlatformImageInfo class.
        /// </summary>
        public PlatformImageInfo(string thumbUrl = default(string), string licenseInfo = default(string))
        {
            ThumbUrl = thumbUrl;
            LicenseInfo = licenseInfo;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "thumbUrl")]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "licenseInfo")]
        public string LicenseInfo { get; set; }

    }
}
