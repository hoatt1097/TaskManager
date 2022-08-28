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
        [StringLength(20)]
        public string UserCode { get; set; }
        [Key]
        [Column("Order_date", TypeName = "date")]
        public DateTime OrderDate { get; set; }
        [Key]
        [StringLength(6)]
        public string ItemCode { get; set; }
        [Key]
        [Column("Repast_ID")]
        public int RepastId { get; set; }
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(6)]
        public string MonthYear { get; set; }
        public int? Week { get; set; }
        public int? DoW { get; set; }
        [Column("Submit_dt", TypeName = "date")]
        public DateTime SubmitDt { get; set; }
        [Required]
        [Column("Submit_TM")]
        [StringLength(5)]
        public string SubmitTm { get; set; }
        [Column("CK_Code")]
        [StringLength(20)]
        public string CkCode { get; set; }
        public int Qty { get; set; }
        [StringLength(20)]
        public string Class { get; set; }
        [Column("Is_Bundled")]
        public bool IsBundled { get; set; }
        [Column("Is_Choosen")]
        public bool IsChoosen { get; set; }
        public bool Status { get; set; }
        [Column("Canteen_ID")]
        [StringLength(5)]
        public string CanteenId { get; set; }
        [Column("Cust_ID")]
        [StringLength(5)]
        public string CustId { get; set; }
        [Column("Plc_ID")]
        [StringLength(5)]
        public string PlcId { get; set; }
        [Column("POST")]
        [StringLength(20)]
        public string Post { get; set; }
        public bool? IsModified { get; set; }
        [Column("Modi_Date", TypeName = "date")]
        public DateTime? ModiDate { get; set; }
        [Column("Modi_Time")]
        [StringLength(5)]
        public string ModiTime { get; set; }
        [Column("Modi_User")]
        [StringLength(20)]
        public string ModiUser { get; set; }
    }
}
