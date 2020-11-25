using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.welectures;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;


namespace webzpitest.Controllers.welectures
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class WelecturesController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/WelecturesSubjects")]
        public welecturessubdetail welecturessubdisplay()
        {

            List<welecturessubjects> welecturessubjectslist = new List<welecturessubjects>();
            welecturessubdetail Details = new welecturessubdetail();
            var weleecturessubjectsRequestmessage = Request.Content.ReadAsStringAsync();
            welecturessubrequest weaudiosubRequest = JsonConvert.DeserializeObject<welecturessubrequest>(weleecturessubjectsRequestmessage.Result.ToString());
            if (weleecturessubjectsRequestmessage.Result.ToString() != null && weleecturessubjectsRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_welectures_subjectdisplay_semwise", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Semcode", SqlDbType.Int).Value = weaudiosubRequest.semcode;
                        cmd.Parameters.Add("@StudentCode", SqlDbType.Int).Value = weaudiosubRequest.studentcode;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new welecturessubjects();
                                slist.subcode = Convert.ToString(dtrow["code"]);
                                slist.ID = Convert.ToString(dtrow["code"]);
                                slist.subjectname = Convert.ToString(dtrow["name"]);
                                slist.semestername = Convert.ToString(dtrow["semestername"]);
                                welecturessubjectslist.Add(slist);
                            }
                            Details.welecturessubdet = welecturessubjectslist;
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
