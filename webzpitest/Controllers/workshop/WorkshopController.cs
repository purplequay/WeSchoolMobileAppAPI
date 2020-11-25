using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.workshop;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.workshop
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class WorkshopController : ApiController
    {
        [HttpGet]
        [Route("WeSchool/WorkshopCity")]
        public citydetails bindworkshopcity()
        {
            List<bindcity> citylist = new List<bindcity>();
            citydetails details = new citydetails();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("select Code,DESCN  from codemaster where TYPE='loc'", con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new bindcity();
                            slist.citycode = Convert.ToInt32(dtrow["code"]);
                            slist.cityname = Convert.ToString(dtrow["DESCN"]);
                            citylist.Add(slist);
                        }
                        details.citydetail = citylist;
                    }
                }
            }
            return details;
        }

        [HttpGet]
        [Route("WeSchool/WorkshopCampaign/{studentcode}")]
        public campaigndetails campaigndet(int studentcode)
        {
            campaigndetails Detail = new campaigndetails();
            List<campaign> campaignlist = new List<campaign>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_Validate_eventbatch", con))
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
                            var slist = new campaign();
                            slist.activity = Convert.ToString(dtrow["activity"]);
                            slist.msg = Convert.ToString(dtrow["msg"]);
                            slist.campaignname = Convert.ToString(dtrow["campaignname"]);
                            slist.campaignstartdate = Convert.ToString(dtrow["campaignstartdate"]);
                            slist.campaignenddate = Convert.ToString(dtrow["campaignenddate"]);
                            slist.eventcode = Convert.ToString(dtrow["eventcode"]);
                            campaignlist.Add(slist);
                        }
                        Detail.campaigndetail = campaignlist;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
            }
            return Detail;
        }

        //[HttpPost]
        //[Route("WeSchool/WorkshopsSpecialEvents")]
        //public specialeventsdetails speceleventsdetailsdisplay()
        //{
        //    specialeventsdetails Detail = new specialeventsdetails();
        //    List<specialevents> specialeventslist = new List<specialevents>();
        //    var specialeventsRequestmessage = Request.Content.ReadAsStringAsync();
        //    specialeventsrequest eventrequest = JsonConvert.DeserializeObject<specialeventsrequest>(specialeventsRequestmessage.Result.ToString());
        //    if (specialeventsRequestmessage.Result.ToString() != null && specialeventsRequestmessage.Result.ToString() != "")
        //    {
        //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("SP_Avalable_Event", con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@CampCode", SqlDbType.NVarChar).Value = eventrequest.eventcode;
        //                cmd.Parameters.Add("@StudentCode", SqlDbType.Int).Value = eventrequest.studentcode;
        //                cmd.Parameters.Add("@locationcode", SqlDbType.Int).Value = eventrequest.citycode;
        //                SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //                DataTable dt = new DataTable();
        //                sda.Fill(dt);
        //                if (dt.Rows.Count > 0)
        //                {
        //                    foreach (DataRow dtrow in dt.Rows)
        //                    {
        //                        var slist = new specialevents();

        //                        slist.refcode = Convert.ToInt32(dtrow["Code"]);
        //                        slist.eventname = Convert.ToString(dtrow["EventName"]);
        //                        slist.eventdetaillink = Convert.ToString(dtrow["Hyperlink"]);
        //                        slist.eventdate = Convert.ToString(dtrow["edate"]);
        //                        slist.eventtime = Convert.ToString(dtrow["etime"]);
        //                        slist.lastdateofregistration = Convert.ToString(dtrow["date"]);
        //                        slist.maxmarks = Convert.ToString(dtrow["Maxmarks"]);
        //                        slist.Amount = Convert.ToInt32(dtrow["Amount"]);
        //                        specialeventslist.Add(slist);
        //                    }
        //                    Detail.specialeventsdetail = specialeventslist;
        //                    return Detail;
        //                }
        //                else
        //                {
        //                    throw new HttpResponseException(HttpStatusCode.NoContent);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        throw new HttpResponseException(HttpStatusCode.BadRequest);
        //    }
        //}


        [HttpPost]
        [Route("WeSchool/WorkshopsSpecialEvents")]
        public specialeventsdetails speceleventsdetailsdisplay()
        {
            specialeventsdetails Detail = new specialeventsdetails();
            List<specialevents> specialeventslist = new List<specialevents>();
            var specialeventsRequestmessage = Request.Content.ReadAsStringAsync();
            specialeventsrequest eventrequest = JsonConvert.DeserializeObject<specialeventsrequest>(specialeventsRequestmessage.Result.ToString());
            if (specialeventsRequestmessage.Result.ToString() != null && specialeventsRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Avalable_Event", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CampCode", SqlDbType.NVarChar).Value = eventrequest.eventcode;
                        cmd.Parameters.Add("@StudentCode", SqlDbType.Int).Value = eventrequest.studentcode;
                        cmd.Parameters.Add("@locationcode", SqlDbType.Int).Value = eventrequest.citycode;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new specialevents();
                                int refcode = Convert.ToInt32(dtrow["Code"]);

                                using (SqlCommand cmd1 = new SqlCommand("Sp_Capacity_Elm", con))
                                {
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add("@EventCode", SqlDbType.Int).Value = refcode;
                                    cmd1.Parameters.Add("@CenterCode", SqlDbType.Int).Value = 1;
                                    SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                                    DataTable dt1 = new DataTable();
                                    sda1.Fill(dt1);
                                    if (dt1.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow1 in dt1.Rows)
                                        {
                                            int filled = Convert.ToInt32(dtrow1["filled"]);
                                            int capacity = Convert.ToInt32(dtrow1["capacity"]);
                                            if(filled < capacity)
                                            {
                                                //slist.allowselect = "True";
                                                slist.refcode = Convert.ToInt32(dtrow["Code"]);

                                                if (slist.refcode == 514)
                                                {
                                                    slist.allowselect = "false";
                                                    slist.msghead = "Leadership Trek To Kumta May 2019";
                                                    slist.msg = "For Registration And Fee Payment following are the details : Online Transfer- Bank of India, Sion Branch, Account No-004120110000174,IFSC code- BKID0000041 Or Please contact Prof. Charuhas Joshi 9820050294,Mr. Sushil Desai 9869195073";

                                                }
                                                else
                                                {
                                                    slist.allowselect = "True";
                                                }
                                                slist.eventname = Convert.ToString(dtrow["EventName"]);
                                                slist.eventdetaillink = Convert.ToString(dtrow["Hyperlink"]);
                                                slist.eventdate = Convert.ToString(dtrow["edate"]);
                                                slist.eventtime = Convert.ToString(dtrow["etime"]);
                                                slist.lastdateofregistration = Convert.ToString(dtrow["date"]);
                                                slist.maxmarks = Convert.ToString(dtrow["Maxmarks"]);
                                                slist.Amount = Convert.ToInt32(dtrow["Amount"]);
                                                slist.RequestFor = "SPECIALEVENT";
                                                slist.otherparam2 = "1";
                                                slist.otherparam1 = Convert.ToString(dtrow["Code"]);  
                                                
                                            }
                                        }
                                    }
                                }                               
                                specialeventslist.Add(slist);
                            }
                            Detail.specialeventsdetail = specialeventslist;
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


        [HttpGet]
        [Route("WeSchool/RegisteredEvents/{studentcode}")]
        public RegisteredEventsdetails RegisteredEventbind(int studentcode)
        {
            RegisteredEventsdetails Detail = new RegisteredEventsdetails();
            List<RegisteredEvents> RegEventlist = new List<RegisteredEvents>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("selectpaidevents", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Studentcode", SqlDbType.Int).Value = studentcode;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new RegisteredEvents();
                            slist.refcode = Convert.ToInt32(dtrow["Code"]);
                            slist.eventname = Convert.ToString(dtrow["EventName"]);
                            slist.eventdetaillink = Convert.ToString(dtrow["Hyperlink"]);
                            slist.eventdate = Convert.ToString(dtrow["edate"]);                         
                            slist.maxmarks = Convert.ToString(dtrow["Maxmarks"]);
                            slist.Amount = Convert.ToInt32(dtrow["Amount"]);
                            RegEventlist.Add(slist);
                        }
                        Detail.RegisteredEventsdetail = RegEventlist;
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
        [Route("WeSchool/WorkshopTermsCondition")]
        public WorkshopTandCdetails WorkshopTCbind()
        {
            List<WorkshopTandC> TClist = new List<WorkshopTandC>();
            WorkshopTandCdetails Detils = new WorkshopTandCdetails();
             var slist = new WorkshopTandC();
            slist.TermsandConditionlink="http://courses.welingkaronline.org/campaign/Event_termsandcondition.aspx";
             TClist.Add(slist);
             Detils.WorkshopsTandCdetail = TClist;           
            return Detils;
        }
    }
}

