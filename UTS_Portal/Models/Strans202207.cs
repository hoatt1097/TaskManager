using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    [Table("STRANS_202207")]
    public partial class Strans202207
    {
        [Column("TRAN_DATE", TypeName = "datetime")]
        public DateTime TranDate { get; set; }
        [Required]
        [Column("TRAN_TIME")]
        [StringLength(8)]
        public string TranTime { get; set; }
        [Column("DUE_DATE", TypeName = "datetime")]
        public DateTime? DueDate { get; set; }
        [Column("EF_DATE", TypeName = "datetime")]
        public DateTime? EfDate { get; set; }
        [Key]
        [Column("TRANS_NUM")]
        [StringLength(18)]
        public string TransNum { get; set; }
        [Required]
        [Column("TRANS_CODE")]
        [StringLength(2)]
        public string TransCode { get; set; }
        [Required]
        [Column("NODE_ID")]
        [StringLength(3)]
        public string NodeId { get; set; }
        [Required]
        [Column("REF_NO")]
        [StringLength(25)]
        public string RefNo { get; set; }
        [Column("REF_DATE", TypeName = "datetime")]
        public DateTime? RefDate { get; set; }
        [Required]
        [Column("INVOICE_NO")]
        [StringLength(12)]
        public string InvoiceNo { get; set; }
        [Column("INVOICE_DT", TypeName = "datetime")]
        public DateTime? InvoiceDt { get; set; }
        [Column("INV_VATAMT", TypeName = "numeric(12, 2)")]
        public decimal InvVatamt { get; set; }
        [Required]
        [Column("INV_DEPT")]
        [StringLength(1)]
        public string InvDept { get; set; }
        [Required]
        [Column("POST")]
        [StringLength(1)]
        public string Post { get; set; }
        [Required]
        [Column("RS_CODE")]
        [StringLength(2)]
        public string RsCode { get; set; }
        [Column("ISIN")]
        public bool Isin { get; set; }
        [Required]
        [Column("ID")]
        [StringLength(5)]
        public string Id { get; set; }
        [Required]
        [Column("CORR_ID")]
        [StringLength(5)]
        public string CorrId { get; set; }
        [Required]
        [Column("KIT_ID")]
        [StringLength(5)]
        public string KitId { get; set; }
        [Required]
        [Column("KIT_TYPE")]
        [StringLength(2)]
        public string KitType { get; set; }
        [Column("KIT_QTY", TypeName = "numeric(8, 0)")]
        public decimal KitQty { get; set; }
        [Key]
        [Column("IDX")]
        public int Idx { get; set; }
        [Key]
        [Column("GOODS_ID")]
        [StringLength(6)]
        public string GoodsId { get; set; }
        [Required]
        [Column("CORR_GSID")]
        [StringLength(6)]
        public string CorrGsid { get; set; }
        [Column("QTY", TypeName = "numeric(12, 3)")]
        public decimal Qty { get; set; }
        [Required]
        [Column("UNIT_SYMB")]
        [StringLength(3)]
        public string UnitSymb { get; set; }
        [Required]
        [Column("BASE_UNIT")]
        [StringLength(3)]
        public string BaseUnit { get; set; }
        [Column("UNITCONV", TypeName = "numeric(4, 0)")]
        public decimal Unitconv { get; set; }
        [Column("AMOUNT", TypeName = "numeric(15, 2)")]
        public decimal Amount { get; set; }
        [Column("SURPLUS", TypeName = "numeric(12, 2)")]
        public decimal Surplus { get; set; }
        [Column("VAT_AMT", TypeName = "numeric(12, 2)")]
        public decimal VatAmt { get; set; }
        [Column("COMMIS_AMT", TypeName = "numeric(12, 2)")]
        public decimal CommisAmt { get; set; }
        [Column("VAT_INCL")]
        public bool VatIncl { get; set; }
        [Column("DISCOUNT", TypeName = "decimal(12, 2)")]
        public decimal Discount { get; set; }
        [Column("DISC_RATE", TypeName = "numeric(5, 2)")]
        public decimal DiscRate { get; set; }
        [Required]
        [Column("TAX_CODE")]
        [StringLength(2)]
        public string TaxCode { get; set; }
        [Column("FOREX_AMT", TypeName = "numeric(12, 4)")]
        public decimal ForexAmt { get; set; }
        [Column("SALE_AMT", TypeName = "numeric(12, 4)")]
        public decimal SaleAmt { get; set; }
        [Column("FOREX_RATE", TypeName = "numeric(12, 4)")]
        public decimal ForexRate { get; set; }
        [Required]
        [Column("FOREX_CYS")]
        [StringLength(3)]
        public string ForexCys { get; set; }
        [Column("EXPIRY_DT", TypeName = "datetime")]
        public DateTime? ExpiryDt { get; set; }
        [Column("USER_ID", TypeName = "numeric(3, 0)")]
        public decimal UserId { get; set; }
        [Column("WS_ID", TypeName = "numeric(3, 0)")]
        public decimal WsId { get; set; }
        [Required]
        [Column("CUST_ID")]
        [StringLength(5)]
        public string CustId { get; set; }
        [Required]
        [Column("CS_ID")]
        [StringLength(20)]
        public string CsId { get; set; }
        [Required]
        [Column("PLC_ID")]
        [StringLength(5)]
        public string PlcId { get; set; }
        [Column("REPAST_ID")]
        public int RepastId { get; set; }
        [Required]
        [Column("REPAST")]
        [StringLength(2)]
        public string Repast { get; set; }
        [Required]
        [Column("CRD_TYPE")]
        [StringLength(2)]
        public string CrdType { get; set; }
        [Required]
        [Column("STAFF_ID")]
        [StringLength(5)]
        public string StaffId { get; set; }
        [Required]
        [Column("CUSTAX_ID")]
        [StringLength(20)]
        public string CustaxId { get; set; }
        [Required]
        [Column("BUY_ID")]
        [StringLength(12)]
        public string BuyId { get; set; }
        [Required]
        [Column("CARD_ID")]
        [StringLength(16)]
        public string CardId { get; set; }
        [Required]
        [Column("REF")]
        [StringLength(25)]
        public string Ref { get; set; }
        [Required]
        [Column("REMARK")]
        [StringLength(120)]
        public string Remark { get; set; }
        [Column("UPDATED")]
        public bool Updated { get; set; }
        [Required]
        [Column("MERC_TYPE")]
        [StringLength(2)]
        public string MercType { get; set; }
        [Column("MBC")]
        public bool Mbc { get; set; }
        [Column("SKU")]
        public bool Sku { get; set; }
        [Column("COPIES", TypeName = "numeric(2, 0)")]
        public decimal Copies { get; set; }
        [Column("SHIFT", TypeName = "numeric(2, 0)")]
        public decimal Shift { get; set; }
        [Column("ISKIOSK")]
        public bool Iskiosk { get; set; }
        [Column("IS_Bundled")]
        public bool IsBundled { get; set; }
        [Required]
        [Column("STATUS")]
        [StringLength(1)]
        public string Status { get; set; }
        [Required]
        [Column("POS_TYPE")]
        [StringLength(1)]
        public string PosType { get; set; }
    }
}
