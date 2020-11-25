using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using webzpitest.Models.POV;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;
using System.Web;
using System.Data;
using System.IO;
using webzpitest.Filters;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.POV
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class POVController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/ProgramsVideo")]
        public ProgramVideosDetail POVBind()
        {
            ProgramVideosDetail Detail = new ProgramVideosDetail();
            List<ProgramVideos> Lists = new List<ProgramVideos>();
            var PVRequestmsg = Request.Content.ReadAsStringAsync();
            ProgramVideosRequest pvrequest = JsonConvert.DeserializeObject<ProgramVideosRequest>(PVRequestmsg.Result.ToString());
            if (PVRequestmsg.Result.ToString() != null && PVRequestmsg.Result.ToString() != "")
            {
                MySqlConnection mysqlcon = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
                using (SqlCommand scmd = new SqlCommand("usp_api_povlogin_mobile", con))
                {
                    scmd.CommandType = CommandType.StoredProcedure;
                    scmd.Parameters.Add("@admissionno", SqlDbType.VarChar).Value = pvrequest.admissionno;
                    scmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = pvrequest.studentcode;
                    SqlDataAdapter sqda = new SqlDataAdapter(scmd);
                    DataTable dat = new DataTable();
                    sqda.Fill(dat);
                    if (dat.Rows.Count > 0)
                    {
                        foreach (DataRow datrow in dat.Rows)
                        {
                            var slistt = new POVLogin();
                            slistt.student_id = Convert.ToInt32(datrow["student_id"]);
                            slistt.student_name = Convert.ToString(datrow["student_name"]);
                            slistt.batch_name = Convert.ToString(datrow["batch_name"]);
                            using (MySqlCommand mcmd = new MySqlCommand("select * from students where admission_no='" + pvrequest.admissionno + "' ", mysqlcon))
                            {
                                mcmd.CommandType = CommandType.Text;
                                MySqlDataAdapter msda = new MySqlDataAdapter(mcmd);
                                DataTable mdt = new DataTable();
                                msda.Fill(mdt);
                                if (mdt.Rows.Count > 0)
                                {
                                    //mysqlcon.Open();
                                    //MySqlCommand mcmdu = new MySqlCommand("update students set first_login='No',modified=now() where admission_no='" +
                                    //    pvrequest.admissionno + "' ", mysqlcon);
                                    //mcmdu.ExecuteNonQuery();
                                    //mysqlcon.Close();
                                }
                                else
                                {
                                    mysqlcon.Open();
                                    string str = @"insert into students (admission_no,student_id,student_name,program_id,program_name,batch_name,first_login,
                                                 pov_completed,created,modified)values('" + pvrequest.admissionno + "'," +
                                                 slistt.student_id + ",'" + slistt.student_name + "',1,'PG Diploma in Management (HB)','" +
                                                 slistt.batch_name + "','Yes',0,Now(),Now())";
                                    MySqlCommand mcmdu = new MySqlCommand(str, mysqlcon);
                                    mcmdu.ExecuteNonQuery();
                                    mysqlcon.Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
                using (MySqlCommand cmd = new MySqlCommand("select * from program_videos", mysqlcon))
                {
                    cmd.CommandType = CommandType.Text;
                    MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        int prev_Videoid=4;
                        string prev_status="Pending";

                        foreach (DataRow dtrow in dt.Rows)
                        {
                            var slist = new ProgramVideos();
                            slist.studentcode = pvrequest.studentcode;
                            slist.video_id = Convert.ToInt32(dtrow["id"]);
                            slist.admissionno = pvrequest.admissionno;
                            slist.videofuration = Convert.ToInt32(dtrow["Video_duration"]);
                            slist.videotitle = Convert.ToString(dtrow["video_title"]);
                            slist.videolink = "http://api.stephenventures.com/videos/" + Convert.ToString(dtrow["video"]);
                            // slist.videolink = Convert.ToString(dtrow["video_link"]);
                            slist.videothumbnail = "http://studentportal.welingkaronline.org/uploads/images/" + Convert.ToString(dtrow["image"]);

                            string qq = @"select sr.student_id as student_id,DATE_FORMAT(sr.completion_date, '%d/%m/%Y') AS CompletionDate, sr.score as score 
                                        from student_reports sr left join student_video_report svr on svr.student_id=sr.student_id 
                                        where svr.admission_no='" + pvrequest.admissionno + "' and sr.video_id=" + slist.video_id + " order by sr.id desc LIMIT 1";
                            MySqlCommand cmd1 = new MySqlCommand(qq, mysqlcon);

                          
                            cmd1.CommandType = CommandType.Text;
                            MySqlDataAdapter sda1 = new MySqlDataAdapter(cmd1);
                            DataTable dt1 = new DataTable();
                            sda1.Fill(dt1);
                            // student_id = Convert.ToInt32(dt1.Rows[0]["student_id"]);
                            if (dt1.Rows.Count > 0)
                            {
                                foreach (DataRow dtrow1 in dt1.Rows)
                                {

                                    slist.student_id = Convert.ToInt32(dtrow1["student_id"]);
                                    slist.status = "Completed";
                                    slist.score = Convert.ToString(dtrow1["score"]);
                                    slist.completiondate = Convert.ToString(dtrow1["CompletionDate"]);
                                    slist.play = "True";
                                    prev_Videoid = slist.video_id;
                                    prev_status = slist.status;
                                }
                            }
                            else
                            {
                                using (MySqlCommand cmd0 = new MySqlCommand("select * from students where admission_no='" + pvrequest.admissionno + "'  order by id desc LIMIT 1", mysqlcon))
                                {
                                    cmd0.CommandType = CommandType.Text;
                                    MySqlDataAdapter sda0 = new MySqlDataAdapter(cmd0);
                                    DataTable dt0 = new DataTable();
                                    sda0.Fill(dt0);
                                    // student_id = Convert.ToInt32(dt1.Rows[0]["student_id"]);
                                    if (dt0.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow0 in dt0.Rows)
                                        {
                                            slist.student_id = Convert.ToInt32(dtrow0["id"]);
                                        }
                                    }
                                }
                                slist.status = "Pending";
                                slist.score = "0";
                                slist.completiondate = "NA";
                                if (slist.video_id == 4)
                                {
                                    slist.play = "True";
                                }
                                else  if(prev_Videoid==4)
                                {
                                   if(prev_status=="Pending")
                                    {
                                        slist.play = "False";
                                    }
                                    else
                                    {
                                        slist.play = "True";
                                    }
                                }
                                else
                                {
                                    if(prev_status=="Pending")
                                    {
                                        slist.play = "False";
                                    }
                                    else
                                    {
                                        slist.play = "True";
                                    }
                                }

                                prev_Videoid = slist.video_id;
                                prev_status = slist.status;

                            }
                          
                            Lists.Add(slist);
                        }
                        Detail.ProgramVideosDetails = Lists;
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
        [Route("WeSchool/POVstatus/{studentcode}")]
        public povstatusdetails POVStatusBind(int studentcode)
        {
            povstatusdetails Detail = new povstatusdetails();
            povcompletestatus Lists = new povcompletestatus();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_pov_completestatus", con))
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
                            Lists.povstatus = Convert.ToString(dtrow["povcompletestatus"]);
                        }
                        Detail.povstatusdetail = Lists;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NoContent);
                    }
                }
                return Detail;
            }
        }


        [HttpPost]
        [Route("WeSchool/POVQuestions")]
        public POVQuestDetail POVQuestionBind()
        {
            POVQuestDetail Detail = new POVQuestDetail();
            List<POVQuestions> Lists = new List<POVQuestions>();
            List<POVOptions> OptonLists = new List<POVOptions>();
            var POVQuestRequestmsg = Request.Content.ReadAsStringAsync();

            POVQuestRequest povquestrequest = JsonConvert.DeserializeObject<POVQuestRequest>(POVQuestRequestmsg.Result.ToString());
            if (POVQuestRequestmsg.Result.ToString() != null && POVQuestRequestmsg.Result.ToString() != "")
            {
                MySqlConnection mysqlcon = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

                string querytotal = @"SELECT  COUNT(*) as totalquestions from questions where program_video_id=" + povquestrequest.video_id + "";

                using (MySqlCommand cmdt = new MySqlCommand(querytotal, mysqlcon))
                {
                    cmdt.CommandType = CommandType.Text;
                    MySqlDataAdapter sdat = new MySqlDataAdapter(cmdt);
                    DataTable dtt = new DataTable();
                    sdat.Fill(dtt);
                    if (dtt.Rows.Count > 0)
                    {
                        foreach (DataRow dttrow in dtt.Rows)
                        {
                            Detail.noofquestions = Convert.ToInt32(dttrow["totalquestions"]);
                            Detail.student_id = povquestrequest.student_id;
                            Detail.video_id = povquestrequest.video_id;
                        }

                        string query = @"SELECT id, REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Replace(REPLACE(REPLACE(REPLACE(REPLACE(
                                 Replace(Replace(REPLACE(REPLACE(REPLACE(question, '<div>', ''),'<p>', ''),'<ol>', ''),
                                '</div>',''),'<li>',''),'</li>',''),'</ol>',''),'</p>',''),'&nbsp;',''),'&ldquo;',''''),
                                '&rsquo;',''''),'&ndash;','-'),'&rdquo;',''''),'&quot;',''''),char(13),''),char(10),'') as question FROM questions 
                                where program_video_id=" + povquestrequest.video_id + "";
                        using (MySqlCommand cmd = new MySqlCommand(query, mysqlcon))
                        {
                            cmd.CommandType = CommandType.Text;
                            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                int questionno = 0;
                                foreach (DataRow dtrow in dt.Rows)
                                {

                                    var slist = new POVQuestions();
                                    slist.questionid = Convert.ToInt32(dtrow["id"]);
                                    slist.question = Convert.ToString(dtrow["question"]);
                                    string query1 = @"SELECT question_id, id, correct_ans,REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Replace(REPLACE(REPLACE(REPLACE
                                            (REPLACE(Replace(Replace(REPLACE(REPLACE(REPLACE(`option`, '<div>', ''),'<p>', ''),'<ol>', ''),
                                            '</div>',''),'<li>',''),'</li>',''),'</ol>',''),'</p>',''),'&nbsp;',''),'&ldquo;',''''),
                                            '&rsquo;',''''),'&ndash;','-'),'&rdquo;',''''),'&quot;',''''),char(13),''),char(10),'') as optionname FROM answers
                                            where `option` is not null and `option` <> '' and question_id=" + slist.questionid + "";
                                    using (MySqlCommand cmd1 = new MySqlCommand(query1, mysqlcon))
                                    {
                                        cmd1.CommandType = CommandType.Text;
                                        MySqlDataAdapter sda1 = new MySqlDataAdapter(cmd1);
                                        DataTable dt1 = new DataTable();
                                        sda1.Fill(dt1);
                                        if (dt1.Rows.Count > 0)
                                        {
                                            OptonLists = new List<POVOptions>();
                                            foreach (DataRow dtrow1 in dt1.Rows)
                                            {
                                                var slists = new POVOptions();
                                                slists.questionid = slist.questionid;
                                                slists.optionid = Convert.ToInt32(dtrow1["id"]);
                                                slists.option = Convert.ToString(dtrow1["optionname"]);
                                                slists.correct_ans = Convert.ToInt32(dtrow1["correct_ans"]);
                                                OptonLists.Add(slists);
                                            }
                                        }
                                        slist.POVOptionsDetails = OptonLists;
                                    }
                                    questionno = questionno + 1;
                                    slist.questionno = questionno;
                                    Lists.Add(slist);
                                }
                                Detail.POVQuestDetails = Lists;
                            }

                            else
                            {
                                throw new HttpResponseException(HttpStatusCode.NoContent);
                            }

                        }
                    }
                }
            }
            return Detail;
        }

        [HttpPost]
        [Route("WeSchool/POVQuizViewAnswers")]
        public POVAnswerDetail POVQuizViewAnswerBind()
        {
            POVAnswerDetail Detail = new POVAnswerDetail();
            List<POVTestQuestionResult> Lists = new List<POVTestQuestionResult>();
            List<POVOptionsResult> OptonLists = new List<POVOptionsResult>();
            var POVQuestRequestmsg = Request.Content.ReadAsStringAsync();
            POVQuestRequest povquestrequest = JsonConvert.DeserializeObject<POVQuestRequest>(POVQuestRequestmsg.Result.ToString());
            if (POVQuestRequestmsg.Result.ToString() != null && POVQuestRequestmsg.Result.ToString() != "")
            {
                MySqlConnection mysqlcon = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
                string querytotal = @"SELECT  COUNT(*) as totalquestions from questions where program_video_id=" + povquestrequest.video_id + "";
                using (MySqlCommand cmdt = new MySqlCommand(querytotal, mysqlcon))
                {
                    cmdt.CommandType = CommandType.Text;
                    MySqlDataAdapter sdat = new MySqlDataAdapter(cmdt);
                    DataTable dtt = new DataTable();
                    sdat.Fill(dtt);
                    if (dtt.Rows.Count > 0)
                    {
                        foreach (DataRow dttrow in dtt.Rows)
                        {
                            Detail.noofquestions = Convert.ToInt32(dttrow["totalquestions"]);
                            Detail.student_id = povquestrequest.student_id;
                            Detail.video_id = povquestrequest.video_id;
                            string query = @"SELECT id, REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Replace(REPLACE(REPLACE(REPLACE(REPLACE(
                                 Replace(Replace(REPLACE(REPLACE(REPLACE(question, '<div>', ''),'<p>', ''),'<ol>', ''),
                                '</div>',''),'<li>',''),'</li>',''),'</ol>',''),'</p>',''),'&nbsp;',''),'&ldquo;',''''),
                                '&rsquo;',''''),'&ndash;','-'),'&rdquo;',''''),'&quot;',''''),char(13),''),char(10),'') as question FROM questions 
                                where program_video_id=" + povquestrequest.video_id + "";
                            using (MySqlCommand cmd = new MySqlCommand(query, mysqlcon))
                            {
                                cmd.CommandType = CommandType.Text;
                                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                                DataTable dt = new DataTable();
                                sda.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    int questionno = 0;
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        var slist = new POVTestQuestionResult();
                                        slist.questionid = Convert.ToInt32(dtrow["id"]);
                                        slist.question = Convert.ToString(dtrow["question"]);

                                        string str = @"SELECT a.question_id, a.id, a.correct_ans,REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Replace
                                        (REPLACE(REPLACE(REPLACE(REPLACE(Replace(Replace(REPLACE(REPLACE(REPLACE(`option`, '<div>', ''),'<p>', ''),
                                        '<ol>', ''),'</div>',''),'<li>',''),'</li>',''),'</ol>',''),'</p>',''),'&nbsp;',''),'&ldquo;',''''),
                                        '&rsquo;',''''),'&ndash;','-'),'&rdquo;',''''),'&quot;',''''),char(13),''),char(10),'') as optionname, 
                                        ql.student_answer_id FROM answers a Left join questions_logs ql on ql.question_id=a.question_id
                                        where `option` is not null and `option` <> '' and 
                                        a.question_id=" + slist.questionid + " and ql.student_id=" + Detail.student_id
                                                    + " and ql.created=(select max(created) from questions_logs where question_id=" + slist.questionid
                                                    + " and student_id=" + Detail.student_id + ")";
                                        using (MySqlCommand cmd1 = new MySqlCommand(str, mysqlcon))
                                        {
                                            cmd1.CommandType = CommandType.Text;
                                            MySqlDataAdapter sda1 = new MySqlDataAdapter(cmd1);
                                            DataTable dt1 = new DataTable();
                                            sda1.Fill(dt1);
                                            if (dt1.Rows.Count > 0)
                                            {
                                                OptonLists = new List<POVOptionsResult>();
                                                foreach (DataRow dtrow1 in dt1.Rows)
                                                {
                                                    var slists = new POVOptionsResult();
                                                    int stuanscheck;
                                                    slists.questionid = slist.questionid;
                                                    slists.optionid = Convert.ToInt32(dtrow1["id"]);
                                                    slists.option = Convert.ToString(dtrow1["optionname"]);
                                                    slists.originalans = Convert.ToInt32(dtrow1["correct_ans"]);
                                                    stuanscheck = Convert.ToInt32(dtrow1["student_answer_id"]);
                                                    if (stuanscheck == slists.optionid)
                                                    {
                                                        slists.studentans = 1;
                                                    }
                                                    else
                                                    {
                                                        slists.studentans = 0;
                                                    }
                                                    OptonLists.Add(slists);
                                                }
                                            }
                                            slist.POVOptionsDetails = OptonLists;
                                        }
                                        questionno = questionno + 1;
                                        slist.questionno = questionno;
                                        Lists.Add(slist);
                                    }
                                    Detail.POVAnswerDetails = Lists;
                                }
                                else
                                {
                                    throw new HttpResponseException(HttpStatusCode.NoContent);
                                }
                            }
                        }
                    }
                }
            }
            return Detail;
        }

        [HttpPost]
        [Route("WeSchool/POVQuizTest")]
        public HttpResponseMessage POVQuiztest()
        {
            MySqlConnection mysqlcon = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
            var POVTestRequestmsg = Request.Content.ReadAsStringAsync();
            POVTestRequest povtestrequest = JsonConvert.DeserializeObject<POVTestRequest>(POVTestRequestmsg.Result);
            if (POVTestRequestmsg.Result.ToString() != null && POVTestRequestmsg.Result.ToString() != "")
            {
                string strquery = @"select * from `questions_logs` where question_id=" + povtestrequest.questionid + " and student_id=" + povtestrequest.student_id
                    + " and created is null and modified is null";
                MySqlCommand cmd = new MySqlCommand(strquery, mysqlcon);
                cmd.CommandType = CommandType.Text;
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    mysqlcon.Open();
                    string query = @"update  `questions_logs` set student_answer_id=" + povtestrequest.optionid + " where question_id=" + povtestrequest.questionid
                        + " and student_id=" + povtestrequest.student_id + " and created is null and modified is null";
                    MySqlCommand mcmdu = new MySqlCommand(query, mysqlcon);
                    mcmdu.ExecuteNonQuery();
                    mysqlcon.Close();
                }
                else
                {
                    mysqlcon.Open();
                    string query = @"insert into `questions_logs` (question_id,student_id,student_answer_id)values(" + povtestrequest.questionid
                                    + "," + povtestrequest.student_id + "," + povtestrequest.optionid + ")";
                    MySqlCommand mcmdu = new MySqlCommand(query, mysqlcon);
                    mcmdu.ExecuteNonQuery();
                    mysqlcon.Close();

                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Route("WeSchool/POVQuizResult")]
        public POVResultdetails POVQuizResult()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            MySqlConnection mysqlcon = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
            POVResultdetails Detail = new POVResultdetails();
            POVResult Listss = new POVResult();
            List<POVTestQuestionResult> Lists = new List<POVTestQuestionResult>();
            List<POVOptionsResult> OptonLists = new List<POVOptionsResult>();
            int noquestions, duration, cnt = 0;
            float score;
            int roundNumber, studentcode;
            string admission_no, program_name, batch_name, student_name;

            var POVResultRequestmsg = Request.Content.ReadAsStringAsync();
            POVResultRequest povresultrequest = JsonConvert.DeserializeObject<POVResultRequest>(POVResultRequestmsg.Result);
            if (POVResultRequestmsg.Result.ToString() != null && POVResultRequestmsg.Result.ToString() != "")
            {
                # region QuizTest Start

                for (int i = 0; i < povresultrequest.questionid.Length; i++)
                {
                    string testquery = @"select * from `questions_logs` where question_id=" + povresultrequest.questionid[i] + " and student_id=" + povresultrequest.student_id
                    + " and created is null and modified is null";
                    MySqlCommand testcmd = new MySqlCommand(testquery, mysqlcon);
                    testcmd.CommandType = CommandType.Text;
                    MySqlDataAdapter testsda = new MySqlDataAdapter(testcmd);
                    DataTable testdt = new DataTable();
                    testsda.Fill(testdt);
                    if (testdt.Rows.Count > 0)
                    {
                        mysqlcon.Open();
                        string updatequery = @"update  `questions_logs` set student_answer_id=" + povresultrequest.optionid[i] + " where question_id=" + povresultrequest.questionid[i]
                            + " and student_id=" + povresultrequest.student_id + " and created is null and modified is null";
                        MySqlCommand updatecmd = new MySqlCommand(updatequery, mysqlcon);
                        updatecmd.ExecuteNonQuery();
                        mysqlcon.Close();
                    }
                    else
                    {
                        mysqlcon.Open();
                        string insertquery = @"insert into `questions_logs` (question_id,student_id,student_answer_id)values(" + povresultrequest.questionid[i]
                                        + "," + povresultrequest.student_id + "," + povresultrequest.optionid[i] + ")";
                        MySqlCommand insertcmd = new MySqlCommand(insertquery, mysqlcon);
                        insertcmd.ExecuteNonQuery();
                        mysqlcon.Close();
                    }
                }

                #endregion

                # region Quiz Result

                mysqlcon.Open();
                string query = @"update `questions_logs` set created=now(), modified=now() where student_id=" + povresultrequest.student_id
                    + " and created is null and modified is null";
                MySqlCommand mcmdu = new MySqlCommand(query, mysqlcon);
                mcmdu.ExecuteNonQuery();
                mysqlcon.Close();

                string querytotal = @"SELECT  COUNT(q.id) as totalquestions,p.video_duration as duration from questions q
                                    left join program_videos p on p.id=q.program_video_id
                                    where q.program_video_id=" + povresultrequest.video_id + "";
                using (MySqlCommand cmdt = new MySqlCommand(querytotal, mysqlcon))
                {
                    cmdt.CommandType = CommandType.Text;
                    MySqlDataAdapter sdat = new MySqlDataAdapter(cmdt);
                    DataTable dtt = new DataTable();
                    sdat.Fill(dtt);
                    if (dtt.Rows.Count > 0)
                    {
                        foreach (DataRow dttrow in dtt.Rows)
                        {
                            Listss.noofquestions = Convert.ToInt32(dttrow["totalquestions"]);
                            Listss.student_id = povresultrequest.student_id;
                            Listss.video_id = povresultrequest.video_id;
                            duration = Convert.ToInt32(dttrow["duration"]);
                            string vquery = @"SELECT id, REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Replace(REPLACE(REPLACE(REPLACE(REPLACE(
                                 Replace(Replace(REPLACE(REPLACE(REPLACE(question, '<div>', ''),'<p>', ''),'<ol>', ''),
                                '</div>',''),'<li>',''),'</li>',''),'</ol>',''),'</p>',''),'&nbsp;',''),'&ldquo;',''''),
                                '&rsquo;',''''),'&ndash;','-'),'&rdquo;',''''),'&quot;',''''),char(13),''),char(10),'') as question FROM questions 
                                where program_video_id=" + povresultrequest.video_id + "";
                            using (MySqlCommand cmd = new MySqlCommand(vquery, mysqlcon))
                            {
                                cmd.CommandType = CommandType.Text;
                                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                                DataTable dt = new DataTable();
                                sda.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    int questionno = 0;
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        var slist = new POVTestQuestionResult();
                                        slist.questionid = Convert.ToInt32(dtrow["id"]);
                                        slist.question = Convert.ToString(dtrow["question"]);

                                        string str = @"SELECT a.question_id, a.id, a.correct_ans,REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Replace
                                        (REPLACE(REPLACE(REPLACE(REPLACE(Replace(Replace(REPLACE(REPLACE(REPLACE(`option`, '<div>', ''),'<p>', ''),
                                        '<ol>', ''),'</div>',''),'<li>',''),'</li>',''),'</ol>',''),'</p>',''),'&nbsp;',''),'&ldquo;',''''),
                                        '&rsquo;',''''),'&ndash;','-'),'&rdquo;',''''),'&quot;',''''),char(13),''),char(10),'') as optionname, 
                                        ql.student_answer_id FROM answers a Left join questions_logs ql on ql.question_id=a.question_id
                                        where `option` is not null and `option` <> '' and 
                                        a.question_id=" + slist.questionid + " and ql.student_id=" + Listss.student_id
                                                    + " and ql.created=(select max(created) from questions_logs where question_id=" + slist.questionid
                                                    + " and student_id=" + Listss.student_id + ")";
                                        using (MySqlCommand cmd1 = new MySqlCommand(str, mysqlcon))
                                        {
                                            cmd1.CommandType = CommandType.Text;
                                            MySqlDataAdapter sda1 = new MySqlDataAdapter(cmd1);
                                            DataTable dt1 = new DataTable();
                                            sda1.Fill(dt1);
                                            if (dt1.Rows.Count > 0)
                                            {
                                                OptonLists = new List<POVOptionsResult>();
                                                foreach (DataRow dtrow1 in dt1.Rows)
                                                {
                                                    var slists = new POVOptionsResult();
                                                    int stuanscheck;
                                                    slists.questionid = slist.questionid;
                                                    slists.optionid = Convert.ToInt32(dtrow1["id"]);
                                                    slists.option = Convert.ToString(dtrow1["optionname"]);
                                                    slists.originalans = Convert.ToInt32(dtrow1["correct_ans"]);
                                                    stuanscheck = Convert.ToInt32(dtrow1["student_answer_id"]);
                                                    if (stuanscheck == slists.optionid)
                                                    {
                                                        slists.studentans = 1;
                                                    }
                                                    else
                                                    {
                                                        slists.studentans = 0;
                                                    }

                                                    if (slists.originalans == 1 && slists.studentans == 1)
                                                    {
                                                        cnt = cnt + 1;
                                                    }
                                                    OptonLists.Add(slists);
                                                }
                                            }
                                            slist.POVOptionsDetails = OptonLists;
                                        }
                                        questionno = questionno + 1;
                                        slist.questionno = questionno;
                                        Lists.Add(slist);
                                    }
                                }
                                else
                                {
                                    throw new HttpResponseException(HttpStatusCode.NoContent);
                                }
                            }
                            Listss.noofcorrectanswers = cnt;
                            noquestions = Listss.noofquestions;
                            score = ((float)cnt / (float)noquestions) * 100;
                            roundNumber = (int)Math.Floor(score + 0.5);
                            Listss.score = score;

                            string strrquery = "select * from students where id=" + povresultrequest.student_id + "";
                            MySqlCommand cmdd = new MySqlCommand(strrquery, mysqlcon);
                            cmdd.CommandType = CommandType.Text;
                            MySqlDataAdapter sdaa = new MySqlDataAdapter(cmdd);
                            DataTable dts = new DataTable();
                            sdaa.Fill(dts);
                            if (dts.Rows.Count > 0)
                            {
                                foreach (DataRow dtrow1 in dts.Rows)
                                {
                                    admission_no = Convert.ToString(dtrow1["admission_no"]);
                                    program_name = Convert.ToString(dtrow1["program_name"]);
                                    batch_name = Convert.ToString(dtrow1["batch_name"]);
                                    studentcode = Convert.ToInt32(dtrow1["student_id"]);
                                    student_name = Convert.ToString(dtrow1["student_name"]);

                                    mysqlcon.Open();
                                    string qquery = @"insert into `student_video_report`(student_id,admission_no,program_name,batch_name,
                                                video_id,duration,score,status,exam_status,completion_date,created,modified)
                                                values(" + povresultrequest.student_id + ",'" + admission_no + "','" +
                                                    program_name + "','" + batch_name + "'," + povresultrequest.video_id + "," +
                                                    duration + "," + cnt + "," + score + ",'Complete',CURDATE(),NOW(),NOW())";
                                    MySqlCommand qcmdu = new MySqlCommand(qquery, mysqlcon);
                                    qcmdu.ExecuteNonQuery();
                                    mysqlcon.Close();

                                    mysqlcon.Open();
                                    string q1query = @"insert into `student_reports`(student_id,
                                                video_id,duration,score,status,exam_status,completion_date,created,modified)
                                                values(" + povresultrequest.student_id + "," + povresultrequest.video_id + "," +
                                                    duration + "," + cnt + "," + score + ",'Complete',CURDATE(),NOW(),NOW())";
                                    MySqlCommand q1cmd = new MySqlCommand(q1query, mysqlcon);
                                    q1cmd.ExecuteNonQuery();
                                    mysqlcon.Close();

                                    con.Open();
                                    string sqlquery = @"insert into tbl_pov_responsedata(student_name,student_id,
                                                video_id,score,exam_status,activeinactive,lastmoddate)
                                                values('" + student_name + "'," + studentcode + "," + povresultrequest.video_id + "," +
                                                              roundNumber + ",'Complete',1,GETDATE())";
                                    SqlCommand sqlcmd = new SqlCommand(sqlquery, con);
                                    sqlcmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            Detail.POVResultdetail = Listss;
                        }
                    }
                }
                #endregion

                return Detail;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

        }
    }
}
