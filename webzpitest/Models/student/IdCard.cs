using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.student
{
    public class IdCard
    {
        public string IdLink { set; get; }
    }
    public class SIdCard
    {
        public IdCard  StudentIdCard{ set; get; }
    }
}