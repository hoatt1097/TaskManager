using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTS_Portal.Models;

namespace UTS_Portal.ViewModels
{
    public class MenusByMonth
    {
        public string DateMonth { get; set; }
        public List<NoonByDay> Noon { get; set; }
    }

    public class NoonByDay
    {
        public int Noon { get; set; }
        public List<Menus> Menus { get; set; }
    }
}
