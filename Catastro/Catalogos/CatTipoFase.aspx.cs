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
    public partial class CatTipoFase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Fase";
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
            ddlFiltro.DataSource = new cTipoFaseBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }
        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cTipoFaseBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cTipoFaseBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta Tipo de Fase";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cTipoFase Fase = new cTipoFaseBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(false);
                txtFase.Text = Fase.Fase.ToString();
                txtDescripcion.Text = Fase.Descripcion.ToString();
                txtFasesig.Text = Fase.FaseSiguiente.ToString();
                txtPorcentaje.Text = Fase.PorcentajeEjecucion.ToString();
                txtSmmultas.Text = Fase.SalarioMinimoMultas.ToString();
                txtDcto.Text = Fase.DoctoImprimir.ToString();
                ddlHon.SelectedValue = Fase.Cobranza.ToString();
                btnGuardar.Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica Tipo de Fase";
                int id = Convert.ToInt32(e.CommandArgument);
                cTipoFase Fase = new cTipoFaseBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(true);
                btnGuardar.Visible = true;
                txtFase.Text = Fase.Fase.ToString();
                txtDescripcion.Text = Fase.Descripcion.ToString();
                txtFasesig.Text = Fase.FaseSiguiente.ToString();
                txtPorcentaje.Text = Fase.PorcentajeEjecucion.ToString();
                txtSmmultas.Text = Fase.SalarioMinimoMultas.ToString();
                txtDcto.Text = Fase.DoctoImprimir.ToString();
                ddlHon.SelectedValue = Fase.Cobranza.ToString();
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
            lbl_titulo.Text = "Alta Tipo de Fase";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
            btnGuardar.Visible = true;
            txtFase.Focus();
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            pnl_Modal.Hide();
            vtnModal.DysplayCancelar = false;
            if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
            {
                cTipoFase Fase = new cTipoFase();
                Fase.Fase = Convert.ToInt32(txtFase.Text);
                Fase.Descripcion = txtDescripcion.Text;
                Fase.FaseSiguiente = Convert.ToInt32(txtFasesig.Text);
                Fase.PorcentajeEjecucion = Convert.ToDecimal(txtPorcentaje.Text);
                Fase.SalarioMinimoMultas = Convert.ToInt32(txtSmmultas.Text);
                Fase.DoctoImprimir = txtDcto.Text;
                Fase.IdUsuario = U.Id;
                Fase.Activo = true;
                Fase.FechaModificacion = DateTime.Now;
                Fase.Cobranza = Convert.ToInt32(ddlHon.SelectedValue);

                MensajesInterfaz msg = new cTipoFaseBL().Insert(Fase);
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                cTipoFase Fase = new cTipoFaseBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Fase.Fase = Convert.ToInt32(txtFase.Text);
                Fase.Descripcion = txtDescripcion.Text;
                Fase.FaseSiguiente = Convert.ToInt32(txtFasesig.Text);
                Fase.PorcentajeEjecucion = Convert.ToDecimal(txtPorcentaje.Text);
                Fase.SalarioMinimoMultas = Convert.ToInt32(txtSmmultas.Text);
                Fase.DoctoImprimir = txtDcto.Text;
                Fase.IdUsuario = U.Id;
                Fase.FechaModificacion = DateTime.Now;
                Fase.Cobranza = Convert.ToInt32(ddlHon.SelectedValue);

                MensajesInterfaz msg = new cTipoFaseBL().Update(Fase);
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
            txtFase.Text = "";
            txtDescripcion.Text = "";
            txtFasesig.Text = "";
            txtPorcentaje.Text = "";
            txtSmmultas.Text = "";
            txtDcto.Text = "";
            ddlHon.SelectedValue= "1";

            ViewState["idMod"] = null;
        }
        private void habilitaCampos(bool activa)
        {
            txtFase.Enabled = activa;
            txtDescripcion.Enabled = activa;
            txtFasesig.Enabled = activa;
            txtPorcentaje.Enabled = activa;
            txtSmmultas.Enabled = activa;
            txtDcto.Enabled = activa;
      }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cTipoFase Fase = new cTipoFaseBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Fase.Activo = false;
                Fase.IdUsuario = U.Id;
                Fase.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cTipoFaseBL().Delete(Fase);
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cTipoFase Fase = new cTipoFaseBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Fase.Activo = true;
                Fase.IdUsuario = U.Id;
                Fase.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cTipoFaseBL().Update(Fase);
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