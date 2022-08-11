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
    public partial class catTipoTramite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Tramite";
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
            ddlFiltro.DataSource = new cTipoTramiteBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }
        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cTipoTramiteBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cTipoTramiteBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta Tipo Tramite";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cTipoTramite Tramite = new cTipoTramiteBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(false);
                txtTramite.Text = Tramite.Tramite.ToString();
                txtDescripcion.Text = Tramite.Descripcion.ToString();
                txtFecha.Text = Tramite.Fecha.ToString();

                btnGuardar.Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica Tipo Tramite";
                int id = Convert.ToInt32(e.CommandArgument);
                cTipoTramite Tramite = new cTipoTramiteBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(true);
                btnGuardar.Visible = true;
                txtTramite.Text = Tramite.Tramite.ToString();
                txtDescripcion.Text = Tramite.Descripcion.ToString();
                txtFecha.Text = Tramite.Fecha.ToString();
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
            lbl_titulo.Text = "Alta Tipo Tramite";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
            btnGuardar.Visible = true;
            txtTramite.Focus();
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            pnl_Modal.Hide();
            vtnModal.DysplayCancelar = false;
            if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
            {
                cTipoTramite Tramite = new cTipoTramite();
                Tramite.Tramite = txtTramite.Text;
                Tramite.Fecha = Convert.ToDateTime(txtFecha.Text);
                Tramite.Descripcion = txtDescripcion.Text;
                Tramite.IdUsuario = U.Id;
                Tramite.Activo = true;
                Tramite.FechaModificacion = DateTime.Now;
                Tramite.ConDescuento = chkDescuento.Checked;
                MensajesInterfaz msg = new cTipoTramiteBL().Insert(Tramite);
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                cTipoTramite Tramite = new cTipoTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));

                Tramite.Tramite = txtTramite.Text;
                Tramite.Fecha = Convert.ToDateTime(txtFecha.Text);
                Tramite.Descripcion = txtDescripcion.Text;
                Tramite.ConDescuento = chkDescuento.Checked;
                Tramite.IdUsuario = U.Id;
                Tramite.Activo = true;
                Tramite.FechaModificacion = DateTime.Now;
                MensajesInterfaz msg = new cTipoTramiteBL().Update(Tramite);
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
            txtDescripcion.Text = "";
            txtTramite.Text = "";
            txtFecha.Text = "";
            ViewState["idMod"] = null;
        }
        private void habilitaCampos(bool activa)
        {
            txtDescripcion.Enabled = activa;
            txtTramite.Enabled = activa;
            txtFecha.Enabled = activa;
            chkDescuento.Enabled = activa;
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cTipoTramite Tramite = new cTipoTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Tramite.Activo = false;
                Tramite.IdUsuario = U.Id;
                Tramite.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cTipoTramiteBL().Delete(Tramite);
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cTipoTramite Tramite = new cTipoTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Tramite.Activo = true;
                Tramite.IdUsuario = U.Id;
                Tramite.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cTipoTramiteBL().Update(Tramite);
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