using System;
using System.Linq;
using webzpitest.Models;
using webzpitest.Context;

namespace webzpitest.Repository
{
    public class AuthenticateConcrete : IAuthenticate
    {
        DatabaseContext _context;
        public AuthenticateConcrete()
        {
            _context = new DatabaseContext();
        }
        public Applicationsdlp GetApplicationsdlpDetils(string AdmissionNo, string password)
        {
            try
            {
                var result = (from applicationKeys in _context.Applicationsdlps
                              where applicationKeys.AdmissionNo == AdmissionNo && applicationKeys.Password == password
                              select applicationKeys).FirstOrDefault();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GenerateToken1(Applicationsdlp ApplicationsdlpKeys, DateTime IssuedOn)
        {
            try
            {
                string randomnumber =
                   string.Join(":", new string[]
                   {   Convert.ToString(ApplicationsdlpKeys.Code),
                webzpitest.EncryptionLibrary.KeyGenerator.GetUniqueKey(),
                Convert.ToString(ApplicationsdlpKeys.AdmissionNo),
                Convert.ToString(IssuedOn.Ticks),
                ApplicationsdlpKeys.AdmissionNo
                   });

                return EncryptionLibrary.EncryptText(randomnumber);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ValidateKeys1(Applicationsdlp ApplicationsdlpKeys)
        {
            try
            {
                var result = (from applicationKeys in _context.Applicationsdlps
                              where applicationKeys.AdmissionNo == ApplicationsdlpKeys.AdmissionNo && applicationKeys.Password == ApplicationsdlpKeys.Password
                              select applicationKeys).Count();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool IsTokenAlreadyExists1(decimal StudentCode)
        {
            try
            {
                var result = (from token in _context.TokensManagers
                              where token.StudentCode == StudentCode
                              select token).Count();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int DeleteGenerateToken1(decimal StudentCode)
        {
            try
            {
                var token = _context.TokensManagers.SingleOrDefault(x => x.StudentCode == StudentCode);
                _context.TokensManagers.Remove(token);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int InsertToken1(TokensManager token)
        {
            try
            {
                _context.TokensManagers.Add(token);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}