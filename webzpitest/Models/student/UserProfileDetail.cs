using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.student
{
    public class UserProfileDetail
    {
        public userprofile userprofile { set; get; }
    }
    
    public class EditProfileRequest
    {
        public int studentcode { get; set; }
        public string correspondance { get; set; }
        public string homeaddress1 { get; set; }
        public string homeaddress2 { get; set; }
        public string homeaddress3 { get; set; }
        public string homeaddress4 { get; set; }
        public string homepincode { get; set; }
        public int homecity { get; set; }
        public int homestatecode { get; set; }
        public int homecountrycode { get; set; }
        public string hometelno { get; set; }
        public string homemobileno { get; set; }
        public string homeemail { get; set; }
        public string officeaddress1 { get; set; }
        public string officeaddress2 { get; set; }
        public string officeaddress3 { get; set; }
        public string officeaddress4 { get; set; }
        public int officecity { get; set; }
        public string officepincode { get; set; }
        public int officestatecode { get; set; }
        public int officecountrycode { get; set; }
        public string officetelno { get; set; }
        public string officemobileno { get; set; }
        public string officeemail { get; set; }      
        public string fb { get; set; }
        public string linkedin { get; set; }
        public string twitter { get; set; }
    }
}