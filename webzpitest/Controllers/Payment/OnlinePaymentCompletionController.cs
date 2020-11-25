using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.Payment;
using webzpitest.Filters;
using Newtonsoft.Json;
using System.IO;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Text;


//using System.Web.Http.Cors;

namespace webzpitest.Controllers.Payment
{
    //// [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[APIAuthorizeAttribute]
    public class OnlinePaymentCompletionController : ApiController
    {

        [HttpGet]
        [Route("WeSchool/PaymentCompletion/{studentcode}")]
        public PaymentCompletiondetails PaymentCompletionbind(int studentcode)
        {
            PaymentCompletiondetails Detail = new PaymentCompletiondetails();
            var slist = new PaymentCompletion();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("usp_api_handlereturn_status", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = studentcode;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                slist.studentcode = studentcode;
                slist.requestfor = Convert.ToString(dt.Rows[0]["RequestFor"]);
                slist.paidstatus = Convert.ToString(dt.Rows[0]["Paid"]);
                slist.amount = Convert.ToInt32(dt.Rows[0]["AMT"]);
                if (slist.paidstatus == "Y")
                {
                    slist.transmsg = "Thank You. Online Transaction For Rs." + slist.amount + " is done Successfully";
                    if (slist.requestfor.ToUpper() == "COURSEFEES")
                    {
                        var coursefees = new Couresfees();
                        coursefees.apiname = "http://api.stephenventures.com/WeSchool/PaymentDetail/";
                        coursefees.studentcode = studentcode;
                        slist.msg = "View Payment Details";
                        coursefees.method = "GET";
                        Detail.CouresfeesDetail = coursefees;
                    }
                    else if (slist.requestfor.ToUpper() == "SPECIALEVENT")
                    {
                        var spevents = new Specialevent();
                        spevents.apiname = "http://api.stephenventures.com/WeSchool/RegisteredEvents/";
                        spevents.studentcode = studentcode;
                        slist.msg = "View Registered Value Added Activities";
                        spevents.method = "GET";
                        Detail.SpecialeventDetail = spevents;
                    }
                    else if (slist.requestfor.ToUpper() == "REEXAM")
                    {
                        var reexam = new ReExams();
                        reexam.apiname = "http://api.stephenventures.com/WeSchool/ReExamDetail/";
                        reexam.studentcode = studentcode;
                        slist.msg = "View Applied ReExam Subjects";
                        reexam.method = "GET";
                        Detail.ReExamDetail = reexam;
                    }
                    else if (slist.requestfor.ToUpper() == "RESCHEDULINGWELIKE")
                    {

                        var welike = new Welikevivabooking();
                        slist.msg = "Book Now WeLike Viva Reschedule";
                        welike.apiname = "https://api.welingkar.net/welikeviva/studentdata";
                        welike.method = "POST";
                        welike.place = Convert.ToString(dt.Rows[0]["Place"]);
                        welike.ecode = Convert.ToInt32(dt.Rows[0]["examyearcode"]);
                        welike.candid = studentcode;
                        welike.student_name = Convert.ToString(dt.Rows[0]["Name"]);
                        welike.admissionno = Convert.ToString(dt.Rows[0]["admissionno"]);
                        welike.password = Convert.ToString(dt.Rows[0]["password"]);
                        welike.email = Convert.ToString(dt.Rows[0]["homeemail"]);
                        welike.mobileno = Convert.ToString(dt.Rows[0]["HomeMobileNo"]);
                        welike.type = "e";
                        welike.project_type = 1;
                        welike.paymentdone = "1";
                        welike.reschedule = "0";
                        Detail.WelikevivabookingDetail = welike;
                    }
                    else if (slist.requestfor.ToUpper() == "RESCHEDULINGVIVA")
                    {
                        var finalviva = new Finalvivabooking();
                        slist.msg = "Book Now Final year Viva Reschedule";
                        finalviva.apiname = "https://api.welingkar.net/welikeviva/studentdata";
                        finalviva.method = "POST";
                        finalviva.place = "o";
                        finalviva.ecode = Convert.ToInt32(dt.Rows[0]["examyearcode"]);
                        finalviva.candid = studentcode;
                        finalviva.student_name = Convert.ToString(dt.Rows[0]["Name"]);
                        finalviva.admissionno = Convert.ToString(dt.Rows[0]["admissionno"]);
                        finalviva.password = Convert.ToString(dt.Rows[0]["password"]);
                        finalviva.email = Convert.ToString(dt.Rows[0]["homeemail"]);
                        finalviva.mobileno = Convert.ToString(dt.Rows[0]["HomeMobileNo"]);
                        finalviva.type = "e";
                        finalviva.project_type = 2;
                        finalviva.paymentdone = "1";
                        finalviva.reschedule = "0";
                        Detail.FinalvivabookingDetail = finalviva;
                    }
                }
                else if (slist.paidstatus == "N")
                {
                    slist.transmsg = "Transaction Fail";
                    slist.msg = Convert.ToString(dt.Rows[0]["tran_errormsg"]);
                }
                else
                {
                    slist.transmsg = "Transaction Aborted";
                    slist.msg = Convert.ToString(dt.Rows[0]["tran_errormsg"]);
                }

                Detail.PaymentCompletionDetail = slist;

            }

            return Detail;
        }


    }
}
