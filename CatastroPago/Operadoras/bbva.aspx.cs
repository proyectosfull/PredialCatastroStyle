using System;
using System.Web.UI;
using System.Security.Cryptography;
using Clases.Utilerias;

namespace CatastroPago.Operadoras
{
    public partial class bbva : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidaPago valida = new ValidaPago();
                string orderId = Session["idOrder"].ToString();  //Request.Form["OrderId"];  ///Request.QueryString["OrderId"]; 
                string idWeb = Session["idInternet"].ToString();
                string total = Session["Importe"].ToString();// Request.Form["Total"];  //Request.QueryString["Total"];
                string backUrl = System.Configuration.ConfigurationManager.AppSettings.Get("ServerPredial");

                mp_account.Value = "4914"; //5562 = TLALTIZAPAN pruebas  /// 4914 -productivo
                mp_product.Value = "1"; //multipagos
                mp_order.Value = orderId;
                mp_reference.Value = orderId;
                mp_node.Value = "0"; //0 = DEFAULT
                mp_concept.Value = "1"; //1 = predial
                mp_amount.Value = total; //agregar 2 decimales 00
                mp_customername.Value = "";
                mp_currency.Value = "1"; //1 pesos,2 dolares  
                mp_signature.Value = valida.checkHMAC(mp_order.Value + mp_reference.Value + mp_amount.Value); //mp_order + mp_reference + mp_amount
                mp_urlsuccess.Value = backUrl;
                mp_urlfailure.Value = backUrl;
                hfId.Value = idWeb;

                Page.ClientScript.RegisterStartupScript(this.GetType(), "Key", "envia_formulario();", true);

                }// != postback

        }

        public string checkHMAC( string cadena)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes("W2844aB7e1f18de2Df7aece57280eebcdEed9de55f4889203a44Cee85c566A255");
            

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);
            byte[] messageBytes = encoding.GetBytes(cadena);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
        }


    }
}