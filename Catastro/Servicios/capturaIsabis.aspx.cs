using System;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Clases.BL;
using Catastro.Controles;
using Clases;
using Clases.Utilerias;
using System.Web.UI.WebControls;

namespace Catastro.Catalogos
{
    public partial class capturaIsabis : System.Web.UI.Page
    {
        //List<vAntecedentePredio> listConsulta = new vVistasBL().ObtieneAntecedentePredio(txtClave.Text,Convert.ToInt32(hdfIdContribuyente.Value),fechaTramite, inicio, fin);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["valorMasAlto"] = 0.0;
                ViewState["NoSalarioMin"] = 0;
                if (Session["parametro"] != null)
                {
                    Dictionary<string, string> parametros = (Dictionary<string, string>)Session["parametro"];
                    if (parametros.ContainsKey("idTramite"))
                    {
                        llenaConfiguracion(Convert.ToInt32(parametros["idTramite"]));
                        if (parametros.ContainsKey("tipoPantalla"))
                        {
                            if (parametros["tipoPantalla"] == "C")
                            {
                                habilitaCampos(false);
                                lblTitulo.Text = "Consulta Isabis";
                                btnGuardar.Visible = false;

                            }
                            else
                            {
                                hdfId.Value = parametros["idTramite"];
                                lblTitulo.Text = "Edición de Isabis";
                                btnGuardar.Visible = true;
                                txtClvCastatral.Enabled = false;
                                imbBuscar.Enabled = false;

                            }
                        }
                    }
                }
                else
                {
                    habilitaCampos(false);
                    txtClvCastatral.Enabled = true;
                    lblCuentaPredialTxt.Text = "";
                    lblCuentaPredial.Visible = false;
                }
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenarConceptoPago()
        {
            ddlConceptoPago.Items.Clear();
            ddlConceptoPago.DataValueField = "Id";
            ddlConceptoPago.DataTextField = "Descripcion";
            ddlConceptoPago.DataSource = new cTipoAvaluoBL().GetAll();
            ddlConceptoPago.DataBind();
            ddlConceptoPago.Items.Insert(0, new ListItem("Seleccionar tipo avaluo", "%"));

        }

        private void llenarValuador()
        {

            var listFinalValuador = new cValuadorBL().GetAll().Select(v => new { Id = v.Id, NombreCompleto = v.nombre + " " + v.ApellidoPaterno + " " + v.ApellidoMaterno }).OrderBy(o => o.NombreCompleto);
            ddlNombreValuador.Items.Clear();
            ddlNombreValuador.DataValueField = "Id";
            ddlNombreValuador.DataTextField = "NombreCompleto";
            ddlNombreValuador.DataSource = listFinalValuador;
            ddlNombreValuador.DataBind();

            ddlNombreValuador.Items.Insert(0, new ListItem("Seleccionar valuador", "%"));

        }

