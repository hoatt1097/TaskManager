using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    [Table("CSCARD")]
    public partial class Cscard
    {
        [Column("IDX")]
        public int Idx { get; set; }
        [Key]
        [Column("CARD_ID")]
        [StringLength(10)]
        public string CardId { get; set; }
        [Required]
        [Column("CARD_ID2")]
        [StringLength(30)]
        public string CardId2 { get; set; }
        [Required]
        [Column("CUST_ID")]
        [StringLength(5)]
        public string CustId { get; set; }
        [Required]
        [Column("PLC_ID")]
        [StringLength(5)]
        public string PlcId { get; set; }
        [Required]
        [Column("CANTEEN_ID")]
        [StringLength(5)]
        public string CanteenId { get; set; }
        [Required]
        [Column("NODE_ID")]
        [StringLength(3)]
        public string NodeId { get; set; }
        [Required]
        [Column("PARENT_ID")]
        [StringLength(20)]
        public string ParentId { get; set; }
        [Required]
        [Column("BARCODE")]
        [StringLength(30)]
        public string Barcode { get; set; }
        [Column("OLD_AMOUNT", TypeName = "numeric(12, 2)")]
        public decimal OldAmount { get; set; }
        [Column("BAL_AMOUNT", TypeName = "numeric(12, 2)")]
        public decimal BalAmount { get; set; }
        [Column("Is_OverDrf")]
        public bool IsOverDrf { get; set; }
        [Column("Draft_Amt", TypeName = "numeric(12, 0)")]
        public decimal DraftAmt { get; set; }
        [Column("Draft_PC", TypeName = "numeric(5, 2)")]
        public decimal DraftPc { get; set; }
        [Column("DEBT_AMT", TypeName = "numeric(12, 2)")]
        public decimal DebtAmt { get; set; }
        [Required]
        [Column("CRD_TYPE")]
        [StringLength(2)]
        public string CrdType { get; set; }
        [Required]
        [Column("USER_TYPE")]
        [StringLength(2)]
        public string UserType { get; set; }
        [Required]
        [Column("Campus_ID")]
        [StringLength(2)]
        public string CampusId { get; set; }
        [Required]
        [Column("PERSON_ID")]
        [StringLength(20)]
        public string PersonId { get; set; }
        [Required]
        [Column("NAME")]
        [StringLength(120)]
        public string Name { get; set; }
        [Required]
        [Column("FNAME")]
        [StringLength(120)]
        public string Fname { get; set; }
        [Required]
        [Column("LNAME")]
        [StringLength(120)]
        public string Lname { get; set; }
        [Column("BIRTHDAY", TypeName = "datetime")]
        public DateTime? Birthday { get; set; }
        [Required]
        [Column("SEX")]
        [StringLength(2)]
        public string Sex { get; set; }
        [Required]
        [Column("ADDRESS")]
        [StringLength(120)]
        public string Address { get; set; }
        [Required]
        [Column("DISTRICT")]
        [StringLength(120)]
        public string District { get; set; }
        [Required]
        [Column("CITY")]
        [StringLength(120)]
        public string City { get; set; }
        [Required]
        [Column("COUNTRY")]
        [StringLength(120)]
        public string Country { get; set; }
        [Required]
        [Column("EMAIL")]
        [StringLength(60)]
        public string Email { get; set; }
        [Required]
        [Column("PHONE")]
        [StringLength(20)]
        public string Phone { get; set; }
        [Required]
        [Column("MOBI")]
        [StringLength(20)]
        public string Mobi { get; set; }
        [Required]
        [Column("COURSE")]
        [StringLength(20)]
        public string Course { get; set; }
        [Required]
        [Column("CLASS")]
        [StringLength(2)]
        public string Class { get; set; }
        [Required]
        [Column("CLASS_NAME")]
        [StringLength(25)]
        public string ClassName { get; set; }
        [Required]
        [Column("PR_PHONE")]
        [StringLength(20)]
        public string PrPhone { get; set; }
        [Column("ISS_DATE", TypeName = "datetime")]
        public DateTime? IssDate { get; set; }
        [Column("DUE_DATE", TypeName = "datetime")]
        public DateTime? DueDate { get; set; }
        [Column("LAST_DATE", TypeName = "datetime")]
        public DateTime? LastDate { get; set; }
        [Column("Entr_Date", TypeName = "datetime")]
        public DateTime? EntrDate { get; set; }
        [Column("Gradu_Date", TypeName = "datetime")]
        public DateTime? GraduDate { get; set; }
        [Required]
        [Column("IMAGE")]
        [StringLength(30)]
        public string Image { get; set; }
        [Required]
        [Column("POST")]
        [StringLength(1)]
        public string Post { get; set; }
        [Required]
        [Column("REMARK")]
        [StringLength(120)]
        public string Remark { get; set; }
        [Required]
        [Column("SIBLING")]
        [StringLength(120)]
        public string Sibling { get; set; }
        [Required]
        [StringLength(60)]
        public string Password { get; set; }
        [Column("Is_Ordered")]
        public bool IsOrdered { get; set; }
        public bool IsTopUp { get; set; }
        public bool IsWebUse { get; set; }
        [Column("STATUS")]
        public bool Status { get; set; }
        [Column("LOCAL_LOCK")]
        public bool LocalLock { get; set; }
    }
}
