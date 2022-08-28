using System;
using System.ComponentModel.DataAnnotations;

namespace UTS_Portal.ViewModels
{
    public class LoginViewModel
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage = ("Please enter Email"))]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [MinLength(5, ErrorMessage = "You must set the Minimum 5 character password")]
        public string Password { get; set; }

        public string AccountType { get; set; }
    }
}
