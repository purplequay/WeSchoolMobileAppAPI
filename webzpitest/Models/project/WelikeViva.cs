using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.project
{
    public class WelikeViva
    {
    }

    public class welikestudent
    {
        public int studentcode { get; set; }
        public string studentname { get; set; }
        public string admissionno { get; set; }
        public string batchname { get; set; }
        public int batchcode { get; set; }
        public string cityname { get; set; }
        public int citycode { get; set; }
        public string mobileno { get; set; }
        public string email { get; set; }
      



    }
    public class welikestudentdetails
    {
        public welikestudent welikestudentdetail { set; get; }
    }


    public class welikedatevalidation
    {

        public string validbook { get; set; }
        public string msg { get; set; }
        public int candid { get; set; }
        public string student_name { get; set; }
        public string admissionno { get; set; }
        public string password { get; set; }
        public int ecode { get; set; }
        public string place { get; set; }
        public string mobileno { get; set; }
        public string email { get; set; }       
        public string type { get; set; }
        public int project_type { get; set; }
        public string paymentdone { get; set; }
        public string reschedule { get; set; }
       

        
       // public string pmsg { get; set; }
       // public string msg { get; set; }

    }
    public class welikedatevalidationdetails
    {
        public welikedatevalidation welikedatevalidationdetail { set; get; }
    }
    public class WelikevivabookingRequest
    {
        public string admissionno { get; set; }
        public int citycode { get; set; }
        public int studentcode { get; set; }

    }



}