        protected void validaValorInmueble(object sender, System.EventArgs e)
        {

            if (chkCoret.Checked)
            {
                txtImpuesto.Text = Utileria.Redondeo(Convert.ToDecimal(ViewState["valorMasAlto"])).ToString("C");
            }
            else if (chkNoCausa.Checked)
            {
                txtImpuesto.Text = Utileria.Redondeo(Convert.ToDecimal(ViewState["valorMasAlto"])).ToString("C");
            }
            else //if (!chkCoret.Checked)
            {
                var valor = 0.0;
                var catastro = 0.0;
                var comercial = 0.0;
                var operacion = 0.0;

                if (txtCatastro.Text != "")
                {
                    catastro = Convert.ToDouble(txtCatastro.Text);
                    valor = catastro;
                }

                if (txtComercial.Text != "")
                {
                    comercial = Convert.ToDouble(txtComercial.Text);
                    if (comercial > valor)
                        valor = comercial;
                }

                if (txtOperacion.Text != "")
                {
                    operacion = Convert.ToDouble(txtOperacion.Text);
                    if (operacion > valor)
                        valor = operacion;
                }
                ViewState["valorMasAlto"] = Utileria.Redondeo(Convert.ToDecimal(valor));
                txtImpuesto.Text = Utileria.Redondeo(Convert.ToDecimal(valor * .02)).ToString("C");
            }
            txtDescuento.Text = "0";
            Decimal educacion = (0 * (Convert.ToDecimal(txtImpuesto.Text.Replace("$", "")) / 100));
            Decimal universidad = (0 * (Convert.ToDecimal(txtImpuesto.Text.Replace("$", "")) / 100));
            Decimal FaedeIndustria = (Convert.ToDecimal(0) * (Convert.ToDecimal(txtImpuesto.Text.Replace("$", "")) / 100));
            txtAdicionales.Text = Utileria.Redondeo(educacion + universidad + FaedeIndustria + FaedeIndustria).ToString("C");
            txtDescuento01.Text = "0";
            txtRecargos.Text = "0";
            txtDescuento02.Text = "0";
            txtImporte.Text = Utileria.Redondeo(Convert.ToDecimal(txtImpuesto.Text.Replace("$", "")) + Convert.ToDecimal(txtAdicionales.Text.Replace("$", ""))).ToString("C");
        }

        private void llenarDatosHabilitar(cPredio Predio)
        {
            llenarConceptoPago();
            llenarValuador();
            habilitaCampos(true);
            txtContruyente.Text = Predio.cContribuyente.ApellidoPaterno + " " + Predio.cContribuyente.ApellidoMaterno + " " + Predio.cContribuyente.Nombre + " " + Predio.cContribuyente.RazonSocial;
        }

        protected void buscarClaveCatastral(object sender, System.EventArgs e)
        {
            //ImpuestoBimestral ib = new ImpuestoBimestral();
            //List<vAntecedentePredio> listConsulta = new vVistasBL().ObtieneAntecedentePredio(txtClvCastatral.Text, Convert.ToInt32(hdfIdContribuyente.Value), fechaTramite, inicio, fin);
            int eActual = DateTime.Now.Year;
            double mesActual = Utileria.Redondeo(DateTime.Now.Month / 2.0);
            int bActual = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(DateTime.Now.Month / 2.0)));

