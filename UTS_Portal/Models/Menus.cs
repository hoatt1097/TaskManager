using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    public partial class Menus
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(6)]
        public string MonthYear { get; set; }
        public int? Week { get; set; }
        public int? DoW { get; set; }
        [Column(TypeName = "date")]
        public DateTime MenuDate { get; set; }
        [Required]
        [StringLength(30)]
        public string Category { get; set; }
        [Required]
        [StringLength(6)]
        public string ItemCode { get; set; }
        [Required]
        [Column("CKCode")]
        [StringLength(20)]
        public string Ckcode { get; set; }
        [Required]
        [StringLength(255)]
        public string OriginalName { get; set; }
        [StringLength(255)]
        public string ItemNameVn { get; set; }
        [StringLength(255)]
        public string ItemNameEn { get; set; }
        public int? Qty { get; set; }
        [Column("RepastID")]
        public int RepastId { get; set; }
        [StringLength(20)]
        public string Class { get; set; }
        public int IsBundled { get; set; }
        public int IsOrdered { get; set; }
        public int Status { get; set; }
    }
}
