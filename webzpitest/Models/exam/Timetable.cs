using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.exam
{
    public class Timetable
    {
        public int timetablecode { set; get; }
        public string timetablename { set; get; }
    }
    public class Timetablenames
    {
        public List<Timetable> Timetablename { set; get; }
    }

    public class Timetableall
    {
        public int timetablecode { set; get; }
        public string timetablename { set; get; }
        public string linkname { set; get; }
        public string filename { set; get; }
    }
    public class Timetabledetail
    {
        public List<Timetableall> Timetabledetails { set; get; }
    }
    public class Timetablesrequest
    {
        public int studentcode { set; get; }
        public int timetablecode { set; get; }
    }

    public class PCPVCF
    {
        public int pcbvcfcode { set; get; }       
        public string linkname { set; get; }
        public string filename { set; get; }
    }
    public class PCPVCFDetails
    {
        public List<PCPVCF> PCPVCFDetail { set; get; }
    }
}