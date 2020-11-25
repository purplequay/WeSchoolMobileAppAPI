using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.notice;
using webzpitest.Filters;
using Newtonsoft.Json;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.notice
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class NoticeController : ApiController
    {
         [HttpGet]
         [Route("WeSchool/NoticeHeading/{studentcode}")]
         public noticedetail notice(int studentcode)
         {
             noticedetail Detail = new noticedetail();
             List<notices> noticeslist = new List<notices>();
             using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
             {
                 using (SqlCommand cmd = new SqlCommand("usp_api_notices_LinkHeading", con))
                 {
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value =studentcode ;
                     SqlDataAdapter sda = new SqlDataAdapter(cmd);
                     DataTable dt = new DataTable();
                     sda.Fill(dt);
                     if (dt.Rows.Count > 0)
                     {
                         foreach (DataRow dtrow in dt.Rows)
                         {
                             var slist = new notices();
                             slist.linkheading = Convert.ToString(dtrow["LinkHeading"]);                           
                             noticeslist.Add(slist);
                         }
                         Detail.noticeheading = noticeslist;
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
         [Route("WeSchool/Noticenames")]
         public noticenamesedetail noticenames()
         {
             noticenamesedetail Detail = new noticenamesedetail();
             List<noticenames> noticelist = new List<noticenames>();
             var noticenamesRequestmessage = Request.Content.ReadAsStringAsync();
             noticenamesrequest noticenamerequest = JsonConvert.DeserializeObject<noticenamesrequest>(noticenamesRequestmessage.Result.ToString());
             if (noticenamesRequestmessage.Result.ToString() != null && noticenamesRequestmessage.Result.ToString() != "")
             {
                 using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                 {
                     using (SqlCommand cmd = new SqlCommand("usp_api_notices_LinkHeading_noticename", con))
                     {
                         cmd.CommandType = CommandType.StoredProcedure;
                         cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = noticenamerequest.studentcode;
                         cmd.Parameters.Add("@linkheading", SqlDbType.NVarChar).Value = noticenamerequest.linkheading;
                         SqlDataAdapter sda = new SqlDataAdapter(cmd);
                         DataTable dt = new DataTable();
                         sda.Fill(dt);
                         if (dt.Rows.Count > 0)
                         {
                             foreach (DataRow dtrow in dt.Rows)
                             {
                                 var slist = new noticenames();
                                 slist.linkheading = Convert.ToString(dtrow["LinkHeading"]);
                                 slist.noticename = Convert.ToString(dtrow["noticename"]);
                                 noticelist.Add(slist);
                             }
                             Detail.noticenamedet = noticelist;
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

         [HttpPost]
         [Route("WeSchool/NoticeAll")]
         public noticealldetail noticeall()
         {
             noticealldetail Detail = new noticealldetail();
             List<noticeall> noticealllist = new List<noticeall>();
             var noticeallRequestmessage = Request.Content.ReadAsStringAsync();
             noticeallrequest noticeallrequest = JsonConvert.DeserializeObject<noticeallrequest>(noticeallRequestmessage.Result.ToString());
             if (noticeallRequestmessage.Result.ToString() != null && noticeallRequestmessage.Result.ToString() != "")
             {
                 using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                 {
                     using (SqlCommand cmd = new SqlCommand("usp_api_notices_noticefiles", con))
                     {
                         cmd.CommandType = CommandType.StoredProcedure;
                         cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = noticeallrequest.studentcode;
                         cmd.Parameters.Add("@linkheading", SqlDbType.NVarChar).Value = noticeallrequest.linkheading;
                         cmd.Parameters.Add("@noticename", SqlDbType.NVarChar).Value = noticeallrequest.noticename;
                         SqlDataAdapter sda = new SqlDataAdapter(cmd);
                         DataTable dt = new DataTable();
                         sda.Fill(dt);
                         if (dt.Rows.Count > 0)
                         {
                             foreach (DataRow dtrow in dt.Rows)
                             {
                                 var slist = new noticeall();
                                 slist.linkheading = Convert.ToString(dtrow["LinkHeading"]);
                                 slist.noticename = Convert.ToString(dtrow["noticename"]);
                                 slist.linkname = Convert.ToString(dtrow["linkName"]);
                                 slist.filename = Convert.ToString(dtrow["fileName"]);
                                 noticealllist.Add(slist);
                             }
                             Detail.noticeall = noticealllist;
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
   
                   