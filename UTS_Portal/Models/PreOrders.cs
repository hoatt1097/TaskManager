using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    public partial class PreOrders
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string UserCode { get; set; }
        [Required]
        [StringLength(6)]
        public string MonthYear { get; set; }
        public int? Week { get; set; }
        public int? Dow { get; set; }
        [Column(TypeName = "date")]
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime SubmittedDate { get; set; }
        [Required]
        [StringLength(5)]
        public string SubmittedTime { get; set; }
        [Required]
        [StringLength(6)]
        public string ItemCode { get; set; }
        [Column("CKCode")]
        [StringLength(20)]
        public string Ckcode { get; set; }
        public int Qty { get; set; }
        public int Repast { get; set; }
        [StringLength(20)]
        public string Class { get; set; }
        public int Bundled { get; set; }
        public int IsOrdered { get; set; }
        public int Status { get; set; }
        [StringLength(20)]
        public string Post { get; set; }
        public int? IsModified { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedDate { get; set; }
        [StringLength(5)]
        public string ModifiedTime { get; set; }
        [StringLength(20)]
        public string ModifiedUser { get; set; }
        [Column("CanteenID")]
        [StringLength(5)]
        public string CanteenId { get; set; }
        [Column("CustID")]
        [StringLength(5)]
        public string CustId { get; set; }
        [Column("PlcID")]
        [StringLength(5)]
        public string PlcId { get; set; }
    }
}
