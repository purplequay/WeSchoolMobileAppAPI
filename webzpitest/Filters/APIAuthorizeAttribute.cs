using webzpitest.Context;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace webzpitest.Filters
{
    public class APIAuthorizeAttribute : AuthorizeAttribute
    {
        private DatabaseContext db = new DatabaseContext();
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }
            HandleUnauthorizedRequest(filterContext);
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }
        private bool Authorize(HttpActionContext actionContext)
        {
            try
            {
                var encodedString = actionContext.Request.Headers.GetValues("WeSchoolAuthorization").First();
                bool validFlag = false;
                if (!string.IsNullOrEmpty(encodedString))
                {
                    var key = EncryptionLibrary.DecryptText(encodedString);
                    string[] parts = key.Split(new char[] { ':' });
                    var UserID = Convert.ToInt32(parts[0]);       // Studentcode
                    var RandomKey = parts[1];                     // Random Key
                    var CompanyID =parts[2];                      // Admission No
                    long ticks = long.Parse(parts[3]);            // Ticks
                    DateTime IssuedOn = new DateTime(ticks);      // Issued On
                    var ClientID = parts[4];                      // Admission No
                    // By passing this parameter 
                    var registerModel = (from register in db.Applicationsdlps
                                         where register.AdmissionNo == CompanyID
                                         && register.Code == UserID
                                         && register.AdmissionNo == ClientID
                                         select register).FirstOrDefault();
                    if (registerModel != null)
                    {
                        // Validating Time
                        var ExpiresOn = (from token in db.TokensManagers
                                         where token.StudentCode == UserID
                                         select token.ExpiresOn).FirstOrDefault();
                        // Validating Token
                        var TokenKey = (from token in db.TokensManagers
                                         where token.StudentCode == UserID
                                         select token.TokenKey).FirstOrDefault();
                        if ((encodedString != TokenKey))
                        {
                            validFlag = false;
                        }
                        else
                        {
                            if ((DateTime.Now > ExpiresOn))
                            {
                                validFlag = false;
                            }
                            else
                            {
                                validFlag = true;
                            }                           
                        }                       
                    }
                    else
                    {
                        validFlag = false;
                    }
                }
                return validFlag;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}