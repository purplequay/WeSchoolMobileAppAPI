using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace webzpitest.Models.placement
{
    public class placementindustrywise
    {
        public int specicode { set; get; }
        public int industrycode { set; get; }
        public string industry { set; get; }
        public string totalcount { set; get; }
    }
    public class placementindustrywisedetail
    {
        public List<placementindustrywise> placementindustrywisedetails { set; get; }
    }
}