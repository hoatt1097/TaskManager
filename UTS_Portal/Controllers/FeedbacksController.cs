using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using UTS_Portal.Helpers;
using UTS_Portal.Models;

namespace UTS_Portal.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly db_utsContext _context;
        public INotyfService _notyfService { get; }

        public FeedbacksController(db_utsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Feedbacks
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var ls = _context.Feedbacks.AsNoTracking().OrderByDescending(x => x.SubmittedDate);
            PagedList<Feedbacks> models = new PagedList<Feedbacks>(ls, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.Total = ls.Count();
            return View(models);
        }

        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Phone,Email,Message,SubmittedDate")] Feedbacks feedbacks)
        {
            if (ModelState.IsValid)
            {
                feedbacks.SubmittedDate = DateTime.Now;
                feedbacks.IsView = 0;
                _context.Add(feedbacks);
                await _context.SaveChangesAsync();

                _notyfService.Success("Send feedback sucessfully!");
                return RedirectToAction(nameof(Create));
            }

            ViewBag.Error = "Please complete all information";
            return View(feedbacks);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUser = UserHelper.GetCurrentUser(HttpContext);
            if (!currentUser.Permissions.Contains("ADMIN"))
            {
                return Json(new { success = false, message = "You have no permission!" });
            }

            var feedbacks = await _context.Feedbacks.FindAsync(id);
            _context.Feedbacks.Remove(feedbacks);
            await _context.SaveChangesAsync();

            // return RedirectToAction(nameof(Index));
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false });
            }

            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return Json(new { success = false });
            }

            feedback.IsView = 1;

            _context.Update(feedback);
            await _context.SaveChangesAsync();

            return Json(new { success = true, content = feedback });
        }

        private bool FeedbacksExists(int id)
        {
            return _context.Feedbacks.Any(e => e.Id == id);
        }
    }
}
