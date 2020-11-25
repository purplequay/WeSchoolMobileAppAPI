using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.notice
{
    public class noticeall
    {
        public string linkheading { get; set; }
        public string noticename { get; set; }      
        public string linkname { get; set; }
        public string filename { get; set; }
    }
    public class noticealldetail
    {
        public List<noticeall> noticeall { set; get; }
    }
    public class noticeallrequest
    {
        public int studentcode { get; set; }
        public string linkheading { get; set; }
        public string noticename { get; set; }
    }
    public class notices
    {
        public string linkheading { get; set; }
    }
    public class noticedetail
    {
        public List<notices> noticeheading { set; get; }
    }
    public class noticenames
    {
        public string linkheading { get; set; }
        public string noticename { get; set; }
    }
    public class noticenamesedetail
    {
        public List<noticenames> noticenamedet { set; get; }
    }
    public class noticenamesrequest
    {
        public int studentcode { get; set; }
        public string linkheading { get; set; }
    }
}