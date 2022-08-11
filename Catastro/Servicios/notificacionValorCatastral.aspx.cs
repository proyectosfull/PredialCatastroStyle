using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Transactions;
using System.Web.UI.WebControls;
using Clases.BL;
using Catastro.Controles;
using Clases;
using Clases.Utilerias;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Configuration;



namespace Catastro.Servicios
{
    public partial class notificacionValorCatastral : System.Web.UI.Page
    {       
        protected void Page_Load(object sender, EventArgs e)
        {                     
            if (!IsPostBack)
            {
                Session["parametro"] = null;                                               
                                           
            }       
        }

        protected void buscarClaveCatastral(object sender, System.EventArgs e)
        {
            int eActual = DateTime.Now.Year;
            double mesActual = Utileria.Redondeo(DateTime.Now.Month / 2.0);
            int bActual = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(DateTime.Now.Month / 2.0)));

            if (txtClvCastatral.Text.Length == 12)
            {
                cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
               if (Predio.cStatusPredio.Descripcion == "S")
                {
                    limpiaCampos();
                    vtnModal.ShowPopup(new Utileria().GetDescription("El predio esta suspendido, favor de revisar la clave con el administrador"), ModalPopupMensaje.TypeMesssage.Alert);
                    txtClvCastatral.Text = "";

                }
                else if (Predio.cStatusPredio.Descripcion == "B")
                {
                    limpiaCampos();
                    vtnModal.ShowPopup(new Utileria().GetDescription("El predio esta dado de baja, favor de revisar la clave con el administrador"), ModalPopupMensaje.TypeMesssage.Alert);
                    txtClvCastatral.Text = "";
                }
                else if (Predio != null)
                {
                    if (Predio.cStatusPredio.Descripcion == "A")
                    {
                        limpiaCampos();
                        llenaDatos(Predio);
                    }
                    else
                    {
                        limpiaCampos();
                        vtnModal.ShowPopup(new Utileria().GetDescription(Predio.cStatusPredio.Descripcion == "B" ? MensajesInterfaz.sTatusPredioBaja : MensajesInterfaz.sTatusPredioSuspendido
                            ), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                }
                else
                {
                    limpiaCampos();
                    vtnModal.ShowPopup(new Utileria().GetDescription("Predio No Encontrado"), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            else
            {
                limpiaCampos();
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);                
            }
        }

        protected void llenaDatos(cPredio Predio)
        {          
            ViewState["idP"] = Predio.Id.ToString();
            lblClaveCastatral.Text = Predio.ClavePredial;
            lblSupTerreno.Text = Convert.ToDouble(Predio.SuperficieTerreno).ToString("N") + " M2   + "+ Convert.ToDouble(Predio.TerrenoComun).ToString("N")+ " M2"+ " A.C";
            lblValorTerreno.Text = Predio.ValorTerreno == null ? "$0" : Convert.ToDouble(Predio.ValorTerreno).ToString("C");
            lblSupConstruccion.Text = Convert.ToDouble(Predio.SuperficieConstruccion).ToString("N") + " M2 ";
            lblValorConstruccion.Text = Predio.ValorConstruccion.ToString("C");
            lblUbicacion.Text = Predio.Calle + " " + Predio.Numero + " " + Predio.cColonia.NombreColonia;
            lblLocalidad.Text = Predio.Localidad;
            lblMunicipio.Text = "EMILIANO ZAPATA";
            lblNombre.Text = Predio.cContribuyente.RazonSocial+' '+  Predio.cContribuyente.ApellidoPaterno + " " + Predio.cContribuyente.ApellidoMaterno+ " " + Predio.cContribuyente.Nombre ;
            lblDomicilio.Text = Predio.cContribuyente.Calle + " " + Predio.cContribuyente.Numero + " " + Predio.cContribuyente.Colonia + " " + Predio.cContribuyente.Localidad
                + " " + Predio.cContribuyente.Municipio;
            lblValorCatastral.Text = Predio.ValorCatastral.ToString("C");
            lblReferencia.Text = Predio.Referencia;
            txtObservaciones.Text = new cPredioObservacionBL().GetObservacionByIdPredio(Predio.Id); ;
            txtValorAumentado.Text = "";
            txtSubAumento.Text = "";
            txtNotificador.Text = "";
            txtEnPoder.Text = "";
            txtQuienDijo.Text = "";
            txtNotificador.ReadOnly = false;
            txtEnPoder.ReadOnly = false;
            txtQuienDijo.ReadOnly = false;
            txtObservaciones.ReadOnly = false;
            txtValorAumentado.ReadOnly = false;
            txtSubAumento.ReadOnly = false;

        }
        protected void limpiaCampos()
        {
            lblClaveCastatral.Text = "";
            lblSupTerreno.Text = "";
            lblValorTerreno.Text = "";
            lblSupConstruccion.Text = "";
            lblValorConstruccion.Text = "";
            lblUbicacion.Text = "";
            lblLocalidad.Text = "";
            lblMunicipio.Text = "";
            lblNombre.Text = "";
            lblDomicilio.Text = "";
            lblValorCatastral.Text = "";
            txtObservaciones.Text = "";
            txtValorAumentado.Text = "";
            txtSubAumento.Text = "";
            txtNotificador.Text = "";
            txtEnPoder.Text = "";
            txtQuienDijo.Text = "";
            txtNotificador.ReadOnly = true;
            txtEnPoder.ReadOnly = true;
            txtQuienDijo.ReadOnly = true;
            txtObservaciones.ReadOnly = true;
            txtValorAumentado.ReadOnly = true;
            txtSubAumento.ReadOnly = true;
            ViewState["idP"] = null;
        }       

        protected void btnRecargar_Click(object sender, EventArgs e)
        {
            Response.Redirect("notificacionValorCatastral.aspx");
        }

        protected void btnGenerarReporte_Click(object sender, EventArgs e)
        {
             try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
                    MensajesInterfaz msg = new MensajesInterfaz();
                    cUsuarios U = new cUsuarios();
                    U = (cUsuarios)Session["usuario"];
                    tTramite tramite = new tTramite();
                    tramite.IdPredio = Convert.ToInt32(ViewState["idP"]);
                    tramite.Fecha = DateTime.Now;
                    tramite.Status = "I";
                    tramite.NombreAdquiriente = lblNombre.Text;
                    tramite.IdTipoTramite = 8; //Notificación de valor catastral
                    if (lblSupTerreno.Text != "")
                        tramite.SuperficieTerreno = Predio.SuperficieTerreno;
                    
                    if (txtObservaciones.Text != "" )
                        tramite.Observacion = txtObservaciones.Text;
                    else
                        tramite.Observacion = "";
                    //tramite.IsabiForaneo = false;
                    tramite.FechaModificacion = DateTime.Now;
                    if (txtSubAumento.Text != "") 
                    {
                        tramite.valorComercial = Convert.ToDouble(txtSubAumento.Text);//superficie aumento
                    }

                    if (txtValorAumentado.Text != "")
                    {
                        tramite.valorComercial = Convert.ToDouble(txtValorAumentado.Text);//valor aumentado
                    }
                    tramite.Activo = true;
                    tramite.IdUsuario = U.Id;
                    msg = new tTramiteBL().Insert(tramite);
                    if (msg != MensajesInterfaz.Ingreso)
                    {
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    }

                    fillPDF();
                    frameRecibo.Src = "~/Temporales/" + ViewState["pdf"].ToString();
                    modalRecibo.Show();
                    String Clientscript = "printPdf();";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", Clientscript, true);
                    //FIN DE LA TRANSACCION 
                    scope.Complete();
                }
            }
             catch (Exception error)
            {
                new Utileria().logError("Cobros.cobrarGenerarRecibo.Exception", error);
                vtnModal.ShowPopup(new Utileria().GetDescription(error.Message), ModalPopupMensaje.TypeMesssage.Alert);
                

            }
        }

        protected void fillPDF()
        {
            String municipo = new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor;
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];

            String paht = Server.MapPath("~/");
            String formOriginal = paht + "Documentos/notificacionValorCatastralPDF/" + "notificacionValorCatastral_" + municipo + ".pdf";
            String ruta = lblClaveCastatral.Text.Trim() + "_" + "notificacionValorCatastral_" + municipo + ".pdf"; 
            String formImprimir = paht + "/Temporales/" + ruta;
            ViewState["pdf"] = ruta;

            PdfReader reader = new PdfReader(formOriginal);
            PdfStamper stam = new PdfStamper(reader, new FileStream(formImprimir, FileMode.Create));
           
            stam.AcroFields.SetField("{{dia}}", DateTime.Now.ToString("dd"));
            stam.AcroFields.SetField("{{mes}}", DateTime.Now.ToString("MMMM").ToUpper());
            stam.AcroFields.SetField("{{ejercicio}}", DateTime.Now.ToString("yyyy"));
            stam.AcroFields.SetField("{{hora}}", DateTime.Now.Hour.ToString());
            stam.AcroFields.SetField("{{minutos}}", DateTime.Now.Minute.ToString());
            if (lblNombre.Text != "") stam.AcroFields.SetField("{{nombreContribuyente}}", HttpUtility.HtmlDecode(lblNombre.Text));
            if (lblClaveCastatral.Text != "") stam.AcroFields.SetField("{{claveCatastral}}", lblClaveCastatral.Text);
            if (lblSupTerreno.Text != "") stam.AcroFields.SetField("{{supTerreno}}", lblSupTerreno.Text);
            if (lblValorTerreno.Text != "") stam.AcroFields.SetField("{{valorTerreno}}", lblValorTerreno.Text);
            if (lblSupConstruccion.Text != "") stam.AcroFields.SetField("{{supConstruccion}}", lblSupConstruccion.Text);
            if (lblValorConstruccion.Text != "") stam.AcroFields.SetField("{{valorConstruccion}}", lblValorConstruccion.Text);
            if (lblUbicacion.Text != "") stam.AcroFields.SetField("{{ubicacion}}", HttpUtility.HtmlDecode(lblUbicacion.Text));
            if (lblLocalidad.Text != "") stam.AcroFields.SetField("{{localidad}}", HttpUtility.HtmlDecode(lblLocalidad.Text));
            if (lblMunicipio.Text != "") stam.AcroFields.SetField("{{municipio}}", HttpUtility.HtmlDecode(lblMunicipio.Text));
            if (lblDomicilio.Text != "") stam.AcroFields.SetField("{{domicilioContribuyente}}", HttpUtility.HtmlDecode(lblDomicilio.Text));
            if (lblReferencia.Text != "") stam.AcroFields.SetField("{{Referencia}}", HttpUtility.HtmlDecode(lblReferencia.Text));


            if (lblValorCatastral.Text != "") stam.AcroFields.SetField("{{valorCatastral}}", lblValorCatastral.Text);
            if (txtObservaciones.Text != "") stam.AcroFields.SetField("{{observaciones}}", HttpUtility.HtmlDecode(txtObservaciones.Text));
            if (txtEnPoder.Text != "") stam.AcroFields.SetField("{{enPoder}}", HttpUtility.HtmlDecode(txtEnPoder.Text));
            if (txtQuienDijo.Text != "") stam.AcroFields.SetField("{{quienDijo}}", HttpUtility.HtmlDecode(txtQuienDijo.Text));
            if (txtNotificador.Text != "") stam.AcroFields.SetField("{{notificador}}", HttpUtility.HtmlDecode(txtNotificador.Text));
            if (txtValorAumentado.Text != "")
                stam.AcroFields.SetField("{{valorAumentado}}", Convert.ToDouble(txtValorAumentado.Text).ToString("C"));
            else
                stam.AcroFields.SetField("{{valorAumentado}}","XXXXX");
            if (txtSubAumento.Text != "")
                stam.AcroFields.SetField("{{supAumentado}}", Convert.ToDouble(txtSubAumento.Text).ToString("N") + " M2 ");
            else
                stam.AcroFields.SetField("{{supAumentado}}", "XXXXX");
            stam.AcroFields.SetField("{{puestoPredio}}",  HttpUtility.HtmlDecode(new cParametroSistemaBL().GetValorByClave("PuestoPredial")));
            stam.AcroFields.SetField("{{directorPredial}}", HttpUtility.HtmlDecode(new cParametroSistemaBL().GetValorByClave("DirectorPredial")));
            stam.AcroFields.SetField("Elaboro", HttpUtility.HtmlDecode(U.Usuario + " " + DateTime.Today.ToString()));
            stam.FormFlattening = true;
            stam.Close();
        }

        protected void imCerrarRecibo_Click(object sender, ImageClickEventArgs e)
        {
            modalRecibo.Hide();
        }        
    }
}