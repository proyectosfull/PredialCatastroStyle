using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases.BL;
using Catastro.Controles;
using Clases;
using Clases.Utilerias;


namespace Transito.Catalogos
{
    public partial class catDescuento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Clave";
                ViewState["sortOnden"] = "asc";
                chkInactivo.Checked = true;
                llenagrid();
                llenaFiltro();
                txtFiltro.Enabled = false;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }


        private void llenaFiltro()
        {           
            ddlFiltro.DataSource = new cDescuentoBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }

        private void llenagrid()
        {
          string[] filtro =  (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cDescuentoBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cDescuentoBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta Descuento";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cDescuento cDescuento = new cDescuentoBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(false);
                lblUltAct.Text = "Ultima Actualización hecha por " + cDescuento.cUsuarios.ApellidoPaterno + " " + cDescuento.cUsuarios.ApellidoPaterno +" "  +cDescuento.cUsuarios.Nombre + 
                    " el dia " + cDescuento.FechaModificacion.ToString();
                txtClave.Text = cDescuento.Clave;
                txtEjercicio.Text = cDescuento.Ejercicio.ToString();
                txtDescripcion.Text = cDescuento.Descripcion;
                txtAutorizacion.Text = cDescuento.Autorizacion;
                txtFechaInicio.Text = cDescuento.FechaInicio.ToShortDateString();
                txtFechaFin.Text = cDescuento.FechaFin.ToShortDateString();
                txtAnticipadoImpuesto.Text = cDescuento.AnticipadoImpuesto.ToString();
                txtAnticipadoAdicional.Text = cDescuento.AnticipadoAdicional.ToString();
                txtAnticipadoLimpieza.Text = cDescuento.AnticipadoLimpieza.ToString();
                txtAnticipadoRecoleccion.Text = cDescuento.AnticipadoRecoleccion.ToString();
                txtAnticipadoDap.Text = cDescuento.AnticipadoDap.ToString();
                txtActualImpuesto.Text = cDescuento.ActualImpuesto.ToString();
                txtActualAdicional.Text = cDescuento.ActualAdicional.ToString();
                txtActualRecargo.Text = cDescuento.ActualRecargo.ToString();
                txtActualLimpieza.Text = cDescuento.ActualLImpieza.ToString();
                txtActualRecoleccion.Text = cDescuento.ActualRecoleccion.ToString();
                txtActualDap.Text = cDescuento.ActualDap.ToString();
                txtDiferencia.Text = cDescuento.Diferencia.ToString();
                txtDiferenciaRecargo.Text = cDescuento.DiferenciaRecargo.ToString();
                txtRezago.Text = cDescuento.Rezago.ToString();
                txtRezagoRecargo.Text = cDescuento.RezagoRecargo.ToString();
                txtRezagoAdicional.Text = cDescuento.RezagoAdicional.ToString();
                txtBasegravable.Text=cDescuento.Basegravable.ToString();
                txtImporte.Text = cDescuento.Importe.ToString();
                txtMultas.Text = cDescuento.Multas.ToString();
                txtEjecucion.Text = cDescuento.Ejecucion.ToString();
                txtHonorarios.Text = cDescuento.Honorarios.ToString();
                txtAnticipadoInfraestructura.Text = cDescuento.AnticipadoInfraestructura.ToString();
                txtInfraestructura.Text = cDescuento.Infraestructura.ToString();
                btnGuardar.Visible = false;
            }
          else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica Descuento";
                int id = Convert.ToInt32(e.CommandArgument);
                cDescuento cDescuento = new cDescuentoBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(true);
                lblUltAct.Text = "Ultima Actualización hecha por " + cDescuento.cUsuarios.ApellidoPaterno + " " + cDescuento.cUsuarios.ApellidoPaterno + " "+ cDescuento.cUsuarios.Nombre + 
                      " el dia " + cDescuento.FechaModificacion.ToString();
                btnGuardar.Visible = true;
                txtClave.Text = cDescuento.Clave;
                txtEjercicio.Text = cDescuento.Ejercicio.ToString();
                txtDescripcion.Text = cDescuento.Descripcion;
                txtAutorizacion.Text = cDescuento.Autorizacion;
                txtFechaInicio.Text = cDescuento.FechaInicio.ToShortDateString();
                txtFechaFin.Text = cDescuento.FechaFin.ToShortDateString();
                txtAnticipadoImpuesto.Text = cDescuento.AnticipadoImpuesto.ToString();
                txtAnticipadoAdicional.Text = cDescuento.AnticipadoAdicional.ToString();
                txtAnticipadoLimpieza.Text = cDescuento.AnticipadoLimpieza.ToString();
                txtAnticipadoRecoleccion.Text = cDescuento.AnticipadoRecoleccion.ToString();
                txtAnticipadoDap.Text = cDescuento.AnticipadoDap.ToString();
                txtActualImpuesto.Text = cDescuento.ActualImpuesto.ToString();
                txtActualAdicional.Text = cDescuento.ActualAdicional.ToString();
                txtActualRecargo.Text = cDescuento.ActualRecargo.ToString();
                txtActualLimpieza.Text = cDescuento.ActualLImpieza.ToString();
                txtActualRecoleccion.Text = cDescuento.ActualRecoleccion.ToString();
                txtActualDap.Text = cDescuento.ActualDap.ToString();
                txtDiferencia.Text = cDescuento.Diferencia.ToString();
                txtDiferenciaRecargo.Text = cDescuento.DiferenciaRecargo.ToString();
                txtRezago.Text = cDescuento.Rezago.ToString();
                txtRezagoRecargo.Text = cDescuento.RezagoRecargo.ToString();
                txtRezagoAdicional.Text = cDescuento.RezagoAdicional.ToString();
                txtBasegravable.Text = cDescuento.Basegravable.ToString();
                txtImporte.Text = cDescuento.Importe.ToString();
                txtMultas.Text = cDescuento.Multas.ToString();
                txtEjecucion.Text = cDescuento.Ejecucion.ToString();
                txtHonorarios.Text = cDescuento.Honorarios.ToString();
                txtAnticipadoInfraestructura.Text = cDescuento.AnticipadoInfraestructura.ToString();
                txtInfraestructura.Text = cDescuento.Infraestructura.ToString();
                ViewState["idMod"] = id;
                pnl_Modal.Show();
            }
            else if (e.CommandName == "EliminarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;                
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
            }
            else if (e.CommandName == "ActivarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;             
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro), ModalPopupMensaje.TypeMesssage.Confirm);
            }

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
                string activo = grd.DataKeys[e.Row.RowIndex].Values[1].ToString();
                if (activo.ToUpper() == "TRUE")
                {
                    ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    imgActivar.Visible = false;
                }
                else
                {
                    ImageButton imgConsulta = (ImageButton)e.Row.FindControl("imgConsulta");
                    imgConsulta.Visible = false;
                    ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgUpdate");
                    imgUpdate.Visible = false;
                    ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    imgDelete.Visible = false;
                }
            }          
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            lbl_titulo.Text = "Alta de Nuevos Descuentos";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
            btnGuardar.Visible = true;
            txtClave.Focus();
        }

        protected Decimal toDecimal(string o) 
        {
            return o!=""? Convert.ToDecimal(o.Replace(".", ".")):0;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            pnl_Modal.Hide();
            vtnModal.DysplayCancelar = false;
            if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
            {
                cDescuento cDescuento = new cDescuento();
                cDescuento.Clave = txtClave.Text;
                cDescuento.Ejercicio = Convert.ToInt32(txtEjercicio.Text);
                cDescuento.Descripcion = txtDescripcion.Text;
                cDescuento.Autorizacion = txtAutorizacion.Text;
                cDescuento.FechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
                cDescuento.FechaFin = Convert.ToDateTime(txtFechaFin.Text);
                cDescuento.AnticipadoImpuesto = txtAnticipadoImpuesto.Text!=""?Convert.ToDecimal(txtAnticipadoImpuesto.Text):0;
                cDescuento.AnticipadoAdicional = txtAnticipadoAdicional.Text!=""?Convert.ToDecimal(txtAnticipadoAdicional.Text):0;
                cDescuento.AnticipadoLimpieza = txtAnticipadoLimpieza.Text!=""?Convert.ToDecimal(txtAnticipadoLimpieza.Text):0;
                cDescuento.AnticipadoRecoleccion = txtAnticipadoRecoleccion.Text!=""?Convert.ToDecimal(txtAnticipadoRecoleccion.Text):0;
                cDescuento.AnticipadoDap = txtAnticipadoDap.Text!=""?Convert.ToDecimal(txtAnticipadoDap.Text):0;
                cDescuento.AnticipadoInfraestructura = txtAnticipadoInfraestructura.Text!=""?Convert.ToDecimal(txtAnticipadoInfraestructura.Text):0;
                cDescuento.ActualImpuesto = txtActualImpuesto.Text!=""? Convert.ToDecimal(txtActualImpuesto.Text):0;
                cDescuento.ActualAdicional = txtActualAdicional.Text!=""?Convert.ToDecimal(txtActualAdicional.Text):0;
                cDescuento.ActualRecargo = txtActualRecargo.Text!=""?Convert.ToDecimal(txtActualRecargo.Text):0;
                cDescuento.ActualLImpieza = txtActualLimpieza.Text!=""?Convert.ToDecimal(txtActualLimpieza.Text):0;
                cDescuento.ActualRecoleccion = txtActualRecoleccion.Text!=""?Convert.ToDecimal(txtActualRecoleccion.Text):0;
                cDescuento.ActualDap = txtActualDap.Text!=""?Convert.ToDecimal(txtActualDap.Text):0;
                cDescuento.Diferencia = txtDiferencia.Text!=""?Convert.ToDecimal(txtDiferencia.Text):0;
                cDescuento.DiferenciaRecargo = txtDiferenciaRecargo.Text!=""?Convert.ToDecimal(txtDiferenciaRecargo.Text):0;
                cDescuento.Rezago = txtRezago.Text!=""?Convert.ToDecimal(txtRezago.Text):0;
                cDescuento.RezagoRecargo = txtRezagoRecargo.Text != "" ? Convert.ToDecimal(txtRezagoRecargo.Text) : 0;
                cDescuento.RezagoAdicional = txtRezagoAdicional.Text!=""?Convert.ToDecimal(txtRezagoAdicional.Text):0;
                cDescuento.Basegravable = txtBasegravable.Text!=""? Convert.ToDecimal(txtBasegravable.Text):0;
                cDescuento.Importe = txtImporte.Text!=""?Convert.ToDecimal(txtImporte.Text):0;
                cDescuento.Multas = txtMultas.Text!=""?Convert.ToDecimal(txtMultas.Text):0;
                cDescuento.Ejecucion = txtEjecucion.Text!=""?Convert.ToDecimal(txtEjecucion.Text):0;
                cDescuento.Honorarios = txtHonorarios.Text != "" ? Convert.ToDecimal(txtHonorarios.Text) : 0;
                cDescuento.Infraestructura = txtInfraestructura.Text!=""?Convert.ToDecimal(txtInfraestructura.Text):0;
                cDescuento.IdUsuario = U.Id;
                cDescuento.Activo = true;
                cDescuento.FechaModificacion = DateTime.Now;
                MensajesInterfaz msg = new cDescuentoBL().Insert(cDescuento);
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                cDescuento cDescuento = new cDescuentoBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                cDescuento.Clave = txtClave.Text;
                cDescuento.Ejercicio = Convert.ToInt32(txtEjercicio.Text);
                cDescuento.Descripcion = txtDescripcion.Text;
                cDescuento.Autorizacion = txtAutorizacion.Text;
                cDescuento.FechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
                cDescuento.FechaFin = Convert.ToDateTime(txtFechaFin.Text);
                cDescuento.AnticipadoImpuesto = txtAnticipadoImpuesto.Text!=""?Convert.ToDecimal(txtAnticipadoImpuesto.Text):0;
                cDescuento.AnticipadoAdicional = txtAnticipadoAdicional.Text!=""?Convert.ToDecimal(txtAnticipadoAdicional.Text):0;
                cDescuento.AnticipadoLimpieza = txtAnticipadoLimpieza.Text!=""?Convert.ToDecimal(txtAnticipadoLimpieza.Text):0;
                cDescuento.AnticipadoRecoleccion = txtAnticipadoRecoleccion.Text!=""?Convert.ToDecimal(txtAnticipadoRecoleccion.Text):0;
                cDescuento.AnticipadoDap = txtAnticipadoDap.Text!=""?Convert.ToDecimal(txtAnticipadoDap.Text):0;
                cDescuento.AnticipadoInfraestructura = txtAnticipadoInfraestructura.Text!=""?Convert.ToDecimal(txtAnticipadoInfraestructura.Text):0;
                cDescuento.ActualImpuesto = txtActualImpuesto.Text!=""? Convert.ToDecimal(txtActualImpuesto.Text):0;
                cDescuento.ActualAdicional = txtActualAdicional.Text!=""?Convert.ToDecimal(txtActualAdicional.Text):0;
                cDescuento.ActualRecargo = txtActualRecargo.Text!=""?Convert.ToDecimal(txtActualRecargo.Text):0;
                cDescuento.ActualLImpieza = txtActualLimpieza.Text!=""?Convert.ToDecimal(txtActualLimpieza.Text):0;
                cDescuento.ActualRecoleccion = txtActualRecoleccion.Text!=""?Convert.ToDecimal(txtActualRecoleccion.Text):0;
                cDescuento.ActualDap = txtActualDap.Text!=""?Convert.ToDecimal(txtActualDap.Text):0;
                cDescuento.Diferencia = txtDiferencia.Text!=""?Convert.ToDecimal(txtDiferencia.Text):0;
                cDescuento.DiferenciaRecargo = txtDiferenciaRecargo.Text!=""?Convert.ToDecimal(txtDiferenciaRecargo.Text):0;
                cDescuento.Rezago = txtRezago.Text!=""?Convert.ToDecimal(txtRezago.Text):0;
                cDescuento.RezagoRecargo = txtRezagoRecargo.Text != "" ? Convert.ToDecimal(txtRezagoRecargo.Text) : 0;
                cDescuento.RezagoAdicional = txtRezagoAdicional.Text!=""?Convert.ToDecimal(txtRezagoAdicional.Text):0;
                cDescuento.Basegravable = txtBasegravable.Text!=""? Convert.ToDecimal(txtBasegravable.Text):0;
                cDescuento.Importe = txtImporte.Text!=""?Convert.ToDecimal(txtImporte.Text):0;
                cDescuento.Multas = txtMultas.Text!=""?Convert.ToDecimal(txtMultas.Text):0;
                cDescuento.Ejecucion = txtEjecucion.Text!=""?Convert.ToDecimal(txtEjecucion.Text):0;
                cDescuento.Honorarios = txtHonorarios.Text != "" ? Convert.ToDecimal(txtHonorarios.Text) : 0;
                cDescuento.Infraestructura = txtInfraestructura.Text!=""?Convert.ToDecimal(txtInfraestructura.Text):0;
                cDescuento.IdUsuario = U.Id;
                cDescuento.FechaModificacion = DateTime.Now;
                MensajesInterfaz msg = new cDescuentoBL().Update(cDescuento);
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            }
           
            limpiaCampos();
            llenagrid();

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ViewState["idMod"] = 0;
            limpiaCampos();
            pnl_Modal.Hide();
        }

        private void limpiaCampos()
        {
            txtClave.Text = "";
            txtEjercicio.Text = "";
            txtDescripcion.Text = "";
            txtAutorizacion.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            txtAnticipadoImpuesto.Text = "";
            txtAnticipadoAdicional.Text = "";
            txtAnticipadoLimpieza.Text = "";
            txtAnticipadoRecoleccion.Text = "";
            txtAnticipadoDap.Text = "";
            txtActualImpuesto.Text = "";
            txtActualAdicional.Text = "";
            txtActualRecargo.Text = "";
            txtActualLimpieza.Text = "";
            txtActualRecoleccion.Text = "";
            txtActualDap.Text = "";
            txtDiferencia.Text = "";
            txtDiferenciaRecargo.Text = "";
            txtRezago.Text = "";
            txtRezagoRecargo.Text = "";
            txtRezagoAdicional.Text = "";
            txtBasegravable.Text = "";
            txtImporte.Text = "";
            txtMultas.Text = "";
            txtEjecucion.Text = "";
            txtHonorarios.Text = "";
            txtAnticipadoInfraestructura.Text = "";
            txtInfraestructura.Text = "";
            ViewState["idMod"] = null;
            pnl1.Focus();
        }
        private void habilitaCampos(bool activa)
        {
            txtClave.Enabled = activa;
            txtEjercicio.Enabled = activa;
            txtDescripcion.Enabled = activa;
            txtAutorizacion.Enabled = activa;
            txtFechaInicio.Enabled = activa;
            txtFechaFin.Enabled = activa;
            txtAnticipadoImpuesto.Enabled = activa;
            txtAnticipadoAdicional.Enabled = activa;
            txtAnticipadoLimpieza.Enabled = activa;
            txtAnticipadoRecoleccion.Enabled = activa;
            txtAnticipadoDap.Enabled = activa;
            txtActualImpuesto.Enabled = activa;
            txtActualAdicional.Enabled = activa;
            txtActualRecargo.Enabled = activa;
            txtActualLimpieza.Enabled = activa;
            txtActualRecoleccion.Enabled = activa;
            txtActualDap.Enabled = activa;
            txtDiferencia.Enabled = activa;
            txtDiferenciaRecargo.Enabled = activa;
            txtRezago.Enabled = activa;
            txtRezagoRecargo.Enabled = activa;
            txtRezagoAdicional.Enabled = activa;
            txtBasegravable.Enabled = activa;
            txtImporte.Enabled = activa;
            txtMultas.Enabled = activa;
            txtEjecucion.Enabled = activa;
            txtHonorarios.Enabled = activa;
            txtAnticipadoInfraestructura.Enabled = activa;
            txtInfraestructura.Enabled = activa;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cDescuento cDescuento = new cDescuentoBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                cDescuento.Activo = false;
                cDescuento.IdUsuario = U.Id;
                cDescuento.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cDescuentoBL().Delete(cDescuento);               
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cDescuento cDescuento = new cDescuentoBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                cDescuento.Activo = true;
                cDescuento.IdUsuario = U.Id;
                cDescuento.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cDescuentoBL().Update(cDescuento);                
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                llenagrid();
                limpiaCampos();
            }
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text ,chkInactivo.Checked.ToString()};            
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

   

    
    }
}