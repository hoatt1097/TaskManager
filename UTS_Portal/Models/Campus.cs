using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    public partial class Campus
    {
        [Column("IDX")]
        public int Idx { get; set; }
        [Key]
        [Column("Campus_ID")]
        [StringLength(2)]
        public string CampusId { get; set; }
        [Required]
        [StringLength(60)]
        public string Description { get; set; }
        [Required]
        [StringLength(120)]
        public string Address { get; set; }
    }
}
