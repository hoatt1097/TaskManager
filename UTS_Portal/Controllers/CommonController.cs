using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UTS_Portal.Models;
using UTS_Portal.ViewModels;

namespace UTS_Portal.Controllers
{
    public class CommonController : Controller
    {
        private readonly db_utsContext _context;

        public CommonController(db_utsContext context)
        {
            _context = context;
        }

        public List<ParentView> GetAllParent(string itemSearchText)
        {
            var parentDB = _context.Cscard.AsNoTracking().ToList();
            List<ParentView> parentViews = new List<ParentView>();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            foreach (var x in parentDB)
            {
                var newP = new ParentView
                {
                    ParentId = x.ParentId.Trim(),
                    Name = x.Name.Trim(),
                    Email = x.Email.Trim(),
                    Password = x.Password.Trim(),
                    ClassName = x.ClassName.Trim(),
                    CardId = x.CardId.Trim(),
                    Phone = x.Phone.Trim(),
                    Bal_Amount = x.BalAmount.ToString("#,###", cul.NumberFormat)
            };
                parentViews.Add(newP);
            }

            return parentViews.Where(x => x.Name.Contains(itemSearchText, StringComparison.OrdinalIgnoreCase) || x.ParentId.Contains(itemSearchText, StringComparison.OrdinalIgnoreCase))
                .Take(10).ToList();
        }
    }
}
