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


namespace Catastro.Catalogos
{
    public partial class catTarifaRecargo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Ejercicio";
                ViewState["sortOnden"] = "desc";
                chkInactivo.Checked = true;
                llenagrid();
                llenaFiltro();
                txtFiltro.Enabled = false;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenaFiltro()
        {           
            ddlFiltro.DataSource = new cTarifaRecargoBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }

        #region Grid
        private void llenagrid()
        {
          string[] filtro =  (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cTarifaRecargoBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cTarifaRecargoBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta Tarifa Recargo";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cTarifaRecargo TarifaRecargo = new cTarifaRecargoBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(false);
                txtEjercicio.Text = TarifaRecargo.Ejercicio.ToString();
                ddlMes.SelectedValue = TarifaRecargo.Bimestre.ToString();
                txtPorcentaje.Text = TarifaRecargo.Porcentaje.ToString();
                btnGuardar.Visible = false;
            }
          else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica Tarifa Recargo";
                int id = Convert.ToInt32(e.CommandArgument);
                cTarifaRecargo TarifaRecargo = new cTarifaRecargoBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(true);
                btnGuardar.Visible = true;
                txtEjercicio.Text = TarifaRecargo.Ejercicio.ToString();
                ddlMes.SelectedValue = TarifaRecargo.Bimestre.ToString();
                txtPorcentaje.Text = TarifaRecargo.Porcentaje.ToString();
                ViewState["idMod"] = id;
                pnl_Modal.Show();
            }
            else if (e.CommandName == "EliminarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
            }
            else if (e.CommandName == "ActivarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                vtnModal.DysplayCancelar = true;
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

        #endregion

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            lbl_titulo.Text = "Alta Tarifa Recargo";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
            btnGuardar.Visible = true;
            txtEjercicio.Focus();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                pnl_Modal.Hide();
                vtnModal.DysplayCancelar = false;
                if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
                {
                    cTarifaRecargo TarifaRecargo = new cTarifaRecargo();
                    TarifaRecargo.Ejercicio = Convert.ToInt32(txtEjercicio.Text);
                    TarifaRecargo.Porcentaje = Convert.ToDecimal(txtPorcentaje.Text);
                    TarifaRecargo.Bimestre = Convert.ToInt32(ddlMes.SelectedValue);
                    TarifaRecargo.IdUsuario = U.Id;
                    TarifaRecargo.Activo = true;
                    TarifaRecargo.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cTarifaRecargoBL().Insert(TarifaRecargo);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }
                else
                {
                    cTarifaRecargo TarifaRecargo = new cTarifaRecargoBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                    TarifaRecargo.Ejercicio = Convert.ToInt32(txtEjercicio.Text);
                    TarifaRecargo.Porcentaje = Convert.ToDecimal(txtPorcentaje.Text);
                    TarifaRecargo.Bimestre = Convert.ToInt32(ddlMes.SelectedValue);
                    TarifaRecargo.IdUsuario = U.Id;
                    TarifaRecargo.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cTarifaRecargoBL().Update(TarifaRecargo);
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
            txtEjercicio.Text="";
            txtPorcentaje.Text = "";

            ddlMes.SelectedValue = "1";
            ViewState["idMod"] = null;
        }
        private void habilitaCampos(bool activa)
        {
            txtEjercicio.Enabled = activa;
            txtPorcentaje.Enabled = activa;
            ddlMes.Enabled = activa;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cTarifaRecargo TarifaRecargo = new cTarifaRecargoBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                TarifaRecargo.Activo = false;
                TarifaRecargo.IdUsuario = U.Id;
                TarifaRecargo.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cTarifaRecargoBL().Delete(TarifaRecargo);
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cTarifaRecargo TarifaRecargo = new cTarifaRecargoBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                TarifaRecargo.Activo = true;
                TarifaRecargo.IdUsuario = U.Id;
                TarifaRecargo.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cTarifaRecargoBL().Update(TarifaRecargo);
                vtnModal.DysplayCancelar = true;
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