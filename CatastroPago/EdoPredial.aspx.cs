using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using Catastro.Controles;
using System.Globalization;
using System.Text;

namespace CatastroPago
{
    public partial class EdoPredial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divPredial.Visible = false;
               
                if (Session["idPredio"].ToString() != null)                
                    buscarPredio(Convert.ToInt32(Session["idPredio"].ToString()));                
                else
                    Response.Redirect("~/Buscar.aspx", false);


                cParametroSistema municipio = new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO");
                if (municipio.Valor == "ZAPATA")
                {
                    ImagenLogo.ImageUrl = "~/Img/logoZapata.jpg";
                    ImagenLogo.Height = 90;
                    ImagenLogo.Width = 252;
                    ViewState["colordiv"] = "#EFBEB1";
                }
                else
                {
                    ImagenLogo.ImageUrl = "~/Img/logoZapata.jpg";
                    ViewState["colordiv"] = "#EFBEB1";
                    ImagenLogo.Height = 72;
                    ImagenLogo.Width = 186;
                }

            }
        }

        public string colorDiv { get { return ViewState["colordiv"].ToString(); } }
        
        protected void btnConsulta_Click(object sender, EventArgs e)
        {           
            Response.Redirect("~/Buscar.aspx", false);
        }

        protected void buscarPredio(int idPredio)
        {
            string Error = "Se presento un problema para calcular el Estado de Cuenta, intente más tarde, Gracias.";
            try
            {
                cPredio Predio = new cPredioBL().GetByConstraint(idPredio);
                if (Predio != null && Predio.Activo == true && (Predio.cStatusPredio.Descripcion == "A" || Predio.cStatusPredio.Descripcion == "S"))
                {
                    divPredial.Visible = true;
                    //divBusqueda.Visible = false;
                    /******* VALORES DEL PREDIO ******/
                    lblClave.Text = Predio.ClavePredial;
                    lblSuperficie.Text = Predio.SuperficieTerreno.ToString();
                    lblPropietario.Text = Predio.cContribuyente.Nombre + " " + Predio.cContribuyente.ApellidoPaterno + " " + Predio.cContribuyente.ApellidoMaterno;
                    lblValor.Text = Convert.ToDecimal(Predio.ValorTerreno).ToString("C");
                    lblDirecion.Text = Predio.Calle + ' ' + Predio.Numero + ' ' + Predio.cColonia.NombreColonia + ' ' + Predio.Localidad;
                    lblConstruida.Text = Predio.SuperficieConstruccion.ToString();
                    lblBase.Text = Predio.ValorCatastral.ToString("C");
                    lblConstruccion.Text = Predio.ValorConstruccion.ToString("C");
                    lblZona.Text = Predio.cUsoSuelo.Descripcion;
                    lblMetros.Text = Predio.MetrosFrente.ToString();

                    Int32 descIP = 0;
                    SaldosC s = new SaldosC();
                    Impuesto i = new Impuesto();
                    Servicio sm = new Servicio();
                    String soloDif = "NO";

                    ///******IMPUESTO PREDIAL******//
                    int ejercicio = Convert.ToInt16(new cParametroCobroBL().GetByClave("EjercicioInicialCobro"));
                    int eIni = 0, bimIni = 1, anioIniIP = 0, bimFin = 6, anioFinIP = 0;
                    if (ejercicio == DateTime.Today.Year + 2)
                    {
                        eIni = ejercicio;
                        ejercicio--;
                    }

                    List<cBaseImpuesto> baseImp = new cBaseImpuestoBL().GetByEjercicioInicial(ejercicio);
                    if (baseImp != null)
                    {
                        anioIniIP = baseImp.FirstOrDefault().Ejercicio;
                        anioFinIP = baseImp.LastOrDefault().Ejercicio;
                    }

                    if (eIni > anioFinIP)
                        anioFinIP = eIni;
                    if (eIni < anioIniIP && eIni != 0)
                        anioIniIP = eIni;

                    int bimIniSM = bimIni, anioIniSM = anioIniIP, bimFinSM = bimFin, anioFinSM = anioFinIP;
                    //IP
                    Periodo per = s.ValidaPeriodoPago(Predio.BimestreFinIp, Predio.AaFinalIp, bimFin, DateTime.Today.Year, "CalculaPredial");
                    if (per.mensaje != 0)
                    {                        
                        vtnModal.ShowPopup(new Utileria().GetDescription(per.mensaje), ModalPopupMensaje.TypeMesssage.Alert);
                        throw new System.ArgumentException(new Utileria().GetDescription(per.mensaje), "Pago en línea");
                        //return;
                    }
                    if (per.eInicial == DateTime.Today.Year + 2)
                    {
                        //ddlBimIniIP.SelectedValue = "0";
                        //ddlEjIniIP.SelectedValue = "0";
                        //ddlBimFinIP.SelectedValue = "0";
                        //ddlEjFinIP.SelectedValue = "0";

                        //ddlBimIniIP.Enabled = false;
                        //ddlEjIniIP.Enabled = false;
                        //ddlBimFinIP.Enabled = false;
                        //ddlEjFinIP.Enabled = false;

                        //rdbIP.Enabled = false;
                    }
                    else
                    {
                        bimIni = per.bInicial;
                        anioIniIP = per.eInicial;
                        bimFin = per.bFinal;
                        anioFinIP = per.eFinal;
                    }


                    i = s.CalculaCobro(Predio.Id, soloDif, bimIni, anioIniIP, bimFin, anioFinIP, descIP, 0, "CalculaPredial");
                    if (i.mensaje != 0)
                    {                        
                        if (i.TextError != null)
                            vtnModal.ShowPopup(Error, ModalPopupMensaje.TypeMesssage.Alert);
                        else
                            vtnModal.ShowPopup(Error, ModalPopupMensaje.TypeMesssage.Alert);
                        throw new System.ArgumentException(new Utileria().GetDescription(i.mensaje), "Pago en línea");
                        //return;
                    }
                    else
                    {
                        txtPeriodo.Text = i.Estado.PeriodoGral.ToString();
                        txtImpuestoAnt.Text = i.Estado.AntImpuesto.ToString("C", CultureInfo.CurrentCulture);
                        txtAdicionalAnt.Text = i.Estado.AntAdicional.ToString("C", CultureInfo.CurrentCulture);
                        txtImpuesto.Text = i.Estado.Impuesto.ToString("C", CultureInfo.CurrentCulture);
                        txtDiferencias.Text = i.Estado.Diferencias.ToString("C", CultureInfo.CurrentCulture);
                        txtRecargosDif.Text = i.Estado.RecDiferencias.ToString("C", CultureInfo.CurrentCulture);

                        txtRezagos.Text = i.Estado.Rezagos.ToString("C", CultureInfo.CurrentCulture);
                        txtRecargos.Text = i.Estado.Recargos.ToString("C", CultureInfo.CurrentCulture);
                        txtAdicional1.Text = i.Estado.Adicional.ToString("C", CultureInfo.CurrentCulture);
                        txtMultas.Text = i.Multa.ToString("N2", CultureInfo.CurrentCulture);
                        txtEjecucion.Text = ( i.Estado.Ejecucion +i.Estado.Honorarios).ToString("C", CultureInfo.CurrentCulture);

                        txtDescuentos.Text = i.Estado.Descuentos.ToString("C", CultureInfo.CurrentCulture);
                        txtTotalIp.Text = i.Estado.Importe.ToString("C", CultureInfo.CurrentCulture);

                    }


                    ///******SERVICIOS MUNICIPALES******//   
                    //servicios
                    per = new Periodo();

                    cParametroSistema municipio = new cParametroSistemaBL().GetByClave("COBROSERVICIOSINTERNET");
                    if (municipio == null) municipio = new cParametroSistema();
                    if (municipio.Valor == "NO" || municipio.Valor == "")
                    {
                        bimIniSM = 0;
                        anioIniSM = 0;
                        bimFinSM = 0;
                        anioFinSM = 0;
                    }
                    else
                    {
                        Periodo perS = s.ValidaPeriodoPago(Predio.BimestreFinSm, Predio.AaFinalSm, bimFinSM, DateTime.Today.Year, "CalculaServicios");
                        if (perS.mensaje != 0)
                        {
                            vtnModal.ShowPopup(new Utileria().GetDescription(per.mensaje), ModalPopupMensaje.TypeMesssage.Alert);
                            throw new System.ArgumentException(new Utileria().GetDescription(per.mensaje), "Pago en línea");
                            //return;
                        }
                        if (per.eInicial == DateTime.Today.Year + 2)
                        {

                        }
                        else
                        {
                            bimIniSM = perS.bInicial;
                            anioIniSM = perS.eInicial;
                            bimFinSM = perS.bFinal;
                            anioFinSM = perS.eFinal;
                        }
                    }

                    sm = s.CalculaCobroSM(Predio.Id, bimIniSM, anioIniSM, bimFinSM, anioFinSM, descIP, 0, "CalculaServicios", Predio.Zona, Predio.IdTipoPredio,Convert.ToDouble(Predio.SuperficieConstruccion));

                    txtPeriodoSM.Text = sm.Estado.PeriodoGral.ToString();
                    txtInfraestructuraAntSm.Text = sm.Estado.AntInfraestructura.ToString("C", CultureInfo.CurrentCulture);
                    txtAdicionalAntSm.Text = sm.Estado.AntAdicional.ToString("C", CultureInfo.CurrentCulture);

                    txtInfraestructuraSm.Text = sm.Estado.Infraestructura.ToString("C", CultureInfo.CurrentCulture);
                    txtRecoleccionSm.Text = sm.Estado.Recoleccion.ToString("C", CultureInfo.CurrentCulture);
                    txtLimpiezaSm.Text = sm.Estado.Limpieza.ToString("C", CultureInfo.CurrentCulture);
                    txtDapSm.Text = sm.Estado.Dap.ToString("C", CultureInfo.CurrentCulture);
                    txtEjecucionSm.Text = (sm.Estado.Ejecucion + sm.Estado.Honorarios).ToString("C", CultureInfo.CurrentCulture);
                    txtMultasSm.Text = sm.Estado.Multa.ToString("C", CultureInfo.CurrentCulture);
                    txtRecargosSm.Text = sm.Estado.Recargo.ToString("C", CultureInfo.CurrentCulture);
                    txtRezagosSm.Text = sm.Estado.Rezagos.ToString("C", CultureInfo.CurrentCulture);
                    txtAdicionalSm.Text = sm.Estado.Adicional.ToString("C", CultureInfo.CurrentCulture);

                    txtTotalSm.Text = sm.Estado.Importe.ToString("C", CultureInfo.CurrentCulture);
                    lblVigencia.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    txtDescuentosSm.Text = sm.Estado.Descuentos.ToString("N", CultureInfo.CurrentCulture);

                    hfImporteTotal.Value = Utileria.Redondeo(i.Estado.Importe + sm.Estado.Importe).ToString();
                    lblImporte.Text = (i.Estado.Importe + sm.Estado.Importe).ToString("C", CultureInfo.CurrentCulture);

                    ViewState["IP"] = i;
                    ViewState["SM"] = sm;
                    ViewState["IdPredio"] = Predio.Id;

                    lblSaldo.Text = (sm.Importe + i.Importe).ToString("C");

                    btnPagar.Enabled = false;
                    //if (lblClave.Text.Trim() == "420000000021" || lblClave.Text.Trim() == "420000000023" || lblClave.Text.Trim() == "420000000034" )
                    //{
                    //    btnPagar.Visible = true;
                    //}
                }
                else
                {                                      
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);                    
                    throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), "Pago en línea");
                    //return;
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("PagoEnLinea.BuscarPredio.Exception", ex);
                btnPagar.Visible = false;
                return;
                //Response.Redirect("~/Buscar.aspx", false);

            }

        }//Buscar predio
       
        protected void btnPagar_Click(object sender, EventArgs e)
        {
            Session["idOrder"] = DateTime.Today.Month + "-" + lblClave.Text.Trim(); 
            string idOrden = DateTime.Today.Month + "-" + lblClave.Text.Trim(); 
            TramiteInternet t = new TramiteInternet();
            MensajesInterfaz msg = new MensajesInterfaz();

            

            // guardar tramite de internet
            msg = t.GuardaInternet((int)ViewState["IdPredio"], ref idOrden, Convert.ToDecimal(hfImporteTotal.Value), (Impuesto)ViewState["IP"], (Servicio)ViewState["SM"]);
            if (msg != MensajesInterfaz.TramiteGuardado)
            {
                vtnModal.ShowPopup("Ocurrio un problema para pagar en línea, ya se reporto al Administrador. Intente más tarde.(1)", ModalPopupMensaje.TypeMesssage.Alert);
                throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), "Pago en línea");
            }
            Session["referencia"] = lblClave.Text;

            cParametroSistema municipio = new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO");
            if (municipio.Valor == "ZAPATA")
            {
                //Session["idOrder"]  se inicializo arriba con 2 valores, mes + clave 
               
                Session["Importe"] = hfImporteTotal.Value;
                
                string[] orden = idOrden.ToString().Split('-');   
                Session["idInternet"] = orden[2];
                Response.Redirect("~/Operadoras/bbva.aspx", false);
            }
            else
            {               
                Session["Importe"] = hfImporteTotal.Value; 
                Session["idOrder"] = idOrden; //se inicializo arriba con 2 valores, mes + clave , mas idInternet retorna la funcion( 3)
                Response.Redirect("~/Operadoras/banorte.aspx", false);
            }   
        }

        protected void btnConsult_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Buscar.aspx", false);
        }
         

    }//CLASS

}