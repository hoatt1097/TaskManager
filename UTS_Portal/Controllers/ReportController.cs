using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UTS_Portal.Helpers;
using UTS_Portal.Models;
using UTS_Portal.ViewModels;

namespace UTS_Portal.Controllers
{
    public class ReportController : Controller
    {
        private readonly db_utsContext _context;
        public INotyfService _notyfService { get; }
        public ReportController(db_utsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SpendingReport(string? parentId, string? from, string? to)
        {
            var currentUser = UserHelper.GetCurrentUser(HttpContext);
            if (currentUser.RoleName?.Trim() == "Parent")
            {
                if (currentUser.Code?.Trim() != parentId?.Trim())
                {
                    _notyfService.Error("User has not permission!");
                    return RedirectToAction("Index", "Home");
                }
            }

            var campusList = new List<SelectListItem>();
            var campus = _context.Campus.ToList();
            foreach (var item in campus)
            {
                var disabled = true;
                var selected = false;
                if (item.CampusId == "00")
                {
                    disabled = false;
                    selected = true;
                }
                campusList.Add(new SelectListItem { Text = item.Description, Value = item.CampusId, Selected = selected, Disabled = disabled });
            }
            ViewData["CampusList"] = campusList;

            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            if (!string.IsNullOrEmpty(parentId))
            {
                if(parentId.ToUpper() == "ALL") 
                {
                    // Defaul show data
                    ViewBag.ParentId = parentId;
                    ViewBag.StudentName = "All user";
                    ViewBag.From = from?.Trim();
                    ViewBag.To = to?.Trim();
                }
                else
                {
                    Cscard cscard = _context.Cscard.Where(x => x.ParentId.Trim() == parentId.Trim()).FirstOrDefault();
                    if (cscard != null)
                    {
                        ViewData["Bal_Amount"] = cscard.BalAmount.ToString("#,###", cul.NumberFormat);
                    }
                    // Defaul show data
                    ViewBag.ParentId = parentId.Trim();
                    ViewBag.StudentName = cscard.Name.Trim();
                    ViewBag.From = from?.Trim();
                    ViewBag.To = to?.Trim();
                }
                
            }

            return View();
        }

        public IActionResult GetSpendingReportData(string from, string to, string usercode)
        {
            /*CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"*/
            List<SpendingReport> data = new List<SpendingReport>();
            if (string.IsNullOrEmpty(usercode) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                return Json(new { data });
            }
            else
            {
                // Call Proc
                var fromStr = DateTime.ParseExact(from, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                var toStr = DateTime.ParseExact(to, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                 
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    if (usercode.ToUpper() == "ALL")
                    {
                        command.CommandText = string.Format("EXEC dbo.Rp_UserSpending @frDate = '{0}', @toDate = '{1}', @customerId = null", fromStr, toStr);
                    } else
                    {
                        command.CommandText = string.Format("EXEC dbo.Rp_UserSpending @frDate = '{0}', @toDate = '{1}', @customerId = '{2}'", fromStr, toStr, usercode);
                    }
                        
                    _context.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            SpendingReport spendingReport = new SpendingReport()
                            {
                                Date = Convert.ToDateTime(result["Tran_Date"]).ToString("dd/MM/yyyy"),
                                Time = result["Tran_Time"].ToString(),
                                ReceiptNumber = result["Trans_Num"].ToString(),
                                Type = result["Trans_Name"].ToString(),
                                Quantity = (int)(decimal)result["Qty"],
                                UnitPrice = (decimal)result["Price"],
                                Amount = (decimal)result["Amount"],
                                CkCode = result["CK_Code"].ToString(),
                                ItemNameVN = result["ItemNameVN"].ToString(),
                                ItemNameEN = result["ItemNameEN"].ToString(),
                                Meal = result["Meal"].ToString(),
                                Remark = result["Remark"].ToString(),
                            };
                            data.Add(spendingReport);
                        }
                        // do something with result
                    }
                }
            }
            
            
            return Json(new { data });
        }

        public IActionResult SaleDetail(string? from, string? to, string? campusId)
        {
            var currentUser = UserHelper.GetCurrentUser(HttpContext);
            if (currentUser.RoleName?.Trim() == "Parent")
            {
                _notyfService.Error("User has not permission!");
                return RedirectToAction("Index", "Home");
            }

            var campusList = new List<SelectListItem>();
            var campus = _context.Campus.ToList();
            foreach (var item in campus)
            {
                var disabled = true;
                var selected = false;
                if (item.CampusId == "00")
                {
                    disabled = false;
                    selected = true;
                }
                campusList.Add(new SelectListItem { Text = item.Description, Value = item.CampusId, Selected = selected, Disabled = disabled });
            }
            ViewData["CampusList"] = campusList;
            ViewBag.From = from?.Trim();
            ViewBag.To = to?.Trim();
            return View();
        }

