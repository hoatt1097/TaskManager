using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTS_Portal.ViewModels
{
    public class HolidayCreateModel
    {
        [StringLength(1000)]
        public String Day { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }
}
