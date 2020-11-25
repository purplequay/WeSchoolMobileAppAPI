
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
    public class ExamController : ApiController
    {
        

         [HttpGet]
         [Route("WeSchool/Syllabus/{stucode}")]
         public studentsyllabus syllabus(int stucode)
         {
             studentsyllabus Detail = new studentsyllabus();
             syllabusdet syllabusdetail = new syllabusdet();
             using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
             {
                 using (SqlCommand cmd = new SqlCommand("usp_api_StudentSyllabus", con))
                 {
                     con.Open();
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = stucode;
                     SqlDataAdapter sda = new SqlDataAdapter(cmd);
                     DataTable dt = new DataTable();
                     sda.Fill(dt);
                     if (dt.Rows.Count > 0)
                     {
                         foreach (DataRow dtrow in dt.Rows)
                         {
                             syllabusdetail.syllabuslink = Convert.ToString(dtrow["Syllabuslink"]);
                         }
                         Detail.stusyllabus = syllabusdetail;
                     }
                     else
                     {
                         throw new HttpResponseException(HttpStatusCode.NoContent);
                     }
                 }
                 return Detail;
             }
         }


         [HttpGet]
         [Route("WeSchool/Guideline/{studentcode}")]
         public Guidelinesnames Guidelinename(int studentcode)
         {
             Guidelinesnames Detail = new Guidelinesnames();
             List<Guidelines> guidelinelist = new List<Guidelines>();

             using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
             {
                 using (SqlCommand cmd = new SqlCommand("usp_api_Exam_Guidelines_noticename", con))
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
                             var slist = new Guidelines();
                             slist.guidelinecode = Convert.ToInt32(dtrow["guidelinecode"]);
                             slist.guidlinename = Convert.ToString(dtrow["noticename"]);
                             guidelinelist.Add(slist);
                         }
                         Detail.Guidelinesname = guidelinelist;
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
         [Route("WeSchool/GuidelineDetail")]
         public Guidelinesdetail Displayguidelines()
         {
             Guidelinesdetail Detail = new Guidelinesdetail();
             List<Guidelineall> guidelinelist = new List<Guidelineall>();
             var guidelineRequestmessage = Request.Content.ReadAsStringAsync();
             Guidelinesrequest guidelinerequest = JsonConvert.DeserializeObject<Guidelinesrequest>(guidelineRequestmessage.Result.ToString());
             if (guidelineRequestmessage.Result.ToString() != null && guidelineRequestmessage.Result.ToString() != "")
             {
                 using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                 {
                     using (SqlCommand cmd = new SqlCommand("usp_api_Exam_Guidelines", con))
                     {
                         cmd.CommandType = CommandType.StoredProcedure;
                         cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = guidelinerequest.studentcode;
                         cmd.Parameters.Add("@guidelinecode", SqlDbType.NVarChar).Value = guidelinerequest.guidelinecode;
                         SqlDataAdapter sda = new SqlDataAdapter(cmd);
                         DataTable dt = new DataTable();
                         sda.Fill(dt);
                         if (dt.Rows.Count > 0)
                         {
                             foreach (DataRow dtrow in dt.Rows)
                             {
                                 var slist = new Guidelineall();
                                 slist.guidelinecode = Convert.ToInt32(dtrow["guidelinecode"]);
                                 slist.guidlinename = Convert.ToString(dtrow["noticename"]);
                                 slist.linkname = Convert.ToString(dtrow["linkName"]);
                                 slist.filename = Convert.ToString(dtrow["filename"]);
                                 guidelinelist.Add(slist);
                             }
                             Detail.Guidelinesdetails = guidelinelist;
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
