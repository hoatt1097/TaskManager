using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTS_Portal.ViewModels
{
    public class OrderSubmit
    {
        public int Day { get; set; }
        public string CurrentMonth { get; set; }
        public List<OrderItem> Breakfast { get; set; }
        public List<OrderItem> Lunch { get; set; }
        public List<OrderItem> Afternoon { get; set; }
    }

    public class OrderItem
    {
        public string Code { get; set; }
        public string Ckcode { get; set; }
        public string OriginName { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public int Qty { get; set; }
        public int Bundled { get; set; }
        public int RepastId { get; set; }
    }
}
