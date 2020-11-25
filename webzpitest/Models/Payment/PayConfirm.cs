using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.Payment
{
    public class PayConfirm
    {
    }

  
    public class PayprocessDetail
    {
        public string deviceId { set; get; }
        public string token { set; get; }
        public string paymentMode { set; get; }
        public string merchantId { set; get; }
       // public string consumerEmailId { set; get; }
        public string txnId { set; get; }
        public string txnSubType { set; get; }

        public string returnUrl { set; get; }
        public string requestdata { set; get; }
        public string cartDescription { set; get; }
        
    }

  

    public class item
    {
        public string itemId { set; get; }
        public string amount { set; get; }
        public string comAmt { set; get; }

    }

    public class PaymentInitiationDetails
    {
        public List<item> items { set; get; }
        public PayprocessDetail PaymentInitiationDetail { set; get; }
    }

    public class PaymentInitiationRequest
    {
        public int studentcode { set; get; }     
        public string Amount { set; get; }
        public string RequestFor { set; get; }
        public string otherparam1 { set; get; }
        public string otherparam2 { set; get; }

    }

    public class PaymentConfirmDetails
    {
        public List<PayConfirmDetail> PaymentConfirmDetail { set; get; }
    }

    public class Paycomnfirmrequest
    {
        public int studentcode { set; get; }
        public string amount { set; get; }
        public string RequestFor { set; get; }
        public string otherparam1 { set; get; }
        public string otherparam2 { set; get; }
    }
    public class PayConfirmDetail
    {
        public string StudentName { set; get; }
        public string Admissionno { set; get; }
        public string BatchName { set; get; }
        public string CourseName { set; get; }
        public string Amount { set; get; }
        public string PayingFor { set; get; }
        public string RequestFor { set; get; }
        public string otherparam1 { set; get; }
        public string otherparam2 { set; get; }

    }

}