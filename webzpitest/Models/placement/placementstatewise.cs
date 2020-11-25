using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace webzpitest.Models.placement
{
    public class placementstatewise
    {
        public int specicode { set; get; }
        public int statecode { set; get; }
        public string state { set; get; }
        public string totalcount { set; get; }
    }
    public class placementstatewisedetail
    {
        public List<placementstatewise> placementstatewisedetails { set; get; }
    }
}