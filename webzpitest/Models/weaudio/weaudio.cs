using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.weaudio
{
    public class weaudio
    {
    }
    public class weaudiosubjects
    {
        public string subcode { set; get; }
        public string subjectname { set; get; }
        public string semestername { set; get; }
    }
    public class weaudiosubdetail
    {
        public List<weaudiosubjects> weaudiosubdet { set; get; }
    }
    public class weaudiosubrequest
    {
        public int semcode { get; set; }
        public int studentcode { get; set; }
    }
}