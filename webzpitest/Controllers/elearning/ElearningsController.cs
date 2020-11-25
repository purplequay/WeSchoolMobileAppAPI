using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.elearning;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;
namespace webzpitest.Controllers.elearning
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class ElearningsController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/ElearningToolkit")]
        public elearningtoolkitdetail elearningtoolkitdisplay()
        {           
            List<elearningtoolkit> elearningtoolkitbindlist = new List<elearningtoolkit>();
            elearningtoolkitdetail Detils = new elearningtoolkitdetail();
            var elearningtoolkitRequestmessage = Request.Content.ReadAsStringAsync();
            elearningtoolkitrequest elearningtoolkitRequest = JsonConvert.DeserializeObject<elearningtoolkitrequest>(elearningtoolkitRequestmessage.Result.ToString());
            if (elearningtoolkitRequestmessage.Result.ToString() != null && elearningtoolkitRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["elearningcon"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_ELearningMCQs_GetChaptersListWithUpload", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;                      
                        cmd.Parameters.Add("@SubjectCode", SqlDbType.Int).Value = elearningtoolkitRequest.SubCode; 
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            var slist = new elearningtoolkit();
                            slist.code = rdr["code"].ToString();
                            slist.chaptername = rdr["chaptername"].ToString();
                            slist.summary = rdr["summary"].ToString();
                            slist.ppt = rdr["ppt"].ToString();
                            slist.mcq = "http://elearning.nokomis.in/QuestionTest.aspx?ID=" + rdr["code"].ToString() + "&Name=" + rdr["chaptername"].ToString() + "&Sid=" + elearningtoolkitRequest.SubCode + "&studentcode=" + elearningtoolkitRequest.StudentCode + "";                          
                            slist.subjectcode = rdr["SubjectCode"].ToString();
                            elearningtoolkitbindlist.Add(slist);
                        }
                        con.Close();
                    }
                    Detils.elearningtoolkitdet = elearningtoolkitbindlist;
                }
                return Detils;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
