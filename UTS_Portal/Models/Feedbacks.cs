using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    public partial class Feedbacks
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(11)]
        public string Phone { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(2000)]
        public string Message { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SubmittedDate { get; set; }
    }
}
