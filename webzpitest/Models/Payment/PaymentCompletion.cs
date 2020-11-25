using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.Payment
{
    public class PaymentCompletion
    {
        public string paidstatus { get; set; }
        public string transmsg { get; set; }
        public string msg { get; set; } 
        public string requestfor { get; set; }
        public int studentcode { get; set; }
        public int amount { get; set; }    
    }
    //public class PaymentCompletionRequest
    //{
    //    public string responsemsg { get; set; }
    //    public int studentcode { get; set; }
    //}
    public class Welikevivabooking
    {
   
        public string apiname { get; set; }
        public string method { get; set; }
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
    }
    public class Finalvivabooking
    {
      
        public string apiname { get; set; }
        public string method { get; set; }
    
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
    }

    public class Specialevent
    {
        public string apiname { get; set; }
        public int studentcode { get; set; }
        public string method { get; set; }

    }

    public class Couresfees
    {
        public string apiname { get; set; }
        public int studentcode { get; set; }
        public string method { get; set; }

    }
    public class ReExams
    {
        public string apiname { get; set; }
        public int studentcode { get; set; }
        public string method { get; set; }

    }
    public class PaymentCompletiondetails
    {
        public PaymentCompletion PaymentCompletionDetail { get; set; }
        public ReExams ReExamDetail { get; set; }
        public Couresfees CouresfeesDetail { get; set; }
        public Specialevent SpecialeventDetail { get; set; }
        public Welikevivabooking WelikevivabookingDetail { get; set; }
        public Finalvivabooking FinalvivabookingDetail { get; set; }

    }
 

}