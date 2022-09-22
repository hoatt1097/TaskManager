using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTS_Portal.ViewModels
{
    public class SaleDetail
    {
        public String Campus { get; set; }
        public String Date { get; set; }
        public String Time { get; set; }
        public String ReceiptNumber { get; set; }
        public String Type { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public String GrpName { get; set; }
        public String CkCode { get; set; }
        public String ItemNameVN { get; set; }
        public String ItemNameEN { get; set; }
        public String Meal { get; set; }
        public String Remark { get; set; }
    }
}
