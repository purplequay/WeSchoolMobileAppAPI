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
using webzpitest.Filters;
//using System.Web.Http.Cors;
namespace webzpitest.Controllers.placement
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class PlacementController : ApiController
    {
        [HttpGet]
        [Route("WeSchool/PlacementCompanyCount/{specicode}")]
        public placementcompanywisedetail placementcompanywise(int specicode)
        {
            placementcompanywisedetail Detail = new placementcompanywisedetail();
            List<placementcompanywise> placementcompanylist = new List<placementcompanywise>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["placementscon"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_CountbyCompanyName_Speci", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Speci", SqlDbType.Int).Value = specicode;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new placementcompanywise();
                            slist.specicode = specicode;
                            slist.company = Convert.ToString(dtrow["Company"]);
                            slist.totalcount = Convert.ToString(dtrow["totalcount"]);
                            placementcompanylist.Add(slist);
                        }
                        Detail.placementcompanywisedetails = placementcompanylist;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
            }
            return Detail;
        }

        [HttpGet]
        [Route("WeSchool/PlacementIndustryCount/{speccode}")]
        public placementindustrywisedetail placementindustrywise(int speccode)
        {
            placementindustrywisedetail Detail = new placementindustrywisedetail();
            List<placementindustrywise> placementindustrylist = new List<placementindustrywise>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["placementscon"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_CountOfDataEntryIndustrywise_speci", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Speci", SqlDbType.Int).Value = speccode;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new placementindustrywise();
                            slist.specicode = speccode;
                            slist.industry = Convert.ToString(dtrow["Industry"]);
                            slist.industrycode = Convert.ToInt32(dtrow["industrycode"]);
                            slist.totalcount = Convert.ToString(dtrow["totalcount"]);
                            placementindustrylist.Add(slist);
                        }
                        Detail.placementindustrywisedetails = placementindustrylist;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
            }
            return Detail;
        }

        [HttpGet]
        [Route("WeSchool/PlacementStateCount/{spcode}")]
        public placementstatewisedetail placementstatewise(int spcode)
        {
            placementstatewisedetail Detail = new placementstatewisedetail();
            List<placementstatewise> placementstatelist = new List<placementstatewise>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["placementscon"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_CountOfDataEntryStatewise_Speci", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Speci", SqlDbType.Int).Value = spcode;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new placementstatewise();
                            slist.specicode = spcode;
                            slist.state = Convert.ToString(dtrow["State"]);
                            slist.statecode = Convert.ToInt32(dtrow["statecode"]);
                            slist.totalcount = Convert.ToString(dtrow["totalcount"]);
                            placementstatelist.Add(slist);
                        }
                        Detail.placementstatewisedetails = placementstatelist;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
            }
            return Detail;
        }

        [HttpGet]
        [Route("WeSchool/PlacementAdS/{refno}")]
        public placementadsdetail placementadsdisplay(int refno)
        {
            List<placementads> placementadslist = new List<placementads>();
            placementadsdetail Detail = new placementadsdetail();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["placementscon"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_Placement_Details", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Refno", SqlDbType.Int).Value = refno;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new placementads();
                            slist.refno = refno;
                            slist.sources = Convert.ToString(dtrow["Sources"]);
                            slist.reference = Convert.ToString(dtrow["Reference"]);
                            slist.page = Convert.ToString(dtrow["Page"]);
                            slist.specialization = Convert.ToString(dtrow["Specialization"]);
                            slist.organization = Convert.ToString(dtrow["Organization"]);
                            slist.industry = Convert.ToString(dtrow["Industry"]);
                            slist.managementlevel = Convert.ToString(dtrow["Managementlevel"]);
                            slist.designation = Convert.ToString(dtrow["Designation"]);
                            slist.location = Convert.ToString(dtrow["Location"]);
                            slist.locationstate = Convert.ToString(dtrow["LocationState"]);
                            slist.numberofopenings = Convert.ToString(dtrow["NumberofOpenings"]);
                            slist.jobprofile = Convert.ToString(dtrow["JobProfile"]);
                            slist.qualifictionrequired = Convert.ToString(dtrow["Qualifictionrequired"]);
                            slist.experiencerequired = Convert.ToString(dtrow["ExperienceRequired"]);
                            slist.compensation = Convert.ToString(dtrow["Compensation"]);
                            slist.agelimit = Convert.ToString(dtrow["agelimit"]);
                            slist.addresscommunicationto = Convert.ToString(dtrow["AddressCommunicationto"]);
                            slist.address1 = Convert.ToString(dtrow["Address1"]);
                            slist.address2 = Convert.ToString(dtrow["Address2"]);
                            slist.address3 = Convert.ToString(dtrow["Address3"]);
                            slist.pincode = Convert.ToString(dtrow["pincode"]);
                            slist.city = Convert.ToString(dtrow["city"]);
                            slist.state = Convert.ToString(dtrow["state"]);
                            slist.email = Convert.ToString(dtrow["EMail"]);
                            slist.website = Convert.ToString(dtrow["website"]);
                            slist.contactnumber = Convert.ToString(dtrow["ContactNumber"]);
                            slist.contactnumber2 = Convert.ToString(dtrow["ContactNumber2"]);
                            slist.mobilenumber = Convert.ToString(dtrow["MobileNumber"]);
                            slist.fax = Convert.ToString(dtrow["fax"]);
                            slist.remarks = Convert.ToString(dtrow["remarks"]);
                            slist.advtdate = Convert.ToString(dtrow["AdvtDate"]);
                            slist.applybefore = Convert.ToString(dtrow["Applybefore"]);
                            slist.walkIninterviewat = Convert.ToString(dtrow["WalkInInterviewat"]);
                            placementadslist.Add(slist);
                        }
                        Detail.placementadsdetails = placementadslist;                       
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                    return Detail;
                }
            }           
        }

        [HttpGet]
        public List<placementstudentsearch> placementstudentsearchdisplay(int studentcode, int specicode)
        {
            List<placementstudentsearch> studentsearchdisplaylist = new List<placementstudentsearch>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["placementscon"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_Student_Search1", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = studentcode;
                    cmd.Parameters.Add("@specicode", SqlDbType.Int).Value = specicode;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var slist = new placementstudentsearch();
                        slist.studentcode = studentcode;
                        slist.specicode = specicode;
                        slist.refno = Convert.ToInt32(rdr["refno"]);
                        slist.advtDate = Convert.ToString(rdr["AdvtDate"]);
                        slist.companyname = Convert.ToString(rdr["CompanyName"]);
                        slist.designation = Convert.ToString(rdr["Designation"]);
                        slist.experiencerequired = Convert.ToString(rdr["ExperienceRequired"]);
                        slist.sources = Convert.ToString(rdr["sources"]);
                        slist.datetimeplaceofwalkIninterview = Convert.ToString(rdr["DateTimePlaceOfWalkInInterview"]);
                        slist.communicatetoname = Convert.ToString(rdr["CommunicateToName"]);
                        slist.email = Convert.ToString(rdr["EMailAddress"]);
                        slist.mobile = Convert.ToString(rdr["Mobile"]);
                        slist.lastdate = Convert.ToString(rdr["LastDate"]);
                        studentsearchdisplaylist.Add(slist);
                    }
                    con.Close();
                }
            }
            return studentsearchdisplaylist;
        }
    }
}
