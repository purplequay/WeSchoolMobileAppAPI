using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.student
{
    public class userprofile
    {
        public int studentcode { set; get; }
        public string firstname { set; get; }
        public string middletname { set; get; }
        public string lastname { set; get; }
        public string admissionno { set; get; }
        public string password { set; get; }
        public string aadharno { set; get; }
        public string correspondance { set; get; }
        public string homeaddress1 { set; get; }
        public string homeaddress2 { set; get; }
        public string homeaddress3 { set; get; }
        public string homeaddress4 { set; get; }
        public string homepincode { set; get; }
        public string hometelno { set; get; }
        public string homemobileno { set; get; }
        public string homeemail { set; get; }
        public string officeemail { set; get; }
        public string officeaddress1 { set; get; }
        public string officeaddress2 { set; get; }
        public string officeaddress3 { set; get; }
        public string officeaddress4 { set; get; }
        public string officetelno { set; get; }
        public string officemobileno { set; get; }
        public string officepincode { set; get; }
        public string fb { set; get; }
        public string twitter { set; get; }
        public string linkedin { set; get; }
        public int batchcode { set; get; }
        public int coursetypecode { set; get; }
        public int coursecode { set; get; }
        public int homecity { set; get; }
        public int officecity { set; get; }
        public int homestatecode { set; get; }
        public int homecountrycode { set; get; }
        public int officecountrycode { set; get; }
        public int officestatecode { set; get; }
        public int specicode { set; get; }
        public string studentimage { get; set; }
    }
}