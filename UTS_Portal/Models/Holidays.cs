using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    public partial class Holidays
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime Day { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
    }
}
