
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.exam;
using webzpitest.Filters;
using Newtonsoft.Json;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.exam
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class TimetableController : ApiController
    {
        [HttpGet]
        [Route("WeSchool/Timetable/{studentcodes}")]
        public Timetablenames Timetablename(int studentcodes)
        {
            Timetablenames Detail = new Timetablenames();
            List<Timetable> timetablelist = new List<Timetable>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_Exam_Timetable_noticename", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = studentcodes;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new Timetable();
                            slist.timetablecode = Convert.ToInt32(dtrow["guidelinecode"]);
                            slist.timetablename = Convert.ToString(dtrow["noticename"]);
                            timetablelist.Add(slist);
                        }
                        Detail.Timetablename = timetablelist;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
            }
            return Detail;
        }

        [HttpPost]
        [Route("WeSchool/TimetableDetail")]
        public Timetabledetail Displaytimetable()
        {
            Timetabledetail Detail = new Timetabledetail();
            List<Timetableall> timetablelist = new List<Timetableall>();
            var timetableRequestmessage = Request.Content.ReadAsStringAsync();
            Timetablesrequest timetablerequest = JsonConvert.DeserializeObject<Timetablesrequest>(timetableRequestmessage.Result.ToString());
            if (timetableRequestmessage.Result.ToString() != null && timetableRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_Exam_Timetable", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = timetablerequest.studentcode;
                        cmd.Parameters.Add("@guidelinecode", SqlDbType.NVarChar).Value = timetablerequest.timetablecode;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new Timetableall();
                                slist.timetablecode = Convert.ToInt32(dtrow["guidelinecode"]);
                                slist.timetablename = Convert.ToString(dtrow["noticename"]);
                                slist.linkname = Convert.ToString(dtrow["linkName"]);
                                slist.filename = Convert.ToString(dtrow["filename"]);
                                timetablelist.Add(slist);
                            }
                            Detail.Timetabledetails = timetablelist;
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
        [Route("WeSchool/PCPVCF/{studentcode}")]
        public PCPVCFDetails PCPVCFbind(int studentcode)
        {
            PCPVCFDetails Detail = new PCPVCFDetails();
            List<PCPVCF> pcpvcflist = new List<PCPVCF>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_PCPVCF_Timetable", con))
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
                            var slist = new PCPVCF();
                            slist.pcbvcfcode = Convert.ToInt32(dtrow["pcpvcfcode"]);
                            slist.linkname = Convert.ToString(dtrow["linkname"]);
                            slist.filename = Convert.ToString(dtrow["filename"]);
                            pcpvcflist.Add(slist);
                        }
                        Detail.PCPVCFDetail = pcpvcflist;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
            }
            return Detail;
        }
    }
}
