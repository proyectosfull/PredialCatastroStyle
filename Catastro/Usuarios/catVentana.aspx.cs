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
    public partial class catVentana : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Ventana";
                ViewState["sortOnden"] = "asc";
                ChkActivos.Checked = true;
                llenaFiltro();
                llenagrid();
                txtFiltro.Enabled = false;
                llenaVentana();
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }
        private void llenaVentana()
        {
            ddlVentanaPadre.DataValueField = "Id";
            ddlVentanaPadre.DataTextField = "Ventana";
            ddlVentanaPadre.DataSource = new cVentanaBL().GetAllVentanaPadre();
            ddlVentanaPadre.DataBind();
            ddlVentanaPadre.Items.Insert(0, new ListItem("Sin Ventana Padre", "0"));
        }

        private void llenaFiltro()
        {
            ddlFiltro.DataSource = new cVentanaBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }
        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cVentanaBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cVentanaBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta Ventana";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cVentana Ventana = new cVentanaBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(false);
                txtClave.Text = Ventana.Clave;
                txtVentana.Text = Ventana.Ventana;
                txtUrl.Text = Ventana.Url;
                txtDescripcion.Text = Ventana.Descripcion;
                txtOrdenacion.Text = Ventana.Orden.ToString();
                ddlVentanaPadre.SelectedValue = Ventana.IdPapa.ToString();
                rblMenu.SelectedIndex = Convert.ToInt32(Ventana.VisibleMenu);
                btn_Guardar.Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica Ventana";
                int id = Convert.ToInt32(e.CommandArgument);
                cVentana Ventana = new cVentanaBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(true);
                btn_Guardar.Visible = true;
                txtClave.Text = Ventana.Clave;
                txtVentana.Text = Ventana.Ventana;
                txtUrl.Text = Ventana.Url;
                txtDescripcion.Text = Ventana.Descripcion;
                txtOrdenacion.Text = Ventana.Orden.ToString();
                ddlVentanaPadre.SelectedValue = Ventana.IdPapa.ToString();
                rblMenu.SelectedIndex = Convert.ToInt32(Ventana.VisibleMenu);
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
            lbl_titulo.Text = "Alta Ventana";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
            btn_Guardar.Visible = true;
            txtClave.Focus();
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            pnl_Modal.Hide();
            vtnModal.DysplayCancelar = false;
            if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
            {
                cVentana Ventana = new cVentana();
                Ventana.Clave = txtClave.Text;
                Ventana.Ventana = txtVentana.Text;
                Ventana.Url = txtUrl.Text;
                Ventana.Descripcion = txtDescripcion.Text;
                Ventana.Orden = Convert.ToInt32(txtOrdenacion.Text);
                Ventana.IdPapa =Convert.ToInt32(ddlVentanaPadre.SelectedValue);
                Ventana.VisibleMenu = rblMenu.SelectedValue == "0" ? false : true;
                Ventana.IdUsuario = U.Id;
                Ventana.Activo = true;
                Ventana.FechaModificacion = DateTime.Now;
                MensajesInterfaz msg = new cVentanaBL().Insert(Ventana);
                llenaVentana();
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);                
            }
            else
            {
                cVentana Ventana = new cVentanaBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Ventana.Clave = txtClave.Text;
                Ventana.Ventana = txtVentana.Text;
                Ventana.Url = txtUrl.Text;
                Ventana.Descripcion = txtDescripcion.Text;
                Ventana.Orden = Convert.ToInt32(txtOrdenacion.Text);
                Ventana.IdPapa = Convert.ToInt32(ddlVentanaPadre.SelectedValue);
                Ventana.VisibleMenu = rblMenu.SelectedValue == "0" ? false : true;
                Ventana.IdUsuario = U.Id;              
                Ventana.FechaModificacion = DateTime.Now;
                MensajesInterfaz msg = new cVentanaBL().Update(Ventana);
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
            txtVentana.Text = "";
            txtDescripcion.Text = "";
            txtOrdenacion.Text = "";
            ddlVentanaPadre.SelectedValue = "0";
            txtUrl.Text = "";
            rblMenu.SelectedIndex = 1;
            ViewState["idMod"] = null;
        }
        private void habilitaCampos(bool activa)
        {
            txtClave.Enabled = activa;
            txtVentana.Enabled = activa;
            txtDescripcion.Enabled = activa;
            txtOrdenacion.Enabled = activa;
            ddlVentanaPadre.Enabled = activa;
            txtUrl.Enabled = activa;
            rblMenu.Enabled = activa;
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cVentana Ventana = new cVentanaBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Ventana.Activo = false;
                Ventana.IdUsuario = U.Id;   
                Ventana.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cVentanaBL().Delete(Ventana);
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cVentana Ventana = new cVentanaBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Ventana.Activo = true;
                Ventana.IdUsuario = U.Id;   
                Ventana.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cVentanaBL().Update(Ventana);
                vtnModal.DysplayCancelar = false;
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
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text, ChkActivos.Checked.ToString() };
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