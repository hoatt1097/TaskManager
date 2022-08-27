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
using UTS_Portal.Extension;
using UTS_Portal.Models;

namespace UTS_Portal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly db_utsContext _context;

        public AccountController(db_utsContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            var taiuseroanID = HttpContext.Session.GetString("AccountId");
            if (taiuseroanID != null) return RedirectToAction("Index", "Home");

            LoginViewModel loginViewModel = new LoginViewModel { AccountType = "Parent" };
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
                    Users user = _context.Users
                    .Include(p => p.Role)
                    .Where(p => p.Active == true)
                    .SingleOrDefault(p => p.Username.ToLower() == model.UserName.ToLower().Trim());

                    if (user == null)
                    {
                        ViewBag.Error = "Account is not correct. Please try again";
                        return View(model);
                    }
                    string pass = Utilities.MD5Hash(model.Password.Trim());
                    if (user.Password!= pass)
                    {
                        ViewBag.Error = "Account is not correct. Please try again";
                        return View(model);
                    }

                    user.LastLogin = DateTime.Now;
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.SetString("UserID", user.Id.ToString());
                    HttpContext.Session.SetString("UserCode", user.Code.ToString());
                    HttpContext.Session.SetString("Fullname", user.Fullname);
                    HttpContext.Session.SetString("Username", user.Username);

                    //identity
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Username", user.Username.ToString()),
                        new Claim("Code", user.Code.ToString()),
                        new Claim("Fullname", user.Fullname.ToString()),
                        new Claim("Email", user.Email.ToString()),
                        new Claim("RoleId", user.RoleId.ToString()),
                        new Claim("RoleName", user.Role.Name),
                        new Claim(ClaimTypes.Role, user.Role.Name)
                    };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);

                    if(user.Role.Name != "Parent")
                    {
                        var NotiCount = _context.Feedbacks.AsNoTracking().Where(x => x.IsView != 1).OrderByDescending(x => x.SubmittedDate).Count();
                        HttpContext.Session.SetString("NotiCount", NotiCount.ToString());
                    } 

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
                HttpContext.Session.Remove("AccountId");
                return RedirectToAction("Login", "Account");
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
