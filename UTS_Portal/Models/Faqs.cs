﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    [Table("FAQs")]
    public partial class Faqs
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public int OrderNo { get; set; }
        [Required]
        [StringLength(2000)]
        public string Title { get; set; }
        [Required]
        public string Contents { get; set; }
    }
}