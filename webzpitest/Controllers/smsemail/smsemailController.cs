using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.smsemail;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.smsemail
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
     [APIAuthorizeAttribute]
    public class smsemailController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/SMSAlert")]
        public smsrepdetail smsreportdisplay()
        {
            List<smsrep> smsreplist = new List<smsrep>();
            smsrepdetail Details = new smsrepdetail();
            var smsrepRequestmessage = Request.Content.ReadAsStringAsync();
            smsrequest smsrepRequest = JsonConvert.DeserializeObject<smsrequest>(smsrepRequestmessage.Result.ToString());
            if (smsrepRequestmessage.Result.ToString() != null && smsrepRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_Webmessage_SMS_Select", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = smsrepRequest.studentcode;
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Value = smsrepRequest.fromdate;
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Value = smsrepRequest.todate;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new smsrep();
                                slist.messagetittle = Convert.ToString(dtrow["Messagetittle"]);
                                slist.posteddate = Convert.ToString(dtrow["PostedDate"]);
                                slist.smscontent = Convert.ToString(dtrow["SMScontent"]);
                                slist.remarks = Convert.ToString(dtrow["remarks"]);
                                smsreplist.Add(slist);
                            }
                            Details.smsrepdetails = smsreplist;
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
