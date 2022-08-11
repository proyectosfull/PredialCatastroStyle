using Catastro.Controles;
using Clases;
using Clases.BL;
using Clases.Models;
using Clases.Utilerias;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Catastro.Catalogos
{
    public partial class catPredios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenaListas();
                ViewState["ContratosAgua"] = new List<ContratoAgua>();
                ViewState["Observaciones"] = new List<PredioObservacion>();
                ViewState["sortCampoBG"] = "Ejercicio";
                ViewState["sortOndenBG"] = "desc";
                TabContainer1.Tabs[4].Visible = false;
                TabContainer1.Tabs[5].Visible = false;
                if (Session["parametro"] != null)
                {
                    Dictionary<string, string> parametros = (Dictionary<string, string>)Session["parametro"];
                    if (parametros.ContainsKey("idPredio"))
                    {
                        llenaConfiguracion(Convert.ToInt32(parametros["idPredio"]));
                        if (parametros.ContainsKey("tipoPantalla"))
                        {
                            hdfId.Value = parametros["idPredio"];
                            if (parametros["tipoPantalla"] == "C")
                            {
                                lblTitulo.Text = "Consulta Predio";
                                activaCampos(false);
                            }
                            else {
                                lblTitulo.Text = "Edición de Predio";
                            }
                            llenagrBG();
                            llenaHistorial();
                            TabContainer1.Tabs[4].Visible = true;
                            TabContainer1.Tabs[5].Visible = true;
                        }
                    }
                }
                else
                {
                    txtAaFinalIP.Visible = false;
                    txtAaFinalSm.Visible = false;
                    txtAaFinalIPV.Enabled = true;
                    txtAaFinalIPV.Visible = true;
                    txtAaFinalSmV.Enabled = true;
                    txtAaFinalSmV.Visible = true;
                    rfvAaFinalIPV.Enabled = true;
                    rfvAaFinalIPV.Visible = true;
                    revAaFinalIPV.Enabled = true;
                    revAaFinalIPV.Visible = true;
                    revAaFinalSmV.Enabled = true;
                    revAaFinalSmV.Visible = true;
                    rfvAaFinalSmV.Enabled = true;
                    rfvAaFinalSmV.Visible = true;
                    ddlBimestreIP.Enabled = true;
                    ddlBimestreSm.Enabled = true;
                }

                ViewState["sortCampo"] = "Nombre";
                ViewState["sortOnden"] = "asc";
                txtFiltro.Enabled = false;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenaListas()
        {
            //ddlColonia.DataValueField = "Id";
            //ddlColonia.DataTextField = "NombreColonia";
            //ddlColonia.DataSource = new cColoniaBL().GetAll();
            //ddlColonia.DataBind();
            //ddlColonia.Items.Insert(0, new ListItem("Seleccionar Colonia", ""));
            
            ddlExentoPago.DataValueField = "Id";
            ddlExentoPago.DataTextField = "Descripcion";
            ddlExentoPago.DataSource = new cExentoPagoBL().GetAll();
            ddlExentoPago.DataBind();
            ddlExentoPago.Items.Insert(0, new ListItem("Seleccionar Exento Pago", ""));

            //ddlStatusPredio.DataValueField = "Id";
            //ddlStatusPredio.DataTextField = "Descripcion";
            //ddlStatusPredio.DataSource = new cStatusPredioBL().GetAll();
            //ddlStatusPredio.DataBind();
            ddlStatusPredio.Items.Insert(0, new ListItem("Seleccionar Estado de Predio", ""));       
                 
            ddlTipoPredio.DataValueField = "Id";
            ddlTipoPredio.DataTextField = "Descripcion";
            ddlTipoPredio.DataSource = new cTipoPredioBL().GetAll();
            ddlTipoPredio.DataBind();
            ddlTipoPredio.Items.Insert(0, new ListItem("Seleccionar Tipo de Predio", ""));

            ddlUsoSuelo.DataValueField = "Id";
            ddlUsoSuelo.DataTextField = "Clave";
            ddlUsoSuelo.DataSource = new cUsoSueloBL().GetAll();
            ddlUsoSuelo.DataBind();
            ddlUsoSuelo.Items.Insert(0, new ListItem("Seleccionar Tipo de Uso de Suelo", ""));

            ddlTipoMovAvaluo.DataValueField = "Id";
            ddlTipoMovAvaluo.DataTextField = "Descripcion";
            ddlTipoMovAvaluo.DataSource = new cTipoMovAvaluoBL().GetAll();
            ddlTipoMovAvaluo.DataBind();
            ddlTipoMovAvaluo.Items.Insert(0, new ListItem("Seleccionar Tipo de Movimiento de Avaluo", ""));

            ddlCondominio.DataValueField = "Id";
            ddlCondominio.DataTextField = "Descripcion";
            ddlCondominio.DataSource = new cCondominioBL().GetAll();
            ddlCondominio.DataBind();
            ddlCondominio.Items.Insert(0, new ListItem("Seleccionar Condominio", ""));
        }

        
        private void activaCampos(bool activa)
        {
            txtClavePredial.Enabled = activa;
            txtCalle.Enabled = activa;
            txtNumero.Enabled = activa;
            txtColonia.Enabled = activa;
            txtCP.Enabled = activa;
            txtLocalidad.Enabled = activa;
            txtFechaAlta.Enabled = activa;
            txtFechaTraslado.Enabled = activa;
            txtZona.Enabled = activa;
            txtMetrosFrente.Enabled = activa;
            ddlUsoSuelo.Enabled = activa;
            ddlExentoPago.Enabled = activa;
            ddlStatusPredio.Enabled = activa;
            txtFechaBaja.Enabled = activa;
            ddlTipoPredio.Enabled = activa;
            imbBuscar.Visible = activa;
            txtConstribuyente.Enabled = activa;
            txtNivel.Enabled = activa;
            txtUbicacionExpediente.Enabled = activa;
            ddlBimestreIP.Enabled = activa;
            txtAaFinalIP.Enabled = activa;
            ddlBimestreSm.Enabled = activa;
            txtAaFinalSm.Enabled = activa;
            btnGuardar.Visible = activa;
            ddlTipoMovAvaluo.Enabled = activa;
            ddlCondominio.Enabled = activa;
            btnAgregarContrato.Visible = activa;
            btnObservaciones.Visible = activa;
            grdContratos.Columns[1].Visible = activa;
            grdObservacions.Columns[1].Visible = activa;
            imgBusContribuyente.Visible = activa;
        }
        private void llenaConfiguracion(int Id)
        {
            cPredio predio = new cPredioBL().GetByConstraint(Id);
            txtClavePredial.Text = predio.ClavePredial;
            txtCalle.Text = predio.Calle;
            txtNumero.Text = predio.Numero;
            //if (!predio.cColonia.Activo)
            //{
            //    ddlColonia.Items.Add(new ListItem(predio.cColonia.NombreColonia, predio.cColonia.Id.ToString()));
            //}
            txtColonia.Text = predio.cColonia.NombreColonia;
            txtCP.Text = predio.CP;
            txtLocalidad.Text = predio.Localidad;
            lblSuperTerreno.Text = predio.SuperficieTerreno == null ? "" : ((double)predio.SuperficieTerreno).ToString("N", CultureInfo.CurrentCulture);
            lblTerrenoPrivativo.Text = predio.TerrenoPrivativo.ToString();
            lblTerrenoComun.Text = predio.TerrenoComun.ToString();
            lblValorTerreno.Text = predio.ValorTerreno == null ? "" : ((double)predio.ValorTerreno).ToString("C", CultureInfo.CurrentCulture);
            lblSuperficeConstruccion.Text = predio.SuperficieConstruccion == null ? "" : ((double)predio.SuperficieConstruccion).ToString("N", CultureInfo.CurrentCulture);
            lblConstruccionPrivativa.Text = predio.ConstruccionPrivativa.ToString();
            lblConstruccionComun.Text = predio.ConstruccionComun.ToString();
            lblValorConstruccion.Text = predio.ValorConstruccion.ToString("C", CultureInfo.CurrentCulture);
            txtFechaAlta.Text = predio.FechaAlta.ToString("dd/MM/yyyy");
           
            txtFechaTraslado.Text = predio.FechaTraslado.ToString("dd/MM/yyyy");
            txtZona.Text = predio.Zona.ToString();
            txtMetrosFrente.Text = predio.MetrosFrente.ToString();
            if (!predio.cUsoSuelo.Activo)
            {
                ddlUsoSuelo.Items.Add(new ListItem(predio.cUsoSuelo.Clave, predio.cUsoSuelo.Id.ToString()));
            }
            ddlUsoSuelo.SelectedValue = predio.IdUsoSuelo.ToString();
            if (predio.IdExentoPago != null && !predio.cExentoPago.Activo)
            {
                ddlExentoPago.Items.Add(new ListItem(predio.cExentoPago.Descripcion, predio.cExentoPago.Id.ToString()));
            }
            ddlExentoPago.SelectedValue = predio.IdExentoPago.ToString();
            //if (!predio.cStatusPredio.Activo)
            //{
            //    ddlStatusPredio.Items.Add(new ListItem(predio.cStatusPredio.Descripcion, predio.cStatusPredio.Id.ToString()));
            //}
            ddlStatusPredio.SelectedValue = predio.IdStatusPredio.ToString();
            txtFechaBaja.Text = predio.FechaBaja != null ? ((DateTime)predio.FechaBaja).ToString("dd/MM/yyyy") :"";
            if (!predio.cTipoPredio.Activo)
            {
                ddlTipoPredio.Items.Add(new ListItem(predio.cTipoPredio.Descripcion, predio.cTipoPredio.Id.ToString()));
            }
            ddlTipoPredio.SelectedValue = predio.IdTipoPredio.ToString();
            hdfContribuyente.Value = predio.IdContribuyente.ToString();
            txtConstribuyente.Text = predio.cContribuyente.ApellidoPaterno.Trim() + " " + predio.cContribuyente.ApellidoMaterno.Trim() + " " + predio.cContribuyente.Nombre.Trim() ;
            //lblTipoFaseIP.Text = predio.IdTipoFaseIp == 0 ? "SIN DOCUMENTO" : new cTipoFaseBL().GetByFase(predio.IdTipoFaseIp);
            //hdfTipoFaseIp.Value = predio.cTipoFase != null ? predio.cTipoFase.Id.ToString() : "";
            lblTipoFaseIP.Text = new cTipoFaseBL().GetByConstraint(predio.IdTipoFaseIp).Descripcion.ToUpper();
            hdfTipoFaseIp.Value = new cTipoFaseBL().GetByConstraint(predio.IdTipoFaseIp).Descripcion.ToUpper();
            txtNivel.Text = predio.Nivel.ToString();
            txtUbicacionExpediente.Text = predio.UbicacionExpediente;
            ddlBimestreIP.SelectedValue = predio.BimestreFinIp.ToString();
            txtAaFinalIP.Text = predio.AaFinalIp.ToString();
            //lblTipoFaseSm.Text = predio.IdTipoFaseSm == 0 ? "SIN DOCUMENTO" : new cTipoFaseBL().GetByFase(predio.IdTipoFaseSm); 
            //hdfTipoFaseSm.Value = predio.cTipoFase1 != null ? predio.cTipoFase1.Id.ToString() : "";
            lblTipoFaseSm.Text = new cTipoFaseBL().GetByConstraint(predio.IdTipoFaseSm).Descripcion.ToUpper();
            hdfTipoFaseSm.Value = new cTipoFaseBL().GetByConstraint(predio.IdTipoFaseSm).Descripcion.ToUpper(); 
            ddlBimestreSm.SelectedValue = predio.BimestreFinSm.ToString();
            txtAaFinalSm.Text = predio.AaFinalSm.ToString();
            if (!predio.cTipoMovAvaluo.Activo)
            {
                ddlTipoMovAvaluo.Items.Add(new ListItem(predio.cTipoMovAvaluo.Descripcion, predio.cTipoMovAvaluo.Id.ToString()));
            }
            ddlTipoMovAvaluo.SelectedValue = predio.IdTipoMovAvaluo.ToString();

            if (!predio.cCondominio.Activo)
            {
                ddlCondominio.Items.Add(new ListItem(predio.cCondominio.Descripcion, predio.cCondominio.Id.ToString()));
            }
            ddlCondominio.SelectedValue = predio.IdCondominio.ToString();

            List<ContratoAgua> contratos = new List<ContratoAgua>();
            foreach (cContratoAgua contrato in predio.cContratoAgua)
            {
                ContratoAgua contratoN = new ContratoAgua();
                contratoN.Activo = contrato.Activo;
                contratoN.Id = contrato.Id;
                contratoN.IdPredio = contrato.IdPredio;
                contratoN.IdUsuario = contrato.IdUsuario;
                contratoN.NoContrato = contrato.NoContrato;
                contratoN.FechaModificacion = contrato.FechaModificacion;
                contratos.Add(contratoN);
            }
            grdContratos.DataSource = contratos;
            grdContratos.DataBind();
            ViewState["ContratosAgua"] = contratos;            
            List<PredioObservacion> observaciones = new List<PredioObservacion>();
            foreach (cPredioObservacion observacion in predio.cPredioObservacion)
            {
                PredioObservacion predioN = new PredioObservacion();
                predioN.Activo = observacion.Activo;
                predioN.Id = observacion.Id;
                predioN.IdPredio = observacion.IdPredio;
                predioN.IdUsuario = observacion.IdUsuario;
                predioN.Observacion = observacion.Observacion;
                predioN.FechaModificacion = observacion.FechaModificacion;
                observaciones.Add(predioN);
            }
            grdObservacions.DataSource = observaciones;
            grdObservacions.DataBind();
            ViewState["Observaciones"] = observaciones;
            cBaseGravable lastBaseGravable = new cBaseGravableBL().GetLastByIdPredio(Id);
            if (lastBaseGravable == null)
                lastBaseGravable = new cBaseGravable();
            lblSuperTerreno.Text = lastBaseGravable.SuperficieTerreno == null ? "" : ((double)lastBaseGravable.SuperficieTerreno).ToString("N", CultureInfo.CurrentCulture);
            lblTerrenoPrivativo.Text = lastBaseGravable.TerrenoPrivativo.ToString();
            lblTerrenoComun.Text = lastBaseGravable.TerrenoComun.ToString();
            lblValorTerreno.Text = lastBaseGravable.ValorTerreno == null ? "" : ((double)lastBaseGravable.ValorTerreno).ToString("C", CultureInfo.CurrentCulture);
            lblSuperficeConstruccion.Text = lastBaseGravable.SuperficieConstruccion.ToString("N", CultureInfo.CurrentCulture);
            lblConstruccionPrivativa.Text = lastBaseGravable.ConstruccionPrivativa.ToString();
            lblConstruccionComun.Text = lastBaseGravable.ConstruccionComun.ToString();
            lblValorConstruccion.Text = lastBaseGravable.ValorConstruccion.ToString("C", CultureInfo.CurrentCulture);
            lblFechaEvaluo.Text = lastBaseGravable.Id==0? predio.FechaAvaluo.ToString("dd/MM/yyyy") :lastBaseGravable.FechaAvaluo.ToString("dd/MM/yyyy");
            lblBaseGravable.Text = lastBaseGravable.Valor.ToString("C", CultureInfo.CurrentCulture);
            // chbAdultoMayor.Checked = predio.
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            MensajesInterfaz msg = new MensajesInterfaz();
            hdfContribuyente.Value = new cContribuyenteBL().GetByName(txtConstribuyente.Text);
            if (hdfContribuyente.Value.Equals(""))
            {
                msg = MensajesInterfaz.ContribuyenteNoValido;
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Error);
                return;
            }
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            cPredio predio = new cPredio();
            if (!(hdfId.Value == string.Empty || hdfId.Value == "0"))
            {
                predio = new cPredioBL().GetByConstraint(Convert.ToInt32(hdfId.Value));
            }

            predio.ClavePredial = txtClavePredial.Text;
            //if (!txtRecibo.Text.Equals(""))
            //    predio.IdRecibo = int.Parse(txtRecibo.Text);
            predio.Calle = txtCalle.Text;
            predio.Numero = txtNumero.Text;
            if (new cColoniaBL().GetByName(txtColonia.Text) == null)
            {
                msg = MensajesInterfaz.ColoniaNoValida;
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Error);
                return;
            }
            predio.IdColonia = new cColoniaBL().GetByName(txtColonia.Text).Id;
            predio.CP = txtCP.Text;
            predio.Localidad = txtLocalidad.Text;
            predio.SuperficieTerreno = lblSuperTerreno.Text == "" ? 0 : (double?)Convert.ToDouble(lblSuperTerreno.Text.Replace(",", ""));
            predio.TerrenoPrivativo = lblTerrenoPrivativo.Text == "" ? null : (double?)Convert.ToDouble(lblTerrenoPrivativo.Text);
            predio.TerrenoComun = lblTerrenoComun.Text == "" ? null : (double?)Convert.ToDouble(lblTerrenoComun.Text);
            predio.ValorTerreno = lblValorTerreno.Text == "" ? 0 : (double?)Convert.ToDouble(lblValorTerreno.Text.Replace("$", "").Replace(",",""));
            predio.SuperficieConstruccion = lblSuperficeConstruccion.Text == "" ? 0 : (double?)Convert.ToDouble(lblSuperficeConstruccion.Text.Replace(",", ""));
            predio.ConstruccionPrivativa = lblConstruccionPrivativa.Text == "" ? null : (double?)Convert.ToDouble(lblConstruccionPrivativa.Text);
            predio.ConstruccionComun = lblConstruccionComun.Text == "" ? null : (double?)Convert.ToDouble(lblConstruccionComun.Text);
            predio.ValorConstruccion = lblValorConstruccion.Text == "" ? 0 : Convert.ToDouble(lblValorConstruccion.Text.Replace("$", "").Replace(",", ""));
            predio.FechaAlta = Convert.ToDateTime(txtFechaAlta.Text);
            predio.FechaAvaluo = lblFechaEvaluo.Text == "" ? DateTime.Now : Convert.ToDateTime(lblFechaEvaluo.Text);
            predio.FechaTraslado = Convert.ToDateTime(txtFechaTraslado.Text);
            predio.Zona = int.Parse(txtZona.Text);
            predio.MetrosFrente = Convert.ToDouble(txtMetrosFrente.Text);
            predio.IdUsoSuelo = int.Parse(ddlUsoSuelo.SelectedValue);
            predio.IdExentoPago = ddlExentoPago.SelectedIndex == 0 ? predio.IdExentoPago : (int?)int.Parse(ddlExentoPago.SelectedValue);
            predio.IdStatusPredio = int.Parse(ddlStatusPredio.SelectedValue);
            predio.FechaBaja = txtFechaBaja.Text != "" ? (DateTime?)Convert.ToDateTime(txtFechaBaja.Text): null;
            predio.IdTipoPredio = int.Parse(ddlTipoPredio.SelectedValue);
            predio.IdContribuyente = int.Parse(hdfContribuyente.Value);            
            predio.IdTipoFaseIp = lblTipoFaseIP.Text == "" ? 1:  predio.IdTipoFaseIp ;
            predio.Nivel = txtNivel.Text == "" ? predio.Nivel : (int?)int.Parse(txtNivel.Text);
            predio.UbicacionExpediente = txtUbicacionExpediente.Text;
            predio.BimestreFinIp = int.Parse(ddlBimestreIP.SelectedValue);
            predio.AaFinalIp = txtAaFinalIPV.Text.Equals("") ? predio.AaFinalIp:  int.Parse(txtAaFinalIPV.Text);            
            predio.IdTipoFaseSm = lblTipoFaseSm.Text == "" ? 1: predio.IdTipoFaseSm ;
            predio.BimestreFinSm = int.Parse(ddlBimestreSm.SelectedValue);
            predio.AaFinalSm = txtAaFinalSmV.Text.Equals("") ? predio.AaFinalSm : int.Parse(txtAaFinalSmV.Text);     
            predio.IdTipoMovAvaluo = int.Parse(ddlTipoMovAvaluo.SelectedValue) > 0 ? int.Parse(ddlTipoMovAvaluo.SelectedValue): 1;
            predio.IdCondominio = int.Parse(ddlCondominio.SelectedValue) > 0 ? int.Parse(ddlCondominio.SelectedValue) : 1;
            //predio.IdCondominio = 1;
            predio.IdCartografia = "";
            predio.IdUsuario = U.Id;
            predio.FechaModificacion = DateTime.Now;
            predio.Activo = true;
            List<ContratoAgua> contratos = (List<ContratoAgua>)ViewState["ContratosAgua"];
            predio.cContratoAgua.Clear();
            foreach (ContratoAgua contrato in contratos)
            {
                cContratoAgua contratoN = new cContratoAgua();
                contratoN.Activo = contrato.Activo;
                contratoN.Id = contrato.Id;
                contratoN.IdPredio = contrato.IdPredio;
                contratoN.IdUsuario = contrato.IdUsuario;
                contratoN.NoContrato = contrato.NoContrato;
                contratoN.FechaModificacion = contrato.FechaModificacion;
                predio.cContratoAgua.Add(contratoN);
            }
            List<PredioObservacion> observaciones = (List<PredioObservacion>)ViewState["Observaciones"];
            predio.cPredioObservacion.Clear();
            foreach (PredioObservacion observacion in observaciones)
            {
                cPredioObservacion predioN = new cPredioObservacion();
                predioN.Activo = observacion.Activo;
                predioN.Id = observacion.Id;
                predioN.IdPredio = observacion.IdPredio;
                predioN.IdUsuario = observacion.IdUsuario;
                predioN.Observacion = observacion.Observacion;
                predioN.FechaModificacion = observacion.FechaModificacion;
                predio.cPredioObservacion.Add(predioN);
            }
            if (hdfId.Value == string.Empty || hdfId.Value == "0")
            {
                if ( new cPredioBL().GetByClavePredial(txtClavePredial.Text) != null)
                {
                    msg = MensajesInterfaz.ClaveResgistrada;
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Error);
                    return;
                }

                msg = new cPredioBL().Insert(predio);
            }
            else { 
                msg = new cPredioBL().Update(predio);
            }
            vtnModal.DysplayCancelar = false;
            vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);
        }
        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text, "true" };
            ViewState["filtro"] = filtro;
            llenagrid();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            //Session["parametro"] = null;
            Response.Redirect("BusquedaPredio.aspx");
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                Response.Redirect("BusquedaPredio.aspx");
            }
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
            pnl_Modal.Show();
        }

        #region contribuyentes
        protected void imbBuscarContribuyente_Click(object sender, ImageClickEventArgs e)
        {
            llenaFiltro();
            //mpeNuevoContribuyente.Show();
        }
        private void llenaFiltro()
        {
            ddlFiltro.DataSource = new cContribuyenteBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
            pnl_Modal.Show();
        }
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cContribuyenteBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cContribuyenteBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();

            }
            pnl_Modal.Show();
        }


        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenagrid();
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ActivarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                cContribuyente Contribuyente = new cContribuyenteBL().GetByConstraint(id);
                txtConstribuyente.Text =  Contribuyente.ApellidoPaterno.Trim() + " " + Contribuyente.ApellidoMaterno.Trim() + " "+ Contribuyente.Nombre.Trim() ;//***
                hdfContribuyente.Value = Contribuyente.Id.ToString();
                pnl_Modal.Hide();
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

        #endregion

        protected void btnCancelarContribuyente_Click(object sender, EventArgs e)
        {
            pnl_Modal.Hide();
        }

        protected void btnAgregarContrato_Click(object sender, EventArgs e)
        {
            List<ContratoAgua> contratos = (List<ContratoAgua>)ViewState["ContratosAgua"];
            ContratoAgua nuevo = new ContratoAgua();
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            nuevo.NoContrato = txtContrato.Text;
            nuevo.IdUsuario = U.Id;
            nuevo.FechaModificacion = DateTime.Now;
            nuevo.Activo = true;
            contratos.Add(nuevo);
            ViewState["ContratosAgua"] = contratos;
            grdContratos.DataSource = contratos;
            grdContratos.DataBind();
            txtContrato.Text = "";
        }

        protected void grdContratos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarRegistro")
            {
                int Id = Convert.ToInt32(e.CommandArgument);
                List<ContratoAgua> contratos = (List<ContratoAgua>)ViewState["ContratosAgua"];
                ContratoAgua g = contratos.FirstOrDefault(dr => dr.Id == Id);
                contratos.Remove(g);
                ViewState["ContratosAgua"] = contratos;
                grdContratos.DataSource = contratos;
                grdContratos.DataBind();
            }
        }

        protected void btnObservaciones_Click(object sender, EventArgs e)
        {
            List<PredioObservacion> observaciones = (List<PredioObservacion>)ViewState["Observaciones"];
            PredioObservacion nuevo = new PredioObservacion();
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            nuevo.Observacion = txtObservacion.Text;
            nuevo.IdUsuario = U.Id;
            nuevo.FechaModificacion = DateTime.Now;
            nuevo.Activo = true;
            observaciones.Add(nuevo);
            ViewState["Observaciones"] = observaciones;
            grdObservacions.DataSource = observaciones;
            grdObservacions.DataBind();
            txtObservacion.Text = "";
        }

        protected void grdObservacions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarRegistro")
            {
                int Id = Convert.ToInt32(e.CommandArgument);
                List<PredioObservacion> observaciones = (List<PredioObservacion>)ViewState["Observaciones"];
                PredioObservacion g = observaciones.FirstOrDefault(dr => dr.Id == Id);
                observaciones.Remove(g);
                ViewState["Observaciones"] = observaciones;
                grdObservacions.DataSource = observaciones;
                grdObservacions.DataBind();
            }
        }

        protected void grdBG_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortCampoBG"] == null)
            {
                ViewState["sortCampoBG"] = e.SortExpression.ToString();
                ViewState["sortOndenBG"] = "asc";
            }
            else
            {
                if (e.SortExpression.ToString() == ViewState["sortCampoBG"].ToString())
                {
                    if (ViewState["sortOndenBG"].ToString() == "asc")
                        ViewState["sortOndenBG"] = "desc";
                    else
                        ViewState["sortOndenBG"] = "asc";
                }
                else
                {
                    ViewState["sortCampoBG"] = e.SortExpression.ToString();
                    ViewState["sortOndenBG"] = "asc";
                }
            }
            llenagrBG();
        }

        protected void grdBG_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdBG.PageIndex = e.NewPageIndex;
            llenagrBG();
        }

        protected void llenagrBG()
        {
            grdBG.DataSource = new vVistasBL().GetVBaseGravableById(hdfId.Value, ViewState["sortCampoBG"].ToString(), ViewState["sortOndenBG"].ToString());
            grdBG.DataBind();
        }

        protected void grdH_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdH.PageIndex = e.NewPageIndex;
            llenaHistorial();
        }

        protected void llenaHistorial()
        {
            List<vAntecedentePredio> lAntecedentes = new vVistasBL().ObtieneAntecedentePredio(txtClavePredial.Text);
            grdH.DataSource = lAntecedentes;
            grdH.DataBind();
            if (lAntecedentes.Count == 0)
            {
                btnReasignarH.Visible = false;
            }
        }

        protected void btnReasignarH_Click(object sender, EventArgs e)
        {
            pnlReasigna_Modal.Show();
        }
        protected void btnGuardarNuevoHistorial_Click(object sender, EventArgs e)
        {
            tTramiteBL Tbl = new tTramiteBL();
            List <tTramite> lTramites = Tbl.GetTramiteIdPredio(Convert.ToInt32(hdfId.Value));
            //cPredio pAnt = new cPredioBL().GetByConstraint(Convert.ToInt32(hdfId.Value));
            cPredio pNuevo = new cPredioBL().GetByClavePredial(txtPredioNuevo.Text);

            MensajesInterfaz msg = new MensajesInterfaz();
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (tTramite t in lTramites)
                {
                    t.IdPredio = pNuevo.Id;
                    msg = Tbl.Update(t);
                    if (msg != MensajesInterfaz.Actualizacion)
                    {
                        throw new System.ArgumentException("ocurrio un problema", "problema");
                    }
                }
                //FIN DE LA TRANSACCION 
                scope.Complete();
            }
            llenaHistorial();   
            vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);

        }

        protected void btnGuardarContribuyente_Click(object sender, EventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            vtnModal.DysplayCancelar = false;
            cContribuyente Contribuyente = new cContribuyente();
            MensajesInterfaz msg = new MensajesInterfaz();
            //Contribuyente = new cContribuyenteBL().GetByConstraint(Convert.ToInt32(hdfId.Value));
            Contribuyente.Nombre = txtNombre.Text;
            Contribuyente.ApellidoPaterno = txtApellidoP.Text;
            Contribuyente.ApellidoMaterno = txtApellidoM.Text;
            Contribuyente.Calle = txtCalleN.Text;
            Contribuyente.Numero = txtNumeroN.Text;
            Contribuyente.Colonia = txtColoniaN.Text;
            Contribuyente.Localidad = txtLocalidadN.Text;
            Contribuyente.Municipio = txtMunicipio.Text;
            Contribuyente.Estado = txtEstado.Text;
            Contribuyente.CP = txtCPN.Text;
            Contribuyente.Email = txtEmail.Text;
            Contribuyente.Telefono = txtTelefono.Text;
            Contribuyente.Curp = txtCurp.Text;
            Contribuyente.IdUsuario = U.Id;
            Contribuyente.Activo = true;
            Contribuyente.FechaModificacion = DateTime.Now;
            Contribuyente.AdultoMayor = chbAdultoMayor.Checked ? "S" : "N";
            msg = new cContribuyenteBL().Insert(Contribuyente);
            if (msg.Equals(MensajesInterfaz.Ingreso))
            {
                txtConstribuyente.Text = txtApellidoP.Text + " " + txtApellidoM.Text + " " + txtNombre.Text;
            }
            vtnModal.DysplayCancelar = false;
            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ContribuyenteAgregado), ModalPopupMensaje.TypeMesssage.Confirm);

        }
        protected void rbltipoPersona_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbltipoPersona.SelectedValue == "Física")
            {
                rfvApellidoPaterno.Enabled = true;
                rvfCurp.Enabled = true;
                chbAdultoMayor.Enabled = true;
            }
            else
            {
                rfvApellidoPaterno.Enabled = false;
                rvfCurp.Enabled = false;
                chbAdultoMayor.Checked = false;
                chbAdultoMayor.Enabled = false;
            }
            mpeNuevoContribuyente.Show();
        }

        protected void imbAgregarContribuyente_Click(object sender, ImageClickEventArgs e)
        {
            //llenaFiltro();
            mpeNuevoContribuyente.Show();
        }
    }
}