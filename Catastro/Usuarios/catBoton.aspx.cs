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
    public partial class catBoton : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Clave";
                ViewState["sortOnden"] = "asc";
                ChkActivos.Checked = true;
                llenaFiltro();
                llenagrid();
                txtFiltro.Enabled = false;
                llenaVentana();
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }
        private void llenaFiltro()
        {           
            ddlFiltro.DataSource = new cBotonBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }
        private void llenaVentana()
        {
            ddlVentana.DataValueField = "Id";
            ddlVentana.DataTextField = "Ventana";
            ddlVentana.DataSource = new cVentanaBL().GetAllVentanaNoPadre();
            ddlVentana.DataBind();
            ddlVentana.Items.Insert(0, new ListItem("Todos", "%"));
        }

        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cBotonBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cBotonBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta Boton";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cBoton Boton = new cBotonBL().GetByConstraint(id);
                limpiaCampos();              
                ddlVentana.SelectedValue = Boton.IdVentana.ToString();
                rblAccesoTotal.SelectedIndex = Convert.ToInt32(Boton.AccesoTotal);
                txtClave.Text = Boton.Clave;
                txtBoton.Text = Boton.Boton;
                txtDescripcion.Text = Boton.Descripcion;
                habilitaCampos(false);
                btn_Guardar.Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica Boton";
                int id = Convert.ToInt32(e.CommandArgument);
                cBoton Boton = new cBotonBL().GetByConstraint(id);
                limpiaCampos();
                btn_Guardar.Visible = true;
                ddlVentana.SelectedValue = Boton.IdVentana.ToString();
                rblAccesoTotal.SelectedIndex = Convert.ToInt32(Boton.AccesoTotal);
                txtClave.Text = Boton.Clave;
                txtBoton.Text = Boton.Boton;
                txtDescripcion.Text = Boton.Descripcion;
                habilitaCampos(true);
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
            lbl_titulo.Text = "Alta Boton";
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
            if (ViewState["idMod"] != null)
            { string a = ViewState["idMod"].ToString(); }
            if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
            {
                cBoton Boton = new cBoton();
                Boton.IdVentana = Convert.ToInt32(ddlVentana.SelectedValue);
                Boton.Clave = txtClave.Text;
                Boton.Boton = txtBoton.Text;
                Boton.Descripcion = txtDescripcion.Text;
                Boton.AccesoTotal = Convert.ToBoolean(rblAccesoTotal.SelectedValue);
                Boton.IdUsuario = U.Id;
                Boton.Activo = true;
                Boton.FechaModificacion = DateTime.Now;
                MensajesInterfaz msg = new cBotonBL().Insert(Boton);
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                cBoton Boton = new cBotonBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Boton.IdVentana = Convert.ToInt32(ddlVentana.SelectedValue);
                Boton.Clave = txtClave.Text;
                Boton.Boton = txtBoton.Text;
                Boton.Descripcion = txtDescripcion.Text;
                Boton.AccesoTotal = Convert.ToBoolean(rblAccesoTotal.SelectedValue);
                Boton.IdUsuario = U.Id;              
                Boton.FechaModificacion = DateTime.Now;
                MensajesInterfaz msg = new cBotonBL().Update(Boton);
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
            txtBoton.Text = "";
            txtDescripcion.Text = "";
            ddlVentana.SelectedValue = "%";
            rblAccesoTotal.SelectedValue = "false";
            ViewState["idMod"] = null;
        }
        private void habilitaCampos(bool activa)
        {
            txtClave.Enabled = activa;
            txtBoton.Enabled = activa;
            txtDescripcion.Enabled = activa;
            ddlVentana.Enabled = activa;
            rblAccesoTotal.Enabled = activa;
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cBoton Boton = new cBotonBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Boton.Activo = false;
                Boton.IdUsuario = U.Id;   
                Boton.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cBotonBL().Delete(Boton);
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cBoton Boton = new cBotonBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Boton.Activo = true;
                Boton.IdUsuario = U.Id;   
                Boton.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cBotonBL().Update(Boton);
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