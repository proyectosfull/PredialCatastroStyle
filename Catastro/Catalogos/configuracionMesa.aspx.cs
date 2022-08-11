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
    public partial class configuracionMesa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Id";
                ViewState["sortOrden"] = "asc";
                chkInactivo.Checked = true;
                llenagrid();
                llenaFiltro();
                llenarNoCaja();
                llenaListas();
                txtFiltro.Enabled = false;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenaFiltro()
        {
            ddlFiltro.DataSource = new tConfiguracionMesaBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }

        private void llenaListas()
        {
            ddlMesa.DataValueField = "Id";
            ddlMesa.DataTextField = "Nombre";
            ddlMesa.DataSource = new cMesaBL().GetAll();
            ddlMesa.DataBind();
            ddlMesa.Items.Insert(0, new ListItem("Seleccionar Mesa", "%"));
        }
        private void llenarNoCaja()
        {
            ddlNumeroCaja.DataSource = new cCajaBL().GetAll();
            ddlNumeroCaja.DataValueField = "Id";
            ddlNumeroCaja.DataTextField = "Caja";
            ddlNumeroCaja.DataBind();
            ddlNumeroCaja.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Caja", "%"));
        }
        private void llenarEmpleados()
        {

            var listaEmpleados = new cUsuariosBL().GetAll().Select(v => new
            {
                Id = v.Id,
                NombreCompleto = v.Nombre + " " + v.ApellidoPaterno
                    + " " + v.ApellidoMaterno
            });
            ddlCajero.Items.Clear();
            ddlCajero.DataSource = listaEmpleados;
            ddlCajero.DataValueField = "Id";
            ddlCajero.DataTextField = "NombreCompleto";
            ddlCajero.DataBind();
            ddlCajero.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Cajero", "%"));


        }

        #region Grid
        private void llenagrid()
        {
          string[] filtro =  (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new tConfiguracionMesaBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new tConfiguracionMesaBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
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
                lblTitulo.Text = "Consulta";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                tConfiguracionMesa confmesa = new tConfiguracionMesaBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(false);
                ddlCajero.SelectedValue = confmesa.IdCajero.ToString();
                ddlMesa.SelectedValue = confmesa.IdMesa.ToString();
                ddlNumeroCaja.SelectedValue = confmesa.IdCaja.ToString();
                txtTurno.Text = confmesa.Turno;
                txtLugar.Text = confmesa.Lugar;
                btnGuardar.Visible = false;
            }
          else if (e.CommandName == "ModificarRegistro")
            {
                lblTitulo.Text = "Modifica";
                int id = Convert.ToInt32(e.CommandArgument);
                tConfiguracionMesa confmesa = new tConfiguracionMesaBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(true);
                btnGuardar.Visible = true;
                ddlCajero.SelectedValue = confmesa.IdCajero.ToString();
                ddlMesa.SelectedValue = confmesa.IdMesa.ToString();
                ddlNumeroCaja.SelectedValue = confmesa.IdCaja.ToString();
                txtTurno.Text = confmesa.Turno;
                txtLugar.Text = confmesa.Lugar;
                ViewState["idMod"] = id;
                btnGuardar.Visible = true;
                pnl_Modal.Show();
            }
            else if (e.CommandName == "EliminarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.CerrarCaja), ModalPopupMensaje.TypeMesssage.Confirm);
            }
            //else if (e.CommandName == "ActivarRegistro")
            //{
            //    int id = Convert.ToInt32(e.CommandArgument);
            //    ViewState["idMod"] = id;
            //    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro), ModalPopupMensaje.TypeMesssage.Confirm);
            //}

        }

        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortCampo"] == null)
            {
                ViewState["sortCampo"] = e.SortExpression.ToString();
                ViewState["sortOrden"] = "asc";
            }
            else
            {
                if (e.SortExpression.ToString() == ViewState["sortCampo"].ToString())
                {
                    if (ViewState["sortOrden"].ToString() == "asc")
                        ViewState["sortOrden"] = "desc";
                    else
                        ViewState["sortOrden"] = "asc";
                }
                else
                {
                    ViewState["sortCampo"] = e.SortExpression.ToString();
                    ViewState["sortOrden"] = "asc";
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
                    String idMesa = e.Row.Cells[0].Text;
                    if (idMesa == "&nbsp;") { idMesa = ""; }
                    e.Row.Cells[0].Text = new cMesaBL().GetByConstraint(int.Parse(idMesa)).Nombre;
                    String idEmpleado = e.Row.Cells[1].Text;
                    if (idEmpleado == "&nbsp;") { idEmpleado = ""; }
                    cUsuarios usuario = new cUsuariosBL().GetByConstraint(int.Parse(idEmpleado));
                    e.Row.Cells[1].Text = usuario.Nombre + " " + usuario.ApellidoPaterno + " " + usuario.ApellidoMaterno;
                    String idCaja = e.Row.Cells[2].Text;
                    if (idCaja == "&nbsp;") { idCaja = ""; }
                    e.Row.Cells[2].Text = new cCajaBL().GetByConstraint(int.Parse(idCaja)).Caja;
                
                    //ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    //imgActivar.Visible = false;
                }
                else
                {
                    String idMesa = e.Row.Cells[0].Text;
                    if (idMesa == "&nbsp;") { idMesa = ""; }
                    e.Row.Cells[0].Text = new cMesaBL().GetByConstraint(int.Parse(idMesa)).Nombre;
                    String idEmpleado = e.Row.Cells[1].Text;
                    if (idEmpleado == "&nbsp;") { idEmpleado = ""; }
                    cUsuarios usuario = new cUsuariosBL().GetByConstraint(int.Parse(idEmpleado));
                    e.Row.Cells[1].Text = usuario.Nombre + " " + usuario.ApellidoPaterno + " " + usuario.ApellidoMaterno;
                    String idCaja = e.Row.Cells[2].Text;
                    if (idCaja == "&nbsp;") { idCaja = ""; }
                    e.Row.Cells[2].Text = new cCajaBL().GetByConstraint(int.Parse(idCaja)).Caja;
                
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
            lblTitulo.Text = "Alta ";
            pnl_Modal.Show();
            ddlMesa.Enabled = true;
            limpiaCampos();
            habilitaCampos(true);
            btnGuardar.Visible = true;
            mostrarCampos(false);
            //txtNombre.Focus();
        }

        //protected Boolean isMod()
        //{
        //    tConfiguracionMesa cajeroCaja = new tConfiguracionMesaBL().GetByCajeroCaja(int.Parse(ddlCajero.SelectedValue), int.Parse(ddlNumeroCaja.SelectedValue));

        //    Boolean valor = true; ;

        //    if (cajeroCaja != null)
        //    {
        //        if (cajeroCaja.Turno == txtTurno.Text || cajeroCaja.Lugar == txtLugar.Text)
        //        {
        //            valor= false;
        //        }
        //        else
        //        {
        //            valor = true;
        //        }
        //    }

        //    return valor;
        //}
        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            pnl_Modal.Hide();
            vtnModal.DysplayCancelar = false;

            //tConfiguracionMesa caja = new tConfiguracionMesaBL().GetByIdCaja(int.Parse(ddlNumeroCaja.SelectedValue));
            //if (caja == null)
            //{

                //if (isMod())
                //{

                    if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
                    {
                        tConfiguracionMesa confmesa = new tConfiguracionMesa();
                        confmesa.IdMesa = int.Parse(ddlMesa.SelectedValue);
                        confmesa.IdCaja = int.Parse(ddlNumeroCaja.SelectedValue);
                        confmesa.IdCajero = int.Parse(ddlCajero.SelectedValue);
                        confmesa.Turno = txtTurno.Text;
                        confmesa.Lugar = txtLugar.Text;
                        confmesa.IdUsuario1 = U.IdUsuario;
                        confmesa.Activo = true;
                        confmesa.FechaModificacion = DateTime.Now;
                        MensajesInterfaz msg = new tConfiguracionMesaBL().Insert(confmesa);
                        vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                    else
                    {
                        tConfiguracionMesa confmesa = new tConfiguracionMesaBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                        confmesa.IdMesa = int.Parse(ddlMesa.SelectedValue);
                        confmesa.IdCaja = int.Parse(ddlNumeroCaja.SelectedValue);
                        confmesa.Turno = txtTurno.Text;
                        confmesa.Lugar = txtLugar.Text;
                        confmesa.IdCajero = int.Parse(ddlCajero.SelectedValue);
                        confmesa.IdUsuario1 = U.IdUsuario;
                        confmesa.FechaModificacion = DateTime.Now;
                        MensajesInterfaz msg = new tConfiguracionMesaBL().Update(confmesa);
                        vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                    }

                    limpiaCampos();
                    llenagrid();

                //}
                //else
                //{
                //    vtnModal.DysplayCancelar = false;
                //    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.CajeroCajaAbierta), ModalPopupMensaje.TypeMesssage.Confirm);
                //}
            //}
            //else
            //{
            //    vtnModal.DysplayCancelar = false;
            //    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.CajeroCajaAbierta), ModalPopupMensaje.TypeMesssage.Confirm);
            //}

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ViewState["idMod"] = 0;
            limpiaCampos();
            pnl_Modal.Hide();
        }

        private void mostrarCampos(Boolean visible)
        {
            txtLugar.Visible = visible;
            lblLugar.Visible = visible;
            txtTurno.Visible = visible;
            lblTurno.Visible = visible;
            //ddlMesa.Visible = visible;
            //lblMesa.Visible = visible;
            ddlNumeroCaja.Visible = visible;
            lblNoCaja.Visible = visible;
            btnGuardar.Visible = visible;
        }

        private void limpiaCampos()
        {
            
            txtLugar.Text = "";
            txtTurno.Text = "";
            //txtMaquina.Text = "";
            llenarNoCaja();
            llenaListas();
            llenarEmpleados();
            mostrarCampos(false);
        }
        private void habilitaCampos(bool activa)
        {
            ddlCajero.Enabled = activa;
           // ddlMesa.Enabled = activa;
            ddlNumeroCaja.Enabled = activa;
            txtLugar.Enabled = activa;
            txtTurno.Enabled = activa;
            ddlCajero.Enabled = activa;
            mostrarCampos(true);
            //txtMaquina.Enabled = activa;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.CerrarCaja))
            {
                tConfiguracionMesa confmesa = new tConfiguracionMesaBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                confmesa.Activo = false;
                confmesa.IdUsuario1 = U.IdUsuario;
                confmesa.FechaModificacion = DateTime.Now;
                confmesa.FechaCierre = DateTime.Now;
                MensajesInterfaz resul = new tConfiguracionMesaBL().Delete(confmesa);
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.CajaCerrada), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                tConfiguracionMesa confmesa = new tConfiguracionMesaBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                confmesa.Activo = true;
                confmesa.IdUsuario1 = U.Id;
                confmesa.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new tConfiguracionMesaBL().Update(confmesa);                
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
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.NoEmpleadoVacio) ||
                     vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.CajaCerrada) )
            {
                pnl_Modal.Show();
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

        protected void idBuscarAgente_Click(object sender, ImageClickEventArgs e)
        {
            String noEmpleado = ddlCajero.SelectedValue;
            if (noEmpleado != "%")
            {
                cUsuarios usuario = new cUsuariosBL().GetByConstraint(int.Parse(noEmpleado));
                mostrarCampos(true);
                tConfiguracionMesa confMesa = new tConfiguracionMesaBL().GetByCajeroMesa(usuario.Id,Convert.ToInt32(ddlMesa.SelectedValue));
                
                if (confMesa != null)
                {
                    ddlNumeroCaja.SelectedValue = confMesa.IdCaja.ToString();
                    ddlMesa.SelectedValue = confMesa.IdMesa.ToString();
                    ddlMesa.Enabled = false;
                    txtLugar.Text = confMesa.Lugar;
                    txtTurno.Text = confMesa.Turno;
                    ddlCajero.Enabled=false;
                    habilitaCampos(false);
                }
                pnl_Modal.Show();
            }
            else
            {
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.NoEmpleadoVacio), ModalPopupMensaje.TypeMesssage.Confirm);
                pnl_Modal.Show();
            }
        }

   

    
    }
}