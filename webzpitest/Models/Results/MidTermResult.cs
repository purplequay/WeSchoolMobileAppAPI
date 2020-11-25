using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.Results
{
    public class MidTermResult
    {
        public int studentcode { set; get; }
        public int examyearcode { set; get; }
        public int subjectcode { set; get; }
        public string subjectname { set; get; }
        public string Assignmarksoutof50 { set; get; }
        public string Assignweightedoutof20 { set; get; }
    }
    public class MidTermResultdetail
    {
        public List<MidTermResult> MidTermResultdetails { set; get; }
    }
    public class MidTermResultrequest
    {
        public int studentcode { set; get; }
        public int examyearcode { set; get; }
    }
}