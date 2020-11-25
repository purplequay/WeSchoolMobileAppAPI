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
    public class ElearningController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/ElearningBook")]
        public elearningbooksdetail elearningbooksdisplay()
        {
            elearningbooks elearningbooklist = new elearningbooks();
            List<elearningbooks> elearningbookbindlist = new List<elearningbooks>();
            elearningbooksdetail Detils = new elearningbooksdetail();
            var elearningbookRequestmessage = Request.Content.ReadAsStringAsync();
            elearningbooksrequest elearningbookRequest = JsonConvert.DeserializeObject<elearningbooksrequest>(elearningbookRequestmessage.Result.ToString());
            if (elearningbookRequestmessage.Result.ToString() != null && elearningbookRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_elearning_subjectdisplay_semwise", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Semcode", SqlDbType.Int).Value = elearningbookRequest.SemCode;
                        cmd.Parameters.Add("@StudentCode", SqlDbType.Int).Value = elearningbookRequest.StudentCode;      
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            var slist = new elearningbooks();
                            slist.SubCode = Convert.ToString(rdr["code"]);
                            slist.SubjectName = Convert.ToString(rdr["name"]);
                            slist.SemesterName = Convert.ToString(rdr["semestername"]);                          
                            elearningbookbindlist.Add(slist);
                        }
                        con.Close();
                    }
                    Detils.elearningbooksdet = elearningbookbindlist;
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
