using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    public partial class Posts
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Alias { get; set; }
        [StringLength(255)]
        public string Scontents { get; set; }
        public string Contents { get; set; }
        [StringLength(255)]
        public string Thumb { get; set; }
        public bool Published { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(255)]
        public string Author { get; set; }
        public int? Views { get; set; }
        public bool? IsHot { get; set; }
    }
}
