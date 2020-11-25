using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.Results
{
    public class EndTermResult
    {     
        public int studentcode { set; get; }
        public int examyearcode { set; get; }
        public int subjectcode { set; get; }  
        public string subjectname { set; get; }
        public string semester { set; get; }
        public string midtermmarksoutof50 { set; get; }
        public string proportionatemarksoutof20 { set; get; }
        public string endtermmarksoutof50 { set; get; }
        public string proportionatemarksoutof80 { set; get; }
        public string totalmarksoutof100 { set; get; }
        public string Result { set; get; }
        

    }
    public class EndTermResultdetail
    {
        public string payreexam { get; set; }
        public string ReExamMsg { get; set; }

        public List<EndTermResult> Endtermresultsdetails { set; get; }
    }

}