using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.college;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.college
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CollegeController : ApiController
    {
        [HttpGet]
        [Route("WeSchool/LoadBatch/{batchcode}")]
        public batchdetail loadbatch(int batchcode)
        {
            List<bindbatch> batchlist = new List<bindbatch>();
            batchdetail Detils = new batchdetail();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_api_loadbatch", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@batchcode", SqlDbType.Int).Value = batchcode;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var slist = new bindbatch();
                        slist.batchcode = Convert.ToInt32(rdr["code"]);
                        slist.batchname = Convert.ToString(rdr["name"]);
                        batchlist.Add(slist);
                    }
                    con.Close();
                }
                Detils.batchdetails = batchlist;
            }
            return Detils;
        }
    }
}
