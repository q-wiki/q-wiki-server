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
        public string Id { get; set; }
        public string DeviceId { get; set; }
        public GamePlatform Platform { get; set; } = GamePlatform.Android;

        public string PushChannelUrl { get; set; }
    }

    public enum GamePlatform
    {
        Ios,
        Android
    }

}
