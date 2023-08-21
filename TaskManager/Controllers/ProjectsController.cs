using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Context;
using TaskManager.Extension;
using TaskManager.Helpers;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly TaskManagerContext _context;
        public INotyfService _notyfService { get; }
        public ProjectsController(TaskManagerContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public async Task<IActionResult> Index(int? page, int channelID = 0)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 30;

            var ls = _context.Projects.AsNoTracking().OrderBy(x => x.Id);

            if (channelID != 0)
            {
                ls = _context.Projects
                .AsNoTracking()
                .Where(x => x.ChannelId == channelID)
                .OrderBy(x => x.Id);
            }

            var models = new PagedList<Projects>(ls, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.Total = ls.Count();
            ViewData["Channel"] = new SelectList(_context.Channels, "Id", "Name", channelID);

            return View(models);
        }

        public IActionResult Filter(int channelID = 0)
        {
            var url = $"/Projects/Index?channelID={channelID}";
            if (channelID == 0)
            {
                url = $"/Projects/Index";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            var progressValues = new List<int>();
            for (int i = 10; i <= 100; i += 10)
            {
                progressValues.Add(i);
            }
            ViewBag.ProgressValues = new SelectList(progressValues);

            var statusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "OPEN", Text = "Open" },
                new SelectListItem { Value = "ON DISCUSSING", Text = "On discussing" },
                new SelectListItem { Value = "FUTURE", Text = "Future" },
                new SelectListItem { Value = "CLOSED", Text = "Close" },
            };
            ViewBag.StatusOptions = new SelectList(statusOptions, "Value", "Text");

            ViewData["ChannelId"] = new SelectList(_context.Channels, "Id", "Name");
            ViewData["PIC"] = new SelectList(_context.Users.Where(x => x.Active == true), "DisplayName", "DisplayName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,Desciption,Active,ChannelId,NextAction,PIC,Progress")] Projects projects)
        {
            try
            {
                var currentUser = UserHelper.GetCurrentUser(HttpContext);
                if (ModelState.IsValid)
                {
                    projects.CreationDate = DateTime.Now;
                    projects.CreatedBy = int.Parse(currentUser.UserId);
                    _context.Add(projects);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Add project sucessfully!");
                    return RedirectToAction(nameof(Index));
                }
                ViewData["ChannelId"] = new SelectList(_context.Channels, "Id", "Name", projects.ChannelId);
                _notyfService.Error("Add project unsucessfully!");
                return View(projects);
            }
            catch
            {
                return View();
            }
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects.FindAsync(id);
            if (projects == null)
            {
                return NotFound();
            }

            var progressValues = new List<int>();
            for (int i = 10; i <= 100; i += 10)
            {
                progressValues.Add(i);
            }
            ViewBag.ProgressValues = new SelectList(progressValues);

            var statusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "OPEN", Text = "Open" },
                new SelectListItem { Value = "ON DISCUSSING", Text = "On discussing" },
                new SelectListItem { Value = "FUTURE", Text = "Future" },
                new SelectListItem { Value = "CLOSED", Text = "Close" },
            };
            ViewBag.StatusOptions = new SelectList(statusOptions, "Value", "Text");

            ViewData["ChannelId"] = new SelectList(_context.Channels, "Id", "Name", projects.ChannelId);
            ViewData["PIC"] = new SelectList(_context.Users.Where(x => x.Active == true), "DisplayName", "DisplayName", projects.ChannelId);
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Description,Active,ChannelId,NextAction,PIC,Progress,CreationDate,CreatedBy")] Projects projects)
        {
            if (id != projects.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = UserHelper.GetCurrentUser(HttpContext);
                    projects.LastUpdateDate = DateTime.Now;
                    projects.LastUpdatedBy = int.Parse(currentUser.UserId);
                    _context.Update(projects);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountsExists(projects.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notyfService.Success("Edit project sucessfully!");
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChannelId"] = new SelectList(_context.Channels, "Id", "Name", projects.ChannelId);
            _notyfService.Error("Edit project unsucessfully!");
            return View(projects);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projects == null)
            {
                return NotFound();
            }

            return View(projects);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projects = await _context.Projects.FindAsync(id);

            var task = _context.Tasks.Where(x => x.ProjectId == projects.Id).ToList();
            if (task.Count > 0)
            {
                return Json(new { success = false, message = "Can not Delete. Project has task" });
            }
            else
            {
                _context.Projects.Remove(projects);
                await _context.SaveChangesAsync();

                // return RedirectToAction(nameof(Index));
                return Json(new { success = true, message = "Deleted Successfully" });
            }
            
        }

        private bool AccountsExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

    }
}
