using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaskManager.Models
{
    public partial class Channels
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public int? LastUpdatedBy { get; set; }

    }
}
