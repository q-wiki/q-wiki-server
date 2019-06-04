﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class Question
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; }

        [Required]
        public string SparqlQuery { get; set; }

        [Required]
        public string TaskDescription { get; set; }

        [ForeignKey(nameof(Category))]
        [StringLength(36)]
        [Required]
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Required]
        public MiniGameType MiniGameType { get; set; }
    }
}