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
    public partial class CatTipoAvaluo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Descripcion";
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
            ddlFiltro.DataSource = new cTipoAvaluoBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }
        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cTipoAvaluoBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cTipoAvaluoBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta Tipo de Avalúo";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cTipoAvaluo Avaluo = new cTipoAvaluoBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(false);
                txtDescripcion.Text = Avaluo.Descripcion.ToString();
                
                btnGuardar.Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica Tipo de Avalúo";
                int id = Convert.ToInt32(e.CommandArgument);
                cTipoAvaluo Avaluo = new cTipoAvaluoBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(true);
                btnGuardar.Visible = true;
                txtDescripcion.Text = Avaluo.Descripcion.ToString();
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
            lbl_titulo.Text = "Alta Tipo de Avalúo";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
            btnGuardar.Visible = true;
            txtDescripcion.Focus();
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
          
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                pnl_Modal.Hide();
                vtnModal.DysplayCancelar = false;
                if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
                {
                    cTipoAvaluo Avaluo = new cTipoAvaluo();
                    Avaluo.Descripcion = txtDescripcion.Text;
                    Avaluo.IdUsuario = U.Id;
                    Avaluo.Activo = true;
                    Avaluo.FechaModificacion = DateTime.Now;

                    MensajesInterfaz msg = new cTipoAvaluoBL().Insert(Avaluo);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }
                else
                {
                    cTipoAvaluo Avaluo = new cTipoAvaluoBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                    Avaluo.Descripcion = txtDescripcion.Text;
                    Avaluo.IdUsuario = U.Id;
                    Avaluo.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cTipoAvaluoBL().Update(Avaluo);
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
            
            ViewState["idMod"] = null;
        }
        private void habilitaCampos(bool activa)
        {
            txtDescripcion.Enabled = activa;
            
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cTipoAvaluo Avaluo = new cTipoAvaluoBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Avaluo.Activo = false;
                Avaluo.IdUsuario = U.Id;
                Avaluo.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cTipoAvaluoBL().Delete(Avaluo);
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cTipoAvaluo Avaluo = new cTipoAvaluoBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Avaluo.Activo = true;
                Avaluo.IdUsuario = U.Id;
                Avaluo.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cTipoAvaluoBL().Update(Avaluo);
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