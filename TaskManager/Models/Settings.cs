using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaskManager.Models
{
    public class Settings
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Key { get; set; }

        [StringLength(1500)]
        public string? Value { get; set; }
    }
}
