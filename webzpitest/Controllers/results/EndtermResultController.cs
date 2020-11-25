using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.Results;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;
namespace webzpitest.Controllers.results
{
    //  [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class EndtermResultController : ApiController
    {
        [HttpGet]
        [Route("WeSchool/EndTermResult/{studentcode}")]
        public EndTermResultdetail loadEndTermResult(int studentcode)
        {
            List<EndTermResult> EndTermResultlist = new List<EndTermResult>();
            EndTermResultdetail Details = new EndTermResultdetail();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["prodresultcon"].ConnectionString);
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["examsercon"].ConnectionString);
            SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);            
                SqlCommand cmd = new SqlCommand("usp_api_Result_EndTerm", con1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = studentcode;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int failcount = 0;
                    int allow = 0;
                    string ReExamMsg="";
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        var slist = new EndTermResult();
                        slist.studentcode = Convert.ToInt32(dtrow["studentcode"]);
                        slist.examyearcode = Convert.ToInt32(dtrow["examyearcode"]);
                        slist.subjectcode = Convert.ToInt32(dtrow["subjectcode"]);
                        slist.subjectname = Convert.ToString(dtrow["subjectname"]);
                        slist.semester = Convert.ToString(dtrow["Semester"]);
                        slist.midtermmarksoutof50 = Convert.ToString(dtrow["MidTermMarksoutof50"]);
                        slist.proportionatemarksoutof20 = Convert.ToString(dtrow["ProportionateMarksoutof20"]);
                        slist.endtermmarksoutof50 = Convert.ToString(dtrow["EndTermMarksoutof50"]);
                        slist.proportionatemarksoutof80 = Convert.ToString(dtrow["ProportionateMarksoutof80"]);
                        slist.totalmarksoutof100 = Convert.ToString(dtrow["TotalMarksoutof100"]);
                        slist.Result = Convert.ToString(dtrow["Result"]);
                        if (slist.Result == "FAIL" || slist.Result == "ABSENT")
                        {
                            failcount = failcount + 1;
                        }
                        EndTermResultlist.Add(slist);
                    }
                    #region reexam allow

                    SqlCommand cmd1 = new SqlCommand("usp_api_allow_reexam", con1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    sda1.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow1 in dt1.Rows)
                        {
                            allow = Convert.ToInt32(dtrow1["allow"]);
                            ReExamMsg = Convert.ToString(dtrow1["validupto"]);
                        }
                    }
                    if (allow == 1)
                    {
                        if (failcount > 0)
                        {
                            Details.payreexam = "True";
                            Details.ReExamMsg = ReExamMsg;
                        }
                        else
                        {
                            Details.payreexam = "False";
                        }
                    }
                    else
                    {
                        Details.payreexam = "False";
                    }

                    #endregion


                    Details.Endtermresultsdetails = EndTermResultlist;
                    return Details;
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NoContent);
                }


            }
        
    }
}
