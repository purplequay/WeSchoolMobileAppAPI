using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using webzpitest.Models.placement;
using Newtonsoft.Json;
using System.Reflection;
using webzpitest.Filters;
//using System.Web.Http.Cors;

namespace webzpitest.Controllers.placement
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [APIAuthorizeAttribute]
    public class PlacementsController : ApiController
    {
        [HttpPost]
        [Route("WeSchool/StatePlacement")]
        public statelistdisplaydetail stateplacementdisplay()
        {
            statelistdisplaydetail Detail = new statelistdisplaydetail();
            List<statelist> statedisplaylist = new List<statelist>();
            var statelistdisplayRequestmessage = Request.Content.ReadAsStringAsync();
            statelistrequest splacementrequest = JsonConvert.DeserializeObject<statelistrequest>(statelistdisplayRequestmessage.Result.ToString());
            if (statelistdisplayRequestmessage.Result.ToString() != null && statelistdisplayRequestmessage.Result.ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["placementscon"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_api_Placement_Display_Student_statelist", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@statecode", SqlDbType.Int).Value = splacementrequest.statecode;
                        cmd.Parameters.Add("@Specicode", SqlDbType.Int).Value = splacementrequest.specicode;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);     
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                var slist = new statelist();
                                slist.specicode = splacementrequest.specicode;
                                slist.statecode = splacementrequest.statecode;
                                slist.refno = Convert.ToInt32(dtrow["refno"]);
                                slist.advertisedate = Convert.ToString(dtrow["AdvtDate"]);
                                slist.companyname = Convert.ToString(dtrow["CompanyName"]);
                                slist.designation = Convert.ToString(dtrow["Designation"]);
                                slist.location = Convert.ToString(dtrow["Location"]);
                                slist.lastDateForApplying = Convert.ToString(dtrow["LastDateForApplying"]);
                                statedisplaylist.Add(slist);
                            }
                            Detail.statelistdisplaydetails = statedisplaylist;
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

        internal static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        internal static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }  
    }
}
