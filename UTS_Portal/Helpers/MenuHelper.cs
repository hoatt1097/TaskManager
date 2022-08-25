using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTS_Portal.Models;
using UTS_Portal.ViewModels;

namespace UTS_Portal.Helpers
{
    public class MenuHelper
    {
        public static List<MenusByMonth> GetMenusByMonth(db_utsContext _context, string month) /* Format: MM/yyyy */
        {
            var DB_Menus = _context.Menus.ToList().Where(x => x.MenuDate.ToString("MM/yyyy") == month && x.Status != 0);
            List<MenusByMonth> MenusByMonth = new List<MenusByMonth>();

            MenusByMonth = DB_Menus.GroupBy(
                    x => x.MenuDate.ToString("dd/MM"),
                    x => x,
                    (key, value) => new MenusByMonth
                    {
                        DateMonth = key,
                        Noon = value.GroupBy(
                                    x => x.RepastId,
                                    x => x,
                                    (key, value) => new NoonByDay
                                    {
                                        Noon = key,
                                        Menus = value.ToList()
                                    }).ToList()
                    })
                    .ToList();
            return MenusByMonth;
        }
    }
}