            if (txtClvCastatral.Text.Trim().Length == 12)
            {
                cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
                if (Predio != null)
                {
                    //if (eActual * 10 + bActual < Predio.AaFinalIp * 10 + Predio.BimestreFinIp)
                    //{
                    //    vtnModal.ShowPopup(new Utileria().GetDescription("El predio tiene un adeudo, no es posible generar un tramite de plano"), ModalPopupMensaje.TypeMesssage.Alert);
                    //    txtClvCastatral.Text = "";
                    //}
                    if (Predio.cStatusPredio.Descripcion == "S")
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("El predio esta suspendido, favor de revisar la clave con el administrador"), ModalPopupMensaje.TypeMesssage.Alert);
                        txtClvCastatral.Text = "";
                    }
                    else if (Predio.cStatusPredio.Descripcion == "B")
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("El predio esta dado de baja, favor de revisar la clave con el administrador"), ModalPopupMensaje.TypeMesssage.Alert);
                        txtClvCastatral.Text = "";
                    }
                    else if(Predio.cStatusPredio.Descripcion == "A")
                    {
                        if (Predio.ClavePredial != "" || Predio.ClavePredial != null )
                        {
                            if (Predio.ClavePredial.Substring(0, 1) == "0")
                                lblCuentaPredial.Visible = true;
                            lblCuentaPredialTxt.Text = Predio.ClaveAnterior == "" || Predio.ClaveAnterior == null ? "" : Predio.ClaveAnterior.Substring(0, 4) + "-" + Predio.ClaveAnterior.Substring(4, 2) + "-" + Predio.ClaveAnterior.Substring(6, 3) + "-" + Predio.ClaveAnterior.Substring(9, 3);
                            llenarDatosHabilitar(Predio);
                        }
                        else
                        {
                            lblCuentaPredial.Visible = true;
                            lblCuentaPredialTxt.Text = Predio.ClaveAnterior == "" || Predio.ClaveAnterior == null ? "" : Predio.ClaveAnterior.Substring(0, 4) + "-" + Predio.ClaveAnterior.Substring(4, 2) + "-" + Predio.ClaveAnterior.Substring(6, 3) + "-" + Predio.ClaveAnterior.Substring(9, 3);
                            llenarDatosHabilitar(Predio);
                        }
                        
                    }
                    else
                    {
                        txtContruyente.Text = "";
                        txtClvCastatral.Text = "";
                        lblCuentaPredialTxt.Text = "";
                        lblCuentaPredial.Visible = false;
                        vtnModal.ShowPopup(new Utileria().GetDescription(Predio.cStatusPredio.Descripcion == "B" ? MensajesInterfaz.sTatusPredioBaja : MensajesInterfaz.sTatusPredioSuspendido
                            ), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                    txtContruyente.Text = "";
                    txtClvCastatral.Text = "";
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);
                txtClvCastatral.Text = "";
            }
        }

        private void limpiaCampos()
        {
            llenarConceptoPago();
            llenarValuador();
            txtClvCastatral.Text = "";
            txtNombreAdquiriente.Text = "";
            txtContruyente.Text = "";
            txtEscritura.Text = "";
            txtFecha.Text = "";
            txtCatastro.Text = "0";
            txtComercial.Text = "0";
            //txtFiscal.Text = "0";
            txtOperacion.Text = "0";
            txtNombreSolicitante.Text = "";
            txtObservaciones.Text = "";
            txtImpuesto.Text = "0";
            txtDescuento.Text = "0";
            txtAdicionales.Text = "0";
            txtDescuento01.Text = "0";
            txtRecargos.Text = "0";
            txtDescuento02.Text = "0";
            txtImporte.Text = "0";
            ViewState["valorMasAlto"] = 0.0;
            ViewState["NoSalarioMin"] = 0;
            chkNotariaForanea.Checked = false;
            chkNoCausa.Checked = false;
            chkCoret.Checked = false;
        }

        protected void guardarIsabis()
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    cUsuarios U = new cUsuarios();
                    U = (cUsuarios)Session["usuario"];

                    tTramite Tramite = new tTramite();
                    MensajesInterfaz msg = new MensajesInterfaz();
                    if (!(hdfId.Value == string.Empty || hdfId.Value == "0"))
                    {
                        Tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(hdfId.Value));
                    }

                    cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);

                    Tramite.Fecha = Convert.ToDateTime(txtFechaRegistro.Text);
                    Tramite.BaseGravable = Convert.ToDouble(txtCatastro.Text);
                    Tramite.valorOperacion = Convert.ToDouble(txtOperacion.Text);
                    Tramite.valorComercial = Convert.ToDouble(txtComercial.Text);
                    Tramite.FechaOperacion = Convert.ToDateTime(txtFecha.Text);
                    Tramite.IdPredio = Predio.Id;
                    Tramite.IdTipoAvaluo = Convert.ToInt32(ddlConceptoPago.SelectedValue);
                    Tramite.IdValuador = Convert.ToInt32(ddlNombreValuador.SelectedValue);
                    Tramite.NombreAdquiriente = txtNombreAdquiriente.Text;
                    Tramite.Notaria = txtNombreSolicitante.Text;
                    Tramite.NumeroEscritura = txtEscritura.Text;
                    Tramite.Observacion = txtObservaciones.Text;
                    Tramite.Status = chkNoCausa.Checked == true ? "I" : "A";
                    Tramite.IdUsuario = U.Id;
                    Tramite.Activo = true;
                    Tramite.ValorMasAlto = Convert.ToDouble(ViewState["valorMasAlto"]);
                    Tramite.FechaModificacion = DateTime.Now;
                    Tramite.Fecha = DateTime.Now;
                    Tramite.IsabiForaneo = chkNotariaForanea.Checked;
                    Tramite.IdTipoTramite = Convert.ToInt32(new cParametroSistemaBL().GetValorByClave("Isabis"));
                    if (chkCoret.Checked == true)
                    {
                        Tramite.NoSalarioMinimo = Convert.ToInt32(ViewState["NoSalarioMin"]);
                    }
                    else
                    {
                        Tramite.NoSalarioMinimo = null;
                    }

                    if (hdfId.Value == string.Empty || hdfId.Value == "0")
                    {
                        msg = new tTramiteBL().Insert(Tramite);
                    }
                    else
                    {
                        msg = new tTramiteBL().Update(Tramite);
                    }


                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);

                    if (hdfId.Value == string.Empty || hdfId.Value == "0")
                    {
                        limpiaCampos();
                        habilitaCampos(false);
                        txtClvCastatral.Enabled = true;
                    }
                    scope.Complete();
                }
            }
            catch (Exception error)
            {
                new Utileria().logError("capturaIsabis.guardarIsabis.Exception", error);
                vtnModal.ShowPopup(new Utileria().GetDescription(error.Message), ModalPopupMensaje.TypeMesssage.Alert);
            }
        }

        private void llenaConfiguracion(int Id)
        {
            tTramite Tramite = new tTramiteBL().GetByConstraint(Id);
            cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(Tramite.IdPredio));
            llenarConceptoPago();
            llenarValuador();
            txtClvCastatral.Text = predio.ClavePredial;
            txtFechaRegistro.Text = Convert.ToDateTime(Tramite.Fecha).ToString("dd/MM/yyyyy");
            txtNombreAdquiriente.Text = Tramite.NombreAdquiriente;
            txtContruyente.Text = predio.cContribuyente.ApellidoPaterno + " " + predio.cContribuyente.ApellidoMaterno + " " + predio.cContribuyente.Nombre;
            txtEscritura.Text = Tramite.NumeroEscritura;
            txtFecha.Text = Convert.ToDateTime(Tramite.FechaOperacion).ToString("dd/MM/yyyyy");
            txtCatastro.Text = Tramite.BaseGravable.ToString();
            txtComercial.Text = Tramite.valorComercial.ToString();
            //txtFiscal.Text = predio.ValorFiscal.ToString();
            txtOperacion.Text = Tramite.valorOperacion.ToString();
            txtNombreSolicitante.Text = Tramite.Notaria;
            txtObservaciones.Text = Tramite.Observacion;
            chkNotariaForanea.Checked = Tramite.IsabiForaneo;
            if (Tramite.NoSalarioMinimo != null)
            {
                ViewState["valorMasAlto"] = Tramite.ValorMasAlto;
                chkCoret.Checked = true;
            }

            if (Tramite.Status == "I")
            {
                validadorOperacion.MinimumValue = "0.0";
                chkNoCausa.Checked = true;
            }

            if (!(Tramite.cTipoAvaluo is null))
            {
                if (Tramite.cTipoAvaluo.Activo)
                {
                    ddlConceptoPago.SelectedValue = Tramite.cTipoAvaluo.Id.ToString();
                }
                else
                {
                    ddlConceptoPago.Items.Add(new ListItem(Tramite.cTipoAvaluo.Descripcion, Tramite.cTipoAvaluo.Id.ToString()));
                    ddlConceptoPago.SelectedValue = Tramite.cTipoAvaluo.Id.ToString();
                }
            }

            if (!(Tramite.cValuador is null))
            {
                if (Tramite.cValuador.Activo)
                {
                    ddlNombreValuador.SelectedValue = Tramite.cValuador.Id.ToString();
                }
                else
                {
                    ddlNombreValuador.Items.Add(new ListItem(Tramite.cValuador.nombre + " " + Tramite.cValuador.ApellidoPaterno + " " + Tramite.cValuador.ApellidoMaterno, Tramite.cTipoAvaluo.Id.ToString()));
                    ddlNombreValuador.SelectedValue = Tramite.cTipoAvaluo.Id.ToString();
                }
            }

            validaValorInmueble(null, null);
        }

        private void habilitaCampos(bool activo)
        {
            txtClvCastatral.Enabled = activo;
            txtFechaRegistro.Enabled = activo;
            txtNombreAdquiriente.Enabled = activo;
            txtContruyente.Enabled = activo;
            txtEscritura.Enabled = activo;
            txtFecha.Enabled = activo;
            txtCatastro.Enabled = activo;
            txtComercial.Enabled = activo;
            //txtFiscal.Enabled = activo;
            txtOperacion.Enabled = activo;
            txtNombreSolicitante.Enabled = activo;
            txtObservaciones.Enabled = activo;
            txtImpuesto.Enabled = activo;
            txtDescuento.Enabled = activo;
            txtAdicionales.Enabled = activo;
            txtDescuento01.Enabled = activo;
            txtRecargos.Enabled = activo;
            txtDescuento02.Enabled = activo;
            txtImporte.Enabled = activo;
            ddlConceptoPago.Enabled = activo;
            ddlNombreValuador.Enabled = activo;
            chkNotariaForanea.Enabled = activo;
            chkNoCausa.Enabled = activo;
            chkCoret.Enabled = activo;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
                vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion) ||
                vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.RegresarMSG))
            {
                Session["parametro"] = null;
                Response.Redirect("BusquedaIsabis.aspx");
            }
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfirmacionGuardado))
            {
                //vtnModal.DysplayCancelar = false; 
                guardarIsabis();
            }

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> parametros = (Dictionary<string, string>)Session["parametro"];
            if (parametros != null)
            {
                if (parametros.ContainsKey("tipoPantalla"))
                {
                    if (parametros["tipoPantalla"] == "M")
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.RegresarMSG), ModalPopupMensaje.TypeMesssage.Confirm);

                    }
                    else
                    {
                        Session["parametro"] = null;
                        Response.Redirect("BusquedaIsabis.aspx");

                    }
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.RegresarMSG), ModalPopupMensaje.TypeMesssage.Confirm);
            }


        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = true;
            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfirmacionGuardado), ModalPopupMensaje.TypeMesssage.Confirm);
        }

        protected void chkNoCausa_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["valorMasAlto"] = 0;
            ViewState["NoSalarioMin"] = 0;

            //if (chkNoCausa.Checked)
            //{
            //    //validadorOperacion.MinimumValue = "0.0";
            //    //txtOperacion.Text = txtOperacion.Text == "" ? "0" : txtOperacion.Text;
            //    ViewState["valorMasAlto"] = 0;
            //    ViewState["NoSalarioMin"] = 0;
            //}
            //else
            //{
            //    validadorOperacion.MinimumValue = "0.1";
            //    txtOperacion.Text = txtOperacion.Text == "" ? "0" : txtOperacion.Text;
            //    //txtOperacion.Text = "";
            //}
            validaValorInmueble(null, null);
        }

        protected void chkCoret_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCoret.Checked)
            {
                int ejercicio = DateTime.Now.Year;
                cTipoTramite tt = new cTipoTramiteBL().GetByConstraint(Convert.ToInt32(new cParametroSistemaBL().GetValorByClave("Isabis")));
                String criIsabis = new cParametroSistemaBL().GetValorByClave("ISABISCRI");
                cConcepto concepto = new cConceptoBL().RegistroByCri(criIsabis, ejercicio, Convert.ToInt32(tt.IdMesa));
                cSalarioMinimo s = new cSalarioMinimoBL().GetByEjercicio(ejercicio);
                decimal corectIsabis = new cParametroCobroBL().GetByClave("UMA ISABIS CORECT");
                ViewState["valorMasAlto"] = Utileria.Redondeo(Convert.ToDecimal(corectIsabis * s.Importe));
                ViewState["NoSalarioMin"] = 0;// concepto.SalarioMin;
            }
            else
            {
                ViewState["valorMasAlto"] = 0.0;
                ViewState["NoSalarioMin"] = 0;
            }
            validaValorInmueble(null, null);
        }


    }
}
