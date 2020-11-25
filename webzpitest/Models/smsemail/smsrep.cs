using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.smsemail
{
    public class smsrep
    {
        public string posteddate { get; set; }
        public string messagetittle { get; set; }
        public string remarks { get; set; }
        public string smscontent { get; set; }  
        //public int smscode { get; set; }
        //public string batchname {get; set; }        
        //public string scopeid { get; set; }  
    }
    public class smsrepdetail
    {
        public List<smsrep> smsrepdetails { set; get; }
    }
    public class smsrequest
    {
        public int studentcode { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
    }
}