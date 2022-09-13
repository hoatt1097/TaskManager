using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTS_Portal.ViewModels
{
    public class ChangePassword
    {
        public String Id { get; set; } 
        public String CurrentPassword { get; set; }
        public String NewPassword { get; set; }
        public String ReNewPassword { get; set; }
    }
}
