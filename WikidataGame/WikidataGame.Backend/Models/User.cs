using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string FirebaseUserId { get; set; }

        [Required]
        public string Username { get; set; }

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
