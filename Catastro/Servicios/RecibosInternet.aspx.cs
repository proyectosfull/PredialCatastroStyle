using Catastro.Controles;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;


using System.Collections;
using System.IO;

using GeneraRecibo33;
using System.Configuration;
using System.Text;

namespace Catastro.Servicios
{
    public partial class RecibosInternet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["parametro"] = null;
                ViewState["sortCampo"] = "id";
                ViewState["sortOnden"] = "desc";
                llenagrid();
                llenaFiltro();
                limpiaDatosCobros(false);
                txtFiltro.Enabled = false;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }
        private void llenaFiltro()
        {
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
            ddlFiltro.Items.Insert(1, new ListItem("Clave Predial", "IdPredio"));
            ddlFiltro.Items.Insert(2, new ListItem("Ejercicio", "Ejercicio"));
        }

        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];
            

            if (filtro == null)
            {
                grd.DataSource = new tInternetBL().GetFilterPago("", "",  ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                filtro[1] = filtro[0] == "IdPredio" ? new cPredioBL().GetByClavePredial(filtro[1]).Id.ToString():filtro[1];
                grd.DataSource = new tInternetBL().GetFilterPago(filtro[0], filtro[1], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();

            }
        }


        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenagrid();
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ConsultarRegistro")
            {
                Int32 id = Convert.ToInt32( e.CommandArgument.ToString());
                tInternet internet = new tInternetBL().GetByConstraint(id);
                limpiaDatosCobros(true);
                ViewState["idI"] = internet.Id;
                llenaPant(internet.cPredio);
                llenagridCobro(internet.Id);
                lblTotal.Text = internet.ImportePagado.ToString("C");
            }
        }

        protected void limpiaDatosCobros(Boolean activa)
        {
            ViewState["idI"] = null;
            ViewState["tipoImpuesto"] = null;
            txtContruyente.Text = "";
            txtCalle.Text = "";
            txtNumero.Text = "";
            txtColonia.Text = "";
            txtCP.Text = "";
            txtLocalidad.Text = "";
            lblTotal.Text = "";

            grdCobros.DataSource = null;
            grdCobros.DataBind();

            Label14.Visible = activa;
            Label5.Visible = activa;
            txtContruyente.Visible = activa;
            Label8.Visible = activa;
            Label9.Visible = activa;
            txtCalle.Visible = activa;
            Label10.Visible = activa;
            txtNumero.Visible = activa;
            Label11.Visible = activa;
            txtColonia.Visible=activa;
            Label12.Visible = activa;
            txtCP.Visible = activa;
            Label2.Visible = activa;
            txtLocalidad.Visible = activa;
            Label3.Visible = activa;
            grdCobros.Visible = activa;
            btnReciboPredial.Visible = activa;
            btnReciboServicios.Visible = activa;
            btnReciboIpMs.Visible = activa;
            Label16.Visible = activa;
            lblTotal.Visible = activa;

        }

        protected void llenaPant(cPredio Predio)
        {
            txtContruyente.Text = Predio.cContribuyente.ApellidoPaterno + " " + Predio.cContribuyente.ApellidoMaterno + " " + Predio.cContribuyente.Nombre;
            txtCalle.Text = Predio.Calle;
            txtNumero.Text = Predio.Numero;
            txtColonia.Text = Predio.cColonia.NombreColonia;
            txtCP.Text = Predio.CP;
            txtLocalidad.Text = Predio.Localidad;
        }

        private void llenagridCobro(Int32 idInternet)
        {
            grdCobros.Visible = true;
            tInternet internet = new tInternetBL().GetByConstraint(idInternet);
            Int32 idMesa = -1;
            if (internet.IdReciboIP != null)
            {
                idMesa = Convert.ToInt32(internet.IdMesaIP);
                btnReciboPredial.Visible = false;
                btnReciboIpMs.Visible = false;
                
            }

            if (internet.IdReciboSM != null)
            {
                idMesa = Convert.ToInt32(internet.IdMesaSM);
                btnReciboServicios.Visible = false;
                btnReciboIpMs.Visible = false;
            }

                grdCobros.DataSource = new tInternetDetalleBL().GetDetalleCobro(idInternet, idMesa);
                grdCobros.DataBind();            
        }


        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortCampo"] == null)
            {
                ViewState["sortCampo"] = e.SortExpression.ToString();
                ViewState["sortOnden"] = "asc";
            }
            else
            {
                if (e.SortExpression.ToString() == ViewState["sortCampo"].ToString())
                {
                    if (ViewState["sortOnden"].ToString() == "asc")
                        ViewState["sortOnden"] = "desc";
                    else
                        ViewState["sortOnden"] = "asc";
                }
                else
                {
                    ViewState["sortCampo"] = e.SortExpression.ToString();
                    ViewState["sortOnden"] = "asc";
                }
            }

            llenagrid();
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {                
                e.Row.Cells[0].Text = new cPredioBL().GetByConstraint(int.Parse(e.Row.Cells[0].Text)).ClavePredial;
                e.Row.Cells[3].Text = Convert.ToDecimal(e.Row.Cells[3].Text).ToString("C");
            }
        }

        protected void grdCobro_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = new cMesaBL().GetByConstraint( Convert.ToInt32(e.Row.Cells[0].Text)).Descripcion;
                e.Row.Cells[2].Text = Convert.ToDecimal(e.Row.Cells[2].Text).ToString("C");
                e.Row.Cells[3].Text = Convert.ToDecimal(e.Row.Cells[3].Text).ToString("C");
                e.Row.Cells[4].Text = Convert.ToDecimal(e.Row.Cells[4].Text).ToString("C");
            }
        }

        #endregion

       
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {


            string tipoImpuesto = ViewState["tipoImpuesto"].ToString();
            tInternet internet = new tInternetBL().GetByConstraint( Convert.ToInt32(ViewState["idI"]));
            string orden = DateTime.Today.Month + "-" + internet.cPredio.ClavePredial + "-" + internet.Id;
            string rutaPDF = string.Empty;

            if (tipoImpuesto == "A")
            {
                GenerarRecibo("IP", orden, "IP", ref rutaPDF, "","A");
                GenerarRecibo("SM", orden, "SM", ref rutaPDF, "","A");
            }
            else
            {
                GenerarRecibo(tipoImpuesto, orden, tipoImpuesto, ref rutaPDF, "", tipoImpuesto);
            }
              
        }

        public void GenerarRecibo(string tipoImpuesto, string idOrden, string noAutorizacion, ref string rutaPDF, string observacion,String tipoRecibo)
        {
            MensajesInterfaz mensaje = new MensajesInterfaz();
            mensaje = new PreparaRecibo().ProcesaRecibo(tipoImpuesto, idOrden, noAutorizacion, ref rutaPDF, "");

            //VISUALIZA RECIBO
            if (mensaje == MensajesInterfaz.TramiteGuardado)
            {
                modalRecibo.Show();
                switch (tipoRecibo)
                {
                    case "IP":
                    case "SM":
                        frameRecibo.Visible = true;
                        frameRecibo.Src = rutaPDF;
                        frameReciboSM.Visible = false;
                        frameReciboIP.Visible = false;          
                        String Clientscript = "printPdf();";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", Clientscript, true);  
                        break;
                    case "A": 
                        frameRecibo.Visible = false;
                        frameReciboSM.Visible = true;
                        frameReciboIP.Visible = true;

                        if (tipoImpuesto == "IP")
                        {
                            frameReciboIP.Src = rutaPDF;
                            String ClientscriptIP = "printPdf();";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", ClientscriptIP, true);  
                        }
                        else
                        {
                            frameReciboSM.Src = rutaPDF;
                            String ClientscriptSM = "printPdf();";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", ClientscriptSM, true);  
                        }
                        
                        break;
                }

                          
                
            }
            else
            {
                throw new System.ArgumentException(new Utileria().GetDescription(mensaje), "Error");
            }
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text };
            ViewState["filtro"] = filtro;
            llenagrid();
        }

        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            if (ddlFiltro.SelectedValue.ToString() == "")
            {
                txtFiltro.Enabled = false;
            }
            else
            {
                txtFiltro.Enabled = true;
            }
        }

        protected void btnReciboPredial_Click(object sender, EventArgs e)
        {
            tInternet internet = new tInternetBL().GetByConstraint(Convert.ToInt32(ViewState["idI"]));
            if (internet.Estado == "T")
            {
                ViewState["tipoImpuesto"] = "IP";

                tTramite tramite = new tTramiteBL().GetTramiteReciboInternet(internet.IdPredio, 5, internet.PeriodoPagadoIP);
                if (tramite != null)
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription("El predio cuenta con un Pago registrado del Periodo " +
                        tramite.BimestreInicial + "/" + tramite.EjercicioInicial + " al " + tramite.BimestreFinal + "/" + tramite.EjercicioFinal +
                        ", ¿Esta seguro que desea continuar con la generación del recibo?."), ModalPopupMensaje.TypeMesssage.Confirm);
                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ReciboInternetConfirmacion), ModalPopupMensaje.TypeMesssage.Confirm);
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.InternetEstado), ModalPopupMensaje.TypeMesssage.Alert);
            }
          

        }

        protected void btnReciboServicios_Click(object sender, EventArgs e)
        {
            tInternet internet = new tInternetBL().GetByConstraint(Convert.ToInt32(ViewState["idI"]));
            if (internet.Estado == "T")
            {
                ViewState["tipoImpuesto"] = "SM";

                tTramite tramite = new tTramiteBL().GetTramiteReciboInternet(internet.IdPredio, 6, internet.PeriodoPagadoSM);
                if (tramite != null)
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription("El predio cuenta con un Pago registrado del Periodo " +
                        tramite.BimestreInicial + "/" + tramite.EjercicioInicial + " al " + tramite.BimestreFinal + "/" + tramite.EjercicioFinal +
                        ", ¿Esta seguro que desea continuar con la generación del recibo?."), ModalPopupMensaje.TypeMesssage.Confirm);
                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ReciboInternetConfirmacion), ModalPopupMensaje.TypeMesssage.Confirm);
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.InternetEstado), ModalPopupMensaje.TypeMesssage.Alert);
            }
          
        }
       

        protected void imCerrarRecibo_Click(object sender, ImageClickEventArgs e)
        {
            modalRecibo.Hide();

            llenagridCobro(Convert.ToInt32(ViewState["idI"]));
            if (grdCobros.Rows.Count == 0)
            {
                limpiaDatosCobros(false);
                llenagrid();
            }
            
        }

        protected void btnReciboIpMs_Click(object sender, EventArgs e)
        {
            tInternet internet = new tInternetBL().GetByConstraint(Convert.ToInt32(ViewState["idI"]));
            if (internet.Estado == "T")
            {
                ViewState["tipoImpuesto"] = "A"; // A de ambos
                String mensaje = "";
                tTramite tramite = new tTramiteBL().GetTramiteReciboInternet(internet.IdPredio, 5, internet.PeriodoPagadoIP);
                if (tramite != null)
                {
                    mensaje = "El predio cuenta con un Pago de Impuesto Predial registrado del Periodo " +
                        tramite.BimestreInicial + "/" + tramite.EjercicioInicial + " al " + tramite.BimestreFinal + "/" + tramite.EjercicioFinal;
                        
                }

                 tramite = new tTramiteBL().GetTramiteReciboInternet(internet.IdPredio, 6, internet.PeriodoPagadoSM);
                if (tramite != null)
                {
                    mensaje = mensaje == "" ? "El predio cuenta con un Pago " : mensaje+" y otro " + " de Servicio Municipal registrado del Periodo " +
                        tramite.BimestreInicial + "/" + tramite.EjercicioInicial + " al " + tramite.BimestreFinal + "/" + tramite.EjercicioFinal;

                }

                mensaje = mensaje != "" ? mensaje + ", " : " ";
                mensaje = mensaje + "¿Esta seguro que desea continuar con la generación del recibo?.";
                vtnModal.ShowPopup(new Utileria().GetDescription(mensaje), ModalPopupMensaje.TypeMesssage.Confirm);
                
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.InternetEstado), ModalPopupMensaje.TypeMesssage.Alert);
            }
          
        }

    }
}