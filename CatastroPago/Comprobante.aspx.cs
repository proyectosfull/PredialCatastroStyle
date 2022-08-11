using System;
using System.Web.UI;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using System.Globalization;
using System.Transactions;

namespace CatastroPago
{
    public partial class Comprobante : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {           

            if (!Page.IsPostBack)
            {
                cParametroSistema municipio = new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO");
                string rutaSM = string.Empty;
                string rutaIP = string.Empty;
                Session["usuario"] = "";

                if (municipio.Valor == "TLALTIZAPAN") //BBVA BANCOMER    
                {
                    //MensajesInterfaz prepa = new PreparaRecibo().ComprobanteInternet("3-420000071021-17", "45587", ref rutaIP, ref rutaSM);
                    ImagenLogo.ImageUrl = "~/Img/tlaltizapan.jpg";
                    ImagenLogo.Height = 90;
                    ImagenLogo.Width = 252;
                    ViewState["colordiv"] = "#FCF418";
                
                    #region RegresoBanco tlatizapan
                    if (Request.Form["mp_order"] != null && (Request.Form["mp_authorization"] != null && Convert.ToInt32(Request.Form["mp_authorization"] ) > 0 ) )  //Request.Form["CONTROL_NUMBER"] != null &&
                    {
                        #region regreso banco tlaltizapan                  
                        ////busca si ya esta pagado el tramite de ser asi... solo informa que ya se pago
                        tInternet internet = new tInternetBL().BuscaOrdenIdPagado(Request.Form["mp_order"].ToString(), Request.Form["mp_authorization"].ToString());
                        if (internet != null)
                        {
                            ApagaEtiquetas("Clave pagada, gracias!");
                            return;
                        }

                        string valida = new ValidaPago().checkHMAC(Request.Form["mp_order"].ToString() + Request.Form["mp_reference"].ToString() +
                                        Request.Form["mp_amount"].ToString() + Request.Form["mp_authorization"].ToString());
                        string signature = Request.Form["mp_signature"].ToString();

                        if (signature == valida)
                        {
                            lblFolio.Text = Request.Form["mp_reference"].ToString();
                            lblCvecatProcesada.Text = Request.Form["mp_reference"].ToString() + "-" + Request.Form["hfId"].ToString();
                            lblImporteTotal.Text = Convert.ToDecimal(Request.Form["mp_amount"]).ToString("N2", CultureInfo.CurrentCulture);
                            lblFechaHora.Text = DateTime.Today.ToString();  // Request.Form["AUTH_RSP_DATE"].ToString();
                            lblClaveAutorizacion.Text = Request.Form["mp_authorization"].ToString();
                            lblEstado.Text = "Operación Exitosa, gracias por su pago.";

                            //actualiza pago
                            MensajesInterfaz msg = new PreparaRecibo().ActualizaTipoPago(Request.Form["mp_reference"].ToString() + "-" + Request.Form["hfId"].ToString(), Request.Form["mp_authorization"].ToString(), Request.Form["mp_paymentMethod"].ToString().Trim());
                            if (Request.Form["mp_paymentMethod"].ToString().Trim() == "CLABE,clabe" || Request.Form["mp_paymentMethod"].ToString().Trim() == "CLABE" || Request.Form["mp_paymentMethod"].ToString().Trim() == "clabe")
                            {
                                lblEstado.Text = "Gracias, su pago se encuentra en proceso de validación, solicite su recibo 3 días después de registrada la transacción";                          
                                return;
                            }
                            try
                            {                                
                                ComprobantePago(Request.Form["mp_order"].ToString()+"-"+ Request.Form["hfId"].ToString(), Request.Form["mp_authorization"].ToString());                                
                            }//try
                            catch (Exception ex)
                            {
                                msg = new cErrorBL().insertcError("Comprobante Internet cParámetros--- idOrden/idinternet: " +Request.Form["mp_order"].ToString() + "-" + Request.Form["hfId"].ToString(), Request.Form["mp_authorization"].ToString());
                                ApagaEtiquetas("Ocurrió un problema al generar su recibo, favor de comunicarse a la Dirección de Impuesto Predial y Catastro.");
                                return;
                            }
                        }                        
                        else
                        {
                            ApagaEtiquetas("Transacción errónea");                            
                            return;
                        }
                        #endregion
                    }//REQUEST
                    else
                    {
                        ApagaEtiquetas("Transacción errónea.");
                        return;
                    }
                    #endregion
                }
                else //YAUTEPEC
                {
                    ImagenLogo.ImageUrl = "~/Img/logo_yaute.jpg";
                    ViewState["colordiv"] = "#ff3399";
                    ImagenLogo.Height = 160;
                    ImagenLogo.Width = 420;

                    #region RegresoBanco

                    //// PRUEBAS  YAUTE Request.Form["CONTROL_NUMBER"]="12-510000900652-22"
                    //string validaPagoS = new tInternetBL().BuscaOrdenIdPagadoBnt("12-510000900652-2", "098507");
                    //MensajesInterfaz msgS = new PreparaRecibo().ActualizaTipoPago("12-510000900652-2", "098507", "TDX");
                    //ComprobantePago("12-510000900652-2", "098507");
                    //// FIN PRUEBAS

                    if (Request.Form["PAYW_RESULT"] != null)
                    {
                        //A – Aprobada  D – Declinada  R – Rechazada    T – Sin respuesta del autorizador 
                        //
                        //ApagaEtiquetas(Request.Form["PAYW_RESULT"].ToString());
                        string strResult = Request.Form["PAYW_RESULT"].ToString();
                        if (strResult == "A")
                        {
                            lblEstado.Text = "OPERACION EXITOSA, GRACIAS POR SU PAGO.";
                            lblFolio.Text = Request.Form["CONTROL_NUMBER"].ToString();
                            lblFechaHora.Text = Request.Form["AUTH_RSP_DATE"].ToString();
                            lblClaveAutorizacion.Text = Request.Form["AUTH_CODE"].ToString();
                            lblCvecatProcesada.Text = Request.Form["CONTROL_NUMBER"].ToString();
                            lblImporteTotal.Visible = false;
                            string validaPago = new tInternetBL().BuscaOrdenIdPagadoBnt(Request.Form["CONTROL_NUMBER"].ToString(), Request.Form["AUTH_CODE"].ToString());
                            if (validaPago ==  "PAGADA")
                            {
                                ApagaEtiquetas("Clave pagada, gracias!");
                                return;
                            }
                            MensajesInterfaz msg = new PreparaRecibo().ActualizaTipoPago(Request.Form["CONTROL_NUMBER"].ToString(), Request.Form["AUTH_CODE"].ToString(), "TDX");
                            ComprobantePago(Request.Form["CONTROL_NUMBER"].ToString(), Request.Form["AUTH_CODE"].ToString());
                                                                    
                            return;
                        }
                        else
                        {
                            if (strResult == "D")
                                ApagaEtiquetas("TARJETA DECLINADA");
                            if (strResult == "R")
                                ApagaEtiquetas("TARJETA RECHAZADA");
                            if (strResult == "T")
                                ApagaEtiquetas("SIN RESPUESTA");
                            return;
                        }
                    }//REQUEST
                    else
                    {
                        ApagaEtiquetas("Transacción errónea");
                        return;
                    }
                    #endregion
                }
            }//
            else
            {
                // En caso contrario, se regresa a la busqueda de predio
                Response.Redirect("~/Buscar.aspx", false);
            }
        }

