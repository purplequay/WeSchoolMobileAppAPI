using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.student
{
    public class LoginUserRequest
    {   
        public string coursetype { get; set; }
        public string batchname { get; set; }
        public string Rollno { get; set; }
        public string Password { get; set; }        
    }
}