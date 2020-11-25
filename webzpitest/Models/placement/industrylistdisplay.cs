using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace webzpitest.Models.placement
{
    public class industrylistdisplay
    {
        public int specicode { set; get; }
        public int industrycode { set; get; }
        public int refno { set; get; }
        public string advertisedate { set; get; }
        public string companyname { set; get; }
        public string designation { set; get; }
        public string location { set; get; }      
        public string lastDateForApplying { set; get; }    
    }
    public class industrylistdisplaydetail
    {
        public List<industrylistdisplay> industrylistdisplaydetails { set; get; }
    }
    public class industrylistrequest 
    {
        public int industrycode { get; set; }
        public int specicode { get; set; }
    }
}