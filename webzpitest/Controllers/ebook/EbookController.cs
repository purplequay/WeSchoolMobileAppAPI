using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.ebook;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;
namespace webzpitest.Controllers.ebook
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class EbookController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/Ebook")]
        public ebooksdetail ebookdisplay()
        {
            ebooks ebooklist = new ebooks();
            List<ebooks> ebookbindlist = new List<ebooks>();
            ebooksdetail Detils = new ebooksdetail();
            var ebookRequestmessage = Request.Content.ReadAsStringAsync();
            ebookrequest ebookRequest = JsonConvert.DeserializeObject<ebookrequest>(ebookRequestmessage.Result.ToString());
            if (ebookRequestmessage.Result.ToString() != null && ebookRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_ebook_display_semwise", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@StudentCode", SqlDbType.NVarChar).Value = ebookRequest.studentcode;
                        cmd.Parameters.Add("@Semcode", SqlDbType.NVarChar).Value = ebookRequest.semcode;
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            var slist = new ebooks();
                            slist.subcode = Convert.ToString(rdr["code"]);
                            slist.subjectname = Convert.ToString(rdr["name"]);
                            slist.semestername = Convert.ToString(rdr["semestername"]);
                            slist.ebooklink = Convert.ToString(rdr["ebooklink"]);
                            ebookbindlist.Add(slist);
                        }
                        con.Close();
                    }
                    Detils.ebooksdet = ebookbindlist;
                }
                return Detils;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
