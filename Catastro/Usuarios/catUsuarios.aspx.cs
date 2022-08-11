using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Catastro.Controles;
using Clases;
using Clases.BL;
using Clases.Utilerias;


namespace Catastro.Catalogos
{
    public partial class catUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                ViewState["sortCampo"] = "Nombre";
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
            List<String> lista = new cUsuariosBL().ListaCampos();
            lista.Remove("Contrasenia");
            ddlFiltro.DataSource = lista;
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
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
                lbl_titulo.Text = "Consulta Usuario";              
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cUsuarios usuarios = new cUsuariosBL().GetByConstraint(id);;
                limpiaCampos();
                habilitaCampos(false);
                txtNombre.Text = usuarios.Nombre;
                txtApellidoPaterno.Text = usuarios.ApellidoPaterno;
                txtApellidoMaterno.Text = usuarios.ApellidoMaterno;
                txtUsuario.Text = usuarios.Usuario;
                txtContrasena.Text = usuarios.Contrasenia;
                txtDireccion.Text = usuarios.Direccion;
                txtArea.Text = usuarios.Area;
                txtNoEmpleado.Text = usuarios.NoEmpleado.ToString();
                
                btnGuardar.Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica Usuario";
                int id = Convert.ToInt32(e.CommandArgument);
                cUsuarios usuarios = new cUsuariosBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(true);
                btnGuardar.Visible = true;
                txtNombre.Text = usuarios.Nombre;
                txtApellidoPaterno.Text = usuarios.ApellidoPaterno;
                txtApellidoMaterno.Text = usuarios.ApellidoMaterno;
                txtUsuario.Text = usuarios.Usuario;               
                txtDireccion.Text = usuarios.Direccion;
                txtArea.Text = usuarios.Area;
                txtNoEmpleado.Text = usuarios.NoEmpleado.ToString();
                chkContrasenia.Checked = false;
                txtContrasena.Enabled = false;
                rfvContrasenia.Enabled = false;

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
                    if (e.Row.Cells[4].Text == "ADMIN")
                    {
                        ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                        imgDelete.Visible = false;
                    }
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
            lbl_titulo.Text = "Alta Usuario";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
            btnGuardar.Visible = true;
            txtNombre.Focus();
            chkContrasenia.Checked = true;           
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];            
            pnl_Modal.Hide();
            vtnModal.DysplayCancelar = false;
            if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
            {//Alta
                if (new cUsuariosBL().GetByNoEmpleadoUsuario(Convert.ToInt32(txtNoEmpleado.Text), txtUsuario.Text) == null)
                {
                    cUsuarios usuarios = new cUsuarios();
                    usuarios.Nombre = txtNombre.Text;
                    usuarios.ApellidoPaterno = txtApellidoPaterno.Text;
                    usuarios.ApellidoMaterno = txtApellidoMaterno.Text;
                    usuarios.Usuario = txtUsuario.Text;
                    usuarios.Contrasenia = new Utileria().GetSHA1(txtContrasena.Text);
                    usuarios.Direccion = txtDireccion.Text;
                    usuarios.Area = txtArea.Text;
                    usuarios.NoEmpleado = Convert.ToInt32(txtNoEmpleado.Text); //no null
                    usuarios.Activo = true;
                    usuarios.IdUsuario = U.Id;
                    usuarios.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cUsuariosBL().Insert(usuarios);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.UsuarioExiste), ModalPopupMensaje.TypeMesssage.Alert);
                }               
            }
            else //Modificacion
            {
                cUsuarios usuarios = new cUsuariosBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                if (usuarios.NoEmpleado == Convert.ToInt32(txtNoEmpleado.Text) && usuarios.Usuario==txtUsuario.Text)
                {//modifica si es el mismo usuario y empleado
                    usuarios.Nombre = txtNombre.Text;
                    usuarios.ApellidoPaterno = txtApellidoPaterno.Text;
                    usuarios.ApellidoMaterno = txtApellidoMaterno.Text;
                    if (chkContrasenia.Checked)
                    {
                        usuarios.Contrasenia = new Utileria().GetSHA1(txtContrasena.Text);
                    }
                    usuarios.Direccion = txtDireccion.Text;
                    usuarios.Area = txtArea.Text;                   
                    usuarios.IdUsuario = U.Id;
                    usuarios.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cUsuariosBL().Update(usuarios);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }
                else if (usuarios.NoEmpleado == Convert.ToInt32(txtNoEmpleado.Text) && new cUsuariosBL().GetByUsuario(txtUsuario.Text)==null)
                { //modifica si es el mismo empleado y el usuario esta libre
                    usuarios.Nombre = txtNombre.Text;
                    usuarios.ApellidoPaterno = txtApellidoPaterno.Text;
                    usuarios.ApellidoMaterno = txtApellidoMaterno.Text;
                    usuarios.Usuario = txtUsuario.Text;
                    if (chkContrasenia.Checked)
                    {
                        usuarios.Contrasenia = new Utileria().GetSHA1(txtContrasena.Text);
                    }
                    usuarios.Direccion = txtDireccion.Text;
                    usuarios.Area = txtArea.Text;
                    usuarios.NoEmpleado = Convert.ToInt32(txtNoEmpleado.Text); //no null
                    usuarios.IdUsuario = U.Id;
                    usuarios.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cUsuariosBL().Update(usuarios);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }
                else if (usuarios.Usuario == txtUsuario.Text && new cUsuariosBL().GetByNoEmpleadoTodos(Convert.ToInt32(txtNoEmpleado.Text)) == null)
                {//modifica si el usuario es el mismo y el empleado esta libre en usuarios
                    usuarios.Nombre = txtNombre.Text;
                    usuarios.ApellidoPaterno = txtApellidoPaterno.Text;
                    usuarios.ApellidoMaterno = txtApellidoMaterno.Text;
                    usuarios.Usuario = txtUsuario.Text;
                    if (chkContrasenia.Checked)
                    {
                        usuarios.Contrasenia = new Utileria().GetSHA1(txtContrasena.Text);
                    }
                    usuarios.Direccion = txtDireccion.Text;
                    usuarios.Area = txtArea.Text;
                    usuarios.NoEmpleado = Convert.ToInt32(txtNoEmpleado.Text); //no null
                    usuarios.IdUsuario = U.Id;
                    usuarios.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cUsuariosBL().Update(usuarios);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.UsuarioExiste), ModalPopupMensaje.TypeMesssage.Alert);                    
                }
              
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
            txtNombre.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtUsuario.Text = "";
            txtContrasena.Text = "";
            txtDireccion.Text = "";
            txtArea.Text = "";
            txtNoEmpleado.Text = "";
            ViewState["idMod"] = null; 
            
        }
        private void habilitaCampos(bool activa)
        {
            txtNombre.Enabled = activa;
            txtApellidoPaterno.Enabled = activa;
            txtApellidoMaterno.Enabled = activa;
            txtUsuario.Enabled = activa;
            txtContrasena.Enabled = activa;
            txtDireccion.Enabled = activa;
            txtArea.Enabled = activa;
            txtNoEmpleado.Enabled = activa;
            chkContrasenia.Enabled = activa;
            
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cUsuarios usuarios = new cUsuariosBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                usuarios.Activo = false;
                usuarios.IdUsuario = U.Id;
                usuarios.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cUsuariosBL().Delete(usuarios);

                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cUsuarios usuarios = new cUsuariosBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                usuarios.Activo = true;
                usuarios.IdUsuario = U.Id;
                usuarios.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cUsuariosBL().Update(usuarios);
                
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

        protected void chkContrasenia_CheckedChanged(object sender, EventArgs e)
        {
            if (chkContrasenia.Checked)
            {
                txtContrasena.Enabled = true;
                rfvContrasenia.Enabled = true;
            }
            else
            {
                txtContrasena.Enabled = false;
                rfvContrasenia.Enabled = false;
            }
            pnl_Modal.Show();
        }        
    }
}