        public string colorDiv { get { return ViewState["colordiv"].ToString(); } }

        //Genera  comprobantes digitales del pago
        public void ComprobantePago(string idOrden, string noAutorizacion)
        {
            PreparaRecibo pr = new PreparaRecibo();
            MensajesInterfaz msg = new MensajesInterfaz();
            string rutaSM = string.Empty;
            string rutaIP = string.Empty;

            msg = pr.ComprobanteInternet( idOrden, noAutorizacion, ref rutaIP, ref rutaSM);
                       
            if (msg != MensajesInterfaz.TramiteGuardado)
            {
                ApagaEtiquetas("Ocurrió un problema al generar su recibo, favor de comunicarse a la Dirección de Impuesto Predial y Catastro.");
                return;
            }
            pnlRecibo.Visible = true;
            lbSM.Visible = false;
            lbSM.Visible = false;
            string path = Server.MapPath("~/");
            if (rutaIP != "")
            {
                frameIP.Visible = true;
                frameIP.Src = rutaIP; // "~/Recibos/Pdf/" + rutaIP + ".pdf";
                lbIP.Visible = true;
            }
            if (rutaSM != "")
            {
                frameSM.Visible = true;
                frameSM.Src = rutaSM; // "~/Recibos/Pdf/" + rutaSM + ".pdf";
                lbSM.Visible = true;
            }                              
        }

        public void ApagaEtiquetas (string texto)
        {
            pnlRecibo.Visible = false;
            frameSM.Visible = false;
            frameIP.Visible = false;

            lblEstado.Text = texto;
            lblFolio.Text = "";
            lblFechaHora.Text = "";
            lblClaveAutorizacion.Text = "";
            lblCvecatProcesada.Text = "";
            lblImporteTotal.Text = "";

            lblFolio.Visible = true;
            lblFechaHora.Visible = true;
            lblClaveAutorizacion.Visible = true;
            lblCvecatProcesada.Visible = true;
            lblImporteTotal.Visible = true;
            lbIP.Visible = true;
            lbSM.Visible = true;
        }

        protected void btnConsulta_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Buscar.aspx", false);
        }
    }
       
}