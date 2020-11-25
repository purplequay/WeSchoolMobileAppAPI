using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace webzpitest.Models.placement
{
    public class placementcompanywise
    {
        public int specicode { set; get; }
        public string company { set; get; }
        public string totalcount { set; get; }      
    }
    public class placementcompanywisedetail
    {
        public List<placementcompanywise> placementcompanywisedetails { set; get; }
    }
}