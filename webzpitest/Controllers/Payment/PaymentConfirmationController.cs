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
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class PaymentConfirmationController : ApiController
    {

        [HttpPost]
        [Route("WeSchool/PayConfirmDetail")]
        public PaymentConfirmDetails Payconfirmdetailsbind()
        {
            PaymentConfirmDetails Detail = new PaymentConfirmDetails();
            List<PayConfirmDetail> PayConfirmlist = new List<PayConfirmDetail>();
            var PayConfirmRequestmessage = Request.Content.ReadAsStringAsync();
            Paycomnfirmrequest PayConfirmrequest = JsonConvert.DeserializeObject<Paycomnfirmrequest>(PayConfirmRequestmessage.Result.ToString());
            if (PayConfirmRequestmessage.Result.ToString() != null && PayConfirmRequestmessage.Result.ToString() != "")
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
                SqlCommand cmd = new SqlCommand("usp_StudentDetails_pgway", con);
                cmd.Parameters.AddWithValue("@Studentcode", Convert.ToInt32(PayConfirmrequest.studentcode));
                cmd.Parameters.AddWithValue("@RequestFor", Convert.ToString(PayConfirmrequest.RequestFor));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        var slist = new PayConfirmDetail();
                        slist.StudentName = Convert.ToString(dtrow["StudentName"]);
                        slist.Admissionno = Convert.ToString(dtrow["Admissionno"]);
                        slist.BatchName = Convert.ToString(dtrow["BatchName"]);
                        slist.CourseName = Convert.ToString(dtrow["CourseName"]);
                        slist.PayingFor = Payingfor(PayConfirmrequest.RequestFor.ToUpper());
                        //slist.Amount = PayConfirmrequest.amount;
                        slist.RequestFor = PayConfirmrequest.RequestFor;
                        if (PayConfirmrequest.RequestFor.ToUpper() == "RESCHEDULINGWELIKE" || PayConfirmrequest.RequestFor.ToUpper() == "RESCHEDULINGVIVA")
                        {
                            slist.Amount = "600";
                        }
                        else
                        {
                            slist.Amount = PayConfirmrequest.amount;
                        }

                        slist.otherparam1 = PayConfirmrequest.otherparam1;
                        slist.otherparam2 = PayConfirmrequest.otherparam2;
                        PayConfirmlist.Add(slist);
                    }
                    Detail.PaymentConfirmDetail = PayConfirmlist;
                }
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }
            return Detail;
        }

        public string Payingfor(string str)
        {
            string returnstr = "";
            //lblname.Text = "Student Name: ";
            switch (str)
            {

                case "SPECIALEVENT"://Challantype 19
                    returnstr = "Special Event";
                    break;
                case "REEXAM":
                    returnstr = "Re-Exam";
                    break;
                //case "RESCHEDULE":
                //    returnstr = "Reschedule";
                //    break;
                //case "ONLINEOVERSEAS":
                //    returnstr = "Overseas";
                //    break;
                case "COURSEFEES":
                    returnstr = "2nd Year Fees";
                    break;
                case "RESCHEDULINGWELIKE":
                    returnstr = "WeLike Rescheduling";
                    break;
                case "RESCHEDULINGVIVA":
                    returnstr = "Viva Rescheduling";
                    break;
                //case "WELIKE":
                //    returnstr = "WeLike";
                //    break;
            }
            return returnstr;
        }




        [HttpPost]
        [Route("WeSchool/OnlinePaymentInitiation")]
        public PaymentInitiationDetails Payinitiationbind()
        {
            string SRCSITEID = "L43473", CRN = "INR";
            PaymentInitiationDetails Detail = new PaymentInitiationDetails();
            List<item> itemdet = new List<item>();
            PayprocessDetail PaymentInitiationlist = new PayprocessDetail();
            var PayInitiateRequestmessage = Request.Content.ReadAsStringAsync();
            PaymentInitiationRequest PayInitiaterequest = JsonConvert.DeserializeObject<PaymentInitiationRequest>(PayInitiateRequestmessage.Result.ToString());
            if (PayInitiateRequestmessage.Result.ToString() != null && PayInitiateRequestmessage.Result.ToString() != "")
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
                SqlCommand cmd = new SqlCommand("usp_Insert_PaymentGateway_MakePayment", con);
                cmd.Parameters.AddWithValue("@Studentcode", Convert.ToInt32(PayInitiaterequest.studentcode));
                cmd.Parameters.AddWithValue("@code", PayInitiaterequest.otherparam1);
                cmd.Parameters.AddWithValue("@SRCSITEID", SRCSITEID);
                cmd.Parameters.AddWithValue("@CRN", CRN);
                cmd.Parameters.AddWithValue("@amount", PayInitiaterequest.Amount);
                cmd.Parameters.AddWithValue("@CenterCode", PayInitiaterequest.otherparam2);
                cmd.Parameters.AddWithValue("@RequestFor", PayInitiaterequest.RequestFor);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        string pgway = Convert.ToString(dtrow["PGWayCode"]);                       
                        var slist = new PayprocessDetail();
                        slist.deviceId = "WEBSH2";
                        string tk, rd, full;
                        string[] strSplitResponse;
                        full = generatetoken(pgway, PayInitiaterequest.RequestFor, PayInitiaterequest.Amount, PayInitiaterequest.studentcode);
                        strSplitResponse = full.Split('/');
                        tk = strSplitResponse[0].ToString();
                        rd = strSplitResponse[1].ToString();
                        slist.token = tk;
                       // slist.requestdata = rd;
                        slist.paymentMode = "all";
                        slist.merchantId = "L43473";
                        slist.txnId = pgway;
                        slist.txnSubType = "DEBIT";
                        slist.returnUrl = "http://api.stephenventures.com/HandleReturn/handlereturnapp.aspx";
                       // slist.consumerEmailId = PayInitiaterequest.RequestFor;
                       // con.Open();
                        //string stinsert = "insert into tbl_pgwayverification_mobile(studentcode,pgwaycode,token,createddate)values(" + PayInitiaterequest.studentcode + "," +
                        //                 pgway + ",'" + slist.token + "',getdate())";
                        //SqlCommand cmdinsert = new SqlCommand(stinsert, con);
                        //cmdinsert.ExecuteNonQuery();
                        //con.Close();
                        slist.cartDescription =  PayInitiaterequest.studentcode + "}{email: " + PayInitiaterequest.RequestFor;
                        var itemlist = new item();
                        itemlist.itemId = "Welingkar";
                        itemlist.amount = PayInitiaterequest.Amount;
                        itemlist.comAmt = "0";
                        itemdet.Add(itemlist);
                        Detail.PaymentInitiationDetail = slist;
                        Detail.items = itemdet;
                    }
                }

            }

            return Detail;
        }

        public string generatetoken(string pgway, string Requestfor, string amount, int studentcode)
        {
            string st;
            string merchantId = "L43473";
            string txnId = pgway;
            string totalamount = amount;
            string accountNo = "";
           // string accountNo = "";
            string consumerId = Convert.ToString(studentcode);
            string consumerMobileNo = "";
            //string consumerMobileNo = "";
            string consumerEmailId = "";
          //  string consumerEmailId = Requestfor;
            string debitStartDate = "";
            string debitEndDate = "";
            string maxAmount = "";
            string amountType = "";
            string frequency = "";
            string cardNumber = "";
            string expMonth = "";
            string expYear = "";
            string cvvCode = "";
            string SALT = "6093249835GNTPFD";
            st = merchantId + "|" + txnId + "|" + totalamount + "|" + accountNo + "|" + consumerId + "|" + consumerMobileNo + "|" + consumerEmailId + "|" + debitStartDate + "|" +
               debitEndDate + "|" + maxAmount + "|" + amountType + "|" + frequency + "|" + cardNumber + "|" + expMonth + "|" + expYear + "|" + cvvCode + "|" + SALT;


            #region SHA-512(WEBSH2)  Currently using

            SHA512 sha512Hash = SHA512.Create();
            byte[] sourceBytes = Encoding.UTF8.GetBytes(st);
            byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

            string token = hash.ToLower().ToString() + "/" + st;
            #endregion

            return token;
        }


    }
}
