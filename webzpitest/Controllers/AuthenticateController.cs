using webzpitest.Models;
using webzpitest.Repository;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using webzpitest.Models.student;
using Newtonsoft.Json;

namespace webzpitest.Controllers
{
    public class AuthenticateController : ApiController
    {
        IAuthenticate _IAuthenticate;
        public AuthenticateController()
        {
            _IAuthenticate = new AuthenticateConcrete();
        }

        [HttpPost]
        [Route("WeSchool/StudentLogin")]
        // POST: api/Authenticate
        public LoginUserDetails Authenticate([FromBody]LoginUserRequest logindetils)
        {
            Token newtoken = new Token();
            LoginUserDetails Detils = new LoginUserDetails();
            string admission = logindetils.coursetype + "/" + logindetils.batchname + "/" + logindetils.Rollno;
            if (string.IsNullOrEmpty(admission) && string.IsNullOrEmpty(logindetils.Password))
            {
                //var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                //message.Content = new StringContent("Not Valid Request");
                //return message;
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            else
            {
                Detils.LoginUser = LoginUserDetails(admission, logindetils.Password);
                Applicationsdlp ClientKeys = new Applicationsdlp();
                ClientKeys.AdmissionNo = admission;
                ClientKeys.Password = logindetils.Password;
                if (_IAuthenticate.ValidateKeys1(ClientKeys))
                {
                    var clientkeys = _IAuthenticate.GetApplicationsdlpDetils(ClientKeys.AdmissionNo, ClientKeys.Password);
                    if (clientkeys == null)
                    {
                        //var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                        //message.Content = new StringContent("InValid Keys");
                        //return message;
                        throw new HttpResponseException(HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (_IAuthenticate.IsTokenAlreadyExists1(clientkeys.Code))
                        {
                            _IAuthenticate.DeleteGenerateToken1(clientkeys.Code);
                            newtoken = GenerateandSaveToken1(clientkeys);
                            Detils.TokenDetails = newtoken;
                        }
                        else
                        {
                            newtoken = GenerateandSaveToken1(clientkeys);
                            Detils.TokenDetails = newtoken;
                        }
                    }
                }
                else
                {
                    return Detils;
                }
            }
            return Detils;
        }
        private LoginUser LoginUserDetails(string admissionno, string password)
        {
            LoginUser loginuserlist = new LoginUser();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_loginuser_mobile", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@admissionno", SqlDbType.NVarChar).Value = admissionno;
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    var slist = new LoginUser();
                    while (rdr.Read())
                    {
                        loginuserlist.studentcode = Convert.ToInt32(rdr["code"]);
                        loginuserlist.specicode = Convert.ToInt32(rdr["specicode"].ToString());
                        loginuserlist.admissionno = Convert.ToString(rdr["admissionno"]);
                        loginuserlist.firstname = Convert.ToString(rdr["Firstname"]);
                        loginuserlist.lastname = Convert.ToString(rdr["Lastname"]);
                        loginuserlist.middletname = Convert.ToString(rdr["Middlename"]);
                        loginuserlist.batchcode = Convert.ToInt32(rdr["BatchCode"].ToString());
                        loginuserlist.coursetypecode = Convert.ToInt32(rdr["CourseTypeCode"].ToString());
                        loginuserlist.coursecode = Convert.ToInt32(rdr["CourseCode"].ToString());
                        loginuserlist.msg = Convert.ToInt32(rdr["msg"]);
                        loginuserlist.coursecompletionstatus = Convert.ToString(rdr["coursecompletionstatus"]);
                        loginuserlist.povcompletestatus = Convert.ToString(rdr["povcompletestatus"]);
                       // loginuserlist.blockstatus = Convert.ToString(rdr["blockstatus"]);
                       // loginuserlist.blockmessage = Convert.ToString(rdr["blockmessage"]);

                        if (loginuserlist.admissionno == "")
                        {
                            throw new HttpResponseException(HttpStatusCode.BadRequest);
                        }
                    }
                    con.Close();
                }
            }
            return loginuserlist;
        }
        private Token GenerateandSaveToken1(Applicationsdlp clientkeys)
        {
            Token newtoken = new Token();
            var IssuedOn = DateTime.Now;
            var newToken = _IAuthenticate.GenerateToken1(clientkeys, IssuedOn);
            TokensManager token = new TokensManager();
            token.TokenID = 0;
            token.TokenKey = newToken;
            token.StudentCode = clientkeys.Code;
            token.IssuedOn = IssuedOn;
            token.ExpiresOn = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpiry"]));
            token.CreatedOn = DateTime.Now;
            var result = _IAuthenticate.InsertToken1(token);
            if (result == 1)
            {
                //HttpResponseMessage response = new HttpResponseMessage();
                newtoken.access_token = newToken;
                newtoken.tokenType = "WeSchoolAuthorization";
                newtoken.ExpiresOn = ConfigurationManager.AppSettings["TokenExpiry"] + " Day";
                //response = Request.CreateResponse(HttpStatusCode.OK, newtoken);
                // response.Headers.Add("Token", newToken);
                // response.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["TokenExpiry"]);
                // response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
                return newtoken;
            }
            else
            {
                return newtoken;
            }
        }
    }
}
