using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using UTS_Portal.Models;

namespace UTS_Portal.Controllers
{
    public class FAQsController : Controller
    {
        private readonly db_utsContext _context;

        public FAQsController(db_utsContext context)
        {
            _context = context;
        }

        // GET: FAQ
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var ls = _context.Faqs.AsNoTracking().OrderBy(x => x.OrderNo);
            PagedList<Faqs> models = new PagedList<Faqs>(ls, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.Total = ls.Count();
            return View(models);
        }

        public async Task<IActionResult> List(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var ls = _context.Faqs.AsNoTracking().OrderBy(x => x.OrderNo);
            PagedList<Faqs> models = new PagedList<Faqs>(ls, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.Total = ls.Count();
            return View(models);
        }

        // GET: FAQ/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faqs = await _context.Faqs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faqs == null)
            {
                return NotFound();
            }

            return View(faqs);
        }

        // GET: FAQ/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FAQ/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderNo,Title,Contents")] Faqs faqs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faqs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faqs);
        }

        // GET: FAQ/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faqs = await _context.Faqs.FindAsync(id);
            if (faqs == null)
            {
                return NotFound();
            }
            return View(faqs);
        }

        // POST: FAQ/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderNo,Title,Contents")] Faqs faqs)
        {
            if (id != faqs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faqs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QaExists(faqs.Id))
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
            return View(faqs);
        }

        // GET: FAQ/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faqs = await _context.Faqs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faqs == null)
            {
                return NotFound();
            }

            return View(faqs);
        }

        // POST: FAQ/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faqs = await _context.Faqs.FindAsync(id);
            _context.Faqs.Remove(faqs);
            await _context.SaveChangesAsync();

            // return RedirectToAction(nameof(Index));
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        private bool QaExists(int id)
        {
            return _context.Faqs.Any(e => e.Id == id);
        }
    }
}
