using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class ImageInfo
    {
        public string Url { get; set; }
        public string DescriptionUrl { get; set; }
        public string ThumbUrl { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string ArtistUrl { get; set; }
        public string LicenseUrl { get; set; }
        public string LicenseName { get; set; }
    }
}
