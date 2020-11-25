using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace webzpitest.Models.placement
{
    public class placementads
    {
        public int refno { set; get; }
        public string sources { set; get; }
        public string reference { set; get; }
        public string page { set; get; }
        public string specialization { set; get; }
        public string organization { set; get; }
        public string industry { set; get; }
        public string managementlevel { set; get; }
        public string designation { set; get; }
        public string location { set; get; }
        public string locationstate { set; get; }
        public string numberofopenings { set; get; }
        public string jobprofile { set; get; }
        public string qualifictionrequired { set; get; }
        public string experiencerequired { set; get; }
        public string compensation { set; get; }
        public string agelimit { set; get; }
        public string addresscommunicationto { set; get; }
        public string address1 { set; get; }
        public string address2 { set; get; }
        public string address3 { set; get; }
        public string pincode { set; get; }
        public string city { set; get; }
        public string state { set; get; }
        public string email { set; get; }
        public string website { set; get; }
        public string contactnumber { set; get; }
        public string contactnumber2 { set; get; }
        public string mobilenumber { set; get; }
        public string fax { set; get; }
        public string remarks { set; get; }
        public string advtdate { set; get; }
        public string applybefore { set; get; }
        public string walkIninterviewat { set; get; }
    }
    public class placementadsdetail
    {
        public List<placementads> placementadsdetails { set; get; }        
    }
}