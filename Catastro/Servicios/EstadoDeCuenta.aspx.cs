using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Globalization;
using System.Transactions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

using Catastro.Controles;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using System.Collections;
using System.IO;
using System.Web;

using GeneraRecibo33;
using System.Configuration;
using System.Text;
using Catastro.ModelosFactura;
using Comprobante = GeneraRecibo33.Comprobante;
using Receptor = GeneraRecibo33.Receptor;
using System.Data.SqlTypes;
using Factura = GeneraRecibo33.Factura;
using System.Xml;
using System.Data.SqlClient;
using SOAPT.Clases;

namespace Catastro.Servicios
{

    public partial class EstadoDeCuenta : System.Web.UI.Page
    {
        List<MetodoGrid> listConcepto = new List<MetodoGrid>();
        cPredio predio = new cPredio();
        Impuesto i = new Impuesto();
        Servicio sm = new Servicio();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblImpuestoAnt.Text = Convert.ToString(DateTime.Today.Year + 1);
            lblImpuestoActual.Text = Convert.ToString(DateTime.Today.Year);
            //lblreza.Text = Convert.ToString(DateTime.Today.Year + 1);


            if (!IsPostBack)
            {
                btnCalcular.Visible = false;
                divEstadoCta.Visible = false;
                divDetallado.Visible = false;
                InicializaEncabezado();
                InicializaDetalle();
                //lblPrincipal.Text = "Cobro de Impuesto Predial ";
                txtClavePredial.Focus();

            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
            ValidaPermisos();
        }
        protected void ValidaPermisos()
        {
            //DESCUENTOS REZAGOS 10-10-40
            txtClaveDescuento.Visible = false;
            lblDescuento.Visible = false;
            imgGuardarDescto.Visible = false;
            imgEliminarDescto.Visible = false;

            //COBROS DE IMPUESTOS SOLO EL ROL CAJERO	10-10-30
            checkBoxJYP.Visible = false;
            btnCobrar.Visible = false;

            //imprimir ultimorecibo
            btnUltimoRec.Visible = false;

            //INICIAR CONVENIO 10-10-10
            btnConvenio.Visible = false;

            //ESTADOS DE CUENTA PDF 10-10-20
            btnEstado.Visible = false;

            //MODIFICA PERIODO DE COBRO INCIAL   10-10-50
            ddlEjIniIP.Enabled = false;
            ddlBimIniIP.Enabled = false;
            ddlBimIniSM.Enabled = false;
            ddlEjIniSm.Enabled = false;


            cUsuarios U = (cUsuarios)Session["usuario"];

            List<vPermisoBoton> permiso = new vVistasBL().ObtienePermisoPorBoton(U.Id);

            foreach (vPermisoBoton per in permiso)
            {
                switch (per.ClaveBoton)
                {
                    case "10-10-10":
                        btnConvenio.Visible = true;
                        break;
                    case "10-10-20":
                        btnEstado.Visible = true;
                        break;
                    case "10-10-30":
                        checkBoxJYP.Visible = true;
                        btnCobrar.Visible = true;
                        btnUltimoRec.Visible = true;
                        break;
                    case "10-10-40":
                        txtClaveDescuento.Visible = true;
                        lblDescuento.Visible = true;
                        imgGuardarDescto.Visible = true;
                        imgEliminarDescto.Visible = true;
                        break;
                    case "10-10-50":
                        ddlEjIniIP.Enabled = true;
                        ddlBimIniIP.Enabled = true;
                        ddlBimIniSM.Enabled = true;
                        ddlEjIniSm.Enabled = true;
                        break;
                        //default:
                        //    ApagaControles();
                        //    break;
                }
            }
        }
        protected void bntNuevaConsulta_Click(object sender, EventArgs e)
        {
            btnCalcular.Visible = false;
            divEstadoCta.Visible = false;
            divDetallado.Visible = false;
            InicializaEncabezado();
            llenaCombos();
            InicializaDetalle();
            ReiniciaControl(null, null);
            txtClavePredial.Focus();
        }
        protected void btnCobrar_Click(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            if (rdbSM.Checked)
            {
                rdbSM.Checked = false;
                rdbIP.Checked = true;
                //txtApagar.Text = txtTotalIp.Text;
            }
            //valida la caja
            int idMesa = new cMesaBL().GetByNombre(new cParametroSistemaBL().GetByClave("MESAIP").Valor);
            tConfiguracionMesa config = new tConfiguracionMesaBL().ValidaConfiguracion(U.Id, idMesa);
            if (config == null)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("El cajero no tiene configurada la mesa de PREDIAL"), Controles.ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            ViewState["CajaIP"] = config.IdCaja;
            ViewState["MesaIP"] = config.IdMesa;

            idMesa = new cMesaBL().GetByNombre(new cParametroSistemaBL().GetByClave("MESAIP").Valor);
            config = new tConfiguracionMesaBL().ValidaConfiguracion(U.Id, idMesa);
            if (config == null)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("El cajero no tiene configurada la mesa de SERVICIOS MUNCIPALES"), Controles.ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            ViewState["CajaSM"] = config.IdCaja;
            ViewState["MesaSM"] = config.IdMesa;
            ddlMetodoPago.SelectedIndex = -1;

            pnl_Modal.Show();

            /// deriva de funcionalidad cobro
            lblImportePago.Text = "Importe: " + txtTotalIp.Text;
            llenaTipoPago();
            activaEtiquetasModal("0", true); //validar que es el 4
            txtObservacion.Text = ViewState["observaciones"] == null ? "" : ViewState["observaciones"].ToString();

            lblCambio.Text = "Cambio: ";
            txtNumeroAprobacion.Text = "";
            grdAlta.DataSource = null;
            grdAlta.DataBind();
            ddlMetodoPago.ClearSelection();
            grdAlta.Visible = false;
            //pnl_Modal.Hide();
        }
        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            InicializaDetalle();
            Calcula();
            string clave = ViewState["ClavePredial"].ToString();
            //lblPrincipal.Text = "Estado de Cuenta";
            lbl_titulo.Text = txtPropietario.Text;
            lblDescripcion.Text = "Clave catastral: " + ViewState["ClavePredial"].ToString().Substring(0, 4) + "-" + ViewState["ClavePredial"].ToString().Substring(4, 2) + "-" +
                                ViewState["ClavePredial"].ToString().Substring(6, 3) + "-" + ViewState["ClavePredial"].ToString().Substring(9, 3);
            lblDetalle.Text = "Tipo predio: " + txtTipoPredio.Text + "     " + "Sup. Terreno: " + txtSuperficieTerreno.Text + "      " + "Sup. Construc: " + txtSuperficieConstruccion.Text + "      " + "Base Gravable: " + txtBaseGravable.Text;
            //btnCobrar.Visible = true;
            if (Convert.ToDecimal(txtTotalIp.Text) > 0)
            {
                // txtApagar.Text = txtTotalIp.Text; //NELY
                //rdbIP.Checked = true;
            }
            else
            {
                // txtApagar.Text = txtTotalSm.Text; //NELY
                // rdbSM.Checked = true;
            }
            ValidaPermisos();
        }
        protected void btnEstado_Click(object sender, EventArgs e)
        {
            CalculaEstado();
            ////VISUALIZA EL RECIBO   
            frameEstado.Src = "~/Temporales/" + ViewState["pdf"].ToString();
            modalEstado.Show();
            ViewState["pdf"] = "";
        }
        protected void btnConvenio_Click(object sender, EventArgs e)
        {
            if (rdbDiferencias.Checked == true)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("El convenio NO aplica solo a las diferencias"), ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            //if (Convert.ToInt32(ddlEjFinIP.SelectedValue) > DateTime.Today.Year || Convert.ToInt32(ddlEjFinSM.SelectedValue) > DateTime.Today.Year)
            //{
            //    vtnModal.ShowPopup(new Utileria().GetDescription("El convenio no acepta pago anticipado"), ModalPopupMensaje.TypeMesssage.Alert);
            //    return;
            //}
            modalConvenio.Show();
        }
        protected void btnAceptarConv_Click(object sender, EventArgs e)
        {
            GeneraConvenioEstadoCta();
        }
        protected void imCerrarRecibo_Click(object sender, ImageClickEventArgs e)
        {
            InicializaDetalle();
            divEstadoCta.Visible = false;
            divEncabezado.Visible = true;
        }
        protected void btnImprimeRecibo_Click(object sender, EventArgs e)
        {
            ImprimeRecibo();
        }
        protected void InicializaEncabezado()
        {
            txtClavePredial.Text = "";
            lblCuentaPredial.Text = "";
            lblCuentaPredialTxt.Text = "";
            txtPropietario.Text = "";
            txtTipoPredio.Text = "";
            txtUbicacion.Text = "";
            txtUltimoPago.Text = " --- ";
            txtUltimoPagoSm.Text = " --- ";
            txtVigencia.Text = "  /  /  ";
            txtZona.Text = "";
            txtMetrosFrente.Text = "0";
            txtValorConstruccion.Text = "0";
            txtValorTerreno.Text = "0";
            txtSuperficieConstruccion.Text = "0";
            txtSuperficieTerreno.Text = "0";
            txtBaseGravable.Text = "0";
            txtImporteDif.Text = "0";
            txtFaseIP.Text = " --- ";
            txtFaseSM.Text = " --- ";
            rdbDiferencias.Visible = true;
            LlenaComboIP(0);
            LlenaComboSM(0);
            activaCombos(false);
            llenaCombos();
        }
        protected void InicializaDetalle()
        {
            txtAdicional.Text = "0";
            txtAdicionalAnt.Text = "0";
            txtImpuesto.Text = "0";
            txtDiferencias.Text = "0";
            txtEjecucion.Text = "0";
            txtDapSm.Text = "0";
            txtAdicionalAntSm.Text = "0";
            txtAdicionalSm.Text = "0";
            txtEjecucionSm.Text = "0";
            txtImporte.Text = "0";
            txtImpuestoAnt.Text = "0";
            txtInfraestructuraAntSm.Text = "0";
            txtInfraestructuraSm.Text = "0";
            txtLimpiezaSm.Text = "0";
            txtMultas.Text = "0";
            txtMultasSm.Text = "0";
            txtRecargos.Text = "0";
            txtRecargosDif.Text = "0";
            txtRecargosSm.Text = "0";
            txtRecoleccionSm.Text = "0";
            txtRezagos.Text = "0";
            txtRezagosSm.Text = "0";
            txtTotalIp.Text = "0";
            txtTotalSm.Text = "0";
            txtDescuentos.Text = "0";
            txtDescuentosSm.Text = "0";
            //txtApagar.Text = "0";
            txtPeriodoIP.Text = "-";
            txtPeriodoSM.Text = "-";
            txtVigencia.Text = "-";
            //txtObservaciones.Text = "";
            //txtCheque.Text = "";
            //txtNoAprobacion.Text = "";
            //txtEfectivo.Text = "";
            //txtTarjeta.Text = "";
            lblWarning.Visible = false;
            lblWarning.Text = "";
            lblDescripcion.Text = "";

            txtDescImpuestoAnt.Text = "";
            txtDescImpuesto.Text = "";
            txtDescDiferencias.Text = "";
            txtDescRecargoDif.Text = "";
            txtDescRezagos.Text = "";
            txtDescRecargos.Text = "";
            txtDescHonorarios.Text = "";
            txtdDescEjecucion.Text = "";
            textDescMultas.Text = "";
        }
        protected void activaCombos(bool activa)
        {
            ddlBimFinIP.Enabled = activa;
            ddlEjFinIP.Enabled = activa;
            ddlBimFinSm.Enabled = activa;
            ddlEjFinSM.Enabled = activa;
        }
        protected void LlenaComboIP(int ejercicio)
        {
            int eIni = 0;

            ddlEjIniIP.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-", "0"));
            ddlEjFinIP.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-", "0"));
            ddlBimIniIP.SelectedIndex = -1;
            ddlEjIniIP.SelectedIndex = -1;
            ddlBimFinIP.SelectedIndex = -1;
            ddlEjFinIP.SelectedIndex = -1;

            if (ejercicio > 0)
            {
                if (ejercicio == DateTime.Today.Year + 2 && ejercicio == DateTime.Today.Year + 2)
                {
                    eIni = ejercicio;
                    ejercicio--;
                }
                ddlEjIniIP.Items.Clear();
                ddlEjIniIP.DataValueField = "Ejercicio";
                ddlEjIniIP.DataTextField = "Ejercicio";
                ddlEjIniIP.DataSource = new cBaseImpuestoBL().GetByEjercicioInicial(ejercicio);
                ddlEjIniIP.DataBind();

                ddlEjFinIP.Items.Clear();
                ddlEjFinIP.DataValueField = "Ejercicio";
                ddlEjFinIP.DataTextField = "Ejercicio";
                ddlEjFinIP.DataSource = new cBaseImpuestoBL().GetByEjercicioInicial(ejercicio);
                ddlEjFinIP.DataBind();

                if (eIni > 0)
                {
                    ddlEjIniIP.Items.Insert(ddlEjIniIP.Items.Count, new System.Web.UI.WebControls.ListItem(eIni.ToString(), eIni.ToString()));
                    ddlEjFinIP.Items.Insert(ddlEjFinIP.Items.Count, new System.Web.UI.WebControls.ListItem(eIni.ToString(), eIni.ToString()));

                }
            }

        }
        protected void LlenaComboSM(int ejercicio)
        {
            int eIni = 0;
            ddlEjIniSm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-", "0"));
            ddlEjFinSM.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-", "0"));
            ddlBimIniSM.SelectedIndex = -1;
            ddlEjIniSm.SelectedIndex = -1;
            ddlBimFinSm.SelectedIndex = -1;
            ddlEjFinSM.SelectedIndex = -1;

            if (ejercicio > 0)
            {
                if (ejercicio == DateTime.Today.Year + 2)
                {
                    eIni = ejercicio;
                    ejercicio--;
                }

                //servicios 
                ddlEjIniSm.Items.Clear();
                ddlEjIniSm.DataValueField = "Ejercicio";
                ddlEjIniSm.DataTextField = "Ejercicio";
                ddlEjIniSm.DataSource = new cBaseImpuestoBL().GetByEjercicioInicial(ejercicio);
                ddlEjIniSm.DataBind();

                ddlEjFinSM.Items.Clear();
                ddlEjFinSM.DataValueField = "Ejercicio";
                ddlEjFinSM.DataTextField = "Ejercicio";
                ddlEjFinSM.DataSource = new cBaseImpuestoBL().GetByEjercicioInicial(ejercicio);
                ddlEjFinSM.DataBind();

                if ((eIni > 0) && (eIni < DateTime.Now.Year + 1))
                {
                    ddlEjIniSm.Items.Insert(ddlEjIniIP.Items.Count, new System.Web.UI.WebControls.ListItem(eIni.ToString(), eIni.ToString()));
                    ddlEjFinSM.Items.Insert(ddlEjFinIP.Items.Count, new System.Web.UI.WebControls.ListItem(eIni.ToString(), eIni.ToString()));
                }
            }

        }
        protected void llenaCombos()
        {
            ddlMetodoPago.Items.Clear();
            ddlMetodoPago.DataValueField = "Clave";
            ddlMetodoPago.DataTextField = "Descripcion";
            ddlMetodoPago.DataSource = new cTipoPagoBL().GetAll();
            ddlMetodoPago.DataBind();
            ddlMetodoPago.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecciona Método", ""));
        }
        protected void buscaPredio(object sender, ImageClickEventArgs e)
        {
            SaldosC s = new SaldosC();
            Utileria h = new Utileria();
            Periodo per = new Periodo();
            cRFC rfc = new cRFC();
            string visiblebtnCalcular = string.Empty;

            if (txtClavePredial.Text.Length < 12)
            {
                //txtClavePredial.Text = 'C' + txtClavePredial.Text;
                txtClavePredial.Text = "";
                lblCuentaPredialTxt.Text = "";
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["IdPredio"] = 0;
                ViewState["ClavePredial"] = "";
                ViewState["CuentaPredial"] = "";
                return;
            }
            if (txtClavePredial.Text != string.Empty)
            {
                //if (rbClave.Checked)
                predio = new cPredioBL().GetByClavePredial(txtClavePredial.Text);
                //else
                //predio = new cPredioBL().GetByCuentaPredial(txtClavePredial.Text);

                if (predio != null)
                {
                    if (predio.cStatusPredio.Descripcion != "A")
                    {
                        InicializaEncabezado();
                        vtnModal.ShowPopup(new Utileria().GetDescription(predio.cStatusPredio.Descripcion == "B" ? MensajesInterfaz.sTatusPredioBaja : MensajesInterfaz.sTatusPredioSuspendido), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }

                    int tc = new tConvenioBL().ObtenerConvenioPorIdPredio(predio.Id);

                    if (tc > 0)
                    {
                        InicializaEncabezado();
                        vtnModal.ShowPopup(new Utileria().GetDescription("La clave predial: " + predio.ClavePredial.Substring(0, 4) +
                            "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" +
                             predio.ClavePredial.Substring(9, 3) + " presenta un convenio."), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }

                    btnCobrar.Visible = true;
                    ViewState["IdPredio"] = predio.Id;
                    ViewState["ClavePredial"] = predio.ClavePredial;
                    ViewState["CuentaPredial"] = "";
                    if (predio.ClavePredial.Substring(0, 1) == "0") //solo si se refiere a una cuenta, la clave empieza con 0
                    {
                        ViewState["CuentaPredial"] = predio.ClaveAnterior;
                        lblCuentaPredialTxt.Visible = true;
                        lblCuentaPredial.Visible = true;
                        lblCuentaPredialTxt.Text = predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClaveAnterior.Substring(9, 3);
                    }

                    //cBaseGravable bg = new cBaseGravableBL().GetByConstraint(predio.Id);
                    txtClavePredial.Text = predio.ClavePredial;
                    txtPropietario.Text = (predio.cContribuyente.RazonSocial) + ' ' + (predio.cContribuyente.ApellidoPaterno) +
                        ' ' + (predio.cContribuyente.ApellidoMaterno) + ' ' + (predio.cContribuyente.Nombre);
                    txtUbicacion.Text = predio.Calle + ' ' + predio.Numero + ' ' + predio.cColonia.NombreColonia + ' ' + predio.Localidad;
                    txtSuperficieConstruccion.Text = predio.SuperficieConstruccion.ToString();
                    txtSuperficieTerreno.Text = predio.SuperficieTerreno.ToString();
                    txtValorTerreno.Text = Convert.ToDouble(predio.ValorTerreno).ToString("N", CultureInfo.CurrentCulture);
                    txtValorConstruccion.Text = Convert.ToDouble(predio.ValorConstruccion).ToString("N", CultureInfo.CurrentCulture);
                    txtTipoPredio.Text = predio.cTipoPredio.Descripcion;

                    txtZona.Text = predio.Zona.ToString();
                    txtMetrosFrente.Text = predio.MetrosFrente.ToString();
                    txtBaseGravable.Text = Convert.ToDouble(predio.ValorCatastral).ToString("N", CultureInfo.CurrentCulture);
                    txtUltimoPago.Text = predio.BimestreFinIp.ToString() + " - " + predio.AaFinalIp.ToString();
                    txtUltimoPagoSm.Text = predio.BimestreFinSm.ToString() + " - " + predio.AaFinalSm.ToString();

                    //tRequerimiento req = new tRequerimientoBL().obtenerRequerimientoporIdPredio(predio.Id, "IP");
                    //txtFaseIP.Text = new cTipoFaseBL().GetByFase(req==null? 1:req.TipoFase);
                    //tRequerimiento reqS = new tRequerimientoBL().otenerRequerimientoporIdPredio(predio.Id, "SM");
                    //txtFaseSM.Text = new cTipoFaseBL().GetByFase(req == null ? 1 : req.TipoFase);
                    //int faseip = new cTipoFaseBL().GetByConstraint(predio.IdTipoFaseIp).Fase;

                    txtFaseIP.Text = new cTipoFaseBL().GetByConstraint(predio.IdTipoFaseIp).Descripcion;
                    txtFaseSM.Text = new cTipoFaseBL().GetByConstraint(predio.IdTipoFaseSm).Descripcion;

                    rfc = new cRfcBL().GetRfcByIdcontribuyente(predio.IdContribuyente);
                    if (rfc != null)
                    {
                        lblNombreFiscaltxt.Text = rfc.Nombre;
                        lblRFCtxt.Text = rfc.RFC;
                        lblCorreoFiscaltxt.Text = rfc.Email;
                        lblDirFiscaltxt.Text = rfc.Calle + "  No: " + rfc.NoExterior + ",  " + rfc.Colonia + ", " + rfc.Municipio;
                        lblEstadotxt.Text = rfc.Estado + ".  CP: " + rfc.CodigoPostal;
                    }

                    int eIni = Convert.ToInt32(new cParametroCobroBL().GetByClave("EjercicioInicialCobro"));
                    llenaCombos();
                    activaCombos(true);

                    //predial
                    per = s.ValidaPeriodoPago(predio.BimestreFinIp, predio.AaFinalIp, 6, DateTime.Today.Year, "CalculaPredial");
                    if (per.mensaje != 0)
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription(per.mensaje), ModalPopupMensaje.TypeMesssage.Alert);
                        InicializaDetalle();
                        return;
                    }
                    if (per.eInicial >= DateTime.Today.Year + 2)
                    {
                        ddlBimIniIP.SelectedIndex = -1;
                        ddlEjIniIP.SelectedIndex = -1;
                        ddlBimFinIP.SelectedIndex = -1;
                        ddlEjFinIP.SelectedIndex = -1;
                        ddlEjIniIP.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-", "0"));
                        ddlEjFinIP.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-", "0"));

                        ddlBimIniIP.Enabled = false;
                        ddlEjIniIP.Enabled = false;
                        ddlBimFinIP.Enabled = false;
                        ddlEjFinIP.Enabled = false;
                        //rdbIP.Enabled = false;
                        visiblebtnCalcular = "F";
                    }
                    else
                    {
                        LlenaComboIP(per.eInicial);
                        if (per.eInicial > per.eFinal)
                        {
                            ddlBimIniIP.SelectedIndex = -1;
                            ddlEjIniIP.SelectedIndex = -1;
                            ddlBimFinIP.SelectedIndex = -1;
                            ddlEjFinIP.SelectedIndex = -1;
                            ddlEjIniIP.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-", "0"));
                            ddlEjFinIP.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-", "0"));
                            ddlBimIniIP.Enabled = false;
                            ddlEjIniIP.Enabled = false;
                            ddlBimFinIP.Enabled = false;
                            ddlEjFinIP.Enabled = false;
                            visiblebtnCalcular = "F";
                        }
                        else
                        {
                            ddlBimIniIP.SelectedValue = per.bInicial.ToString();
                            ddlEjIniIP.SelectedValue = per.eInicial.ToString();
                            ddlBimFinIP.SelectedValue = per.bFinal.ToString();
                            ddlEjFinIP.SelectedValue = per.eFinal.ToString();
                        }
                    }

                    //servicios
                    per = new Periodo();
                    Periodo perS = s.ValidaPeriodoPago(predio.BimestreFinSm, predio.AaFinalSm, 6, DateTime.Today.Year, "CalculaServicios");
                    if (perS.mensaje != 0)
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription(per.mensaje), ModalPopupMensaje.TypeMesssage.Alert);
                        InicializaDetalle();
                        return;
                    }
                    if (perS.eInicial >= DateTime.Today.Year + 2)
                    {
                        ddlBimIniSM.SelectedIndex = -1;
                        ddlEjIniSm.SelectedIndex = -1;
                        ddlBimFinSm.SelectedIndex = -1;
                        ddlEjFinSM.SelectedIndex = -1;
                        ddlEjIniSm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-", "0"));
                        ddlEjFinSM.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-", "0"));
                        ddlBimIniSM.Enabled = false;
                        ddlEjIniSm.Enabled = false;
                        ddlBimFinSm.Enabled = false;
                        ddlEjFinSM.Enabled = false;
                        rdbSM.Enabled = false;
                        visiblebtnCalcular = visiblebtnCalcular + "F";
                    }
                    else
                    {
                        LlenaComboSM(perS.eInicial);
                        if (perS.eInicial > perS.eFinal) //DateTime.Today.Year + 2)//if (perS.eInicial > perS.eFinal)
                        {
                            ddlBimIniSM.SelectedIndex = -1;
                            ddlEjIniSm.SelectedIndex = -1;
                            ddlBimFinSm.SelectedIndex = -1;
                            ddlEjFinSM.SelectedIndex = -1;
                            ddlEjIniSm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-", "0"));
                            ddlEjFinSM.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-", "0"));
                            ddlBimIniSM.Enabled = false;
                            ddlEjIniSm.Enabled = false;
                            ddlBimFinSm.Enabled = false;
                            ddlEjFinSM.Enabled = false;
                            visiblebtnCalcular = visiblebtnCalcular + "F";
                        }
                        else ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        {
                            ddlBimIniSM.SelectedValue = perS.bInicial.ToString();
                            ddlEjIniSm.SelectedValue = perS.eInicial.ToString();
                            ddlBimFinSm.SelectedValue = perS.bFinal.ToString();
                            ddlEjFinSM.SelectedValue = perS.eFinal.ToString();
                        }
                    }
                    i = s.CalculaCobro(predio.Id, "SI", predio.BimestreFinIp, predio.AaFinalIp, predio.BimestreFinSm, predio.AaFinalIp, 0, 0, "CalculaPredial");
                    if (i.mensaje != 0)
                    {
                        InicializaEncabezado();
                        InicializaDetalle();
                        vtnModal.ShowPopup(new Utileria().GetDescription(i.mensaje), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }
                    if (i.Importe > 0)
                    {
                        txtImporteDif.Text = (i.Importe).ToString("N", CultureInfo.CurrentCulture);
                        rdbDiferencias.Visible = true;
                        visiblebtnCalcular = "";
                    }
                    else
                        rdbDiferencias.Visible = false;


                    //validar si la fecha de avaluo es menor a dos años
                    string fechaAvaluo = new cParametroSistemaBL().GetValorByClave("VALIDACIONFECHAAVALUO");
                    if (fechaAvaluo == "SI")
                    {
                        DateTime vigencia = DateTime.Today;
                        vigencia = vigencia.AddDays(-730);

                        if (predio.FechaAvaluo <= vigencia)
                        {
                            btnCalcular.Visible = false;
                            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FechaAvaluo), ModalPopupMensaje.TypeMesssage.Alert);
                            return;
                        }
                    }

                }
                else
                {
                    txtClavePredial.Text = "";
                    //if (rbClave.Checked)
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                    //else
                    //   vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.CuentaInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                    ViewState["IdPredio"] = 0;
                    ViewState["ClavePredial"] = "";
                    InicializaDetalle();
                    InicializaEncabezado();
                    return;
                }
                btnCalcular.Visible = true;
                if (visiblebtnCalcular == "FF") btnCalcular.Visible = false;
                txtClaveDescuento.Enabled = true;
                txtClaveDescuento.Text = new tDescuentoAsignadoBL().RegresaClaveDesctoByIdPredio(predio.Id);
                if (txtClaveDescuento.Text.Trim() != "")
                    txtClaveDescuento.Enabled = false;
            }
            else
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
        }
        protected void CalculaEstado()
        {
            int idPredio = Convert.ToInt32(ViewState["IdPredio"].ToString());
            SaldosC s = new SaldosC();
            Impuesto i = new Impuesto();
            Servicio sm = new Servicio();
            cBaseGravable bg = new cBaseGravable();
            cRFC rfc = new cRFC();
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];

            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            predio = new cPredioBL().GetByClavePredial(ViewState["ClavePredial"].ToString());
            //rfc = new cRfcBL().GetRfcByIdpredio(predio.IdContribuyente);
            rfc = new cRfcBL().GetRfcByIdcontribuyente(predio.IdContribuyente);
            i = (Impuesto)ViewState["IP"];
            sm = (Servicio)ViewState["SM"];

            string Path = Server.MapPath("~/"); //ConfigurationManager.AppSettings["NombreSitioWeb"];  //Server.MapPath("~/");

            string formOriginal = Path + "/Documentos/EstadoDeCuenta_" + new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor + ".pdf";
            string ruta = predio.ClavePredial.ToString().Trim() + "-" + DateTime.Now.Millisecond.ToString() + ".pdf";
            ViewState["pdf"] = ruta;
            string formImprimir = Path + "/Temporales/" + ruta;

            #region  llenar pdf
            PdfReader reader = new PdfReader(formOriginal);
            PdfStamper stamper = new PdfStamper(reader, new FileStream(formImprimir, FileMode.Create));

            //se comentan los campos con la informacion de los servicios municipales
            AcroFields fields = stamper.AcroFields;

            fields.SetField("UltimoPerPagado", predio.BimestreFinIp.ToString() + " - " + predio.AaFinalIp.ToString());
            fields.SetField("Vigencia", "Vigencia al " + oUltimoDiaDelMes.ToString("dd") + " de " + oUltimoDiaDelMes.ToString("MMMM").ToUpper() + " de " + oUltimoDiaDelMes.ToString("yyyy"));
            fields.SetField("Cuenta Predial", "");
            if (!(predio.ClaveAnterior is null || predio.ClaveAnterior == "")) fields.SetField("Cuenta Predial", predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClaveAnterior.Substring(9, 3));
            fields.SetField("Clave Catastral", predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
            fields.SetField("Contribuyente", HttpUtility.HtmlDecode(predio.cContribuyente.ApellidoPaterno.Trim() + ' ' + predio.cContribuyente.ApellidoMaterno.Trim() + ' ' + predio.cContribuyente.Nombre.Trim() + ' ' + predio.cContribuyente.RazonSocial.Trim()));
          fields.SetField("Direccion", HttpUtility.HtmlDecode(predio.Calle) + ' ' + predio.Numero);
            fields.SetField("Direccion2", HttpUtility.HtmlDecode(predio.cColonia.NombreColonia + ' ' + predio.Localidad));
            fields.SetField("Antecedente", HttpUtility.HtmlDecode(predio.cPredioObservacion.Count > 0 ? predio.cPredioObservacion.ToString() : ""));
            fields.SetField("Referencia", "");
            if (!(predio.Referencia is null)) fields.SetField("Referencia", HttpUtility.HtmlDecode(predio.Referencia));
            //fields.SetField("Zona", predio.Zona.ToString());
            //fields.SetField("Metros Frente", predio.MetrosFrente.ToString());
            fields.SetField("Val Terreno", Convert.ToDecimal(predio.ValorTerreno).ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("Superficie Terreno", predio.SuperficieTerreno.ToString() + " m2");
            fields.SetField("Val Construccion", predio.ValorConstruccion.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("Superficie Construccion", predio.SuperficieConstruccion.ToString() + " m2");
            fields.SetField("Uso Suelo", predio.cUsoSuelo.Descripcion.ToString());
            fields.SetField("Base Gravable", predio.ValorCatastral.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("SuperficeAreaComun", predio.TerrenoComun.ToString() + " m2");
            fields.SetField("PeriodoIP", i.Estado.PeriodoGral);
            fields.SetField("ImpuestoAntIP", i.Estado.AntImpuesto.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("AdicionalAntIP", i.Estado.AntAdicional.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("ImpuestoIP", i.Estado.Impuesto.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("DiferenciasIP", i.Estado.Diferencias.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("RecDiferenciasIP", i.Estado.RecDiferencias.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("AdicionalIP", i.Estado.Adicional.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("RezagosIP", i.Estado.Rezagos.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("RecargosIP", i.Estado.Recargos.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("NotificacionIP", i.Estado.Honorarios.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("EjecucionIP", i.Estado.Ejecucion.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("MultasIP", i.Estado.Multas.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("DescuentosIP", i.Estado.Descuentos.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("SubTotalIP", i.Estado.ImporteNeto.ToString("C", CultureInfo.CurrentCulture));


            fields.SetField("DescImpuestoAntIP", i.Estado.DescAntImpuesto.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("AdicionalAntIP", i.Estado.DescAntAdicional.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("DescImpuestoIP", i.Estado.DescImpuesto.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("DescDiferenciasIP", i.Estado.DescDiferencias.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("DescRecDiferenciasIP", i.Estado.DescRecDiferencias.ToString("C", CultureInfo.CurrentCulture));
           // fields.SetField("AdicionalIP", i.Estado.Adicional.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("DescRezagosIP", i.Estado.DescRezagos.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("DescRecargosIP", i.Estado.DescRecargos.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("DescNotificacionIP", i.Estado.DescHonorarios.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("DescEjecucionIP", i.Estado.DescEjecucion.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("DescMultasIP", i.Estado.DescMultas.ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("ImporteTotal", i.Estado.Importe.ToString("C", CultureInfo.CurrentCulture));

            if (rfc.RFC != null)
            {
                fields.SetField("RFCFiscal", rfc.RFC.ToString());
                fields.SetField("ColoniaFiscal", rfc.Colonia == null ? "" : rfc.Colonia.ToString());
                fields.SetField("EstadoFiscal", rfc.Estado == null ? "" : rfc.Estado.ToString());
                fields.SetField("CPFiscal", rfc.RFC == null ? "" : rfc.CodigoPostal.ToString());
                fields.SetField("CorreoFiscal", rfc.Email == null ? "" : rfc.Email.ToString());
            }

            //fields.SetField("PeriodoSM", sm.Estado.PeriodoGral);
            //fields.SetField("InfraestructuraAntSM", sm.Estado.AntInfraestructura.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("AdicionalAntSM", sm.Estado.AntAdicional.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("InfraestructuraSM", sm.Estado.Infraestructura.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("RecResiduosSM", sm.Estado.Recoleccion.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("LimpiezaSM", sm.Estado.Limpieza.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("DAPSM", sm.Estado.Dap.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("AdicionalSM", sm.Estado.Adicional.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("RezagosSM", sm.Estado.Rezagos.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("RecargosSM", sm.Estado.Recargo.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("NotificacionSM", sm.Estado.Honorarios.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("EjecucionSM", sm.Estado.Ejecucion.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("MultasSM", sm.Estado.Multa.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("DescuentosSM", sm.Estado.Descuentos.ToString("C", CultureInfo.CurrentCulture));
            //fields.SetField("SubTotalSM", sm.Estado.Importe.ToString("C", CultureInfo.CurrentCulture));

            fields.SetField("Importe Total", (i.Estado.Importe + sm.Estado.Importe).ToString("C", CultureInfo.CurrentCulture));
            fields.SetField("Elaboro", HttpUtility.HtmlDecode(U.Nombre+ " " +U.ApellidoPaterno+ " "+ U.ApellidoMaterno+ " " + DateTime.Today.ToString()));
            fields.SetField("Restriccion", HttpUtility.HtmlDecode(new cParametroSistemaBL().GetValorByClave("RESTRICCIONEDOCTA")).ToString());
            if (i.Anual != null)
            {

                i.Anual.Sort((p, q) => string.Compare(p.PeriodoGral, q.PeriodoGral));
                string col;
                int k = 5;
                int l = i.Anual.Count;
                int ej = DateTime.Now.Year - l;
                decimal valor = 0;

                foreach (ImpuestoEdo item in i.Anual)
                {
                    col = "a" + k.ToString();
                    bg = new cBaseGravableBL().GetByPredAnio(predio.Id, Convert.ToInt32(item.PeriodoGral.Substring(2, 4)));
                    if (bg == null)
                        valor = 0;
                    else
                        valor = bg.Valor;

                    fields.SetField("a" + k.ToString(), item.PeriodoGral.Substring(2, 4)); //ejercicio
                    fields.SetField("b" + k.ToString(), item.PeriodoGral.Substring(0, 1)); //bim
                    fields.SetField("g" + k.ToString(), valor.ToString("C", CultureInfo.CurrentCulture)); //base gravable
                    fields.SetField("i" + k.ToString(), (item.Impuesto).ToString("C", CultureInfo.CurrentCulture)); //impuesto
                    fields.SetField("r" + k.ToString(), (item.Recargos).ToString("C", CultureInfo.CurrentCulture)); //recargos
                    fields.SetField("e" + k.ToString(), "0"); //ejecucion
                    fields.SetField("m" + k.ToString(), "0"); //multas
                    fields.SetField("d" + k.ToString(), item.Descuentos.ToString("C", CultureInfo.CurrentCulture)); //descuentos
                    fields.SetField("t" + k.ToString(), item.Importe.ToString("C", CultureInfo.CurrentCulture)); //importe
                    k--;
                }

                valor = 0;
                decimal impuesto = 0;

                for (int j = ej; j > (DateTime.Now.Year - 5); j--)
                {
                    col = "a" + k.ToString();
                    bg = new cBaseGravableBL().GetByPredAnio(predio.Id, j);
                    if (bg == null)
                    {
                        valor = 0;
                        impuesto = 0;
                    }
                    else
                    {
                        valor = bg.Valor;
                        impuesto = CalculaImpuestoAnual(predio.Id, j, bg.Valor);
                    }
                    fields.SetField("a" + k.ToString(), j.ToString()); //ejercicio
                    fields.SetField("b" + k.ToString(), "6"); //bim
                    fields.SetField("g" + k.ToString(), valor.ToString("C")); //base gravable
                    fields.SetField("i" + k.ToString(), impuesto.ToString("C")); //impuesto
                    fields.SetField("r" + k.ToString(), "0"); //recargos
                    fields.SetField("e" + k.ToString(), "0"); //ejecucion
                    fields.SetField("m" + k.ToString(), "0"); //multas
                    fields.SetField("d" + k.ToString(), "0"); //descuentos
                    fields.SetField("t" + k.ToString(), "0"); //importe
                    k--;
                }
            }

            #endregion
            stamper.FormFlattening = true;
            stamper.Close();
        }
        protected decimal CalculaImpuestoAnual(int idPredio, int ejercicio, decimal diferencia)
        {
            decimal imp = 0;
            decimal bi = 0, bg = 0, sm = 0;
            string descto = string.Empty, error = string.Empty;
            SaldosC s = new SaldosC();
            //i = s.CalculaCobro(idPredio, "NO", 1, ejercicio, 6, ejercicio, 0, 0, "CalculaPredial");
            bi = new cBaseImpuestoBL().GetByEjercicio(ejercicio);
            //bg = new cBaseGravableBL().GetByBasePredEjercicio(idPredio,  ejercicio);
            sm = new cSalarioMinimoBL().GetSMbyEjercicio(ejercicio);
            cPredio predio = new cPredioBL().GetByConstraint(idPredio);

            imp = s.ImpuestoPorBimestreCuota(diferencia, bi, sm, predio.cTipoPredio.Id, ejercicio, ref error);

            if (error != "")
                imp = 0;
            else
                imp = imp * 6;

            return imp;
        }
        protected void Calcula()
        {
            int idPredio = Convert.ToInt32(ViewState["IdPredio"].ToString());
            cPredio predio = new cPredioBL().GetByConstraint(idPredio);

            SaldosC s = new SaldosC();
            Impuesto i = new Impuesto();
            Servicio sm = new Servicio();
            ViewState["rbtDetalladoIP"] = "NO";
            int descJyP = 0;
            int descRez = 0;

            string soloDif = "NO";
            if (rdbDiferencias.Checked == true) soloDif = "SI";

            #region calculo detallado
            if (rdbDetalladoIP.Checked == true)
            {
                List<ImpuestoBimestral> ib = new List<ImpuestoBimestral>();

                ib = s.CalculaImpuestoBimestral(idPredio, Convert.ToInt32(ddlBimIniIP.SelectedValue), Convert.ToInt32(ddlEjIniIP.SelectedValue),
                    Convert.ToInt32(ddlBimFinIP.SelectedValue), Convert.ToInt32(ddlEjFinIP.SelectedValue), predio.cTipoPredio.Id);
                if (ib.Count > 0)
                {
                    if (ib[0].TextError != null || ib[0].mensaje > 0)
                    {
                        if (ib[0].TextError != null) vtnModal.ShowPopup(ib[0].TextError, ModalPopupMensaje.TypeMesssage.Alert);
                        else vtnModal.ShowPopup(new Utileria().GetDescription(ib[0].mensaje), ModalPopupMensaje.TypeMesssage.Alert);
                        InicializaDetalle();
                        return;
                    }

                    DataTable dt = new DataTable();
                    //dt = s.ConvertToDataTable(ib);
                    dt = s.ConvertToDatatableIP(ib);
                    grd.DataSource = dt;
                    grd.DataBind();

                    ViewState["dtDetalladoIP"] = dt;
                }

                string cobrarSmEnCaja = new cParametroSistemaBL().GetValorByClave("COBROSERVICIOSCAJAS").ToString();

                if (cobrarSmEnCaja == "SI")
                {
                    string cobroDapSinConst = new cParametroSistemaBL().GetValorByClave("DAP_SINCONSTRUCCION").ToString() != "" ? new cParametroSistemaBL().GetValorByClave("DAP_SINCONSTRUCCION").ToString() : "NO";
                    List<ServicioBimestral> sb = new List<ServicioBimestral>();
                    sb = s.ServicioBimestral(idPredio, Convert.ToInt32(ddlBimIniSM.SelectedValue), Convert.ToInt32(ddlEjIniSm.SelectedValue),
                                        Convert.ToInt32(ddlBimFinSm.SelectedValue), Convert.ToInt32(ddlEjFinSM.SelectedValue), predio.Zona, predio.IdTipoPredio, cobroDapSinConst, Convert.ToDouble(predio.SuperficieConstruccion));
                    if (sb.Count > 0)
                    {
                        if (sb[0].TextError != null || sb[0].mensaje > 0)
                        {
                            if (sb[0].TextError != null) vtnModal.ShowPopup(sb[0].TextError, ModalPopupMensaje.TypeMesssage.Alert);
                            else vtnModal.ShowPopup(new Utileria().GetDescription(sb[0].mensaje), ModalPopupMensaje.TypeMesssage.Alert);
                            InicializaDetalle();
                            return;
                        }
                        DataTable dts = new DataTable();
                        //dts = s.ConvertToDataTable(sb);
                        dts = s.ConvertToDatatableSM(sb);
                        grds.DataSource = dts;
                        grds.DataBind();
                    }
                }
                divEncabezado.Visible = false;
                divEstadoCta.Visible = false;

                divDetallado.Visible = true;
                grd.Visible = true;
                return;
            }
            #endregion

            #region calculo detallado diferencias
            if (rdbDetalladoDif.Checked == true)
            {
                List<Diferencias> ib = new List<Diferencias>();
                List<cTarifaRecargo> listRecargos = new cTarifaRecargoBL().GetAll();
                ib = new cDiferenciaBL().CalculaDiferencias(idPredio, ref listRecargos);
                if (ib.Count > 0)
                {
                    DataTable dt = new DataTable();
                    //dt = s.ConvertToDataTable(ib);
                    dt = s.ConvertToDatatableDif(ib);
                    grd.DataSource = dt;
                    grd.DataBind();
                }

                divEncabezado.Visible = false;
                divEstadoCta.Visible = false;

                divDetallado.Visible = true;
                grd.Visible = true;
                return;
            }
            #endregion


            if (checkBoxJYP.Checked == true)
            {
                string clave = new cParametroSistemaBL().GetValorByClave("DescuentoJyP");
                cDescuento des = new cDescuentoBL().GetByClave(clave, DateTime.Today);
                if (des != null)
                    descJyP = des.Id;
            }

            if (txtClaveDescuento.Text != "")
            {
                cDescuento desR = new cDescuentoBL().GetByClave(txtClaveDescuento.Text, DateTime.Today);
                if (desR != null && desR.Id > 0)
                    descRez = desR.Id;
                else
                {
                    vtnModal.ShowPopup("Clave de Descuento no encontrada, revise la vigencia o el nombre de la clave", ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }
            }

            ///******IMPUESTO PREDIAL******//
            i = s.CalculaCobro(idPredio, soloDif, Convert.ToInt32(ddlBimIniIP.SelectedValue), Convert.ToInt32(ddlEjIniIP.SelectedValue),
                            Convert.ToInt32(ddlBimFinIP.SelectedValue), Convert.ToInt32(ddlEjFinIP.SelectedValue), descJyP, descRez, "CalculaPredial");
            if (i.mensaje != 0)
            {
                if (i.TextError != null)
                    vtnModal.ShowPopup(i.TextError, ModalPopupMensaje.TypeMesssage.Alert);
                else
                    vtnModal.ShowPopup(new Utileria().GetDescription(i.mensaje), ModalPopupMensaje.TypeMesssage.Alert);

                InicializaDetalle();
                return;
            }
            txtPeriodoIP.Text = i.Estado.PeriodoGral;
            txtImpuestoAnt.Text = i.Estado.AntImpuesto.ToString("N", CultureInfo.CurrentCulture);
            txtAdicionalAnt.Text = i.Estado.AntAdicional.ToString("N", CultureInfo.CurrentCulture);
            txtImpuesto.Text = i.Estado.Impuesto.ToString("N", CultureInfo.CurrentCulture);
            txtDiferencias.Text = i.Estado.Diferencias.ToString("N", CultureInfo.CurrentCulture);
            txtRecargosDif.Text = i.Estado.RecDiferencias.ToString("N", CultureInfo.CurrentCulture);

            txtRezagos.Text = i.Estado.Rezagos.ToString("N", CultureInfo.CurrentCulture);
            txtRecargos.Text = i.Estado.Recargos.ToString("N", CultureInfo.CurrentCulture);
            txtAdicional.Text = i.Estado.Adicional.ToString("N2", CultureInfo.CurrentCulture);
            txtMultas.Text = i.Multa.ToString("N2", CultureInfo.CurrentCulture);
            txtHonorarios.Text = (i.Estado.Honorarios).ToString("N2", CultureInfo.CurrentCulture);
            txtEjecucion.Text = (i.Estado.Ejecucion).ToString("N2", CultureInfo.CurrentCulture);

            txtDescImpuestoAnt.Text = (i.Estado.DescAntImpuesto).ToString("N2", CultureInfo.CurrentCulture);
            txtDescImpuesto.Text = (i.Estado.DescImpuesto).ToString("N2", CultureInfo.CurrentCulture);
            txtDescDiferencias.Text = (i.Estado.DescDiferencias).ToString("N2", CultureInfo.CurrentCulture);
            txtDescRecargoDif.Text = (i.Estado.DescRecDiferencias).ToString("N2", CultureInfo.CurrentCulture);
            txtDescRezagos.Text = (i.Estado.DescRezagos).ToString("N2", CultureInfo.CurrentCulture);
            txtDescRecargos.Text = (i.Estado.DescRecargos).ToString("N2", CultureInfo.CurrentCulture);
            txtDescHonorarios.Text =(i.Estado.DescHonorarios).ToString("N2", CultureInfo.CurrentCulture);
            txtdDescEjecucion.Text = (i.Estado.DescEjecucion).ToString("N2", CultureInfo.CurrentCulture);
            textDescMultas.Text = (i.Estado.DescMultas).ToString("N2", CultureInfo.CurrentCulture);


            txtDescuentos.Text = i.Estado.Descuentos.ToString("N", CultureInfo.CurrentCulture);
            txtTotalIp.Text = i.Estado.Importe.ToString("N2", CultureInfo.CurrentCulture);

            txtVigencia.Text = DateTime.Today.ToString("dd/MM/yyyy");

            ViewState["ImporteIp"] = i.Importe;
            ViewState["DescuentoIp"] = i.DescuentoGral;

            //if (i.Estado.Importe == 0)
            //{
            //   // rdbIP.Checked = false;
            //   // rdbSM.Checked = true;
            //}

            if (rdbDiferencias.Checked == false)
            {

                sm = s.CalculaCobroSM(idPredio, Convert.ToInt32(ddlBimIniSM.SelectedValue), Convert.ToInt32(ddlEjIniSm.SelectedValue),
                                    Convert.ToInt32(ddlBimFinSm.SelectedValue), Convert.ToInt32(ddlEjFinSM.SelectedValue), descJyP, descRez, "CalculaServicios", predio.Zona, predio.IdTipoPredio, Convert.ToDouble(predio.SuperficieConstruccion));

                if (sm.TextError != null || sm.mensaje != 0)
                {
                    if (sm.TextError == "" && sm.mensaje == 0)
                        sm = s.InicializaSM(sm);
                    else if (sm.TextError.Trim() != "")
                    {
                        vtnModal.ShowPopup(sm.TextError, ModalPopupMensaje.TypeMesssage.Alert);
                        sm = s.InicializaSM(sm);
                        sm.Estado = new ServicioEdo();
                        sm.Estado.Importe = 0;
                    }
                    else
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription(sm.mensaje), ModalPopupMensaje.TypeMesssage.Alert);
                        sm = s.InicializaSM(sm);
                        sm.Estado = new ServicioEdo();
                        sm.Estado.Importe = 0;
                    }
                }
                else
                {
                    txtPeriodoSM.Text = sm.Estado.PeriodoGral;
                    txtInfraestructuraAntSm.Text = sm.Estado.AntInfraestructura.ToString("N", CultureInfo.CurrentCulture);
                    txtAdicionalAntSm.Text = sm.Estado.AntAdicional.ToString("N", CultureInfo.CurrentCulture);

                    txtInfraestructuraSm.Text = sm.Estado.Infraestructura.ToString("N", CultureInfo.CurrentCulture);
                    txtRecoleccionSm.Text = sm.Estado.Recoleccion.ToString("N", CultureInfo.CurrentCulture);
                    txtLimpiezaSm.Text = sm.Estado.Limpieza.ToString("N", CultureInfo.CurrentCulture);
                    txtDapSm.Text = sm.Estado.Dap.ToString("N", CultureInfo.CurrentCulture);
                    txtHonorariosSm.Text = (sm.Estado.Honorarios).ToString("N", CultureInfo.CurrentCulture);
                    txtEjecucionSm.Text = (sm.Estado.Ejecucion).ToString("N", CultureInfo.CurrentCulture);
                    txtMultasSm.Text = sm.Estado.Multa.ToString("N", CultureInfo.CurrentCulture);
                    txtRecargosSm.Text = sm.Estado.Recargo.ToString("N", CultureInfo.CurrentCulture);
                    txtRezagosSm.Text = sm.Estado.Rezagos.ToString("N", CultureInfo.CurrentCulture);
                    txtAdicionalSm.Text = sm.Estado.Adicional.ToString("N", CultureInfo.CurrentCulture);

                    txtTotalSm.Text = sm.Estado.Importe.ToString("N", CultureInfo.CurrentCulture);
                    txtVigencia.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    txtDescuentosSm.Text = sm.Estado.Descuentos.ToString("N", CultureInfo.CurrentCulture);
                }
            }
            else             //if (rdbDiferencias.Checked == true)
            {
                txtInfraestructuraAntSm.Text = (0).ToString("N", CultureInfo.CurrentCulture);
                txtRecoleccionSm.Text = (0).ToString("N", CultureInfo.CurrentCulture);
                txtLimpiezaSm.Text = (0).ToString("N", CultureInfo.CurrentCulture);
                txtDapSm.Text = (0).ToString("N", CultureInfo.CurrentCulture);
                txtEjecucionSm.Text = (0).ToString("N", CultureInfo.CurrentCulture);
                txtHonorariosSm.Text = (0).ToString("N", CultureInfo.CurrentCulture);
                txtMultasSm.Text = (0).ToString("N", CultureInfo.CurrentCulture);
                txtRecargosSm.Text = (0).ToString("N", CultureInfo.CurrentCulture);
                txtRezagosSm.Text = (0).ToString("N", CultureInfo.CurrentCulture);
                txtAdicionalSm.Text = (0).ToString("N", CultureInfo.CurrentCulture);
                txtTotalSm.Text = (0).ToString("N", CultureInfo.CurrentCulture);
                txtDescuentosSm.Text = (0).ToString("N", CultureInfo.CurrentCulture);
                sm.Estado = new ServicioEdo();
                sm.Estado.Importe = 0;
                sm.Estado.Descuentos = 0;
            }


            if (DateTime.Today.Day >= 11)
            {
                decimal indice = new cIndicePrecioBL().ValorIndiceActual();
                if (indice == 0)
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "El Indice Nacional de Precios no se ha actualizado, consulta al Adminsitrador.";
                }
            }
            ViewState["ImporteSM"] = sm.Estado.Importe;
            ViewState["DescuentoSM"] = sm.Estado.Descuentos;

            txtImporte.Text = (i.Estado.Importe + sm.Estado.Importe).ToString("N", CultureInfo.CurrentCulture);

            ViewState["IP"] = i;
            ViewState["SM"] = sm;
            divEstadoCta.Visible = true;
            divEncabezado.Visible = false;

        }
        protected void ImprimeRecibo()
        {

            if (rdbIP.Checked == true)
            {
                ProcesaRecibo("IP");
                LlenaComboIP(DateTime.Today.Year);

            }
            else
            {
                ProcesaRecibo("SM");
                LlenaComboSM(DateTime.Today.Year);
            }
            apagaEtiquetas();
        }
        protected void rdbSM_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbSM = sender as RadioButton;
            if (rbSM.Checked)
            {
                rdbIP.Checked = false;
                //txtApagar.Text = txtTotalSm.Text; NELY
                pnl_Modal.Show();
            }
        }
        protected void rdbIP_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbIP = sender as RadioButton;
            if (rbIP.Checked)
            {
                rdbSM.Checked = false;
                lblImportePago.Text = txtTotalIp.Text;
                pnl_Modal.Show();
            }

        }
        protected void GeneraConvenioEstadoCta()
        {
            try
            {
                tConvenioEdoCta tcIP = new tConvenioEdoCta();
                tConvenioEdoCta tcSM = new tConvenioEdoCta();
                tConvenio c = new tConvenio();
                string ConvenioActivo = string.Empty;
                Boolean ActualizaEdoCta = false;
                int id = Convert.ToInt32(ViewState["IdPredio"]);

                //busca predio en convenios, si se encuentra como activo no se puede ingresar otro estado de cuenta
                //mensaje tiene un convenio Activo,
                tcIP = new tConvenioEdoCtaBL().GetByIdPredial(id);

                if (tcIP != null)//ya existe un estado de cuenta del mes
                {
                    //busca si el predio ya cuenta con un convenio inciado
                    c = new tConvenioBL().GetByIdConvenioEstadoCta(tcIP.Id);
                    if (c != null && c.Status == "A")
                    {
                        if (ConvenioActivo == "A")
                        {
                            vtnModal.ShowPopup(new Utileria().GetDescription("La clave Catastral ya cuenta con un Convenio"), ModalPopupMensaje.TypeMesssage.Alert);
                            return;
                        }
                    }
                    else
                    {
                        ActualizaEdoCta = true;
                    }

                    //return;
                }
                //si no esta y ya se cuenta con un estado de cuenta en tEstadoConvenio, unicamente se actualiza de lo contrario se ingresa 
                //un registro nuevo
                using (TransactionScope scope = new TransactionScope())
                {
                    cUsuarios U = new cUsuarios();
                    U = (cUsuarios)Session["usuario"];

                    tConvenioEdoCta d = new tConvenioEdoCta();
                    MensajesInterfaz msg = new MensajesInterfaz();
                    Impuesto i = new Impuesto();
                    Servicio s = new Servicio();
                    string tipoImpuesto = "IP";
                    i = (Impuesto)ViewState["IP"];
                    s = (Servicio)ViewState["SM"];

                    if (rbtnIP.Checked == true)
                    {
                        s = new Servicio();
                        s.Estado = new ServicioEdo();
                        tipoImpuesto = "IP";
                    }
                    else if (rbtnSM.Checked == true)
                    {
                        i = new Impuesto();
                        i.Estado = new ImpuestoEdo();
                        tipoImpuesto = "SM";
                    }
                    else
                    {
                        rbtnSM.Checked = true;
                        rbtnIP.Checked = true;
                    }

                    d.IdPredio = (int)ViewState["IdPredio"];
                    d.Folio = 0;
                    d.FechaEmision = DateTime.Today;
                    d.Status = "A";
                    d.TipoImpuesto = tipoImpuesto;
                    d.PeriodoIP = i.Estado.PeriodoGral != null ? i.Estado.PeriodoGral : null;
                    if (i.IdDiferencia > 0)
                        d.IdDiferencias = i.IdDiferencia;
                    if (i.idDescuentoRez > 0)
                        d.IdDescuento = i.idDescuentoRez;
                    d.IdRequerimiento = i.IdRequerimiento;
                    //IP
                    d.Impuesto = i.Estado.Impuesto > 0 ? i.Estado.Impuesto : 0;
                    d.Adicional = i.Estado.Adicional > 0 ? i.Estado.Adicional : 0;
                    d.Recargo = i.Estado.Recargos > 0 ? i.Estado.Recargos : 0;
                    d.Rezago = i.Estado.Rezagos > 0 ? i.Estado.Rezagos : 0;
                    d.Diferencia = i.Estado.Diferencias > 0 ? i.Estado.Diferencias : 0;
                    d.RecargoDiferencia = i.Estado.RecDiferencias > 0 ? i.Estado.RecDiferencias : 0;
                    d.Honorarios = i.Estado.Honorarios > 0 ? i.Estado.Honorarios : 0;
                    d.Ejecucion = i.Estado.Ejecucion > 0 ? i.Estado.Ejecucion : 0;
                    d.Multa = i.Estado.Multas > 0 ? i.Estado.Multas : 0;
                    d.DescuentoIP = i.Estado.Descuentos > 0 ? i.Estado.Descuentos : 0;
                    d.PeriodoSM = s.Estado.PeriodoGral != null ? s.Estado.PeriodoGral : null;
                    //SM
                    d.Infraestructura = s.Estado.Infraestructura > 0 ? s.Estado.Infraestructura : 0;
                    d.AdicionalSM = s.Estado.Adicional > 0 ? s.Estado.Adicional : 0;
                    d.RecargoSM = s.Estado.Recargo > 0 ? s.Estado.Recargo : 0;
                    d.RezagoSM = s.Estado.Rezagos > 0 ? s.Estado.Rezagos : 0;
                    d.Recoleccion = s.Estado.Recoleccion > 0 ? s.Estado.Recoleccion : 0;
                    d.Limpieza = s.Estado.Limpieza > 0 ? s.Estado.Limpieza : 0;
                    d.DAP = s.Estado.Dap > 0 ? s.Estado.Dap : 0;
                    d.MultaSM = s.Estado.Multa > 0 ? s.Estado.Multa : 0;
                    d.HonorariosSM = s.Estado.Honorarios > 0 ? s.Estado.Honorarios : 0;
                    d.EjecucionSM = s.Estado.Ejecucion > 0 ? s.Estado.Ejecucion : 0;
                    d.Importe = i.Estado.Importe + s.Estado.Importe;
                    d.Activo = true;
                    d.IdUsuario = U.Id;
                    d.FechaModificacion = DateTime.Now;
                    //Desc IP
                    d.DescImpuesto = i.ActDescImpuesto > 0 ? i.ActDescImpuesto : 0;
                    d.DescAdicional = (i.ActDescAdicional > 0 ? i.ActDescAdicional : 0) + (i.ActDescAdicDiferencia > 0 ? i.ActDescAdicDiferencia : 0) + (i.RezDescAdicional > 0 ? i.RezDescAdicional : 0);
                    d.DescRecargo = (i.RezDescRecargos > 0 ? i.RezDescRecargos : 0) + (i.ActDescRecargo > 0 ? i.ActDescRecargo : 0);
                    d.DescRezago = i.RezDescRezagos > 0 ? i.RezDescRezagos : 0;
                    d.DescDiferencia = (i.RezDescDiferencias > 0 ? i.RezDescDiferencias : 0) + (i.ActDescDiferencias > 0 ? i.ActDescDiferencias : 0);
                    d.DescRecargoDiferencia = (i.RezDescRecDiferencias > 0 ? i.RezDescRecDiferencias : 0) + (i.ActDescRecDiferencias > 0 ? i.ActDescRecDiferencias : 0);
                    d.DescHonorarios = i.HonorariosDesc > 0 ? i.HonorariosDesc : 0;
                    d.DescEjecucion = i.EjecucionDesc > 0 ? i.EjecucionDesc : 0;
                    d.DescMulta = i.MultaDesc > 0 ? i.MultaDesc : 0;
                    //Desc SM
                    d.DescInfraestructura = s.ActDescInfraestructura > 0 ? s.ActDescInfraestructura : 0;
                    d.DescRecoleccion = s.ActDescRecRecoleccion > 0 ? s.ActDescRecRecoleccion : 0;
                    d.DescLimpieza = s.ActDescLimpieza > 0 ? s.ActDescLimpieza : 0;
                    d.DescDAP = s.ActDescDap > 0 ? s.ActDescDap : 0;
                    d.DescAdicionalSM = (s.ActDescAdicDap > 0 ? s.ActDescAdicDap : 0) +
                                        (s.ActDescAdicInfraestructura > 0 ? s.ActDescAdicInfraestructura : 0) +
                                        (s.ActDescAdicLimpieza > 0 ? s.ActDescAdicLimpieza : 0) +
                                        (s.ActDescAdicRecoleccion > 0 ? s.ActDescAdicRecoleccion : 0) +
                                        (s.RezDescAdicional > 0 ? s.RezDescAdicional : 0);
                    d.DescRecargoSM = (s.RezDescRecargos > 0 ? s.RezDescRecargos : 0) +
                                      (s.ActDescRecDap > 0 ? s.ActDescRecDap : 0) +
                                      (s.ActDescRecInfraestructura > 0 ? s.ActDescRecInfraestructura : 0) +
                                      (s.ActDescRecLimpieza > 0 ? s.ActDescRecLimpieza : 0) +
                                      (s.ActDescRecRecoleccion > 0 ? s.ActDescRecRecoleccion : 0);
                    d.DescRezagoSM = s.RezDescRezagos > 0 ? s.RezDescRezagos : 0;
                    d.DescHonorariosSM = s.HonorariosDesc > 0 ? s.HonorariosDesc : 0;
                    d.DescEjecucionSM = s.EjecucionDesc > 0 ? s.EjecucionDesc : 0;
                    d.DescMultaSM = s.MultaDesc > 0 ? s.MultaDesc : 0;

                    if (ActualizaEdoCta == true)
                    {
                        d.Id = tcIP.Id;
                        d.Folio = tcIP.Folio;
                        msg = new tConvenioEdoCtaBL().Update(d);
                        if (msg != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    }
                    else
                    {
                        msg = new tConvenioEdoCtaBL().Insert(d);
                        if (msg != MensajesInterfaz.Ingreso)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        d.Folio = d.Id;
                        msg = new tConvenioEdoCtaBL().Update(d);
                        if (msg != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    }

                    scope.Complete();
                    InicializaDetalle();
                    divEstadoCta.Visible = false;
                    divEncabezado.Visible = true;


                    Response.Redirect("~/Convenios/Convenio.aspx?Clave= " + txtClavePredial.Text, false);
                }
            }
            catch (Exception error)
            {
                new Utileria().logError("Estado de cuenta, GeneraConvenioEstadoCta", error);
                vtnModal.ShowPopup(new Utileria().GetDescription(error.Message), ModalPopupMensaje.TypeMesssage.Alert);
            }
        }
        protected void ReiniciaControl(object sender, EventArgs e)
        {
            ViewState["idMod"] = 0;
            InicializaDetalle();
            divEncabezado.Visible = true;
            divEstadoCta.Visible = false;
            divDetallado.Visible = false;
            grd = new GridView();
            rdbCalculoCompleto.Checked = true;
            rdbDetalladoIP.Checked = false;
            rdbDiferencias.Checked = false;
            txtClaveDescuento.Text = "";
            rbtnAM.Checked = false;
            rdbIP.Checked = true;
            rdbSM.Checked = false;
            checkBoxJYP.Checked = false;
            lblDescripcion.Text = "";
            //lblPrincipal.Text = "Impuesto Predial";
            lbl_titulo.Text = "";
            InicializaEncabezado();
        }
        protected void apagaEtiquetas()
        {
            //    lblEfectivoError.Visible = false;
            //    lblEfectivo.Visible = false;
            //    txtEfectivo.Visible = false;
            //    txtEfectivo.Text = "";
            //    lblCambioValor.Visible = false;
            //    lblTarjeta.Visible = false;
            //    txtTarjeta.Visible = false;
            //    txtTarjeta.Text = "";
            //    lblCheque.Visible = false;
            //    txtCheque.Visible = false;
            //    txtCheque.Text = "";
            //    txtNoAprobacion.Visible = false;
            //    txtNoAprobacion.Text = "";
            //    lblNoAprobacion.Visible = false;
            //    btnAceptarEfectivo.Visible = false;
            //    btnAceptarCheque.Visible = false;
            //    btnAceptarTarjeta.Visible = false;
            //    btnImprimeRecibo.Visible = false;
            //    lblEfectivoError.Visible = false;
            //    txtObservaciones.Text = "";
        }

        #region Cobrar
        protected void llenaTipoPago()
        {
            ddlMetodoPago.Items.Clear();
            ddlMetodoPago.DataValueField = "Clave";
            ddlMetodoPago.DataTextField = "Descripcion";
            ddlMetodoPago.DataSource = new cTipoPagoBL().GetAll();
            ddlMetodoPago.DataBind();
            ddlMetodoPago.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecciona Método", ""));

        }
        protected void btnCancelarCobro_Click(object sender, EventArgs e)
        {
            lblCambio.Text = "Cambio: ";
            txtNumeroAprobacion.Text = "";
            grdAlta.DataSource = null;
            grdAlta.DataBind();
            grdAlta.Visible = false;
            pnl_Modal.Hide();
        }
        protected void btnAceptarPago_Click(object sender, EventArgs e)
        {
            //valida importe de depposito vs importe total
            Decimal total = 0;
            listConcepto = LeerGrid("", "", ref total);


            if (total != Convert.ToDecimal(txtImporte.Text))
                vtnModal.ShowPopup("La suma de pagos no es igual al importe del Impuesto. Diferencia: " + (Convert.ToDecimal(txtImporte.Text) - total).ToString() + ".", ModalPopupMensaje.TypeMesssage.Error);
            else
                ProcesaRecibo("IP");
        }

        //protected void btnCobrar_Click(object sender, EventArgs e)
        //{
        //    //btnCobrar.Enabled = false;
        //    txtObservacion.Visible = true;
        //    txtObservacion.Text = "";
        //    lblImportePago.Text = "Importe: " + lblTotal.Text;
        //    //llenaTipoPago(); //NELY
        //    activaEtiquetasModal("99", true);
        //    txtObservacion.Text = ViewState["observaciones"] == null ? "" : ViewState["observaciones"].ToString();
        //}

        protected void btnAceptarCobro_Click(object sender, EventArgs e)
        {
            // ingresar el metodo Nely
            //llenaGridConceptos(Convert.ToInt32(ViewState["idConcepto"].ToString()));//, Convert.ToDecimal(txtTipoCobroSalario.Text));
            //txtTipoCobroSalario.Text = "";
            pnl_Modal.Hide();
        }
        protected void ddlMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (txtMonto.Text == "")
            //    vtnModal.ShowPopup(new Utileria().GetDescription("Seleccionar método de pago"), Controles.ModalPopupMensaje.TypeMesssage.Alert);

            switch (ddlMetodoPago.SelectedValue)
            {
                case "0": break;
                case "01"://Efectivo
                    //validadorEfectivo.MinimumValue = Convert.ToString(ViewState["ImportePagado"]);
                    //validadorEfectivo.ErrorMessage = "Ingresar solo números mayor o igual a " + lblTotal.Text + " y menor a $99,999,999.99";
                    //lblImportePago.Text = "Importe: " + lblTotal.Text;
                    activaEtiquetasModal("01", true);
                    break;
                case "02": //Cheque
                    lblNumeroAprobacion.Text = "No. Cheque:";
                    activaEtiquetasModal("02", true);
                    break;
                case "03": //Transferencia
                    lblNumeroAprobacion.Text = "No. Tansferencia:";
                    activaEtiquetasModal("03", true);
                    break;
                case "04"://Tarjeta de credito
                    lblNumeroAprobacion.Text = "Tarjeta crédito:";
                    activaEtiquetasModal("04", true);
                    break;
                case "28"://Tarjeta de Debito
                    lblNumeroAprobacion.Text = "Tarjeta débito";
                    activaEtiquetasModal("28", true);
                    break;
                default:
                    vtnModal.ShowPopup(new Utileria().GetDescription("El método de pago no se encuentra habilitado"), Controles.ModalPopupMensaje.TypeMesssage.Alert);
                    break;
            }
            btnAgregarConcepto.Visible = true;
            btnAgregarConcepto.Text = "Agregar Concepto";
        }
        private void activaEtiquetasModal(string tipo, bool visible)
        {
            lblTituloConceptoId.Visible = true;
            lblImportePago.Visible = !visible;
            lblMetodoPago.Visible = visible;
            ddlMetodoPago.Visible = visible;

            btnCancelarCobro.Visible = true;
            btnAceptarCobro.Visible = !visible; //en cero
            btnAceptarPago.Visible = !visible;

            lblNumeroAprobacion.Visible = !visible;
            txtNumeroAprobacion.Visible = !visible;
            lblInstitucion.Visible = !visible;
            txtInstitucion.Visible = !visible;
            lblObservacion.Visible = !visible;
            txtObservacion.Visible = !visible;
            lblMonto.Visible = !visible;
            txtMonto.Visible = !visible;
            lblTituloConceptoId.Text = "Pago";
            lblCambio.Visible = !visible;
            btnAgregarConcepto.Visible = !visible;
            txtMonto.Text = "";
            txtNumeroAprobacion.Text = "";
            txtInstitucion.Text = "";

            switch (tipo)
            {
                case "0": //Para el caso de Agregar El cobro
                    lblMetodoPago.Visible = visible;
                    ddlMetodoPago.Visible = visible;
                    lblTituloConceptoId.Text = "Cobros";
                    //lblTipoCobro.Visible = visible;
                    //txtTipoCobroSalario.Visible = visible;
                    btnCancelarCobro.Visible = true;
                    btnAceptarCobro.Visible = visible;

                    //lblCambio.Visible = true;
                    break;
                case "01": //Efectivo
                    lblImportePago.Visible = visible;
                    lblMonto.Visible = visible;
                    txtMonto.Visible = visible;
                    //lblNumeroAprobacion.Visible = visible;
                    //txtNumeroAprobacion.Visible = visible;
                    //lblInstitucion.Visible = visible;
                    //txtInstitucion.Visible = visible;
                    lblObservacion.Visible = visible;
                    txtObservacion.Visible = visible;
                    btnAgregarConcepto.Visible = visible;
                    btnAceptarPago.Visible = true;
                    break;

                case "02": //Cheque   
                case "03": //Transferencia
                case "04": //Tarjeta credito
                case "28": //Tarjeta debito
                    lblImportePago.Visible = visible;
                    //lblCambio.Visible = visible;      
                    lblMonto.Visible = visible;
                    txtMonto.Visible = visible;
                    lblNumeroAprobacion.Visible = visible;
                    txtNumeroAprobacion.Visible = visible;
                    lblInstitucion.Visible = visible;
                    txtInstitucion.Visible = visible;
                    lblObservacion.Visible = visible;
                    txtObservacion.Visible = visible;
                    btnAgregarConcepto.Visible = visible;
                    btnAceptarPago.Visible = true;
                    break;
                default: break;
            }
            pnl_Modal.Show();

        }
        protected void grdAlta_Sorting(object sender, GridViewSortEventArgs e)
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

        }
        protected void grdAlta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = grdAlta.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string clave = grdAlta.DataKeys[e.Row.RowIndex].Values[1].ToString();

                ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                imgDelete.Visible = true;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                // e.Row.Cells[0].Text = "Total";
                //e.Row.Cells[0].Text = subtotal.ToString("C")
                Label lbl = (Label)e.Row.FindControl("lblSubtotal");
                //subt = (Convert.ToDecimal(ViewState["SubtotalMetodo"])).ToString("N2");
                lblCambio.Text = (Convert.ToDecimal(ViewState["SubtotalMetodo"])).ToString("N2");
                //lbl.Text = lblCambio.Text;
            }

        }
        protected void grdAlta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            decimal total = 0;
            if (e.CommandName == "EliminarRegistro")
            {
                string clave = e.CommandArgument.ToString();

                if (grdAlta.Rows.Count >= 1)
                {
                    listConcepto = LeerGrid("D", clave, ref total);
                    grdAlta.DataSource = listConcepto;
                    grdAlta.DataBind();
                }
                else
                    grdAlta.DeleteRow(grdAlta.SelectedIndex);
            }
            else if (e.CommandName == "ActivarRegistro")
            {
                string id = e.CommandArgument.ToString();
                // eliminarActivarAltas(true, id);
            }
            else if (e.CommandName == "EditarImporte")
            {
                String id = e.CommandArgument.ToString();
                if (id.Substring(0, 1) == "N")
                {
                    ViewState["idConcepto"] = ViewState["idConceptoN"];
                    ViewState["idTD"] = id;
                    btnAgregarConcepto_Click(null, null);
                }
                else
                {
                    tTramite t = new tTramiteBL().GetByConstraint(Convert.ToInt32(id));
                    ViewState["idT"] = t.Id;
                }
            }
            pnl_Modal.Show();
        }
        protected void btnAgregarConcepto_Click(object sender, EventArgs e)
        {
            cTipoPago cTipo = new cTipoPago();
            MetodoGrid lAux = new MetodoGrid();
            //sacar los metodos de pago anteriores
            ViewState["SubtotalMetodo"] = "";
            decimal subtotalMetodo = 0;

            foreach (GridViewRow gvr in grdAlta.Rows)
            {
                lAux = new MetodoGrid();
                Label Id = gvr.FindControl("lblId") as Label;
                Label Clave = gvr.FindControl("lblClave") as Label;
                Label TipoCobro = gvr.FindControl("lblTipoCobro") as Label;
                Label Importe = gvr.FindControl("lblImporte") as Label;
                Label Aprobacion = gvr.FindControl("lblAprobacion") as Label;
                Label Institucion = gvr.FindControl("lblInstitucion") as Label;

                cTipo = new cTipoPagoBL().GetByConstraint(Convert.ToInt16(Id.Text));
                lAux.Id = cTipo.Id;
                lAux.Clave = Clave.Text;
                lAux.TipoCobro = Clave.Text + " " + cTipo.Descripcion;
                lAux.Importe = Convert.ToDecimal(Importe.Text);
                lAux.Transaccion = Aprobacion.Text;
                lAux.Institucion = Institucion.Text;
                listConcepto.Add(lAux);

                subtotalMetodo = subtotalMetodo + lAux.Importe;
                ViewState["SubtotalMetodo"] = subtotalMetodo.ToString();
            }

            if (Convert.ToDecimal(txtImporte.Text) > 0)
            {
                lAux = new MetodoGrid();
                lAux.Id = new cTipoPagoBL().GetByClave(ddlMetodoPago.SelectedValue);
                cTipo = new cTipoPagoBL().GetByConstraint(lAux.Id);
                lAux.Clave = ddlMetodoPago.SelectedValue;
                lAux.TipoCobro = cTipo.Clave + " " + cTipo.Descripcion;
                lAux.Importe = Convert.ToDecimal(txtMonto.Text);
                lAux.Transaccion = txtNumeroAprobacion.Text == "" ? "0" : txtNumeroAprobacion.Text;
                lAux.Institucion = txtInstitucion.Text == "" ? "Efectivo" : txtInstitucion.Text;
                listConcepto.Add(lAux);
                subtotalMetodo = subtotalMetodo + lAux.Importe;
                ViewState["SubtotalMetodo"] = subtotalMetodo.ToString();
                grdAlta.DataSource = listConcepto;
                grdAlta.DataBind();
                grdAlta.Visible = true;
                pnl_Modal.Show();
            }

            txtMonto.Text = "";
            txtNumeroAprobacion.Text = "";
            txtInstitucion.Text = "";
            lblMonto.Visible = false;
            txtMonto.Visible = false;
            ddlMetodoPago.SelectedIndex = 0;
            //btnAceptarPago.Visible = true;
            lblNumeroAprobacion.Visible = false;
            txtNumeroAprobacion.Text = "";
            txtNumeroAprobacion.Visible = false;
            lblInstitucion.Visible = false;
            txtInstitucion.Text = "";
            txtInstitucion.Visible = false;
        }
        public partial class MetodoGrid
        {
            public int Id { get; set; }
            public string Clave { get; set; }
            public string TipoCobro { get; set; }
            public decimal Importe { get; set; }
            public string Transaccion { get; set; }
            public string Institucion { get; set; }
        }
        private List<MetodoGrid> LeerGrid(string mov, string valor, ref decimal sumatoria)
        {
            List<MetodoGrid> lis = new List<MetodoGrid>();
            MetodoGrid lAux = new MetodoGrid();
            foreach (GridViewRow gvr in grdAlta.Rows)
            {
                lAux = new MetodoGrid();
                Label Id = gvr.FindControl("lblId") as Label;
                Label Clave = gvr.FindControl("lblClave") as Label;
                Label TipoCobro = gvr.FindControl("lblTipoCobro") as Label;
                Label Importe = gvr.FindControl("lblImporte") as Label;
                Label Aprobacion = gvr.FindControl("lblAprobacion") as Label;
                Label Institucion = gvr.FindControl("lblInstitucion") as Label;

                cTipoPago cTipo = new cTipoPagoBL().GetByConstraint(Convert.ToInt16(Id.Text));
                lAux.Id = cTipo.Id;
                lAux.Clave = Clave.Text;
                lAux.TipoCobro = Clave.Text + " " + cTipo.Descripcion;
                lAux.Importe = Convert.ToDecimal(Importe.Text);
                lAux.Transaccion = Aprobacion.Text;
                lAux.Institucion = Institucion.Text;



                if (mov == "D" && lAux.Id.ToString() == valor) //D Eliminar de la lista
                    lAux = null;
                else
                {
                    sumatoria = sumatoria + Convert.ToDecimal(Importe.Text);
                    lis.Add(lAux);
                }
            }

            return lis;
        }
        private ResultDAO enviarCorreo(string reciboPath, string xmlPath, string selloDigital, string correo, string recibo)
        {
            //creamos nuestra lista de archivos a enviar
            List<string> archivos = new List<string>();
            archivos.Add(reciboPath);
            archivos.Add(xmlPath);
            archivos.Add(selloDigital);

            ResultDAO resultEMAIL = (new Mail()).enviarHotmail(
                            correo,
                            "Factura de pago del servicio del agua potable.\n  Número de recibo: " + recibo,
                            "Factura",
                            archivos);


            return resultEMAIL;
        }
        public string Letras(string numero, int tipo)
        {
            GeneraRecibo.NumLetras l = new GeneraRecibo.NumLetras();

            if (tipo == 1) //Que es para lo metros
            {
                l.ConvertirDecimales = true;
                l.ApocoparUnoParteDecimal = true;
                l.SeparadorDecimalSalida = "punto";
                l.ApocoparUnoParteEntera = true;

                return l.ToCustomCardinal(Convert.ToDouble(numero)) + " mts cuadrados";
            }
            else // Para los pesos
            {
                //al uso en México (creo):
                l.MascaraSalidaDecimal = "00/100 M.N.";
                l.SeparadorDecimalSalida = "pesos";
                l.ApocoparUnoParteEntera = true;

                return l.ToCustomCardinal(Convert.ToDouble(numero));

            }
        }
        //Convierte a decimales Facturacion Nueva
        private string formatearDecimales(string value)
        {
            if (value.Contains("."))
            {
                string inicialPart = value.Split('.')[0];
                string decimalPart = value.Split('.')[1];
                if (decimalPart.Length > 1)
                {
                    return inicialPart + "." + decimalPart.Substring(0, 2);
                }
                else if (decimalPart.Length == 1)
                {
                    return inicialPart + "." + decimalPart + "0";
                }
                else
                {
                    return inicialPart + ".00";
                }
            }
            return value + ".00";
        }

        #endregion

        #region Descuentos de rezagos
        protected void imgGuardarDescto_Click(object sender, ImageClickEventArgs e)
        {
            cUsuarios U = new cUsuarios();
            MensajesInterfaz msg = new MensajesInterfaz();
            DateTime vigencia = DateTime.Now;

            U = (cUsuarios)Session["usuario"];
            int idPredio = Convert.ToInt32(ViewState["IdPredio"].ToString());

            if (DateTime.Today.Month != 12)
                vigencia = new DateTime(DateTime.Today.Year, DateTime.Today.Month + 1, 1);
            else
                vigencia = new DateTime(DateTime.Today.Year + 1, DateTime.Today.Month - 11, 1);
            vigencia = vigencia.AddDays(-1);

            try
            {
                //msg = new tDescuentoAsignadoBL().CancelaVigentesPorIdPredio(idPredio);
                //if (msg != MensajesInterfaz.Actualizacion)
                //    throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");


                cDescuento descto = new cDescuentoBL().GetByClave(txtClaveDescuento.Text, DateTime.Today);
                if (descto != null && descto.Clave != "")
                {
                    tDescuentoAsignado d = new tDescuentoAsignado();

                    d.IdDescuento = descto.Id;
                    d.IdPredio = idPredio;
                    d.Vigencia = vigencia;
                    d.Estado = "ASIGNADO";
                    d.Activo = true;
                    d.IdUsuario = U.Id;
                    d.FechaModificacion = DateTime.Today;
                    msg = new tDescuentoAsignadoBL().Insert(d);
                    if (msg != MensajesInterfaz.Ingreso)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    InicializaDetalle();
                    divEstadoCta.Visible = false;
                    divEncabezado.Visible = true;
                    txtClaveDescuento.Enabled = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription("Clave de descuento asignada al predio "), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            catch (Exception error)
            {
                new Utileria().logError("ImgGuardarDescuentos", error, " -- Parametros:" + idPredio.ToString());
                vtnModal.ShowPopup(new Utileria().GetDescription(error.Message), ModalPopupMensaje.TypeMesssage.Alert);
            }
        }
        protected void imgEliminarDescto_Click(object sender, ImageClickEventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            int idPredio = Convert.ToInt32(ViewState["IdPredio"].ToString());
            MensajesInterfaz msg = new MensajesInterfaz();

            try
            {
                tDescuentoAsignado descto = new tDescuentoAsignadoBL().GetDescuentoByIdPredio(Convert.ToInt32(ViewState["IdPredio"].ToString()));
                if (descto != null && descto.IdPredio > 0)
                {
                    //descto.Estado = "ELIMINADO";
                    //descto.Activo = false;
                    //descto.IdUsuario = U.Id;
                    //descto.FechaModificacion = DateTime.Today;

                    //msg = new tDescuentoAsignadoBL().Update(descto);                    
                    //if (msg != MensajesInterfaz.Actualizacion)
                    //    throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    msg = new tDescuentoAsignadoBL().CancelaVigentesPorIdPredio(idPredio, U.Id);
                    if (msg != MensajesInterfaz.Actualizacion)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                    InicializaDetalle();
                    divEstadoCta.Visible = false;
                    divEncabezado.Visible = true;
                    vtnModal.ShowPopup(new Utileria().GetDescription("Clave de descuento deshabilitada del predio "), ModalPopupMensaje.TypeMesssage.Alert);
                    txtClaveDescuento.Text = "";
                    txtClaveDescuento.Enabled = true;
                }

            }
            catch (Exception error)
            {
                new Utileria().logError("imgEliminarDescto_Click", error, " -- Parametros:" + idPredio.ToString());
                vtnModal.ShowPopup(new Utileria().GetDescription(error.Message), ModalPopupMensaje.TypeMesssage.Alert);
            }
        }

        #endregion 

        #region Factura
        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            modalFactura.Show();
        }
        protected void btnBuscarRFC_Click(object sender, EventArgs e)
        {
            lblValidaRFC.Visible = false;
            lblValidaRFC.Text = "";
            char[] caracteres = txtRFCbuscar.Text.ToCharArray();

            if (caracteres.Length >= 12 && caracteres.Length <= 13)
            {
                int bandera = 0;
                if (caracteres.Length == 12)
                {
                    for (int i = 0; i < caracteres.Length && bandera == 0; i++)
                    {

                        ///Validar primeras 3 letras
                        if (i < 3)
                        {
                            if (!Char.IsLetter(caracteres[i]) && caracteres[i] != '&')
                            {
                                bandera = 1;
                                break;
                            }

                        }
                        ///Validar Fecha
                        else
                        {
                            if (i < 9)
                            {
                                DateTime fecha;
                                string f = caracteres[i].ToString() + caracteres[(i + 1)].ToString() + "/" + caracteres[(i + 2)].ToString() + caracteres[(i + 3)].ToString() + "/" + caracteres[(i + 4)].ToString() + caracteres[(i + 5)].ToString();
                                i = i + 5;
                                if (!DateTime.TryParseExact(f, "yy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                                {
                                    bandera = 1;
                                    break;
                                }
                            }
                            else
                            {
                                if (!Char.IsLetterOrDigit(caracteres[i]))
                                {
                                    bandera = 1;
                                    break;
                                }
                            }
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < caracteres.Length && bandera == 0; i++)
                    {

                        ///Validar primeras 4 letras
                        if (i < 4)
                        {
                            if (!Char.IsLetter(caracteres[i]) && caracteres[i] != '&')
                            {
                                bandera = 1;
                                break;
                            }

                        }
                        ///Validar Fecha
                        else
                        {
                            if (i < 10)
                            {
                                DateTime fecha;
                                string f = caracteres[i].ToString() + caracteres[(i + 1)].ToString() + "/" + caracteres[(i + 2)].ToString() + caracteres[(i + 3)].ToString() + "/" + caracteres[(i + 4)].ToString() + caracteres[(i + 5)].ToString();
                                i = i + 5;
                                if (!DateTime.TryParseExact(f, "yy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                                {
                                    bandera = 1;
                                    break;
                                }
                            }
                            else
                            {
                                if (!Char.IsLetterOrDigit(caracteres[i]))
                                {
                                    bandera = 1;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (bandera == 0)
                {

                    cRFC rec = new cRfcBL().GetByRfc(txtRFCbuscar.Text);
                    lblMensaje.Visible = false;
                    if (rec != null)
                    {
                        btnEditar.Visible = true;
                        ViewState["RFC"] = rec.RFC;
                        InformacionRFC.Visible = true;
                        txtRFC.Text = rec.RFC;
                        txtRFC.Enabled = false;
                        //txtCalle.Text = rec.Calle;
                        //txtEstado.Text = rec.Estado;
                        //txtCP.Text = rec.CodigoPostal;
                        //txtNoExt.Text = rec.NoExterior;
                        //txtNoInt.Text = rec.NoInterior;
                        txtNombre.Text = rec.Nombre;
                        //txtMunicipio.Text = rec.Municipio;
                        //txtPais.Text = rec.Pais;
                        //txtColonia.Text = rec.Colonia;
                        //txtLocalidad.Text = rec.Localidad;
                        //txtReferencia.Text = rec.Referencia;
                        txtCorreoReg.Text = rec.Email;
                        btnGeneraFactura.Visible = true;
                        btnGuardar.Visible = false;
                        btnRFCRegistro.Visible = false;
                        if (rec.RFC.Length == 13)
                        {
                            ddlUsuCFDI.DataSource = new cUsoCFDIBL().GetByTipoPersona("FISICA");
                            ddlUsuCFDI.DataValueField = "Clave";
                            ddlUsuCFDI.DataTextField = "Descripcion";
                            ddlUsuCFDI.DataBind();
                            ddlUsuCFDI.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Uso CFDI", ""));
                        }
                        else
                        {
                            ddlUsuCFDI.DataSource = new cUsoCFDIBL().GetByTipoPersona("MORAL");
                            ddlUsuCFDI.DataValueField = "Clave";
                            ddlUsuCFDI.DataTextField = "Descripcion";
                            ddlUsuCFDI.DataBind();
                            ddlUsuCFDI.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Uso CFDI", ""));
                        }
                    }
                    else
                    {
                        lblMensaje.Visible = true;
                        btnRFCRegistro.Visible = true;
                        activaTxt(true);
                        txtRFC.Text = txtRFCbuscar.Text;
                        ViewState["RFC"] = string.Empty;
                        btnEditar.Visible = false;
                        btnGuardar.Visible = false;
                        InformacionRFC.Visible = false;
                    }
                    //btnBuscarRFC.Visible = false;                    
                    activaTxt(false);
                }
                else
                {
                    lblValidaRFC.Visible = true;
                    lblValidaRFC.Text = "ESTRUCTURA DE RFC ERRONEA.";
                }
            }
            else
            {
                lblValidaRFC.Visible = true;
                lblValidaRFC.Text = "EL RFC DEBE CONTENER 12 O 13 CARACTERES VALIDOS.";
            }
            modalFactura.Show();
        }
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            btnGuardar.Visible = true;
            btnGeneraFactura.Visible = false;
            btnEditar.Visible = false;
            activaTxt(true);
            txtRFC.Enabled = false;

            modalFactura.Show();
        }
        private void activaTxt(bool val)
        {
            txtRFC.Enabled = val;
            //txtCalle.Enabled = val;
            //txtEstado.Enabled = val;
            //txtCP.Enabled = val;
            //txtNoExt.Enabled = val;
            //txtNoInt.Enabled = val;
            txtNombre.Enabled = val;
            //txtMunicipio.Enabled = val;
            //txtPais.Enabled = val;
            //txtColonia.Enabled = val;
            //txtLocalidad.Enabled = val;
            //txtReferencia.Enabled = val;
            txtCorreoReg.Enabled = val;

        }
        protected void btnRFCRegistro_Click(object sender, EventArgs e)
        {
            lblMensaje.Visible = false;
            activaTxt(true);
            txtRFC.Enabled = false;
            InformacionRFC.Visible = true;
            btnGeneraFactura.Visible = false;
            btnGuardar.Visible = true;
            btnRFCRegistro.Visible = false;

            txtNombre.Text = "";
            //txtCalle.Text = "";
            //txtMunicipio.Text = "";
            //txtEstado.Text = "";
            //txtCP.Text = "";
            //txtColonia.Text = "";
            //txtNoExt.Text = "";
            //txtLocalidad.Text = "";
            //txtNoInt.Text = "";
            //txtReferencia.Text = "";
            txtCorreoReg.Text = "";

            modalFactura.Show();
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string respuesta = new Utileria().validaRFC(txtRFC.Text.ToUpper().Trim());
            if (respuesta == "CORRECTO")
            {
                cRFC rec;
                string op = "";
                if (ViewState["RFC"].ToString() != string.Empty)
                {
                    rec = new cRfcBL().GetByRfc((string)ViewState["RFC"]);
                    op = "Edicion";
                    ViewState["RFC"] = string.Empty;
                }
                else
                {
                    rec = new cRFC();
                    op = "Insercion";
                    ViewState["RFC"] = string.Empty;
                }
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];

                rec.RFC = txtRFC.Text;
                //rec.Calle = txtCalle.Text;
                //rec.Estado = txtEstado.Text;
                //rec.CodigoPostal = txtCP.Text;
                //rec.NoExterior = txtNoExt.Text;
                //rec.NoInterior = txtNoInt.Text;
                rec.Nombre = txtNombre.Text;
                //rec.Municipio = txtMunicipio.Text;
                //rec.Pais = txtPais.Text;
                //rec.Colonia = txtColonia.Text;
                //rec.Localidad = txtLocalidad.Text;
                //rec.Referencia = txtReferencia.Text;
                rec.Email = txtCorreoReg.Text;
                rec.Activo = true;
                rec.IdUsuario = U.Id;
                rec.FechaModificacion = DateTime.Now;

                if (op == "Insercion")
                {
                    MensajesInterfaz msg = new cRfcBL().Insert(rec);
                    if (msg == MensajesInterfaz.IngresoRFC)
                    {
                        ViewState["RFC"] = rec.RFC;
                        btnGeneraFactura.Visible = true;
                        btnEditar.Visible = true;
                        activaTxt(false);
                        btnGuardar.Visible = false;
                        //modalRecibo.Show();
                        modalFactura.Show();
                        if (rec.RFC.Length == 13)
                        {
                            ddlUsuCFDI.DataSource = new cUsoCFDIBL().GetByTipoPersona("FISICA");
                            ddlUsuCFDI.DataValueField = "Clave";
                            ddlUsuCFDI.DataTextField = "Descripcion";
                            ddlUsuCFDI.DataBind();
                            ddlUsuCFDI.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Uso CFDI", ""));
                        }
                        else
                        {
                            ddlUsuCFDI.DataSource = new cUsoCFDIBL().GetByTipoPersona("MORAL");
                            ddlUsuCFDI.DataValueField = "Clave";
                            ddlUsuCFDI.DataTextField = "Descripcion";
                            ddlUsuCFDI.DataBind();
                            ddlUsuCFDI.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Uso CFDI", ""));
                        }
                    }
                    else
                    {
                        modalFactura.Hide();
                        vtnModal.DysplayCancelar = false;
                        vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                }
                else
                {
                    MensajesInterfaz msg = new cRfcBL().Update(rec);
                    if (msg == MensajesInterfaz.ActualizacionRFC)
                    {
                        ViewState["RFC"] = rec.RFC;
                        btnGeneraFactura.Visible = true;
                        btnEditar.Visible = true;
                        activaTxt(false);
                        btnGuardar.Visible = false;
                        //modalRecibo.Show();
                        modalFactura.Show();
                        if (rec.RFC.Length == 13)
                        {
                            ddlUsuCFDI.DataSource = new cUsoCFDIBL().GetByTipoPersona("FISICA");
                            ddlUsuCFDI.DataValueField = "Clave";
                            ddlUsuCFDI.DataTextField = "Descripcion";
                            ddlUsuCFDI.DataBind();
                            ddlUsuCFDI.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Uso CFDI", ""));
                        }
                        else
                        {
                            ddlUsuCFDI.DataSource = new cUsoCFDIBL().GetByTipoPersona("MORAL");
                            ddlUsuCFDI.DataValueField = "Clave";
                            ddlUsuCFDI.DataTextField = "Descripcion";
                            ddlUsuCFDI.DataBind();
                            ddlUsuCFDI.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Uso CFDI", ""));
                        }
                    }
                    else
                    {
                        modalFactura.Hide();
                        vtnModal.DysplayCancelar = false;
                        vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                }
            }
            else
            {
                lblValidaRFC.Text = respuesta;
                lblValidaRFC.Visible = true;
                modalRecibo.Hide();
                modalFactura.Show();
            }
        }
        protected void btnGeneraFactura_Click(object sender, EventArgs e)
        {
            string RFC = ViewState["RFC"].ToString();
            int IdRecibo = (int)ViewState["IdRecibo"];
            string usuarioFactura = ConfigurationManager.AppSettings["usuarioFactura"].ToString();
            string passwordFactura = ConfigurationManager.AppSettings["passwordFactura"].ToString();
            bool productivoFactura = Convert.ToBoolean(ConfigurationManager.AppSettings["productivoFactura"]);

            ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
            Factura fact = reciboCFDI.generaFacturaRecibo(RFC, IdRecibo, usuarioFactura, passwordFactura, productivoFactura, ddlUsuCFDI.SelectedItem.Value);

            tRecibo recibo = new tReciboBL().GetByConstraint(IdRecibo);
            recibo.RutaFactura = fact.Ruta;
            recibo.Rfc = fact.Rfc;
            recibo.FechaFactura = fact.FechaFactura;
            recibo.Facturado = true;

            MensajesInterfaz msg = new tReciboBL().Update(recibo);
            if (msg == MensajesInterfaz.Actualizacion)
            {
                String path = Server.MapPath("~/");

                string urlpath = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
                FileStream newFile = new FileStream(path + "/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf", FileMode.Create);
                newFile.Write(fact.pdfBytes, 0, fact.pdfBytes.Length);
                newFile.Close();
                frameRecibo.Src = "~/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf";
                modalRecibo.Show();
                String Clientscript = "printPdf();";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", Clientscript, true);

                cRFC rfcObj = new cRfcBL().GetByRfc(RFC);
                if (rfcObj.RFC.Length > 1)
                {
                    byte[] arrayXml = Encoding.UTF8.GetBytes(fact.xml);
                    MensajesInterfaz correo = new Utileria().sendEMail(rfcObj.Email, "SERVICIO DE ENTREGA DE FACTURA.", "SERVICIO DE ENTREGA DE FACTURA. <br> Favor de no contestar este correo, debido a que es un servicio automatico.", new MemoryStream(arrayXml), new MemoryStream(fact.pdfBytes));
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription(correo), ModalPopupMensaje.TypeMesssage.Confirm);
                }
                else
                {
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FacturaCorreoError), ModalPopupMensaje.TypeMesssage.Confirm);
                }

                if (recibo.Facturado)
                {
                    //tbFacturar.Visible = false;
                    //divCerrarFactura.Visible = true;
                }
                else
                {
                    //tbFacturar.Visible = true;
                    //divCerrarFactura.Visible = false;
                }
            }
        }
        protected void btnCancelarTodo_Click(object sender, EventArgs e)
        {
            modalRecibo.Show();
            //Response.Redirect("~/Servicios/EstadoDeCuenta.aspx", false);
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            modalRecibo.Hide();
            btnCalcular.Visible = false;
            divEstadoCta.Visible = false;
            divDetallado.Visible = false;
            InicializaEncabezado();
            llenaCombos();
            InicializaDetalle();
            ReiniciaControl(null, null);
            txtClavePredial.Focus();

        }
        protected void btnDescargaXML_Click(object sender, EventArgs e)
        {
            int IdRecibo = Convert.ToInt32(ViewState["IdRecibo"]);
            tRecibo recibo = new tReciboBL().GetByConstraint(Convert.ToInt32(IdRecibo));
            string path = ConfigurationManager.AppSettings["RutaRecibos"];//Server.MapPath("~/");
            byte[] arrayXml = File.ReadAllBytes(path + recibo.RutaFactura);
            Response.Clear();
            MemoryStream ms = new MemoryStream(arrayXml);
            Response.ContentType = "text/xml";
            Response.AddHeader("content-disposition", "attachment;filename=Factura.xml");
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();
        }
        protected void btnDescargaPDF_Click(object sender, EventArgs e)
        {
            int IdRecibo = Convert.ToInt32(ViewState["IdRecibo"]);
            tRecibo recibo = new tReciboBL().GetByConstraint(Convert.ToInt32(IdRecibo));
            string FechaFact33 = ConfigurationManager.AppSettings["FechaFact33"].ToString();
            string path = Server.MapPath("~/");
            ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
            GeneraRecibo33.Recibo RF = reciboCFDI.consultaFacturaPDF(recibo, recibo.IdFIEL, path);
            Response.Clear();
            MemoryStream msPdf = new MemoryStream(RF.pdfBytes);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Factura.pdf");
            Response.Buffer = true;
            msPdf.WriteTo(Response.OutputStream);
            Response.End();
        }
        protected void btnCorreo_Click(object sender, EventArgs e)
        {
            ModalEnvioCorreo.Show();
        }
        protected void btnEnvioCorreo_Click(object sender, EventArgs e)
        {
            int IdRecibo = Convert.ToInt32(ViewState["IdRecibo"]);
            tRecibo recibo = new tReciboBL().GetByConstraint(Convert.ToInt32(IdRecibo));
            string FechaFact33 = ConfigurationManager.AppSettings["FechaFact33"].ToString();
            string path = Server.MapPath("~/");

            ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
            GeneraRecibo33.Recibo RF = reciboCFDI.consultaFacturaPDF(recibo, recibo.IdFIEL, path);
            byte[] arrayXml = Encoding.UTF8.GetBytes(RF.xml);
            MensajesInterfaz correo = new Utileria().sendEMail(txtCorreoEnvio.Text, "SERVICIO DE ENTREGA DE FACTURA.", "SERVICIO DE ENTREGA DE FACTURA. <br> Favor de no contestar este correo, debido a que es un servicio automatico.", new MemoryStream(arrayXml), new MemoryStream(RF.pdfBytes));
            vtnModal.ShowPopup(new Utileria().GetDescription(correo), ModalPopupMensaje.TypeMesssage.Alert);
        }

        #endregion

        #region Busqueda por contribuyentes
        private void llenaFiltro()
        {
            ddlFiltro.Items.Clear();
            ddlFiltro.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos", ""));
            //ddlFiltro.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Nombre", "UPPER( LTRIM(RTRIM(cc.Nombre) )+' '+LTRIM(RTRIM(cc.ApellidoPaterno))+' '+LTRIM(RTRIM(cc.ApellidoMaterno)))+' '+LTRIM(RTRIM(cc.RazonSocial)"));
            ddlFiltro.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Nombre", "cc.Nombre+' '+cc.ApellidoPaterno+' '+ cc.ApellidoMaterno +' '+ cc.RazonSocial"));
            //ddlFiltro.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Dirección", "UPPER( LTRIM(RTRIM(vp.NombreCalle) )+' '+LTRIM(RTRIM(vp.numero))+' '+LTRIM(RTRIM(vp.NombreColonia))+' '+LTRIM(RTRIM(vp.localidad))+' '+LTRIM(RTRIM(vp.CP)))"));
            ddlFiltro.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Dirección", "UPPER( vp.NombreCalle+' '+vp.numero+' '+vp.NombreColonia+' '+vp.localidad+' '+vp.CP)"));
            ddlFiltro.DataBind();
        }
        protected void buscarPropietario(object sender, ImageClickEventArgs e)
        {
            llenaFiltro();
            grdPropietarios.Visible = false;
            grdPropietarios.DataSource = null;
            grdPropietarios.DataBind();
            txtFiltro.Text = "";
            txtFiltro.Enabled = false;
            modalPropietario.Show();
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
            modalPropietario.Show();
        }
        protected void consultarPropietario(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text, "true" };
            ViewState["filtro"] = filtro;
            ViewState["sortCampo"] = "ClavePredial";
            ViewState["sortOnden"] = "asc";
            grdPropietarios.Visible = true;
            llenagridPropietario();
        }
        private void llenagridPropietario()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grdPropietarios.DataSource = new vVistasBL().GetFilterVPredioPropietario("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grdPropietarios.DataBind();
            }
            else
            {
                grdPropietarios.DataSource = new vVistasBL().GetFilterVPredioPropietario(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grdPropietarios.DataBind();

            }
            modalPropietario.Show();
        }
        protected void btnCancelarPropietario_Click(object sender, EventArgs e)
        {
            modalPropietario.Hide();
        }
        protected void grdPropietarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ActivarRegistro")
            {

                vPredios predio = new vVistasBL().GetByConstraint(Convert.ToInt32(e.CommandArgument));
                if (predio != null)
                {
                    cContribuyente Contribuyente = new cContribuyenteBL().GetByConstraint(predio.IdContribuyente);
                    //txtPropietario.Text = Contribuyente.Nombre + " " + Contribuyente.ApellidoPaterno + " " + Contribuyente.ApellidoMaterno;
                    txtClavePredial.Text = predio.ClavePredial.Trim();
                    modalPropietario.Hide();
                    buscaPredio(null, null);
                    //buscarClaveCatastral(null, null);
                }
            }
        }
        protected void grdPropietarios_Sorting(object sender, GridViewSortEventArgs e)
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

            llenagridPropietario();
        }
        protected void grdPropietarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPropietarios.PageIndex = e.NewPageIndex;
            llenagridPropietario();
        }
        protected void grdPropietarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPredios predio = new vVistasBL().GetByConstraint(Convert.ToInt32(grdPropietarios.DataKeys[e.Row.RowIndex].Values[0]));
                if (predio != null)
                {
                    cContribuyente c = new cContribuyenteBL().GetByConstraint(predio.IdContribuyente);
                    e.Row.Cells[1].Text = (c.ApellidoPaterno != null ? " " + c.ApellidoPaterno : "") + (c.ApellidoMaterno != null ? " " + c.ApellidoMaterno : "") + " " + (c.Nombre != null ? " " + c.Nombre : "");
                    e.Row.Cells[2].Text = predio.NombreCalle + " " + predio.numero + " " + predio.NombreColonia + " " + predio.localidad + " " + predio.CP;

                }

            }
        }

        #endregion

        protected void ProcesaRecibo(string tipoImpuesto)
        {
            SaldosC s = new SaldosC();
            Comprobante comprobante = new Comprobante();
            Receptor receptor = new Receptor();
            DatosRecibo datosRecibo = new DatosRecibo();
            ConceptoGral cgral = new ConceptoGral();
            Impuesto i = new Impuesto();
            Servicio sm = new Servicio();
            tRecibo recibo = new tRecibo();
            tTramite tramite = new tTramite();
            double vImporte = 0, vDescuento = 0, vSubtotal = 0;
            string vSerie = string.Empty;
            string vMesa = string.Empty;
            DateTime fechaPago = DateTime.Now;

            //para ambos tipos
            vSerie = new cParametroSistemaBL().GetValorByClave("SERIE").ToString();
            if (vSerie == "")
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.DefinirSerie), ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }

            int idPredio = Convert.ToInt32(ViewState["IdPredio"].ToString());
            cPredio predio = new cPredioBL().GetByConstraint(idPredio);
            cUsuarios U = new cUsuarios();
            cRFC rfc = new cRFC();

            //rfc = new cRfcBL().GetRfcByIdpredio(predio.IdContribuyente);
            rfc = new cRfcBL().GetRfcByIdcontribuyente(predio.IdContribuyente);
            U = (cUsuarios)Session["usuario"];

            if (tipoImpuesto == "IP")//PREDIAL
            {
                vSubtotal = Convert.ToDouble(ViewState["ImporteIp"].ToString()) + Convert.ToDouble(ViewState["DescuentoIp"].ToString());
                vDescuento = Convert.ToDouble(ViewState["DescuentoIp"].ToString());
                vImporte = Convert.ToDouble(ViewState["ImporteIp"].ToString());
                vMesa = new cParametroSistemaBL().GetValorByClave("MesaIP").ToString();
                if (vMesa == "")
                {
                    vtnModal.Dispose();
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.DefinirMesaIP), ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }

                i = (Impuesto)ViewState["IP"];
                //TRAMITE                             
                if (!rdbDiferencias.Checked)
                {
                    tramite.BimestreInicial = i.Estado.PeriodoGral.Length == 9 ? 0 : Convert.ToInt32(i.Estado.PeriodoGral.Substring(0, 1));
                    tramite.EjercicioInicial = i.Estado.PeriodoGral.Length == 9 ? 0 : Convert.ToInt32(i.Estado.PeriodoGral.Substring(2, 4));
                    tramite.BimestreFinal = i.Estado.PeriodoGral.Length == 9 ? 0 : Convert.ToInt32(i.Estado.PeriodoGral.Substring(9, 1));
                    tramite.EjercicioFinal = i.Estado.PeriodoGral.Length == 9 ? 0 : Convert.ToInt32(i.Estado.PeriodoGral.Substring(11, 4));
                }
                else
                {
                    tramite.BimestreInicial = i.Estado.PeriodoGral.Length == 9 ? 0 : Convert.ToInt32(i.Estado.PeriodoGral.Substring(0, 1));
                    tramite.EjercicioInicial = i.Estado.PeriodoGral.Length == 9 ? 0 : Convert.ToInt32(i.Estado.PeriodoGral.Substring(2, 1));
                    tramite.BimestreFinal = i.Estado.PeriodoGral.Length == 9 ? 0 : Convert.ToInt32(i.Estado.PeriodoGral.Substring(6, 1));
                    tramite.EjercicioFinal = i.Estado.PeriodoGral.Length == 9 ? 0 : Convert.ToInt32(i.Estado.PeriodoGral.Substring(8, 4));
                }
                tramite.IdRequerimiento = i.IdRequerimiento;
                tramite.Tipo = "IP";
                tramite.Periodo = i.Estado.PeriodoGral;
                tramite.IdTipoTramite = 5;

                string complemento = "";//", " + "Tipo predio " + predio.cTipoPredio.Descripcion + ", Super. Terreno " + predio.SuperficieTerreno + ", Super. Const. " + predio.SuperficieConstruccion + ", Base gravable " + predio.ValorCatastral;
                cgral = s.ConceptosP(i, predio.ClavePredial, complemento);
            }
            else //SERVICIOS
            {
                //string vDesc = ViewState["DescuentoSM"].ToString();
                vDescuento = Convert.ToDouble(ViewState["DescuentoSM"].ToString());
                vImporte = Convert.ToDouble(ViewState["ImporteSM"].ToString());
                vSubtotal = vImporte + vDescuento;
                vMesa = new cParametroSistemaBL().GetValorByClave("MESASM").ToString();
                if (vMesa == "")
                {
                    vtnModal.Dispose();
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.DefinirMesaSM), ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }
                sm = (Servicio)ViewState["SM"];
                //TRAMITE
                tramite.BimestreInicial = sm.Estado.PeriodoGral.Length == 9 ? 0 : Convert.ToInt32(sm.Estado.PeriodoGral.Substring(0, 1));
                tramite.EjercicioInicial = sm.Estado.PeriodoGral.Length == 9 ? 0 : Convert.ToInt32(sm.Estado.PeriodoGral.Substring(2, 4));
                tramite.BimestreFinal = sm.Estado.PeriodoGral.Length == 9 ? 0 : Convert.ToInt32(sm.Estado.PeriodoGral.Substring(9, 1));
                tramite.EjercicioFinal = sm.Estado.PeriodoGral.Length == 9 ? 0 : Convert.ToInt32(sm.Estado.PeriodoGral.Substring(11, 4));
                //tramite.IdRequerimiento = 0; // sm.IdRequerimiento;
                tramite.Tipo = "SM";
                tramite.Periodo = sm.Estado.PeriodoGral;
                tramite.IdTipoTramite = 6;

                cgral = s.ConceptosS((Servicio)ViewState["SM"], predio.ClavePredial);
            }

            //CONCEPTOS PARA RECIBO DETALLE
            if (cgral.Error != null)
            {
                vtnModal.Dispose();
                vtnModal.ShowPopup(new Utileria().GetDescription(cgral.Error), ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }

            //GUARDA RECIBO 
            try
            {
                tRecibo r = new tRecibo();
                List<Catastro.ModelosFactura.Concepto> lConceptos = new List<Catastro.ModelosFactura.Concepto>();
                using (TransactionScope scope = new TransactionScope())
                {
                    //tramite.Id  = 0;
                    tramite.IdPredio = predio.Id;
                    tramite.Fecha = DateTime.Now;
                    tramite.Status = "P";
                    tramite.BaseGravable = predio.ValorCatastral;
                    tramite.SuperficieTerreno = predio.SuperficieTerreno;
                    tramite.TerrenoPrivativo = predio.TerrenoPrivativo;
                    tramite.TerrenoComun = predio.TerrenoComun;
                    tramite.SuperficieConstruccion = predio.SuperficieConstruccion;
                    tramite.ConstruccionPrivativa = predio.ConstruccionPrivativa;
                    tramite.ConstruccionComun = predio.ConstruccionComun;
                    tramite.Observacion = "";
                    tramite.IdDiferencia = null;
                    if (i.IdDiferencia > 0)
                    {
                        tramite.Observacion = new cDiferenciaBL().GetDiferenciaPeriodoByPredio(predio.ClavePredial);
                        tramite.IdDiferencia = i.IdDiferencia;// null;
                    }
                    tramite.IdRequerimiento = null;
                    if (i.IdRequerimiento > 0) //null;
                        tramite.IdRequerimiento = i.IdRequerimiento; //null;
                    tramite.IdUsuario = U.Id;
                    tramite.Activo = true;
                    tramite.FechaModificacion = fechaPago;

                    //buscar si el tramite ya esta guardado antes de grabar
                    tTramite tr = new tTramiteBL().BuscaTramitePagado(Convert.ToInt32(tramite.IdPredio), tramite.IdTipoTramite, tramite.Periodo);
                    if (tr != null)
                        throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.ClavePagada), " ");


                    MensajesInterfaz msg = new tTramiteBL().Insert(tramite);
                    if (msg != MensajesInterfaz.Ingreso)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                    //RECIBO
                    #region recibo
                    //recibo.Id  // No de recibo //not null
                    r.IdCaja = tipoImpuesto == "IP" ? (int)ViewState["CajaIP"] : (int)ViewState["CajaSM"];
                    r.FechaPago = fechaPago; //not null
                    r.EstadoRecibo = "P"; //not null
                    r.Contribuyente = (predio.cContribuyente.ApellidoPaterno == null || predio.cContribuyente.ApellidoPaterno == "" ? "" : predio.cContribuyente.ApellidoPaterno.Trim())
                        + ' ' + (predio.cContribuyente.ApellidoMaterno == null || predio.cContribuyente.ApellidoMaterno == "" ? "" : predio.cContribuyente.ApellidoMaterno.Trim())
                        + ' ' + (predio.cContribuyente.Nombre == null || predio.cContribuyente.Nombre == "" ? "" : predio.cContribuyente.Nombre.Trim()) 
                        + ' ' + (predio.cContribuyente.RazonSocial == null || predio.cContribuyente.RazonSocial == "" ? "" : predio.cContribuyente.RazonSocial.Trim()); //not null
                    r.Rfc = rfc.RFC == null ? "XAXX010101000" : rfc.RFC; //persona.RFC; //null
                    r.Domicilio = (predio.Calle == null || predio.Calle == "" ? "" : predio.Calle.Trim()) + " #" +
                        (predio.Numero == null || predio.Numero == "" ? "" : predio.Numero.Trim()) + ", COL. " +
                        (predio.cColonia.NombreColonia == null || predio.cColonia.NombreColonia == "" ? "" : predio.cColonia.NombreColonia.Trim()) + ", C.P. " +
                        (predio.CP == null || predio.CP == "" ? "" : predio.CP.Trim()) + ", LOC. " +
                        (predio.Localidad == null || predio.Localidad == "" ? "" : predio.Localidad.Trim()); //null
                    r.ImportePagado = Convert.ToDecimal(vImporte); //not null
                    r.ImporteNeto = Convert.ToDecimal(vSubtotal);//not null
                    r.ImporteDescuento = Convert.ToDecimal(vDescuento); //not null
                    r.MaquinaPago = "-"; //not null
                    //r.IdUsuarioCancela = 0; //null
                    //r.MotivoCancelacion = ""; //null
                    //r.FechaCancelacion =  ""; //null
                    r.IdUsuarioCobra = U.Id; //not null
                    if (i.idDescuentoRez > 0)
                        r.IdDescuento = i.idDescuentoRez;
                    r.IdMesaCobro = tipoImpuesto == "IP" ? (int)ViewState["MesaIP"] : (int)ViewState["MesaSM"]; //not null
                    r.IdTipoPago = listConcepto[0].Id;  //new cTipoPagoBL().GetByClave(ddlMetodoPago.SelectedValue); //not null
                    r.IdTramite = tramite.Id; //not null
                    //r.TramiteEntregado = "";
                    //r.FechaEntrega = ;//null
                    //r.NoTipoPago
                    //r.NoAutorizacion = 0;
                    r.IdFIEL = new cFIELBL().GetByActive().Id; //not null     
                    r.Serie = vSerie; //not null
                    r.Ruta = "-"; //se asigna al generar el recibo//not null
                    r.IdMesa = tipoImpuesto == "IP" ? (int)ViewState["MesaIP"] : (int)ViewState["MesaSM"];//not null
                    r.CodigoSeguridad = "-"; //se asigna al generar el recibo//null
                    r.RND = 0; //al generar el recibo //null
                    r.DatosPredio = "Clave Catastral: " + predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3) + "     Tipo predio: " + predio.cTipoPredio.Descripcion.Trim() + "     Super. Terreno: " + predio.SuperficieTerreno + "m2.     Super. Const.: " + predio.SuperficieConstruccion + "m2.    Base gravable: $" + predio.ValorCatastral.ToString("#.##");
                    r.Observaciones = txtObservacion.Text;//not null
                    r.Activo = true; //not null
                    r.IdUsuario = U.Id;//not null
                    r.FechaModificacion = fechaPago;  //not null
                    r.usoCfdi = rfc.UsoCFDI is null || rfc.UsoCFDI == "" ? "G03" : rfc.UsoCFDI;

                    if (r.IdTipoPago > 0) //recibo.NoTipoPago = listConcepto[0].Transaccion;
                    {
                        r.NoTipoPago = listConcepto[0].Transaccion;
                        r.NoAutorizacion = listConcepto[0].Institucion;
                    }
                    //string tipoPago = ddlMetodoPago.SelectedValue.ToString();
                    //if (tipoPago == "04" || tipoPago == "28")
                    //{
                    //    r.NoTipoPago = txtTarjeta.Text; //null
                    //    r.NoAutorizacion = txtNoAprobacion.Text;//null
                    //}
                    //else if (tipoPago == "02")
                    //{
                    //    r.NoTipoPago = txtCheque.Text;//null
                    //}
                    //else if (tipoPago == "03")
                    //{
                    //    r.NoTipoPago = txtCheque.Text;//null
                    //}

                    ////RECIBO DETALLE  
                    foreach (ConceptosRec conceptos in cgral.conceptos)
                    {
                        tReciboDetalle reciboDetalle = new tReciboDetalle();
                        reciboDetalle.IdRecibo = r.Id;
                        reciboDetalle.IdConcepto = Convert.ToInt32(conceptos.IdCri);
                        reciboDetalle.IdControlSistema = conceptos.IdControlSistema;//SIN ID
                        reciboDetalle.ImporteNeto = Convert.ToDecimal(conceptos.Importe);
                        reciboDetalle.ImporteDescuento = Convert.ToDecimal(conceptos.Descuento);
                        reciboDetalle.ImporteAdicional = Convert.ToDecimal(conceptos.Adicional);
                        reciboDetalle.ImporteTotal = Convert.ToDecimal(conceptos.Importe - conceptos.Descuento);
                        reciboDetalle.Cantidad = 1;
                        reciboDetalle.DescuentoDecretoPorcentaje = 0;
                        reciboDetalle.IdConceptoRef = conceptos.IdConceptoRef;
                        reciboDetalle.DescuentoEspecialPorcentaje = 0;
                        reciboDetalle.Activo = true;
                        reciboDetalle.FechaModificacion = fechaPago;
                        reciboDetalle.IdUsuario = U.Id;
                        r.tReciboDetalle.Add(reciboDetalle);
                    }
                    new cErrorBL().insertcError("EstadoDeCuenta.aspx", "Inserta recibo linea 1485, clave predial:" + predio.ClavePredial);
                    msg = new tReciboBL().Insert(r);
                    if (msg != MensajesInterfaz.Ingreso)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                    #endregion

                    //agregar metodos
                    if (listConcepto.Count > 0)
                    {
                        tReciboMetodoPago m = new tReciboMetodoPago();
                        cTipoPago c = new cTipoPago();
                        foreach (MetodoGrid v in listConcepto)
                        {
                            m = new tReciboMetodoPago();
                            try
                            {
                                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                                conn.Open();

                                SqlCommand cmd1 = new SqlCommand(); // Create a object of SqlCommand class
                                cmd1.Connection = conn; //Pass the connection object to Command

                                string qry = "INSERT INTO dbo.tReciboMetodoPago(IdRecibo, Banco, NumeroCheque, Cuenta, Importe, IdTipopago, Activo, IdUsuario, FechaModificacion) " +
                                              " VALUES(@idRecibo, @banco, @numeroCheque, @cuenta, @importe, @idTipopago, @activo, @idUsuario,getdate()) ; ";
                                cmd1.CommandText = qry;
                                cmd1.Parameters.AddWithValue("@idRecibo", r.Id);
                                cmd1.Parameters.AddWithValue("@banco", v.Institucion);
                                cmd1.Parameters.AddWithValue("@numeroCheque", v.Transaccion);
                                cmd1.Parameters.AddWithValue("@cuenta", v.Clave);
                                cmd1.Parameters.AddWithValue("@importe", v.Importe);
                                cmd1.Parameters.AddWithValue("@idTipopago", v.Id);
                                cmd1.Parameters.AddWithValue("@activo", true);
                                cmd1.Parameters.AddWithValue("@idUsuario", U.Id);

                                cmd1.ExecuteNonQuery();
                                cmd1.Parameters.Clear();
                                cmd1.Dispose();
                                conn.Close();

                            }
                            catch (Exception ex)
                            {
                                //estatus = "REGISTRADO";
                                //resultDAO.MESSAGE = "El pago fue registrado pero no pudo ser facturado por una excepción.\n" + ex;
                            }
                        }
                    }

                    //ACTUALIZACION DE PREDIOS
                    #region actualizacion despues de guardar
                    if (tipoImpuesto == "IP")//PREDIAL
                    {
                        if (tramite.BimestreInicial > 0 && tramite.BimestreFinal > 0)
                        {
                            predio.BimestreFinIp = Convert.ToInt32(tramite.BimestreFinal);
                            predio.AaFinalIp = Convert.ToInt32(tramite.EjercicioFinal);
                            predio.ReciboIp = r.Id;
                            predio.IdReciboIp = r.Id;
                            predio.FechaPagoIp = fechaPago;

                            msg = new cPredioBL().Update(predio);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        }

                        //DIFERENCIAS
                        if (i.IdDiferencia > 0) //i.ActDiferencias > 0 || i.RezDiferencias > 0)
                        {
                            cDiferencia dif = new cDiferenciaBL().GetByConstraint(i.IdDiferencia);// new cDiferenciaBL().GetByClaveCatastral(predio.Id);
                            dif.Status = "P";
                            msg = new cDiferenciaBL().Update(dif);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                            //tramite.IdDiferencia = dif.Id;
                            //msg = new tTramiteBL().Update(tramite);
                            //if (msg != MensajesInterfaz.Actualizacion)
                            //    throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        }

                        if (i.IdRequerimiento > 0)
                        {
                            tRequerimiento req = new tRequerimientoBL().obtenerRequerimientoporIdPredio(predio.Id, "IP");

                            req.Status = "P";
                            req.FechaPago = fechaPago;
                            req.Recibo = r.Id;
                            req.Status = "P";
                            req.Activo = false;

                            msg = new tRequerimientoBL().Update(req);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                            //tramite.IdRequerimiento = req.Id;
                            //msg = new tTramiteBL().Update(tramite);
                            //if (msg != MensajesInterfaz.Actualizacion)
                            //    throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                            predio.IdTipoFaseIp = 1;
                            msg = new cPredioBL().Update(predio);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                            msg = new tRequerimientoBL().CancelAnteriorPorPredioPagado(req.IdPredio, req.TipoImpuesto, req.Id);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        }
                    }

                    if (tipoImpuesto == "SM")
                    {
                        predio.BimestreFinSm = Convert.ToInt32(tramite.BimestreFinal);
                        predio.AaFinalSm = Convert.ToInt32(tramite.EjercicioFinal);
                        predio.ReciboSm = r.Id;
                        predio.IdReciboSm = r.Id;
                        predio.FechaPagoSm = fechaPago;
                        msg = new cPredioBL().Update(predio);
                        if (msg != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                        if (i.IdRequerimiento > 0)
                        {
                            tRequerimiento req = new tRequerimientoBL().GetByConstraint(i.IdRequerimiento);
                            //tRequerimiento req = new tRequerimientoBL().obtenerRequerimientoporIdPredio(i.IdRequerimiento, "SM");
                            req.Status = "P";
                            req.FechaPago = fechaPago;
                            req.Recibo = r.Id;
                            req.Status = "P";
                            req.Activo = false;

                            msg = new tRequerimientoBL().Update(req);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                            tramite.IdRequerimiento = req.Id;
                            msg = new tTramiteBL().Update(tramite);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                            predio.IdTipoFaseIp = 1;
                            msg = new cPredioBL().Update(predio);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                            msg = new tRequerimientoBL().CancelAnteriorPorPredioPagado(req.IdPredio, req.TipoImpuesto, req.Id);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        }
                    }

                    if (i.idDescuentoRez > 0)
                    {
                        tDescuentoAsignado desc = new tDescuentoAsignadoBL().BusqPorIdPredioIdDescuento(predio.Id, i.idDescuentoRez);
                        desc.Activo = false;
                        desc.Estado = "APLICADO";

                        msg = new tDescuentoAsignadoBL().Update(desc);
                        if (msg != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    }
                    #endregion

                    #region digital

                    foreach (ConceptosRec cr in cgral.conceptos)
                    {
                        Catastro.ModelosFactura.Concepto concepto = new Catastro.ModelosFactura.Concepto();
                        concepto.Cantidad = Convert.ToInt64(cr.Cantidad);
                        concepto.Descripcion = cr.Descripcion;
                        concepto.Importe = this.formatearDecimales(cr.Importe.ToString());
                        concepto.ValorUnitario = this.formatearDecimales(cr.ValorUnitario.ToString());
                        concepto.NoIdentificacion = cr.Id;
                        concepto.ClaveUnidad = cr.ClaveUnidadMedida;
                        concepto.ClaveProdServ = cr.ClaveProdServ;
                        concepto.Descuento = this.formatearDecimales(cr.Descuento.ToString());
                        lConceptos.Add(concepto);
                    }

                    // SI LA TRANSACCION ESTA COMPLETA GENERA EL RECIBO
                    //COMPROBANTE using GeneraRecibo;
                    comprobante.cajero = U.Nombre + " " + U.ApellidoPaterno + " " + U.ApellidoMaterno;

                    //comprobante.FormaDePago = ddlMetodoPago.SelectedItem.Text;
                    comprobante.FormaDePago = listConcepto[0].Clave; //not null
                    comprobante.MetodoDePago = "PUE"; //"EN UNA SOLA EXHIBICIÓN";
                    comprobante.motivoDescuento = "";
                    comprobante.Serie = vSerie;
                    comprobante.Mesa = vMesa;
                    comprobante.TipoDeComprobante = "I";
                    comprobante.Folio = r.Id.ToString().PadLeft(6, '0');
                    comprobante.SubTotal = Convert.ToDouble(vSubtotal);
                    comprobante.descuento = Convert.ToDouble(vDescuento);
                    comprobante.Total = Convert.ToDouble(vImporte);
                    comprobante.DatosPredio = "Clave Catastral: " + predio.ClavePredial.Trim() + "     Tipo predio: " + predio.cTipoPredio.Descripcion.Trim() + "     Super. Terreno: " + predio.SuperficieTerreno + "m2.     Super. Const.: " + predio.SuperficieConstruccion + "m2.    Base gravable: $" + predio.ValorCatastral.ToString("#.##");
                    comprobante.Observaciones = txtObservacion.Text;//not null

                    //DATOS RECEPTOR   
                    receptor.Nombre = (predio.cContribuyente.ApellidoPaterno == null || predio.cContribuyente.ApellidoPaterno == "" ? "" : predio.cContribuyente.ApellidoPaterno.Trim())
                        + ' ' + (predio.cContribuyente.ApellidoMaterno == null || predio.cContribuyente.ApellidoMaterno == "" ? "" : predio.cContribuyente.ApellidoMaterno.Trim())
                        + ' ' + (predio.cContribuyente.Nombre == null || predio.cContribuyente.Nombre == "" ? "" : predio.cContribuyente.Nombre.Trim()) + ' ' + (predio.cContribuyente.RazonSocial == null || predio.cContribuyente.RazonSocial == "" ? "" : predio.cContribuyente.RazonSocial.Trim());
                    receptor.RFC = rfc.RFC == null ? "XAXX010101000" : rfc.RFC;
                    receptor.email = predio.cContribuyente.Email;
                    receptor.UsoCFDI = "P01";
                    receptor.Id = predio.Id;
                    //Se agrega solo el domicilio del contribuyente, de no traer se agrega el del predio
                    // predio.Calle.Trim() + ' ' + predio.Numero.Trim() + ' ' + predio.cColonia.NombreColonia.Trim() + ' ' + predio.Localidad.Trim(); 
                    string domimicilioContribuyente = (predio.cContribuyente.Calle is null || predio.cContribuyente.Calle == "" ? "" : predio.cContribuyente.Calle.Trim()) + " #"+
                        (predio.cContribuyente.Numero is null || predio.cContribuyente.Numero == "" ? "" : predio.cContribuyente.Numero.Trim()) + " COL. "+
                        (predio.cContribuyente.Colonia is null || predio.cContribuyente.Colonia == "" ? "" : predio.cContribuyente.Colonia.Trim()) +" C.P. "+
                        (predio.cContribuyente.CP is null || predio.cContribuyente.CP == "" ? "" : predio.cContribuyente.CP)+ ", "+
                        (predio.cContribuyente.Municipio is null || predio.cContribuyente.Municipio == "" ? "" : predio.cContribuyente.Municipio.Trim());
                        
                    receptor.Domicilio = domimicilioContribuyente is null || domimicilioContribuyente == "" ? r.Domicilio : domimicilioContribuyente;
                    //

                    //path = Server.MapPath("~/");

                    #endregion

                    //FIN DE LA TRANSACCION
                    scope.Complete();
                }

                //Nueva Forma de Factura y Timbrar
                Catastro.ModelosFactura.Factura factura = new Catastro.ModelosFactura.Factura();
                Catastro.ModelosFactura.Comprobante comprobanteFactura = new Catastro.ModelosFactura.Comprobante();
                Catastro.ModelosFactura.TimbradoEx tEx = new Catastro.ModelosFactura.TimbradoEx();

                string usoCFDI = "";
                string cvMetodoPago = "";
                string rfcEmitio = "";

                comprobanteFactura.Conceptos = lConceptos;
                comprobanteFactura.Emisor = tEx.getEmisor();
                comprobanteFactura.Receptor = tEx.getReceptor(receptor.Nombre, receptor.RFC);
                comprobanteFactura.CondicionesDePago = "CONTADO";
                comprobanteFactura.FormaPago = comprobante.FormaDePago;
                comprobanteFactura.MetodoPago = comprobante.MetodoDePago;
                comprobanteFactura.TipoDeComprobante = comprobante.TipoDeComprobante; // ingreso
                comprobanteFactura.Fecha = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
                comprobanteFactura.LugarExpedicion = Constantes.cpSistema;
                comprobanteFactura.Moneda = "MXN";
                comprobanteFactura.TipoCambio = 1;
                comprobanteFactura.NoCertificado = Constantes.noCertificadoEmisor;
                //string noCertificadoEmisor = guardarJson("No Certificado: " + comprobanteFactura.NoCertificado);
                comprobanteFactura.Serie = "U";
                //comprobanteFactura.Serie = "FW09";
                comprobanteFactura.Descuento = this.formatearDecimales(comprobante.descuento.ToString());
                comprobanteFactura.SubTotal = this.formatearDecimales(comprobante.SubTotal.ToString());


                comprobanteFactura.Total = this.formatearDecimales(comprobante.Total.ToString());

                factura.Comprobante = comprobanteFactura;

                usoCFDI = factura.Comprobante.Receptor.UsoCfdi;
                cvMetodoPago = factura.Comprobante.MetodoPago;
                rfcEmitio = factura.Comprobante.Emisor.Rfc;

                // Generamos Timbrado
                var json = factura.ToJson();
                string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

                SqlXml newXml = new SqlXml();
                string xmlPath = "";
                string selloPath = "";
                string folioFiscal = "";
                string noCertificado = "";
                string noCertificadoSAT = "";
                string fechaTimbrado = "";
                string selloCFDI = "";
                string selloSAT = "";
                string cadenaOriginalCFDI = "";
                string selloComprobante = "";
                string query = "";

                ServiceFacturaXpress.ServicioTimbradoWSPortTypeClient te = new ServiceFacturaXpress.ServicioTimbradoWSPortTypeClient();
                ServiceFacturaXpress.RespuestaTimbrado rt;

                bool estadoConexion;
                System.Uri Url = new System.Uri("https://www.google.com/");

                System.Net.WebRequest WebRequest;
                WebRequest = System.Net.WebRequest.Create(Url);
                System.Net.WebResponse objetoResp;

                try
                {
                    objetoResp = WebRequest.GetResponse();
                    estadoConexion = true;
                    objetoResp.Close();
                }
                catch (Exception e)
                {
                    estadoConexion = false;
                }
                WebRequest = null;

                //Comprobamos si hay internet
                if (estadoConexion)
                {

                    //string jsonEnviado = guardarJson(json);
                    if (Constantes.modoPruebas)
                    {
                        rt = te.timbrarJSON(Constantes.apikey, base64, txtKeyDev.Text, txtCerDev.Text);
                    }
                    else
                    {
                        rt = te.timbrarJSON(Constantes.apikey, base64, txtKey.Text, txtCer.Text);
                    }

                    //string jsonRespuesta = guardarJson( rt.message);


                    if (rt.code.Equals("200"))
                    {
                        xmlPath = tEx.crearXMLTimbrado(rt.data,U.Usuario+"-"+r.Id);

                        DateTime fechaF = DateTime.Parse(factura.Comprobante.Fecha);

                        newXml = new SqlXml(new XmlTextReader(xmlPath));

                        // Actualizar cantidad de timbres restantes

                        try
                        {
                            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                            Dictionary<string, string> xmlFF = ManejadorXML.obtenerFacturaTimbradaXML(xmlPath);

                            folioFiscal = xmlFF["UUID"];
                            noCertificado = xmlFF["NoCertificado"];
                            noCertificadoSAT = xmlFF["NoCertificadoSAT"];
                            fechaTimbrado = xmlFF["FechaTimbrado"];
                            selloCFDI = xmlFF["SelloCFD"];
                            selloSAT = xmlFF["SelloSAT"];
                            selloComprobante = xmlFF["Sello"];

                            cadenaOriginalCFDI = "||1.0|" + folioFiscal + "|" + factura.Comprobante.Fecha + "|" + selloCFDI + "|" + selloSAT;

                            selloPath = tEx.generarSelloQR(folioFiscal, factura.Comprobante.Emisor.Rfc, factura.Comprobante.Receptor.Rfc, factura.Comprobante.Total.Trim(), selloComprobante, U.Usuario+"-"+r.Id);

                            con.Open();

                            SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                            cmd.Connection = con; //Pass the connection object to Command

                            query = "INSERT INTO tReciboFactura (IdPredio,FolioFiscal,CertificadoEmisor,CertificadoSAT,FechahoraCertificado,SelloCDFI,selloSAT,Cadena,Fechafactura,NumCaja,selloDigital,xml,IdRecibo)  VALUES " +
                                "(@IdPredio, @folioFiscal, @CertificadoEmisor, @CertificadoSAT, @FechahoraCertificado, @SelloCDFI, @selloSAT, @Cadena, @Fechafactura, @NumCaja, @selloDigital, @xml, @IdRecibo) ; ";
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@IdPredio", idPredio);
                            cmd.Parameters.AddWithValue("@folioFiscal", folioFiscal);
                            cmd.Parameters.AddWithValue("@CertificadoEmisor", noCertificado);
                            cmd.Parameters.AddWithValue("@CertificadoSAT", noCertificadoSAT);
                            cmd.Parameters.AddWithValue("@FechahoraCertificado", fechaTimbrado);
                            cmd.Parameters.AddWithValue("@SelloCDFI", selloCFDI);
                            cmd.Parameters.AddWithValue("@selloSAT", selloSAT);
                            cmd.Parameters.AddWithValue("@Cadena", cadenaOriginalCFDI);
                            cmd.Parameters.AddWithValue("@Fechafactura", fechaF);
                            cmd.Parameters.AddWithValue("@NumCaja", r.IdCaja);
                            cmd.Parameters.AddWithValue("@selloDigital", selloComprobante);
                            cmd.Parameters.AddWithValue("@xml", newXml.Value);
                            cmd.Parameters.AddWithValue("@IdRecibo", r.Id);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            query = "update tRecibo set Facturado=@Facturado where Id=@IdRecibo; ";
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@Facturado", "1");
                            cmd.Parameters.AddWithValue("@IdRecibo", r.Id);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            cmd.Dispose();
                            con.Close();

                        }
                        catch (Exception ex)
                        {
                            //estatus = "REGISTRADO";
                            //resultDAO.MESSAGE = "El pago fue registrado pero no pudo ser facturado por una excepción.\n" + ex;
                        }
                    }
                }

                /////Termina nueva Forma de Factura
                //Genera RECIBO
                ResultDAO resultPDF = new ResultDAO();
                ClienteBean clienteBean = new ClienteBean();
                PagoBean pagoBean = new PagoBean();
                FacturaBean facturaBean = new FacturaBean();

                predio.ClavePredial = predio.ClavePredial == "" || predio.ClavePredial == null ? "" : predio.ClavePredial;

                if (predio.ClavePredial == "" || predio.ClavePredial == null)
                {
                    clienteBean.NumContrato = predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? "" : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClaveAnterior.Substring(9, 3);
                }
                else
                {
                    if (predio.ClavePredial.Substring(0, 1) == "0") //solo si se refiere a una cuenta, la clave empieza con 0
                    {
                        clienteBean.NumContrato = predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? "" : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClaveAnterior.Substring(9, 3);
                    }
                    else
                    {
                        clienteBean.NumContrato = predio.ClavePredial == "" || predio.ClavePredial == null ? "" : predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3);
                    }

                }

                clienteBean.Rfc = receptor.RFC;
                clienteBean.Direccion = receptor.Domicilio;
                clienteBean.DireccionPredio = r.Domicilio;
                clienteBean.CorreoElectronico = receptor.email;
                clienteBean.NombreCompleto = receptor.Nombre;
                clienteBean.Giro = predio.cTipoPredio.Descripcion;
                clienteBean.ClaveGiro = predio.cTipoPredio.Id.ToString();

                clienteBean.ClaveCatastral = predio.ClavePredial == "" || predio.ClavePredial == null ? "" : (predio.ClavePredial.Substring(0, 1) == "0" ? "" : predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3)); //Validar con si tiene 0 vacia
                clienteBean.CuentaCatastral = predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? "" : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClaveAnterior.Substring(9, 3);
                clienteBean.TipoPredio = predio.cTipoPredio.Descripcion;
                clienteBean.SuperficieTerreno = predio.SuperficieTerreno.ToString();
                clienteBean.AreaComun = predio.TerrenoComun.ToString();
                clienteBean.SuperficieAreaConstruccion = predio.SuperficieConstruccion.ToString();
                clienteBean.BaseImpuesto = Convert.ToDecimal(tramite.BaseGravable).ToString("C"); // predio.cBaseGravable.ToString();
                clienteBean.Referencia = predio.Referencia;
                clienteBean.UsuarioFacturo = U.Usuario;

                pagoBean.PagoTotal = comprobante.Total.ToString();
                pagoBean.Subtotal = comprobante.SubTotal.ToString();
                pagoBean.Descuento = comprobante.descuento.ToString();
                pagoBean.NumRecibo = comprobante.Folio;
                pagoBean.MetodoPago = comprobante.MetodoDePago + " - " + tEx.getMetodoPago(comprobante.MetodoDePago);
                pagoBean.Observaciones = comprobante.Observaciones;
                pagoBean.FormaPago = comprobante.FormaDePago + " - " + tEx.getFormaPago(comprobante.FormaDePago);
                pagoBean.FechaRealizo = DateTime.Now.ToString("dd/MM/yyyy");
                pagoBean.Expedicion = comprobante.LugarExpedicion;

                pagoBean.CantidadLetra = Letras(comprobante.Total.ToString(), 0);

                DateTime fecha = DateTime.ParseExact(fechaTimbrado, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.CurrentCulture);
                facturaBean.CadenaOriginal = cadenaOriginalCFDI;
                facturaBean.CertificadoCSD = selloCFDI;
                facturaBean.CertificadoEmisor = noCertificado;
                facturaBean.CertificadoSAT = noCertificadoSAT;
                facturaBean.DireccionFiscal = receptor.Domicilio;
                facturaBean.FolioFiscal = folioFiscal;
                facturaBean.HoraFecha = fecha.ToString();
                facturaBean.NombreFiscal = receptor.Nombre;
                facturaBean.Rfc = receptor.RFC;
                facturaBean.RFCEmisor = Constantes.rfcEmisor;
                facturaBean.RFCReceptor = receptor.RFC;
                facturaBean.SelloComprobante = selloComprobante;
                facturaBean.SelloDigitalCFDI = selloCFDI;
                facturaBean.SelloSAT = selloSAT;
                facturaBean.Total = comprobante.Total.ToString();
                facturaBean.UsoCfdi =rfc.UsoCFDI is null || rfc.UsoCFDI =="" ? "G03" : rfc.UsoCFDI;

                string hoy = DateTime.Now.ToString(Constantes.formatoFechaArchivos);
                string reciboPath = Server.MapPath("~/Temporales");
                
                reciboPath += Constantes.recibosSistemaFolder;
                if (!System.IO.Directory.Exists(reciboPath))
                {
                    System.IO.Directory.CreateDirectory(reciboPath);
                }
                reciboPath += U.Usuario + "-" + pagoBean.NumRecibo + "-" + hoy + "-RECIBO.pdf";

                if (PDF.crearRecibo(clienteBean, pagoBean, facturaBean, (!folioFiscal.Equals("")), reciboPath, selloPath, cgral.conceptos))
                {
                    resultPDF.SUCCESS = true;
                    resultPDF.PARAM_AUX = reciboPath;
                    //this.enviarCorreo(reciboPath, xmlPath, selloPath, correo,r.Id,;
                }
                else
                {
                    resultPDF.SUCCESS = false;
                }

                //VISUALIZA RECIBO
                ////GENERA PDF                   
                //FileStream newFile = new FileStream(path + reciboPath, FileMode.Create);
                //newFile.Write(rec.pdfBytes, 0, rec.pdfBytes.Length);
                //newFile.Close(); 

                //tbFacturar.Visible = true;

                frameRecibo.Src = "~/Temporales/sipred-files/Recibos expedidos/" + U.Usuario + "-" + pagoBean.NumRecibo + "-" + hoy + "-RECIBO.pdf";
                //btnSiguiente.Visible = true;
                modalRecibo.Show();
                //String Clientscript = "printPdf();";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", Clientscript, true);
                ViewState["IdRecibo"] = r.Id;
                ViewState["pdf"] = "";
            }
            catch (Exception ex)
            {
                new Utileria().logError("Estado de cuenta, ProcesaRecibo", ex);
                InicializaDetalle();
                vtnModal.ShowPopup(new Utileria().GetDescription(ex.Message), ModalPopupMensaje.TypeMesssage.Alert);
            }
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.IngresoRFC))
            {
                modalFactura.Show();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActualizacionRFC))
            {
                modalFactura.Show();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.FacturaCorreoError))
            {
                modalRecibo.Show();
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ViewState["idMod"] = 0;
            InicializaDetalle();
            divEncabezado.Visible = true;
            divEstadoCta.Visible = false;
            divDetallado.Visible = false;
            grd = new GridView();
            lblDescripcion.Text = "";
            //lblPrincipal.Text = "Impuesto Predial y Servicios Municipales";
            lbl_titulo.Text = "";
        }
        protected void btnCancelarBusqueda_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Servicios/EstadoDeCuenta.aspx", false);
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Servicios/EstadoDeCuenta.aspx", false);
        }
        protected void imCerrarEstado_Click(object sender, ImageClickEventArgs e)
        {
            modalEstado.Hide();
        }
        protected void btnUltimoRec_Click(object sender, EventArgs e)
        {
            if (txtClavePredial.Text != "")
            {
                int id = new cPredioBL().GetByClavePredial(txtClavePredial.Text).Id;
                tTramite ttram = new tTramiteBL().GettTramiteByPredio(new cPredioBL().GetByClavePredial(txtClavePredial.Text).Id, 5);
                //recibo = ;
                Reporte(new tReciboBL().GetByIdTramite(ttram.Id));
            }
            else
                vtnModal.ShowPopup(new Utileria().GetDescription("Clave Catastral vacia."), ModalPopupMensaje.TypeMesssage.Alert);
        }
        private void Reporte(tRecibo recibo)
        {
            string FechaFact33 = ConfigurationManager.AppSettings["FechaFact33"].ToString();
            string path = Server.MapPath("~/");

            if (recibo.FechaPago < DateTime.ParseExact(FechaFact33, "dd/MM/yyyy", CultureInfo.InvariantCulture))
            {
                GeneraRecibo.Recibo rec;
                GeneraRecibo.ReciboCFDI reciboCFDI = new GeneraRecibo.ReciboCFDI();
                if (recibo.Facturado)
                {
                    rec = reciboCFDI.consultaFacturaPDF(recibo, recibo.IdFIEL, path);
                }
                else
                {
                    rec = reciboCFDI.consultaRecibo(recibo, recibo.IdFIEL, path);
                }
                FileStream newFile = new FileStream(path + "/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf", FileMode.Create);
                newFile.Write(rec.pdfBytes, 0, rec.pdfBytes.Length);
                newFile.Close();
                frameRecibo.Src = "~/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf";
                modalRecibo.Show();

                //entorno de produccion            
                ViewState["IdRecibo"] = recibo.Id;
                //if (recibo.Facturado)
                //{
               //tbFacturar.Visible = false;
                //    divCerrarFactura.Visible = true;
                //}
                //else
                //{
               //tbFacturar.Visible = true;
                //    divCerrarFactura.Visible = false;
                //}
            }
            else
            {
                GeneraRecibo33.Recibo rec;
                ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
                if (recibo.Facturado)
                {
                    rec = reciboCFDI.consultaFacturaPDF(recibo, recibo.IdFIEL, path);
                }
                else
                {
                    rec = reciboCFDI.consultaRecibo(recibo, recibo.IdFIEL, path);
                }
                FileStream newFile = new FileStream(path + "/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf", FileMode.Create);
                newFile.Write(rec.pdfBytes, 0, rec.pdfBytes.Length);
                newFile.Close();
                frameRecibo.Src = "~/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf";
                modalRecibo.Show();

                //entorno de produccion            
                ViewState["IdRecibo"] = recibo.Id;
                //if (recibo.Facturado)
                //{
               //tbFacturar.Visible = false;
                //    divCerrarFactura.Visible = true;
                //}
                //else
                //{
               //tbFacturar.Visible = true;
                //    divCerrarFactura.Visible = false;
                //}
            }
        }
        protected void Button2_Click1(object sender, EventArgs e)
        {
            modalRecibo.Show();
        }
        protected void btnExportar_Click(object sender, EventArgs e)
        {
            //SaldosC s = new SaldosC();
            //DataTable dtDetalladoIP = ViewState["dtDetalladoIP"] as DataTable;
            //string fileName = ViewState["ClavePredial"].ToString();
            //fileName = "Desglosado_" + fileName + "_" + DateTime.Now.Second.ToString();
            //s.GetExcelFromDataTable(dtDetalladoIP,  fileName);
            //ViewState["dtDetalladoIP"] = null;
            //ExportExcel();
        }

    }
}

