using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.student
{
    public class LoginUserDetails
    {
        public LoginUser LoginUser { set; get; }
        public Token TokenDetails { set; get; }
        public string Errormsg { get; set; }   
    }


}