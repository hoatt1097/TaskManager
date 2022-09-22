using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTS_Portal.ViewModels
{
    public class SaleMeal
    {
        public String Campus { get; set; }
        public String Meal { get; set; }
        public String Date { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
