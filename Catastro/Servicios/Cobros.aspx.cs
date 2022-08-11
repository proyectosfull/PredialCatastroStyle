using System.Linq;

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.ComponentModel;
using System.Globalization;
using System.Transactions;
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
using Microsoft.Reporting.WebForms;
using Catastro.ModelosFactura;
using Comprobante = GeneraRecibo33.Comprobante;
using Receptor = GeneraRecibo33.Receptor;
using ZXing;
using ZXing.QrCode;
using System.Data.SqlTypes;
using System.Xml;
using System.Data.SqlClient;
using Factura = GeneraRecibo33.Factura;
using SOAPT.Clases;

namespace Catastro.Servicios
{
    public partial class Cobros : System.Web.UI.Page
    {
        List<MetodoGrid> listConcepto = new List<MetodoGrid>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnl_Modal.Hide();
                llenaTram();
                llenaTipoPago();
                limpiaCampos();
                cargarCajero();
                txtClvCastatral.Focus();
                ViewState["sortCampo"] = "id";
                ViewState["sortOnden"] = "asc";
                //btnRecalculo.Enabled = false;
            }

            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        protected void llenaTram()
        {
            var listTipoTramite = new cTipoTramiteBL().GetAll().Where(t => t.Tramite != "Predial" && t.Tramite != "Servicios").ToList().OrderBy(t => t.Tramite);

            ddlTipoTramite.DataValueField = "Id";
            ddlTipoTramite.DataTextField = "Tramite";
            ddlTipoTramite.DataSource = listTipoTramite;
            ddlTipoTramite.DataBind();
            ddlTipoTramite.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Tramite", "%"));
            ddlTipoTramite.Items.ToString();
        }

        protected void llenaTipoPago()
        {
            ddlMetodoPago.Items.Clear();
            ddlMetodoPago.DataValueField = "Clave";
            ddlMetodoPago.DataTextField = "Descripcion";
            ddlMetodoPago.DataSource = new cTipoPagoBL().GetAll();
            ddlMetodoPago.DataBind();
            ddlMetodoPago.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
        }

        protected void limpiaCampos()
        {
            ViewState["idP"] = null;
            ViewState["Importe"] = null;
            ViewState["idT"] = null;
            ViewState["nombreC"] = "";
            lblTotal.Text = "";
            txtContruyente.Text = "";
            Label5.Text = "Propietario";
            txtCalle.Text = "";
            txtNumero.Text = "";
            txtColonia.Text = "";
            txtCP.Text = "";
            txtLocalidad.Text = "";
            llenaTram();
            ddlTipoTramite.Enabled = false;
            limpiacamposTramite();
        }

        protected void limpiacamposTramite()
        {
            ViewState["ImportePagado"] = 0;
            ViewState["ImporteNeto"] = 0;
            ViewState["ImporteDescuento"] = 0;
            ViewState["TipoTramite"] = 0;
            ViewState["idConcepto"] = 0;
            ViewState["idCriIsabis"] = "";
            ViewState["idCriIsabisRe"] = "";
            lblTotal.Text = "";
            lblMensajeConvenio.Text = "";
            lblMensajeConvenio.Visible = false;
            grd.DataSource = null;
            grd.DataBind();
            grd.Visible = false;
            grdCobros.DataSource = null;
            grdCobros.DataBind();
            grdCobros.Visible = false;
            Label16.Visible = false;
            lblTotal.Text = "";
            lblTotal.Visible = false;
            lblMetodoPago.Visible = false;
            ddlMetodoPago.Visible = false;
            btnCobrar.Visible = false;
            //btnCobrar.Enabled = true;
            btnRecalculo.Visible = false;
            llenaTipoPago();

        }

        protected void llenaPant(cPredio Predio)
        {
            ViewState["ClavePredial"] = Predio.ClavePredial.ToString();
            ViewState["idP"] = Predio.Id.ToString();

            txtContruyente.Text = (Predio.cContribuyente.ApellidoPaterno == null ? "" : Predio.cContribuyente.ApellidoPaterno) + " "+
                                   (Predio.cContribuyente.ApellidoMaterno == null ? "" : Predio.cContribuyente.ApellidoMaterno) + " "+
                                   (Predio.cContribuyente.Nombre == null ? "" : Predio.cContribuyente.Nombre) + " " +
                                   (Predio.cContribuyente.RazonSocial == null || Predio.cContribuyente.RazonSocial == "" ? "" : Predio.cContribuyente.RazonSocial);

            //txtContruyente.Text = Predio.cContribuyente.ApellidoPaterno + " " + Predio.cContribuyente.ApellidoMaterno + " " + Predio.cContribuyente.Nombre + " " + Predio.cContribuyente.RazonSocial;
            txtPropietario.Text = txtContruyente.Text;
            ViewState["nombreC"] = txtContruyente.Text;
            txtCalle.Text = Predio.Calle;
            txtNumero.Text = Predio.Numero;
            txtColonia.Text = Predio.cColonia.NombreColonia;
            txtCP.Text = Predio.CP;
            txtLocalidad.Text = Predio.Localidad;
            ddlTipoTramite.Enabled = true;
        }

