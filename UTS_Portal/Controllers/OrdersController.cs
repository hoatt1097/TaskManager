using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTS_Portal.Helpers;
using UTS_Portal.Models;
using UTS_Portal.ViewModels;

namespace UTS_Portal.Controllers
{
    public class OrdersController : Controller
    {
        private readonly db_utsContext _context;

        public INotyfService _notyfService { get; }

        public OrdersController(db_utsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public IActionResult Index(string? month)
        {
            //Format month from select: MM/yyyy
            // Get selectbox select month
            string SelectBoxElement = BuildTxtMonth();
            ViewBag.SelectBoxElement = SelectBoxElement;
            
            if (month != null)
            {
                // Call data menu in month
                DateTime date = DateTime.Parse(month + "/1");
                List<CalendarMonth> CalendarMonth = DateHelper.GetCalendar(_context, date);
                ViewBag.CalendarMonth = CalendarMonth;

                // Collect data menu item by month
                List<MenusByMonth> MenusByMonth = MenuHelper.GetMenusByMonth(_context, month);
                ViewBag.MenusByMonth = MenusByMonth;

                // Check menu item data has data
                ViewBag.HasMenuData = MenusByMonth.Count > 0 ? true : false;
            }

            ViewBag.CurrentMonth = month;
            return View();
        }

        [HttpPost]
        public IActionResult OrderSubmit(List<OrderSubmit> orderSubmit)
        {
            var a = 0;
            return Json(new { success = true, message = $"Submit order successfully!" });
        }
        public string BuildTxtMonth()
        {
            var MonthActive = _context.MenuInfos.OrderBy(x => x.Month).Where(x => x.Status == true)
                .Select(x=> x.Month.ToString("MM/yyyy")).ToList();

            string selectElm = "<select class='custom-select' id='txtMonthSelect' name='txtMonthSelect' style='min-width: 200px;'>";

            foreach(var m in MonthActive)
            {
                selectElm += "<option value='" + m + "'>" + m + "</option>";
            }
            selectElm += "</select >";

            return selectElm;
        }
    }
}
