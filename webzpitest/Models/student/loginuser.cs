using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.student
{
    public class LoginUser
    {
        public int studentcode { set; get; }
        public int specicode { set; get; }
        public string firstname { set; get; }
        public string middletname { set; get; } 
        public string lastname { set; get; }
        public string admissionno { set; get; }
        public int coursecode { set; get; }
        public int batchcode { set; get; }
        public int coursetypecode { set; get; }
        public int msg { set; get; }
        public string coursecompletionstatus { set; get; }
        public string povcompletestatus { set; get; }
      //  public string blockstatus { set; get; }
      //  public string blockmessage { set; get; }
    }
}