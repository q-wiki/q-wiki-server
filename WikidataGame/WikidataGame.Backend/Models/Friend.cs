using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class Friend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RelationId { get; set; }

        [Required]
        public Guid UserId { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [Required]
        public Guid FriendId { get; set; }

        [ForeignKey(nameof(FriendId))]
        public virtual User FriendUser { get; set; }
    }
}
