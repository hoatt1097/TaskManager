using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTS_Portal.ViewModels
{
    public class HolidaysByMonth
    {
        public string Month { get; set; }
        public List<Holiday> Holidays { get; set; }
    }

    public class Holiday
    {
        public string Day { get; set; }
        public string Description { get; set; }
    }
}
