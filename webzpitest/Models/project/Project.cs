using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.project
{
    public class Project
    {
    }
    public class projectyear
    {
        public string Project_Welike { set; get; }
        public string FinalYear_Project { set; get; }
    }
    public class projectyeardetail
    {
        public List<projectyear> projectyeardetails { set; get; }
    }
    public class projectwelike
    {
        public string LinkName { set; get; }
        public string FileName { set; get; }
    }
    public class projectwelikedetail
    {
        public List<projectwelike> projectwelikedetails { set; get; }
    }
    public class finalyearprojlink
    {
        public string linkname { set; get; }
    }
    public class finalyearprojlinkdetails
    {
        public List<finalyearprojlink> fyprojlinknames { set; get; }
    }
    public class finalyearprojlist
    {
        public string linkname { set; get; }
        public string linkheading { set; get; }
        public string filename { get; set; }
    }

    public class finalyearprojdetails
    {
        public List<finalyearprojlist> fyprojlinkheadings { set; get; }
    }
    public class finalyearprojrequest
    {
        public int studentcode { get; set; }
        public string linkname { get; set; }
 
    }
}