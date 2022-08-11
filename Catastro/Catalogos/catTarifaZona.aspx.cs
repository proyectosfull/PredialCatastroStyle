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
    public partial class catTarifaZona : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Ejercicio";
                ViewState["sortOnden"] = "desc";
                llenaFiltro();
                txtFiltro.Enabled = false;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenaFiltro()
        {
            ddlFiltro.DataSource = new cTarifaZonaBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }

        private void llenarTipoCobro()
        {

            ddlTipoCobro.Items.Clear();
            ddlTipoCobro.DataValueField = "Id";
            ddlTipoCobro.DataTextField = "Descripcion";
            ddlTipoCobro.DataSource = new cTipoCobroBL().GetAll();
            ddlTipoCobro.DataBind();
            ddlTipoCobro.Items.Insert(0, new ListItem("Seleccionar Tipo Cobro", "%"));

        }    

        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cTarifaZonaBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cTarifaZonaBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta Tarifa Zona";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cTarifaZona TarifaZona = new cTarifaZonaBL().GetByConstraint(id);
                llenarTipoCobro();
                limpiaCampos();
                habilitaCampos(false);
                txtEjercicio.Text = TarifaZona.Ejercicio.ToString();
                txtZona.Text = TarifaZona.Zona.ToString();
                txtTarifa.Text = TarifaZona.Tarifa.ToString();
                cTipoCobro tipoCobro = new cTipoCobroBL().GetByConstraint(TarifaZona.IdTipoCobro);
                if(tipoCobro != null)
                    if (tipoCobro.Activo)
                    {
                        ddlTipoCobro.SelectedValue = TarifaZona.IdTipoCobro.ToString();
                    }
                    else {
                        ddlTipoCobro.Items.Add(new ListItem(tipoCobro.Descripcion, tipoCobro.Codigo));
                        ddlTipoCobro.SelectedValue = TarifaZona.IdTipoCobro.ToString();
                    }
                btnGuardar.Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {                
                lbl_titulo.Text = "Modifica Tarifa Zona";
                int id = Convert.ToInt32(e.CommandArgument);
                cTarifaZona TarifaZona = new cTarifaZonaBL().GetByConstraint(id);
                llenarTipoCobro();
                limpiaCampos();
                habilitaCampos(true);
                btnGuardar.Visible = true;
                txtEjercicio.Text = TarifaZona.Ejercicio.ToString();
                txtZona.Text = TarifaZona.Zona.ToString();
                txtTarifa.Text = TarifaZona.Tarifa.ToString();
                cTipoCobro tipoCobro = new cTipoCobroBL().GetByConstraint(TarifaZona.IdTipoCobro);
                if (tipoCobro != null)
                    if (tipoCobro.Activo)
                    {
                        ddlTipoCobro.SelectedValue = TarifaZona.cTipoCobro.Id.ToString();
                    }
                    else
                    {
                        ddlTipoCobro.Items.Add(new ListItem(tipoCobro.Descripcion, tipoCobro.Codigo));
                        ddlTipoCobro.SelectedValue = TarifaZona.cTipoCobro.Id.ToString();
                    }
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
                int idTipoCobro = int.Parse(e.Row.Cells[3].Text);
                e.Row.Cells[3].Text = new cTipoCobroBL().GetByConstraint(idTipoCobro).Descripcion;

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
            lbl_titulo.Text = "Alta Tarifa Zona";
            pnl_Modal.Show();
            limpiaCampos();
            llenarTipoCobro();
            habilitaCampos(true);
            btnGuardar.Visible = true;
            txtEjercicio.Focus();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            pnl_Modal.Hide();
            vtnModal.DysplayCancelar = false;
            if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
            {
                cTarifaZona TarifaZona = new cTarifaZona();
                TarifaZona.Ejercicio = Convert.ToInt32( txtEjercicio.Text);
                TarifaZona.Tarifa = Convert.ToDecimal(txtTarifa.Text);
                TarifaZona.Zona = Convert.ToInt32(txtZona.Text);
                TarifaZona.IdUsuario = U.Id;
                TarifaZona.Activo = true;
                TarifaZona.IdTipoCobro =Convert.ToInt32( ddlTipoCobro.SelectedValue);
                TarifaZona.FechaModificacion = DateTime.Now;
                MensajesInterfaz msg = new cTarifaZonaBL().Insert(TarifaZona);
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                cTarifaZona TarifaZona = new cTarifaZonaBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                TarifaZona.Ejercicio = Convert.ToInt32(txtEjercicio.Text);
                TarifaZona.Tarifa = Convert.ToDecimal(txtTarifa.Text);
                TarifaZona.Zona = Convert.ToInt32(txtZona.Text);
                TarifaZona.IdTipoCobro = Convert.ToInt32(ddlTipoCobro.SelectedValue);
                TarifaZona.IdUsuario = U.Id;
                TarifaZona.FechaModificacion = DateTime.Now;
                MensajesInterfaz msg = new cTarifaZonaBL().Update(TarifaZona);
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
            txtEjercicio.Text = "";
            txtTarifa.Text = "";

            txtZona.Text = "";
            ViewState["idMod"] = null;
        }
        private void habilitaCampos(bool activa)
        {
            txtEjercicio.Enabled = activa;
            txtTarifa.Enabled = activa;
            txtZona.Enabled = activa;
            ddlTipoCobro.Enabled = activa;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cTarifaZona TarifaZona = new cTarifaZonaBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                TarifaZona.Activo = false;
                TarifaZona.IdUsuario = U.Id;
                TarifaZona.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cTarifaZonaBL().Delete(TarifaZona);
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cTarifaZona TarifaZona = new cTarifaZonaBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                TarifaZona.Activo = true;
                TarifaZona.IdUsuario = U.Id;
                TarifaZona.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cTarifaZonaBL().Update(TarifaZona);
                vtnModal.DysplayCancelar = true;
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