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
    public class MidtermResultController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/MidTermResult")]
        public MidTermResultdetail loadMidTermResult()
        {
            List<MidTermResult> MidtermResultlist = new List<MidTermResult>();
            MidTermResultdetail Details = new MidTermResultdetail();
            var MidTermResultRequestmessage = Request.Content.ReadAsStringAsync();
            MidTermResultrequest MidtermRequest = JsonConvert.DeserializeObject<MidTermResultrequest>(MidTermResultRequestmessage.Result.ToString());
            if (MidTermResultRequestmessage.Result.ToString() != null && MidTermResultRequestmessage.Result.ToString() != "")
            {
               // using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["prodresultcon"].ConnectionString))
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_api_Result_MidTerm", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = MidtermRequest.studentcode;
                        cmd.Parameters.Add("@examyearcode", SqlDbType.Int).Value = MidtermRequest.examyearcode;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new MidTermResult();
                                slist.studentcode = Convert.ToInt32(dtrow["studentcode"]);
                                slist.examyearcode = Convert.ToInt32(dtrow["examyearcode"]);
                                slist.subjectcode = Convert.ToInt32(dtrow["subjectcode"]);
                                slist.subjectname = Convert.ToString(dtrow["SubjectName"]);
                                slist.Assignmarksoutof50 = Convert.ToString(dtrow["AssignMarksoutof50"]);
                                slist.Assignweightedoutof20 = Convert.ToString(dtrow["AssignWeightedoutof20"]);
                                MidtermResultlist.Add(slist);
                            }
                            Details.MidTermResultdetails = MidtermResultlist;
                            return Details;
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
