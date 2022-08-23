using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTS_Portal.ViewModels
{
    public class CalendarMonth
    {
        public String DateMonth { get; set; } // 01/08, 02/08
        public String DateString { get; set; } // Monday
        public int Day { get; set; } // 1, 2, 3, 4
        public int Week { get; set; } // 1, 2, 3, 4
        public bool IsHoliday { get; set; } // true, false
    }
}
