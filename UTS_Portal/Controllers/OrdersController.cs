using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            try
            {
                var currentUser = UserHelper.GetCurrentUser(HttpContext);
                var monthYear = orderSubmit.First().CurrentMonth.Replace("/", "");

                foreach(var day in orderSubmit)
                {
                    string dayString = day.Day.ToString();
                    if(day.Day < 10)
                    {
                        dayString = "0" + day.Day;
                    }
                    var orderDate = DateTime.ParseExact(dayString + "/" + day.CurrentMonth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (day.Breakfast != null)
                    {
                        foreach (var item in day.Breakfast)
                        {
                            PreOrders order = new PreOrders
                            {
                                UserCode = currentUser.Code,
                                MonthYear = monthYear,
                                Week = null,
                                DoW = null,
                                OrderDate = orderDate,
                                SubmitDt = DateTime.Now,
                                SubmitTm = DateTime.Now.ToString("HH:mm"),
                                ItemCode = item.Code.Trim(),
                                CkCode = item.Ckcode.Trim(),
                                Qty = item.Qty,
                                RepastId = 1,
                                Class = "",
                                IsBundled = item.Bundled == 1 ? true : false,
                                IsChoosen = true,
                                Status = true,
                            };

                            _context.PreOrders.Add(order);
                        }
                    }

                    if (day.Lunch != null)
                    {
                        foreach (var item in day.Lunch)
                        {
                            PreOrders order = new PreOrders
                            {
                                UserCode = currentUser.Code,
                                MonthYear = monthYear,
                                Week = null,
                                DoW = null,
                                OrderDate = orderDate,
                                SubmitDt = DateTime.Now,
                                SubmitTm = DateTime.Now.ToString("HH:mm"),
                                ItemCode = item.Code.Trim(),
                                CkCode = item.Ckcode.Trim(),
                                Qty = item.Qty,
                                RepastId = 2,
                                Class = "",
                                IsBundled = item.Bundled == 1 ? true : false,
                                IsChoosen = true,
                                Status = true,
                            };

                            _context.PreOrders.Add(order);
                        }
                    }

                    if(day.Afternoon != null)
                    {
                        foreach (var item in day.Afternoon)
                        {
                            PreOrders order = new PreOrders
                            {
                                UserCode = currentUser.Code,
                                MonthYear = monthYear,
                                Week = null,
                                DoW = null,
                                OrderDate = orderDate,
                                SubmitDt = DateTime.Now,
                                SubmitTm = DateTime.Now.ToString("HH:mm"),
                                ItemCode = item.Code.Trim(),
                                CkCode = item.Ckcode.Trim(),
                                Qty = item.Qty,
                                RepastId = 3,
                                Class = "",
                                IsBundled = item.Bundled == 1 ? true : false,
                                IsChoosen = true,
                                Status = true,
                            };

                            _context.PreOrders.Add(order);
                        }
                    }
                }

                _context.SaveChanges();
                return Json(new { success = true, message = $"Submit order successfully!" });
            }
            catch (DbUpdateException)
            {
                return Json(new { success = false, message = $"Submit order unsuccessfully! Data is exist." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = $"Something is error" });
            }
        }
        public string BuildTxtMonth()
        {
            var MonthActive = _context.MenuInfos.OrderBy(x => x.Month).Where(x => x.Status == true)
                .Select(x=> x.Month.ToString("MM/yyyy")).ToList();

            string selectElm = "<select class='custom-select' id='txtMonthSelect' name='txtMonthSelect'>";

            foreach(var m in MonthActive)
            {
                selectElm += "<option value='" + m + "'>" + m + "</option>";
            }
            selectElm += "</select >";

            return selectElm;
        }
    }
}
