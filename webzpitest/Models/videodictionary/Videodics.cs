using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.videodictionary
{
    public class Videodics
    {
        public string chaptername { set; get; }
        public string Videoname { set; get; }
        public string description { set; get; }
        public string videolink { set; get; } 
    }
    public class videodicdetail
    {
        public List<Videodics> videodicdetails { set; get; }
    }
    public class videodicrequest
    {
        public int Specicode { get; set; }
        public int Subjectcode { get; set; }
    }
    public class videodicsub
    {
        public string SubCode { set; get; }
        public string SubjectName { set; get; }
        public string SemesterName { set; get; }
    }
    public class videodicsubdetail
    {
        public List<videodicsub> videodicsubjects { set; get; }
    }
    public class videodicsubrequest
    {
        public int SemCode { get; set; }
        public int StudentCode { get; set; }
    }
}