using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using UTS_Portal.Extension;
using UTS_Portal.Models;

namespace UTS_Portal.Controllers
{
    public class MenusController : Controller
    {
        private readonly db_utsContext _context;

        public MenusController(db_utsContext context)
        {
            _context = context;
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

        // POST: MenuInfos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Month,Images,Status")] MenuInfos menuInfos, IEnumerable<Microsoft.AspNetCore.Http.IFormFile> Images)
        {
            if (ModelState.IsValid)
            {
                var existMonth = _context.MenuInfos.ToList().Where(x => x.Month.ToString("yyyyMM") == menuInfos.Month.ToString("yyyyMM")).FirstOrDefault();
                if (existMonth != null)
                {
                    ModelState.AddModelError("Month", "Month is exist");
                    return View(menuInfos);
                }

                //Handle Images
                var listImagePath = new List<string>();
                if (Images != null)
                {
                    int fileNumber = 1;
                    foreach(var image in Images)
                    {
                        string newFilename = "menus_" + menuInfos.Month.ToString("yyyyMM") + "_" + fileNumber;
                        string folder = "menus/" + menuInfos.Month.ToString("yyyyMM");
                        string extension = Path.GetExtension(image.FileName);
                        string imageName = newFilename + extension;
                        var imagePath = await Utilities.UploadFile(image, folder, imageName.ToLower());

                        listImagePath.Add(folder.ToLower() + "/" + imagePath);
                        fileNumber++;
                    } 
                }

                menuInfos.Images = string.Join(";", listImagePath);
                _context.Add(menuInfos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menuInfos);
        }

        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuInfos = await _context.MenuInfos.FindAsync(id);
            if (menuInfos == null)
            {
                return NotFound();
            }
            return View(menuInfos);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Month,Images")] MenuInfos menuInfos)
        {
            if (id != menuInfos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }
            return View(menuInfos);
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
                string folder = "menus/" + menuInfos.Month.ToString("yyyyMM");
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
    }
}
