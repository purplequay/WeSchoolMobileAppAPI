using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.ReExam;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.ReExam
{
    //  [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class ReExamController : ApiController
    {

        [HttpGet]
        [Route("WeSchool/ReExamDetail/{studentcode}")]
        public ReExamdetails ReExamdetailsBind(int studentcode)
        {
            List<ReExamSubjects> ReExamltlist = new List<ReExamSubjects>();
            ReExamdetails Details = new ReExamdetails();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                string Debit, Instlment2, Instlment3, Instlment4, ReExamFeesPaid, RexamFeescode, Error;
                int Balance;
                SqlCommand cmd = new SqlCommand("usp_validate_Reregistraion_Forstudent", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@StudentCode", SqlDbType.Int).Value = studentcode;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        Balance = Convert.ToInt32(dtrow["balance"]);
                        Debit = Convert.ToString(dtrow["Debit"]);
                        Instlment2 = Convert.ToString(dtrow["DueMonthsSecondInstallment"]);
                        Instlment3 = Convert.ToString(dtrow["DueMonthsThirdInstallment"]);
                        Instlment4 = Convert.ToString(dtrow["DueMonthsFourthInstallment"]);
                        Error = Convert.ToString(dtrow["error"]);

                        SqlCommand cmd1 = new SqlCommand("Pr_ReExamchallans_Select", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add("@studentcode", SqlDbType.Int).Value = studentcode;
                        cmd1.Parameters.AddWithValue("@FormNo", 0);
                        cmd1.Parameters.AddWithValue("@mode", 2);
                        SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                        DataTable dt1 = new DataTable();
                        sda1.Fill(dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow1 in dt1.Rows)
                            {
                                ReExamFeesPaid = Convert.ToString(dtrow1["ReExamFeesPaid"]);
                                RexamFeescode = Convert.ToString(dtrow1["code"]);

                                if (ReExamFeesPaid == "True")
                                {
                                    Details.Message = "You have already submitted the Re-Exam form and your Form No.is " + RexamFeescode;
                                    Details.CanPay = "False";
                                    Details.Downloadbtn = "True";

                                    string Query = "Select code,FailedExamYear,OldCourseCode,oldCourseType from ReExamChallans where code='" + RexamFeescode + "'";
                                    SqlCommand cmdd = new SqlCommand(Query, con);
                                    SqlDataAdapter sdaa = new SqlDataAdapter(cmdd);
                                    DataTable dtt = new DataTable();
                                    sdaa.Fill(dtt);
                                    if (dtt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtroww in dtt.Rows)
                                        {
                                            int ExamYearCode = Convert.ToInt32(dtroww["FailedExamYear"]);
                                            int CourseCode = Convert.ToInt32(dtroww["OldCourseCode"]);
                                            int TypeCode = Convert.ToInt32(dtroww["oldCourseType"]);
                                            int code = Convert.ToInt32(dtroww["code"]);
                                            Details.Downloadlink = "http://courses.welingkaronline.org/ReExam_Form_Print/Print_reexam_form_ssrs.aspx?cid=" + CourseCode + "&ctid=" + TypeCode + "&ey=" + ExamYearCode + "&code=" + code;
                                        }
                                    }

                                }
                                else if (Error == "regpending")
                                {
                                    Details.Message = "The Re-registration fees must be paid before applying for re-examination. Please Contact to Institution";
                                    Details.CanPay = "False";
                                    Details.Downloadbtn = "False";
                                }
                                else if ((Balance > 0 && Error == "nopending") || (Balance == 0 && Error == "nopending") || (Balance < 6500 && Error == "nopending"))
                                {
                                    string Query1 = "Pr_PGReexam_Select 1," + studentcode + "";
                                    SqlCommand cmdq = new SqlCommand(Query1, con);
                                    SqlDataAdapter sdaq = new SqlDataAdapter(cmdq);
                                    DataTable dtq = new DataTable();
                                    sdaq.Fill(dtq);
                                    if (dtq.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrowq in dtq.Rows)
                                        {
                                            var slist = new ReExamSubjects();
                                            Details.CanPay = "True";
                                            Details.Downloadbtn = "False";
                                            slist.studentcode = studentcode;
                                            slist.Admissionno = Convert.ToString(dtrowq["admissionno"]);
                                            slist.examyearcode = Convert.ToInt32(dtrowq["examyearcode"]);
                                            slist.subjectcode = Convert.ToInt32(dtrowq["subjectcode"]);
                                            slist.subjectname = Convert.ToString(dtrowq["subjectname"]);
                                            slist.totalmarks = Convert.ToString(dtrowq["totalmarks"]);
                                            slist.fees = Convert.ToInt32(dtrowq["fees"]);
                                            ReExamltlist.Add(slist);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Error == "regpending")
                            {
                                Details.Message = "The Re-registration fees must be paid before applying for re-examination. Please Contact to Institution";
                                Details.CanPay = "False";
                                Details.Downloadbtn = "False";
                            }
                            else if ((Balance > 0 && Error == "nopending") || (Balance == 0 && Error == "nopending") || (Balance < 6500 && Error == "nopending"))
                            {
                                string Query1 = "Pr_PGReexam_Select 1," + studentcode + "";
                                SqlCommand cmdq = new SqlCommand(Query1, con);
                                SqlDataAdapter sdaq = new SqlDataAdapter(cmdq);
                                DataTable dtq = new DataTable();
                                sdaq.Fill(dtq);
                                if (dtq.Rows.Count > 0)
                                {
                                    foreach (DataRow dtrowq in dtq.Rows)
                                    {
                                        var slist = new ReExamSubjects();
                                        Details.CanPay = "True";
                                        Details.Downloadbtn = "False";
                                        slist.studentcode = studentcode;
                                        slist.Admissionno = Convert.ToString(dtrowq["admissionno"]);
                                        slist.examyearcode = Convert.ToInt32(dtrowq["examyearcode"]);
                                        slist.subjectcode = Convert.ToInt32(dtrowq["subjectcode"]);
                                        slist.subjectname = Convert.ToString(dtrowq["subjectname"]);
                                        slist.totalmarks = Convert.ToString(dtrowq["totalmarks"]);
                                        slist.fees = Convert.ToInt32(dtrowq["fees"]);
                                        ReExamltlist.Add(slist);
                                    }
                                }
                            }


                        }

                    }
                }
                Details.ReExamSubjectsdetail = ReExamltlist;
            }

            return Details;

        }


        [HttpGet]
        [Route("WeSchool/ReExamPaymentMode")]
        public ReExampaymodedetails ReExampaymodebind()
        {
            ReExampaymodedetails Detail = new ReExampaymodedetails();
            List<ReExampaymode> paymodelist = new List<ReExampaymode>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select code as paymentcode,descn as paymentmode from codemaster where code in(33,34)", con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new ReExampaymode();
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



        [HttpPost]
        [Route("WeSchool/ReExamSubjectPaymentDetail")]
        public ReExamFeesPaymentdetails ReExamFeesPaymentdetailbind()
        {
            ReExamFeesPaymentdetails Detail = new ReExamFeesPaymentdetails();
            List<ReExamFeesPayment> ReExamFeesPaymentlist = new List<ReExamFeesPayment>();
            var ReExamFeesPaymentRequestmessage = Request.Content.ReadAsStringAsync();
            ReExamFeesPaymentDetailRequest ReExamFeesPaymentRequest = JsonConvert.DeserializeObject<ReExamFeesPaymentDetailRequest>(ReExamFeesPaymentRequestmessage.Result.ToString());
            if (ReExamFeesPaymentRequestmessage.Result.ToString() != null && ReExamFeesPaymentRequestmessage.Result.ToString() != "")
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
                var slist = new ReExamFeesPayment();
                string ss="Select top 1 admissionno, (FirstName+' '+MiddleName+' '+lastname)as name from applicationsdlp where code="+ ReExamFeesPaymentRequest.studentcode;
                 SqlCommand cmd = new SqlCommand(ss, con);                 
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {

                            slist.studentname = Convert.ToString(dtrow["name"]);
                            slist.admissionno = Convert.ToString(dtrow["admissionno"]);
                          
                        }
                       
                    }
                slist.studentcode = ReExamFeesPaymentRequest.studentcode;
                slist.subjectcodes = ReExamFeesPaymentRequest.subjectcodes;
                slist.examyearcode = ReExamFeesPaymentRequest.examyearcode;
                slist.reexamtotalfees = ReExamFeesPaymentRequest.reexamtotalfees;
                slist.RequestFor = "REEXAM";
                slist.otherparam1 = ReExamFeesPaymentRequest.subjectcodes;
                slist.otherparam2 = Convert.ToString(ReExamFeesPaymentRequest.examyearcode);
                slist.amount =Convert.ToInt32(ReExamFeesPaymentRequest.reexamtotalfees);
                slist.confirmmsg = "Are you sure you selected (Subject Code:" + ReExamFeesPaymentRequest.subjectcodes + ") these subjects only.";
                ReExamFeesPaymentlist.Add(slist);
                Detail.ReExamFeesPaymentdetail = ReExamFeesPaymentlist;
                return Detail;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Route("WeSchool/ReExamDDPayment")]
        public ReExamDDPaymentDetail ReExamFeesDDPayment()
        {
            ReExamDDPaymentDetail Detail = new ReExamDDPaymentDetail();
            List<ReExamDDPayment> ReExamFeesPaymentlist = new List<ReExamDDPayment>();
            var ReExamFeesDDPaymentRequestmessage = Request.Content.ReadAsStringAsync();
            ReExamDDPaymentRequest ReExamFeesDDPaymentRequest = JsonConvert.DeserializeObject<ReExamDDPaymentRequest>(ReExamFeesDDPaymentRequestmessage.Result.ToString());
            if (ReExamFeesDDPaymentRequestmessage.Result.ToString() != null && ReExamFeesDDPaymentRequestmessage.Result.ToString() != "")
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
                if (ReExamFeesDDPaymentRequest.PaymentModeCode == 33)
                {
                    using (SqlCommand cmd = new SqlCommand("select code,admissionno,departmentcode,coursecode,batchcode,coursetypecode from applicationsdlp where code=" + ReExamFeesDDPaymentRequest.studentcode, con))
                    {

                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                int departmentcode, coursecode, batchcode, coursetypecode, postexamyearcode;
                                string admissionno, st;
                                admissionno = Convert.ToString(dtrow["admissionno"]);
                                departmentcode = Convert.ToInt32(dtrow["departmentcode"]);
                                coursecode = Convert.ToInt32(dtrow["coursecode"]);
                                batchcode = Convert.ToInt32(dtrow["batchcode"]);
                                coursetypecode = Convert.ToInt32(dtrow["coursetypecode"]);
                                string retString = admissionno.Substring(5, 1);
                                if (retString == "J")
                                {
                                    st = @"select top 1 code, yearmonth as name from examyearcode where departmentcode=55 and currentexamyear=1 and 
                                            janjulbatch=1 and yearmonth like '%DLP%'";
                                }
                                else
                                {
                                    st = @"select top 1 code, yearmonth as name from examyearcode where departmentcode=55 and currentexamyear=1 
                                           and aproctbatch=1 and yearmonth like '%DLP%'";
                                }

                                SqlCommand cmdd = new SqlCommand(st, con);
                                cmdd.CommandType = CommandType.Text;
                                SqlDataAdapter sdaa = new SqlDataAdapter(cmdd);
                                DataTable dtt = new DataTable();
                                sdaa.Fill(dtt);
                                if (dtt.Rows.Count > 0)
                                {
                                    foreach (DataRow dtroww in dtt.Rows)
                                    {
                                        postexamyearcode = Convert.ToInt32(dtroww["code"]);

                                        con.Open();
                                        string str = "Pr_ReExamChallans_Insert_new";
                                        SqlCommand cmd1 = new SqlCommand(str, con);
                                        cmd1.CommandType = CommandType.StoredProcedure;
                                        cmd1.Parameters.Add("@studentcode", SqlDbType.Int).Value = ReExamFeesDDPaymentRequest.studentcode;
                                        cmd1.Parameters.Add("@Departmentcode", SqlDbType.Int).Value = departmentcode;
                                        cmd1.Parameters.Add("@OldAdmissionNo", SqlDbType.VarChar).Value = admissionno;
                                        cmd1.Parameters.Add("@OldCourseCode", SqlDbType.Int).Value = coursecode;
                                        cmd1.Parameters.Add("@OldBatchCode", SqlDbType.Int).Value = batchcode;
                                        cmd1.Parameters.Add("@OldCourseType", SqlDbType.Int).Value = coursetypecode;
                                        cmd1.Parameters.Add("@ExamYearCode", SqlDbType.Int).Value = postexamyearcode;
                                        cmd1.Parameters.Add("@FailedExamcode", SqlDbType.Int).Value = ReExamFeesDDPaymentRequest.examyearcode;
                                        cmd1.Parameters.Add("@PaymentModeCode", SqlDbType.Int).Value = ReExamFeesDDPaymentRequest.PaymentModeCode;
                                        cmd1.Parameters.Add("@DDNo", SqlDbType.VarChar).Value = ReExamFeesDDPaymentRequest.ddno;
                                        cmd1.Parameters.Add("@DDBank", SqlDbType.VarChar).Value = ReExamFeesDDPaymentRequest.ddbank;
                                        cmd1.Parameters.Add("@dddate", SqlDbType.VarChar).Value = ReExamFeesDDPaymentRequest.dddate;
                                        cmd1.Parameters.Add("@challanno", SqlDbType.VarChar).Value = "";
                                        cmd1.Parameters.Add("@challantypecode", SqlDbType.Int).Value = 10;
                                        cmd1.Parameters.Add("@challandate", SqlDbType.VarChar).Value = "";
                                        cmd1.Parameters.Add("@amount", SqlDbType.VarChar).Value = ReExamFeesDDPaymentRequest.reexamtotalfees;
                                        cmd1.Parameters.Add("@resubjects", SqlDbType.VarChar).Value = ReExamFeesDDPaymentRequest.subjectcodes;
                                        cmd1.Parameters.Add("@ReExamFeesPaid", SqlDbType.TinyInt).Value = 1;
                                        cmd1.Parameters.Add("@User_ID", SqlDbType.Int).Value = ReExamFeesDDPaymentRequest.studentcode;
                                        int ii = cmd1.ExecuteNonQuery();
                                        if (ii > 0)
                                        {
                                            con.Close();
                                            #region "pg-reexam-Insert"
                                            con.Open();
                                            string insertstr = "Exec Pr_PgReexam_Update " + ReExamFeesDDPaymentRequest.studentcode + ",'" + ReExamFeesDDPaymentRequest.subjectcodes + "'";
                                            SqlCommand cmdpg = new SqlCommand(insertstr, con);
                                            cmdpg.ExecuteNonQuery();
                                            con.Close();
                                            #endregion                                           
                                        }

                                       
                                        string Query = @"Select top 1 code,Failedexamyear,OldCourseCode,oldCourseType from ReExamChallans  
                                                        where StudentCode=" + ReExamFeesDDPaymentRequest.studentcode 
                                                        + " and FailedExamYear=" + ReExamFeesDDPaymentRequest.examyearcode + "";
                                        SqlCommand cmd2 = new SqlCommand(Query, con);
                                        SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
                                        DataTable dt2 = new DataTable();
                                        sda2.Fill(dt2);
                                        if (dt2.Rows.Count > 0)
                                        {
                                            foreach (DataRow dtrow2 in dt2.Rows)
                                            {
                                                var slist = new ReExamDDPayment();
                                                int ExamYearCode = Convert.ToInt32(dtrow2["Failedexamyear"]);
                                                int CourseCode = Convert.ToInt32(dtrow2["OldCourseCode"]);
                                                int TypeCode = Convert.ToInt32(dtrow2["oldCourseType"]);
                                                int code = Convert.ToInt32(dtrow2["code"]);
                                                slist.msg = @"Congratulations. Form No is " + Convert.ToString(code) + ". Click  Here To Download The Form";
                                                slist.downloadformlink = "http://courses.welingkaronline.org/ReExam_Form_Print/Print_reexam_form_ssrs.aspx?cid=" + CourseCode + "&ctid=" + TypeCode + "&ey=" + ExamYearCode + "&code=" + code;
                                                ReExamFeesPaymentlist.Add(slist);
                                            }
                                            Detail.ReExamDDPaymentDetails = ReExamFeesPaymentlist;
                                        }  
                                    }
                                }
                            }
                        }
                    }
                    
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
            }

            return Detail;

        }



    }
}
