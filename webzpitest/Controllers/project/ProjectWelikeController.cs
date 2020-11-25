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
    public class ProjectWelikeController : ApiController
    {

        [HttpGet]
        [Route("WeSchool/ProjectWelike/{studentcode}")]
        public projectwelikedetail Projectwelike(int studentcode)
        {
            List<projectwelike> projectwelikelist = new List<projectwelike>();
            projectwelikedetail Details = new projectwelikedetail();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_Project_Welike", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = studentcode;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new projectwelike();
                            slist.LinkName = Convert.ToString(dtrow["linkName"]);
                            slist.FileName = Convert.ToString(dtrow["fileName"]);
                            projectwelikelist.Add(slist);
                        }
                        Details.projectwelikedetails = projectwelikelist;
                        return Details;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
            }
        }
    }
}
