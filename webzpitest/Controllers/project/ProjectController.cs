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
    public class ProjectController : ApiController
    {
        [HttpGet]
        [Route("WeSchool/ProjectYear/{studentcode}")]
        public projectyeardetail loadprojectyear(int studentcode)
        {
            List<projectyear> projectyearlist = new List<projectyear>();
            projectyeardetail Details = new projectyeardetail();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_projectyear", con))
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
                            var slist = new projectyear();
                            slist.Project_Welike = Convert.ToString(dtrow["ProjectWelike"]);
                            slist.FinalYear_Project = Convert.ToString(dtrow["FinalYear_Project"]);
                            projectyearlist.Add(slist);
                        }
                        Details.projectyeardetails = projectyearlist;
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
