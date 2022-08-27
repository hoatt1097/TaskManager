﻿using System;
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
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID != null) return RedirectToAction("Index", "Home");

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
                    Users kh = _context.Users
                    .Include(p => p.Role)
                    .Where(p => p.Active == true)
                    .SingleOrDefault(p => p.Username.ToLower() == model.UserName.ToLower().Trim());

                    if (kh == null)
                    {
                        ViewBag.Error = "Account is not correct. Please try again";
                        return View(model);
                    }
                    string pass = Utilities.MD5Hash(model.Password.Trim());
                    if (kh.Password!= pass)
                    {
                        ViewBag.Error = "Account is not correct. Please try again";
                        return View(model);
                    }

                    kh.LastLogin = DateTime.Now;
                    _context.Update(kh);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.SetString("UserID", kh.Id.ToString());
                    HttpContext.Session.SetString("UserCode", kh.Code.ToString());
                    HttpContext.Session.SetString("Fullname", kh.Fullname);
                    HttpContext.Session.SetString("Username", kh.Username);

                    //identity
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, kh.Username),
                        new Claim(ClaimTypes.Email, kh.Email),
                        new Claim("Id", kh.Id.ToString()),
                        new Claim("Username", kh.Username.ToString()),
                        new Claim("Code", kh.Code.ToString()),
                        new Claim("Fullname", kh.Fullname.ToString()),
                        new Claim("Email", kh.Email.ToString()),
                        new Claim("RoleId", kh.RoleId.ToString()),
                        new Claim("RoleName", kh.Role.Name),
                        new Claim(ClaimTypes.Role, kh.Role.Name)
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
