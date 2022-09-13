using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    [Table("User_Type")]
    public partial class UserType
    {
        [Required]
        [StringLength(1)]
        public string Prefix { get; set; }
        [Required]
        [Column("Crd_Type")]
        [StringLength(2)]
        public string CrdType { get; set; }
        [Required]
        [StringLength(120)]
        public string Descript { get; set; }
        public int Status { get; set; }
    }
}
