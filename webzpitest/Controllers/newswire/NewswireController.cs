using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.newswire;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.newswire
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class NewswireController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/Newswirespecialization")]
        public newswirespecialization newswirespecializationdisplay()
        {
            List<newswirecatagory> newswirecatagorylist = new List<newswirecatagory>();
            newswirespecialization Detils = new newswirespecialization();
            var newswirecatagoryRequestmessage = Request.Content.ReadAsStringAsync();
            newswirecatagoryrequest newswireRequest = JsonConvert.DeserializeObject<newswirecatagoryrequest>(newswirecatagoryRequestmessage.Result.ToString());
            if (newswirecatagoryRequestmessage.Result.ToString() != null && newswirecatagoryRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["newswirecon"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_newswire_specialisation", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SPcode", SqlDbType.Int).Value = newswireRequest.SpeciCode;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new newswirecatagory();
                                slist.Code = Convert.ToInt16(dtrow["code"].ToString());
                                slist.SpecName = dtrow["name"].ToString();
                                newswirecatagorylist.Add(slist);
                            }
                            Detils.newswiresspecialization = newswirecatagorylist;
                            return Detils;
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

        [HttpPost]
        [Route("WeSchool/NewswireDetail")]
        public newswirestdetail newswiredetailsdisplay()
        {
            List<newswires> newswirelist = new List<newswires>();
            newswirestdetail Details = new newswirestdetail();
            var newswireRequestmessage = Request.Content.ReadAsStringAsync();
            newswirerequest newswireRequest = JsonConvert.DeserializeObject<newswirerequest>(newswireRequestmessage.Result.ToString());
            if (newswireRequestmessage.Result.ToString() != null && newswireRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["newswirecon"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_searchstudent", con))
                    {                       
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@speci", SqlDbType.Int).Value = newswireRequest.SpeciCode;
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Value = newswireRequest.fromdate;
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Value = newswireRequest.todate;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new newswires();
                                slist.Code = Convert.ToInt32(dtrow["codeid"].ToString());
                                slist.Msghead = dtrow["msghead"].ToString();
                                slist.DateofRef = dtrow["dateofref"].ToString();
                                slist.Newswirelink = dtrow["Newswirelink"].ToString();
                                newswirelist.Add(slist);                               
                            }
                            Details.newswiredetail = newswirelist;
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
