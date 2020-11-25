using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.student
{
    public class Holiday
    {
        public string HolidayLink { set; get; }
    }  
    public class HolidayList
    {
        public Holiday Holidays { set; get; }
    }
}