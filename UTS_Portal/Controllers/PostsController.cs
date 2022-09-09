using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using UTS_Portal.Extension;
using UTS_Portal.Models;

namespace UTS_Portal.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly db_utsContext _context;

        public INotyfService _notyfService { get; }

        public PostsController(db_utsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? page)
        {
            var collection = _context.Posts.AsNoTracking().ToList();
            foreach (var item in collection)
            {
                if (item.CreatedDate == null)
                {
                    item.CreatedDate = DateTime.Now;
                    _context.Update(item);
                    _context.SaveChanges();
                }
            }

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var ls = _context.Posts.AsNoTracking().OrderByDescending(x => x.CreatedDate);
            PagedList<Posts> models = new PagedList<Posts>(ls, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.Total = ls.Count();
            return View(models);
        }

        public async Task<IActionResult> List(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 7;
            var ls = _context.Posts.AsNoTracking().Where(x => x.Published == true).OrderByDescending(x => x.CreatedDate);
            PagedList<Posts> models = new PagedList<Posts>(ls, pageNumber, pageSize);

            var listPopular = _context.Posts.AsNoTracking().Where(x => x.Published == true).OrderByDescending(x => x.Views).Take(5).ToList();

            ViewBag.CurrentPage = pageNumber;
            ViewBag.Total = ls.Count();

            ViewBag.ListPopular = listPopular;
            return View(models);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts.FirstOrDefaultAsync(m => m.Alias == id);
            if (posts == null)
            {
                return NotFound();
            }

            return View(posts);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            Posts post = new Posts { Author = HttpContext.Session.GetString("Fullname") };
            return View(post);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Scontents,Contents,Thumb,Published,CreatedDate,Author,Views,IsHot")] Posts posts, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                //Handle Thumb
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Utilities.SEOUrl(posts.Title) + extension;
                    posts.Thumb = await Utilities.UploadFile(fThumb, @"news", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(posts.Thumb)) posts.Thumb = "default.jpg";
                posts.Alias = Utilities.SEOUrl(posts.Title);
                posts.CreatedDate = DateTime.Now;

                _context.Add(posts);
                await _context.SaveChangesAsync();
                _notyfService.Success("Add new post sucessfully!");
                return RedirectToAction(nameof(Index));
            }
            return View(posts);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts.FindAsync(id);
            if (posts == null)
            {
                return NotFound();
            }
            return View(posts);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Scontents,Contents,Thumb,Published,CreatedDate,Author,Views,IsHot")] Posts posts, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != posts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Handle Thumb
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string imageName = Utilities.SEOUrl(posts.Title) + extension;
                        posts.Thumb = await Utilities.UploadFile(fThumb, @"news", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(posts.Thumb)) posts.Thumb = "default.jpg";
                    posts.Alias = Utilities.SEOUrl(posts.Title);

                    _context.Update(posts);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Update successfully");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostsExists(posts.Id))
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
            return View(posts);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posts == null)
            {
                return NotFound();
            }

            return View(posts);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var posts = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(posts);
            await _context.SaveChangesAsync();

            // return RedirectToAction(nameof(Index));
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        private bool PostsExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
