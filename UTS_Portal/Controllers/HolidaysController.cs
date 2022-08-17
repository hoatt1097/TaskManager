using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UTS_Portal.Models;
using UTS_Portal.ViewModels;

namespace UTS_Portal.Controllers
{
    public class HolidaysController : Controller
    {
        private readonly db_utsContext _context;
        public INotyfService _notyfService { get; }
        public HolidaysController(db_utsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Holidays
        public async Task<IActionResult> Index(int yearFilter = 0)
        {
            var DB_Holidays = _context.Holidays.OrderBy(x => x.Day).ToList();

            if (yearFilter != 0)
            {
                DB_Holidays = DB_Holidays.Where(x => x.Day.Year == yearFilter).ToList();
            }
            
            var DB_Holidays_Convert = DB_Holidays.Select(k => 
                    new { 
                        Key = k.Day.ToString("yyyy/MM"), 
                        Value = new Holiday { Day = k.Day.ToString("yyyy-MM-dd"), Description = k.Description }
                    }).ToList();

            List<HolidaysByMonth> list = DB_Holidays_Convert.GroupBy(
                    x => x.Key, 
                    x => x.Value, 
                    (key, value) => new HolidaysByMonth{ Month = key, Holidays = value.OrderBy(x => x.Day).ToList()}
                ).ToList();

            // Create new YearFilter
            var yearList = new List<Object>() ;
            int currentYear = DateTime.Now.Year;
            for(int i = currentYear - 1; i <= currentYear + 3; i++)
            {
                yearList.Add(new { Id = i, Name = i });
            }
            ViewData["YearFilter"] = new SelectList(yearList, "Id", "Name", yearFilter);
            return View(list);
        }

        public IActionResult ListHolidayFiltter(int yearFilter = 0)
        {
            var url = $"/Holidays/Index?yearFilter={yearFilter}";
            if (yearFilter == 0)
            {
                url = $"/Holidays/Index";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Holidays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidays = await _context.Holidays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (holidays == null)
            {
                return NotFound();
            }

            return View(holidays);
        }

        // GET: Holidays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Holidays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Day,Description")] HolidayCreateModel holidays)
        {
            if (ModelState.IsValid)
            {
                List<String> listDay = holidays.Day.Split(",").ToList();
                foreach(string day in listDay)
                {
                    
                    Holidays isHolidaysExists = _context.Holidays.Where(e => e.Day == DateTime.Parse(day)).FirstOrDefault();
                    if(isHolidaysExists != null)
                    {
                        isHolidaysExists.Description = holidays.Description;
                        _context.Update(isHolidaysExists);
                    } 
                    else
                    {
                        Holidays newHoliday = new Holidays
                        {
                            Day = DateTime.Parse(day),
                            Description = holidays.Description
                        };
                        _context.Add(newHoliday);
                    }
                    await _context.SaveChangesAsync();
                }
                _notyfService.Success("Add holidays sucessfully!");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Error("Add holidays unsucessfully!");
            return View(holidays);
        }

        // GET: Holidays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidays = await _context.Holidays.FindAsync(id);
            if (holidays == null)
            {
                return NotFound();
            }
            return View(holidays);
        }

        // POST: Holidays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Day,Description")] Holidays holidays)
        {
            if (id != holidays.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(holidays);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HolidaysExists(holidays.Id))
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
            return View(holidays);
        }

        // GET: Holidays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidays = await _context.Holidays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (holidays == null)
            {
                return NotFound();
            }

            return View(holidays);
        }

        // POST: Holidays/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string month)
        {
            var holidays = _context.Holidays.ToList().Where(x => x.Day.ToString("yyyy/MM") == month).ToList();

            foreach(var holiday in holidays)
            {
                _context.Holidays.Remove(holiday);
            }
            
            await _context.SaveChangesAsync();

            // return RedirectToAction(nameof(Index));
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        private bool HolidaysExists(int id)
        {
            return _context.Holidays.Any(e => e.Id == id);
        }
    }
}
