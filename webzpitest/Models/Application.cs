using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace webzpitest.Models
{
    [Table("Application")]
    public class Application
    {
        [Key]
        public Int64 Code { get; set; }
        public string AdmissionNo { get; set; }
        public string password { get; set; } 
    }
}