using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class GameRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid SenderId { get; set; }


        [ForeignKey(nameof(SenderId))]
        public virtual User Sender { get; set; }

        [Required]
        public Guid RecipientId { get; set; }

        [ForeignKey(nameof(RecipientId))]
        public virtual User Recipient { get; set; }
    }
}
