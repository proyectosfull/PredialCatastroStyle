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

namespace Catastro.Configuracion
{
    public partial class catUsuariosBoton : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Nombre";
                ViewState["sortOnden"] = "asc";
                chkInactivo.Checked = true;
                llenaDdl();
                llenagrid();
                llenaFiltro();
                txtFiltro.Enabled = false;
                ViewState["ventana"] = "0";
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }
        private void llenaDdl()
        {
            ddlUsuario.DataValueField = "Id";
            ddlUsuario.DataTextField = "Usuario";
            ddlUsuario.DataSource = new cUsuariosBL().GetAll();
            ddlUsuario.DataBind();
            ddlBotones.DataValueField = "Id";
            ddlBotones.DataTextField = "Boton";
            ddlBotones.DataSource = new cBotonBL().GetAll();
            ddlBotones.DataBind();
        }

    private void llenaFiltro()
        {
            ddlFiltro.DataSource = new cUsuariosBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }

        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cUsuariosBL().GetFilterConBotones("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cUsuariosBL().GetFilterConBotones(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta Usuario";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                ddlUsuario.SelectedValue = id.ToString();
                habilitaCampos(false);
                llenagridBoton(id);
                grdBoton.Columns[4].Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica Permiso";
                int id = Convert.ToInt32(e.CommandArgument);
                limpiaCampos();
                habilitaCampos(true);
                ddlUsuario.Enabled = false;
                ddlUsuario.SelectedValue = id.ToString();
                llenagridBoton(id);
                pnl_Modal.Show();
                grdBoton.Columns[4].Visible = true;
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
            lbl_titulo.Text = "Agrega Permiso";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            pnl_Modal.Hide();
            vtnModal.DysplayCancelar = false;
            if (!ddlUsuario.SelectedValue.Equals("0") || !ddlBotones.SelectedValue.Equals("0"))
            {
                int idUsuario = int.Parse(ddlUsuario.SelectedValue);
                int idBoton = int.Parse(ddlBotones.SelectedValue);
                if (!new mUsuBotBL().ExisteBoton(idUsuario, idBoton))
                {
                    mUsuBot musu = new mUsuBot();
                    musu.IdBoton = idBoton;
                    musu.IdUsuario = idUsuario;
                    musu.Activo = true;
                    musu.IdUsuarioMod = ((cUsuarios)Session["usuario"]).Id;
                    musu.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new mUsuBotBL().Insert(musu);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                    llenagridBoton(idUsuario);
                    ViewState["ventana"] = "1";
                }              
                
            }
            limpiaCampos();
            
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ViewState["idMod"] = 0;
            limpiaCampos();
            pnl_Modal.Hide();
        }

        private void limpiaCampos()
        {
        }
        private void habilitaCampos(bool activa)
        {
            ddlUsuario.Enabled = activa;
            ddlBotones.Visible = activa;
            btnAgregar.Visible = activa;
            lblBoton.Visible = activa;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                if (Convert.ToInt32(ViewState["idMod"]) != 0 && Convert.ToInt32(ViewState["idModBoton"]) == 0)
                {
                    MensajesInterfaz resul = new mUsuBotBL().ActivaDesactiva(Convert.ToInt32(ViewState["idMod"]), ((cUsuarios)Session["usuario"]).Id, false);
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                    ViewState["idMod"] = 0;
                    llenagrid();
                }
                if (Convert.ToInt32(ViewState["idModBoton"]) != 0)
                {
                    MensajesInterfaz resul = new mUsuBotBL().ActivaDesactivaBoton(Convert.ToInt32(ViewState["idMod"]), Convert.ToInt32(ViewState["idModBoton"]), ((cUsuarios)Session["usuario"]).Id, false);
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                    ViewState["idModBoton"] = 0;
                    llenagridBoton(Convert.ToInt32(ViewState["idMod"]));
                    ViewState["ventana"] = "1";
                }
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                if (Convert.ToInt32(ViewState["idMod"]) != 0 && Convert.ToInt32(ViewState["idModBoton"]) == 0)
                {
                    MensajesInterfaz resul = new mUsuBotBL().ActivaDesactiva(Convert.ToInt32(ViewState["idMod"]), ((cUsuarios)Session["usuario"]).Id, true);
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                    ViewState["idMod"] = 0;
                    llenagrid();
                }
                if (Convert.ToInt32(ViewState["idModBoton"]) != 0)
                {
                    MensajesInterfaz resul = new mUsuBotBL().ActivaDesactivaBoton(Convert.ToInt32(ViewState["idMod"]), Convert.ToInt32(ViewState["idModBoton"]), ((cUsuarios)Session["usuario"]).Id, true);
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                    ViewState["idModBoton"] = 0;
                    llenagridBoton(Convert.ToInt32(ViewState["idMod"]));
                    ViewState["ventana"] = "1";
                }
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                llenagrid();
                limpiaCampos();
                if (ViewState["ventana"].ToString().Equals("1"))
                {
                    pnl_Modal.Show();
                    ViewState["ventana"] = "0";
                }
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

        #region Grid
        private void llenagridBoton(int idUsuario)
        {

            grdBoton.DataSource = new cBotonBL().GetAllBotonesByUsuario(idUsuario);
            grdBoton.DataBind();
        }
        protected void grdBoton_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenagridBoton(Convert.ToInt32(ddlUsuario.SelectedValue));
        }
        protected void grdBoton_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idModBoton"] = id;
                ViewState["idMod"] = int.Parse(ddlUsuario.SelectedValue);
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
            }
            else if (e.CommandName == "ActivarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idModBoton"] = id;
                ViewState["idMod"] = int.Parse(ddlUsuario.SelectedValue);
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro), ModalPopupMensaje.TypeMesssage.Confirm);
            }

        }
        protected void grdBoton_Sorting(object sender, GridViewSortEventArgs e)
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
        protected void grdBoton_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string activo = grdBoton.DataKeys[e.Row.RowIndex].Values[1].ToString();

                if (activo.ToUpper() == "TRUE")
                {
                    ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    imgActivar.Visible = false;
                }
                else
                {                    
                    ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    imgDelete.Visible = false;
                }
            }

        }
        #endregion
    }
}