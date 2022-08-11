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
using System.Transactions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Catastro.Catalogos
{
    public partial class catRFC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //llenaConfiguracion();
                ViewState["sortCampo"] = "RFC";
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
            ddlFiltro.DataSource = new cRfcBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }

        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cRfcBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cRfcBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta RFC";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cRFC rfc = new cRfcBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(false);
                llenaConfiguracion(rfc);
                btnGuardar.Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica RFC";
                int id = Convert.ToInt32(e.CommandArgument);
                cRFC rfc = new cRfcBL().GetByConstraint(id);
                limpiaCampos();
                habilitaCampos(true);
                btnGuardar.Visible = true;
                llenaConfiguracion(rfc);
                ViewState["idMod"] = id;
                txtRFC.Enabled = false;                
                pnl_Modal.Show();
            }
            else if (e.CommandName == "EliminarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
            }
            else if (e.CommandName == "ActivarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
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
                int idRFC = int.Parse(e.Row.Cells[0].Text);
                int id = int.Parse(e.Row.Cells[0].Text);
                string rfc = e.Row.Cells[1].Text;
                string idporedio = e.Row.Cells[2].Text;
                string nombre = e.Row.Cells[3].Text;
                

                cRFC cRfc = new cRfcBL().GetByConstraint(id);


                e.Row.Cells[2].Text = new cPredioBL().GetPredioByIdContribuyente(int.Parse(cRfc.IdContribuyente.ToString())).ClavePredial;
                //e.Row.Cells[1].Text = new cContribuyenteBL().GetByConstraint(idcon).ClavePredial;

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

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            lbl_titulo.Text = "Alta de RFC";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
            btnGuardar.Visible = true;
        }
        protected void limpiaCampos()
        {
            hdfId.Value = "";
            txtRFC.Text = "";
            txtNombre.Text = "";
            txtCalle.Text = "";
            txtMunicipio.Text = "";
            txtEstado.Text = "";
            txtPais.Text = "";
            txtCP.Text = "";
            txtNumero.Text = "";
            txtNumeroInt.Text = "";
            txtColonia.Text = "";
            txtLocalidad.Text = "";
            txtReferecnia.Text = "";
            txtEmail.Text = "";
            txtClave.Text = "";
        }
      
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string respuesta = new Utileria().validaRFC(txtRFC.Text.ToUpper().Trim());
            if (respuesta == "CORRECTO")
            {
                lblValidaRFC.Text = respuesta;
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                cPredio p = new cPredioBL().GetByClavePredial(txtClave.Text);
                pnl_Modal.Hide();
                vtnModal.DysplayCancelar = false;
                if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
                {
                    #region nuevo
                    cRFC rfc = new cRfcBL().GetByRfc(txtRFC.Text.Trim().ToUpper());
                   
                    if (rfc == null)
                    {
                        rfc = new cRFC();
                        rfc.RFC = txtRFC.Text.ToUpper().Trim();
                        rfc.Nombre = txtNombre.Text.ToUpper().Trim();
                        rfc.Calle = txtCalle.Text.ToUpper().Trim();
                        rfc.Municipio = txtMunicipio.Text.ToUpper().Trim();
                        rfc.Estado = txtEstado.Text.ToUpper().Trim();
                        rfc.Pais = txtPais.Text.ToUpper().Trim();
                        rfc.CodigoPostal = txtCP.Text.ToUpper().Trim();
                        rfc.NoExterior = txtNumero.Text.ToUpper().Trim();
                        rfc.NoInterior = txtNumeroInt.Text.ToUpper().Trim();
                        rfc.Colonia = txtColonia.Text.ToUpper().Trim();
                        rfc.Localidad = txtLocalidad.Text.ToUpper().Trim();
                        rfc.Referencia = txtReferecnia.Text.ToUpper().Trim();
                        rfc.Email = txtEmail.Text.Trim().ToLower();
                        rfc.IdContribuyente = p.IdContribuyente;
                        rfc.RegimenFiscal = txtRegimen.Text;
                        rfc.UsoCFDI = txtUsoCFDI.Text;
                        rfc.IdUsuario = U.Id;
                        rfc.Activo = true;
                        rfc.FechaModificacion = DateTime.Now;
                        MensajesInterfaz msg = new cRfcBL().Insert(rfc);
                        vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                    else
                    {
                        rfc.IdContribuyente = p.IdContribuyente;
                        rfc.RFC = txtRFC.Text.ToUpper().Trim();
                        rfc.Nombre = txtNombre.Text.ToUpper().Trim();
                        rfc.Calle = txtCalle.Text.ToUpper().Trim();
                        rfc.Municipio = txtMunicipio.Text.ToUpper().Trim();
                        rfc.Estado = txtEstado.Text.ToUpper().Trim();
                        rfc.Pais = txtPais.Text.ToUpper().Trim();
                        rfc.CodigoPostal = txtCP.Text.ToUpper().Trim();
                        rfc.NoExterior = txtNumero.Text.ToUpper().Trim();
                        rfc.NoInterior = txtNumeroInt.Text.ToUpper().Trim();
                        rfc.Colonia = txtColonia.Text.ToUpper().Trim();
                        rfc.Localidad = txtLocalidad.Text.ToUpper().Trim();
                        rfc.Referencia = txtReferecnia.Text.ToUpper().Trim();
                        rfc.Email = txtEmail.Text.Trim().ToLower();
                        rfc.RegimenFiscal = txtRegimen.Text;
                        rfc.UsoCFDI = txtUsoCFDI.Text;
                        rfc.IdUsuario = U.Id;
                        rfc.Activo = true;
                        rfc.FechaModificacion = DateTime.Now;
                        MensajesInterfaz msg = new cRfcBL().Update(rfc);
                        vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                    #endregion
                }
                else
                {
                    cRFC rfc = new cRfcBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                    rfc.RFC = txtRFC.Text.ToUpper().Trim();
                    rfc.Nombre = txtNombre.Text.ToUpper().Trim();
                    rfc.Calle = txtCalle.Text.ToUpper().Trim();
                    rfc.Municipio = txtMunicipio.Text.ToUpper().Trim();
                    rfc.Estado = txtEstado.Text.ToUpper().Trim();
                    rfc.Pais = txtPais.Text.ToUpper().Trim();
                    rfc.CodigoPostal = txtCP.Text.ToUpper().Trim();
                    rfc.NoExterior = txtNumero.Text.ToUpper().Trim();
                    rfc.NoInterior = txtNumeroInt.Text.ToUpper().Trim();
                    rfc.Colonia = txtColonia.Text.ToUpper().Trim();
                    rfc.Localidad = txtLocalidad.Text.ToUpper().Trim();
                    rfc.Referencia = txtReferecnia.Text.ToUpper().Trim();
                    rfc.Email = txtEmail.Text.Trim().ToLower();
                    rfc.RegimenFiscal = txtRegimen.Text;
                    rfc.IdContribuyente = p.IdContribuyente;
                    rfc.UsoCFDI = txtUsoCFDI.Text;
                    rfc.IdUsuario = U.Id;
                    rfc.Activo = true;
                    rfc.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cRfcBL().Update(rfc);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }

                limpiaCampos();
                llenagrid();
            }
            else
            {
                lblValidaRFC.Text = respuesta;
                pnl_Modal.Show();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ViewState["idMod"] = 0;
            limpiaCampos();
            pnl_Modal.Hide();
        }

        private void llenaConfiguracion(cRFC rfc)
        {
            if (rfc != null)
            {
                hdfId.Value = rfc.Id.ToString();
                
                //string clavePredial = new cPredioBL().GetPredioByIdContribuyente(int.Parse(rfc.IdContribuyente.ToString())).ClavePredial;
                ViewState["idPredio"] = rfc.IdContribuyente;
                cPredio p = new cPredioBL().GetPredioByIdContribuyente(int.Parse(rfc.IdContribuyente.ToString()));

                txtRFC.Text = rfc.RFC;
                txtClave.Text = p.ClavePredial;
                txtNombre.Text = rfc.Nombre;
                txtCalle.Text = rfc.Calle;
                txtMunicipio.Text = rfc.Municipio;
                txtEstado.Text = rfc.Estado;
                txtPais.Text = rfc.Pais;
                txtCP.Text = rfc.CodigoPostal;
                txtNumero.Text = rfc.NoExterior;
                txtNumeroInt.Text = rfc.NoInterior;
                txtColonia.Text = rfc.Colonia;
                txtLocalidad.Text = rfc.Localidad;
                txtReferecnia.Text = rfc.Referencia;
                txtEmail.Text = rfc.Email == "" || rfc.Email == null ? "" : rfc.Email.ToLower();
                txtRegimen.Text = rfc.RegimenFiscal;
                txtUsoCFDI.Text = rfc.UsoCFDI;
                txtClave.Text = p.ClavePredial;
                
            }
        }
        private void habilitaCampos(bool activa)
        {
            txtRFC.Enabled = activa;
            txtNombre.Enabled = activa;
            //txtCalle.Enabled = activa;
            //txtMunicipio.Enabled = activa;
            //txtEstado.Enabled = activa;
            //txtPais.Enabled = activa;
            //txtCP.Enabled = activa;
            //txtNumero.Enabled = activa;
            //txtNumeroInt.Enabled = activa;
            //txtColonia.Enabled = activa;
            //txtLocalidad.Enabled = activa;
            //txtReferecnia.Enabled = activa;
            txtEmail.Enabled = activa;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cRFC rfc = new cRfcBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                rfc.Activo = false;
                rfc.IdUsuario = U.Id;
                rfc.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cRfcBL().Delete(rfc);
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cRFC rfc = new cRfcBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                rfc.Activo = true;
                rfc.IdUsuario = U.Id;
                rfc.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cRfcBL().Update(rfc);
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