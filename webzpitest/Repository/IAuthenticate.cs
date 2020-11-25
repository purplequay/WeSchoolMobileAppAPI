using webzpitest.Models;
using System;
namespace webzpitest.Repository
{
    public interface IAuthenticate
    {
        Applicationsdlp GetApplicationsdlpDetils(string AdmissionNo, string password);
        string GenerateToken1(Applicationsdlp ApplicationsdlpKeys, DateTime IssuedOn);
        bool ValidateKeys1(Applicationsdlp ApplicationsdlpKeys);
        bool IsTokenAlreadyExists1(decimal StudentCode);
        int DeleteGenerateToken1(decimal StudentCode);
        int InsertToken1(TokensManager token);
    }
}
