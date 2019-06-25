using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class User
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; }

        [Required]
        public string DeviceId { get; set; }

        [Required]
        public GamePlatform Platform { get; set; } = GamePlatform.Android;

        public string PushToken { get; set; }

        public string PushRegistrationId { get; set; }
    }

    public enum GamePlatform
    {
        Ios,
        Android
    }

}
