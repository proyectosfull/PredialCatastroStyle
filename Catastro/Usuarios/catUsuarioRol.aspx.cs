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
    public partial class catUsuarioRol : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Usuario";
                ViewState["sortOnden"] = "asc";
                //ChkActivos.Checked = true;
                llenaFiltro();
                txtFiltro.Enabled = false;
                llenaListas();
                llenagrid();
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }
        private void llenaFiltro()
        {
            ddlFiltro.DataSource = new cUsuariosBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }
        private void llenaListas()
        {
            ddlUsuario.DataValueField = "Id";
            ddlUsuario.DataTextField = "Usuario";
            ddlUsuario.DataSource = new cUsuariosBL().GetAll();
            ddlUsuario.DataBind();
            ddlUsuario.Items.Insert(0, new ListItem("Seleccionar Usuario", "%"));

            chkListRoles.DataValueField = "Id";
            chkListRoles.DataTextField = "Rol";
            chkListRoles.DataSource = new cRolBL().GetAll();
            chkListRoles.DataBind();
        }

        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cUsuariosBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cUsuariosBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta Usuarios Roles";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                ddlUsuario.SelectedValue = id.ToString();
                List<mUsuRol> roles = new mUsuRolBL().GetAllUsuario(id);
                limpiaRoles();
                activaRoles(roles);
                habilitaCampos(false);
                btn_Guardar.Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica Usuarios Roles";
                int id = Convert.ToInt32(e.CommandArgument);
                ddlUsuario.SelectedValue = id.ToString();
                List<mUsuRol> roles = new mUsuRolBL().GetAllUsuario(id);
                limpiaRoles();
                activaRoles(roles);
                habilitaCampos(true);
                btn_Guardar.Visible = true;
                pnl_Modal.Show();
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
        #endregion

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            lbl_titulo.Text = "Alta Usuario Rol";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
            btn_Guardar.Visible = true;

        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            if (ddlUsuario.SelectedIndex != 0)
            {
                
                DateTime fecha = DateTime.Now;
                pnl_Modal.Hide();
                vtnModal.DysplayCancelar = false;
                int IdUsuario = Convert.ToInt32(ddlUsuario.SelectedValue);
                List<mUsuRol> roles = new List<mUsuRol>();
                foreach (ListItem item in chkListRoles.Items)
                {
                    if (item.Selected)
                    {
                        mUsuRol rol = new mUsuRol();
                        rol.IdRol = Convert.ToInt32(item.Value);
                        rol.IdUsuario = IdUsuario;
                        rol.Activo = true;
                        rol.IdUsuarioMod = U.Id;
                        rol.FechaModificacion = fecha;
                        roles.Add(rol);
                    }
                }
                MensajesInterfaz msg = new mUsuRolBL().InsertLista(roles, Convert.ToInt32(ddlUsuario.SelectedValue), U.Id, fecha);
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                limpiaCampos();
                llenagrid();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ViewState["idMod"] = 0;
            limpiaCampos();
            pnl_Modal.Hide();
        }
        private void limpiaCampos()
        {
            ddlUsuario.SelectedIndex = 0;
            limpiaRoles();
        }
        private void limpiaRoles()
        {

            foreach (ListItem item in chkListRoles.Items)
            {
                item.Selected = false;
            }
        }
        private void habilitaCampos(bool activa)
        {
            ddlUsuario.Enabled = activa;
            foreach (ListItem item in chkListRoles.Items)
            {
                item.Enabled = activa;
            }
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                //cVentana Ventana = new cVentanaBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                //Ventana.Activo = false;
                //Ventana.IdUsuario = U.Id;
                //Ventana.FechaModificacion = DateTime.Now;
                //MensajesInterfaz resul = new cVentanaBL().Delete(Ventana);
                //vtnModal.DysplayCancelar = false;
                //vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Informacion);
                //ViewState["idMod"] = 0;
                //llenagrid();
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
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text, "true" };
            ViewState["filtro"] = filtro;
            llenagrid();
        }
        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFiltro.SelectedValue.ToString() == "")
            {
                txtFiltro.Text = "";
                txtFiltro.Enabled = false;
            }
            else
            {
                txtFiltro.Enabled = true;
            }
        }

        protected void ddlUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlUsuario.SelectedIndex != 0)
            {
                List<mUsuRol> roles = new mUsuRolBL().GetAllUsuario(Convert.ToInt32(ddlUsuario.SelectedValue));
                limpiaRoles();
                activaRoles(roles);
                habilitaCampos(true);
            }
            pnl_Modal.Show();
        }

        protected void activaRoles(List<mUsuRol> roles)
        {
            foreach (ListItem item in chkListRoles.Items)
            {
                mUsuRol rol = roles.FirstOrDefault(r => r.IdRol == Convert.ToInt32(item.Value));
                if (rol != null)
                    item.Selected = true;
            }
        }

    }
}