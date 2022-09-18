using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    public partial class MenuInfos
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime Month { get; set; }
        [StringLength(2000)]
        public string Images { get; set; }
        public bool Status { get; set; }
        [Required]
        [Column("CampusID")]
        [StringLength(2)]
        public string CampusId { get; set; }
        [Column("StartDT", TypeName = "date")]
        public DateTime StartDt { get; set; }
        [Column("EndDT", TypeName = "date")]
        public DateTime EndDt { get; set; }
    }
}
