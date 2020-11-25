using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.exam
{
    public class Guidelines
    {
        public int guidelinecode { set; get; }
        public string guidlinename { set; get; }
    }
    public class Guidelinesnames
    {
        public List<Guidelines> Guidelinesname { set; get; }
    }    
    public class Guidelineall
    {
        public int guidelinecode { set; get; }
        public string guidlinename { set; get; }
        public string linkname { set; get; }
        public string filename { set; get; }        
    }
    public class Guidelinesdetail
    {
        public List<Guidelineall> Guidelinesdetails { set; get; }
    }
    public class Guidelinesrequest
    {
        public int studentcode { set; get; }
        public int guidelinecode { set; get; }
    }

}