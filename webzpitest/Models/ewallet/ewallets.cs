using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace webzpitest.Models.ewallet
{
    public class ewallets
    {
        public int studentcode { set; get; }
        public string ewalletdescription { set; get; }
        public string ewalletlink { set; get; }  
    }
    public class ewalletsdetail
    {
        public List<ewallets> ewalletdetail { set; get; }
    }
}