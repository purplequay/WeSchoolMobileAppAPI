using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.placement;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.placement
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class PlacementiController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/IndustryPlacement")]
        public industrylistdisplaydetail industryplacementdisplay()
        {
            industrylistdisplaydetail Detail = new industrylistdisplaydetail();
            List<industrylistdisplay> industrydisplaylist = new List<industrylistdisplay>();
            var industrylistdisplayRequestmessage = Request.Content.ReadAsStringAsync();
            industrylistrequest iplacementrequest = JsonConvert.DeserializeObject<industrylistrequest>(industrylistdisplayRequestmessage.Result.ToString());
            if (industrylistdisplayRequestmessage.Result.ToString() != null && industrylistdisplayRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["placementscon"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_Placement_Display_Student_industrylist", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@industrycode", SqlDbType.Int).Value = iplacementrequest.industrycode;
                        cmd.Parameters.Add("@Specicode", SqlDbType.Int).Value = iplacementrequest.specicode;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new industrylistdisplay();
                                slist.specicode = iplacementrequest.specicode;
                                slist.industrycode = iplacementrequest.industrycode;
                                slist.refno = Convert.ToInt32(dtrow["refno"]);
                                slist.advertisedate = Convert.ToString(dtrow["AdvtDate"]);
                                slist.companyname = Convert.ToString(dtrow["CompanyName"]);
                                slist.designation = Convert.ToString(dtrow["Designation"]);
                                slist.location = Convert.ToString(dtrow["Location"]);
                                slist.lastDateForApplying = Convert.ToString(dtrow["LastDateForApplying"]);
                                industrydisplaylist.Add(slist);
                            }
                            Detail.industrylistdisplaydetails = industrydisplaylist;
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
