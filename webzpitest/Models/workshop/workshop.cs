using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.workshop
{
    public class workshop
    {
    }
    public class bindcity
    {
        public string cityname { get; set; }
        public int citycode { get; set; }
    }
    public class citydetails
    {
        public List<bindcity> citydetail { set; get; }
    }

    public class campaign
    {
        public string activity { get; set; }
        public string msg { get; set; }
        public string eventcode { get; set; }
        public string campaignname { get; set; }
        public string campaignstartdate { get; set; }
        public string campaignenddate { get; set; }

    }
    public class campaigndetails
    {
        public List<campaign> campaigndetail { set; get; }
    }

    public class specialevents
    {
        public string allowselect { get; set; }
        public string msghead { get; set; }
        public string msg { get; set; }
        public int refcode { get; set; }
        public string eventname { get; set; }
        public string eventdetaillink { get; set; }
        public string eventdate { get; set; }
        public string eventtime { get; set; }
        public string lastdateofregistration { get; set; }
        public string maxmarks { get; set; }
        public int Amount { get; set; }
        public string RequestFor { get; set; }
        public string otherparam1 { get; set; }
        public string otherparam2 { get; set; }

       

    }
    public class specialeventsdetails
    {
        public List<specialevents> specialeventsdetail { set; get; }
    }
    public class specialeventsrequest
    {
        public string eventcode { set; get; }
        public int studentcode { set; get; }
        public int citycode { set; get; }
    }

    public class RegisteredEvents
    {
        public int refcode { get; set; }
        public string eventname { get; set; }
        public string eventdetaillink { get; set; }
        public string eventdate { get; set; }
        public string maxmarks { get; set; }
        public int Amount { get; set; }
    }

    public class RegisteredEventsdetails
    {
        public List<RegisteredEvents> RegisteredEventsdetail { set; get; }
    }

    public class WorkshopTandC
    {
        public string TermsandConditionlink { get; set; }
    }
    public class WorkshopTandCdetails
    {
        public List<WorkshopTandC> WorkshopsTandCdetail { get; set; }
    }

}