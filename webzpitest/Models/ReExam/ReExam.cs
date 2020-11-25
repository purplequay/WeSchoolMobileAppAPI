using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.ReExam
{
    public class ReExam
    {


    }

    public class ReExamdetails
    {
        public string CanPay { set; get; }
        public string Downloadbtn { set; get; }
        public string Message { set; get; }
        public string Downloadlink { set; get; }
        public List<ReExamSubjects> ReExamSubjectsdetail { set; get; }
    }

    public class ReExamSubjects
    {
        public int studentcode { set; get; }
        public string Admissionno { set; get; }
        public int examyearcode { set; get; }
        public int subjectcode { set; get; }
        public string subjectname { set; get; }
        public string totalmarks { set; get; }
        public int fees { set; get; }


    }

    public class ReExampaymode
    {
        public int paymentcode { get; set; }
        public string paymentmode { get; set; }
    }
    public class ReExampaymodedetails
    {
        public List<ReExampaymode> paymentmodedetail { set; get; }
    }


    public class ReExamFeesPaymentDetailRequest
    {
        public int studentcode { set; get; }
        public string subjectcodes { set; get; }
        public string reexamtotalfees { set; get; }
        public int examyearcode { set; get; }

    }

    public class ReExamFeesPayment
    {
        public int studentcode { set; get; }
        public string studentname { set; get; }
        public string admissionno { set; get; }
        public string subjectcodes { set; get; }
        public string reexamtotalfees { set; get; }
        public int examyearcode { set; get; }
        public string confirmmsg { set; get; }
        public string RequestFor { get; set; }
        public string otherparam1 { get; set; }
        public string otherparam2 { get; set; }
        public int amount { set; get; }

    }

    public class ReExamFeesPaymentdetails
    {
        public List<ReExamFeesPayment> ReExamFeesPaymentdetail { set; get; }
    }


    public class ReExamDDPaymentRequest
    {
        public int studentcode { set; get; }
        public string subjectcodes { set; get; }
        public string reexamtotalfees { set; get; }
        public int examyearcode { set; get; }
        public int PaymentModeCode { set; get; }      
        public string ddno { set; get; }
        public string ddbank { set; get; }
        public string dddate { set; get; }

    }
    public class ReExamDDPayment
    {
        public string msg { get; set; }

        public string downloadformlink { get; set; }
    }
    public class ReExamDDPaymentDetail
    {
        public List<ReExamDDPayment> ReExamDDPaymentDetails { set; get; }
    }

}