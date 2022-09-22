using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                Cscard cscard = _context.Cscard.Where(x => x.ParentId.Trim() == parentId.Trim()).FirstOrDefault();
                if(cscard != null)
                {
                    ViewData["Bal_Amount"] = cscard.BalAmount.ToString("#,###", cul.NumberFormat);
                }
                // Defaul show data
                ViewBag.ParentId = parentId.Trim();
                ViewBag.StudentName = cscard.Name.Trim();
                ViewBag.From = from?.Trim();
                ViewBag.To = to?.Trim();
            }

            return View();
        }

        public IActionResult GetSpendingReportData(string from, string to, string usercode)
        {
            List<SpendingReport> data = new List<SpendingReport>();
            if (string.IsNullOrEmpty(usercode) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                return Json(new { data });
            }
            else
            {
                // Call Proc
                for (int i = 0; i < 159; i++)
                {
                    SpendingReport spendingReport = new SpendingReport()
                    {
                        Date = DateTime.Now.ToString("dd/MM/yyyy"),
                        Time = DateTime.Now.ToString("HH:mm"),
                        ReceiptNumber = i.ToString(),
                        Type = "Type",
                        Quantity = (i).ToString(),
                        UnitPrice = "10000",
                        Amount = (i * 10000).ToString(),
                        CkCode = "CkCode",
                        ItemNameVN = "ItemNameVN",
                        ItemNameEN = "ItemNameEN",
                        Meal = "Meal",
                        Remark = "Remark"
                    };
                    data.Add(spendingReport);
                }
            }
            
            
            return Json(new { data });
        }

        public IActionResult SaleReport()
        {
            return View();
        }
    }
}
