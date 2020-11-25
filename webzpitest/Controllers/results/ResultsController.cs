using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.Results;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;
namespace webzpitest.Controllers.results
{
    //  [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class ResultsController : ApiController
    {

        [HttpGet]
        [Route("WeSchool/Result/{studentcode}")]
        public Resultsdetail loadResult(int studentcode)
        {
            List<Results> Resultlist = new List<Results>();
            Resultsdetail Details = new Resultsdetail();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_Result", con))
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
                            var slist = new Results();
                            slist.midterm = Convert.ToString(dtrow["MidTerm"]);
                            slist.endterm = Convert.ToString(dtrow["EndTerm"]);
                            slist.studentcode = Convert.ToInt32(dtrow["studentcode"]);
                            slist.admissionno = Convert.ToString(dtrow["admissionno"]);
                            slist.batchname = Convert.ToString(dtrow["batchname"]);
                            slist.linkname = Convert.ToString(dtrow["linkname"]);
                            slist.examyearcode = Convert.ToInt16(dtrow["examyear"]);
                            slist.resultview = Convert.ToInt16(dtrow["resultview"]);

                            if (slist.resultview == 0)
                            {                          
                                throw new HttpResponseException(HttpStatusCode.NoContent);
                            }                        

                            Resultlist.Add(slist);
                        }
                        Details.Resultsdetails = Resultlist;
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
