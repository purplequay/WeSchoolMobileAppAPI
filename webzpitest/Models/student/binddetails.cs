using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.student
{
    public class bindcity
    {
        public string cityname { get; set; }
        public int citycode { get; set; }       
    }
    public class citydetails
    {
        public List<bindcity> citydetail { set; get; }
    }
    public class bindstate
    {
        public string statename { get; set; }
        public int statecode { get; set; }
    }
    public class statedetails
    {
        public List<bindstate> statedetail { set; get; }
    }
    public class bindcountry
    {
        public string countryname { get; set; }
        public int countrycode { get; set; }
    }
    public class countrydetails
    {
        public List<bindcountry> countrydetail { set; get; }
    }
}