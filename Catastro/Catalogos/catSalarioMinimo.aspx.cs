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
    public partial class catSalarioMinimo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Ejercicio";
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
            ddlFiltro.DataSource = new cSalarioMinimoBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }

        #region Grid
        private void llenagrid()
        {
          string[] filtro =  (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cSalarioMinimoBL().GetFilter("","","TRUE",ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cSalarioMinimoBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta UMA";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cSalarioMinimo SalarioMinimo = new cSalarioMinimoBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(false);             
                txtEjercicio.Text = SalarioMinimo.Ejercicio.ToString();
                txtImporte.Text = SalarioMinimo.Importe.ToString();                
                txtDescripcion.Text = SalarioMinimo.Descripcion;
                btnGuardar.Visible = false;
            }
          else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica UMA";
                int id = Convert.ToInt32(e.CommandArgument);
                cSalarioMinimo SalarioMinimo = new cSalarioMinimoBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(true);
                txtEjercicio.Enabled = false;
                btnGuardar.Visible = true;
                txtEjercicio.Text = SalarioMinimo.Ejercicio.ToString();
                txtImporte.Text = SalarioMinimo.Importe.ToString();
                txtDescripcion.Text = SalarioMinimo.Descripcion;
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
            lbl_titulo.Text = "Alta UMA";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
            btnGuardar.Visible = true;
            txtEjercicio.Focus();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Int32 ejercicio = Convert.ToInt32(txtEjercicio.Text);
            if (new cSalarioMinimoBL().GetAll().Where(s=>s.Ejercicio==ejercicio).ToList().Count == 0)
            {
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                pnl_Modal.Hide();
                vtnModal.DysplayCancelar = false;
                if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
                {
                    cSalarioMinimo SalarioMinimo = new cSalarioMinimo();
                    SalarioMinimo.Ejercicio = Convert.ToInt32(txtEjercicio.Text);
                    SalarioMinimo.Importe = Convert.ToDecimal(txtImporte.Text);
                    SalarioMinimo.Descripcion = txtDescripcion.Text;
                    SalarioMinimo.IdUsuario = U.Id;
                    SalarioMinimo.Activo = true;
                    SalarioMinimo.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cSalarioMinimoBL().Insert(SalarioMinimo);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }
                else
                {
                    cSalarioMinimo SalarioMinimo = new cSalarioMinimoBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                    SalarioMinimo.Ejercicio = Convert.ToInt32(txtEjercicio.Text);
                    SalarioMinimo.Importe = Convert.ToDecimal(txtImporte.Text);
                    SalarioMinimo.Descripcion = txtDescripcion.Text;
                    SalarioMinimo.IdUsuario = U.Id;
                    SalarioMinimo.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cSalarioMinimoBL().Update(SalarioMinimo);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.SalarioVigente), ModalPopupMensaje.TypeMesssage.Alert);
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
            txtImporte.Text="";

            txtDescripcion.Text = "";
            ViewState["idMod"] = null;
        }
        private void habilitaCampos(bool activa)
        {
            txtEjercicio.Enabled = activa;
            txtImporte.Enabled = activa;            
            txtDescripcion.Enabled = activa;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {               
                cSalarioMinimo SalarioMinimo = new cSalarioMinimoBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                SalarioMinimo.Activo = false;
                SalarioMinimo.IdUsuario = U.Id;
                SalarioMinimo.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cSalarioMinimoBL().Delete(SalarioMinimo);               
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cSalarioMinimo SalarioMinimo = new cSalarioMinimoBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                if (new cSalarioMinimoBL().GetAll().Where(s => s.Ejercicio == SalarioMinimo.Ejercicio).ToList().Count == 0)
                {                    
                    SalarioMinimo.Activo = true;
                    SalarioMinimo.IdUsuario = U.Id;
                    SalarioMinimo.FechaModificacion = DateTime.Now;
                    MensajesInterfaz resul = new cSalarioMinimoBL().Update(SalarioMinimo);
                    vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.SalarioVigente), ModalPopupMensaje.TypeMesssage.Alert);
                }
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