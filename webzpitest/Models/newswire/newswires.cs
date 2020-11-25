using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.newswire
{
    public class newswires
    {
        public int Code { get; set; }
        public string Msghead { get; set; }
        public string DateofRef { get; set; }
        public string Newswirelink { get; set; }
    }
    public class newswirestdetail
    {
        public List<newswires> newswiredetail { set; get; }
    }
    public class newswirerequest
    {
        public int SpeciCode { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
    }   
    public class newswirecatagory
    {
        public int Code { get; set; }
        public string SpecName { get; set; }
    }
    public class newswirecatagoryrequest
    {
        public int SpeciCode { get; set; }      
    }
    public class newswirespecialization
    {
        public List<newswirecatagory> newswiresspecialization { set; get; }
    }
}