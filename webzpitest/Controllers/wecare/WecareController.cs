using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.wecare;
using Newtonsoft.Json;
using webzpitest.Filters;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.wecare
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class WecareController : ApiController
    {

        [HttpGet]
        [Route("WeSchool/WeCareNewQuery/{studentcode}")]
        public wecaredetail Wecaredisplay(int studentcode)
        {
            wecaredetail Detail = new wecaredetail();

            List<wecarepassparameter> wecare = new List<wecarepassparameter>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_wecare_getdetails", con))
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
                            var slist = new wecarepassparameter();
                            slist.studentcode = studentcode;
                            slist.admissionno = Convert.ToString(dtrow["admissionno"]); ;
                            slist.name = Convert.ToString(dtrow["name"]);
                            slist.mobile = Convert.ToString(dtrow["mobile"]);
                            slist.email = Convert.ToString(dtrow["email"]);
                            slist.password = Convert.ToString(dtrow["password"]);
                            slist.specialization = Convert.ToString(dtrow["specialization"]);
                            wecare.Add(slist);

                        }
                        Detail.Wecaredet = wecare;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }

                }
                
            }

            return Detail;
        }
    }
}
