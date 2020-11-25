using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.Results
{
    public class Results
    {
        public string midterm { set; get; }
        public string endterm { set; get; }
        public int studentcode { set; get; }
        public string admissionno { set; get; }
        public string batchname { set; get; }
        public string linkname { set; get; }
        public int examyearcode { set; get; }
        public int resultview { set; get; }
    }
    public class Resultsdetail
    {
        public List<Results> Resultsdetails { set; get; }
    }
}