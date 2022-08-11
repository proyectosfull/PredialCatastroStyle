using Catastro.Controles;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Catastro.Catalogos
{
    public partial class BusquedaBaseGravable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["parametro"] = null;
                ViewState["sortCampo"] = "Ejercicio DESC, BIMESTRE";
                ViewState["sortOnden"] = "desc";
                txtFiltro.Focus();
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        protected void configuracionModal(bool v)
        {
            txtFechaEvaluo.Enabled = v;
            ddlEjercicio.Enabled = v;
            ddlBimestre.Enabled = v;
            txtSuperTerreno.Enabled = v;
            txtSuperficeConstruccion.Enabled = v;
            txtTerrenoPrivativo.Enabled = v;
            txtConstruccionPrivativa.Enabled = v;
            txtTerrenoComun.Enabled = v;
            txtConstruccionComun.Enabled = v;
            txtValorTerreno.Enabled = v;
            txtValorConstruccion.Enabled = v;
            //txtValorConstruccionComun.Enabled = v;
            //txtValorConstruccionPrivativa.Enabled = v;
            //txtPrototipo.Enabled = v;
            txtEjercicio.Enabled = v;
            cHKejercicios.Enabled = v;
        }

        #region Grid
        private void llenagrid()
        {
            int IdPredio = (int)ViewState["IdPredio"];
            grd.DataSource = BaseGrav((new vVistasBL().GetFilterVBaseGravableHist(ViewState["ClavePredial"].ToString(), ViewState["sortOnden"].ToString())));
            grd.DataBind();
            List<cBaseGravable> lbg = new cBaseGravableBL().GetListByIdPredio(IdPredio);
            ListItem i = new ListItem();
            if (lbg != null)
            {
                foreach (cBaseGravable bg in lbg)
                {
                    i = new ListItem();
                    i.Value = bg.Ejercicio.ToString();
                    i.Text = bg.Ejercicio.ToString();
                    ddlEjercicio.Items.Add(i);
                }
            }
        }

        private DataTable BaseGrav(List<vBaseGravable> uinstPer)
        {
            cUsuarios ur = (cUsuarios)Session["usuario"];
            DataTable datos = new DataTable("DTSorting");
            datos.Columns.Add("Nombre");
            datos.Columns.Add("Ejercicio");
            datos.Columns.Add("Bimestre");
            datos.Columns.Add("Valor");
            datos.Columns.Add("Id");
            datos.Columns.Add("Activo");
            datos.Columns.Add("IdUsuario");
            datos.Columns.Add("FechaModificacion");

            if (uinstPer != null)
            {
                foreach (vBaseGravable b in uinstPer)
                {
                    DataRow workRow = datos.NewRow();
                    workRow[0] = b.Nombre;
                    workRow[1] = b.Ejercicio;
                    workRow[2] = b.Bimestre;
                    workRow[3] = b.Valor;
                    workRow[4] = b.Id;
                    workRow[5] = b.Activo;
                    cUsuarios usu = new cUsuariosBL().GetByConstraint(b.IdUsuario);
                    if (usu.Id == 1)
                        workRow[6] = "Sistema Anterior";
                    else
                        workRow[6] = usu.Usuario;
                    workRow[7] = b.FechaModificacion;
                    datos.Rows.Add(workRow);
                }
                if (ViewState["sortCampo"] != null && ViewState["sortOnden"] != null)
                {
                    if (ViewState["sortCampo"].ToString() != String.Empty && ViewState["sortOnden"].ToString() != String.Empty)
                    {
                        datos.DefaultView.Sort = string.Format("{0} {1}", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                    }
                }
            }
            return datos;
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
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                cBaseGravable BaseGravable = new cBaseGravableBL().GetByConstraint(id);
                txtEjercicio.Text = BaseGravable.Ejercicio.ToString();
                llenaConfiguracion(BaseGravable);
                ddlEjercicio.Items.Clear();
                configuracionModal(false);
                btnhistorial.Visible = true;
                btnGuardar.Visible = false;
                btnCierraPanel.Visible = true;
                ddlEjercicio.Visible = false;
                txtEjercicio.Visible = true;
                cHKejercicios.Checked = false;
                pnlBG_Modal.Show();
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                //cbejercicios.Checked = false;
                ddlEjercicio.Items.Clear();
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                cBaseGravable BaseGravable = new cBaseGravableBL().GetByConstraint(id);
                int IdPredio = (int)ViewState["IdPredio"];
                List<cBaseGravable> lbg = new cBaseGravableBL().GetListByIdPredioAct(IdPredio);
                ListItem i = new ListItem();
                foreach (cBaseGravable bg in lbg)
                {
                    i = new ListItem();
                    i.Value = bg.Ejercicio.ToString();
                    i.Text = bg.Ejercicio.ToString();
                    ddlEjercicio.Items.Add(i);
                }
                ddlEjercicio.SelectedValue = BaseGravable.Ejercicio.ToString();
                llenaConfiguracion(BaseGravable);
                configuracionModal(true);
                btnhistorial.Visible = true;
                btnGuardar.Visible = true;
                btnCierraPanel.Visible = true;
                ddlEjercicio.Visible = true;
                txtEjercicio.Visible = false;
                cHKejercicios.Checked = false;
                pnlBG_Modal.Show();
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
                if (activo.ToUpper() == "FALSE")
                {
                    ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgUpdate");
                    imgUpdate.Visible = false;
                }
            }
        }

        #endregion

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(ViewState["IdPredio"].ToString()));
            txtClavePredial.Text = predio.ClavePredial;
            if (predio.ClavePredial.Substring(0, 1) == "0")
                lblCuentaPredial.Text = "Cuenta Predial: " + predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClaveAnterior.Substring(9, 3);
            lblNombrePredio.Text = predio.cContribuyente.Nombre + " " + predio.cContribuyente.ApellidoPaterno + " " + predio.cContribuyente.ApellidoMaterno;
            lblPeriodoPagado.Text ="Ultimo periodo pagado: " + predio.BimestreFinIp.ToString() + ' ' + predio.AaFinalIp.ToString();
            txtFechaEvaluo.Text = "";
            txtSuperTerreno.Text = "";
            txtSuperficeConstruccion.Text = "";
            txtTerrenoPrivativo.Text = "";
            txtConstruccionPrivativa.Text = "";
            txtTerrenoComun.Text = "";
            txtConstruccionComun.Text = "";
            txtValorTerreno.Text = "";
            txtValorConstruccion.Text = "";
            //txtValorConstruccionComun.Text = "";
            //txtValorConstruccionPrivativa.Text = "";
            //txtPrototipo.Text = "";
            txtEjercicio.Text = "";
            txtValor.Text = "";

            configuracionModal(true);
            btnhistorial.Visible = true;
            btnGuardar.Visible = true;
            btnCierraPanel.Visible = true;
            ddlEjercicio.Visible = false;
            txtEjercicio.Visible = true;
            cHKejercicios.Checked = false;
            pnlBG_Modal.Show();
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cBaseGravable BaseGravable = new cBaseGravableBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                BaseGravable.Activo = false;
                BaseGravable.IdUsuario = U.Id;
                BaseGravable.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cBaseGravableBL().Delete(BaseGravable);
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                //llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cBaseGravable BaseGravable = new cBaseGravableBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                BaseGravable.Activo = true;
                BaseGravable.IdUsuario = U.Id;
                BaseGravable.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cBaseGravableBL().Update(BaseGravable);
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                //llenagrid();
            }
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                Session["parametro"] = null;
                Response.Redirect("BusquedaBaseGravable.aspx");
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.BaseRepetida))
            {
                MensajesInterfaz msg = new cBaseGravableBL().UpdateOtrasBases(txtClavePredial.Text, int.Parse(ddlEjercicio.SelectedValue));
                //hdfidBG.Value = "0";
                guarda();
            }
        }
        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            cPredio Predio = new cPredioBL().GetByClavePredial(txtFiltro.Text);
            if (Predio == null)
            {
                ViewState["IdPredio"] = null;
                ViewState["ClavePredial"] = null;
                btnNuevo.Visible = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                btnNuevo.Visible = true;
                ViewState["IdPredio"] = Predio.Id;
                ViewState["ClavePredial"] = Predio.ClavePredial;
                llenagrid();
            }
        }

        #region modal
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            txtValorTerreno_TextChanged(null, null);
            guarda();
        }
        protected void guarda()
        {
            DateTime FechaAvaluo;
            if (DateTime.TryParse(txtFechaEvaluo.Text, out FechaAvaluo))
            {
                cUsuarios U = (cUsuarios)Session["usuario"];
                vtnModal.DysplayCancelar = false;
                MensajesInterfaz msg = new MensajesInterfaz();
                cBaseGravable BaseGravable = null;
                //if (ddlEjercicio.Visible == true)
                if (!cHKejercicios.Checked)
                {
                    BaseGravable = new cBaseGravableBL().GetByPredAnio(Convert.ToInt32(ViewState["IdPredio"]), Convert.ToInt32(ddlEjercicio.Visible == true ? ddlEjercicio.SelectedItem.Text : txtEjercicio.Text));
                    if (BaseGravable != null)
                    {
                        BaseGravable.Activo = false;
                        msg = new cBaseGravableBL().Update(BaseGravable);
                    }

                    BaseGravable = new cBaseGravable();
                    BaseGravable.IdPredio = int.Parse(ViewState["IdPredio"].ToString());
                    if (ddlEjercicio.Visible == true)
                    {
                        BaseGravable.Ejercicio = int.Parse(ddlEjercicio.SelectedItem.Text);
                    }
                    else
                    {
                        BaseGravable.Ejercicio = int.Parse(txtEjercicio.Text);
                    }
                    BaseGravable.Bimestre = int.Parse(ddlBimestre.SelectedValue);
                    BaseGravable.FechaAvaluo = Convert.ToDateTime(txtFechaEvaluo.Text);
                    BaseGravable.SuperficieTerreno = txtSuperTerreno.Text == "" ? null : (double?)Convert.ToDouble(txtSuperTerreno.Text);
                    BaseGravable.TerrenoPrivativo = txtTerrenoPrivativo.Text == "" ? null : (double?)Convert.ToDouble(txtTerrenoPrivativo.Text);
                    BaseGravable.TerrenoComun = txtTerrenoComun.Text == "" ? null : (double?)Convert.ToDouble(txtTerrenoComun.Text);
                    BaseGravable.ValorTerreno = txtValorTerreno.Text == "" ? null : (double?)Convert.ToDouble(txtValorTerreno.Text.Replace(",", "").Replace("$", ""));
                    BaseGravable.SuperficieConstruccion = Convert.ToDouble(txtSuperficeConstruccion.Text);
                    BaseGravable.ConstruccionPrivativa = txtConstruccionPrivativa.Text == "" ? null : (double?)Convert.ToDouble(txtConstruccionPrivativa.Text.Replace(",", "").Replace("$", ""));
                    BaseGravable.ConstruccionComun = txtConstruccionComun.Text == "" ? null : (double?)Convert.ToDouble(txtConstruccionComun.Text.Replace(",", "").Replace("$", ""));
                    BaseGravable.ValorConstruccion = Convert.ToDouble(txtValorConstruccion.Text.Replace(",", "").Replace("$", ""));
                    BaseGravable.Valor = Convert.ToDecimal(BaseGravable.ValorConstruccion + (BaseGravable.ValorTerreno == null ? 0 : BaseGravable.ValorTerreno));
                    //BaseGravable.ValorConstruccionPrivativa = txtValorConstruccionPrivativa.Text == "" ? null : (double?)Convert.ToDouble(txtValorConstruccionPrivativa.Text.Replace(",", "").Replace("_", ""));
                    //BaseGravable.ValorConstruccionComun = txtValorConstruccionComun.Text == "" ? null : (double?)Convert.ToDouble(txtValorConstruccionComun.Text.Replace(",", "").Replace("_", ""));
                    //BaseGravable.Prototipo = txtPrototipo.Text == "" ? null : (double?)Convert.ToDouble(txtPrototipo.Text);
                    BaseGravable.IdUsuario = U.Id;
                    BaseGravable.Activo = true;
                    BaseGravable.FechaModificacion = DateTime.Now;
                    msg = new cBaseGravableBL().Insert(BaseGravable);
                    if (BaseGravable.Ejercicio >= DateTime.Now.Year && (msg == MensajesInterfaz.Ingreso || msg == MensajesInterfaz.Actualizacion))
                    {
                        cPredio predio = new cPredioBL().GetByConstraint(BaseGravable.IdPredio);
                        predio.SuperficieConstruccion = BaseGravable.SuperficieConstruccion;
                        predio.SuperficieTerreno = BaseGravable.SuperficieTerreno;
                        predio.ValorTerreno = BaseGravable.ValorTerreno;
                        predio.ValorConstruccion = BaseGravable.ValorConstruccion;
                        predio.ValorCatastral = Convert.ToDouble(BaseGravable.Valor);
                        predio.ValorComercial = Convert.ToDouble(BaseGravable.Valor);
                        predio.ValorOperacion = Convert.ToDouble(BaseGravable.Valor);
                        predio.FechaAvaluo = FechaAvaluo;
                        msg = new cPredioBL().Update(predio);
                    }
                }
                else
                {
                    for (int i = Convert.ToInt32(ddlEjercicio.Visible == true ? ddlEjercicio.SelectedItem.Text : txtEjercicio.Text); i <= DateTime.Now.Year; i++)
                    {                       
                        BaseGravable = new cBaseGravableBL().GetByPredAnio(Convert.ToInt32(ViewState["IdPredio"]), i);
                        if (BaseGravable != null)
                        {
                            BaseGravable.Activo = false;
                            msg = new cBaseGravableBL().Update(BaseGravable);
                        }

                        BaseGravable = new cBaseGravable();
                        BaseGravable.IdPredio = int.Parse(ViewState["IdPredio"].ToString());
                        if (ddlEjercicio.Visible == true)
                        {
                            BaseGravable.Ejercicio = i;
                        }
                        else
                        {
                            BaseGravable.Ejercicio = i;
                        }
                        BaseGravable.Bimestre = int.Parse(ddlBimestre.SelectedValue);
                        BaseGravable.FechaAvaluo = Convert.ToDateTime(txtFechaEvaluo.Text);
                        BaseGravable.SuperficieTerreno = txtSuperTerreno.Text == "" ? null : (double?)Convert.ToDouble(txtSuperTerreno.Text);
                        BaseGravable.TerrenoPrivativo = txtTerrenoPrivativo.Text == "" ? null : (double?)Convert.ToDouble(txtTerrenoPrivativo.Text);
                        BaseGravable.TerrenoComun = txtTerrenoComun.Text == "" ? null : (double?)Convert.ToDouble(txtTerrenoComun.Text);
                        BaseGravable.ValorTerreno = txtValorTerreno.Text == "" ? null : (double?)Convert.ToDouble(txtValorTerreno.Text.Replace(",", "").Replace("$", ""));
                        BaseGravable.SuperficieConstruccion = Convert.ToDouble(txtSuperficeConstruccion.Text);
                        BaseGravable.ConstruccionPrivativa = txtConstruccionPrivativa.Text == "" ? null : (double?)Convert.ToDouble(txtConstruccionPrivativa.Text.Replace(",", "").Replace("$", ""));
                        BaseGravable.ConstruccionComun = txtConstruccionComun.Text == "" ? null : (double?)Convert.ToDouble(txtConstruccionComun.Text.Replace(",", "").Replace("$", ""));
                        BaseGravable.ValorConstruccion = Convert.ToDouble(txtValorConstruccion.Text.Replace(",", "").Replace("$", ""));
                        BaseGravable.Valor = Convert.ToDecimal(BaseGravable.ValorConstruccion + (BaseGravable.ValorTerreno == null ? 0 : BaseGravable.ValorTerreno));
                        //BaseGravable.ValorConstruccionPrivativa = txtValorConstruccionPrivativa.Text == "" ? null : (double?)Convert.ToDouble(txtValorConstruccionPrivativa.Text.Replace(",", "").Replace("_", ""));
                        //BaseGravable.ValorConstruccionComun = txtValorConstruccionComun.Text == "" ? null : (double?)Convert.ToDouble(txtValorConstruccionComun.Text.Replace(",", "").Replace("_", ""));
                        //BaseGravable.Prototipo = txtPrototipo.Text == "" ? null : (double?)Convert.ToDouble(txtPrototipo.Text);
                        BaseGravable.IdUsuario = U.Id;
                        BaseGravable.Activo = true;
                        BaseGravable.FechaModificacion = DateTime.Now;
                        msg = new cBaseGravableBL().Insert(BaseGravable);
                        if (BaseGravable.Ejercicio >= DateTime.Now.Year && (msg == MensajesInterfaz.Ingreso || msg == MensajesInterfaz.Actualizacion))
                        {
                            cPredio predio = new cPredioBL().GetByConstraint(BaseGravable.IdPredio);
                            predio.SuperficieConstruccion = BaseGravable.SuperficieConstruccion;
                            predio.SuperficieTerreno = BaseGravable.SuperficieTerreno;
                            predio.ValorTerreno = BaseGravable.ValorTerreno;
                            predio.ValorConstruccion = BaseGravable.ValorConstruccion;
                            predio.ValorCatastral = Convert.ToDouble(BaseGravable.Valor);
                            predio.ValorComercial = Convert.ToDouble(BaseGravable.Valor);
                            predio.ValorOperacion = Convert.ToDouble(BaseGravable.Valor);
                            predio.FechaAvaluo = FechaAvaluo;
                            msg = new cPredioBL().Update(predio);
                        }
                    }    
                }             


                if (msg == MensajesInterfaz.Actualizacion || msg == MensajesInterfaz.Ingreso)
                {
                    lblAlerta.Visible = true;
                    lblAlerta.ForeColor = System.Drawing.Color.FromName("Green");
                    lblAlerta.Text = "Cambios realizados con Éxito!";
                }
                else
                {
                    lblAlerta.Visible = true;
                    lblAlerta.ForeColor = System.Drawing.Color.FromName("Red");
                    lblAlerta.Text = "Ocurrió un problema al actualizar los datos.";
                }
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);
                //ddlEjercicio.Items.Clear();
                pnlBG_Modal.Show();
                imbBuscar_Click(null, null);
            }
            else
            {
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup("La fecha de avaluo no es valida.", ModalPopupMensaje.TypeMesssage.Alert);
            }
        }
        private void llenaConfiguracion(cBaseGravable BaseGravable)
        {
            txtEjercicio.Text = BaseGravable.Ejercicio.ToString();
            txtClavePredial.Text = BaseGravable.cPredio.ClavePredial;
            lblNombrePredio.Text = BaseGravable.cPredio.cContribuyente.Nombre + " " + BaseGravable.cPredio.cContribuyente.ApellidoPaterno + " " + BaseGravable.cPredio.cContribuyente.ApellidoMaterno;
            lblPeriodoPagado.Text = "Ultimo periodo pagado: " + BaseGravable.cPredio.BimestreFinIp.ToString() + ' ' + BaseGravable.cPredio.AaFinalIp.ToString();
            ddlBimestre.SelectedValue = BaseGravable.Bimestre.ToString();
            txtValor.Text = BaseGravable.Valor.ToString("C");
            txtFechaEvaluo.Text = BaseGravable.FechaAvaluo.ToString("dd/MM/yyyyy");
            txtSuperTerreno.Text = BaseGravable.SuperficieTerreno.ToString();
            txtTerrenoPrivativo.Text = BaseGravable.TerrenoPrivativo.ToString();
            txtTerrenoComun.Text = BaseGravable.TerrenoComun.ToString();
            txtValorTerreno.Text = Convert.ToDecimal(BaseGravable.ValorTerreno).ToString("0,0.00");
            txtSuperficeConstruccion.Text = BaseGravable.SuperficieConstruccion.ToString();
            txtConstruccionPrivativa.Text = BaseGravable.ConstruccionPrivativa.ToString();
            txtConstruccionComun.Text = BaseGravable.ConstruccionComun.ToString();
            txtValorConstruccion.Text = BaseGravable.ValorConstruccion.ToString("0,0.00");
        }
        protected void txtValorTerreno_TextChanged(object sender, EventArgs e)
        {
            double valorCons = 0;
            double valorTerr = 0;
            if (txtValorConstruccion.Text != string.Empty)
            {
                valorCons = Double.Parse(txtValorConstruccion.Text.Replace(",", "").Replace("$", ""));
                txtValorConstruccion.Text = valorCons.ToString("0,0.00");
            }
            if (txtValorTerreno.Text != string.Empty)
            {
                valorTerr = Double.Parse(txtValorTerreno.Text.Replace(",", "").Replace("$", ""));
                txtValorTerreno.Text = valorTerr.ToString("0,0.00");
            }
            txtValor.Text = (valorCons + valorTerr).ToString("C");
            if (sender != null)
            {
                TextBox obj = (TextBox)sender;
                if (obj.ID == "txtValorTerreno")
                {
                    txtValorConstruccion.Focus();
                }
                else
                {
                    btnGuardar.Focus();
                }
            }
            pnlBG_Modal.Show();
        }

        protected void ddlEjercicio_TextChanged(object sender, EventArgs e)
        {
            int IdPredio = (int)ViewState["IdPredio"];
            cBaseGravable BaseGravable = new cBaseGravableBL().GetByPredAnio(IdPredio,Convert.ToInt32(ddlEjercicio.SelectedValue));
            llenaConfiguracion(BaseGravable);
            pnlBG_Modal.Show();
        }

        protected void btnHistorial_Click(object sender, EventArgs e)
        {
            lblAlerta.Visible = false;
            txtFiltro.Text = txtClavePredial.Text;
            ddlEjercicio.Items.Clear();
            llenagrid();
            pnlBG_Modal.Hide();
        }

        protected void btnCierraPanel_Click(object sender, EventArgs e)
        {
            lblAlerta.Visible = false;
            ViewState["IdPredio"] = null;
            ViewState["ClavePredial"] = null;
            grd.DataSourceID = null;
            grd.DataSource = null;
            grd.DataBind();
            configuracionModal(true);
            ddlEjercicio.Items.Clear();
            btnNuevo.Visible = false;
            txtFiltro.Text = "";
            txtFiltro.Focus();
            pnlBG_Modal.Hide();
        }

        #endregion



    }
}