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
    public class WelikeVivaController : ApiController
    {

        [HttpGet]
        [Route("WeSchool/WelikeVivainfo/{studentcode}")]
        public welikestudentdetails Welikevivainfoload(int studentcode)
        {
            welikestudent slist = new welikestudent();
            welikestudentdetails Details = new welikestudentdetails();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Pr_SelectStudentDetils", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@StudentCode", SqlDbType.Int).Value = studentcode;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            //var slist = new welikestudent();
                            slist.studentcode = studentcode;
                            slist.studentname = Convert.ToString(dtrow["Name"]);
                            slist.admissionno = Convert.ToString(dtrow["admi"]);
                            slist.batchname = Convert.ToString(dtrow["bName"]);
                            slist.cityname = Convert.ToString(dtrow["CityName"]);
                            slist.mobileno = Convert.ToString(dtrow["HomeMobileNo"]);
                            slist.email = Convert.ToString(dtrow["homeemail"]);
                            slist.batchcode = Convert.ToInt16(dtrow["batchCode"]);
                            slist.citycode = Convert.ToInt16(dtrow["citycode"]);
                            // welikestudentlist.Add(slist);
                        }
                        Details.welikestudentdetail = slist;
                        return Details;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
            }
        }


        [HttpPost]
        [Route("WeSchool/Welikedatevalidation")]
        public welikedatevalidationdetails welikedatevalidationbind()
        {
            welikedatevalidationdetails Detail = new welikedatevalidationdetails();
            welikedatevalidation slist = new welikedatevalidation();
            var welikedatevalidationRequestmessage = Request.Content.ReadAsStringAsync();
            WelikevivabookingRequest welikevivabookingrequest = JsonConvert.DeserializeObject<WelikevivabookingRequest>(welikedatevalidationRequestmessage.Result.ToString());
            if (welikedatevalidationRequestmessage.Result.ToString() != null && welikedatevalidationRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("welike_datevalidation_city", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@admissionno", SqlDbType.NVarChar).Value = welikevivabookingrequest.admissionno;
                        cmd.Parameters.Add("@citycode", SqlDbType.Int).Value = welikevivabookingrequest.citycode;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                string Valid, ECode;
                                Valid = Convert.ToString(dtrow["msg"]);
                                ECode = Convert.ToString(dtrow["ExamYearCode"]);
                                if (Valid == "Welike")
                                {

                                    slist.validbook = "True";
                                    slist.place = Convert.ToString(dtrow["Place"]);
                                    slist.ecode = Convert.ToInt16(dtrow["ExamYearCode"]);
                                    slist.candid = welikevivabookingrequest.studentcode;
                                    slist.student_name = Convert.ToString(dtrow["Name"]);
                                    slist.admissionno = welikevivabookingrequest.admissionno;
                                    slist.password = Convert.ToString(dtrow["password"]);
                                    slist.email = Convert.ToString(dtrow["homeemail"]);
                                    slist.mobileno = Convert.ToString(dtrow["HomeMobileNo"]);
                                    slist.type = "r";
                                    slist.project_type = 1;
                                    slist.paymentdone = "0";
                                    slist.reschedule = "0";

                                }
                                else if (Valid == "ReShedulePaid")
                                {
                                    slist.validbook = "True";
                                    slist.place = Convert.ToString(dtrow["Place"]);
                                    slist.ecode = Convert.ToInt16(dtrow["ExamYearCode"]);
                                    slist.candid = welikevivabookingrequest.studentcode;
                                    slist.student_name = Convert.ToString(dtrow["Name"]);
                                    slist.admissionno = welikevivabookingrequest.admissionno;
                                    slist.password = Convert.ToString(dtrow["password"]);
                                    slist.email = Convert.ToString(dtrow["homeemail"]);
                                    slist.mobileno = Convert.ToString(dtrow["HomeMobileNo"]);
                                    slist.type = "e";
                                    slist.paymentdone = "0";
                                    slist.reschedule = "0";
                                    slist.project_type = 1;


                                }
                                else if (Valid == "WelikeLateFees")
                                {
                                    slist.validbook = "True";
                                    slist.place = Convert.ToString(dtrow["Place"]);
                                    slist.ecode = Convert.ToInt16(dtrow["ExamYearCode"]);
                                    slist.candid = welikevivabookingrequest.studentcode;
                                    slist.student_name = Convert.ToString(dtrow["Name"]);
                                    slist.admissionno = welikevivabookingrequest.admissionno;
                                    slist.password = Convert.ToString(dtrow["password"]);
                                    slist.email = Convert.ToString(dtrow["homeemail"]);
                                    slist.mobileno = Convert.ToString(dtrow["HomeMobileNo"]);
                                    slist.type = "e";
                                    slist.paymentdone = "0";
                                    slist.reschedule = "0";
                                    slist.project_type = 1;

                                }
                                else if (Valid == "DatePassed")
                                {
                                    slist.validbook = "False";
                                    slist.msg = "Not valid date for Welike Viva booking";
                                    //throw new HttpResponseException(HttpStatusCode.NoContent);
                                }
                            }
                            Detail.welikedatevalidationdetail = slist;
                            return Detail;
                        }
                        else
                        {
                            throw new HttpResponseException(HttpStatusCode.NoContent);
                        }
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
