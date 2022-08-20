using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTS_Portal.ViewModels;

namespace UTS_Portal.Helpers
{
    public class DateHelper
    {
		public static List<CalendarMonth> GetCalendar (DateTime datetime)
        {
			List<CalendarMonth> calendarMonths = new List<CalendarMonth>();
			int totalDateMonth = DateTime.DaysInMonth(datetime.Year, datetime.Month);
			int StartWeek = 1;
			for (int i = 1; i <= totalDateMonth; i++)
			{
				DateTime date = new DateTime(datetime.Year, datetime.Month, i);
				var DoW = date.DayOfWeek;
				if (DoW.ToString() == "Saturday" && i != 1)
				{
					StartWeek++;
				}

				// Check day is holiday
				bool IsHoliday = DoW.ToString() == "Saturday" || DoW.ToString() == "Sunday";
				// Check day is holiday

				CalendarMonth m = new CalendarMonth();
				m.DateMonth = date.ToString("MM/dd");
				m.DateString = DoW.ToString();
				m.Day = date.Day;
				m.Week = StartWeek;
				m.IsHoliday = IsHoliday;

				calendarMonths.Add(m);
			}

			return calendarMonths;
		}
    }
}
