using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaskManager.Models
{
    public partial class Projects
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Status { get; set; }

        [StringLength(5000)]
        public string Description { get; set; }

        [StringLength(2500)]
        public string NextAction { get; set; }

        [StringLength(255)]
        public string PIC { get; set; }

        public int ChannelId { get; set; }

        public int Progress { get; set; }

        public bool Active { get; set; }

        public DateTime? CreationDate { get; set; } = default(DateTime?);

        public int? CreatedBy { get; set; }

        public DateTime? LastUpdateDate { get; set; } = default(DateTime?);

        public int? LastUpdatedBy { get; set; }

    }
}
