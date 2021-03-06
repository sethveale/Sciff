﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sciff.Tests.LibraryDomain
{
    public class Page
    {
        [Key]
        [Column(Order = 0)]
        [Range(typeof(long), Constants.MinIsbnString, Constants.MaxIsbnString)]
        [ForeignKey("Book")]
        public long Isbn { get; set; }

        public virtual Book Book { get; set; }

        [Key]
        [Column(Order = 1)]
        [Range(1, int.MaxValue)]
        public int Number { get; set; }

        [Required]
        public string Text { get; set; }

        public virtual ICollection<Topic> Subjects { get; set; }
    }
}