using webzpitest.Models;
using System.Data.Entity;
namespace webzpitest.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("con")
        {
        }        
        public DbSet<Applicationsdlp> Applicationsdlps { get; set; }       
        public DbSet<TokensManager> TokensManagers { get; set; }      
    }
}