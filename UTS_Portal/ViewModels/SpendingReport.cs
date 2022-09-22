using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTS_Portal.ViewModels
{
    public class SpendingReport
    {
        public String Date { get; set; }
        public String Time { get; set; }
        public String ReceiptNumber { get; set; }
        public String Type { get; set; }
        public String Quantity { get; set; }
        public String UnitPrice { get; set; }
        public String Amount { get; set; }
        public String CkCode { get; set; }
        public String ItemNameVN { get; set; }
        public String ItemNameEN { get; set; }
        public String Meal { get; set; }
        public String Remark { get; set; }
    }
}
