using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class User : IdentityUser<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        [Required]
        public GamePlatform PushPlatform { get; set; } = GamePlatform.Android;

        public string PushToken { get; set; }

        public string PushRegistrationId { get; set; }

        public string ProfileImageUrl { get; set; }
    }

    public enum GamePlatform
    {
        Ios,
        Android
    }
}