        protected void buscarClaveCatastral(object sender, System.EventArgs e)
        {
            if (txtClvCastatral.Text.Length == 12)
            {
                cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
                if (Predio != null)
                {
                    if (Predio.cStatusPredio.Descripcion == "A")
                    {
                        limpiaCampos();
                        llenaPant(Predio);
                    }
                    else
                    {
                        limpiaCampos();
                        vtnModal.ShowPopup(new Utileria().GetDescription(Predio.cStatusPredio.Descripcion == "B" ? MensajesInterfaz.sTatusPredioBaja : MensajesInterfaz.sTatusPredioSuspendido
                            ), ModalPopupMensaje.TypeMesssage.Alert);
                    }

                }
                else
                {
                    limpiaCampos();
                    vtnModal.ShowPopup(new Utileria().GetDescription("Predio No Encontrado"), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            else
            {
                limpiaCampos();
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);
                //btnNuevotramitre.Visible = true;
                lblTotal.Text = "";
                //lbltotalSimbolo.Visible = false;
            }
        }

        private void cobrarGenerarRecibo()
        {
            try
            {
                Comprobante comprobante = new Comprobante();
                Receptor receptor = new Receptor();
                DatosRecibo datosRecibo = new DatosRecibo();
                cPredio Predio = new cPredio();
                tTramite tramite = new tTramite();
                tRecibo recibo = new tRecibo();
                cUsuarios U = new cUsuarios();

                List<ConceptosRec> lconceptos = new List<ConceptosRec>();
                List<Catastro.ModelosFactura.Concepto> lConceptos = new List<Catastro.ModelosFactura.Concepto>();
                using (TransactionScope scope = new TransactionScope())
                {
                    MensajesInterfaz msg = new MensajesInterfaz();
                    
                    U = (cUsuarios)Session["usuario"];
                    tRequerimiento requerimiento = null;
                    Predio = new cPredioBL().GetByConstraint(Convert.ToInt32(ViewState["idP"]));
                    tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idT"]));
                    cTipoTramite tipoTramite = new cTipoTramiteBL().GetByConstraint(Convert.ToInt32(ddlTipoTramite.SelectedValue));
                    decimal importePagado = Convert.ToDecimal(ViewState["ImportePagado"].ToString());
                    decimal importeNeto = Convert.ToDecimal(ViewState["ImporteNeto"].ToString());
                    decimal importeDescuento = Convert.ToDecimal(ViewState["ImporteDescuento"].ToString());
                    String vSerie = new cParametroSistemaBL().GetValorByClave("SERIE").ToString();
                    
                    int idMesaCobro = 0;
                    if (vSerie == "")
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.DefinirSerie), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }

                    //Se actualiza el tramite o se inserta
                    if (tramite != null)
                    {
                        if (tramite.Status != "A")
                        {
                            throw new System.ArgumentException(new Utileria().GetDescription("Imposible generar el cobro, el tramite a cambia de estado Activo."));
                        }
                        tramite.FechaModificacion = DateTime.Now;
                        tramite.IdUsuario = U.Id;
                        tramite.Status = "P";

                        if (tramite.IdTipoTramite == 3) // Convenios
                        {
                            if (new tTramiteBL().ultimoPagoConvenio(Convert.ToInt32(tramite.IdConvenio), tramite.Id) == 1)
                            {
                                tConvenioEdoCta convenioEdoCta = new tConvenioEdoCtaBL().GetByConstraint(tramite.tConvenio.IdConvenioEdoCta);
                                cPredio p = new cPredioBL().GetByConstraint(Convert.ToInt32(tramite.IdPredio));
                                cDiferencia diferencia = null;

                                if (convenioEdoCta.IdRequerimiento != null && convenioEdoCta.IdRequerimiento != 0)
                                {
                                    requerimiento = new tRequerimientoBL().GetByConstraint(Convert.ToInt32(convenioEdoCta.IdRequerimiento));
                                    if (requerimiento != null)
                                    {
                                        requerimiento.Status = "P";
                                        requerimiento.FechaPago = DateTime.Now;
                                        requerimiento.IdUsuario = U.Id;
                                        requerimiento.FechaModificacion = DateTime.Now;
                                    }
                                    else
                                    {
                                        throw new System.ArgumentException(new Utileria().GetDescription("Imposible realizar el pago no se encontró el Requerimiento"), "Error");
                                    }

                                }

                                if (convenioEdoCta != null)
                                {
                                    tConvenio convenio = tramite.tConvenio;
                                    convenioEdoCta.Status = "P";
                                    convenioEdoCta.IdUsuario = U.Id;
                                    convenioEdoCta.FechaModificacion = DateTime.Now;
                                    int ini = 0, fin = 0;
                                    String periodo = "";
                                    if (convenioEdoCta.PeriodoIP != null)
                                    {
                                        ini = convenioEdoCta.PeriodoIP.Trim().IndexOf("-") + 2;
                                        fin = convenioEdoCta.PeriodoIP.Trim().Length;
                                        periodo = convenioEdoCta.PeriodoIP.Trim().Substring(ini, fin - ini);
                                        ini = periodo.IndexOf(" ");
                                        fin = periodo.Length;
                                        p.BimestreFinIp = Convert.ToInt32(periodo.Substring(0, ini));
                                        p.AaFinalIp = Convert.ToInt32(periodo.Substring(ini + 1, fin - (ini + 1)));

                                    }

                                    if (convenioEdoCta.PeriodoSM != null)
                                    {
                                        ini = convenioEdoCta.PeriodoSM.Trim().IndexOf("-") + 2;
                                        fin = convenioEdoCta.PeriodoSM.Trim().Length;
                                        periodo = convenioEdoCta.PeriodoSM.Trim().Substring(ini, fin - ini);
                                        ini = periodo.IndexOf(" ");
                                        fin = periodo.Length;
                                        p.BimestreFinSm = Convert.ToInt32(periodo.Substring(0, ini));
                                        p.AaFinalSm = Convert.ToInt32(periodo.Substring(ini + 1, fin - (ini + 1)));

                                    }

                                    if (periodo != "")
                                    {
                                        p.IdUsuario = U.Id;
                                        p.FechaModificacion = DateTime.Now;
                                        msg = new cPredioBL().Update(p);
                                        if (msg != MensajesInterfaz.Actualizacion)
                                        {
                                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                                        }
                                    }

                                    if (convenioEdoCta.IdDiferencias != null && convenioEdoCta.IdDiferencias != 0)
                                    {
                                        diferencia = new cDiferenciaBL().GetByConstraint(Convert.ToInt32(convenioEdoCta.IdDiferencias));
                                        if (diferencia != null)
                                        {
                                            diferencia.Status = "P";
                                            diferencia.IdUsuario = U.Id;
                                            //diferencia.Activo = false;
                                            diferencia.FechaModificacion = DateTime.Now;
                                        }
                                        else
                                        {
                                            throw new System.ArgumentException(new Utileria().GetDescription("Imposible realizar el pago no se encontró la Diferencia"), "Error");
                                        }


                                        msg = new cDiferenciaBL().Update(diferencia);
                                        if (msg != MensajesInterfaz.Actualizacion)
                                        {
                                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                                        }
                                    }

                                    msg = new tConvenioEdoCtaBL().Update(convenioEdoCta);
                                    if (msg != MensajesInterfaz.Actualizacion)
                                    {
                                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                                    }

                                    if (convenio != null)
                                    {
                                        convenio.Status = "P";
                                        //convenio.Activo = false;
                                        msg = new tConvenioBL().Update(convenio);
                                        if (msg != MensajesInterfaz.Actualizacion)
                                        {
                                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                                        }
                                    }
                                    else
                                    {
                                        throw new System.ArgumentException(new Utileria().GetDescription("Imposible generar el cobro, el convenio no se encuentra"));
                                    }
                                }
                                else
                                {
                                    throw new System.ArgumentException(new Utileria().GetDescription("Imposible generar el cobro, el convenio estado de cuenta no se encuentra"));
                                }
                            }
                        }

                        msg = new tTramiteBL().Update(tramite);
                        if (msg != MensajesInterfaz.Actualizacion)
                        {
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        }


                    }
                    else
                    {
                        tramite = new tTramite();
                        tramite.IdPredio = Convert.ToInt32(ViewState["idP"]);
                        tramite.Fecha = DateTime.Now;
                        tramite.Status = "P";
                        tramite.IdTipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);
                        tramite.Activo = true;
                        tramite.IdUsuario = U.Id;
                        tramite.FechaModificacion = DateTime.Now;
                        msg = new tTramiteBL().Insert(tramite);
                        if (msg != MensajesInterfaz.Ingreso)
                        {
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        }
                    }
                    //Se realiza el recibo

                    tramite.BaseGravable = Predio.ValorCatastral;
                    tConfiguracionMesa cm = new tConfiguracionMesaBL().GetByIdUsuario(U.Id);
                    cRFC rfc = new cRFC();
                    //rfc = new cRfcBL().GetRfcByIdpredio(Predio.IdContribuyente);
                    rfc = new cRfcBL().GetRfcByIdcontribuyente(Predio.IdContribuyente);
                    if (cm != null)
                    {
                        idMesaCobro = cm.IdMesa;
                    }
                    else
                    {
                        throw new System.ArgumentException(new Utileria().GetDescription("Imposible generar el cobro, cajero no configurado"));
                    }

                    //Para el Isabis(2) el nombre se todo del tramite
                    if (tramite.IdTipoTramite == 2 || (tramite.IdTipoTramite == 4 && tramite.NombreAdquiriente != null))
                    {//Para el Catastro(4) el nombre se todo del tramite si trae nombre Adquiriente
                        receptor.Nombre = (tramite.NombreAdquiriente == null || tramite.NombreAdquiriente == "" ? "" : tramite.NombreAdquiriente.Trim());
                        recibo.Contribuyente = (tramite.NombreAdquiriente == null  || tramite.NombreAdquiriente == "" ? "": tramite.NombreAdquiriente.Trim());
                    }
                    else
                    {
                        receptor.Nombre = (Predio.cContribuyente.ApellidoPaterno == null || Predio.cContribuyente.ApellidoPaterno == "" ? "" : Predio.cContribuyente.ApellidoPaterno.Trim())
                        + ' ' + (Predio.cContribuyente.ApellidoMaterno == null || Predio.cContribuyente.ApellidoMaterno == "" ? "" : Predio.cContribuyente.ApellidoMaterno.Trim())
                        + ' ' + (Predio.cContribuyente.Nombre == null || Predio.cContribuyente.Nombre == "" ? "" : Predio.cContribuyente.Nombre.Trim()) + ' ' + (Predio.cContribuyente.RazonSocial == null || Predio.cContribuyente.RazonSocial == "" ? "" : Predio.cContribuyente.RazonSocial.Trim());
                        recibo.Contribuyente = (Predio.cContribuyente.ApellidoPaterno == null || Predio.cContribuyente.ApellidoPaterno == "" ? "" : Predio.cContribuyente.ApellidoPaterno.Trim())
                        + ' ' + (Predio.cContribuyente.ApellidoMaterno == null || Predio.cContribuyente.ApellidoMaterno == "" ? "" : Predio.cContribuyente.ApellidoMaterno.Trim())
                        + ' ' + (Predio.cContribuyente.Nombre == null || Predio.cContribuyente.Nombre == "" ? "" : Predio.cContribuyente.Nombre.Trim()) + ' ' + (Predio.cContribuyente.RazonSocial == null || Predio.cContribuyente.RazonSocial == "" ? "" : Predio.cContribuyente.RazonSocial.Trim());
                    }

                    

                    recibo.IdCaja = new tConfiguracionMesaBL().GetByIdUsuario(U.Id).IdCaja;
                    recibo.FechaPago = DateTime.Now;
                    recibo.EstadoRecibo = "P";

                    recibo.Rfc = rfc.RFC;
                    recibo.Domicilio = (Predio.Calle == null || Predio.Calle=="" ?"" : Predio.Calle.Trim()) + " #" + 
                        (Predio.Numero == null || Predio.Numero =="" ?"" : Predio.Numero.Trim()) + ", COL. " + 
                        (Predio.cColonia.NombreColonia == null || Predio.cColonia.NombreColonia =="" ? "" :Predio.cColonia.NombreColonia.Trim()) + ", C.P. " +
                        (Predio.CP == null || Predio.CP =="" ?"" : Predio.CP.Trim())+", LOC. "+
                        (Predio.Localidad == null || Predio.Localidad == "" ? "": Predio.Localidad.Trim());
                    recibo.ImportePagado = importePagado;
                    recibo.ImporteNeto = importeNeto;
                    recibo.ImporteDescuento = importeDescuento;
                    recibo.MaquinaPago = HttpContext.Current.Request.Url.Host;// "---";
                    recibo.IdUsuarioCobra = U.Id;
                    //recibo.IdDescuento = 1;
                    recibo.IdMesaCobro = new tConfiguracionMesaBL().GetByIdUsuario(U.Id).IdMesa; //not null
                    recibo.IdTipoPago = listConcepto[0].Id; // Convert.ToInt32(ddlMetodoPago.SelectedValue);
                    recibo.IdTramite = tramite.Id;
                    recibo.IdFIEL = new cFIELBL().GetByActive().Id;
                    recibo.Serie = vSerie;
                    recibo.Ruta = "-";
                    recibo.IdMesa = idMesaCobro;
                    recibo.CodigoSeguridad = "-";
                    recibo.RND = 0;
                    recibo.DatosPredio = "Clave Catastral: " + Predio.ClavePredial.Substring(0, 4) + "-" + Predio.ClavePredial.Substring(4, 2) + "-" + Predio.ClavePredial.Substring(6, 3) + "-" + Predio.ClavePredial.Substring(9, 3) + "     Tipo predio: " + (Predio.cTipoPredio.Descripcion == null || Predio.cTipoPredio.Descripcion =="" ? "" : Predio.cTipoPredio.Descripcion.Trim()) + "     Super. Terreno: " + Predio.SuperficieTerreno + "m2.     Super. Const.: " + Predio.SuperficieConstruccion + "m2.    Base gravable: $" + Predio.ValorCatastral.ToString("#.##");
                    recibo.Observaciones = txtObservacion.Text;// "Pago del Predio";//+ Predio.ClavePredial;
                    recibo.Activo = true;
                    recibo.IdUsuario = U.Id;
                    recibo.FechaModificacion = DateTime.Now;
                    recibo.usoCfdi = rfc.UsoCFDI is null || rfc.UsoCFDI == "" ? "G03" : rfc.UsoCFDI;

                    //if (txtNumeroAprobacion.Text != "")
                    //{
                    //    recibo.NoAutorizacion = txtNumeroAprobacion.Text;
                    //}        
                    //if (txtNumeroTarjeta.Text != "" || txtCheque.Text != "")
                    //{
                    //    recibo.NoTipoPago = txtNumeroTarjeta.Text == "" ? txtCheque.Text : txtNumeroTarjeta.Text;
                    //}
                    if ( recibo.IdTipoPago > 1) //recibo.NoTipoPago = listConcepto[0].Transaccion ;
                    {
                        recibo.NoTipoPago = listConcepto[0].Transaccion;
                        recibo.NoAutorizacion = listConcepto[0].Institucion;
                    }

                    //Se realiza el detalle del recibo
                    foreach (GridViewRow row in grdCobros.Rows)
                    {

                        int idConcepto = Convert.ToInt32(grdCobros.DataKeys[row.RowIndex].Value.ToString());
                        cConcepto concepto = new cConceptoBL().GetByConstraint(idConcepto);
                        TextBox txtImporte = (TextBox)row.FindControl("txtImporte");
                        TextBox txtporcentaje = (TextBox)row.FindControl("txtPorcentaje");
                        TextBox txtdescuento = (TextBox)row.FindControl("txtDescuento");
                        TextBox txtcosto = (TextBox)row.FindControl("txtCosto");
                        Label lblConcepto = (Label)row.FindControl("lblConcepto");
                        Label lblConceptoRef = (Label)row.FindControl("lblIdConceptoP");

                        //ingresar trmaite de plano o tramite de certificado
                        int tipoPlano = Convert.ToInt32(new cParametroSistemaBL().GetValorByClave("Plano"));
                        if (concepto.IdTipoTramite == tipoPlano)  //7 plano
                        {
                            tTramite tramitePln = new tTramite();
                            tramitePln.Fecha = DateTime.Now;
                            tramitePln.IdPredio = Predio.Id;
                            tramitePln.IdTipoTramite = tipoPlano;
                            tramitePln.Status = "P";
                            tramitePln.IdUsuario = U.Id;
                            tramitePln.Activo = true;
                            tramitePln.FechaModificacion = DateTime.Now;
                            tramitePln.FechaOperacion = DateTime.Now;
                            tramitePln.SuperficieConstruccion = Predio.SuperficieConstruccion;
                            tramitePln.TerrenoComun = Predio.TerrenoComun;
                            tramitePln.SuperficieTerreno = Predio.SuperficieTerreno;
                            tramite.Observacion = "Recibo de pago "  + recibo.Id.ToString() + " Plano Catastra, Super. Terreno: " + Predio.SuperficieTerreno + "m2.  AC. "+ Predio.TerrenoComun + " m2.  Super. Const.: " + Predio.SuperficieConstruccion + "m2.    Base gravable: $" + Predio.ValorCatastral.ToString("#.##");
                            tramitePln.Tipo = "IP";
                            tramitePln.IdTramiteRef = tramite.Id;

                            msg = new tTramiteBL().Insert(tramitePln);
                            //vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                            if (msg != MensajesInterfaz.Ingreso)
                            {
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                            }
                        }

                        if (concepto.IdTipoTramite == 1)  // 1 certificado
                        {
                            tTramite tramiteCerti = new tTramite();
                            tramiteCerti.Fecha = DateTime.Now;
                            tramiteCerti.IdPredio = Predio.Id;
                            tramiteCerti.IdTipoTramite = Convert.ToInt32(new cParametroSistemaBL().GetValorByClave("Certificado"));
                            tramiteCerti.Status = "P";
                            tramiteCerti.IdUsuario = U.Id;
                            tramiteCerti.Activo = true;
                            tramiteCerti.FechaModificacion = DateTime.Now;
                            tramiteCerti.FechaOperacion = DateTime.Now;   
                            //tramiteCerti.Adeudo = Convert.ToDecimal(ViewState["importe"].ToString());
                            tramiteCerti.SuperficieConstruccion = Predio.SuperficieConstruccion;
                            tramiteCerti.SuperficieTerreno = Predio.SuperficieTerreno;
                            tramiteCerti.BaseGravable = Predio.ValorCatastral;
                            tramiteCerti.BimestreInicial = Predio.BimestreFinIp;
                            tramiteCerti.EjercicioInicial = Predio.AaFinalIp;
                            tramiteCerti.Tipo = "IP";
                            tramiteCerti.IdTramiteRef = tramite.Id;                           
                            tramiteCerti.BimestreFinal = ObtenerBimestre(DateTime.Now.Month);
                            tramiteCerti.EjercicioFinal = DateTime.Now.Year;
                            msg = new tTramiteBL().Insert(tramiteCerti);

                            if (msg != MensajesInterfaz.Ingreso)
                            {
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                            }

                        }

                        tReciboDetalle reciboDetalle = new tReciboDetalle();
                        reciboDetalle.IdRecibo = recibo.Id;
                        reciboDetalle.IdConcepto = idConcepto;
                        reciboDetalle.IdControlSistema = 1;//SIN ID
                        reciboDetalle.ImporteNeto = Convert.ToDecimal(txtcosto.Text.Replace("$", ""));
                        reciboDetalle.ImporteDescuento = Convert.ToDecimal(txtdescuento.Text.Replace("$", ""));
                        reciboDetalle.ImporteAdicional = 0;
                        reciboDetalle.ImporteTotal = Convert.ToDecimal(txtImporte.Text.Replace("$", ""));
                        reciboDetalle.Cantidad = 1;
                        reciboDetalle.DescuentoDecretoPorcentaje = 0;
                        reciboDetalle.DescuentoEspecialPorcentaje = 0;
                        reciboDetalle.Activo = true;
                        reciboDetalle.FechaModificacion = DateTime.Now;
                        reciboDetalle.IdUsuario = U.Id;
                        reciboDetalle.IdConceptoRef = Convert.ToInt32(lblConceptoRef.Text);
                        recibo.tReciboDetalle.Add(reciboDetalle);

                        Conceptos c = new Conceptos();
                        c.Cantidad = 1;
                        c.CuentaPredial = Predio.ClavePredial;
                        switch (tramite.IdTipoTramite)
                        {
                            case 1://	Certificados
                            case 4://	Catastro                            
                            case 7://	Plano 
                            case 3://   Convenio
                                c.Descripcion = lblConcepto.Text;
                                break;
                            case 2://	Isabis

                                if (concepto.EsAdicional == false && idConcepto == Convert.ToInt32(ViewState["idCriIsabis"]))
                                {
                                    c.Descripcion = lblConcepto.Text + "\n\n TIPO DE AVALUO: " + tramite.cTipoAvaluo.Descripcion +
                                    ", NÚMERO DE ESCRITURA: " + tramite.NumeroEscritura + ", FECHA DE OPERACIÓN: " + tramite.FechaOperacion +
                                    "Y VALOR MAS ALTO: " + tramite.ValorMasAlto;
                                }
                                else
                                {
                                    c.Descripcion = lblConcepto.Text;
                                }

                                break;
                                //case 3://   Convenio
                                //     concepto = new cConceptoBL().GetByConstraint(idConcepto);
                                //     if ((tramite.tConvenio.ConDetalle == "N" && concepto.EsAdicional == false) || 
                                //         (tramite.tConvenio.ConDetalle == "S" && Convert.ToInt32(lblConceptoRef.Text)==0))
                                //     {
                                //         c.Descripcion = lblConcepto.Text + "\n\n CONVENIO NÚMERO " + tramite.tConvenio.Folio +
                                //        " PARCIALIDAD " + tramite.NoParcialidad + " DE " + tramite.tConvenio.NoParcialidades;
                                //    }
                                //     else
                                //     {
                                //         c.Descripcion = lblConcepto.Text;
                                //     }
                                //    break;
                        }

                        c.Id = concepto.Cri;
                        c.Importe = Convert.ToDouble(reciboDetalle.ImporteNeto);
                        c.Unidad = "N/A";
                        c.ValorUnitario = Convert.ToDouble(reciboDetalle.ImporteNeto);
                        c.ClaveProdServ = concepto.cProdServ.ClaveProdServ;
                        c.ClaveUnidad = concepto.cUnidadMedida.ClaveUnidad;
                        c.CuentaPredial = (Predio.ClavePredial == null || Predio.ClavePredial =="" ? "" : Predio.ClavePredial.Trim());
                        c.Descuento = Convert.ToDouble(reciboDetalle.ImporteDescuento);
                        //lConceptos.Add(c);
                        ConceptosRec conceptlist = new ConceptosRec();

                        conceptlist.Id = c.CuentaPredial;
                        conceptlist.Importe = c.Importe;
                        conceptlist.Unidad = c.Unidad;
                        conceptlist.ValorUnitario = c.ValorUnitario;
                        conceptlist.ClaveProdServ = c.ClaveProdServ;
                        conceptlist.ClaveUnidadMedida = c.ClaveUnidad;
                        conceptlist.CuentaPredial = c.CuentaPredial;
                        conceptlist.Descripcion = c.Descripcion;
                        conceptlist.Descuento = c.Descuento;


                        Catastro.ModelosFactura.Concepto conceptoslist = new Catastro.ModelosFactura.Concepto();
                        conceptoslist.Cantidad = Convert.ToInt64(c.Cantidad);
                        conceptoslist.Descripcion = c.Descripcion;
                        conceptoslist.Importe = this.formatearDecimales(c.Importe.ToString());
                        conceptoslist.ValorUnitario = this.formatearDecimales(c.ValorUnitario.ToString());
                        conceptoslist.NoIdentificacion = c.Id;
                        conceptoslist.ClaveUnidad = c.ClaveUnidad;
                        conceptoslist.ClaveProdServ = c.ClaveProdServ;
                        conceptoslist.Descuento = this.formatearDecimales(c.Descuento.ToString());
                        lConceptos.Add(conceptoslist);
                        lconceptos.Add(conceptlist);
                    }
                    new cErrorBL().insertcError("Cobro.aspx", "Inserta recibo linea 485, clave predial:" + Predio.ClavePredial);
                    msg = new tReciboBL().Insert(recibo);
                    if (msg != MensajesInterfaz.Ingreso)
                    {
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    }

                    if (listConcepto.Count > 0)
                    {
                        tReciboMetodoPago m = new tReciboMetodoPago();
                        cTipoPago c = new cTipoPago();
                        foreach (MetodoGrid v in listConcepto)
                        {
                            m = new tReciboMetodoPago();
                            m.IdRecibo = recibo.Id;
                            m.Banco = v.Institucion;
                            m.NumeroCheque = v.Transaccion;
                            m.Cuenta = v.Clave;
                            m.Importe = v.Importe;
                            m.IdTipopago = v.Id;
                            m.Activo = true;
                            m.IdUsuario = U.Id; ;
                            m.FechaModificacion = DateTime.Now; 
                            msg = new tReciboMetodoPagoBL().Insert(m);
                            //if (msg != MensajesInterfaz.Ingreso)
                            //{
                            //    throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                            //}

                            try
                            {
                                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                                conn.Open();

                                SqlCommand cmd1 = new SqlCommand(); // Create a object of SqlCommand class
                                cmd1.Connection = conn; //Pass the connection object to Command

                                string qry = "INSERT INTO dbo.tReciboMetodoPago(IdRecibo, Banco, NumeroCheque, Cuenta, Importe, IdTipopago, Activo, IdUsuario, FechaModificacion) " +
                                              " VALUES(@idRecibo, @banco, @numeroCheque, @cuenta, @importe, @idTipopago, @activo, @idUsuario,getdate()) ; ";
                                cmd1.CommandText = qry;
                                cmd1.Parameters.AddWithValue("@idRecibo", recibo.Id);
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


                    //COMPROBANTE using GeneraRecibo;
                    comprobante.cajero = U.Nombre + " " + U.ApellidoPaterno + " " + U.ApellidoMaterno;
                    //comprobante.FormaDePago = new cTipoPagoBL().GetByConstraint(Convert.ToInt32(ddlMetodoPago.SelectedItem.Value)).Clave;
                    comprobante.FormaDePago = listConcepto[0].Clave; //se cambia esta linea para tomar el metodo de pago de la lista de metodos de pago
                    comprobante.MetodoDePago = "PUE";// ddlMetodoPago.SelectedItem.Text; ;
                    comprobante.motivoDescuento = "";
                    comprobante.Serie = vSerie;
                    comprobante.Mesa = idMesaCobro.ToString();
                    comprobante.TipoDeComprobante = "I";
                    comprobante.Folio = recibo.Id.ToString().PadLeft(6, '0');
                    comprobante.SubTotal = Convert.ToDouble(importeNeto);
                    comprobante.descuento = Convert.ToDouble(importeDescuento);
                    comprobante.Total = Convert.ToDouble(importePagado);
                    comprobante.DatosPredio = "Clave Catastral: " + Predio.ClavePredial.Substring(0, 4) + "-" + Predio.ClavePredial.Substring(4, 2) + "-" + Predio.ClavePredial.Substring(6, 3) + "-" + Predio.ClavePredial.Substring(9, 3) + "     Tipo predio: " + (Predio.cTipoPredio.Descripcion == null || Predio.cTipoPredio.Descripcion == "" ?"" :Predio.cTipoPredio.Descripcion.Trim()) + "     Super. Terreno: " + Predio.SuperficieTerreno + "m2.     Super. Const.: " + Predio.SuperficieConstruccion + "m2.    Base gravable: $" + Predio.ValorCatastral.ToString("#.##");
                    comprobante.Observaciones = (txtObservacion.Text == null || txtObservacion.Text==""?"":txtObservacion.Text.Trim());

                    //DATOS RECEPTOR   
                    //receptor.Nombre =  Predio.cContribuyente.ApellidoPaterno + ' ' + Predio.cContribuyente.ApellidoMaterno + " "+Predio.cContribuyente.Nombre ;
                    //receptor.Calle = Predio.Calle + ' ' + Predio.Numero + ' ' + Predio.cColonia.NombreColonia + ' ' + Predio.Localidad;
                    //receptor.CodigoPostal = Predio.CP;
                    //receptor.Colonia = Predio.cColonia.NombreColonia;
                    //receptor.Estado = new cParametroSistemaBL().GetValorByClave("ESTADO").ToString();
                    //receptor.Localidad = Predio.Localidad;
                    //receptor.Municipio = new cParametroSistemaBL().GetValorByClave("NOMBRE_MUNICIPIO").ToString();
                    //receptor.NoExterior = Predio.Numero;
                    //receptor.NoInterior = " ";
                    //receptor.Pais = "MEXICO";
                    //receptor.Referencia = Predio.ClavePredial;
                    receptor.RFC = rfc.RFC == null ? "XAXX010101000" : rfc.RFC;
                    receptor.email = Predio.cContribuyente.Email;
                    //receptor.Nombre = Predio.cContribuyente; el nombre completo se asigno lineas arriba
                    receptor.Id = Predio.Id;
                    receptor.UsoCFDI = "P01";
                    receptor.Domicilio = recibo.Domicilio;

                    //GENERA RECIBO 
                    //Recibo rec = new Recibo();
                    //string path;
                    //ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
                    //path = Server.MapPath("~/");
                    //rec = reciboCFDI.generaRecibo(datosRecibo, path);

                    //if (rec.Errores != null)
                    //{
                    //    if (rec.Errores.Length > 0)
                    //    {
                    //        throw new System.ArgumentException(new Utileria().GetDescription(rec.Errores), "Error");
                    //    }
                    //}

                    ////ACTUALIZA EL RECIBO CON LOS DATOS GENERADOS POR LA FUNCION GENERA RECIBO
                    //recibo.CodigoSeguridad = rec.codigoReimpresion;
                    //recibo.Ruta = rec.Ruta;
                    //recibo.RND = Convert.ToByte(rec.RND);

                    //msg = new tReciboBL().Update(recibo);
                    //if (msg != MensajesInterfaz.Actualizacion)
                    //    throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                    //if (requerimiento != null)
                    //{
                    //    requerimiento.Recibo = recibo.Id;
                    //    msg = new tRequerimientoBL().Update(requerimiento);
                    //    if (msg != MensajesInterfaz.Actualizacion)
                    //    {
                    //        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    //    }
                    //}
                    //FIN DE LA TRANSACCION 
                    scope.Complete();
                    ViewState["IdRecibo"] = recibo.Id;
                    ////VISUALIZA EL RECIBO
                    //FileStream newFile = new FileStream(path + "/Recibos/Pdf/" + rec.codigoReimpresion + ".pdf", FileMode.Create);
                    //newFile.Write(rec.pdfBytes, 0, rec.pdfBytes.Length);
                    //newFile.Close();
                    ////pnl_Modal.Hide();
                    //frameRecibo.Src = "~/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf";
                    //btnSiguiente.Visible = true;
                    //modalRecibo.Show();
                    //String Clientscript = "printPdf();";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", Clientscript, true);
                    
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
                //string noCertificadoEmisor = guardarJson("No Certificado: "+comprobanteFactura.NoCertificado);
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

                    //string jsonRespuesta = guardarJson(rt.message);

                    if (rt.code.Equals("200"))
                    {
                        xmlPath = tEx.crearXMLTimbrado(rt.data,U.Usuario);

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

                            selloPath = tEx.generarSelloQR(folioFiscal, factura.Comprobante.Emisor.Rfc, factura.Comprobante.Receptor.Rfc, factura.Comprobante.Total.Trim(), selloComprobante,U.Usuario);

                            con.Open();

                            SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                            cmd.Connection = con; //Pass the connection object to Command

                            query = "INSERT INTO tReciboFactura (IdPredio,FolioFiscal,CertificadoEmisor,CertificadoSAT,FechahoraCertificado,SelloCDFI,selloSAT,Cadena,Fechafactura,NumCaja,selloDigital,xml,IdRecibo)  VALUES " +
                                "(@IdPredio, @folioFiscal, @CertificadoEmisor, @CertificadoSAT, @FechahoraCertificado, @SelloCDFI, @selloSAT, @Cadena, @Fechafactura, @NumCaja, @selloDigital, @xml, @IdRecibo) ; ";
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@IdPredio", tramite.IdPredio);
                            cmd.Parameters.AddWithValue("@folioFiscal", folioFiscal);
                            cmd.Parameters.AddWithValue("@CertificadoEmisor", noCertificado);
                            cmd.Parameters.AddWithValue("@CertificadoSAT", noCertificadoSAT);
                            cmd.Parameters.AddWithValue("@FechahoraCertificado", fechaTimbrado);
                            cmd.Parameters.AddWithValue("@SelloCDFI", selloCFDI);
                            cmd.Parameters.AddWithValue("@selloSAT", selloSAT);
                            cmd.Parameters.AddWithValue("@Cadena", cadenaOriginalCFDI);
                            cmd.Parameters.AddWithValue("@Fechafactura", fechaF);
                            cmd.Parameters.AddWithValue("@NumCaja", recibo.IdCaja);
                            cmd.Parameters.AddWithValue("@selloDigital", selloComprobante);
                            cmd.Parameters.AddWithValue("@xml", newXml.Value);
                            cmd.Parameters.AddWithValue("@IdRecibo", recibo.Id);

                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            query = "update tRecibo set Facturado=@Facturado where Id=@IdRecibo; ";
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@Facturado", "1");
                            cmd.Parameters.AddWithValue("@IdRecibo", recibo.Id);
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

                    /////Termina nueva Forma de Factura

                }

                //Genera RECIBO
                ResultDAO resultPDF = new ResultDAO();
                ClienteBean clienteBean = new ClienteBean();
                PagoBean pagoBean = new PagoBean();
                FacturaBean facturaBean = new FacturaBean();

                cRFC rfc2 = new cRFC();
                //rfc = new cRfcBL().GetRfcByIdpredio(Predio.IdContribuyente);
                rfc2 = new cRfcBL().GetRfcByIdcontribuyente(Predio.IdContribuyente);

                Predio.ClavePredial = Predio.ClavePredial == "" || Predio.ClavePredial == null ? "" : Predio.ClavePredial;

                if (Predio.ClavePredial == "" || Predio.ClavePredial == null)
                {
                    clienteBean.NumContrato = Predio.ClaveAnterior == "" || Predio.ClaveAnterior == null ? "" : Predio.ClaveAnterior.Substring(0, 4) + "-" + Predio.ClaveAnterior.Substring(4, 2) + "-" + Predio.ClaveAnterior.Substring(6, 3) + "-" + Predio.ClaveAnterior.Substring(9, 3);
                }
                else
                {
                    if (Predio.ClavePredial.Substring(0, 1) == "0") //solo si se refiere a una cuenta, la clave empieza con 0
                    {
                        clienteBean.NumContrato = Predio.ClaveAnterior == "" || Predio.ClaveAnterior == null ? "" : Predio.ClaveAnterior.Substring(0, 4) + "-" + Predio.ClaveAnterior.Substring(4, 2) + "-" + Predio.ClaveAnterior.Substring(6, 3) + "-" + Predio.ClaveAnterior.Substring(9, 3);
                    }
                    else { clienteBean.NumContrato = Predio.ClavePredial.Substring(0, 4) + "-" + Predio.ClavePredial.Substring(4, 2) + "-" + Predio.ClavePredial.Substring(6, 3) + "-" + Predio.ClavePredial.Substring(9, 3); }

                }
                string domimicilioContribuyente = (Predio.cContribuyente.Calle is null || Predio.cContribuyente.Calle == "" ? "" : Predio.cContribuyente.Calle.Trim()) +" #"+
                       (Predio.cContribuyente.Numero is null || Predio.cContribuyente.Numero == "" ? "" : Predio.cContribuyente.Numero.Trim()) + " COL. "+
                       (Predio.cContribuyente.Colonia is null || Predio.cContribuyente.Colonia == "" ? "" : Predio.cContribuyente.Colonia.Trim()) +" C.P. "+
                       (Predio.cContribuyente.CP is null || Predio.cContribuyente.CP =="" ? "": Predio.cContribuyente.CP.Trim())+ ", "+
                       (Predio.cContribuyente.Municipio is null || Predio.cContribuyente.Municipio == "" ? "" : Predio.cContribuyente.Municipio.Trim());

                clienteBean.Rfc = receptor.RFC;
                clienteBean.Direccion = domimicilioContribuyente is null || domimicilioContribuyente == "" ? recibo.Domicilio : domimicilioContribuyente;
                clienteBean.DireccionPredio = recibo.Domicilio;
                clienteBean.CorreoElectronico = receptor.email;
                clienteBean.NombreCompleto = receptor.Nombre;
                clienteBean.Giro = Predio.cTipoPredio.Descripcion;
                clienteBean.ClaveGiro = Predio.cTipoPredio.Id.ToString();
                clienteBean.ClaveCatastral = (Predio.ClavePredial == null || Predio.ClavePredial == "" ? "" : Predio.ClavePredial.Substring(0, 1) == "0" ? "" : Predio.ClavePredial.Substring(0, 4) + "-" + Predio.ClavePredial.Substring(4, 2) + "-" + Predio.ClavePredial.Substring(6, 3) + "-" + Predio.ClavePredial.Substring(9, 3));  //Validar con si tiene 0 vacia
                clienteBean.CuentaCatastral = (Predio.ClaveAnterior == null || Predio.ClaveAnterior == "" ? "" : Predio.ClaveAnterior.Substring(0, 4) + "-" + Predio.ClaveAnterior.Substring(4, 2) + "-" + Predio.ClaveAnterior.Substring(6, 3) + "-" + Predio.ClaveAnterior.Substring(9, 3));
                clienteBean.TipoPredio = Predio.cTipoPredio.Descripcion;
                clienteBean.SuperficieTerreno = Predio.SuperficieTerreno.ToString();
                clienteBean.AreaComun = Predio.TerrenoComun.ToString();
                clienteBean.SuperficieAreaConstruccion = Predio.SuperficieConstruccion.ToString();
                clienteBean.BaseImpuesto = Convert.ToDecimal(tramite.BaseGravable).ToString("C"); // predio.cBaseGravable.ToString();
                clienteBean.Referencia = Predio.Referencia;
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
                facturaBean.UsoCfdi = rfc2.UsoCFDI is null || rfc2.UsoCFDI =="" ? "G03" : rfc2.UsoCFDI;

                string hoy = DateTime.Now.ToString(Constantes.formatoFechaArchivos);
                string reciboPath = Server.MapPath("~/Temporales");

                reciboPath += Constantes.recibosSistemaFolder;
                if (!System.IO.Directory.Exists(reciboPath))
                {
                    System.IO.Directory.CreateDirectory(reciboPath);
                }
                reciboPath += U.Usuario + "-" + pagoBean.NumRecibo + "-" + hoy + "-RECIBO.pdf";



                if (PDF.crearRecibo(clienteBean, pagoBean, facturaBean, (!folioFiscal.Equals("")), reciboPath, selloPath, lconceptos))
                {
                    resultPDF.SUCCESS = true;
                    resultPDF.PARAM_AUX = reciboPath;


                    //List<string> archivos = new List<string>();
                    //archivos.Add(reciboPath);
                    //archivos.Add(xmlPath);
                    //archivos.Add(selloPath);
                    ////ResultDAO resultEMAIL = (new Mail()).enviarHotmail(
                    ////    clienteBean.CorreoElectronico,
                    ////    "Factura de pago del servicio del agua potable.\n  Número de recibo: " + recibo,
                    ////    "Factura",
                    ////    archivos);

                    //////VISUALIZA EL RECIBO
                    //FileStream newFile = new FileStream(reciboPath , FileMode.Create);
                    ////newFile.Write(rec.pdfBytes, 0, rec.pdfBytes.Length);
                    //newFile.Close();
                    //////pnl_Modal.Hide();
                    //frameRecibo.Src = reciboPath; // "~/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf";
                    //btnSiguiente.Visible = true;
                    //modalRecibo.Show();
                    //String Clientscript = "printPdf();";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", Clientscript, true);

                }
                else
                {
                    resultPDF.SUCCESS = false;
                }

                frameRecibo.Src = "~/Temporales/sipred-files/Recibos expedidos/" + U.Usuario + "-" + pagoBean.NumRecibo + "-" + hoy + "-RECIBO.pdf";
                //btnSiguiente.Visible = true;
                modalRecibo.Show();
                //String Clientscript = "printPdf();";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", Clientscript, true);
                ViewState["IdRecibo"] = recibo.Id;
                ViewState["pdf"] = "";
            }
            catch (Exception error)
            {
                new Utileria().logError("Cobros.cobrarGenerarRecibo.Exception", error);
                vtnModal.ShowPopup(new Utileria().GetDescription(error.Message), ModalPopupMensaje.TypeMesssage.Alert);


            }
        }
        //Archivo existe
        public string guardarJson(string json)
        {
            bool result = File.Exists(Constantes.jsonEnviado);
            if (result == true)
            {
                string Text = json;
                using (StreamWriter writetext = File.AppendText(Constantes.jsonEnviado))
                {
                    writetext.WriteLine(Text);
                }
                return "OK";
            }
            else
            {
                using (StreamWriter mylogs = File.AppendText(Constantes.jsonEnviado))         //se crea el archivo
                {
                    //se adiciona alguna información y la fecha
                    DateTime dateTime = new DateTime();
                    dateTime = DateTime.Now;
                    mylogs.WriteLine(json);
                    mylogs.Close();
                }
                return "OK";
            }
            return "NO";
        }
        //CANTIDAD EN LETRA
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

        //protected void btnCancelarSeleccion_Click(object sender, EventArgs e)
        //{
        //    pnl_Modal.Hide();
        //    lblTotal.Text = "";
        //    //lbltotalSimbolo.Visible = false;
        //    txtClvCastatral.Enabled = false;
        //    //btnNuevotramitre.Visible = true;
        //}
        //protected void btnNuevotramitre_Click(object sender, EventArgs e)
        //{
        //    lblTotal.Text = "";
        //    //lbltotalSimbolo.Visible = false;
        //    txtClvCastatral.Text = "";
        //    txtPropietario.Text = "";
        //    activaEtiquetasModal(0, true);
        //    llenaTram();
        //    limpiaCampos();
        //    txtClvCastatral.Focus();
        //    txtClvCastatral.Enabled = false;
        //    //btnNuevotramitre.Visible = false;
        //}
        protected void cargarCajero()/*btnTipoTramite_Click (object sender, EventArgs e)*/
        {

            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            txtCajero.Text = U.NoEmpleado.ToString();
        }
        //protected void llenaGrid(Int32 id)
        //{
        //    switch (Convert.ToInt32(ddlTipoTramite.SelectedValue))
        //    {
        //        case 1:
        //        case 2:
        //        case 3:
        //            llenaGridTramites(id);
        //            break;
        //        case 4:
        //        case 7:
        //            llenaGridConceptos(id, Convert.ToDecimal("0"));
        //            break;
        //    }
        //}

        private void llenaGridConceptos(Int32 idTramite)//, Decimal NSalarioImporte)
        {
            ViewState["TipoTramite"] = ddlTipoTramite.SelectedIndex;
            ViewState["Importe"] = "0";
            ViewState["idT"] = idTramite;
            grdCobros.Visible = true;
            DateTime d = DateTime.Now;
            int tt = int.Parse(ddlTipoTramite.SelectedValue);
            int idConceptoP = 0;
            //cConcepto concepto = new cConceptoBL().GetListByConstraintEjercicio(idConcepto, Convert.ToInt32(d.Year));
            cSalarioMinimo s = new cSalarioMinimoBL().GetByEjercicio(Convert.ToInt32(d.Year));
            //List<cConcepto> listconcepto =new tTramiteDetalleBL().GetAllIdTramite(idTramite);
            tTramite tramite = new tTramiteBL().GetByConstraint(idTramite);
            ViewState["observaciones"] = tramite.Observacion == null ? "" : tramite.Observacion;
            List<tTramiteDetalle> listTramiteDetalle = new tTramiteDetalleBL().GetAllIdTramite(idTramite);
            List<conceptoGrid> listConcepto = new List<conceptoGrid>();
            if (listTramiteDetalle.Count > 0)
            {
                if (s != null)
                {
                    foreach (tTramiteDetalle td in listTramiteDetalle)
                    {
                        cConcepto concepto = td.cConcepto;
                        bool add = false; bool TipoS = false; decimal costoI = 0; //bool cerrar = true;                    
                        conceptoGrid conceptoAux = new conceptoGrid();
                        conceptoAux.Id = concepto.Id.ToString();
                        conceptoAux.IdMesa = concepto.IdMesa.ToString();
                        conceptoAux.Concepto = concepto.Nombre;
                        conceptoAux.IdTramite = "0";
                        conceptoAux.TipoCobro = concepto.TipoCobro;
                        conceptoAux.IdConceptoP = "0";
                        int idcConcepto = concepto.Id;
                        String costo = "";
                        idConceptoP = concepto.EsAdicional == false ? concepto.Id : 0;
                        if (concepto.TipoCobro == "S")
                        {
                            add = concepto.Adicional == true ? true : false;
                            TipoS = true;
                            costoI = s.Importe * concepto.SalarioMax;
                            costo = Convert.ToString(Utileria.Redondeo(costoI));
                            conceptoAux.Costo = costo;
                            conceptoAux.Porcentaje = "0";
                            conceptoAux.Descuento = "0";
                            conceptoAux.Importe = costo;
                            if (concepto.SinDescuento == true)
                            {
                                tPrediosDescuento preddesc = new tPrediosDescuentoBL().GetForCobro(concepto.Id, Convert.ToInt32(ViewState["idP"].ToString()), Convert.ToInt32(ViewState["idT"].ToString()), d);
                                if (preddesc != null)
                                {

                                    decimal pI = Convert.ToDecimal(preddesc.Porcentaje);
                                    decimal desc = Utileria.Redondeo(pI * (costoI / 100));
                                    decimal import = Utileria.Redondeo(costoI - desc);
                                    conceptoAux.Porcentaje = Convert.ToString(Utileria.Redondeo(preddesc.Porcentaje));
                                    conceptoAux.Descuento = Convert.ToString(Utileria.Redondeo(desc));
                                    conceptoAux.Importe = Convert.ToString(Utileria.Redondeo(import));

                                }
                            }
                        }
                        if (concepto.TipoCobro == "P")
                        {
                            if (TipoS == true)
                            {
                                decimal pI = Convert.ToDecimal(concepto.Importe);
                                decimal desc = Utileria.Redondeo((pI * (costoI / 100)));
                                costo = Convert.ToString(Utileria.Redondeo(desc));
                                conceptoAux.Costo = costo;
                                conceptoAux.Porcentaje = "0";
                                conceptoAux.Descuento = "0";
                                conceptoAux.Importe = costo;
                            }
                            else
                            {
                                conceptoAux.Porcentaje = concepto.Importe.ToString();
                            }
                        }
                        if (concepto.TipoCobro == "I")
                        {
                            costoI = Convert.ToDecimal(td.Importe);
                            conceptoAux.Costo = td.Importe.ToString();
                            conceptoAux.Porcentaje = "0";
                            conceptoAux.Descuento = "0";
                            conceptoAux.Importe = td.Importe.ToString();
                            conceptoAux.TipoCobro = "S";
                            if (concepto.SinDescuento == true)
                            {
                                tPrediosDescuento preddesc = new tPrediosDescuentoBL().GetForCobro(concepto.Id, Convert.ToInt32(ViewState["idP"].ToString()), Convert.ToInt32(ViewState["idT"].ToString()), d);
                                if (preddesc != null)
                                {
                                    decimal pI = Convert.ToDecimal(preddesc.Porcentaje);
                                    decimal desc = Utileria.Redondeo((pI * (Convert.ToDecimal(td.Importe) / 100)));
                                    decimal import = Utileria.Redondeo(costoI - desc);
                                    costo = Convert.ToString(Utileria.Redondeo(desc));
                                    conceptoAux.Porcentaje = Convert.ToString(Utileria.Redondeo(preddesc.Porcentaje));
                                    conceptoAux.Descuento = Convert.ToString(Utileria.Redondeo(desc));
                                    conceptoAux.Importe = Convert.ToString(Utileria.Redondeo(import));
                                }
                            }
                        }
                        listConcepto.Add(conceptoAux);
                        if (concepto.Adicional == true)
                        {
                            cConcepto addic = new cConceptoBL().AdicionalMesaCatastro(Convert.ToInt32(d.Year), Convert.ToInt32(3));
                            if (addic != null)
                            {
                                conceptoGrid cx = new conceptoGrid();
                                cx.IdConceptoP = idConceptoP == 0 ? "0" : idConceptoP.ToString();
                                cx.Id = addic.Id.ToString();
                                cx.IdMesa = addic.IdMesa.ToString();
                                cx.IdTramite = "0";
                                cx.TipoCobro = addic.TipoCobro;
                                decimal ouio = Utileria.Redondeo(Convert.ToDecimal(conceptoAux.Costo) * (Convert.ToDecimal(addic.Importe) / 100));
                                cx.Costo = Convert.ToString(ouio);
                                if (addic.TipoCobro == "P")
                                {
                                    cx.Porcentaje = addic.Importe.ToString();
                                }
                                else
                                {
                                    cx.Porcentaje = "0";
                                }
                                cx.Descuento = "0";
                                cx.Importe = cx.Costo;
                                cx.Concepto = addic.Descripcion;
                                listConcepto.Add(cx);
                            }
                        }

                    }

                    listConcepto = llenarListaConcepto(listConcepto);
                    if (listConcepto != null)
                    {
                        grdCobros.DataSource = listConcepto;
                        grdCobros.DataBind();
                        cerrarGrid();

                    }
                    else
                    {
                        //btnRecalculo.Enabled = false;
                        vtnModal.ShowPopup(new Utileria().GetDescription("Imposible agregar el concepto ya se encuentra en la lista"), ModalPopupMensaje.TypeMesssage.Alert);
                    }

                }
                else
                {
                    //btnRecalculo.Enabled = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription("No se Encuentra UMA del Ejercicio Actual"), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            else
            {
                //btnRecalculo.Enabled = false;
                vtnModal.ShowPopup(new Utileria().GetDescription("El tramite no cuenta con concepto de cobro activo"), ModalPopupMensaje.TypeMesssage.Alert);
            }
        }

        private void llenaGridConvenios(Int32 idTramite)
        {
            ViewState["TipoTramite"] = ddlTipoTramite.SelectedIndex;
            ViewState["Importe"] = "1";
            ViewState["idT"] = idTramite;
            grdCobros.Visible = true;
            DateTime d = DateTime.Now;
            int tt = int.Parse(ddlTipoTramite.SelectedValue);
            cConcepto traCo = null;

            tTramite tramite = new tTramiteBL().GetByConstraint(idTramite);
            ViewState["observaciones"] = tramite.Observacion == null ? "" : tramite.Observacion;
            if (tramite == null)
            {
                //btnRecalculo.Enabled = false;
                vtnModal.ShowPopup(new Utileria().GetDescription("Imposible agregar la mensualidad, el tramite no se encuentra"), ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            else //Valida la mensualidad de pago
            {
                int isPago = new tTramiteBL().validaMensualidadPagoConvenio(tramite.Id, tramite.IdConvenio, tramite.NoParcialidad);
                if (isPago == 0)
                {
                    //vtnModal.ShowPopup(new Utileria().GetDescription("Imposible agregar la mensualidad, Tiene  pagos pendientes"), ModalPopupMensaje.TypeMesssage.Alert);
                    //return;
                    lblMensajeConvenio.Text = "Este trámite no puede ser cobrado debido a que no lleva una secuencia en las parcialidades";
                    lblMensajeConvenio.Visible = true;
                }
                else
                {
                    lblMensajeConvenio.Text = "";
                    lblMensajeConvenio.Visible = false;
                    if (isPago == -1)
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ErrorGeneral), ModalPopupMensaje.TypeMesssage.Error);
                        return;
                    }
                }
            }

            List<conceptoGrid> listConcepto = new List<conceptoGrid>();
            int idcConcepto = 0;
            if (tramite.tConvenio.ConDetalle == "N")
            {
                String parametroSistema = new cParametroSistemaBL().GetValorByClave("CONVENIO");
                if (parametroSistema != null)
                {
                    traCo = new cConceptoBL().getConceptoByCriEjercicio(parametroSistema, Convert.ToInt32(d.Year), tt);
                }
                else
                {
                    //btnRecalculo.Enabled = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription("Imposible agregar la mensualidad, No se encuentra el valor del cri para el CONVENIO"), ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }

                if (traCo != null)
                {
                    //decimal costoI = 0;                     
                    conceptoGrid conceptoAux = new conceptoGrid();
                    conceptoAux.Id = traCo.Id.ToString();
                    conceptoAux.IdMesa = traCo.IdMesa.ToString();
                    conceptoAux.IdTramite = tramite.Id.ToString();
                    conceptoAux.TipoCobro = traCo.TipoCobro;
                    conceptoAux.IdConceptoP = "0";
                    idcConcepto = traCo.Id;
                    //String costo = "";

                    if (traCo.EsAdicional == false)
                    {
                        conceptoAux.Concepto = traCo.Nombre + ". \n\n CONVENIO FOLIO: " + tramite.tConvenio.Folio +
                            ", PARCIALIDAD " + tramite.NoParcialidad + " DE " + tramite.tConvenio.NoParcialidades;
                    }
                    else
                    {
                        conceptoAux.Concepto = traCo.Nombre;
                    }

                    //costoI = Convert.ToDecimal(tramite.Mensualidad) + Convert.ToDecimal(tramite.Interes); //+ interes;
                    //costo = Convert.ToString(Utileria.Redondeo(costoI));
                    conceptoAux.Costo = Convert.ToString(Utileria.Redondeo(Convert.ToDecimal(tramite.Mensualidad) + Convert.ToDecimal(tramite.Interes)));
                    conceptoAux.Porcentaje = "0";
                    conceptoAux.Descuento = "0";
                    conceptoAux.Importe = conceptoAux.Costo;

                    btnRecalculo.Enabled = true;

                    listConcepto.Add(conceptoAux);
                }
                else
                {
                    //btnRecalculo.Enabled = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription("No se Encuentran Conceptos de Cobro del Ejercicio Actual"), ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }
            }
            else
            {
                List<tTramiteDetalle> listTramiteDetalle = new tTramiteDetalleBL().GetAllIdTramite(idTramite);
                if (listTramiteDetalle.Count > 0)
                {
                    foreach (tTramiteDetalle td in listTramiteDetalle)
                    {
                        conceptoGrid conceptoAux = new conceptoGrid();
                        cConcepto concepto = td.cConcepto;
                        conceptoAux.Id = concepto.Id.ToString();
                        conceptoAux.IdMesa = concepto.IdMesa.ToString();
                        conceptoAux.IdTramite = td.IdTramite.ToString();
                        conceptoAux.TipoCobro = "I";//concepto.TipoCobro;
                        conceptoAux.IdConceptoP = td.IdConceptoRef.ToString();
                        idcConcepto = td.IdConceptoRef == 0 ? 0 : Convert.ToInt32(td.IdConceptoRef);

                        if (td.IdConceptoRef == 0)
                        {
                            conceptoAux.Concepto = concepto.Nombre + ". \n\n CONVENIO FOLIO: " + tramite.tConvenio.Folio +
                                ", PARCIALIDAD " + tramite.NoParcialidad + " DE " + tramite.tConvenio.NoParcialidades;
                        }
                        else
                        {
                            conceptoAux.Concepto = concepto.Nombre;
                        }

                        conceptoAux.Costo = Convert.ToString(Utileria.Redondeo(Convert.ToDecimal(td.Importe)));
                        conceptoAux.Porcentaje = "0";
                        conceptoAux.Descuento = "0";
                        conceptoAux.Importe = conceptoAux.Costo;

                        btnRecalculo.Enabled = true;

                        listConcepto.Add(conceptoAux);
                    }

                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription("El tramite no cuenta con concepto de cobro activo"), ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }
            }
            //Calcula interes por pago Tardio
            if (d > tramite.Fecha)
            {
                decimal interes = 0;
                interes = new tTramiteBL().interesFaltaPagoConvenioIdTramite(tramite.Id);
                if (interes.Equals(-1))
                {
                    //btnRecalculo.Enabled = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription("Imposible agregar la mensualidad, No se ah podido calcular el interés por Falta de Pago"), ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }
                else
                {
                    if (interes > 0)
                    {
                        String convenioRecargo = new cParametroSistemaBL().GetValorByClave("CONVENIO_RECARGO");
                        if (convenioRecargo != null)
                        {
                            traCo = new cConceptoBL().getConceptoByCriEjercicio(convenioRecargo, Convert.ToInt32(d.Year), tt);
                            if (traCo != null)
                            {
                                conceptoGrid conceptoAux = new conceptoGrid();
                                conceptoAux.Id = traCo.Id.ToString();
                                conceptoAux.IdMesa = traCo.IdMesa.ToString();
                                conceptoAux.IdTramite = tramite.Id.ToString();
                                conceptoAux.TipoCobro = "I";
                                if (idcConcepto != 0)
                                {
                                    conceptoAux.IdConceptoP = idcConcepto.ToString();
                                }
                                conceptoAux.Costo = interes.ToString();
                                conceptoAux.Porcentaje = "0";
                                conceptoAux.Descuento = "0";
                                conceptoAux.Concepto = traCo.Nombre;
                                conceptoAux.Importe = interes.ToString();

                                List<tPrediosDescuento> predDesc = new tPrediosDescuentoBL().GetDescuentoCobro(idTramite, Convert.ToInt32(tramite.IdPredio), d);
                                tPrediosDescuento preddescR = predDesc.Where(p => p.IdConcepto == traCo.Id).FirstOrDefault();

                                if (preddescR != null)
                                {
                                    conceptoAux.Descuento = Utileria.Redondeo(preddescR.Porcentaje * (Convert.ToDecimal(conceptoAux.Importe) / 100)).ToString();
                                    conceptoAux.Importe = (Convert.ToDecimal(conceptoAux.Costo) - Convert.ToDecimal(conceptoAux.Descuento)).ToString();
                                    conceptoAux.Porcentaje = Convert.ToString(Utileria.Redondeo(preddescR.Porcentaje));
                                }

                                listConcepto.Add(conceptoAux);
                            }
                            else
                            {
                                //btnRecalculo.Enabled = false;
                                vtnModal.ShowPopup(new Utileria().GetDescription("No se Encuentran Conceptos de Cobro para el recargo por pago tardio del Ejercicio Actual"), ModalPopupMensaje.TypeMesssage.Alert);
                                return;
                            }
                        }
                        else
                        {
                            //btnRecalculo.Enabled = false;
                            vtnModal.ShowPopup(new Utileria().GetDescription("Imposible agregar la mensualidad, No se encuentra el valor del cri para el recargo por pago tardio"), ModalPopupMensaje.TypeMesssage.Alert);
                            return;
                        }
                    }
                }
            }

            listConcepto = llenarListaConcepto(listConcepto);
            if (listConcepto != null)
            {
                grdCobros.DataSource = listConcepto;
                grdCobros.DataBind();
                cerrarGrid();
            }
            else
            {
                //btnRecalculo.Enabled = false;
                vtnModal.ShowPopup(new Utileria().GetDescription("Imposible agregar el concepto ya se encuentra en la lista"), ModalPopupMensaje.TypeMesssage.Alert);
            }


        }

        private void activaPago(bool tipo)
        {
            btnCobrar.Visible = lblMensajeConvenio.Visible == true ? false : tipo;
            btnEstado.Visible = lblMensajeConvenio.Visible == true ? false : tipo;
            lblMetodoPago.Visible = tipo;
            ddlMetodoPago.Visible = tipo;
            //btnCobrar.Enabled = false;
        }

        private void cerrarGrid()
        {
            Decimal total = 0; Decimal costo = 0; Decimal descuento = 0; Decimal importeTotal = 0;//int porcentaje = 0;  int importe = 0; int Cost = 0;
            int Imp = 0; Imp = Convert.ToInt32(ViewState["Importe"].ToString());
            activaPago(true);
            foreach (GridViewRow row in grdCobros.Rows)
            {
                //costo = 0;  porcentaje = 0; descuento = 0; importe = 0;
                int idGrid = Convert.ToInt32(grdCobros.DataKeys[row.RowIndex].Value.ToString());
                TextBox txtImporte = (TextBox)row.FindControl("txtImporte");
                TextBox txtporcentaje = (TextBox)row.FindControl("txtPorcentaje");
                TextBox txtdescuento = (TextBox)row.FindControl("txtDescuento");
                TextBox txtcosto = (TextBox)row.FindControl("txtCosto");
                Label tipoCobro = (Label)row.FindControl("lblTipoCobro");
                Label idConcepto = (Label)row.FindControl("lblId");

                if (tipoCobro.Text == "S" || tipoCobro.Text == "P")
                {
                    if (Imp == 0)
                    {
                        txtcosto.Enabled = false;
                        txtdescuento.Enabled = false;
                        txtporcentaje.Enabled = false;
                        txtImporte.Enabled = false;

                        if (txtImporte.Text != "")
                        {
                            if (tipoCobro.Text == "S")
                            {
                                costo = costo + Convert.ToDecimal(txtImporte.Text.Replace("$", ""));
                            }
                        }
                    }
                    else
                    {
                        if (tipoCobro.Text == "P")
                        {
                            decimal pI = Convert.ToDecimal(txtporcentaje.Text);
                            decimal desc = Utileria.Redondeo(pI * (Convert.ToDecimal(costo) / 100));
                            // decimal import = Utileria.Redondeo(Convert.ToDecimal(costo) - desc, 2);
                            txtcosto.Text = Convert.ToString(Utileria.Redondeo(desc));
                            txtImporte.Text = Convert.ToString(Utileria.Redondeo(desc));
                            txtdescuento.Text = "0";
                        }
                        else
                        {
                            costo = costo + Convert.ToDecimal(txtcosto.Text.Replace("$", ""));
                        }


                    }

                    total = txtImporte.Text == "" ? total : total + Convert.ToDecimal(txtImporte.Text.Replace("$", ""));
                    importeTotal = txtcosto.Text == "" ? importeTotal : importeTotal + Convert.ToDecimal(txtcosto.Text.Replace("$", ""));
                    descuento = txtdescuento.Text == "" ? descuento : descuento + Convert.ToDecimal(txtdescuento.Text.Replace("$", ""));

                    txtImporte.Text = txtImporte.Text == "" ? "" : Utileria.Redondeo(Convert.ToDecimal(txtImporte.Text.Replace("$", ""))).ToString("C");
                    txtcosto.Text = txtcosto.Text == "" ? "" : Utileria.Redondeo(Convert.ToDecimal(txtcosto.Text.Replace("$", ""))).ToString("C");
                    txtdescuento.Text = txtdescuento.Text == "" ? "" : Utileria.Redondeo(Convert.ToDecimal(txtdescuento.Text.Replace("$", ""))).ToString("C");

                }
                else
                {
                    if (Imp == 1)
                    {
                        if (txtcosto.Text != "")
                        {
                            if (txtporcentaje.Text != "0")
                            {
                                decimal pI = Convert.ToDecimal(txtporcentaje.Text);
                                decimal desc = Utileria.Redondeo(pI * (Convert.ToDecimal(txtcosto.Text.Replace("$", "")) / 100));
                                decimal import = Utileria.Redondeo(Convert.ToDecimal(txtcosto.Text.Replace("$", "")) - desc);
                                txtdescuento.Text = Convert.ToString(Utileria.Redondeo(desc));
                                txtImporte.Text = Convert.ToString(Utileria.Redondeo(import));
                                costo = costo + Convert.ToDecimal(txtcosto.Text.Replace("$", ""));
                                //total = total + Convert.ToInt32(import);
                            }
                            else
                            {
                                //total = total + Convert.ToDecimal(txtcosto.Text);
                                costo = costo + Convert.ToDecimal(txtcosto.Text.Replace("$", ""));
                                txtImporte.Text = txtcosto.Text;
                            }
                            txtcosto.Enabled = false;
                            txtdescuento.Enabled = false;
                            txtporcentaje.Enabled = false;
                            txtImporte.Enabled = false;
                            btnRecalculo.Visible = false;
                        }
                        else
                        {
                            activaPago(false);
                            vtnModal.ShowPopup(new Utileria().GetDescription("Se necesitan todas los costos para hacer el calculo"), ModalPopupMensaje.TypeMesssage.Alert);
                        }
                    }
                    else
                    {
                        txtporcentaje.Text = txtporcentaje.Text == "" ? "0" : txtporcentaje.Text;
                        txtdescuento.Text = txtdescuento.Text == "" ? "0" : txtdescuento.Text;
                        txtImporte.Text = txtImporte.Text == "" ? "" : txtImporte.Text;
                        txtdescuento.Enabled = false;
                        txtporcentaje.Enabled = false;
                        txtImporte.Enabled = false;
                        if (Convert.ToInt32(ViewState["TipoTramite"]) == 2)
                        {
                            if ( //idConcepto.Text == ViewState["idCriIsabisRe"].ToString() ||
                            idConcepto.Text == ViewState["idCriIsabis"].ToString())
                            {
                                costo = costo + Convert.ToDecimal(txtcosto.Text.Replace("$", ""));
                                txtcosto.Enabled = false;
                                btnRecalculo.Visible = false;
                            }
                            else
                            {
                                activaPago(false);
                                btnRecalculo.Visible = true;
                            }
                        }
                        else
                        {
                            activaPago(false);
                            btnRecalculo.Visible = true;
                        }

                    }
                    total = txtImporte.Text == "" ? total : total + Convert.ToDecimal(txtImporte.Text.Replace("$", ""));
                    importeTotal = txtcosto.Text == "" ? importeTotal : importeTotal + Convert.ToDecimal(txtcosto.Text.Replace("$", ""));
                    descuento = txtdescuento.Text == "" ? descuento : descuento + Convert.ToDecimal(txtdescuento.Text.Replace("$", ""));

                    txtImporte.Text = txtImporte.Text == "" ? "" : Utileria.Redondeo(Convert.ToDecimal(txtImporte.Text.Replace("$", ""))).ToString("C");
                    txtcosto.Text = txtcosto.Text == "" ? "" : Utileria.Redondeo(Convert.ToDecimal(txtcosto.Text.Replace("$", ""))).ToString("C");
                    txtdescuento.Text = txtdescuento.Text == "" ? "" : Utileria.Redondeo(Convert.ToDecimal(txtdescuento.Text.Replace("$", ""))).ToString("C");
                }

            }
            ViewState["ImportePagado"] = total.ToString();
            ViewState["ImporteNeto"] = importeTotal.ToString();
            ViewState["ImporteDescuento"] = descuento.ToString();
            //lbltotalSimbolo.Visible = true;
            Label16.Visible = true;
            lblTotal.Visible = true;
            ViewState["ImpApagar"] = total;
            lblTotal.Text = total.ToString("C");
        }

        protected void btnRecalculo_Click(object sender, EventArgs e)
        {
            ViewState["Importe"] = "1";
            cerrarGrid();
        }

        protected void selectTipoTramite(object sender, System.EventArgs e)
        {
            rpt.LocalReport.DataSources.Clear();
            btnEstado.Visible = false;
            //rpt.LocalReport.Refresh();
            pnlReport.Visible = false;
            limpiacamposTramite();
            txtContruyente.Text = ViewState["nombreC"].ToString();
            Label5.Text = "Propietario";
            grd.Visible = true;
            switch (Convert.ToInt32(ddlTipoTramite.SelectedValue))
            {
                case 1:
                case 7:
                    grd.Columns[1].HeaderText = "Tipo de Certificado";
                    break;
                case 2:
                    grd.Columns[1].HeaderText = "Tipo Avaluo";
                    break;
                case 3:
                    grd.Columns[1].HeaderText = "Parcialidad y Fecha";
                    break;
                case 4:
                    grd.Columns[1].HeaderText = "Fecha o Contribuyente";
                    break;
                default:
                    grd.DataSource = null;
                    grd.DataBind();
                    return;
            }
            llenaGridTramites();
        }

        private void llenaGridTramites()
        {
            Int32 idTipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);
            grd.DataSource = new vTramiteBL().GetFilterVTramiteCertificado(txtClvCastatral.Text,
                    idTipoTramite, "'A'", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
            grd.DataBind();

            //switch (idTipoTramite)
            //{
            //    case 1://1.-Certificados 
            //    case 2://2.- Isabis
            //    case 3://3.- Convenios
            //    case 7://7.- Plano
            //        grd.DataSource = new vTramiteBL().GetFilterVTramiteCertificado(txtClvCastatral.Text,
            //        idTipoTramite, "'A'", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
            //        grd.DataBind();
            //        break;
            //    case 4://4.-Catastro 
            //        cTipoTramite cTipoTramite = new cTipoTramiteBL().GetByConstraint(idTipoTramite);
            //        grd.DataSource = new cConceptoBL().GetFilterCobroCatastro("IdMesa", Convert.ToInt32(cTipoTramite.IdMesa), 1,
            //           ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
            //        grd.DataBind();
            //        break;
            //    //case 7: //7.- Plano
            //    //    grd.DataSource = new cConceptoBL().GetFilterCobro("IdTipoTramite", idTipoTramite, 1,
            //    //         ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
            //    //    grd.DataBind();
            //    //    break;
            //    default:
            //        grd.DataSource = null;
            //        grd.DataBind();
            //        break;
            //}

        }


        private void llenaGridTramites(Int32 idTramite)
        {
            ViewState["TipoTramite"] = ddlTipoTramite.SelectedValue;
            ViewState["idT"] = idTramite.ToString();
            ViewState["Importe"] = "0";
            DateTime d = DateTime.Now.Date;
            DateTime fecha = new DateTime();
            int tt = int.Parse(ddlTipoTramite.SelectedValue);
            tTramite tramite = new tTramiteBL().GetByConstraint(idTramite);
            ViewState["observaciones"] = tramite.Observacion == null ? "" : tramite.Observacion;
            String IdMesa = "0";
            String criIsabisRe = "";
            String criIsabis = "";
            Int32 idConceptoP = 0;
            //decimal recargoMes = 0;
            if (tramite.cTipoTramite.IdMesa != null)
            {
                IdMesa = tramite.cTipoTramite.IdMesa.ToString();
            }
            List<cConcepto> conceptos = new cConceptoBL().GetListByConstraint(tt, Convert.ToInt32(d.Year), Convert.ToInt32(IdMesa));
            //Para los tramites Isabis se calcula un recargo si se pasan de 15 dias locales o 20 dias foraneos
            if (tt == 2)
            {
                criIsabis = new cParametroSistemaBL().GetValorByClave("ISABISCRI");
                txtContruyente.Text = tramite.NombreAdquiriente;
                Label5.Text = "Adquiriente";
                //tramite.FechaOperacion =Convert.ToDateTime(tramite.FechaOperacion).AddDays(1);
                tramite.FechaOperacion = Convert.ToDateTime(tramite.FechaOperacion).Date;
                String criIsabisReAux = new cParametroSistemaBL().GetValorByClave("RECARGO");
                if (criIsabisReAux != null)
                {
                    conceptos.RemoveAll(c => c.Cri.Equals(criIsabisReAux));

                }

                if (tramite.FechaOperacion != d)
                {
                    DateTime dia = Convert.ToDateTime(tramite.FechaOperacion).Date;
                    fecha = fechaHabiles(dia, tramite.IsabiForaneo == true ? 20 : 15).Date;
                    if (d > fecha)
                    {
                        //recargoMes = new cTarifaRecargoBL().tarifaRecargoMes(new tTramiteBL().ObtenerBimestre(Convert.ToDateTime(dia).Month), Convert.ToDateTime(dia).Year);
                        //criIsabisRe = new cParametroSistemaBL().GetValorByClave("RECARGO");
                        if (criIsabisReAux != null)
                        {
                            criIsabisRe = criIsabisReAux;
                            cConcepto cConceptoRecargo = new cConceptoBL().RegistroByCri(criIsabisRe, Convert.ToInt32(d.Year), Convert.ToInt32(IdMesa));
                            if (cConceptoRecargo != null)
                            {
                                conceptos.Add(cConceptoRecargo);
                            }
                            else
                            {
                                vtnModal.ShowPopup(new Utileria().GetDescription("Imposible agregar el Isabis, No se encuentra el Concepto del cri para el RECARGO"), ModalPopupMensaje.TypeMesssage.Alert);
                                return;
                            }

                        }
                        else
                        {
                            vtnModal.ShowPopup(new Utileria().GetDescription("Imposible agregar el Isabis, No se encuentra el valor del cri para el RECARGO"), ModalPopupMensaje.TypeMesssage.Alert);
                            return;
                        }

                    }
                }

            }

            if (conceptos != null)
            {
                cSalarioMinimo s = new cSalarioMinimoBL().GetByEjercicio(Convert.ToInt32(d.Year));
                if (s != null)
                {
                    bool TipoS = false; decimal costoI = 0; decimal costoT = 0; // bool cerrar = true; bool add = false; 
                    List<conceptoGrid> listConcepto = new List<conceptoGrid>();
                    List<tPrediosDescuento> predDesc = new tPrediosDescuentoBL().GetDescuentoCobro(idTramite, Convert.ToInt32(tramite.IdPredio), DateTime.Now);
                    foreach (cConcepto concepto in conceptos)
                    {
                        conceptoGrid conceptoAux = new conceptoGrid();
                        conceptoAux.Id = concepto.Id.ToString();
                        conceptoAux.IdMesa = concepto.IdMesa.ToString();
                        conceptoAux.Concepto = concepto.Nombre;
                        conceptoAux.IdTramite = tramite.Id.ToString();
                        conceptoAux.TipoCobro = concepto.TipoCobro;
                        int idcConcepto = concepto.Id;
                        String costo = "";

                        //idConceptoP = concepto.EsAdicional == false ? concepto.Id: idConceptoP;
                        idConceptoP = concepto.EsAdicional == false && concepto.Cri != criIsabisRe ? concepto.Id : idConceptoP;
                        conceptoAux.IdConceptoP = concepto.EsAdicional == false ? "0" : idConceptoP.ToString();
                        if (concepto.TipoCobro == "S")
                        {

                            TipoS = true;
                            costoI = s.Importe * concepto.SalarioMin;
                            costo = Convert.ToString(Utileria.Redondeo(costoI));
                            conceptoAux.Costo = costo;
                            if (concepto.SinDescuento == true)
                            {
                                if (predDesc != null || criIsabisRe != "")
                                {
                                    tPrediosDescuento preddesc = null;
                                    if (criIsabisRe == concepto.Cri)
                                    {
                                        preddesc = new tPrediosDescuentoBL().GetForCobro(concepto.Id, Convert.ToInt32(ViewState["idP"].ToString()), Convert.ToInt32(ViewState["idT"].ToString()), d);
                                    }
                                    else
                                    {
                                        preddesc = predDesc.Where(p => p.IdConcepto == concepto.Id).FirstOrDefault();
                                    }

                                    if (preddesc != null)
                                    {
                                        decimal pI = Convert.ToDecimal(preddesc.Porcentaje);
                                        decimal desc = Utileria.Redondeo(pI * (costoI / 100));
                                        decimal import = Utileria.Redondeo(costoI - desc);
                                        conceptoAux.Porcentaje = Convert.ToString(Utileria.Redondeo(preddesc.Porcentaje));
                                        conceptoAux.Descuento = Convert.ToString(Utileria.Redondeo(desc));
                                        conceptoAux.Importe = Convert.ToString(Utileria.Redondeo(import));
                                    }
                                    else
                                    {
                                        conceptoAux.Porcentaje = "0";
                                        conceptoAux.Descuento = "0";
                                        conceptoAux.Importe = costo;
                                    }
                                }

                            }
                            else
                            {
                                conceptoAux.Porcentaje = "0";
                                conceptoAux.Descuento = "0";
                                conceptoAux.Importe = costo;
                            }

                            costoT = costoT + Convert.ToDecimal(conceptoAux.Importe);
                        }
                        if (concepto.TipoCobro == "P")
                        {
                            if (TipoS == true)
                            {
                                decimal pI = Convert.ToDecimal(concepto.Importe);
                                //decimal desc = Utileria.Redondeo((pI * (costoI / 100)), 2);
                                //costo = Convert.ToString(Utileria.Redondeo(desc));
                                costo = Utileria.Redondeo((pI * (costoI / 100))).ToString();
                                conceptoAux.Costo = costo;
                                conceptoAux.Porcentaje = "0";
                                if (concepto.EsAdicional == false)
                                    conceptoAux.Porcentaje = concepto.Importe.ToString();
                                conceptoAux.Descuento = "0";
                                conceptoAux.Importe = costo;
                            }
                            else
                            {
                                conceptoAux.Costo = concepto.Importe.ToString();
                                conceptoAux.Porcentaje = concepto.Importe.ToString();

                            }

                            costoT = costoT + Convert.ToDecimal(conceptoAux.Importe);
                        }
                        if (concepto.TipoCobro == "I")
                        {
                            conceptoAux.Porcentaje = "0";
                            conceptoAux.Descuento = "0";
                            conceptoAux.Importe = "0";
                            //Para el caso de los isabis el costo se calcula
                            if (tramite.IdTipoTramite == 2)
                            {
                                if (concepto.Cri == criIsabis)
                                {
                                    Double impporte = tramite.NoSalarioMinimo == null ? Convert.ToDouble(tramite.ValorMasAlto) * 0.02 :
                                                        Convert.ToDouble(tramite.ValorMasAlto);
                                    conceptoAux.Costo = Utileria.Redondeo(Convert.ToDecimal(impporte)).ToString();
                                    conceptoAux.Importe = conceptoAux.Costo;
                                    ViewState["idCriIsabis"] = concepto.Id.ToString();
                                    TipoS = true;
                                    costoI = Utileria.Redondeo(Convert.ToDecimal(conceptoAux.Costo));
                                }
                                if (concepto.Cri == criIsabisRe)
                                {
                                    ViewState["idCriIsabisRe"] = concepto.Id.ToString();
                                    decimal pI = porcentajeRecargo(fecha);// + recargoMes;
                                    conceptoAux.Costo = Utileria.Redondeo(pI * Convert.ToDecimal(costoT)).ToString();
                                    conceptoAux.Costo = conceptoAux.Costo == "0" ? "1" : conceptoAux.Costo;
                                    conceptoAux.Importe = conceptoAux.Costo;
                                    conceptoAux.IdConceptoP = idConceptoP.ToString();
                                    conceptoAux.TipoCobro = "S";

                                }
                            }


                            if (concepto.SinDescuento == true)
                            {
                                if (predDesc != null || criIsabisRe != "")
                                {
                                    tPrediosDescuento preddesc = null;
                                    if (criIsabisRe == concepto.Cri)
                                    {
                                        preddesc = new tPrediosDescuentoBL().GetForCobro(concepto.Id, Convert.ToInt32(ViewState["idP"].ToString()), Convert.ToInt32(ViewState["idT"].ToString()), DateTime.Now);
                                    }
                                    else
                                    {
                                        preddesc = predDesc.Where(p => p.IdConcepto == concepto.Id).FirstOrDefault();
                                    }

                                    if (preddesc != null)
                                    {
                                        if (concepto.Cri == criIsabisRe || concepto.Cri == criIsabis)
                                        {

                                            conceptoAux.Descuento = Utileria.Redondeo(preddesc.Porcentaje * (Convert.ToDecimal(conceptoAux.Importe) / 100)).ToString();
                                            conceptoAux.Importe = (Convert.ToDecimal(conceptoAux.Costo) - Convert.ToDecimal(conceptoAux.Descuento)).ToString();

                                        }
                                        else
                                        {
                                            conceptoAux.Porcentaje = Convert.ToString(Utileria.Redondeo(preddesc.Porcentaje));
                                        }

                                    }
                                }
                            }

                            costoT = costoT + Convert.ToDecimal(conceptoAux.Importe);
                        }
                        listConcepto.Add(conceptoAux);
                    }


                    grdCobros.DataSource = listConcepto;
                    grdCobros.DataBind();
                    cerrarGrid();

                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription("No se Encuentra UMA del Ejercicio Actual"), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("No se Encuentran Conceptos de Cobro del Ejercicio Actual"), ModalPopupMensaje.TypeMesssage.Alert);
            }
        }


        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "ConsultaPago")
            {
                grdCobros.DataSource = null;
                grdCobros.DataBind();
                Int32 id = Convert.ToInt32(e.CommandArgument.ToString());
                switch (Convert.ToInt32(ddlTipoTramite.SelectedValue))
                {
                    case 1://Certificados
                    case 2://Isabis
                    case 7://Plano 
                        grdCobros.Visible = true;
                        llenaGridTramites(id);
                        break;
                    case 3://Convenio
                        if (grdCobros.Rows.Count == 0)
                        {
                            llenaGridConvenios(id);
                        }
                        else
                        {
                            vtnModal.ShowPopup(new Utileria().GetDescription("Imposible agregar otro mensualidad, solo se permie generar el pago de una mensualidad"), ModalPopupMensaje.TypeMesssage.Alert);
                        }
                        break;
                    case 4://Catastro
                        llenaGridConceptos(id);
                        break;
                }

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

            //llenaGrid(Convert.ToInt32(ddlTipoTramite.SelectedValue));
            llenaGridTramites();
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenaGridTramites();
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                tTramite tramite = new tTramite();
                e.Row.Cells[0].Text = txtClvCastatral.Text;
                switch (Convert.ToInt32(ddlTipoTramite.SelectedValue))
                {
                    case 1:
                    case 7:
                        tramite = new tTramiteBL().GetByConstraint(int.Parse(e.Row.Cells[1].Text));
                        if (tramite.Tipo == "IP")
                            e.Row.Cells[1].Text = "IMPUESTO PREDIAL";
                        else
                            e.Row.Cells[1].Text = "SERVICIOS MUNICIPALES";
                        break;
                    case 2:
                        tramite = new tTramiteBL().GetByConstraint(int.Parse(e.Row.Cells[1].Text));
                        e.Row.Cells[1].Text = tramite.cTipoAvaluo.Descripcion;
                        break;
                    case 3:
                        tramite = new tTramiteBL().GetByConstraint(int.Parse(e.Row.Cells[1].Text));
                        e.Row.Cells[1].Text = " PARCIALIDAD " + tramite.NoParcialidad + " --- " + tramite.Fecha.ToString("dd-MMMM-yyyy");
                        break;
                    case 4:
                        tramite = new tTramiteBL().GetByConstraint(int.Parse(e.Row.Cells[1].Text));
                        e.Row.Cells[1].Text = tramite.NombreAdquiriente != null ? (tramite.Fecha.ToString("dd-MMMM-yyyy") + " --- " + tramite.NombreAdquiriente) : tramite.Fecha.ToString("dd-MMMM-yyyy");
                        break;
                        //case 4:                    
                        //    cConcepto concepto = new cConceptoBL().GetByConstraint(int.Parse(e.Row.Cells[1].Text));
                        //    e.Row.Cells[1].Text = concepto.Cri + " - " + concepto.Nombre;
                        //    break;
                };
            }
        }
        
        private List<conceptoGrid> llenarListaConcepto(List<conceptoGrid> listConcepto)
        {
            String idConcepto = listConcepto.Select(i => i.Id).First();
            foreach (GridViewRow gvr in grdCobros.Rows)
            {
                Label Id = gvr.FindControl("lblId") as Label;
                if (idConcepto != Id.Text || ddlTipoTramite.SelectedValue == "3")
                {
                    Label tramite = gvr.FindControl("lblIdTramite") as Label;
                    Label Mesa = gvr.FindControl("lblIdMesa") as Label;
                    Label tipoCobro = gvr.FindControl("lblTipoCobro") as Label;
                    Label Conceptos = gvr.FindControl("lblConcepto") as Label;
                    Label ConceptoP = gvr.FindControl("lblIdConceptoP") as Label;
                    TextBox Costo = gvr.FindControl("txtCosto") as TextBox;
                    TextBox Porcentaje = gvr.FindControl("txtPorcentaje") as TextBox;
                    TextBox Descuento = gvr.FindControl("txtDescuento") as TextBox;
                    TextBox Importe = gvr.FindControl("txtImporte") as TextBox;

                    conceptoGrid conceptoAux = new conceptoGrid();
                    conceptoAux.Id = Id.Text;
                    conceptoAux.IdMesa = Mesa.Text;
                    conceptoAux.Importe = Importe.Text;
                    conceptoAux.Porcentaje = Porcentaje.Text;
                    conceptoAux.IdTramite = tramite.Text;
                    conceptoAux.Concepto = Conceptos.Text;
                    conceptoAux.Costo = Costo.Text;
                    conceptoAux.Descuento = Descuento.Text;
                    conceptoAux.TipoCobro = tipoCobro.Text;
                    conceptoAux.IdConceptoP = ConceptoP.Text;
                    listConcepto.Add(conceptoAux);
                }
                else
                {
                    return null;
                }
            }
            return listConcepto;
        }

        private partial class conceptoGrid
        {
            public string IdTramite { get; set; }
            public string IdMesa { get; set; }
            public string Id { get; set; }
            public string Concepto { get; set; }
            public string Costo { get; set; }
            public String Porcentaje { get; set; }
            public string Descuento { get; set; }
            public string Importe { get; set; }
            public string TipoCobro { get; set; }
            public string IdConceptoP { get; set; }


        }

        private DateTime fechaHabiles(DateTime d, Int32 dias)
        {
            int aux = 0;
            List<cDiaFestivo> diasFestivos = new cDiaFestivoBL().GetAll();
            bool isDiaFestivo = false;
            while (aux < dias)
            {
                if (d.DayOfWeek == DayOfWeek.Saturday)
                {
                    d = d.AddDays(2);
                }
                else if (d.DayOfWeek == DayOfWeek.Sunday)
                {
                    d = d.AddDays(1);
                }
                else
                {
                    isDiaFestivo = false;
                    foreach (cDiaFestivo diaFestivo in diasFestivos)
                    {
                        if (diaFestivo.Fecha.Equals(d))
                        {
                            d = d.AddDays(1);
                            isDiaFestivo = true;
                            break;
                        }
                    }
                    if (isDiaFestivo == false)
                    {
                        d = d.AddDays(1);
                        aux++;
                    }
                }
            }

            if (d.DayOfWeek == DayOfWeek.Saturday)
            {
                d = d.AddDays(2);
            }
            else if (d.DayOfWeek == DayOfWeek.Sunday)
            {
                d = d.AddDays(1);
            }
            return d;
        }
      
        protected void imCerrarRecibo_Click(object sender, ImageClickEventArgs e)
        {
            modalRecibo.Hide();
            limpiaCampos();
            btnCancelarCobro_Click(null, null);
            txtClvCastatral.Text = "";
            txtPropietario.Text = "";
        }

        protected decimal porcentajeRecargo(DateTime d)
        {
            int eInicial = d.Year;
            int mInicial = d.Month;
            int mFinal = DateTime.Now.Month;
            ////Valida año para recargos
            //int ejercicioCobro = Convert.ToInt32(new cParametroCobroBL().GetByClave("EjercicioInicialCobro"));
            //if (ejercicioCobro == -1)
            //{
            //    per.mensaje = MensajesInterfaz.EjercicioInicialCobro;
            //    return per;
            //}
            //string aplicarPrescripcion = new cParametroSistemaBL().GetValorByClave("APLICARPRESCRIPCION");
            //if (aplicarPrescripcion == "" || aplicarPrescripcion == null)
            //{
            //    per.mensaje = MensajesInterfaz.AplicarPrescripcion;
            //    return per;
            //}
            //if (aplicarPrescripcion == "SI")
            //{
            //    int aaPrescripcion = Convert.ToInt32(new cParametroCobroBL().GetByClave("EjercicioDePrescripcion"));
            //    if (aaPrescripcion <= 0)
            //    {
            //        per.mensaje = MensajesInterfaz.AniosPrescripcion;
            //        return per;
            //    }

            //    eAnterior = (DateTime.Today).Year - aaPrescripcion;
            //}




            if (d.Year < (DateTime.Now.Year - 5))
            {
                eInicial = DateTime.Now.Year - 5;
                mInicial = 1;
            }

            mFinal = d.Day > DateTime.Now.Day ? mFinal - 1 : mFinal;
            return new cTarifaRecargoBL().PorcentajeRecargoIsabi(mInicial, eInicial, mFinal, DateTime.Now.Year);
        }

        private void llenaFiltro()
        {
            ddlFiltro.Items.Clear();
            ddlFiltro.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos", ""));
            ddlFiltro.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Nombre", "UPPER(NombreCompleto)"));
            ddlFiltro.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Dirección", "UPPER(Domicilio)"));
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
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text.ToUpper() };
            ViewState["filtro"] = filtro;
            ViewState["sortCampo"] = "ClavePredial";
            grdPropietarios.Visible = true;
            llenagridPropietario();
        }
        private void llenagridPropietario()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grdPropietarios.DataSource = new vBuscarPredioContDomBL().GetFilter("", "", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grdPropietarios.DataBind();
            }
            else
            {
                grdPropietarios.DataSource = new vBuscarPredioContDomBL().GetFilter(filtro[0], filtro[1], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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

                vBuscarPredioContDom predio = new vBuscarPredioContDomBL().GetByConstraint(Convert.ToInt32(e.CommandArgument));
                if (predio != null)
                {
                    //cContribuyente Contribuyente = new cContribuyenteBL().GetByConstraint(predio.IdContribuyente);
                    txtPropietario.Text = predio.NombreCompleto;// Contribuyente.ApellidoPaterno + " " + Contribuyente.ApellidoMaterno + " "+Contribuyente.Nombre;
                    txtClvCastatral.Text = (predio.ClavePredial== null|| predio.ClavePredial == ""?"":predio.ClavePredial.Trim());
                    modalPropietario.Hide();
                    buscarClaveCatastral(null, null);
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
                //vBuscarPredioContDom predio = new vBuscarPredioContDomBL().GetByConstraint(Convert.ToInt32(grdPropietarios.DataKeys[e.Row.RowIndex].Values[0]));
                //if (predio != null)
                //{
                //    //cContribuyente c = new cContribuyenteBL().GetByConstraint(predio.IdContribuyente);
                //    e.Row.Cells[1].Text = predio.NombreCompleto;//c.Nombre + (c.ApellidoPaterno != null ? " " + c.ApellidoPaterno : "") + (c.ApellidoMaterno != null ? " " + c.ApellidoMaterno : "");
                //    e.Row.Cells[2].Text = predio.Domicilio;//predio.NombreCalle + " " + predio.numero + " " + predio.NombreColonia + " " + predio.localidad + " " + predio.CP;

                //}

            }
        }

        protected void txtEfectivo_TextChanged(object sender, EventArgs e)
        {

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
                        //txtCalleRFC.Text = rec.Calle;
                        //txtEstado.Text = rec.Estado;
                        //txtCPRFC.Text = rec.CodigoPostal;
                        //txtNoExt.Text = rec.NoExterior;
                        //txtNoInt.Text = rec.NoInterior;
                        txtNombre.Text = rec.Nombre;
                        //txtMunicipio.Text = rec.Municipio;
                        //txtPais.Text = rec.Pais;
                        //txtColoniaRFC.Text = rec.Colonia;
                        //txtLocalidadRFC.Text = rec.Localidad;
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
                            ddlUsuCFDI.Items.Insert(0, new ListItem("Seleccionar Uso CFDI", ""));
                        }
                        else
                        {
                            ddlUsuCFDI.DataSource = new cUsoCFDIBL().GetByTipoPersona("MORAL");
                            ddlUsuCFDI.DataValueField = "Clave";
                            ddlUsuCFDI.DataTextField = "Descripcion";
                            ddlUsuCFDI.DataBind();
                            ddlUsuCFDI.Items.Insert(0, new ListItem("Seleccionar Uso CFDI", ""));
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
            txtNombre.Enabled = val;
            //txtCalleRFC.Enabled = val;
            //txtEstado.Enabled = val;
            //txtCPRFC.Enabled = val;
            //txtNoExt.Enabled = val;
            //txtNoInt.Enabled = val;            
            //txtMunicipio.Enabled = val;
            //txtPais.Enabled = val;
            //txtColoniaRFC.Enabled = val;
            //txtLocalidadRFC.Enabled = val;
            //txtReferencia.Enabled = val;
            txtCorreoReg.Enabled = val;

        }

        protected void btnRFCRegistro_Click(object sender, EventArgs e)
        {
            lblMensaje.Visible = false;
            activaTxt(true);
            //txtCPRFC.Enabled = false;
            InformacionRFC.Visible = true;
            btnGeneraFactura.Visible = false;
            btnGuardar.Visible = true;
            btnRFCRegistro.Visible = false;

            txtNombre.Text = "";
            //txtCalleRFC.Text = "";
            //txtMunicipio.Text = "";
            //txtEstado.Text = "";
            //txtCPRFC.Text = "";
            //txtColoniaRFC.Text = "";
            //txtNoExt.Text = "";
            //txtLocalidadRFC.Text = "";
            //txtNoInt.Text = "";
            //txtReferencia.Text = "";
            txtCorreoReg.Text = "";

            modalFactura.Show();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string respuesta = new Utileria().validaRFC(txtRFC.Text==null|| txtRFC.Text=="" ?"":txtRFC.Text.ToUpper().Trim());
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
                //rec.Calle = txtCalleRFC.Text;
                //rec.Estado = txtEstado.Text;
                //rec.CodigoPostal = txtCP.Text;
                //rec.NoExterior = txtNoExt.Text;
                //rec.NoInterior = txtNoInt.Text;
                rec.Nombre = txtNombre.Text;
                //rec.Municipio = txtMunicipio.Text;
                //rec.Pais = txtPais.Text;
                //rec.Colonia = txtColoniaRFC.Text;
                //rec.Localidad = txtLocalidadRFC.Text;
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
                            ddlUsuCFDI.Items.Insert(0, new ListItem("Seleccionar Uso CFDI", ""));
                        }
                        else
                        {
                            ddlUsuCFDI.DataSource = new cUsoCFDIBL().GetByTipoPersona("MORAL");
                            ddlUsuCFDI.DataValueField = "Clave";
                            ddlUsuCFDI.DataTextField = "Descripcion";
                            ddlUsuCFDI.DataBind();
                            ddlUsuCFDI.Items.Insert(0, new ListItem("Seleccionar Uso CFDI", ""));
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
                            ddlUsuCFDI.Items.Insert(0, new ListItem("Seleccionar Uso CFDI", ""));
                        }
                        else
                        {
                            ddlUsuCFDI.DataSource = new cUsoCFDIBL().GetByTipoPersona("MORAL");
                            ddlUsuCFDI.DataValueField = "Clave";
                            ddlUsuCFDI.DataTextField = "Descripcion";
                            ddlUsuCFDI.DataBind();
                            ddlUsuCFDI.Items.Insert(0, new ListItem("Seleccionar Uso CFDI", ""));
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
            Factura fact = reciboCFDI.generaFacturaRecibo(RFC, IdRecibo, usuarioFactura, passwordFactura, productivoFactura,ddlUsuCFDI.SelectedItem.Value);

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
            //Response.Redirect("~/Servicios/Cobros.aspx", false);
        }

        protected void btnCancelarBusqueda_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Servicios/Cobros.aspx", false);
        }

        #endregion

        protected void Button2_Click(object sender, EventArgs e)
        {
            modalRecibo.Show();
        }

        protected void btnEstado_Click(object sender, EventArgs e)
        {
           
            List<cParametroSistema> listConfiguraciones = new cParametroSistemaBL().GetAll();
            string NombreMunicipio = listConfiguraciones.FirstOrDefault(c => c.Clave == "NOMBRE_MUNICIPIO").Valor;
            string Dependencia = listConfiguraciones.FirstOrDefault(c => c.Clave == "DEPENDENCIA").Valor;
            string Area = listConfiguraciones.FirstOrDefault(c => c.Clave == "AREA").Valor;
            string UrlLogo = Server.MapPath("~") + listConfiguraciones.FirstOrDefault(c => c.Clave == "LOGO").Valor;
            string CLAVE_CATASTRAL_GENERICA = listConfiguraciones.FirstOrDefault(c => c.Clave == "CLAVE_CATASTRAL_GENERICA").Valor;
            FileStream fS = new FileStream(UrlLogo, FileMode.Open, FileAccess.Read);
            byte[] LogoByte = new byte[fS.Length];
            fS.Read(LogoByte, 0, (int)fS.Length);
            fS.Close();

            DataTable ConfGral = new DataTable("ConfGral");
            ConfGral.Columns.Add("NombreMunicipio");
            ConfGral.Columns.Add("Dependencia");
            ConfGral.Columns.Add("Area");
            ConfGral.Columns.Add("Logo", typeof(Byte[]));
            ConfGral.Columns.Add("Usuario");
            ConfGral.Columns.Add("Elaboro");
            ConfGral.Columns.Add("VoBo");            
            cUsuarios U = (cUsuarios)Session["usuario"];
            string nombre = U.Nombre + " " + U.ApellidoPaterno + " " + U.ApellidoMaterno;
         
            //--------------------------------
            DataTable Conceptos = new DataTable("Conceptos");
            Conceptos.Columns.Add("Clave");
            Conceptos.Columns.Add("Descripcion");
            Conceptos.Columns.Add("Costo");
            Conceptos.Columns.Add("Porcentaje");
            Conceptos.Columns.Add("Descuento");
            Conceptos.Columns.Add("Importe");

            decimal total = 0;
            foreach (GridViewRow row in grdCobros.Rows)
            {
                int idConcepto = Convert.ToInt32(grdCobros.DataKeys[row.RowIndex].Value.ToString());
                //cConcepto concepto = new cConceptoBL().GetByConstraint(idConcepto);
                TextBox txtImporte = (TextBox)row.FindControl("txtImporte");
                TextBox txtporcentaje = (TextBox)row.FindControl("txtPorcentaje");
                TextBox txtdescuento = (TextBox)row.FindControl("txtDescuento");
                TextBox txtcosto = (TextBox)row.FindControl("txtCosto");
                total = total + Convert.ToDecimal(txtcosto.Text.Replace("$",""));
                Label lblConcepto = (Label)row.FindControl("lblConcepto");
                Label lblConceptoRef = (Label)row.FindControl("lblIdConceptoP");

                Conceptos.Rows.Add(idConcepto, lblConcepto.Text, txtImporte.Text, txtporcentaje.Text, txtdescuento.Text, txtcosto.Text);
            }

            ConfGral.Rows.Add(NombreMunicipio, Dependencia, Area, LogoByte, "", nombre, total.ToString("C"));

            ////INICIA REPORTE
            //rpt.Visible = true;
            pnlReport.Visible = true;
            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            rpt.LocalReport.DisplayName = "Estado de Cuenta";
            rpt.LocalReport.ReportPath = "Reportes/EdoCuentaCatastro.rdlc";
            rpt.LocalReport.EnableExternalImages = true;
            rpt.LocalReport.DataSources.Clear();
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Conceptos", Conceptos));
            
            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("ClavePredial", ViewState["ClavePredial"].ToString()));
            if (ViewState["ClavePredial"].ToString().Trim() == CLAVE_CATASTRAL_GENERICA.Trim())
            {
                tTramite tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idT"].ToString()));
                parameters.Add(new ReportParameter("Propietario", tramite.NombreAdquiriente.Trim()));
            }
            else
            {
                parameters.Add(new ReportParameter("Propietario", ViewState["nombreC"].ToString().Trim()));
            }            
            parameters.Add(new ReportParameter("Direccion", txtCalle.Text.Trim() + ", " + txtNumero.Text.Trim() + ", " + txtColonia.Text.Trim() + ", " + txtCP.Text.Trim() + ", " + txtLocalidad.Text.Trim()));
            parameters.Add(new ReportParameter("Vigencia", DateTime.Now.Date.ToString("dd/MM/yyyy")));

            rpt.LocalReport.SetParameters(parameters);
            rpt.LocalReport.Refresh();
           
            //MPEpnlEdoCuenta.Show();

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


        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            modalRecibo.Hide();
            limpiaCampos();
        }

        //protected void buscarClaveCatastral(object sender, ImageClickEventArgs e)
        //{
        //    int local;
        //}


        protected void btnCancelarCobro_Click(object sender, EventArgs e)
        {               
            lblCambio.Text = "Cambio: ";
            txtNumeroAprobacion.Text = "";            
            txtTipoCobroSalario.Text = "";
            grdAlta.DataSource = null;
            grdAlta.DataBind();
            grdAlta.Visible = false;
            pnl_Modal.Hide();
        }

        protected void btnAceptarPago_Click(object sender, EventArgs e)
        {
            decimal total = 0;
            listConcepto = LeerGrid("", "", ref total);
            decimal aPagar = Convert.ToDecimal(ViewState["ImpApagar"]);
            if (total != aPagar)
            {
                vtnModal.ShowPopup("La suma de pagos no es igual al importe del Impuesto. Diferencia: " + (aPagar - total).ToString() + ".", ModalPopupMensaje.TypeMesssage.Error);
                pnl_Modal.Show();
            }
            else

                cobrarGenerarRecibo();

        }

        protected void btnCobrar_Click(object sender, EventArgs e)
        {
            //btnCobrar.Enabled = false;
            txtObservacion.Visible = true;
            txtObservacion.Text = "";
            lblImportePago.Text = "Importe: " + lblTotal.Text;
            llenaTipoPago();
            activaEtiquetasModal("99", true);
            txtObservacion.Text = ViewState["observaciones"] == null ? "" : ViewState["observaciones"].ToString();
        }

        protected void btnAceptarCobro_Click(object sender, EventArgs e)
        {
            llenaGridConceptos(Convert.ToInt32(ViewState["idConcepto"].ToString()));//, Convert.ToDecimal(txtTipoCobroSalario.Text));
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
            //lblMetodoPago.
            lblTipoCobro.Visible = !visible;
            txtTipoCobroSalario.Visible = !visible;

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
                    lblMetodoPago.Visible = !visible;
                    ddlMetodoPago.Visible = !visible;
                    lblTituloConceptoId.Text = "Cobros";
                    lblTipoCobro.Visible = visible;
                    txtTipoCobroSalario.Visible = visible;
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
            decimal subtotal = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = grdAlta.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string clave = grdAlta.DataKeys[e.Row.RowIndex].Values[1].ToString();

                //if (activo.ToUpper() == "TRUE")
                //{
                //    ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                //    imgActivar.Visible = false;
                //    //if (tipoCobro != "I")
                //    //{
                //    //    ImageButton imgEditar = (ImageButton)e.Row.FindControl("imgUpdate");
                //    //    imgEditar.Visible = false;

                //    //}
                //}
                //else
                //{
                //    //ImageButton imgEditar = (ImageButton)e.Row.FindControl("imgUpdate");
                //    //imgEditar.Visible = false;
                //    if (id.Substring(0, 1) == "N")
                //    {
                //        ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                //        imgActivar.Visible = false;
                //    }
                //    else
                //    {
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                imgDelete.Visible = true;
                //    }
                //}
                //}
                                             
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                decimal subt = 0;

                // e.Row.Cells[0].Text = "Total";
                //e.Row.Cells[0].Text = subtotal.ToString("C")
                Label lbl = (Label)e.Row.FindControl("lblSubtotal");               
                //subt = (Convert.ToDecimal(ViewState["SubtotalMetodo"])).ToString("N2");
                lblCambio.Text = (Convert.ToDecimal(ViewState["SubtotalMetodo"])).ToString("N2");
                //lbl.Text = lblCambio.Text;
                
            }
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    Label lbl = (Label)e.Row.FindControl("lblCambios");
            //    if (Convert.ToInt32(ViewState["mensajeCambioAlta"]) > 0)
            //    {
            //        //lblCambiosFooter.Text = "Cambios pendientes por Guardar";
            //        lblCambiosFooter.ForeColor = System.Drawing.Color.Red;
            //        //revContribuyente.Enabled = false;
            //    }
            //    else
            //    {
            //        lblCambiosFooter.Text = "";
            //    }
            //    //}
        }

        protected void grdAlta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            decimal sumatoria = 0;
            if (e.CommandName == "EliminarRegistro")
            {              
                string clave = e.CommandArgument.ToString();
                
                if (grdAlta.Rows.Count >= 1)
                { 
                    listConcepto = LeerGrid("D",clave, ref sumatoria);
                    grdAlta.DataSource = listConcepto;
                    grdAlta.DataBind();
                }
                else
                    grdAlta.DeleteRow( grdAlta.SelectedIndex );
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
            decimal aPagar = Convert.ToDecimal(ViewState["ImpApagar"]); //total del tramite

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

            if (Convert.ToDecimal(txtMonto.Text) > 0)
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
                ViewState["SubtotalMetodo"] = subtotalMetodo.ToString() ;
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

        private List<MetodoGrid> LeerGrid(string mov, string valor, ref decimal sumatoria  )
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
                if (mov == "D" && lAux.Clave == valor)
                    lAux = null;
                else
                {
                    sumatoria = sumatoria + Convert.ToDecimal(Importe.Text);
                    lis.Add(lAux);
                }
            }

            return lis;
        }


        protected int ObtenerBimestre(int mes)
        {
            int bimestre = 0;
            switch (mes)
            {
                case 1:
                case 2: { bimestre = 1; break; }
                case 3:
                case 4: { bimestre = 2; break; }
                case 5:
                case 6: { bimestre = 3; break; }
                case 7:
                case 8: { bimestre = 4; break; }
                case 9:
                case 10: { bimestre = 5; break; }
                case 11:
                case 12: { bimestre = 6; break; }
            }
            return bimestre;
        }



    }



}