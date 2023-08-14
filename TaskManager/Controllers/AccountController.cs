using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using TaskManager.Context;
using TaskManager.Extension;
using TaskManager.Helpers;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly TaskManagerContext _context;

        public AccountController(TaskManagerContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            var Id = HttpContext.Session.GetString("UserId");
            if (Id != null) return RedirectToAction("Index", "Home");

            LoginViewModel loginViewModel = new LoginViewModel { };
            return View(loginViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Users users = _context.Users
                       .Include(p => p.Role)
                       .Where(p => p.Active == true)
                       .SingleOrDefault(p => p.Username.ToLower() == model.UserName.ToLower().Trim());

                    if (users == null)
                    {
                        ViewBag.Error = "Account is not correct. Please try again";
                        return View(model);
                    }

                    //string pass = Utilities.MD5Hash(model.Password.Trim());
                    string pass = model.Password.Trim();
                    if (users.Password != pass)
                    {
                        ViewBag.Error = "Account is not correct. Please try again";
                        return View(model);
                    }

                    users.LastLogin = DateTime.Now;
                    _context.Update(users);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.SetString("UserId", users.Id.ToString().Trim());
                    HttpContext.Session.SetString("Username", users.Username.ToString().Trim());
                    HttpContext.Session.SetString("DisplayName", users.DisplayName);
                    HttpContext.Session.SetString("LastLogin", users.LastLogin.ToString());
                    HttpContext.Session.SetString("ChannelId", users.ChannelId.ToString());
                    HttpContext.Session.SetString("ChannelName", users.ChannelName);


                    //identity
                    var userClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, users.Username?.Trim()),
                            //new Claim(ClaimTypes.Email, users.Email?.Trim()),
                            new Claim("UserId", users.Id.ToString().Trim()),
                            new Claim("Username", users.Username.ToString().Trim()),
                            new Claim("DisplayName", users.DisplayName.ToString().Trim()),
                            new Claim("LastLogin", users.LastLogin.ToString().Trim()),
                            new Claim("ChannelId", users.ChannelId.ToString().Trim()),
                            new Claim("ChannelName", users.ChannelName.Trim()),
                            new Claim("RoleName", users.Role.Name.Trim()),
                            new Claim(ClaimTypes.Role, users.Role.Code.Trim())
                        };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("UserId");
                return RedirectToAction("Login", "Account");
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
