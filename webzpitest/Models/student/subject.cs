using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.student
{
    public class subject
    {
        public int subjectcode { set; get; }
        public string subjectname { set; get; }
    }
    public class subjectDetail
    {
        public List<subject> subjectdetails { set; get; }
    }
}