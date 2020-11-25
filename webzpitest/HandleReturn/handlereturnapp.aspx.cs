using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

public partial class handlereturnapp : System.Web.UI.Page
{

    #region Variable Declaration

    string strHEX, strPGActualReponseWithChecksum, strPGActualReponseEncrypted, strPGActualReponseDecrypted, strPGresponseChecksum, strPGTxnStatusCode;
    string[] strPGChecksum, strPGTxnString;
    bool isDecryptable = false;
    string pgwaycode = "";
    string strPG_TxnStatus = string.Empty,
    strPG_ClintTxnRefNo = string.Empty,//studentcode
            strPG_TPSLTxnBankCode = string.Empty,
            strPG_TPSLTxnID = string.Empty,//pgwaycode
             strPG_TPSLTxnTime = string.Empty,//pgwaycode
            strPG_TxnAmount = string.Empty,
            strPG_TxnDateTime = string.Empty,
            strPG_Token = string.Empty,
            strPG_Hash = string.Empty,
            strPG_TxnDate = string.Empty,
             strPG_TXN_MSG = string.Empty,
              strPG_mandate_reg_no = string.Empty,
             strPG_TXN_ERR_MSG = string.Empty,
              strPG_ClintReqMetaData = string.Empty,
               strPG_bal_amt = string.Empty,
                strPG_card_id = string.Empty,
                 strPG_aliasname = string.Empty,
                    strPG_token = string.Empty,
                     strPG_hash = string.Empty,
                    strPG_BnkTxnid = string.Empty,
            strPG_TxnTime = string.Empty;
    string strPGResponse;
    string[] strSplitDecryptedResponse;
    string[] strArrPG_TxnDateTime;
    string requestfor = "";
    string studentcode = "";
    string challanno = "";
    string amount = "";
    string paid = "", OtherParam1 = "", OtherParam2 = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (Request["msg"] != null)
            {
                Session["PGResponse"] = Request["msg"];

                strPGResponse = Request["msg"].ToString();

                //  Response.Write(strPGResponse);
             //   Response.Write("<br/> TechProcess Response: " + strPGResponse);


                strSplitDecryptedResponse = strPGResponse.Split('|');

                strPG_TxnStatus = strSplitDecryptedResponse[0].ToString();
                strPG_TXN_MSG = strSplitDecryptedResponse[1].ToString();
                strPG_TXN_ERR_MSG = strSplitDecryptedResponse[2].ToString();
                strPG_ClintTxnRefNo = strSplitDecryptedResponse[3].ToString();
                strPG_TPSLTxnBankCode = strSplitDecryptedResponse[4].ToString();
                strPG_TPSLTxnID = strSplitDecryptedResponse[5].ToString();
                strPG_TxnAmount = strSplitDecryptedResponse[6].ToString();
                strPG_ClintReqMetaData = strSplitDecryptedResponse[7].ToString();
                strPG_TPSLTxnTime = strSplitDecryptedResponse[8].ToString();
                strPG_bal_amt = strSplitDecryptedResponse[9].ToString();
                strPG_card_id = strSplitDecryptedResponse[10].ToString();
                strPG_aliasname = strSplitDecryptedResponse[11].ToString();
                strPG_BnkTxnid = strSplitDecryptedResponse[12].ToString();
                strPG_mandate_reg_no = strSplitDecryptedResponse[13].ToString();
                strPG_token = strSplitDecryptedResponse[14].ToString();
                strPG_hash = strSplitDecryptedResponse[15].ToString();
               



                // GetPGRespnseData(strSplitDecryptedResponse);
                //Response.Write(strSplitDecryptedResponse);
                if (strPG_TxnStatus == "0300")
                {
                    string T = " <br/>Transaction Success :: <br/>" + strPG_TxnStatus;
                   // Response.Write(T);
                    Session["PGPAID"] = "Y";

                }
                else if (strPG_TxnStatus == "0396")
                {
                    Session["PGPAID"] = "W";
                }
                else
                {
                    //string T = "<br/>Transaction Fail :: <br/>" + "Response :: <br/>" + strPG_TXN_MSG + ",  " + strPG_TXN_ERR_MSG;
                    Session["PGPAID"] = "N";
                }


                paid = Convert.ToString(Session["PGPAID"]);
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
                SqlCommand cmd = new SqlCommand("usp_Insertupdate_Paymentgateway_mobileapp", con);
                // cmd.Parameters.AddWithValue("@studentcode", Convert.ToInt32(pgwaycode));
                cmd.Parameters.AddWithValue("@PgwayCode", Convert.ToInt32(strPG_ClintTxnRefNo));
                cmd.Parameters.AddWithValue("@PaymentStatus", Convert.ToString(paid + "|" + strPG_TPSLTxnID + "|" + strPG_TxnStatus));
                // cmd.Parameters.AddWithValue("@Token", Convert.ToString(strPG_Token));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    requestfor = dt.Rows[0]["MSG"].ToString();
                    amount = dt.Rows[0]["Amount"].ToString();
                    challanno = dt.Rows[0]["Challanno"].ToString();
                    studentcode = dt.Rows[0]["studentcode"].ToString();
                    OtherParam1 = dt.Rows[0]["OtherParam1"].ToString();
                    OtherParam2 = dt.Rows[0]["OtherParam2"].ToString();

                    con.Open();
                    string sv = @"insert into tbl_pgway_return_mobile(studentcode,pgwaycode,responsedata,tran_status,tran_msg,tran_errormsg,lastmoddate)values
                                (" + studentcode + "," + Convert.ToInt32(strPG_ClintTxnRefNo) + ",'" + strPGResponse + "','" + paid + "','" + strPG_TXN_MSG + "','" +
                                  strPG_TXN_ERR_MSG + "',getdate())";
                    SqlCommand cmdinsert = new SqlCommand(sv, con);
                    cmdinsert.ExecuteNonQuery();
                    con.Close();
                }

                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:back_onclick(); ", true);


            }

        }
        catch (Exception ex)
        {
        }
    }
    public void GetPGRespnseData(string[] parameters)
    {
        string[] strGetMerchantParamForCompare;
        for (int i = 0; i < parameters.Length; i++)
        {
            strGetMerchantParamForCompare = parameters[i].ToString().Split('=');
            if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_STATUS")
            {
                strPG_TxnStatus = Convert.ToString(strGetMerchantParamForCompare[1]);
            }
            else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_TXN_REF")
            {
                strPG_ClintTxnRefNo = Convert.ToString(strGetMerchantParamForCompare[1]);
            }
            else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_BANK_CD")
            {
                strPG_TPSLTxnBankCode = Convert.ToString(strGetMerchantParamForCompare[1]);
            }
            else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_ID")
            {
                strPG_TPSLTxnID = Convert.ToString(strGetMerchantParamForCompare[1]);
            }
            else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_AMT")
            {
                strPG_TxnAmount = Convert.ToString(strGetMerchantParamForCompare[1]);
            }
            else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_TIME")
            {
                strPG_TxnDateTime = Convert.ToString(strGetMerchantParamForCompare[1]);
                strArrPG_TxnDateTime = strPG_TxnDateTime.Split(' ');
                strPG_TxnDate = Convert.ToString(strArrPG_TxnDateTime[0]);
                strPG_TxnTime = Convert.ToString(strArrPG_TxnDateTime[1]);
            }
            else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_RQST_META")
            {
                strPG_TxnDateTime = Convert.ToString(strGetMerchantParamForCompare[1]);
                string[] a = strPG_TxnDateTime.Split('}');
                string[] b = a[0].Split(':');

                strPG_TxnTime = Convert.ToString(b[1]);
                pgwaycode = strPG_TxnTime;
                Response.Write(pgwaycode);
            }
            else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TOKEN")
            {
                strPG_Token = Convert.ToString(strGetMerchantParamForCompare[1]);
            }
            else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "HASH")
            {
                strPG_Hash = Convert.ToString(strGetMerchantParamForCompare[1]);
            }

            else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_MSG")
            {
                strPG_TXN_MSG = Convert.ToString(strGetMerchantParamForCompare[1]);
            }

            else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_ERR_MSG")
            {
                strPG_TXN_ERR_MSG = Convert.ToString(strGetMerchantParamForCompare[1]);
            }
        }
    }

}