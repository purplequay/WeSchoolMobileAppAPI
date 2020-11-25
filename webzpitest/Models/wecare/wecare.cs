using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.wecare
{
    public class wecare
    {
       
    }

   

    public class wecarepassparameter
    {
        public int studentcode { get; set; }
        public string admissionno { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string specialization { get; set; }
    }


    public class wecaredetail
    {
        public List<wecarepassparameter> Wecaredet { get; set; }
    }

}