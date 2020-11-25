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
    public class EmailController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/EmailAlert")]
        public emailrepdetail Emailreportdisplay()
        {
            List<emailrep> emailreplist = new List<emailrep>();
            emailrepdetail Details = new emailrepdetail();
            var emailrepRequestmessage = Request.Content.ReadAsStringAsync();
            emailrequest emailrepRequest = JsonConvert.DeserializeObject<emailrequest>(emailrepRequestmessage.Result.ToString());
            if (emailrepRequestmessage.Result.ToString() != null && emailrepRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_Webmessage_EMAIL_FromStudent_Select", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = emailrepRequest.studentcode;
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Value = emailrepRequest.fromdate;
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Value = emailrepRequest.todate;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new emailrep();
                                slist.messagetittle = Convert.ToString(dtrow["Messagetittle"]);
                                slist.posteddate = Convert.ToString(dtrow["PostedDate"]);
                                slist.EmailContent = Convert.ToString(dtrow["EmailContent"]);
                                slist.remarks = Convert.ToString(dtrow["remarks"]);
                                emailreplist.Add(slist);
                            }
                            Details.emailrepdetails = emailreplist;
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
