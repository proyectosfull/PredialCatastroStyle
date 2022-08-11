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

namespace Catastro.Catalogos
{
    public partial class catTarifaRecoleccion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Id";
                ViewState["sortOnden"] = "desc";
                chkInactivo.Checked = true;
                llenagrid();
                llenaFiltro();
                txtFiltro.Enabled = false;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenarTipoCobro() {

            ddlTipoCobro.Items.Clear();
            ddlTipoCobro.DataValueField = "Id";
            ddlTipoCobro.DataTextField = "Descripcion";
            ddlTipoCobro.DataSource = new cTipoCobroBL().GetAll();
            ddlTipoCobro.DataBind();
            ddlTipoCobro.Items.Insert(0, new ListItem("Seleccionar Tipo Cobro", "%"));
            
        }      
        private void llenaFiltro()
        {
            ddlFiltro.DataSource = new cTarifaRecoleccionBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }
        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cTarifaRecoleccionBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cTarifaRecoleccionBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                llenarTipoCobro();
                lbl_titulo.Text = "Consulta Tarifa de Recolección";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cTarifaRecoleccion Tarifa = new cTarifaRecoleccionBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(false);
                txtEjercicio.Text = Tarifa.Ejercicio.ToString();
                txtTarifa.Text = Tarifa.Tarifa.ToString();
                cTipoCobro tipoCobro = new cTipoCobroBL().GetByConstraint(Tarifa.IdTipoCobro);
                if (tipoCobro != null)
                    if (tipoCobro.Activo)
                    {
                        ddlTipoCobro.SelectedValue = Tarifa.IdTipoCobro.ToString();
                    }
                    else
                    {
                        ddlTipoCobro.Items.Add(new ListItem(tipoCobro.Descripcion, tipoCobro.Codigo));
                        ddlTipoCobro.SelectedValue = Tarifa.IdTipoCobro.ToString();
                    }

                btnGuardar.Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                llenarTipoCobro();
                lbl_titulo.Text = "Modifica Tarifa de Recolección";
                int id = Convert.ToInt32(e.CommandArgument);
                cTarifaRecoleccion Tarifa = new cTarifaRecoleccionBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(true);
                btnGuardar.Visible = true;
                txtEjercicio.Text = Tarifa.Ejercicio.ToString();
                txtTarifa.Text = Tarifa.Tarifa.ToString();
                cTipoCobro tipoCobro = new cTipoCobroBL().GetByConstraint(Tarifa.IdTipoCobro);
                if (tipoCobro != null)
                    if (tipoCobro.Activo)
                    {
                        ddlTipoCobro.SelectedValue = Tarifa.IdTipoCobro.ToString();
                    }
                    else
                    {
                        ddlTipoCobro.Items.Add(new ListItem(tipoCobro.Descripcion, tipoCobro.Codigo));
                        ddlTipoCobro.SelectedValue = Tarifa.IdTipoCobro.ToString();
                    }
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
                int idTipoCobro = int.Parse(e.Row.Cells[2].Text);
                e.Row.Cells[2].Text = new cTipoCobroBL().GetByConstraint(idTipoCobro).Descripcion;

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
            lbl_titulo.Text = "Alta Tarifa de Recolección";
            pnl_Modal.Show();
            limpiaCampos();
            llenarTipoCobro();
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
                cTarifaRecoleccion Tarifa = new cTarifaRecoleccion();
                Tarifa.Ejercicio = Convert.ToInt32(txtEjercicio.Text);
                Tarifa.Tarifa = Convert.ToDecimal(txtTarifa.Text);
                Tarifa.IdTipoCobro = Convert.ToInt32( ddlTipoCobro.SelectedValue);
                Tarifa.IdUsuario = U.Id;
                Tarifa.Activo = true;
                Tarifa.FechaModificacion = DateTime.Now;

                MensajesInterfaz msg = new cTarifaRecoleccionBL().Insert(Tarifa);
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                cTarifaRecoleccion Tarifa = new cTarifaRecoleccionBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));

                Tarifa.Ejercicio = Convert.ToInt32(txtEjercicio.Text);
                Tarifa.Tarifa = Convert.ToDecimal(txtTarifa.Text);
                Tarifa.IdTipoCobro = Convert.ToInt32(ddlTipoCobro.SelectedValue);
                Tarifa.IdUsuario = U.Id;
                Tarifa.Activo = true;
                Tarifa.FechaModificacion = DateTime.Now;
                MensajesInterfaz msg = new cTarifaRecoleccionBL().Update(Tarifa);
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
            txtEjercicio.Text = "";
            txtTarifa.Text = "";

            ViewState["idMod"] = null;
        }
        private void habilitaCampos(bool activa)
        {
            txtEjercicio.Enabled = activa;
            txtTarifa.Enabled = activa;
            ddlTipoCobro.Enabled = activa;

        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cTarifaRecoleccion Tarifa = new cTarifaRecoleccionBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Tarifa.Activo = false;
                Tarifa.IdUsuario = U.Id;
                Tarifa.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cTarifaRecoleccionBL().Delete(Tarifa);
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cTarifaRecoleccion Tarifa = new cTarifaRecoleccionBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Tarifa.Activo = true;
                Tarifa.IdUsuario = U.Id;
                Tarifa.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cTarifaRecoleccionBL().Update(Tarifa);
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
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text, chkInactivo.Checked.ToString() };
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