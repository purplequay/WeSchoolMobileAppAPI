using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace webzpitest.Models.elearning
{
    public class elearningtoolkit
    {
        public string code { get; set; }
        public string chaptername { get; set; }
        public string summary { get; set; }
        public string ppt { get; set; }
        public string mcq { get; set; }

        // public string welearn { get; set; }
        public string subjectcode { get; set; }
    }
    public class elearningtoolkitdetail
    {
        public List<elearningtoolkit> elearningtoolkitdet { set; get; }
    }
    public class elearningtoolkitrequest
    {
        public int StudentCode { get; set; }
        public int SubCode { get; set; }
    }
    public class elearningbooks
    {
        public string SubCode { set; get; }
        public string SubjectName { set; get; }
        public string SemesterName { set; get; }
    }
    public class elearningbooksdetail
    {
        public List<elearningbooks> elearningbooksdet { set; get; }
    }
    public class elearningbooksrequest
    {
        public int SemCode { get; set; }
        public int StudentCode { get; set; }
    }
}