using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.welectures
{
    public class welectures
    {
    }
    public class welecturessubjects
    {
        public string ID { set; get; }
        public string subcode { set; get; }       
        public string subjectname { set; get; }
        public string semestername { set; get; }
    }
    public class welecturessubdetail
    {
        public List<welecturessubjects> welecturessubdet { set; get; }
    }
    public class welecturessubrequest
    {
        public int semcode { get; set; }
        public int studentcode { get; set; }
    }
}