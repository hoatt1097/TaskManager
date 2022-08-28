using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            foreach(var x in parentDB)
            {
                var newP = new ParentView
                {
                    ParentId = x.ParentId,
                    Name = x.Name,
                    Email = x.Email,
                    Password = x.Password,
                    ClassName = x.ClassName,
                    CardId = x.CardId,
                    Phone = x.Phone
                };
                parentViews.Add(newP);
            }

            return parentViews.Where(x => x.Name.Contains(itemSearchText, StringComparison.OrdinalIgnoreCase)).Take(10).ToList();
        }
    }
}
