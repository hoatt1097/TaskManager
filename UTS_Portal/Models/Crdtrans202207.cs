using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    [Table("CRDTRANS_202207")]
    public partial class Crdtrans202207
    {
        [Key]
        [Column("TRAN_DATE", TypeName = "datetime")]
        public DateTime TranDate { get; set; }
        [Key]
        [Column("TRANS_NUM")]
        [StringLength(18)]
        public string TransNum { get; set; }
        [Required]
        [Column("TRAN_TIME")]
        [StringLength(8)]
        public string TranTime { get; set; }
        [Key]
        [Column("TRANS_CODE")]
        [StringLength(2)]
        public string TransCode { get; set; }
        [Key]
        [Column("CARD_ID")]
        [StringLength(10)]
        public string CardId { get; set; }
        [Required]
        [Column("CARD_ID2")]
        [StringLength(20)]
        public string CardId2 { get; set; }
        [Required]
        [Column("CRD_TYPE")]
        [StringLength(2)]
        public string CrdType { get; set; }
        [Required]
        [Column("CUST_ID")]
        [StringLength(5)]
        public string CustId { get; set; }
        [Required]
        [Column("PLC_ID")]
        [StringLength(5)]
        public string PlcId { get; set; }
        [Required]
        [Column("REPAST")]
        [StringLength(2)]
        public string Repast { get; set; }
        [Key]
        [Column("NODE_ID")]
        [StringLength(3)]
        public string NodeId { get; set; }
        [Key]
        [Column("GOODS_ID")]
        [StringLength(6)]
        public string GoodsId { get; set; }
        [Column("QTY", TypeName = "numeric(8, 0)")]
        public decimal Qty { get; set; }
        [Column("AMOUNT", TypeName = "decimal(12, 2)")]
        public decimal Amount { get; set; }
        [Column("FOREX_RATE", TypeName = "numeric(12, 4)")]
        public decimal ForexRate { get; set; }
        [Column("FOREX_AMT", TypeName = "numeric(12, 4)")]
        public decimal ForexAmt { get; set; }
        [Required]
        [Column("FOREX_CYS")]
        [StringLength(3)]
        public string ForexCys { get; set; }
        [Required]
        [Column("RS_CODE")]
        [StringLength(2)]
        public string RsCode { get; set; }
        [Column("USER_ID")]
        public int UserId { get; set; }
        [Column("WS_ID")]
        public int WsId { get; set; }
        [Required]
        [Column("POST")]
        [StringLength(1)]
        public string Post { get; set; }
        [Required]
        [Column("REMARK")]
        [StringLength(120)]
        public string Remark { get; set; }
        [Column("REPAST_ID")]
        public int RepastId { get; set; }
        [Column("ISTOPUP")]
        public bool Istopup { get; set; }
        [Column("ISKIOSK")]
        public bool Iskiosk { get; set; }
        [Column("STATUS")]
        public bool Status { get; set; }
    }
}
