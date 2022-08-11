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
    public partial class catBaseGravable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["parametro"] != null)
                {
                    Dictionary<string, string> parametros = (Dictionary<string, string>)Session["parametro"];
                    if (parametros.ContainsKey("idBase"))
                    {
                        llenaConfiguracion(Convert.ToInt32(parametros["idBase"]));
                        if (parametros.ContainsKey("tipoPantalla"))
                        {
                            if (parametros["tipoPantalla"] == "C")
                            {
                                habilitaCampos(false);
                                lblTitulo.Text = "Consulta Base Gravable";

                            }
                            else {
                                hdfId.Value = parametros["idBase"];
                                lblTitulo.Text = "Edición de Base Gravable";
                            }
                        }
                    }
                }
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenaConfiguracion(int Id)
        {
            cBaseGravable BaseGravable = new cBaseGravableBL().GetByConstraint(Id);
            cPredio predio = new cPredioBL().GetByConstraint(BaseGravable.IdPredio);
            txtClavePredial.Text = predio.ClavePredial;
            lblNombrePredio.Text = predio.cContribuyente.ApellidoPaterno + " " + predio.cContribuyente.ApellidoMaterno +" "+predio.cContribuyente.Nombre;
            ViewState["IdPredio"] = predio.Id;
            txtEjercicio.Text = BaseGravable.Ejercicio.ToString();
            ddlBimestre.SelectedValue = BaseGravable.Bimestre.ToString();
            txtValor.Text = BaseGravable.Valor.ToString();
            txtFechaEvaluo.Text = BaseGravable.FechaAvaluo.ToString("dd/MM/yyyyy");
            txtSuperTerreno.Text = BaseGravable.SuperficieTerreno.ToString();
            txtTerrenoPrivativo.Text = BaseGravable.TerrenoPrivativo.ToString();
            txtTerrenoComun.Text = BaseGravable.TerrenoComun.ToString();
            txtValorTerreno.Text = BaseGravable.ValorTerreno.ToString();
            txtSuperficeConstruccion.Text = BaseGravable.SuperficieConstruccion.ToString();
            txtConstruccionPrivativa.Text = BaseGravable.ConstruccionPrivativa.ToString();
            txtConstruccionComun.Text = BaseGravable.ConstruccionComun.ToString();
            txtValorConstruccion.Text = BaseGravable.ValorConstruccion.ToString();
            //txtValorConstruccionPrivativa.Text = BaseGravable.ValorConstruccionPrivativa.ToString();
            //txtValorConstruccionComun.Text = BaseGravable.ValorConstruccionComun.ToString();
            //txtPrototipo.Text = BaseGravable.Prototipo.ToString();
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {            
            buscaPredio();
            if (lblNombrePredio.Text == "")
                return;            
            if (!new cBaseGravableBL().EsUnicaAnual(txtClavePredial.Text, int.Parse(txtEjercicio.Text)))
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.BaseRepetida), ModalPopupMensaje.TypeMesssage.Confirm);
                return;
            }
            guarda();
        }
        protected void guarda()
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            vtnModal.DysplayCancelar = false;
            MensajesInterfaz msg = new MensajesInterfaz();
            cBaseGravable BaseGravable = new cBaseGravable();
            if (!(hdfId.Value == string.Empty || hdfId.Value == "0"))
            {
                BaseGravable = new cBaseGravableBL().GetByConstraint(Convert.ToInt32(hdfId.Value));
            }

            BaseGravable.IdPredio = int.Parse(ViewState["IdPredio"].ToString());
            BaseGravable.Ejercicio = int.Parse(txtEjercicio.Text);
            BaseGravable.Bimestre = int.Parse(ddlBimestre.SelectedValue);
            BaseGravable.FechaAvaluo = Convert.ToDateTime(txtFechaEvaluo.Text);
            BaseGravable.SuperficieTerreno = txtSuperTerreno.Text == "" ? null : (double?)Convert.ToDouble(txtSuperTerreno.Text);
            BaseGravable.TerrenoPrivativo = txtTerrenoPrivativo.Text == "" ? null : (double?)Convert.ToDouble(txtTerrenoPrivativo.Text);
            BaseGravable.TerrenoComun = txtTerrenoComun.Text == "" ? null : (double?)Convert.ToDouble(txtTerrenoComun.Text);
            BaseGravable.ValorTerreno = txtValorTerreno.Text == "" ? null : (double?)Convert.ToDouble(txtValorTerreno.Text);
            BaseGravable.SuperficieConstruccion = Convert.ToDouble(txtSuperficeConstruccion.Text);
            BaseGravable.ConstruccionPrivativa = txtConstruccionPrivativa.Text == "" ? null : (double?)Convert.ToDouble(txtConstruccionPrivativa.Text);
            BaseGravable.ConstruccionComun = txtConstruccionComun.Text == "" ? null : (double?)Convert.ToDouble(txtConstruccionComun.Text);
            BaseGravable.ValorConstruccion = Convert.ToDouble(txtValorConstruccion.Text);
            BaseGravable.Valor = Convert.ToDecimal(BaseGravable.ValorConstruccion + (BaseGravable.ValorTerreno == null ? 0 : BaseGravable.ValorTerreno));
            //BaseGravable.ValorConstruccionPrivativa = txtValorConstruccionPrivativa.Text == "" ? null : (double?)Convert.ToDouble(txtValorConstruccionPrivativa.Text);
            //BaseGravable.ValorConstruccionComun = txtValorConstruccionComun.Text == "" ? null : (double?)Convert.ToDouble(txtValorConstruccionComun.Text);
            //BaseGravable.Prototipo = txtPrototipo.Text == "" ? null : (double?)Convert.ToDouble(txtPrototipo.Text);
            BaseGravable.IdUsuario = U.Id;
            BaseGravable.Activo = true;
            BaseGravable.FechaModificacion = DateTime.Now;
            if (hdfId.Value == string.Empty || hdfId.Value == "0")
            {
                msg = new cBaseGravableBL().Insert(BaseGravable);
            }
            else
            {
                msg = new cBaseGravableBL().Update(BaseGravable);
            }
            if (BaseGravable.Ejercicio >= DateTime.Now.Year && (msg == MensajesInterfaz.Ingreso || msg == MensajesInterfaz.Actualizacion))
            {
                cPredio predio = new cPredioBL().GetByConstraint(BaseGravable.IdPredio);
                predio.SuperficieConstruccion = BaseGravable.SuperficieConstruccion;
                predio.SuperficieTerreno = BaseGravable.SuperficieTerreno;
                predio.ValorTerreno = BaseGravable.ValorTerreno;
                predio.ValorConstruccion = BaseGravable.ValorConstruccion;
                predio.ValorCatastral = Convert.ToDouble(BaseGravable.Valor);
                msg = new cPredioBL().Update(predio);
            }
            vtnModal.DysplayCancelar = false;
            vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session["parametro"] = null;
            Response.Redirect("BusquedaBaseGravable.aspx");
        }
        
        private void habilitaCampos(bool activa)
        {
            txtClavePredial.Enabled = activa;
            txtEjercicio.Enabled = activa;
            ddlBimestre.Enabled = activa;
            txtValor.Enabled = activa;
            txtFechaEvaluo.Enabled = activa;
            txtSuperTerreno.Enabled = activa;
            txtTerrenoPrivativo.Enabled = activa;
            txtTerrenoComun.Enabled = activa;
            txtValorTerreno.Enabled = activa;
            txtSuperficeConstruccion.Enabled = activa;
            txtConstruccionPrivativa.Enabled = activa;
            txtConstruccionComun.Enabled = activa;
            txtValorConstruccion.Enabled = activa;
            txtValorConstruccionPrivativa.Enabled = activa;
            txtValorConstruccionComun.Enabled = activa;
            txtPrototipo.Enabled = activa;
            btnGuardar.Visible = activa;
            imbBuscarPredio.Visible = activa;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
           if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                Session["parametro"] = null;
                Response.Redirect("BusquedaBaseGravable.aspx");
            }
            else if(vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.BaseRepetida))
            {
                MensajesInterfaz msg = new cBaseGravableBL().UpdateOtrasBases(txtClavePredial.Text, int.Parse(txtEjercicio.Text));
                hdfId.Value = "0";
                guarda();
            }
        }
        protected void imbBuscarPredio_Click(object sender, ImageClickEventArgs e)
        {
            buscaPredio();
        }

        protected void buscaPredio()
        {
            if (txtClavePredial.Text != string.Empty)
            {
                cPredio predio = new cPredioBL().GetByClavePredial(txtClavePredial.Text);
                if (predio != null)
                {
                    lblNombrePredio.Text =  "Contribuyente: "+predio.cContribuyente.ApellidoPaterno + " " + predio.cContribuyente.ApellidoMaterno + " " + predio.cContribuyente.Nombre;
                    if (predio.ClavePredial.Substring(0, 1) == "0")
                        lblCuentaPredial.Text = "Cuenta Predial: "+predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClaveAnterior.Substring(9, 3);
                    ViewState["IdPredio"] = predio.Id;
                }
                else
                {
                    txtClavePredial.Text = "";
                    lblNombrePredio.Text = "";
                    lblCuentaPredial.Text = "";
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                    ViewState["IdPredio"] = 0;
                }
            }
            else
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
        }

        protected void txtValorTerreno_TextChanged(object sender, EventArgs e)
        {
            double valorCons = 0;
            double valorTerr = 0;
            if (txtValorConstruccion.Text != string.Empty )
            {
                valorCons = Double.Parse(txtValorConstruccion.Text);
            }
            if (txtValorTerreno.Text != string.Empty)
            {
                valorTerr = Double.Parse(txtValorTerreno.Text);
            }
            txtValor.Text = (valorCons + valorTerr).ToString("C");
        }
    }
}