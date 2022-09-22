using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTS_Portal.ViewModels
{
    public class ParentView
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ClassName { get; set; }
        public string CardId { get; set; }
        public string Password { get; set; }
        public string Bal_Amount { get; set; }
    }
}
