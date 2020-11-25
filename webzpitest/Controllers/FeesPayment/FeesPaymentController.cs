using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.FeesPayment;
using webzpitest.Filters;
using Newtonsoft.Json;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.FeesPayment
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class FeesPaymentController : ApiController
    {

        [HttpGet]
        [Route("WeSchool/PaymentMode/{studentcode}")]
        public paymentmodedetails paymodebind(int studentcode)
        {
            paymentmodedetails Detail = new paymentmodedetails();
            List<paymode> paymodelist = new List<paymode>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_paymentfees2ndyr_paymode", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = studentcode;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new paymode();
                            slist.paymentcode = Convert.ToInt32(dtrow["paymentcode"]);
                            slist.paymentmode = Convert.ToString(dtrow["paymentmode"]);
                            paymodelist.Add(slist);
                        }
                        Detail.paymentmodedetail = paymodelist;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
            }
            return Detail;
        }



        [HttpGet]
        [Route("WeSchool/PaymentDetail/{studentcode}")]
        public FeesPaymentdetails FeesPaymentdetailsbind(int studentcode)
        {
            FeesPaymentdetails Detail = new FeesPaymentdetails();
            List<Fees2ndyrPayment> paymentDetaillist = new List<Fees2ndyrPayment>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("select top 1 Admissionno,password,franchiseeid from applicationsdlp where code=" + studentcode, con))
                {
                    cmd.CommandType = CommandType.Text;
                    // cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = studentcode;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            string admissionno, password, ctype, franchiseeid;
                            admissionno = Convert.ToString(dtrow["Admissionno"]);
                            password = Convert.ToString(dtrow["password"]);
                            ctype = admissionno.Substring(0, 4);
                            franchiseeid = Convert.ToString(dtrow["franchiseeid"]);
                            if (ctype.ToUpper() == "MPGD")
                            {
                                var slist = new Fees2ndyrPayment();
                                slist.canpay = "False";
                                slist.downloadbtn = "False";
                                slist.attentionmsg = "ALREADY DONE FULL PAYMENT FOR COURSE";
                                paymentDetaillist.Add(slist);
                                Detail.FeesPaymentdetail = paymentDetaillist;

                            }
                            else if (franchiseeid == "21" || franchiseeid == "22" || franchiseeid == "23" || franchiseeid == "24")
                            {
                                var slist = new Fees2ndyrPayment();
                                slist.canpay = "False";
                                slist.downloadbtn = "False";
                                slist.attentionmsg = "Kindly use your student login from Desktop/Laptop/Browser to make your fees payment";
                                paymentDetaillist.Add(slist);
                                Detail.FeesPaymentdetail = paymentDetaillist;
                            }

                            else
                            {

                                using (SqlCommand cmd1 = new SqlCommand("Pr_Applicationsdlp_selectForPayment_dummy", con))
                                {
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add("@Admissionno", SqlDbType.NVarChar).Value = admissionno;
                                    cmd1.Parameters.Add("@Password", SqlDbType.NVarChar).Value = password;
                                    SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                                    DataTable dt1 = new DataTable();
                                    sda1.Fill(dt1);
                                    if (dt1.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow1 in dt1.Rows)
                                        {

                                            var slist = new Fees2ndyrPayment();
                                            string msg, lastdatestatus;
                                            msg = Convert.ToString(dtrow1["msg"]);
                                            if (msg == "1")
                                            {
                                                lastdatestatus = Convert.ToString(dtrow1["LastDatestatus"]);
                                                slist.FelStudent = "False";
                                                slist.canpay = "True";
                                                slist.admissionno = Convert.ToString(dtrow1["Admissionno"]);
                                                slist.studentname = Convert.ToString(dtrow1["Name"]);
                                                //  slist.coursename = Convert.ToString(dtrow1["course"]);
                                                //  slist.coursecode = Convert.ToInt32(dtrow1["coursecode"]);
                                                slist.franciseeid = Convert.ToInt32(dtrow1["franchiseeid"]);
                                                //  slist.batchname = Convert.ToString(dtrow1["batchname"]);
                                                //  slist.batchcode = Convert.ToInt32(dtrow1["batchcode"]);
                                                //  slist.payingfor = "2nd Year Fees";
                                                slist.RequestFor = "COURSEFEES";
                                                slist.otherparam1 = "0";
                                                slist.otherparam2 = "0";
                                                slist.downloadbtn = "False";
                                                if (slist.franciseeid == 0)
                                                {
                                                    slist.attentionmsg = "Welingkar Student";
                                                }
                                                else if (slist.franciseeid == 2)
                                                {
                                                    slist.attentionmsg = "Cheque/DD to be made in favour of - FEL A/C Welingkar";

                                                }
                                                else if (slist.franciseeid == 8)
                                                {
                                                    slist.attentionmsg = "Cheque/DD to be made in favour of - JARO A/C Welingkar";

                                                }
                                                else
                                                {
                                                    slist.attentionmsg = admissionno;
                                                }

                                                if (lastdatestatus == "datecrossed")
                                                {
                                                    slist.amount = (1000 + Convert.ToInt32(dtrow1["amountsecondinstallment"]));
                                                    slist.amountdescription = "Late Fees Rs.1000 has been included";
                                                }
                                                else
                                                {
                                                    slist.amount = Convert.ToInt32(dtrow1["amountsecondinstallment"]);
                                                    slist.amountdescription = "";
                                                }
                                            }
                                            else if (msg == "5")
                                            {
                                                slist.canpay = "False";
                                                slist.FelStudent = "False";
                                                slist.downloadbtn = "False";
                                                slist.attentionmsg = "Invalid Admissionno and Password";
                                            }
                                            else if (msg == "7")
                                            {
                                                slist.canpay = "False";
                                                slist.FelStudent = "False";
                                                slist.downloadbtn = "False";
                                                slist.attentionmsg = Convert.ToString(dtrow1["msg1"]); ;
                                            }
                                            else if (msg == "F")
                                            {
                                                slist.FelStudent = "True";

                                                using (SqlCommand cmdf = new SqlCommand("Pr_Applicationsdlp_selectForPaymentFel", con))
                                                {
                                                    cmdf.CommandType = CommandType.StoredProcedure;
                                                    cmdf.Parameters.Add("@Admissionno", SqlDbType.NVarChar).Value = admissionno;                                                   
                                                    SqlDataAdapter sdaf = new SqlDataAdapter(cmdf);
                                                    DataTable dtf = new DataTable();
                                                    sdaf.Fill(dtf);
                                                    if (dtf.Rows.Count > 0)
                                                    {
                                                        foreach (DataRow dtrowf in dtf.Rows)
                                                        {
                                                          
                                                            string msgf, lastdatestatusf;
                                                            msgf = Convert.ToString(dtrowf["msg"]);
                                                            if (msgf == "1")
                                                            {

                                                                string retString = admissionno.Substring(5, 2);
                                                                string endString = admissionno.Substring(7, 2);
                                                                lastdatestatusf = Convert.ToString(dtrowf["LastDatestatus"]);
                                                                slist.FelStudent = "True";
                                                                slist.canpay = "True";
                                                                slist.admissionno = Convert.ToString(dtrowf["Admissionno"]);
                                                                slist.studentname = Convert.ToString(dtrowf["Name"]);
                                                                slist.franciseeid = Convert.ToInt32(dtrowf["franchiseeid"]);
                                                                slist.feeplancode = Convert.ToInt32(dtrowf["feeplancode"]);
                                                                slist.RequestFor = "COURSEFEES";
                                                                slist.otherparam1 = "0";
                                                                slist.otherparam2 = "0";
                                                                slist.downloadbtn = "False";
                                                                if (slist.franciseeid == 0)
                                                                {
                                                                    slist.attentionmsg = "Welingkar Student";
                                                                }
                                                                else if (slist.franciseeid == 2)
                                                                {
                                                                    slist.attentionmsg = "Cheque/DD to be made in favour of - FEL A/C Welingkar";

                                                                }
                                                                else if (slist.franciseeid == 8)
                                                                {
                                                                    slist.attentionmsg = "Cheque/DD to be made in favour of - JARO A/C Welingkar";

                                                                }
                                                                if (lastdatestatusf == "datecrossed")
                                                                {
                                                                    slist.amount = Convert.ToInt32(dtrowf["amount"]);
                                                                    slist.amountdescription = "";
                                                                }
                                                                else
                                                                {
                                                                    slist.amount = Convert.ToInt32(dtrowf["amount"]);
                                                                    slist.amountdescription = "";
                                                                }

                                                                if (slist.feeplancode == 204)
                                                                {
                                                                    slist.partamount1 = 9500;
                                                                    slist.partamount2 = 9500;
                                                                }
                                                                else
                                                                {
                                                                    slist.partamount1 = 8000;
                                                                    slist.partamount2 = 8000;
                                                                }
                                                                DateTime dates = DateTime.Now;
                                                                int endyear = Convert.ToInt32("20" + endString);
                                                                if (retString == "JA")
                                                                {
                                                                    if (Convert.ToInt32(dates.Year) > endyear)
                                                                    {
                                                                        slist.partdddate1 = "03-Sep-" + dates.Year.ToString();
                                                                        slist.partdddate2 = "06-Sep-" + dates.Year.ToString();
                                                                    }
                                                                    else if (Convert.ToInt32(dates.Year) == endyear)
                                                                    {
                                                                        slist.partdddate1 = "03-Sep-" + Convert.ToString((Convert.ToInt32(dates.Year) + 1));
                                                                        slist.partdddate2 = "06-Sep-" + Convert.ToString((Convert.ToInt32(dates.Year) + 1));
                                                                    }
                                                                }
                                                                else if (retString == "AP")
                                                                {
                                                                    if (Convert.ToInt32(dates.Year) > endyear)
                                                                    {
                                                                        slist.partdddate1 = "06-Jun-" + dates.Year.ToString();
                                                                        slist.partdddate2 = "09-Jun-" + dates.Year.ToString();
                                                                    }
                                                                    else if (Convert.ToInt32(dates.Year) == endyear)
                                                                    {
                                                                        slist.partdddate1 = "06-Jun-" + Convert.ToString((Convert.ToInt32(dates.Year) + 1));
                                                                        slist.partdddate2 = "09-Jun-" + Convert.ToString((Convert.ToInt32(dates.Year) + 1));
                                                                    }

                                                                }
                                                                else if (retString == "JL")
                                                                {
                                                                    if (Convert.ToInt32(dates.Year) > endyear)
                                                                    {
                                                                        slist.partdddate1 = "09-Jun-" + dates.Year.ToString();
                                                                        slist.partdddate2 = "12-Jul-" + dates.Year.ToString();
                                                                    }
                                                                    else if (Convert.ToInt32(dates.Year) == endyear)
                                                                    {
                                                                        slist.partdddate1 = "09-Jun-" + Convert.ToString((Convert.ToInt32(dates.Year) + 1));
                                                                        slist.partdddate2 = "12-Jul-" + Convert.ToString((Convert.ToInt32(dates.Year) + 1));
                                                                    }
                                                                }
                                                                else if (retString == "OC")
                                                                {
                                                                    if (Convert.ToInt32(dates.Year) > endyear)
                                                                    {
                                                                        slist.partdddate1 = "12-Jul-" + dates.Year.ToString();
                                                                        slist.partdddate2 = "03-Sep-" + dates.Year.ToString();
                                                                    }
                                                                    else if (Convert.ToInt32(dates.Year) == endyear)
                                                                    {
                                                                        slist.partdddate1 = "12-Jul-" + Convert.ToString((Convert.ToInt32(dates.Year) + 1));
                                                                        slist.partdddate2 = "03-Sep-" + Convert.ToString((Convert.ToInt32(dates.Year) + 1));
                                                                    }
                                                                }

                                                            }
                                                            else if (msgf == "5")
                                                            {
                                                                slist.canpay = "False";
                                                                slist.FelStudent = "True";
                                                                slist.downloadbtn = "False";
                                                                slist.attentionmsg = "Invalid Admissionno and Password";
                                                            }
                                                            else
                                                            {
                                                                slist.canpay = "False";
                                                                slist.downloadbtn = "True";
                                                                slist.attentionmsg = Convert.ToString(dtrowf["msg"]) + " " + Convert.ToString(dtrowf["formno"]);
                                                                slist.downloadformlink = "http://courses.welingkaronline.org/Coursefeesinstallment/Print_2ndyr_form_ssrs.aspx?Code=" + Convert.ToString(dtrowf["formno"]) + "";

                                                            }
                                           
                                                        }
                                                    }
                                                }


                                            }
                                            else
                                            {
                                                slist.canpay = "False";
                                                slist.downloadbtn = "True";
                                                slist.attentionmsg = Convert.ToString(dtrow1["msg"]) + " " + Convert.ToString(dtrow1["formno"]);
                                                slist.downloadformlink = "http://courses.welingkaronline.org/Coursefeesinstallment/Print_2ndyr_form_ssrs.aspx?Code=" + Convert.ToString(dtrow1["formno"]) + "";
                                            }
                                            paymentDetaillist.Add(slist);
                                            Detail.FeesPaymentdetail = paymentDetaillist;
                                        }

                                    }
                                    else
                                    {
                                        throw new HttpResponseException(HttpStatusCode.NoContent);
                                    }

                                }
                            }

                        }
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
            }
            return Detail;
        }

        [HttpPost]
        [Route("WeSchool/fees2ndyearChequeDD")]
        public ChequeDDPaymentDetail fees2ndyearChequeDD()
        {
            ChequeDDPaymentDetail Detail = new ChequeDDPaymentDetail();
            ChequeDDPayment ChequeDDPaymentlist = new ChequeDDPayment();
            var ChequeDDRequestmessage = Request.Content.ReadAsStringAsync();
            ChequeDDPaymentrequest ChequeDDrequest = JsonConvert.DeserializeObject<ChequeDDPaymentrequest>(ChequeDDRequestmessage.Result.ToString());
            if (ChequeDDRequestmessage.Result.ToString() != null && ChequeDDRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Pr_Challans_InsertForCourseFees", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = ChequeDDrequest.studentcode;
                    cmd.Parameters.Add("@departmentcode", SqlDbType.Int).Value = 55;
                    cmd.Parameters.Add("@Admissionno", SqlDbType.NVarChar).Value = ChequeDDrequest.admissionno;
                    cmd.Parameters.Add("@Paymentmode", SqlDbType.Int).Value = ChequeDDrequest.paymentmodecode;
                    cmd.Parameters.Add("@ddno", SqlDbType.NVarChar).Value = ChequeDDrequest.ddno;
                    cmd.Parameters.Add("@ddbank", SqlDbType.NVarChar).Value = ChequeDDrequest.ddbank;
                    cmd.Parameters.Add("@dddate", SqlDbType.DateTime).Value = ChequeDDrequest.dddate;
                    cmd.Parameters.Add("@CourseFees", SqlDbType.Int).Value = ChequeDDrequest.amount;
                    cmd.Parameters.Add("@lastmodusercode", SqlDbType.Int).Value = ChequeDDrequest.studentcode;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    using (SqlCommand cmd1 = new SqlCommand("select code from challans where studentcode=" + ChequeDDrequest.studentcode + " and challantypecode=11", con))
                    {
                        cmd1.CommandType = CommandType.Text;
                        SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                        DataTable dt1 = new DataTable();
                        sda1.Fill(dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt1.Rows)
                            {
                               // var slist = new ChequeDDPayment();
                                string st = @"Congratulations. Your Form No  " + Convert.ToString(dtrow["code"])
                                    + ". You have successfully submited your Online-CourseFees Installment Form. " +
                                    "Send the printout of the Course Fees Installment Form along with payment to the Institute.";
                                ChequeDDPaymentlist.attentionmsg = st;
                                ChequeDDPaymentlist.downloadformlink = "http://courses.welingkaronline.org/Coursefeesinstallment/Print_2ndyr_form_ssrs.aspx?Code=" + Convert.ToString(dtrow["code"]) + "";
                                //ChequeDDPaymentlist.Add(slist);
                            }
                        }
                        else
                        {
                            throw new HttpResponseException(HttpStatusCode.NoContent);
                        }
                        Detail.fees2ndyrDDPayment = ChequeDDPaymentlist;
                        return Detail;
                    }
                }
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }



        [HttpPost]
        [Route("WeSchool/Felfees2ndyearChequeDD")]
        public FelChequeDDPaymentDetail Felfees2ndyearChequeDD()
        {
            FelChequeDDPaymentDetail Detail = new FelChequeDDPaymentDetail();
            FelChequeDDPayment FelDDPaymentlist = new FelChequeDDPayment();
            var FelDDRequestmessage = Request.Content.ReadAsStringAsync();
            FelChequeDDPaymentrequest FelDDrequest = JsonConvert.DeserializeObject<FelChequeDDPaymentrequest>(FelDDRequestmessage.Result.ToString());
            if (FelDDRequestmessage.Result.ToString() != null && FelDDRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Pr_Challans_InsertForCourseFees_Fel", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Admissionno", SqlDbType.NVarChar).Value = FelDDrequest.admissionno;
                    cmd.Parameters.Add("@Amt1", SqlDbType.Int).Value = FelDDrequest.amount;
                    cmd.Parameters.Add("@Paymode1", SqlDbType.Int).Value = FelDDrequest.paymentmodecode;
                    cmd.Parameters.Add("@ddno1", SqlDbType.NVarChar).Value = FelDDrequest.ddno;
                    cmd.Parameters.Add("@dddate1", SqlDbType.DateTime).Value = FelDDrequest.dddate;
                    cmd.Parameters.Add("@ddbank1", SqlDbType.NVarChar).Value = FelDDrequest.ddbank;
                    cmd.Parameters.Add("@MultiPayMode", SqlDbType.Bit).Value = 1;
                    cmd.Parameters.Add("@Amt2", SqlDbType.Int).Value = FelDDrequest.partamount1;
                    cmd.Parameters.Add("@Paymode2", SqlDbType.Int).Value = FelDDrequest.paymentmodecode1;
                    cmd.Parameters.Add("@ddno2", SqlDbType.NVarChar).Value = FelDDrequest.partddno1;
                    cmd.Parameters.Add("@dddate2", SqlDbType.DateTime).Value = FelDDrequest.partdddate1;
                    cmd.Parameters.Add("@ddbank2", SqlDbType.NVarChar).Value = FelDDrequest.partddbank1;
                    cmd.Parameters.Add("@Amt3", SqlDbType.Int).Value = FelDDrequest.partamount2;
                    cmd.Parameters.Add("@Paymode3", SqlDbType.Int).Value = FelDDrequest.paymentmodecode2;
                    cmd.Parameters.Add("@ddno3", SqlDbType.NVarChar).Value = FelDDrequest.partddno2;
                    cmd.Parameters.Add("@dddate3", SqlDbType.DateTime).Value = FelDDrequest.partdddate2;
                    cmd.Parameters.Add("@ddbank3", SqlDbType.NVarChar).Value = FelDDrequest.partddbank2;
                    cmd.Parameters.Add("@LastModUserCode", SqlDbType.Int).Value = FelDDrequest.studentcode;
                    cmd.Parameters.Add("@Feeplancode", SqlDbType.Int).Value = FelDDrequest.feeplancode;
                    cmd.ExecuteNonQuery();
                    con.Close();

                    con.Open();
                    SqlCommand cmdd = new SqlCommand("Pr_Challans_InsertForCourseFees1_Fel", con);
                    cmdd.CommandType = CommandType.StoredProcedure;
                    cmdd.Parameters.Add("@Admissionno", SqlDbType.NVarChar).Value = FelDDrequest.admissionno;
                    cmdd.Parameters.Add("@Amt1", SqlDbType.Int).Value = FelDDrequest.partamount2;
                    cmdd.Parameters.Add("@Paymode1", SqlDbType.Int).Value = FelDDrequest.paymentmodecode2;
                    cmdd.Parameters.Add("@ddno1", SqlDbType.NVarChar).Value = FelDDrequest.partddno2;
                    cmdd.Parameters.Add("@dddate1", SqlDbType.DateTime).Value = FelDDrequest.partdddate2;
                    cmdd.Parameters.Add("@ddbank1", SqlDbType.NVarChar).Value = FelDDrequest.partddbank2;                
                    cmdd.Parameters.Add("@LastModUserCode", SqlDbType.Int).Value = FelDDrequest.studentcode;                  
                    cmdd.ExecuteNonQuery();
                    con.Close();
                    using (SqlCommand cmd1 = new SqlCommand("select top 1 code from challans where studentcode=" + FelDDrequest.studentcode + " and challantypecode=11", con))
                    {
                        cmd1.CommandType = CommandType.Text;
                        SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                        DataTable dt1 = new DataTable();
                        sda1.Fill(dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt1.Rows)
                            {
                                string st = @"Congratulations. Your Form No  " + Convert.ToString(dtrow["code"])
                                    + ". You have successfully submited your Online-CourseFees Installment Form.";
                                FelDDPaymentlist.attentionmsg = st;
                                FelDDPaymentlist.downloadformlink = "http://courses.welingkaronline.org/Coursefeesinstallment/Print_2ndyr_form_ssrs.aspx?Code=" + Convert.ToString(dtrow["code"]) + "";
                            }
                        }
                        else
                        {
                            throw new HttpResponseException(HttpStatusCode.NoContent);
                        }
                        Detail.Felfees2ndyrDDPayment = FelDDPaymentlist;
                        return Detail;
                    }
                }
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
