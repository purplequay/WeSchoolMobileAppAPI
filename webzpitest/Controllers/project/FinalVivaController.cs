using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.project;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;
namespace webzpitest.Controllers.project
{
    //  [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class FinalVivaController : ApiController
    {

        [HttpPost]
        [Route("WeSchool/FinalVivaConfirmation")]
        public Finaldatevalidationdetails Finaldatevalidationbind()
        {
            Finaldatevalidationdetails Detail = new Finaldatevalidationdetails();
            List<Finaldatevalidation> finaldatevalidationlist = new List<Finaldatevalidation>();
            var finaldatevalidationRequestmessage = Request.Content.ReadAsStringAsync();
            FinalvivabookingRequest finalvivabookingrequest = JsonConvert.DeserializeObject<FinalvivabookingRequest>(finaldatevalidationRequestmessage.Result.ToString());
            if (finaldatevalidationRequestmessage.Result.ToString() != null && finaldatevalidationRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("checkprojectvivabooking", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = finalvivabookingrequest.studentcode;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new Finaldatevalidation();
                                string checkvalid = Convert.ToString(dtrow["msg"]);
                                if (checkvalid == "Valid")
                                {
                                    slist.candid = finalvivabookingrequest.studentcode;
                                    slist.admissionno = finalvivabookingrequest.admissionno;
                                }
                                else
                                {
                                    using (SqlCommand cmd1 = new SqlCommand("project_vivalinksn", con))
                                    {
                                        cmd1.CommandType = CommandType.StoredProcedure;
                                        cmd1.Parameters.Add("@Admissionno", SqlDbType.NVarChar).Value = finalvivabookingrequest.admissionno;
                                        SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                                        DataTable dt1 = new DataTable();
                                        sda1.Fill(dt1);
                                        if (dt1.Rows.Count > 0)
                                        {
                                            foreach (DataRow dtrow1 in dt1.Rows)
                                            {
                                                string spvalid = Convert.ToString(dtrow1["msg"]);
                                                string ECode = Convert.ToString(dtrow1["ECode"]);

                                                if (spvalid == "ValidWitoutLAteFees")
                                                {
                                                    slist.validbook = "True";
                                                    string ms = Convert.ToString(dtrow1["Remarks"]);
                                                    if (ms == "Reglular")
                                                    {
                                                        slist.type = "r";
                                                       
                                                    }
                                                    else
                                                    {
                                                        slist.type = "e";
                                                       
                                                    }
                                                    slist.candid = finalvivabookingrequest.studentcode;
                                                    slist.admissionno = finalvivabookingrequest.admissionno;
                                                    slist.place = Convert.ToString(dtrow1["place"]);
                                                    slist.student_name = Convert.ToString(dtrow1["SNAme"]);
                                                    slist.password = Convert.ToString(dtrow1["Password"]);
                                                    slist.email = Convert.ToString(dtrow1["homeemail"]);
                                                    slist.mobileno = Convert.ToString(dtrow1["HomeMobileNo"]);
                                                    slist.project_type = 2;
                                                    slist.ecode = Convert.ToInt16(dtrow1["ECode"]);
                                                    //slist.type = "r";
                                                    slist.paymentdone = "0";
                                                    slist.reschedule = "0";

                                                }

                                                else if (spvalid == "LateFeesPaidorResheduleFeesPaid")
                                                {
                                                    slist.validbook = "True";
                                                    slist.candid = finalvivabookingrequest.studentcode;
                                                    slist.admissionno = finalvivabookingrequest.admissionno;
                                                    slist.place = Convert.ToString(dtrow1["place"]);
                                                    slist.student_name = Convert.ToString(dtrow1["SNAme"]);
                                                    slist.password = Convert.ToString(dtrow1["Password"]);
                                                    slist.email = Convert.ToString(dtrow1["homeemail"]);
                                                    slist.mobileno = Convert.ToString(dtrow1["HomeMobileNo"]);
                                                    slist.project_type = 2;
                                                    slist.ecode = Convert.ToInt16(dtrow1["ECode"]);
                                                    slist.type = "e";
                                                    slist.paymentdone = "0";
                                                    slist.reschedule = "0";
                                                }
                                                else if (spvalid == "StudentPayMustLateFee")
                                                {
                                                    slist.validbook = "True";
                                                    slist.candid = finalvivabookingrequest.studentcode;
                                                    slist.admissionno = finalvivabookingrequest.admissionno;
                                                    slist.place = Convert.ToString(dtrow1["place"]);
                                                    slist.student_name = Convert.ToString(dtrow1["SNAme"]);
                                                    slist.password = Convert.ToString(dtrow1["Password"]);
                                                    slist.email = Convert.ToString(dtrow1["homeemail"]);
                                                    slist.mobileno = Convert.ToString(dtrow1["HomeMobileNo"]);
                                                    slist.project_type = 2;
                                                    slist.ecode = Convert.ToInt16(dtrow1["ECode"]);
                                                    slist.type = "e";
                                                    slist.paymentdone = "0";
                                                    slist.reschedule = "0";
                                                }
                                                else
                                                {
                                                    slist.validbook = "False";
                                                    slist.msg = "Not valid date for Final Year Viva booking";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            throw new HttpResponseException(HttpStatusCode.NoContent);
                                        }
                                    }
                                }
                                finaldatevalidationlist.Add(slist);
                            }
                            Detail.Finaldatevalidationdetail = finaldatevalidationlist;
                        }
                        else
                        {
                            throw new HttpResponseException(HttpStatusCode.NoContent);
                        }

                    }
                }
            }
            return Detail;
        }
    }
}
