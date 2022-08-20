using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTS_Portal.ViewModels
{
    public class CalendarMonth
    {
        public String DateMonth { get; set; }
        public String DateString { get; set; }
        public int Day { get; set; }
        public int Week { get; set; }
        public bool IsHoliday { get; set; }
    }
}
