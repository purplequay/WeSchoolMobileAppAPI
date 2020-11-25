using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.videodictionary;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.videodictionary
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
     [APIAuthorizeAttribute]
    public class VideodictionaryController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/VideoDicSubjects")]
        public videodicsubdetail videodicsubjectdisplay()
        {
            List<videodicsub> videodicsublist = new List<videodicsub>();
            videodicsubdetail Details = new videodicsubdetail();
            var videodicsubRequestmessage = Request.Content.ReadAsStringAsync();
            videodicsubrequest videodicsubRequest = JsonConvert.DeserializeObject<videodicsubrequest>(videodicsubRequestmessage.Result.ToString());
            if (videodicsubRequestmessage.Result.ToString() != null && videodicsubRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_videodic_subjects_semwise", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;                       
                        cmd.Parameters.Add("@StudentCode", SqlDbType.Int).Value = videodicsubRequest.StudentCode;
                        cmd.Parameters.Add("@Semcode", SqlDbType.Int).Value = videodicsubRequest.SemCode;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new videodicsub();
                                slist.SubCode = Convert.ToString(dtrow["code"]);
                                slist.SubjectName = Convert.ToString(dtrow["name"]);
                                slist.SemesterName = Convert.ToString(dtrow["semestername"]);
                                videodicsublist.Add(slist);
                            }
                            Details.videodicsubjects = videodicsublist;
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
        
        [HttpPost]
        [Route("WeSchool/VideoDictionary")]
        public videodicdetail videodictionarydisplay()
        {
            List<Videodics> videodiclist = new List<Videodics>();
            videodicdetail Details = new videodicdetail();
            var videodicRequestmessage = Request.Content.ReadAsStringAsync();
            videodicrequest videodicRequest = JsonConvert.DeserializeObject<videodicrequest>(videodicRequestmessage.Result.ToString());
            if (videodicRequestmessage.Result.ToString() != null && videodicRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_VideoDictionary_SelectStudentView", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "|";
                        cmd.Parameters.Add("@spcode", SqlDbType.Int).Value = videodicRequest.Specicode;
                        cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("@subcode", SqlDbType.Int).Value = videodicRequest.Subjectcode;                       
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new Videodics();
                                slist.description = Convert.ToString(dtrow["discription"]);
                                slist.chaptername = Convert.ToString(dtrow["ChName"]);
                                slist.videolink = Convert.ToString(dtrow["filename"]);
                                slist.Videoname = Convert.ToString(dtrow["name"]);     
                                videodiclist.Add(slist);
                            }
                            Details.videodicdetails = videodiclist;
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
