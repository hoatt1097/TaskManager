using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PagedList.Core;
using UTS_Portal.Extension;
using UTS_Portal.Helpers;
using UTS_Portal.Models;
using UTS_Portal.ViewModels;

namespace UTS_Portal.Controllers
{
    public class MenusController : Controller
    {
        private readonly db_utsContext _context;

        public INotyfService _notyfService { get; }
        public MenusController(db_utsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Menus
        public async Task<IActionResult> Index(int? page, int yearFilter = 0)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;

            var ls = _context.MenuInfos.OrderBy(x => x.Month);

            if (yearFilter != 0)
            {
                ls = _context.MenuInfos.Where(x => x.Month.Year == yearFilter).OrderBy(x => x.Month);
            }

            var models = new PagedList<MenuInfos>(ls, pageNumber, pageSize);

            // Create new YearFilter
            var yearList = new List<Object>();
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 1; i <= currentYear + 3; i++)
            {
                yearList.Add(new { Id = i, Name = i });
            }
            ViewData["YearFilter"] = new SelectList(yearList, "Id", "Name", yearFilter);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.Total = ls.Count();

            return View(models);
        }

        public IActionResult ListMenusFiltter(int yearFilter = 0)
        {
            var url = $"/Menus/Index?yearFilter={yearFilter}";
            if (yearFilter == 0)
            {
                url = $"/Menus/Index";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuInfos = await _context.MenuInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuInfos == null)
            {
                return NotFound();
            }

            return View(menuInfos);
        }

        // GET: Menus/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit = 268435456)]
        [RequestSizeLimit(268435456)]
        public async Task<IActionResult> Create([Bind("Id,Month,Images,Status")] MenuInfos menuInfos, IEnumerable<Microsoft.AspNetCore.Http.IFormFile> fThumbs)
        {
            if (ModelState.IsValid)
            {
                var existMonth = _context.MenuInfos.ToList().Where(x => x.Month.ToString("MMyyyy") == menuInfos.Month.ToString("MMyyyy")).FirstOrDefault();
                if (existMonth != null)
                {
                    ModelState.AddModelError("Month", "Month is exist");
                    return View(menuInfos);
                }

                //Handle Images
                var listImagePath = new List<string>();
                if (fThumbs != null && fThumbs.Count() > 0)
                {
                    int fileNumber = 1;
                    foreach(var image in fThumbs)
                    {
                        string newFilename = "menus_" + menuInfos.Month.ToString("MMyyyy") + "_" + fileNumber;
                        string folder = "menus/" + menuInfos.Month.ToString("MMyyyy");
                        string extension = Path.GetExtension(image.FileName);
                        string imageName = newFilename + extension;
                        var imagePath = await Utilities.UploadFile(image, folder, imageName.ToLower());

                        listImagePath.Add(folder.ToLower() + "/" + imagePath);
                        fileNumber++;
                    }
                    menuInfos.Images = string.Join(";", listImagePath);
                }

                _context.Add(menuInfos);
                await _context.SaveChangesAsync();

                _notyfService.Success("Add new menu sucessfully!");
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error("Add new menu unsucessfully!");
            return View(menuInfos);
        }

        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int? id, string? tab)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.TabView = tab != null ? tab : "info";

            var menuInfos = await _context.MenuInfos.FindAsync(id);
            if (menuInfos == null)
            {
                return NotFound();
            }

            // Get all images in folder
            ViewBag.MenuFileNames = Utilities.GetAllFiles("menus/" + menuInfos.Month.ToString("MMyyyy"));

            // Call data menu in month
            List<CalendarMonth> CalendarMonth = DateHelper.GetCalendar(_context, menuInfos.Month);
            ViewBag.CalendarMonth = CalendarMonth;

            // Collect data menu item by month
            List<MenusByMonth> MenusByMonth = MenuHelper.GetMenusByMonth(_context, menuInfos.Month.ToString("MM/yyyy"));
            ViewBag.MenusByMonth = MenusByMonth;

            // Check menu item data has data
            ViewBag.HasMenuData = MenusByMonth.Count > 0 ? true : false;

