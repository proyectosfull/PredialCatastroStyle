using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatastroPago.Operadoras
{
    public partial class banorte : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string ls_orderId = Session["idOrder"].ToString();  //Request.Form["OrderId"];  ///Request.QueryString["OrderId"]; ;
                string ls_total = Session["Importe"].ToString();// Request.Form["Total"];  //Request.QueryString["Total"];
                string backUrl = System.Configuration.ConfigurationManager.AppSettings.Get("ServerPredial") ;

                MERCHANT_ID.Value = "7940700";
                USER.Value = "a7940700";
                PASSWORD.Value = "Ya07%!Mun";
                TERMINAL_ID.Value = "79407001"; //valor retornado en el archivo de depositos
                MODE.Value = "PRD";   //AUT  SIMULADOR ACEPTADA -  DEC SIMULADOR DECLINADA // PRD PRODUCCION (real)
                ENTRY_MODE.Value = "MANUAL"; // valor fijo
                Mr.Value = "0";  //valor fijo
                CMD_TRANS.Value = "AUTH";
                RESPONSE_URL.Value = backUrl;
                CONTROL_NUMBER.Value = ls_orderId;
                AMOUNT.Value = ls_total;
                MerchantNumber.Value = "7940700";
                MerchantName.Value = "MUNICIPIO DE YAUTEPEC";
                MerchantCity.Value = "MORELOS";

                //CUSTOMER_REF1.Value = ""; //variables para enviar inromacion adicional
                //CUSTOMER_REF2.Value = ""; 
                //CUSTOMER_REF3.Value = ""; 
                //CUSTOMER_REF4.Value = ""; 
                //CUSTOMER_REF5.Value = "";             
                //NumberOfPayments.Value = "0"; //pagos en meses si esta activada la opcion

                Page.ClientScript.RegisterStartupScript(this.GetType(), "Key", "envia_formulario();", true);


            }// != postback
        }
    }
}