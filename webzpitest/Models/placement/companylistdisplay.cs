using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace webzpitest.Models.placement
{
    public class companylistdisplay
    {
        public int specicode { set; get; }
        public int refno { set; get; }
        public string advertisedate { set; get; }
        public string companyname { set; get; }
        public string designation { set; get; }
        public string location { set; get; }
        public string lastDateForApplying { set; get; }
    }
    public class companylistdisplaydetail
    {
        public List<companylistdisplay> companylistdisplaydetails { set; get; }
    }
    public class companylistrequest
    {
        public string companyname { get; set; }
        public int specicode { get; set; }
    }
}