using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTS_Portal.Models;
using UTS_Portal.ViewModels;

namespace UTS_Portal.Helpers
{
    public class DateHelper
    {
		public static List<CalendarMonth> GetCalendar (db_utsContext _context, DateTime datetime)
        {
			List<Holidays> holidays = GetHolidaysByMonth(_context, datetime.ToString("MM/yyyy"));

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
				bool IsWeekend = DoW.ToString() == "Saturday" || DoW.ToString() == "Sunday";
				bool IsHoliday = holidays.Any(x => x.Day.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy"));
				bool IsOffDay = _context.Menus.ToList().Where(x => x.Status == 1 && x.MenuDate.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy")).FirstOrDefault() == null ? true : false;
				// Check day is holiday

				CalendarMonth m = new CalendarMonth();
				m.DateMonth = date.ToString("dd/MM");
				m.DateString = DoW.ToString();
				m.Day = date.Day;
				m.Week = StartWeek;
				m.IsHoliday = IsWeekend || IsHoliday || IsOffDay;

				calendarMonths.Add(m);
			}

			return calendarMonths;
		}

		public static List<Holidays> GetHolidaysByMonth(db_utsContext _context, string month)
		{
			var DB_Holidays = _context.Holidays.ToList().Where(x => x.Day.ToString("MM/yyyy") == month).ToList();
			return DB_Holidays;
		}
	}
}
