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
    public class FinalYearProjectController : ApiController
    {
        [HttpGet]
        [Route("WeSchool/FinalProjectLinks/{studentcode}")]
        public finalyearprojlinkdetails FinalYearProjectLinks(int studentcode)
        {
            List<finalyearprojlink> fYprojectlinklist = new List<finalyearprojlink>();
            finalyearprojlinkdetails Details = new finalyearprojlinkdetails();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_Finalyearproject_noticelist", con))
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
                            var slist = new finalyearprojlink();
                            slist.linkname = Convert.ToString(dtrow["linkname"]);
                            fYprojectlinklist.Add(slist);
                        }
                        Details.fyprojlinknames = fYprojectlinklist;
                        return Details;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
            }
        }

        [HttpPost]
        [Route("WeSchool/FinalProjectLinkheading")]
        public finalyearprojdetails finalyearprojdetail()
        {
            finalyearprojdetails Detail = new finalyearprojdetails();
            List<finalyearprojlist> finalyearprojlists = new List<finalyearprojlist>();
            var fyprojRequestmessage = Request.Content.ReadAsStringAsync();
            finalyearprojrequest fyprojrequest = JsonConvert.DeserializeObject<finalyearprojrequest>(fyprojRequestmessage.Result.ToString());
            if (fyprojRequestmessage.Result.ToString() != null && fyprojRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_Finalyearproject_noticelist_linkname", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = fyprojrequest.studentcode;
                        cmd.Parameters.Add("@linkname", SqlDbType.NVarChar).Value = fyprojrequest.linkname;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new finalyearprojlist();
                                slist.linkheading = Convert.ToString(dtrow["linkheading"]);
                                slist.linkname = Convert.ToString(dtrow["linkname"]);
                                slist.filename = Convert.ToString(dtrow["filename"]);
                                finalyearprojlists.Add(slist);
                            }
                            Detail.fyprojlinkheadings = finalyearprojlists;
                            return Detail;
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
