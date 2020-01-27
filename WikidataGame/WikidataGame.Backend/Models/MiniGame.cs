using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class MiniGame
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Game))]
        [Required]
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }

        [Required]
        public MiniGameType Type { get; set; }

        [Required]
        public string TaskDescription { get; set; }

        [NotMapped]
        public ICollection<string> AnswerOptions
        {
            get {
                return JsonConvert.DeserializeObject<List<string>>(AnswerOptionsString);
            }
            set
            {
                AnswerOptionsString = JsonConvert.SerializeObject(value);
            }
        }

        [Required]
        public string AnswerOptionsString { get; set; }

        [NotMapped]
        public ICollection<string> CorrectAnswer
        {
            get
            {
                return JsonConvert.DeserializeObject<List<string>>(CorrectAnswerString);
            }
            set
            {
                CorrectAnswerString = JsonConvert.SerializeObject(value);
            }
        }

        [Required]
        public string CorrectAnswerString { get; set; }

        [Required]
        public MiniGameStatus Status { get; set; } = MiniGameStatus.Unknown;

        [ForeignKey(nameof(User))]
        [Required]
        public Guid PlayerId { get; set; }
        public virtual User Player { get; set; }

        [ForeignKey(nameof(Tile))]
        [Required]
        public Guid TileId { get; set; }
        public virtual Tile Tile { get; set; }

        [ForeignKey(nameof(Category))]
        [Required]
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Required]
        public Guid QuestionId { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public virtual Question Question { get; set; }

        public string ImageUrl { get; set; }

        public string ImageInfoJson { get; set; }

        [NotMapped]
        public ImageInfo ImageInfo
        {
            get
            {
                return string.IsNullOrEmpty(ImageInfoJson) ? null : JsonConvert.DeserializeObject<ImageInfo>(ImageInfoJson);
            }
            set
            {
                ImageInfoJson = JsonConvert.SerializeObject(value);
            }
        }

    }

    public enum MiniGameType
    {
        Sort,
        Image,
        MultipleChoice
    }

    public enum MiniGameStatus
    {
        Win,
        Lost,
        Unknown
    }
}
