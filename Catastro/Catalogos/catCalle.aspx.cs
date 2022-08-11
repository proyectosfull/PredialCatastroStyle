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
    public partial class catCalle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "NombreCalle";
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
            ddlFiltro.DataSource = new cCalleBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }

        private void llenaTipoVialidad()
        {
            ddlTipoVialidad.Items.Clear();
            ddlTipoVialidad.DataValueField = "Id";
            ddlTipoVialidad.DataTextField = "Descripcion";
            ddlTipoVialidad.DataSource = new cTipoVialidadBL().GetAll();
            ddlTipoVialidad.DataBind();
            ddlTipoVialidad.Items.Insert(0, new ListItem("Seleccionar Cargo", "%"));

        }

        #region Grid
        private void llenagrid()
        {
          string[] filtro =  (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cCalleBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cCalleBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                llenaTipoVialidad();
                lbl_titulo.Text = "Consulta de la Calle";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cCalle calle = new cCalleBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(false);
                txtNombreCalle.Text = calle.NombreCalle;
                if (calle.cTipoVialidad.Activo)
                {
                    ddlTipoVialidad.SelectedValue = calle.cTipoVialidad.Id.ToString();
                }
                else
                {                  
                    ddlTipoVialidad.Items.Add(new ListItem(calle.cTipoVialidad.Descripcion, calle.cTipoVialidad.Id.ToString()));
                    ddlTipoVialidad.SelectedValue = calle.cTipoVialidad.Id.ToString();

                }               
                
                txtDescripcion.Text = calle.Descripcion;
                btnGuardar.Visible = false;
            }
          else if (e.CommandName == "ModificarRegistro")
            {
                llenaTipoVialidad();
                lbl_titulo.Text = "Modificación de la Calle";
                int id = Convert.ToInt32(e.CommandArgument);
                cCalle calle = new cCalleBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(true);
                btnGuardar.Visible = true;
                txtNombreCalle.Text = calle.NombreCalle;
                if (calle.cTipoVialidad.Activo)
                {
                    ddlTipoVialidad.SelectedValue = calle.cTipoVialidad.Id.ToString();
                }
                else
                {
                    ddlTipoVialidad.Items.Add(new ListItem(calle.cTipoVialidad.Descripcion, calle.cTipoVialidad.Id.ToString()));
                    ddlTipoVialidad.SelectedValue = calle.cTipoVialidad.Id.ToString();
                }   
                txtDescripcion.Text = calle.Descripcion;
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
                int idTipoVialidad = int.Parse(e.Row.Cells[2].Text);
                e.Row.Cells[2].Text = new cTipoVialidadBL().GetByConstraint(idTipoVialidad).Descripcion;

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
            llenaTipoVialidad();
            lbl_titulo.Text = "Alta de la Calle";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
            btnGuardar.Visible = true;
            txtNombreCalle.Focus();
        }

        protected void btnGuardar_Click(object sender, EventArgs e){            
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                pnl_Modal.Hide();
                vtnModal.DysplayCancelar = false;
                if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0") {
                    cCalle Calle = new cCalle();
                    Calle.NombreCalle = txtNombreCalle.Text;
                    Calle.IdTipoVialidad = Convert.ToInt32(ddlTipoVialidad.SelectedValue);
                    Calle.Descripcion = txtDescripcion.Text;
                    Calle.IdUsuario = U.Id;
                    Calle.Activo = true;
                    Calle.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cCalleBL().Insert(Calle);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                } else{
                    cCalle Calle = new cCalleBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                    Calle.NombreCalle = txtNombreCalle.Text;
                    Calle.IdTipoVialidad = Convert.ToInt32(ddlTipoVialidad.SelectedValue);
                    Calle.Descripcion = txtDescripcion.Text;
                    Calle.IdUsuario = U.Id;
                    Calle.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cCalleBL().Update(Calle);
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
            txtNombreCalle.Text = "";
            txtDescripcion.Text = "";
            ViewState["idMod"] = null;
        }
        private void habilitaCampos(bool activa)
        {
            txtNombreCalle.Enabled = activa;
           ddlTipoVialidad.Enabled = activa;            
            txtDescripcion.Enabled = activa;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {               
                cCalle Calle = new cCalleBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Calle.Activo = false;
                Calle.IdUsuario = U.Id;
                Calle.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cCalleBL().Delete(Calle);               
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cCalle Calle = new cCalleBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Calle.Activo = true;
                Calle.IdUsuario = U.Id;
                Calle.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cCalleBL().Update(Calle);                
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