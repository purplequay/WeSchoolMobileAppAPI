using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace webzpitest.Models
{ 
    public class Token
    {            
        public string access_token { get; set; }
        public string tokenType { get; set; }
        public string ExpiresOn { get; set; }
    }
}