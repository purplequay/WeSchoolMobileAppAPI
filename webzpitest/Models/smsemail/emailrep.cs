using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.smsemail
{
    public class emailrep
    {
        public string posteddate { get; set; }
        public string messagetittle { get; set; }
        public string remarks { get; set; }
        public string EmailContent { get; set; }
        //public int code { get; set; }
        //public string batchname {get; set; } 
    }
    public class emailrepdetail
    {
        public List<emailrep> emailrepdetails { set; get; }
    }
    public class emailrequest
    {
        public int studentcode { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
    }
}