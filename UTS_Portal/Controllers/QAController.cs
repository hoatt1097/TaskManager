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
    public class QAController : Controller
    {
        private readonly db_utsContext _context;

        public QAController(db_utsContext context)
        {
            _context = context;
        }

        // GET: QA
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var ls = _context.Qa.AsNoTracking().OrderBy(x => x.OrderNo);
            PagedList<Qa> models = new PagedList<Qa>(ls, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.Total = ls.Count();
            return View(models);
        }

        // GET: QA/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qa = await _context.Qa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qa == null)
            {
                return NotFound();
            }

            return View(qa);
        }

        // GET: QA/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QA/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderNo,Title,Contents")] Qa qa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(qa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(qa);
        }

        // GET: QA/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qa = await _context.Qa.FindAsync(id);
            if (qa == null)
            {
                return NotFound();
            }
            return View(qa);
        }

        // POST: QA/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderNo,Title,Contents")] Qa qa)
        {
            if (id != qa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QaExists(qa.Id))
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
            return View(qa);
        }

        // GET: QA/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qa = await _context.Qa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qa == null)
            {
                return NotFound();
            }

            return View(qa);
        }

        // POST: QA/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qa = await _context.Qa.FindAsync(id);
            _context.Qa.Remove(qa);
            await _context.SaveChangesAsync();

            // return RedirectToAction(nameof(Index));
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        private bool QaExists(int id)
        {
            return _context.Qa.Any(e => e.Id == id);
        }
    }
}
