using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    [Table("GOODS")]
    public partial class Goods
    {
        [Key]
        [Column("GOODS_ID")]
        [StringLength(6)]
        public string GoodsId { get; set; }
        [Required]
        [Column("BARCODE")]
        [StringLength(13)]
        public string Barcode { get; set; }
        [Required]
        [Column("UPC_CODE")]
        [StringLength(13)]
        public string UpcCode { get; set; }
        [Required]
        [Column("GRP_ID")]
        [StringLength(4)]
        public string GrpId { get; set; }
        [Required]
        [Column("CLS_CODE")]
        [StringLength(2)]
        public string ClsCode { get; set; }
        [Required]
        [Column("SHORT_NAME")]
        [StringLength(120)]
        public string ShortName { get; set; }
        [Required]
        [Column("FULL_NAME")]
        [StringLength(120)]
        public string FullName { get; set; }
        [Required]
        [Column("EN_Name")]
        [StringLength(120)]
        public string EnName { get; set; }
        [Required]
        [Column("Other_Name")]
        [StringLength(120)]
        public string OtherName { get; set; }
        [Required]
        [Column("DESCRIPT")]
        [StringLength(120)]
        public string Descript { get; set; }
        [Required]
        [StringLength(120)]
        public string Allergy1 { get; set; }
        [Required]
        [StringLength(120)]
        public string Allergy2 { get; set; }
        [Required]
        [StringLength(120)]
        public string Allergy3 { get; set; }
        [Required]
        [Column("REF")]
        [StringLength(20)]
        public string Ref { get; set; }
        [Required]
        [Column("Other_Code")]
        [StringLength(30)]
        public string OtherCode { get; set; }
        [Required]
        [Column("PICEUNIT")]
        [StringLength(3)]
        public string Piceunit { get; set; }
        [Required]
        [Column("PACKUNIT")]
        [StringLength(3)]
        public string Packunit { get; set; }
        [Column("UNITCONV", TypeName = "numeric(4, 0)")]
        public decimal Unitconv { get; set; }
        [Required]
        [Column("PACKUNIT2")]
        [StringLength(3)]
        public string Packunit2 { get; set; }
        [Column("UNITCONV2", TypeName = "numeric(4, 0)")]
        public decimal Unitconv2 { get; set; }
        [Column("EXPIRY")]
        public bool Expiry { get; set; }
        [Column("WARRANTY")]
        public bool Warranty { get; set; }
        [Required]
        [Column("SUPP_ID")]
        [StringLength(5)]
        public string SuppId { get; set; }
        [Column("MBC")]
        public bool Mbc { get; set; }
        [Column("SKU")]
        public bool Sku { get; set; }
        [Column("DMS", TypeName = "numeric(8, 2)")]
        public decimal Dms { get; set; }
        [Required]
        [Column("ITEM_TYPE")]
        [StringLength(2)]
        public string ItemType { get; set; }
        [Required]
        [Column("MERC_TYPE")]
        [StringLength(2)]
        public string MercType { get; set; }
        [Required]
        [Column("TAX_CODE")]
        [StringLength(2)]
        public string TaxCode { get; set; }
        [Required]
        [Column("MERC_ACID")]
        [StringLength(10)]
        public string MercAcid { get; set; }
        [Required]
        [Column("SYMBOL")]
        [StringLength(20)]
        public string Symbol { get; set; }
        [Column("RTPRICE", TypeName = "numeric(12, 2)")]
        public decimal Rtprice { get; set; }
        [Column("OPEN_DATE", TypeName = "datetime")]
        public DateTime? OpenDate { get; set; }
        [Required]
        [Column("IMAGE")]
        [StringLength(60)]
        public string Image { get; set; }
        [Column("DOMESTIC")]
        public bool Domestic { get; set; }
        [Column("Is_Bundled")]
        public bool IsBundled { get; set; }
        [Column("Is_Ordered")]
        public bool IsOrdered { get; set; }
        public bool IsConsign { get; set; }
        [Column("IsPrintBC")]
        public bool IsPrintBc { get; set; }
        public bool IsPosShow { get; set; }
        [Required]
        [Column("POST")]
        [StringLength(1)]
        public string Post { get; set; }
        [Required]
        [Column("STATUS")]
        [StringLength(2)]
        public string Status { get; set; }
    }
}
