using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.FeesPayment
{
    public class FeesPayment
    {
      
    }

    public class Fees2ndyrPayment
    {

        public string FelStudent { get; set; }
        public string canpay { get; set; }
        public string downloadbtn { get; set; }
        public string attentionmsg { get; set; }
        public string downloadformlink { get; set; }
        public int franciseeid { get; set; }
        public int feeplancode { get; set; }
        public string studentname { get; set; }
        public string admissionno { get; set; }
        //  public string batchname { get; set; }
        // public int batchcode { get; set; }
        //  public string coursename { get; set; }
        // public int coursecode { get; set; }
        public int amount { get; set; }
        public int partamount1 { get; set; }
        public int partamount2 { get; set; }

        public string partdddate1 { get; set; }
        public string partdddate2 { get; set; }

        public string amountdescription { get; set; }
        // public string payingfor { get; set; }
        public string RequestFor { get; set; }
        public string otherparam1 { get; set; }
        public string otherparam2 { get; set; }
     
     

    }

    public class FeesPaymentdetails
    {
      
        public List<Fees2ndyrPayment> FeesPaymentdetail { set; get; }
        //public FelFees2ndyrPayment FELFeesPaymentdetail { set; get; }

    }

    //public class FelFees2ndyrPayment
    //{

        
    //    public int franciseeid { get; set; }
    //    public int feeplancode { get; set; }
    //    public string studentname { get; set; }
    //    public string admissionno { get; set; }
    //    //  public string batchname { get; set; }
    //    // public int batchcode { get; set; }
    //    //  public string coursename { get; set; }
    //    // public int coursecode { get; set; }
    //    public int amount { get; set; }
    //    public int PartAmount1 { get; set; }
    //    public int PartAmount2 { get; set; }

    //    public string PartDDDate1 { get; set; }
    //    public string PartDDDate2 { get; set; }

    //    public string amountdescription { get; set; }
    //    // public string payingfor { get; set; }
    //    public string RequestFor { get; set; }
    //    public string otherparam1 { get; set; }
    //    public string otherparam2 { get; set; }
      

    //}
    public class paymode
    {
        public int paymentcode { get; set; }
        public string paymentmode { get; set; }
    }
    public class paymentmodedetails
    {
        public List<paymode> paymentmodedetail { set; get; }
    }
    public class ChequeDDPayment
    {      
        public string attentionmsg { get; set; }
        public string downloadformlink { get; set; }    
    }
    public class ChequeDDPaymentDetail
    {
        public ChequeDDPayment fees2ndyrDDPayment { set; get; }
    }

    public class ChequeDDPaymentrequest
    {
        public int studentcode { set; get; }
        public string admissionno { set; get; }
        public int paymentmodecode { set; get; }
        public string ddno { set; get; }
        public string ddbank { set; get; }
        public DateTime dddate { set; get; }
        public int amount { set; get; }

    }


    public class FelChequeDDPaymentrequest
    {
        public int studentcode { set; get; }
        public string admissionno { set; get; }
        public int paymentmodecode { set; get; }
        public int paymentmodecode1 { set; get; }
        public int paymentmodecode2 { set; get; }
        public int amount { set; get; }
        public string ddno { set; get; }
        public string ddbank { set; get; }
        public DateTime dddate { set; get; }   
        public int partamount1 { get; set; }
        public string partddno1 { set; get; }
        public string partddbank1 { set; get; }
        public string partdddate1 { get; set; }
        public int partamount2 { get; set; }
        public string partddno2 { set; get; }
        public string partddbank2 { set; get; }
        public string partdddate2 { get; set; }
        public int feeplancode { get; set; }

    }

    public class FelChequeDDPayment
    {
        public string attentionmsg { get; set; }
        public string downloadformlink { get; set; }
    }
    public class FelChequeDDPaymentDetail
    {
        public FelChequeDDPayment Felfees2ndyrDDPayment { set; get; }
    }

}