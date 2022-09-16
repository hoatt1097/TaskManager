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
                        Breakfast = Breakfast.OrderBy(x => x.Bundled).ToList(),
                        Lunch = Lunch.OrderBy(x => x.Bundled).ToList(),
                        Afternoon = Afternoon.OrderBy(x => x.Bundled).ToList()
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
            string filename = "";
            if (type == "Parent")
            {
                if(selectDay == "all")
                {
                    filename = "Order_" + parentId.Trim() + "_" + month.Replace("/", "") + ".xlsx";
                    data = _context.PreOrders.ToList().Where(x => x.MonthYear == month.Replace("/","").Trim() && x.UserCode.Trim() == parentId.Trim())
                                                .OrderBy(x => x.OrderDate)
                                                .ThenBy(n => n.RepastId)
                                                .ThenBy(n => n.IsBundled).ToList();
                } 
                else
                {
                    filename = "Order_" + parentId.Trim() + "_" + selectDay.Replace("/", "") + ".xlsx";
                    data = _context.PreOrders.ToList().Where(x => x.OrderDate.ToString("dd/MM/yyyy") == selectDay.Trim() && x.UserCode.Trim() == parentId.Trim())
                                                .OrderBy(x => x.OrderDate)
                                                .ThenBy(n => n.RepastId)
                                                .ThenBy(n => n.IsBundled).ToList();
                }
            } 
            else
            {
                if (selectDay == "all")
                {
                    filename = "Order_All_" + month.Replace("/", "") + ".xlsx";
                    data = _context.PreOrders.ToList().Where(x => x.MonthYear == month.Replace("/", "").Trim())
                                                .OrderBy(x => x.UserCode)
                                                .ThenBy(n => n.OrderDate)
                                                .ThenBy(n => n.RepastId)
                                                .ThenBy(n => n.IsBundled).ToList();
                }
                else
                {
                    filename = "Order_All_" + selectDay.Replace("/", "") + ".xlsx";
                    data = _context.PreOrders.ToList().Where(x => x.OrderDate.ToString("dd/MM/yyyy") == selectDay.Trim())
                                                .OrderBy(x => x.UserCode)
                                                .ThenBy(n => n.OrderDate)
                                                .ThenBy(n => n.RepastId)
                                                .ThenBy(n => n.IsBundled).ToList();
                }
            }

            List<string> columns = new List<string>(new string[] {
                "Campus",
                "Campus Name",
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
                "Spending Date",
                "Spending time",
                "Total Spending Qty",
            });
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "export");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            using (var fs = new FileStream(path + "/" + filename, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(filename);

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
                    Campus campus = _context.Campus.Where(x => x.CampusId.Trim() == cscard.CampusId.Trim()).FirstOrDefault();

                    row = excelSheet.CreateRow(rowIndex);
                    row.CreateCell(columns.IndexOf("Campus")).SetCellValue(cscard.CampusId); // Campus
                    row.CreateCell(columns.IndexOf("Campus Name")).SetCellValue(campus?.Description); // Campus Name
                    row.CreateCell(columns.IndexOf("Submit date")).SetCellValue(item.SubmitDt.ToString("dd/MM/yyyy")); // Submit date
                    row.CreateCell(columns.IndexOf("Submit time")).SetCellValue(item.SubmitTm?.ToString()); // Submit time
                    row.CreateCell(columns.IndexOf("Account Code")).SetCellValue(item.UserCode?.Trim()); // Account Code
                    row.CreateCell(columns.IndexOf("Canteen Code")).SetCellValue(item.CanteenId?.Trim()); // Canteen Code
                    row.CreateCell(columns.IndexOf("Account name")).SetCellValue(cscard.Name?.Trim()); // Account name
                    row.CreateCell(columns.IndexOf("Class/Title")).SetCellValue(item.Class?.Trim()); // Class/Title
                    row.CreateCell(columns.IndexOf("Period")).SetCellValue(item.MonthYear?.Trim()); // Period
                    row.CreateCell(columns.IndexOf("Menu date")).SetCellValue(item.OrderDate.ToString("dd/MM/yyyy")); // Menu date
                    row.CreateCell(columns.IndexOf("Week")).SetCellValue(item.Week?.ToString()); // Week
                    row.CreateCell(columns.IndexOf("DoW")).SetCellValue(item.DoW?.ToString()); // DoW
                    row.CreateCell(columns.IndexOf("Category")).SetCellValue(menus.Category?.Trim()); // Category
                    row.CreateCell(columns.IndexOf("Item Code")).SetCellValue(menus.ItemCode?.Trim()); // Item Code
                    row.CreateCell(columns.IndexOf("CK code")).SetCellValue(menus.Ckcode?.Trim()); // CK code
                    row.CreateCell(columns.IndexOf("Item Name (VN)")).SetCellValue(menus.ItemNameVn?.Trim()); // Item Name (VN)
                    row.CreateCell(columns.IndexOf("Item Name (EN)")).SetCellValue(menus.ItemNameVn?.Trim()); // Item Name (EN)
                    row.CreateCell(columns.IndexOf("Qty")).SetCellValue(item.Qty); // Qty
                    row.CreateCell(columns.IndexOf("Repast #")).SetCellValue(item.RepastId); // Repast #
                    row.CreateCell(columns.IndexOf("Bundled Qty")).SetCellValue(item.IsBundled ? "1" : ""); // Bundled Qty
                    row.CreateCell(columns.IndexOf("Spending Date")).SetCellValue(item.RepastDt?.ToString("dd/MM/yyyy")); // Spending Date
                    row.CreateCell(columns.IndexOf("Spending time")).SetCellValue(item.RepastTm); // Spending time
                    row.CreateCell(columns.IndexOf("Total Spending Qty")).SetCellValue((item.RepastDt != null) ? item.Qty.ToString() : ""); // Total Spending Qty

                    rowIndex++;
                }
                workbook.Write(fs);
            }


            return Json(new { success = true, message = "Export Successfully!", filename });
        }

        public FileResult DownloadFile(string filename)
        {
            //Build the File Path.
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "export", filename);

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Utilities.DeleteFile(path);
            //Send the File to Download.
            return File(bytes, "application/octet-stream", filename);
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
                return Json(new { success = false, type= "ExistOrder", message = $"Your pre-order already existed. Please contact canteen manager to revise!</br>Bạn đã đặt món rồi. Nếu cần chỉnh sửa, vui lòng liên lạc với quản lý Canteen!" });
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
            Goods goods = _context.Goods.Where(x => x.Ref.Trim() == Ckcode).FirstOrDefault();
            string info = "";
            if(goods?.Allergy1 != null && goods?.Allergy1 != string.Empty)
            {
                info += "- " + goods?.Allergy1 + "<br/>";
            }
            if (goods?.Allergy2 != null && goods?.Allergy2 != string.Empty)
            {
                info += "- " + goods?.Allergy2 + "<br/>";
            }
            if (goods?.Allergy3 != null && goods?.Allergy3 != string.Empty)
            {
                info += "- " + goods?.Allergy3;
            }

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
                Ckcode = menu?.Ckcode?.Trim(),
                Information = info
            };

            return Json(new { success = false, itemDetail = itemDetail });
        }

        public IActionResult GetUpdateOrder(string language, string usercode, string orderday, string currentmonth, string breakfast, string lunch, string afternoon)
        {
            DateTime date = DateTime.Parse(currentmonth + "/" + orderday);
            List<Menus> listMenu = _context.Menus.ToList().Where(x => x.MenuDate.ToString("ddMMyyyy") == date.ToString("ddMMyyyy")).ToList();
            List<PreOrders> listOrder = _context.PreOrders.ToList().Where(x => x.UserCode.Trim() == usercode.Trim() && x.OrderDate.ToString("ddMMyyyy") == date.ToString("ddMMyyyy")).ToList();

            List<Menus> bf = listMenu.Where(x => x.RepastId == 1 && x.IsBundled == 0 ).ToList();
            List<Menus> bf_op = listMenu.Where(x => x.RepastId == 1 && x.IsBundled == 1).ToList();

            List<Menus> ln = listMenu.Where(x => x.RepastId == 2 && x.IsBundled == 0).ToList();
            List<Menus> ln_op = listMenu.Where(x => x.RepastId == 2 && x.IsBundled == 1).ToList();

            List<Menus> af = listMenu.Where(x => x.RepastId == 3 && x.IsBundled == 0).ToList();
            List<Menus> af_op = listMenu.Where(x => x.RepastId == 3 && x.IsBundled == 1).ToList();

            if(language == "en")
            {
                var bf_Select = new List<Object>();
                var ln_Select = new List<Object>();
                var af_Select = new List<Object>();

                bf_Select.Add(new { Id = "0", Name = "Do not eat" });
                foreach (var item in bf)
                {
                    bf_Select.Add(new { Id = item.Ckcode.Trim(), Name = item.ItemNameEn });
                }
                var bf_html = new SelectList(bf_Select, "Id", "Name", breakfast);

                foreach (var item in ln)
                {
                    ln_Select.Add(new { Id = item.Ckcode.Trim(), Name = item.ItemNameEn });
                }
                var ln_html = new SelectList(ln_Select, "Id", "Name", lunch);

                foreach (var item in af)
                {
                    af_Select.Add(new { Id = item.Ckcode.Trim(), Name = item.ItemNameEn });
                }
                var af_html = new SelectList(af_Select, "Id", "Name", afternoon);


                // Option food
                var bf_op_text = "";
                foreach(var item in bf_op)
                {
                    bf_op_text += "-" + item.ItemNameEn + "</br>";
                }

                var ln_op_text = "";
                foreach (var item in ln_op)
                {
                    ln_op_text += "-" + item.ItemNameEn + "</br>";
                }

                var af_op_text = "";
                foreach (var item in af_op)
                {
                    af_op_text += "-" + item.ItemNameEn + "</br>";
                }

                return Json(new { success = true, bf_html, bf_op_text, ln_html, ln_op_text, af_html, af_op_text });
            } 
            else
            {
                var bf_Select = new List<Object>();
                var ln_Select = new List<Object>();
                var af_Select = new List<Object>();

                bf_Select.Add(new { Id = "0", Name = "Không ăn" });
                foreach (var item in bf)
                {
                    bf_Select.Add(new { Id = item.Ckcode.Trim(), Name = item.ItemNameVn });
                }
                var bf_html = new SelectList(bf_Select, "Id", "Name", breakfast);

                foreach (var item in ln)
                {
                    ln_Select.Add(new { Id = item.Ckcode.Trim(), Name = item.ItemNameVn });
                }
                var ln_html = new SelectList(ln_Select, "Id", "Name", lunch);

                foreach (var item in af)
                {
                    af_Select.Add(new { Id = item.Ckcode.Trim(), Name = item.ItemNameVn });
                }
                var af_html = new SelectList(af_Select, "Id", "Name", afternoon);


                // Option food
                var bf_op_text = "";
                foreach (var item in bf_op)
                {
                    bf_op_text += "-" + item.ItemNameVn + "</br>";
                }

                var ln_op_text = "";
                foreach (var item in ln_op)
                {
                    ln_op_text += "-" + item.ItemNameVn + "</br>";
                }

                var af_op_text = "";
                foreach (var item in af_op)
                {
                    af_op_text += "-" + item.ItemNameVn + "</br>";
                }

                return Json(new { success = true, bf_html, bf_op_text, ln_html, ln_op_text, af_html, af_op_text });
            }            
        }

        public async Task<IActionResult> UpdateOrder(string usercode, string orderday, string currentmonth, string breakfast, string lunch, string afternoon)
        {
            var currentUser = UserHelper.GetCurrentUser(HttpContext);
            DateTime date = DateTime.Parse(currentmonth + "/" + orderday);

            PreOrders bf = _context.PreOrders.ToList().Where(x =>
                                        x.OrderDate.ToString("ddMMyyyy") == date.ToString("ddMMyyyy")
                                        && x.UserCode.Trim() == usercode.Trim()
                                        && x.RepastId == 1
                                        && x.IsBundled == false).FirstOrDefault();

            PreOrders bf_op = _context.PreOrders.ToList().Where(x =>
                                        x.OrderDate.ToString("ddMMyyyy") == date.ToString("ddMMyyyy")
                                        && x.UserCode.Trim() == usercode.Trim()
                                        && x.RepastId == 1
                                        && x.IsBundled == true).FirstOrDefault();
            if (breakfast == "0")
            {
                if(bf != null)
                {
                    _context.PreOrders.Remove(bf);
                }
                if(bf_op != null)
                {
                    _context.PreOrders.Remove(bf_op);
                }
            } 
            else
            {
                Goods goodsBf = _context.Goods.ToList().Where(x => x.Ref.Trim() == breakfast.Trim()).FirstOrDefault();
                if(bf != null)
                {
                    PreOrders newBf = new PreOrders
                    {
                        UserCode = usercode.Trim(),
                        MonthYear = date.ToString("MMyyyy"),
                        Week = null,
                        DoW = null,
                        OrderDate = date,
                        SubmitDt = bf.SubmitDt,
                        SubmitTm = bf.SubmitTm,
                        ItemCode = goodsBf.GoodsId.Trim(),
                        CkCode = goodsBf.Ref.Trim(),
                        Qty = 1,
                        RepastId = 1,
                        Class = "",
                        IsBundled = false,
                        IsChoosen = true,
                        Status = true,
                        IsModified = true,
                        ModiDate = DateTime.Now,
                        ModiTime = DateTime.Now.ToString("HH:mm"),
                        ModiUser = currentUser.Code
                    };
                    _context.PreOrders.Remove(bf);
                    _context.PreOrders.Add(newBf);
                } 
                else
                {
                    PreOrders newBf = new PreOrders
                    {
                        UserCode = usercode.Trim(),
                        MonthYear = date.ToString("MMyyyy"),
                        Week = null,
                        DoW = null,
                        OrderDate = date,
                        SubmitDt = DateTime.Now,
                        SubmitTm = DateTime.Now.ToString("HH:mm"),
                        ItemCode = goodsBf.GoodsId.Trim(),
                        CkCode = goodsBf.Ref.Trim(),
                        Qty = 1,
                        RepastId = 1,
                        Class = "",
                        IsBundled = false,
                        IsChoosen = true,
                        Status = true,
                        IsModified = true,
                        ModiDate = DateTime.Now,
                        ModiTime = DateTime.Now.ToString("HH:mm"),
                        ModiUser = currentUser.Code
                    };

                    _context.PreOrders.Add(newBf);
                }
                
                if(bf_op != null)
                {
                    bf_op.IsModified = true;
                    bf_op.ModiDate = DateTime.Now;
                    bf_op.ModiTime = DateTime.Now.ToString("HH:mm");
                    bf_op.ModiUser = currentUser.Code;
                    _context.PreOrders.Update(bf_op);
                } 
                else
                {
                    Menus option = _context.Menus.ToList().Where(x =>
                                        x.MenuDate.ToString("ddMMyyyy") == date.ToString("ddMMyyyy")
                                        && x.RepastId == 1
                                        && x.IsBundled == 1).FirstOrDefault();
                    PreOrders newBfOp = new PreOrders
                    {
                        UserCode = usercode.Trim(),
                        MonthYear = date.ToString("MMyyyy"),
                        Week = null,
                        DoW = null,
                        OrderDate = date,
                        SubmitDt = DateTime.Now,
                        SubmitTm = DateTime.Now.ToString("HH:mm"),
                        ItemCode = option.ItemCode.Trim(),
                        CkCode = option.Ckcode.Trim(),
                        Qty = 1,
                        RepastId = 1,
                        Class = "",
                        IsBundled = true,
                        IsChoosen = true,
                        Status = true,
                        IsModified = true,
                        ModiDate = DateTime.Now,
                        ModiTime = DateTime.Now.ToString("HH:mm"),
                        ModiUser = currentUser.Code
                    };
                    _context.PreOrders.Add(newBfOp);
                }            
            }

            // Lunch
            PreOrders ln = _context.PreOrders.ToList().Where(x =>
                                    x.OrderDate.ToString("ddMMyyyy") == date.ToString("ddMMyyyy")
                                    && x.UserCode.Trim() == usercode.Trim()
                                    && x.RepastId == 2
                                    && x.IsBundled == false).FirstOrDefault();

            List<PreOrders> ln_ops = _context.PreOrders.ToList().Where(x =>
                                        x.OrderDate.ToString("ddMMyyyy") == date.ToString("ddMMyyyy")
                                        && x.UserCode.Trim() == usercode.Trim()
                                        && x.RepastId == 2
                                        && x.IsBundled == true).ToList();
            if (ln.CkCode.Trim() != lunch.Trim())
            {
                Goods goodsLn = _context.Goods.ToList().Where(x => x.Ref.Trim() == lunch.Trim()).FirstOrDefault();
                PreOrders newLn = new PreOrders
                {
                    UserCode = usercode.Trim(),
                    MonthYear = date.ToString("MMyyyy"),
                    Week = null,
                    DoW = null,
                    OrderDate = date,
                    SubmitDt = ln.SubmitDt,
                    SubmitTm = ln.SubmitTm,
                    ItemCode = goodsLn.GoodsId.Trim(),
                    CkCode = goodsLn.Ref.Trim(),
                    Qty = 1,
                    RepastId = 2,
                    Class = "",
                    IsBundled = false,
                    IsChoosen = true,
                    Status = true,
                    IsModified = true,
                    ModiDate = DateTime.Now,
                    ModiTime = DateTime.Now.ToString("HH:mm"),
                    ModiUser = currentUser.Code
                };
                _context.PreOrders.Remove(ln);
                _context.PreOrders.Add(newLn);

                foreach (var item in ln_ops)
                {
                    item.IsModified = true;
                    item.ModiDate = DateTime.Now;
                    item.ModiTime = DateTime.Now.ToString("HH:mm");
                    item.ModiUser = currentUser.Code;
                    _context.PreOrders.Update(item);
                }
            }


            // afternoon
            PreOrders af = _context.PreOrders.ToList().Where(x =>
                                    x.OrderDate.ToString("ddMMyyyy") == date.ToString("ddMMyyyy")
                                    && x.UserCode.Trim() == usercode.Trim()
                                    && x.RepastId == 3
                                    && x.IsBundled == false).FirstOrDefault();

            List<PreOrders> af_ops = _context.PreOrders.ToList().Where(x =>
                                        x.OrderDate.ToString("ddMMyyyy") == date.ToString("ddMMyyyy")
                                        && x.UserCode.Trim() == usercode.Trim()
                                        && x.RepastId == 3
                                        && x.IsBundled == true).ToList();
            if (af.CkCode.Trim() != afternoon.Trim())
            {
                Goods goodsAf = _context.Goods.ToList().Where(x => x.Ref.Trim() == afternoon.Trim()).FirstOrDefault();
                PreOrders newAf = new PreOrders
                {
                    UserCode = usercode.Trim(),
                    MonthYear = date.ToString("MMyyyy"),
                    Week = null,
                    DoW = null,
                    OrderDate = date,
                    SubmitDt = af.SubmitDt,
                    SubmitTm = af.SubmitTm,
                    ItemCode = goodsAf.GoodsId.Trim(),
                    CkCode = goodsAf.Ref.Trim(),
                    Qty = 1,
                    RepastId = 3,
                    Class = "",
                    IsBundled = false,
                    IsChoosen = true,
                    Status = true,
                    IsModified = true,
                    ModiDate = DateTime.Now,
                    ModiTime = DateTime.Now.ToString("HH:mm"),
                    ModiUser = currentUser.Code
                };
                _context.PreOrders.Remove(af);
                _context.PreOrders.Add(newAf);

                foreach (var item in af_ops)
                {
                    item.IsModified = true;
                    item.ModiDate = DateTime.Now;
                    item.ModiTime = DateTime.Now.ToString("HH:mm");
                    item.ModiUser = currentUser.Code;
                    _context.PreOrders.Update(item);
                }
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Update order sucessfully" });
        }
    }
}
