using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    [Table("SYSVAR")]
    public partial class Sysvar
    {
        [Key]
        [Column("VARNAME")]
        [StringLength(15)]
        public string Varname { get; set; }
        [Required]
        [Column("TYPE")]
        [StringLength(1)]
        public string Type { get; set; }
        [Required]
        [Column("VALUE")]
        [StringLength(60)]
        public string Value { get; set; }
        [Required]
        [Column("DESCRIPT")]
        [StringLength(120)]
        public string Descript { get; set; }
        [Required]
        [Column("INPUTMASK")]
        [StringLength(12)]
        public string Inputmask { get; set; }
        [Required]
        [Column("MODIFY")]
        public bool? Modify { get; set; }
        [Required]
        [Column("INVALID")]
        [StringLength(12)]
        public string Invalid { get; set; }
        [Required]
        [Column("DEP_CODE")]
        [StringLength(1)]
        public string DepCode { get; set; }
    }
}
