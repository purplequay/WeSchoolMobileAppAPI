using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.ewallet;
using webzpitest.Filters;
//using System.Web.Http.Cors;
namespace webzpitest.Controllers.ewallet
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class EwalletController : ApiController
    {        
        [HttpGet]
        [Route("WeSchool/Ewallet/{studentcode}")]
        public ewalletsdetail ewallet(int studentcode)
        {
            List<ewallets> ewalletbindlist = new List<ewallets>();
            ewalletsdetail Details = new ewalletsdetail();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_gratuationcertificate", con))
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
                            var slist = new ewallets();
                            slist.studentcode = Convert.ToInt32(dtrow["studentcode"]);
                            slist.ewalletdescription = Convert.ToString(dtrow["Decs"]);
                            slist.ewalletlink = Convert.ToString(dtrow["link"]);
                            ewalletbindlist.Add(slist);
                        }
                        Details.ewalletdetail = ewalletbindlist;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
            }
            return Details;
        }
    }
}
