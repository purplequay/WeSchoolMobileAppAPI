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
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class PlacementcController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/CompanyPlacement")]
        public companylistdisplaydetail companyplacementdisplay() 
        {
            companylistdisplaydetail Detail = new companylistdisplaydetail();
            List<companylistdisplay> companydisplaylist = new List<companylistdisplay>();
            var companylistdisplayRequestmessage = Request.Content.ReadAsStringAsync();
            companylistrequest cplacementrequest = JsonConvert.DeserializeObject<companylistrequest>(companylistdisplayRequestmessage.Result.ToString());
            if (companylistdisplayRequestmessage.Result.ToString() != null && companylistdisplayRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["placementscon"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_Placement_Display_Student_companylist", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar).Value = cplacementrequest.companyname;
                        cmd.Parameters.Add("@Specicode", SqlDbType.Int).Value = cplacementrequest.specicode;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new companylistdisplay();
                                slist.specicode = cplacementrequest.specicode;
                                slist.refno = Convert.ToInt32(dtrow["refno"]);
                                slist.advertisedate = Convert.ToString(dtrow["AdvtDate"]);
                                slist.companyname = Convert.ToString(dtrow["CompanyName"]);
                                slist.designation = Convert.ToString(dtrow["Designation"]);
                                slist.location = Convert.ToString(dtrow["Location"]);
                                slist.lastDateForApplying = Convert.ToString(dtrow["LastDateForApplying"]);
                                companydisplaylist.Add(slist);
                            }
                            Detail.companylistdisplaydetails = companydisplaylist;
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
