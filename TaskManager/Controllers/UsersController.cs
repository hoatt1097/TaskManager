using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Context;
using TaskManager.Extension;
using TaskManager.Helpers;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly TaskManagerContext _context;
        public INotyfService _notyfService { get; }
        public UsersController(TaskManagerContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public async Task<IActionResult> Index(int? page, int roleID = 0)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;

            var ls = _context.Users.Include(a => a.Role).AsNoTracking().OrderBy(x => x.Id);

            if (roleID != 0)
            {
                ls = _context.Users
                .AsNoTracking()
                .Where(x => x.RoleId == roleID)
                .Include(x => x.Role)
                .OrderBy(x => x.Id);
            }

            var models = new PagedList<Users>(ls, pageNumber, pageSize);

            ViewBag.CurrentRole = roleID;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.Total = ls.Count();
            ViewData["Role"] = new SelectList(_context.Roles, "Id", "Name", roleID);

            return View(models);
        }

        public IActionResult Filtter(int roleID = 0)
        {
            var url = $"/Users/Index?roleID={roleID}";
            if (roleID == 0)
            {
                url = $"/Users/Index";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            ViewData["ChannelId"] = new SelectList(_context.Channels, "Id", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fullname,Email,Phone,Username,Password,DisplayName,Active,CreationDate,RoleId,ChannelId,LastLogin")] Users accounts)
        {
            try
            {
                var currentUser = UserHelper.GetCurrentUser(HttpContext);
                if (ModelState.IsValid)
                {
                    accounts.CreationDate = DateTime.Now;
                    accounts.Password = Utilities.MD5Hash(accounts.Password);
                    accounts.CreatedBy = int.Parse(currentUser.UserId);
                    var channel = _context.Channels.FirstOrDefault(e => e.Id == accounts.ChannelId);
                    if (channel != null)
                    {
                        accounts.ChannelName = channel.Name;
                    }
                    _context.Add(accounts);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Add user sucessfully!");
                    return RedirectToAction(nameof(Index));
                }
                ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", accounts.RoleId);
                _notyfService.Error("Add user unsucessfully!");
                return View(accounts);
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Users.FindAsync(id);
            if (accounts == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", accounts.RoleId);
            ViewData["ChannelId"] = new SelectList(_context.Channels, "Id", "Name", accounts.ChannelId);
            return View(accounts);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fullname,Email,Password,Phone,Username,DisplayName,Active,CreationDate,RoleId,ChannelId,LastLogin")] Users accounts, string PasswordNew)
        {
            if (id != accounts.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = UserHelper.GetCurrentUser(HttpContext);
                    if (!string.IsNullOrEmpty(PasswordNew))
                    {
                        /*accounts.Password = Utilities.MD5Hash(PasswordNew);*/
                        accounts.Password = PasswordNew;
                    }
                    var channel = _context.Channels.FirstOrDefault(e => e.Id == accounts.ChannelId);
                    if(channel != null)
                    {
                        accounts.ChannelName = channel.Name;
                    }
                    accounts.LastUpdateDate = DateTime.Now;
                    accounts.LastUpdatedBy = int.Parse(currentUser.UserId);
                    _context.Update(accounts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountsExists(accounts.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notyfService.Success("Edit user sucessfully!");
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", accounts.RoleId);
            _notyfService.Error("Edit user unsucessfully!");
            return View(accounts);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Users
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accounts == null)
            {
                return NotFound();
            }

            return View(accounts);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accounts = await _context.Users.FindAsync(id);
            _context.Users.Remove(accounts);
            await _context.SaveChangesAsync();

            // return RedirectToAction(nameof(Index));
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        private bool AccountsExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
