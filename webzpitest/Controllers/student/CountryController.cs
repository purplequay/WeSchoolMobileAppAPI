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
    public class CountryController : ApiController
    {
        [HttpGet]
        [Route("WeSchool/Country")]
        public countrydetails bindcountry()
        {
            List<bindcountry> countrylist = new List<bindcountry>();
            countrydetails details = new countrydetails();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_bindcountry", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new bindcountry();
                            slist.countrycode = Convert.ToInt32(dtrow["code"]);
                            slist.countryname = Convert.ToString(dtrow["name"]);
                            countrylist.Add(slist);
                        }
                        details.countrydetail = countrylist;
                    }
                }
            }
            return details;
        }
    }
}