            return View(menuInfos);
        }

        public async Task<IActionResult> Images(int? id)
        {
            // Get all images in folder
            var menuInfos = await _context.MenuInfos.FindAsync(id);
            ViewBag.MenuId = menuInfos.Id;
            ViewBag.CurrentMonth = menuInfos.Month.ToString("MM/yyyy");
            ViewBag.MenuFileNames = Utilities.GetAllFiles("menus/" + menuInfos.Month.ToString("MMyyyy"));
            return View();
        }

        public async Task<IActionResult> ImagesView(string? month)
        {
            //Format month from select: MM/yyyy
            // Get selectbox select month
            string SelectBoxElement = BuildTxtMonth();
            ViewBag.SelectBoxElement = SelectBoxElement;

            if (month != null)
            {
                // Get all images in folder
                var menuInfos = _context.MenuInfos.ToList().Where(x => x.Month.ToString("MM/yyyy") == month && x.Status == true).FirstOrDefault();
                if (menuInfos == null)
                {
                    return NotFound();
                }
                ViewBag.MenuId = menuInfos.Id;
                ViewBag.CurrentMonth = menuInfos.Month.ToString("MM/yyyy");
                ViewBag.MenuFileNames = Utilities.GetAllFiles("menus/" + menuInfos.Month.ToString("MMyyyy"));
            }

            ViewBag.CurrentMonth = month;
            return View();
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Month,Images,Status")] MenuInfos menuInfos, IEnumerable<Microsoft.AspNetCore.Http.IFormFile> fThumbs)
        {
            if (id != menuInfos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Handle Images
                    var listImagePath = new List<string>();
                    if (fThumbs != null && fThumbs.Count() > 0)
                    {
                        string folder = "menus/" + menuInfos.Month.ToString("MMyyyy");
                        // Delete all images
                        if (menuInfos.Month != null)
                        {
                            Utilities.DeleteAllFiles(folder);
                        }

                        foreach (var image in fThumbs)
                        {
                            string imageName = image.FileName;
                            var imagePath = await Utilities.UploadFile(image, folder, imageName.ToLower());

                            listImagePath.Add(folder.ToLower() + "/" + imagePath);
                        }
                        menuInfos.Images = folder;
                    }

                    _context.Update(menuInfos);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuInfosExists(menuInfos.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                _notyfService.Success("Update  menu sucessfully!");

                // Get all images in folder
                ViewBag.MenuFileNames = Utilities.GetAllFiles("menus/" + menuInfos.Month.ToString("MMyyyy"));

                // Call data menu in month
                List<CalendarMonth> CalendarMonth = DateHelper.GetCalendar(_context, menuInfos.Month);
                ViewBag.CalendarMonth = CalendarMonth;

                // Collect data menu item by month
                List<MenusByMonth> MenusByMonth = MenuHelper.GetMenusByMonth(_context, menuInfos.Month.ToString("MM/yyyy"));
                ViewBag.MenusByMonth = MenusByMonth;

                // Check menu item data has data
                ViewBag.HasMenuData = MenusByMonth.Count > 0 ? true : false;

                return View(menuInfos);
            }

            return View(menuInfos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoto(int id, IEnumerable<Microsoft.AspNetCore.Http.IFormFile> updateImages)
        {
            var menuInfos = await _context.MenuInfos.FirstOrDefaultAsync(m => m.Id == id);
            if (ModelState.IsValid)
            {
                try
                {
                    //Handle Images
                    if (updateImages != null && updateImages.Count() > 0)
                    {
                        var listImagePath = new List<string>();
                        string folder = "menus/" + menuInfos.Month.ToString("MMyyyy");

                        foreach (var image in updateImages)
                        {
                            string imageName = image.FileName;
                            var imagePath = await Utilities.UploadFile(image, folder, imageName.ToLower());

                            listImagePath.Add(folder.ToLower() + "/" + imagePath);
                        }
                        menuInfos.Images = folder;

                        _context.Update(menuInfos);
                        await _context.SaveChangesAsync();

                        // Get all images in folder
                        ViewBag.MenuFileNames = Utilities.GetAllFiles("menus/" + menuInfos.Month.ToString("MMyyyy"));

                        // Call data menu in month
                        List<CalendarMonth> CalendarMonth = DateHelper.GetCalendar(_context, menuInfos.Month);
                        ViewBag.CalendarMonth = CalendarMonth;

                        // Collect data menu item by month
                        List<MenusByMonth> MenusByMonth = MenuHelper.GetMenusByMonth(_context, menuInfos.Month.ToString("MM/yyyy"));
                        ViewBag.MenusByMonth = MenusByMonth;

                        // Check menu item data has data
                        ViewBag.HasMenuData = MenusByMonth.Count > 0 ? true : false;

                        _notyfService.Success("Add photos sucessfully!");
                        return RedirectToAction("Edit", new { id });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }   
            }
            _notyfService.Error("Add photos unsucessfully!");
            return RedirectToAction("Edit", new { id });
        }

        // GET: Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuInfos = await _context.MenuInfos.FirstOrDefaultAsync(m => m.Id == id);
            if (menuInfos == null)
            {
                return NotFound();
            }

            return View(menuInfos);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuInfos = await _context.MenuInfos.FindAsync(id);

            // Delete all images in project
            if(menuInfos.Month != null)
            {
                string folder = "menus/" + menuInfos.Month.ToString("MMyyyy");
                Utilities.DeleteAllFiles(folder);
            }

            _context.MenuInfos.Remove(menuInfos);
            await _context.SaveChangesAsync();

            // return RedirectToAction(nameof(Index));
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        private bool MenuInfosExists(int id)
        {
            return _context.MenuInfos.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadMenu(int id, IEnumerable<Microsoft.AspNetCore.Http.IFormFile> fMenus)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(fMenus == null || fMenus.Count() == 0)
                    {
                        _notyfService.Error("Please choose file to import!");
                        return RedirectToAction("Edit", new { id = id, tab = "import_menu" });
                    }

                    // Get menu
                    var menuInfos = await _context.MenuInfos.FirstOrDefaultAsync(m => m.Id == id);
                    string MonthImport = menuInfos.Month.ToString("MMyyyy");

                    //Handle File
                    var file = fMenus.FirstOrDefault();
                    var inputstream = file.OpenReadStream();

                    XSSFWorkbook workbook = new XSSFWorkbook(inputstream);

                    var FIRST_ROW_NUMBER = 1;

                    ISheet sheet = workbook.GetSheetAt(0);
                    // Example: var firstCellRow = (int)sheet.GetRow(0).GetCell(0).NumericCellValue;

                    // Check data
                    if(sheet.LastRowNum < 1)
                    {
                        _notyfService.Error("Data not exist!");
                        return RedirectToAction("Edit", new { id = id, tab = "import_menu" });
                    }

                    // Check data
                    if (sheet.GetRow(1).GetCell(0).ToString() != MonthImport)
                    {
                        string error = "Data " + sheet.GetRow(1).GetCell(0).ToString() + " not correct!";
                        _notyfService.Error(error);
                        return RedirectToAction("Edit", new { id = id, tab = "import_menu" });
                    }

                    // Delete all data of current month
                    var OldData = _context.Menus.Where(x => x.MonthYear == MonthImport);
                    _context.Menus.RemoveRange(OldData);
                    await _context.SaveChangesAsync();

                    for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++)
                    {
                        IRow currentRow = sheet.GetRow(rowIdx);
                        if (currentRow == null || currentRow.Cells == null || currentRow.Cells.Count() < FIRST_ROW_NUMBER) break;

                        var df = new DataFormatter();

                        if (currentRow.GetCell(6) == null || currentRow.GetCell(6).ToString() == "") continue;

                        var Ckcode = currentRow.GetCell(6).ToString();
                        Goods goods = _context.Goods.Where(x => x.Ref == Ckcode).FirstOrDefault();

                        var MonthYear = MonthImport;
                        /*var Week = (int?)currentRow.GetCell(1).NumericCellValue;
                        var Dow = (int?)currentRow.GetCell(2).NumericCellValue;*/
                        var MenuDate = DateTime.ParseExact(currentRow.GetCell(3)?.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var Category = currentRow.GetCell(4).ToString();
                        var ItemCode = goods != null ? goods.GoodsId : "";
                        var OriginalName = goods != null ? goods.OtherName : currentRow.GetCell(7).ToString();
                        var ItemNameVn = goods != null ? goods.ShortName : currentRow.GetCell(8).RichStringCellValue.ToString(); // Fomular
                        var ItemNameEn = goods != null ? goods.EnName : currentRow.GetCell(9).RichStringCellValue.ToString(); // Fomular
                        var Qty = (int?)currentRow.GetCell(10).NumericCellValue;
                        var Repast = (int)currentRow.GetCell(11).NumericCellValue;
                        var Class = currentRow.GetCell(12) != null ? currentRow.GetCell(12).ToString() : "";
                        var Bundled = (int)currentRow.GetCell(13).NumericCellValue;
                        var IsOrdered = (int)currentRow.GetCell(14).NumericCellValue;
                        /*var Status = (int)currentRow.GetCell(15).NumericCellValue;*/
                        var Status = 1;

                        Menus menu = new Menus
                        {
                            MonthYear = MonthYear.Trim(),
                            /*Week = Week,
                            DoW = Dow,*/
                            MenuDate = MenuDate,
                            Category = Category.Trim(),
                            ItemCode = ItemCode.Trim(),
                            Ckcode = Ckcode.Trim(),
                            OriginalName = OriginalName.Trim(),
                            ItemNameVn = ItemNameVn.Trim(), // Fomular
                            ItemNameEn = ItemNameEn.Trim(), // Fomular
                            Qty = Qty,
                            RepastId = Repast,
                            Class = Class.Trim(),
                            IsBundled = Bundled,
                            IsOrdered = IsOrdered,
                            Status = Status,
                        };

                        _context.Add(menu);
                    }

                    _context.SaveChanges();
                    _notyfService.Success("Add menu sucessfully!");
                    return RedirectToAction("Edit", new { id = id, tab="import_menu" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                _notyfService.Error("Add menu unsucessfully!");
                return RedirectToAction("Edit", new { id = id, tab = "import_menu" });
            }

            return RedirectToAction("Edit", new { id = id, tab = "import_menu" });
        }

        public string BuildTxtMonth()
        {
            var MonthActive = _context.MenuInfos.OrderBy(x => x.Month).Where(x => x.Status == true)
                .Select(x => x.Month.ToString("MM/yyyy")).ToList();

            string selectElm = "<select class='custom-select' id='txtMonthSelect' name='txtMonthSelect'>";

            foreach (var m in MonthActive)
            {
                selectElm += "<option value='" + m + "'>" + m + "</option>";
            }
            selectElm += "</select >";

            return selectElm;
        }
    }

   
}
