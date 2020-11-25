using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.weaudio;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.weaudio
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class weaudioController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/AudioBookSubjects")]
        public weaudiosubdetail weaudiosubdisplay()
        {

            List<weaudiosubjects> weaudiosubjectslist = new List<weaudiosubjects>();
            weaudiosubdetail Details = new weaudiosubdetail();
            var weaudiosubjectsRequestmessage = Request.Content.ReadAsStringAsync();
            weaudiosubrequest weaudiosubRequest = JsonConvert.DeserializeObject<weaudiosubrequest>(weaudiosubjectsRequestmessage.Result.ToString());
            if (weaudiosubjectsRequestmessage.Result.ToString() != null && weaudiosubjectsRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_weaudio_subjectdisplay_semwise", con))
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
                                var slist = new weaudiosubjects();
                                slist.subcode = Convert.ToString(dtrow["code"]);
                                slist.subjectname = Convert.ToString(dtrow["name"]);
                                slist.semestername = Convert.ToString(dtrow["semestername"]);
                                weaudiosubjectslist.Add(slist);
                            }
                            Details.weaudiosubdet = weaudiosubjectslist;
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
