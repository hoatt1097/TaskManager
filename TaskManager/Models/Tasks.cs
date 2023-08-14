using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaskManager.Models
{
    public partial class Tasks
    {
        [Key]
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [StringLength(255)]
        public string ProjectName { get; set; }

        [Required]
        [StringLength(500)]
        public string Subject { get; set; }

        [StringLength(5000)]
        public string Description { get; set; }

        [StringLength(5000)]
        public string Action { get; set; }

        [StringLength(255)]
        public string HelpdeskTicket { get; set; }

        [StringLength(255)]
        public string Status { get; set; }

        [StringLength(255)]
        public string Progress { get; set; }

        [StringLength(255)]
        public string PIC { get; set; }

        [StringLength(255)]
        public string Requestor { get; set; }

        [StringLength(255)]
        public string Assignee { get; set; }

        public string StartDate { get; set; }

        public string DueDate { get; set; }

        public string EstimateDate { get; set; }

        public bool Active { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public int? LastUpdatedBy { get; set; }

    }
}
