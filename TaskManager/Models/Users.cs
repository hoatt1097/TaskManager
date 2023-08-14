using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaskManager.Models
{
    public partial class Users
    {
        [Key]
        public int Id { get; set; }

        public int RoleId { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(255)]
        public string DisplayName { get; set; }

        [StringLength(255)]
        public string Fullname { get; set; }
 
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        public bool Active { get; set; }

        public int ChannelId { get; set; }

        public string ChannelName { get; set; }

        public DateTime? LastLogin { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public int? LastUpdatedBy { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(Roles.Users))]
        public virtual Roles Role { get; set; }

    }
}
