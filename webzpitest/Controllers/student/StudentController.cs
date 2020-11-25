using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.student;
using Newtonsoft.Json;
using webzpitest.Filters;
//using System.Web.Http.Cors;
namespace webzpitest.Controllers.student
{
    //  [EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class StudentController : ApiController
    {

        [HttpGet]
        [Route("WeSchool/Semester/{studentcodes}")]
        public Semesterdetail loadsemester(int studentcodes)
        {
            List<Semesterlist> semlist = new List<Semesterlist>();
            Semesterdetail Detils = new Semesterdetail();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_semesterlist", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = studentcodes;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var slist = new Semesterlist();
                        slist.Sem_1 = Convert.ToString(rdr["Sem_1"]);
                        slist.Sem_2 = Convert.ToString(rdr["Sem_2"]);
                        slist.Sem_3 = Convert.ToString(rdr["Sem_3"]);
                        slist.Sem_4 = Convert.ToString(rdr["Sem_4"]);
                        semlist.Add(slist);
                    }
                    con.Close();
                }
                Detils.semesterdetails = semlist;
            }
            return Detils;
        }

        [HttpGet]
        [Route("WeSchool/IDCard/{stucode}")]
        public SIdCard SIdcard(int stucode)
        {
            SIdCard Detail = new SIdCard();
            IdCard idcard = new IdCard();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_IDcard", con))
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
                            idcard.IdLink = Convert.ToString(dtrow["IdLink"]);
                        }
                        Detail.StudentIdCard = idcard;
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
        [Route("WeSchool/Holiday/{scode}")]
        public HolidayList Holiday(int scode)
        {
            HolidayList Detail = new HolidayList();
            Holiday holiday = new Holiday();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_holidaylist_mobile", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = scode;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            holiday.HolidayLink = Convert.ToString(dtrow["HolidayLink"]);
                        }
                        Detail.Holidays = holiday;
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
        [Route("WeSchool/userprofiledetails/{studentscode}")]
        public UserProfileDetail userprofiledetails(int studentscode)
        {
            userprofile profile = new userprofile();
            UserProfileDetail Detils = new UserProfileDetail();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_getuserprofile", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = studentscode;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        profile.studentcode = Convert.ToInt32(rdr["code"]);
                        profile.admissionno = Convert.ToString(rdr["admissionno"]);
                        profile.firstname = Convert.ToString(rdr["Firstname"]);
                        profile.lastname = Convert.ToString(rdr["Lastname"]);
                        profile.middletname = Convert.ToString(rdr["Middlename"]);
                        profile.aadharno = Convert.ToString(rdr["adharno"]);
                        profile.correspondance = Convert.ToString(rdr["Correspondance"]);
                        profile.homeaddress1 = Convert.ToString(rdr["HomeAddress1"]);
                        profile.homeaddress2 = Convert.ToString(rdr["HomeAddress2"]);
                        profile.homeaddress3 = Convert.ToString(rdr["HomeAddress3"]);
                        profile.homeaddress4 = Convert.ToString(rdr["HomeAddress4"]);
                        profile.homepincode = Convert.ToString(rdr["HomePincode"]);
                        profile.hometelno = Convert.ToString(rdr["HomeTelno"]);
                        profile.homemobileno = Convert.ToString(rdr["HomeMobileNo"]);
                        profile.homeemail = Convert.ToString(rdr["HomeEmail"]);
                        profile.officeemail = Convert.ToString(rdr["OfficeEmail"]);
                        profile.officeaddress1 = Convert.ToString(rdr["OfficeAddress1"]);
                        profile.officeaddress2 = Convert.ToString(rdr["OfficeAddress2"]);
                        profile.officeaddress3 = Convert.ToString(rdr["OfficeAddress3"]);
                        profile.officeaddress4 = Convert.ToString(rdr["OfficeAddress4"]);
                        profile.officetelno = Convert.ToString(rdr["OfficeTelno"]);
                        profile.officemobileno = Convert.ToString(rdr["OfficeMobileNo"]);
                        profile.officepincode = Convert.ToString(rdr["OfficePincode"]);
                        profile.fb = Convert.ToString(rdr["fb"]);
                        profile.twitter = Convert.ToString(rdr["twitter"]);
                        profile.linkedin = Convert.ToString(rdr["linkedin"]);
                        profile.batchcode = Convert.ToInt32(rdr["BatchCode"].ToString());
                        profile.coursetypecode = Convert.ToInt32(rdr["CourseTypeCode"].ToString());
                        profile.coursecode = Convert.ToInt32(rdr["CourseCode"].ToString());
                        profile.homecity = Convert.ToInt32(rdr["HomeCity"].ToString());
                        profile.officecity = Convert.ToInt32(rdr["OfficeCity"].ToString());
                        profile.homestatecode = Convert.ToInt32(rdr["HomeStateCode"].ToString());
                        profile.officestatecode = Convert.ToInt32(rdr["OfficeStateCode"].ToString());
                        profile.homecountrycode = Convert.ToInt32(rdr["HomeCountryCode"].ToString());
                        profile.officecountrycode = Convert.ToInt32(rdr["OfficeCountryCode"].ToString());
                        profile.specicode = Convert.ToInt32(rdr["specicode"].ToString());
                        profile.studentimage = Convert.ToBase64String((byte[])rdr["picture"]);
                    }
                    con.Close();
                }
            }
            Detils.userprofile = profile;
            return Detils;
        }

        [HttpGet]
        [Route("WeSchool/Subjects/{studentcode}")]
        public subjectDetail subjectsdetail(int studentcode)
        {
            List<subject> subjectlist = new List<subject>();
            subjectDetail Details = new subjectDetail();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_selectValidSubjectList", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@studentcode", SqlDbType.Int).Value = studentcode;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var slist = new subject();
                        slist.subjectcode = Convert.ToInt32(rdr["code"]);
                        slist.subjectname = Convert.ToString(rdr["name"]);
                        subjectlist.Add(slist);
                    }
                    con.Close();
                    Details.subjectdetails = subjectlist;
                }
            }
            return Details;
        }

        [HttpPatch]
        [Route("WeSchool/EditStudentDetail")]
        public HttpResponseMessage Editstudentdetail([FromBody]EditProfileRequest epr)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_EditContactDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@studentcode", epr.studentcode);
                    cmd.Parameters.AddWithValue("@Correspondance", epr.correspondance);
                    cmd.Parameters.AddWithValue("@HomeAddress1", epr.homeaddress1);
                    cmd.Parameters.AddWithValue("@HomeAddress2", epr.homeaddress2);
                    cmd.Parameters.AddWithValue("@HomeAddress3", epr.homeaddress3);
                    cmd.Parameters.AddWithValue("@HomeAddress4", epr.homeaddress4);
                    cmd.Parameters.AddWithValue("@HomePincode", epr.homepincode);
                    cmd.Parameters.AddWithValue("@HomeCity", epr.homecity);
                    cmd.Parameters.AddWithValue("@HomeStateCode", epr.homestatecode);
                    cmd.Parameters.AddWithValue("@HomeCountryCode", epr.homecountrycode);
                    cmd.Parameters.AddWithValue("@HomeTelNo", epr.hometelno);
                    cmd.Parameters.AddWithValue("@HomeMobileNo", epr.homemobileno);
                    cmd.Parameters.AddWithValue("@HomeEMail", epr.homeemail);
                    cmd.Parameters.AddWithValue("@OfficeAddress1", epr.officeaddress1);
                    cmd.Parameters.AddWithValue("@OfficeAddress2", epr.officeaddress2);
                    cmd.Parameters.AddWithValue("@OfficeAddress3", epr.officeaddress3);
                    cmd.Parameters.AddWithValue("@OfficeAddress4", epr.officeaddress4);
                    cmd.Parameters.AddWithValue("@OfficeCity", epr.officecity);
                    cmd.Parameters.AddWithValue("@OfficePincode", epr.officepincode);
                    cmd.Parameters.AddWithValue("@OfficeStateCode", epr.officestatecode);
                    cmd.Parameters.AddWithValue("@OfficeCountryCode", epr.officecountrycode);
                    cmd.Parameters.AddWithValue("@OfficeTelNo", epr.officetelno);
                    cmd.Parameters.AddWithValue("@OfficeMobileNo", epr.officemobileno);
                    cmd.Parameters.AddWithValue("@OfficeEMail", epr.officeemail);
                    cmd.Parameters.AddWithValue("@fb", epr.fb);
                    cmd.Parameters.AddWithValue("@linkedin", epr.linkedin);
                    cmd.Parameters.AddWithValue("@twitter ", epr.twitter);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (Convert.ToInt16(dt.Rows[0]["Msg"]) == 1)
                    {
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                    else
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotModified);
                    }
                }
            }
        }

    }
}