        public IActionResult GetSaleDetailData(string from, string to, string campusId)
        {
            List<SaleDetail> data = new List<SaleDetail>();
            if (string.IsNullOrEmpty(campusId) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                return Json(new { data });
            }
            else
            {
                // Call Proc
                var fromStr = DateTime.ParseExact(from, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                var toStr = DateTime.ParseExact(to, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = string.Format("EXEC dbo.Rp_SaleDtl @frDate = '{0}', @toDate = '{1}', @ReportType = 1, @CampusId = ''", fromStr, toStr);

                    _context.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            SaleDetail item = new SaleDetail()
                            {
                                Campus = result["Campus"].ToString(),
                                Date = Convert.ToDateTime(result["Tran_Date"]).ToString("dd/MM/yyyy"),
                                Time = result["Tran_Time"].ToString(),
                                ReceiptNumber = result["Trans_Num"].ToString(),
                                Type = result["Trans_Name"].ToString(),
                                Quantity = (int)(decimal)result["Qty"],
                                UnitPrice = (decimal)result["Price"],
                                Amount = (decimal)result["Amount"],
                                GrpName = result["Grp_Name"].ToString(),
                                CkCode = result["CK_Code"].ToString(),
                                ItemNameVN = result["ItemNameVN"].ToString(),
                                ItemNameEN = result["ItemNameEN"].ToString(),
                                Meal = result["Meal"].ToString(),
                                Remark = result["Remark"].ToString(),
                            };
                            data.Add(item);
                        }
                        // do something with result
                    }
                }
            }


            return Json(new { data });
        }

        public IActionResult SaleCategory(string? from, string? to, string? campusId)
        {
            var currentUser = UserHelper.GetCurrentUser(HttpContext);
            if (currentUser.RoleName?.Trim() == "Parent")
            {
                _notyfService.Error("User has not permission!");
                return RedirectToAction("Index", "Home");
            }

            var campusList = new List<SelectListItem>();
            var campus = _context.Campus.ToList();
            foreach (var item in campus)
            {
                var disabled = true;
                var selected = false;
                if (item.CampusId == "00")
                {
                    disabled = false;
                    selected = true;
                }
                campusList.Add(new SelectListItem { Text = item.Description, Value = item.CampusId, Selected = selected, Disabled = disabled });
            }
            ViewData["CampusList"] = campusList;
            ViewBag.From = from?.Trim();
            ViewBag.To = to?.Trim();
            return View();
        }

        public IActionResult GetSaleCategoryData(string from, string to, string campusId)
        {
            List<SaleCategory> data = new List<SaleCategory>();
            if (string.IsNullOrEmpty(campusId) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                return Json(new { data });
            }
            else
            {
                // Call Proc
                var fromStr = DateTime.ParseExact(from, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                var toStr = DateTime.ParseExact(to, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = string.Format("EXEC dbo.Rp_SaleDtl @frDate = '{0}', @toDate = '{1}', @ReportType = 2, @CampusId = ''", fromStr, toStr);

                    _context.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            SaleCategory item = new SaleCategory()
                            {
                                Campus = result["Campus"].ToString(),
                                Date = Convert.ToDateTime(result["Tran_Date"]).ToString("dd/MM/yyyy"),
                                Quantity = (int)(decimal)result["Qty"],
                                Amount = (decimal)result["Amount"],
                                GrpName = result["Grp_Name"].ToString(),
                            };
                            data.Add(item);
                        }
                        // do something with result
                    }
                }
            }


            return Json(new { data });
        }

        public IActionResult SaleMeal(string? from, string? to, string? campusId)
        {
            var currentUser = UserHelper.GetCurrentUser(HttpContext);
            if (currentUser.RoleName?.Trim() == "Parent")
            {
                _notyfService.Error("User has not permission!");
                return RedirectToAction("Index", "Home");
            }

            var campusList = new List<SelectListItem>();
            var campus = _context.Campus.ToList();
            foreach (var item in campus)
            {
                var disabled = true;
                var selected = false;
                if (item.CampusId == "00")
                {
                    disabled = false;
                    selected = true;
                }
                campusList.Add(new SelectListItem { Text = item.Description, Value = item.CampusId, Selected = selected, Disabled = disabled });
            }
            ViewData["CampusList"] = campusList;
            ViewBag.From = from?.Trim();
            ViewBag.To = to?.Trim();
            return View();
        }

        public IActionResult GetSaleMealData(string from, string to, string campusId)
        {
            List<SaleMeal> data = new List<SaleMeal>();
            if (string.IsNullOrEmpty(campusId) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                return Json(new { data });
            }
            else
            {
                // Call Proc
                var fromStr = DateTime.ParseExact(from, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                var toStr = DateTime.ParseExact(to, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = string.Format("EXEC dbo.Rp_SaleDtl @frDate = '{0}', @toDate = '{1}', @ReportType = 3, @CampusId = ''", fromStr, toStr);

                    _context.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            SaleMeal item = new SaleMeal()
                            {
                                Campus = result["Campus"].ToString(),
                                Date = Convert.ToDateTime(result["Tran_Date"]).ToString("dd/MM/yyyy"),
                                Quantity = (int)(decimal)result["Qty"],
                                Amount = (decimal)result["Amount"],
                                Meal = result["Meal"].ToString(),
                            };
                            data.Add(item);
                        }
                        // do something with result
                    }
                }
            }


            return Json(new { data });
        }
    }
}
