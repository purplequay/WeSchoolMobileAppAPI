using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.student;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.student
{
    //  [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class StateController : ApiController
    {
        [HttpGet]
        [Route("WeSchool/State")]
        public statedetails bindstate()
        {
            List<bindstate> statelist = new List<bindstate>();
            statedetails details = new statedetails();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_bindstate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new bindstate();
                            slist.statecode = Convert.ToInt32(dtrow["code"]);
                            slist.statename = Convert.ToString(dtrow["name"]);
                            statelist.Add(slist);
                        }
                        details.statedetail = statelist;
                    }
                }
            }
            return details;
        }
    }
}
