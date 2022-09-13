using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UTS_Portal.Extension;
using UTS_Portal.Helpers;
using UTS_Portal.Models;
using UTS_Portal.ViewModels;

namespace UTS_Portal.Controllers
{
    public class OrdersController : Controller
    {
        private readonly db_utsContext _context;

        public INotyfService _notyfService { get; }

        public OrdersController(db_utsContext context, INotyfService notyfService, IConfiguration configuration)
        {
            _context = context;
            _notyfService = notyfService;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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

        public IActionResult History(string? parentId, string? month)
        {
            var currentUser = UserHelper.GetCurrentUser(HttpContext);
            if (currentUser.RoleName?.Trim() == "Parent")
            {
                if(currentUser.Code?.Trim() != parentId?.Trim())
                {
                    _notyfService.Error("User has not permission!");
                    return RedirectToAction("Index", "Home");
                }

                // Defaul show data
                ViewBag.ParentId = currentUser.Code?.Trim();
                ViewBag.StudentName = currentUser.Username?.Trim();
            }

            //Format month from select: MM/yyyy
            var ListParent = _context.Cscard.AsNoTracking().ToList();
            var SelectBoxOrderMonth = BuildSelectBoxOrderMonth(month);
            ViewBag.SelectBoxOrderMonth = SelectBoxOrderMonth;

            if (parentId?.Trim() != null && month?.Trim() != null)
            {
                List<OrderSubmit> listOrderSubmit = new List<OrderSubmit>();

                var orderHistory = _context.PreOrders
                    .Where(x => x.UserCode.Trim() == parentId.Trim() && x.MonthYear == month.Replace("/", ""))
                    .ToList().OrderBy(x => x.OrderDate).ToList();

                var listOrderDate = orderHistory.Select(x => x.OrderDate).Distinct();
                foreach(var day in listOrderDate)
                {
                    int Day = day.Day;
                    string CurrentMonth = month;
                    List<OrderItem> Breakfast = new List<OrderItem>();
                    List<OrderItem> Lunch = new List<OrderItem>();
                    List<OrderItem> Afternoon = new List<OrderItem>();

                    var listOrderByDate = orderHistory.Where(x => x.OrderDate == day).ToList();
                    foreach(var item in listOrderByDate)
                    {

                        Goods goods = _context.Goods.Where(x => x.GoodsId == item.ItemCode).FirstOrDefault();

                        OrderItem orderItem = new OrderItem
                        {
                            Code = item.ItemCode?.Trim(),
                            Ckcode = item.CkCode?.Trim(),
                            OriginName = goods != null ? goods.OtherName?.Trim() : "",
                            NameVn = goods != null ? goods.FullName?.Trim() : "",
                            NameEn = goods != null ? goods.EnName?.Trim() : "",
                            Qty = item.Qty,
                            Bundled = item.IsBundled == true ? 1 : 0,
                            RepastId = item.RepastId
                        };

                        if(orderItem.RepastId == 1)
                        {
                            Breakfast.Add(orderItem);
                        }

                        if (orderItem.RepastId == 2)
                        {
                            Lunch.Add(orderItem);
                        }

                        if (orderItem.RepastId >= 3)
                        {
                            Afternoon.Add(orderItem);
                        }
                    }

                    OrderSubmit orderDay = new OrderSubmit
                    {
                        Day = Day,
                        CurrentMonth = CurrentMonth,
                        Breakfast = Breakfast,
                        Lunch = Lunch,
                        Afternoon = Afternoon
                    };

                    listOrderSubmit.Add(orderDay);
                }

                // Defaul show data
                var parent = _context.Cscard.AsNoTracking().Where(x => x.ParentId == parentId).FirstOrDefault();
                ViewBag.ParentId = parent?.ParentId?.Trim();
                ViewBag.StudentName = parent?.Name?.Trim();

                ViewBag.ListOrderSubmit = listOrderSubmit;
            }
            
            return View();
        }

        public IActionResult ExportFile(string? parentId, string? month, string? selectDay, string? type)
        {
            List<PreOrders> data = new List<PreOrders>();
            if(type == "Parent")
            {
                if(selectDay == "all")
                {
                    data = _context.PreOrders.ToList().Where(x => x.MonthYear == month.Trim() && x.UserCode.Trim() == parentId.Trim())
                                                .OrderBy(x => x.OrderDate)
                                                .ThenBy(n => n.RepastId).ToList();
                } 
                else
                {
                    data = _context.PreOrders.ToList().Where(x => x.OrderDate.ToString("dd/MM/yyyy") == selectDay.Trim() && x.UserCode.Trim() == parentId.Trim())
                                                .OrderBy(x => x.OrderDate)
                                                .ThenBy(n => n.RepastId).ToList();
                }
            } 
            else
            {
                if (selectDay == "all")
                {
                    data = _context.PreOrders.ToList().Where(x => x.MonthYear == month.Trim())
                                                .OrderBy(x => x.UserCode)
                                                .ThenBy(n => n.OrderDate)
                                                .ThenBy(n => n.RepastId).ToList();
                }
                else
                {
                    data = _context.PreOrders.ToList().Where(x => x.OrderDate.ToString("dd/MM/yyyy") == selectDay.Trim())
                                                .OrderBy(x => x.UserCode)
                                                .ThenBy(n => n.OrderDate)
                                                .ThenBy(n => n.RepastId).ToList();
                }
            }

            List<string> columns = new List<string>(new string[] {
                "Submit date",
                "Submit time",
                "Account Code",
                "Canteen Code",
                "Account name",
                "Class/Title",
                "Period",
                "Menu date",
                "Week",
                "DoW",
                "Category",
                "Item Code",
                "CK code",
                "Item Name (VN)",
                "Item Name (EN)",
                "Qty",
                "Repast #",
                "Bundled Qty",
                "Repast Date",
                "Repast time",
            });

            using (var fs = new FileStream("Order.xlsx", FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Sheet1");

                IRow row = excelSheet.CreateRow(0);
                int columnIndex = 0;

                foreach (string col in columns)
                {
                    row.CreateCell(columnIndex).SetCellValue(col);
                    columnIndex++;
                }

                int rowIndex = 1;
                foreach (PreOrders item in data)
                {
                    Cscard cscard = _context.Cscard.Where(x => x.ParentId == item.UserCode).FirstOrDefault();
                    Goods goods = _context.Goods.Where(x => x.Ref == item.CkCode).FirstOrDefault();
                    Menus menus = _context.Menus.Where(x => x.MenuDate == item.OrderDate && x.ItemCode == item.ItemCode).FirstOrDefault();

                    row = excelSheet.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(item.SubmitDt); // Submit date
                    row.CreateCell(1).SetCellValue(item.SubmitTm); // Submit time
                    row.CreateCell(2).SetCellValue(item.UserCode); // Account Code
                    row.CreateCell(3).SetCellValue(item.CanteenId); // Canteen Code
                    row.CreateCell(4).SetCellValue(cscard.Name); // Account name
                    row.CreateCell(5).SetCellValue(item.Class); // Class/Title
                    row.CreateCell(6).SetCellValue(item.MonthYear); // Period
                    row.CreateCell(7).SetCellValue(item.OrderDate); // Menu date
                    row.CreateCell(8).SetCellValue(item.Week?.ToString()); // Week
                    row.CreateCell(9).SetCellValue(item.DoW?.ToString()); // DoW
                    row.CreateCell(10).SetCellValue(menus.Category); // Category
                    row.CreateCell(11).SetCellValue(item.SubmitDt); // Item Code
                    row.CreateCell(12).SetCellValue(item.SubmitDt); // Submit date
                    row.CreateCell(13).SetCellValue(item.SubmitDt); // Submit date
                    row.CreateCell(14).SetCellValue(item.SubmitDt); // Submit date
                    row.CreateCell(15).SetCellValue(item.SubmitDt); // Submit date
                    row.CreateCell(16).SetCellValue(item.SubmitDt); // Submit date
                    row.CreateCell(17).SetCellValue(item.SubmitDt); // Submit date
                    row.CreateCell(18).SetCellValue(item.SubmitDt); // Submit date
                    row.CreateCell(19).SetCellValue(item.SubmitDt); // Submit date
                    row.CreateCell(20).SetCellValue(item.SubmitDt); // Submit date

                    rowIndex++;
                }
                workbook.Write(fs);
            }


            return Json(new { success = true, message = "Export Successfully!" });
        }

        public IActionResult GetListDayExport(string? month)
        {
            var ListDays = _context.Menus.Where(x => x.MonthYear == month.Trim().Replace("/", "")).OrderBy(x => x.MenuDate).Select(x => x.MenuDate).Distinct();
            string html = "";
            html += "<label>Export Days: </label>";
            html += "<select class='custom-select mt-3 mb-3 ml-3' id='SelectDay' name='SelectDay' style='width: 160px;'>";
            html += "<option value ='all'>All month</option>";
            foreach(var day in ListDays)
            {
                html += "<option value ='" + day.ToString("dd/MM/yyyy") + "'>" + day.ToString("dd/MM/yyyy") + "</option>";
            }
            html += "</select>";
            return Json(new { success = true, html });
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
                            if (item.Ckcode == "0") break;

                            PreOrders order = new PreOrders
                            {
                                UserCode = currentUser.Code,
                                MonthYear = monthYear,
                                Week = null,
                                DoW = null,
                                OrderDate = orderDate,
                                SubmitDt = DateTime.Now,
                                SubmitTm = DateTime.Now.ToString("HH:mm"),
                                ItemCode = item.Code?.Trim(),
                                CkCode = item.Ckcode?.Trim(),
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
                            if (item.Ckcode == "0") break;

                            PreOrders order = new PreOrders
                            {
                                UserCode = currentUser.Code,
                                MonthYear = monthYear,
                                Week = null,
                                DoW = null,
                                OrderDate = orderDate,
                                SubmitDt = DateTime.Now,
                                SubmitTm = DateTime.Now.ToString("HH:mm"),
                                ItemCode = item.Code?.Trim(),
                                CkCode = item.Ckcode?.Trim(),
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
                            if (item.Ckcode == "0") break;

                            PreOrders order = new PreOrders
                            {
                                UserCode = currentUser.Code,
                                MonthYear = monthYear,
                                Week = null,
                                DoW = null,
                                OrderDate = orderDate,
                                SubmitDt = DateTime.Now,
                                SubmitTm = DateTime.Now.ToString("HH:mm"),
                                ItemCode = item.Code?.Trim(),
                                CkCode = item.Ckcode?.Trim(),
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
                return Json(new { success = false, message = $"Your pre-order already existed. Please contact canteen manager to revise!" });
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
        public SelectList BuildSelectBoxOrderMonth(string? month)
        {
            var Month = _context.MenuInfos.OrderBy(x => x.Month).ToList();
            var MonthList = new List<Object>();
            string currentMonth = month != null ? month : DateTime.Now.ToString("MM/yyyy");
            foreach (var m in Month)
            {
                MonthList.Add(new { Id = m.Month.ToString("MM/yyyy"), Name = m.Month.ToString("MM/yyyy") });
            }
            return new SelectList(MonthList, "Id", "Name", currentMonth);
        }

        public IActionResult GetItemDetail(string Ckcode, string Day, string CurrentMonth)
        {
            var hostname = Configuration.GetValue<string>("AppConfig:HostName");
            if(!hostname.EndsWith("/"))
            {
                hostname = hostname + "/";
            }
            DateTime date = DateTime.Parse(CurrentMonth + "/" + Day);
            Menus menu = _context.Menus.ToList().Where(x => x.Ckcode.Trim() == Ckcode.Trim() && x.MenuDate.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy")).FirstOrDefault();


            List<string> allImages = Utilities.GetAllFiles("menus/" + date.ToString("MMyyyy"));

            var imagePath = "";
            if(menu != null)
            {
                var path = "menus/" + date.ToString("MMyyyy") + "/" + Ckcode.Trim() + ".jpg";
                if (allImages.Contains(path))
                {
                    imagePath = hostname + "images/menus/" + date.ToString("MMyyyy") + "/" + Ckcode.Trim() + ".jpg";
                    
                } else
                {
                    imagePath = hostname + "images/logo/food-default.jpg";
                }
            }

            ItemDetail itemDetail = new ItemDetail
            {
                ImagePath = imagePath,
                OriginalName = menu?.OriginalName?.Trim(),
                ItemNameEn = menu?.ItemNameEn?.Trim(),
                ItemNameVn = menu?.ItemNameVn?.Trim(),
                ItemCode = menu?.ItemCode?.Trim(),
                Category = menu?.Category?.Trim(),
                Ckcode = menu?.Ckcode?.Trim()
            };

            return Json(new { success = false, itemDetail = itemDetail });
        }

    }
}
