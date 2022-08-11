using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Transactions;
using System.Web.UI.WebControls;
using Clases.BL;
using Catastro.Controles;
using Clases;
using Clases.Utilerias;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Configuration;

namespace Catastro.Catalogos
{
    public partial class Certificado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["parametro"] = null;

                txtFechaAdeudo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                ViewState["bActual"] = ObtenerBimestre(DateTime.Now.Month);
                ViewState["idTramite"] = 0;
                ViewState["bimestreI"] =0;
                ViewState["bimestreF"] = 0;
                ViewState["ejercicioI"] = 0;
                ViewState["ejercicioF"] = 0;
                ViewState["importe"] = 0;

            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfirmacionGuardado), ModalPopupMensaje.TypeMesssage.Confirm);
        }

        protected void btnRecargar_Click(object sender, EventArgs e)
        {

            Response.Redirect("Certificado.aspx");
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
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {

            //if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso))
            //{
            //    buscarClaveCatastral(null,null);
            //}
            //else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfirmacionGuardado))
            //{
            //    //guardarConceptoOmision();
            //}

        }

        protected void btnPrepago_Click(object sender, EventArgs e)
        {
            try
            {

                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                tTramite Tramite = new tTramite();
                cPredio predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);

                if (new cParametroSistemaBL().GetValorByClave("VALIDACIONFECHAAVALUO") == "SI")
                {
                    if (predio.FechaAvaluo < (DateTime.Now.AddYears(-2)))
                    {
                        throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.NoAGenerarTramite));
                    }
                }

                Tramite.Fecha = DateTime.Now;
                Tramite.IdPredio = predio.Id;
                Tramite.IdTipoTramite = Convert.ToInt32(new cParametroSistemaBL().GetValorByClave("Certificado"));
                if (chkGratuito.Checked)
                {
                    Tramite.Status = "I";
                }
                else
                {
                    Tramite.Status = "A";
                }
                Tramite.IdUsuario = U.Id;
                Tramite.Activo = true;
                Tramite.FechaModificacion = DateTime.Now;
                Tramite.FechaOperacion = DateTime.Now;
                //string HOLA = ViewState["bimestreF"].ToString();
                Tramite.BimestreFinal = Convert.ToInt32("2");
                Tramite.EjercicioFinal = Convert.ToInt32(ViewState["ejercicioF"].ToString());
                Tramite.Adeudo = Convert.ToDecimal(ViewState["importe"].ToString());

                if (rdbPredial.Checked)
                {
                    Tramite.SuperficieConstruccion = predio.SuperficieConstruccion;
                    Tramite.SuperficieTerreno = predio.SuperficieTerreno;
                    Tramite.BaseGravable = predio.ValorCatastral;
                    Tramite.BimestreInicial = predio.BimestreFinIp;
                    Tramite.EjercicioInicial = predio.AaFinalIp;
                    Tramite.Tipo = "IP";
                    if (Decimal.Compare(Convert.ToDecimal(ViewState["importe"].ToString()), Convert.ToDecimal(0)) == 1)
                    {
                        Tramite.Periodo = Convert.ToInt32(ViewState["bimestreI"].ToString()) + "/" + Convert.ToInt32(ViewState["ejercicioI"].ToString()) + " AL " +
                           Convert.ToInt32(ViewState["bimestreF"].ToString()) + "/" + Convert.ToInt32(ViewState["ejercicioF"].ToString());
                    }
                    else
                    {
                        Tramite.Periodo = predio.BimestreFinIp + "/" + predio.AaFinalIp;
                    }
                }
                else
                {
                    Tramite.BimestreInicial = predio.BimestreFinSm;
                    Tramite.EjercicioInicial = predio.AaFinalSm;
                    Tramite.Tipo = "SM";

                    if (Decimal.Compare(Convert.ToDecimal(ViewState["importe"].ToString()), Convert.ToDecimal(0)) == 1)
                    {
                        Tramite.Periodo = Convert.ToInt32(ViewState["bimestreI"].ToString()) + "/" + Convert.ToInt32(ViewState["ejercicioI"].ToString()) + " AL " +
                           Convert.ToInt32(ViewState["bimestreF"].ToString()) + "/" + Convert.ToInt32(ViewState["ejercicioF"].ToString());
                    }
                    else
                    {
                        Tramite.Periodo = predio.BimestreFinSm + "/" + predio.AaFinalSm;
                    }
                }

                Tramite.BimestreFinal = ObtenerBimestre(DateTime.Now.Month);
                Tramite.EjercicioFinal = DateTime.Now.Year;
                MensajesInterfaz msg = new tTramiteBL().Insert(Tramite);
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                buscarClaveCatastral(null, null);
            }
            catch (Exception error)
            {
                new Utileria().logError("Certificado.btnPrepago_Click.Exception", error);
                vtnModal.ShowPopup(new Utileria().GetDescription(error.Message), ModalPopupMensaje.TypeMesssage.Alert);

            }    
            

        }

        protected int validaTramite(cPredio predio)
        {
            String tipoTramite = new cParametroSistemaBL().GetValorByClave("Certificado");
            String tipo = rdbPredial.Checked == true ? "IP" : "SM";
            List<tTramite> lisTramite = new tTramiteBL().GetTiposTramiteIdPredio(predio.Id, Convert.ToInt32(tipoTramite), "P", tipo);
            if (lisTramite == null) return 3;

            if (lisTramite.Count > 0 )//Tramite pendiene para imprimir
            {
                ViewState["idTramite"] = lisTramite.FirstOrDefault().Id;
                return 1;                
            }
            else
            {
                lisTramite = new tTramiteBL().GetTramiteFechaIniFin(DateTime.Now.AddDays(-6), DateTime.Now, predio.Id, Convert.ToInt32(tipoTramite), "A", tipo);
                if (lisTramite.Count > 0)//Trammite pendiente de pago
                {
                    ViewState["idTramite"] = lisTramite.FirstOrDefault().Id;
                    return 2;
                }
                else// No tiene tramite pendiente de pago
                {
                    return 3;
                }
            }
        }

        protected void llenarDatos(cPredio predio)
        {             
            switch (validaTramite(predio)) { 
                case 1:// Tiene un tramite pendiente para imprimir
            {

                btnPrepago.Visible = false;
                btnImprimir.Visible = true;
                break;
            }
                case 2: //Tiene tramite pendente de pago
                {
                    lblMensajeTPendiente.Visible = true;
                    lblMensajeTPendiente.Text = ("TRAMITE PENDIENTE DE PAGO");
                    btnPrepago.Visible = false;
                    btnImprimir.Visible = false;
                    break;
                }
                case 3:// se vencio el pago pendiente se debe de generar otro tramite
                {
                    // btnPrepago.Visible = true; tramite vencido se dehabilita para no generar tramites
                    btnImprimir.Visible = false;
                    lblMensajeTPendiente.Visible = false;
                    break;
                }
            }

            //Boolean isAdeudo = true;

            //int bimestreAct = Convert.ToInt16(ViewState["bActual"]);
            //int ejercicioAct = DateTime.Now.Year;
            int bimestre = ObtenerBimestre(Convert.ToInt32(Convert.ToDateTime(txtFechaAdeudo.Text).ToString("MM")));
            int ejercicio = Convert.ToInt32(Convert.ToDateTime(txtFechaAdeudo.Text).ToString("yyyy"));
            //if (predio.AaFinalIp == ejercicio)
            //{
            //    if (predio.BimestreFinSm < bimestre)
            //    {
            //        isAdeudo = false;
            //    }
            //}
            //else
            //{
            //    if (predio.AaFinalIp < ejercicio)
            //    {
            //        isAdeudo = false;
            //    }
            //}

            ViewState["idPredio"] = predio.Id;
            lblDirectorPredial.Text = "EL QUE SUSCRIBE " + new cParametroSistemaBL().GetValorByClave("DirectorPredial") +
                ", " + new cParametroSistemaBL().GetValorByClave("PuestoPredial") + ", DE "
                + new cParametroSistemaBL().GetValorByClave("NOMBRE_MUNICIPIO") + ", MORELOS";
            lblFechaEncabezado.Text = "QUE EN LOS ARCHIVOS DE ESTA OFICINA, AL " + DateTime.Now.Day + " DE " + DateTime.Now.ToString("MMMM").ToUpper() + " DEL " + ejercicio +
                ", SE ENCUENTRA REGISTRADO UN PREDIO, CON LOS SIGUIENTES DATOS:";
            cContribuyente contribuyente = predio.cContribuyente;
            lblNombreText.Text = contribuyente.RazonSocial +" "+ contribuyente.ApellidoPaterno + " " + contribuyente.ApellidoMaterno+" " + contribuyente.Nombre ;
            
            lblCuentaCatastral.Text = "Clave catastral: ";
            lblCuentaCatastralText.Text = predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3);
            if (predio.ClavePredial.Substring(0, 1) == "0")
            {
                if ( !(predio.ClaveAnterior is null))
                {
                    lblCuentaCatastral.Text = "Cuenta predial: ";
                    lblCuentaCatastralText.Text = predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClaveAnterior.Substring(9, 3);
                }
               
            }

            lblUbicacionText.Text = (predio.Calle + " " + predio.Numero + " Col. " + predio.cColonia.NombreColonia + " " +
                                        predio.CP + " " + predio.Localidad + ", MORELOS").ToUpper();            
            lblMensajePie.Text = "SE EXPIDE EL PRESENTE CERTIFICADO Y PARA LOS FINES LEGALES QUE CORRESPONDAN";
            
            if (rdbPredial.Checked)
            {

                visibleLabel(true, 1);
                //Periodo per = new SaldosC().ValidaPeriodoPago(predio.BimestreFinIp, predio.AaFinalIp, 6, DateTime.Today.Year);
                //Impuesto deuda = new SaldosC().CalculaCobro(predio.Id, "NO", per.bInicial, per.eInicial, bimestre, ejercicio, 0, 0,"CalculaServicios");
                Impuesto deuda = new Impuesto();
                if (predio.BimestreFinIp >= bimestre && predio.AaFinalIp >= ejercicio)
                {
                    deuda.Importe = 0;
                    deuda.TextError = null;                    
                }
                else
                {
                    deuda = new SaldosC().CalculaCobro(predio.Id, "NO", predio.BimestreFinIp, predio.AaFinalIp, bimestre, ejercicio, 0, 0, "CalculaServicios");
                }
                   
                if (deuda.TextError != null && deuda.mensaje > 0 )//MensajesInterfaz.CalculoImpuesto)
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(deuda.mensaje), ModalPopupMensaje.TypeMesssage.Alert);
                    limpiarDatos();
                    visibleLabel(false,3);
                    return;
                }
                if(Decimal.Compare(deuda.Importe,Convert.ToDecimal(0)) != 1)
                {
                    lblMensajeAdeudo.Text = "CERTIFICADO DE NO ADEUDO DEL IMPUESTO PREDIAL";                    
                }
                else
                {
                    lblMensajeAdeudo.Text = "CERTIFICADO DE ADEUDO DEL IMPUESTO PREDIAL";
                    lblMensajeAdeudo1.Text = "EL CUAL PRESENTA UN ADEUDO DEL BIMESTRE " + deuda.Estado.PeriodoGral + " DE " + deuda.Estado.Importe.ToString("C");
                    string[] periodos=   deuda.Estado.PeriodoGral.Split('-');
                    ViewState["bimestreI"] = deuda.Estado.PeriodoGral.Substring(0, 1);                    
                    ViewState["ejercicioI"] =  periodos[0].Length < 7 ?  deuda.Estado.PeriodoGral.Substring(2, 4) : deuda.Estado.PeriodoGral.Substring(2, 4);
                    ViewState["bimestreF"] =  deuda.Estado.PeriodoGral.Substring(9, 1);
                    ViewState["ejercicioF"] = periodos[1].Length < 7 ? deuda.Estado.PeriodoGral.Substring(11,4) : deuda.Estado.PeriodoGral.Substring(3, 4);
                    ViewState["importe"] = deuda.Importe;
                }
                lblPeriodoPagoText.Text = predio.BimestreFinIp + "/" + predio.AaFinalIp;
                lblSuperficiePredioText.Text = Convert.ToDouble(predio.SuperficieTerreno).ToString("N") + " M2 ";//+ " M&#178; " ;
                lblSuperficiePredioLetra.Text =Letras(predio.SuperficieTerreno.ToString(),1).ToUpper() ;
                lblSuperficieConsText.Text = Convert.ToDouble(predio.SuperficieConstruccion).ToString("N") + " M2 ";// " M&#178; ";
                lblSuperficieConsLetra.Text = Letras(predio.SuperficieConstruccion.ToString(), 1).ToUpper() ;
                lblValorCastText.Text = predio.ValorCatastral.ToString("C");
                lblValorCastLetra.Text =" " + Letras(predio.ValorCatastral.ToString(), 2).ToUpper();
               
                tRecibo reci;
                if (predio.Id >= 46233 && predio.Id <= 46399 && predio.Id != 46327)
                {
                    //PREDIO MASIVAS
                    string clv = new cParametroSistemaBL().GetValorByClave("PREDIO MASIVAS").ToString();
                    cPredio rPredio = new cPredioBL().GetByClavePredial(clv);                   
                    reci = new tReciboBL().GetReciboCatastrobyIPredio(rPredio.Id);
                }
                else
                {
                    reci = new tReciboBL().GetReciboPredialbyIPredio(predio.Id);
                }
                if (reci != null)                    
                {
                    if (reci.FechaPago != null)
                    {
                        lblFechaPagoText.Text = Convert.ToDateTime(reci.FechaPago).ToString("dd") + " de "
                            + Convert.ToDateTime(reci.FechaPago).ToString("MMMM").ToLower() +
                            " de " + Convert.ToDateTime(reci.FechaPago).ToString("yyyy");
                    }
                    if (reci.Id > 0 ) lblREciboText.Text = reci.Id.ToString();

                    //if (predio.FechaPagoIp != null)
                    //{
                    //    lblFechaPagoText.Text = Convert.ToDateTime(predio.FechaPagoIp).ToString("dd") + " DE "
                    //        + Convert.ToDateTime(predio.FechaPagoIp).ToString("MMMM") +
                    //         " DE " + Convert.ToDateTime(predio.FechaPagoIp).ToString("yyyy");
                    //}
                    //else
                    //{
                    //    lblFechaPagoText.Text = "";
                    //}
                    //if (predio.ReciboIp != null)
                    //{
                    //    lblREciboText.Text = predio.ReciboIp.ToString();
                    //    lblMensajePie.Text = lblMensajePie.Text + ", HABIENDO CUBIERTO LOS DERECHOS CAUSADOS, CON RECIBO OFICIAL No. " + predio.ReciboIp;
                    //}
                }
                else
                {
                    lblREciboText.Text = "";
                    lblMensajePie.Text = lblMensajePie.Text + ".";
                }

            }

            if (rdbServicioMun.Checked)
            {
                visibleLabel(true, 2);
                //Periodo per = new SaldosC().ValidaPeriodoPago(predio.BimestreFinSm, predio.AaFinalSm, 6, DateTime.Today.Year);
                //Servicio deuda = new SaldosC().CalculaCobroSM(predio.Id, per.bInicial, per.eInicial, bimestre, ejercicio, 0);

                //Periodo per = new SaldosC().ValidaPeriodoPago(predio.BimestreFinSm, predio.AaFinalSm, 6, DateTime.Today.Year);
                //Servicio deuda = new SaldosC().CalculaCobroSM(predio.Id, predio.BimestreFinSm, predio.AaFinalSm, bimestre, ejercicio, 0,0, " ");

                Servicio deuda = new Servicio();
                if (predio.BimestreFinIp == bimestre && predio.AaFinalIp == ejercicio)
                {
                    deuda.Importe = 0;
                    deuda.TextError = null;
                }
                else
                {
                    deuda = new SaldosC().CalculaCobroSM(predio.Id, predio.BimestreFinSm, predio.AaFinalSm, bimestre, ejercicio, 0, 0, " ",predio.Zona,predio.IdTipoPredio,Convert.ToDouble(predio.SuperficieConstruccion));
                }

                if (deuda.TextError != null && deuda.mensaje > 0)
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(deuda.mensaje), ModalPopupMensaje.TypeMesssage.Alert);
                    limpiarDatos();
                    visibleLabel(false, 3);
                    return;
                }

                if (Decimal.Compare(deuda.Estado.Importe, Convert.ToDecimal(0)) != 1)
                {
                    lblMensajeAdeudo.Text = "CERTIFICADO DE NO ADEUDO DE SERVICIOS MUNICIPALES";
                }
                else
                {
                    lblMensajeAdeudo.Text = "CERTIFICADO DE ADEUDO DE SERVICIOS MUNICIPALES";
                    lblMensajeAdeudo1.Text = "EL CUAL PRESENTA UN ADEUDO DEL BIMESTRE " + deuda.Estado.PeriodoGral.Substring(0,1)  + "/" +deuda.Estado.PeriodoGral.Substring(2,4) +
                                    " AL " + deuda.Estado.PeriodoGral.Substring(8,1) +  "/" + deuda.Estado.PeriodoGral.Substring(10,4) + " DE " + deuda.Estado.Importe.ToString("C");
                    string[] periodos = deuda.Estado.PeriodoGral.Split('-');
                    ViewState["bimestreI"] = deuda.Estado.PeriodoGral.Substring(0, 1);
                    ViewState["ejercicioI"] = periodos[0].Length < 7 ? deuda.Estado.PeriodoGral.Substring(2, 1) : deuda.Estado.PeriodoGral.Substring(2, 4);
                    ViewState["bimestreF"] = deuda.Estado.PeriodoGral.Substring(1, 1);
                    ViewState["ejercicioF"] = periodos[1].Length < 7 ? deuda.Estado.PeriodoGral.Substring(3, 1) : deuda.Estado.PeriodoGral.Substring(3, 4);
                    ViewState["importe"] = deuda.Importe;
                }
                lblMetrosLinealesText.Text = Convert.ToDouble(predio.MetrosFrente).ToString("N") + " M2 ";// " M&#178; ";
                lblMetrosLinealesLetra.Text=Letras(predio.MetrosFrente.ToString(), 1).ToUpper() ;
                lblZonaText.Text = predio.Zona.ToString();

                if (predio.BimestreFinSm >0)
                {
                    lblPeriodoPagoText.Text = predio.BimestreFinSm + "/" + predio.AaFinalSm;
                }
                else
                {
                    lblPeriodoPagoText.Text = "";
                }

                if (predio.FechaPagoSm != null)
                {
                    lblFechaPagoText.Text = Convert.ToDateTime(predio.FechaPagoSm).ToString("dd") + " DE "
                        + Convert.ToDateTime(predio.FechaPagoSm).ToString("MMMM") +
                         " DE " + Convert.ToDateTime(predio.FechaPagoSm).ToString("yyyy");
                }
                else
                {
                    lblFechaPagoText.Text = "";
                }
                if (predio.ReciboSm != null)
                {
                    lblREciboText.Text = predio.ReciboSm.ToString();
                    lblMensajePie.Text = lblMensajePie.Text + ", HABIENDO CUBIERTO LOS DERECHOS CAUSADOS, CON RECIBO OFICIAL No. " + predio.ReciboSm;
                }
                else
                {
                    lblREciboText.Text = "";
                    lblMensajePie.Text = lblMensajePie.Text + ".";
                }
            }

        }

        protected void llenarDatosCopia(int idTramite)
        {
           
            tTramite tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(idTramite));
            cPredio predio = tramite.cPredio;
            ViewState["idTramite"] = idTramite;
            ViewState["bimestreI"] = tramite.BimestreInicial;
            ViewState["bimestreF"] = tramite.BimestreFinal;
            ViewState["ejercicioI"] = tramite.EjercicioInicial;
            ViewState["ejercicioF"] = tramite.EjercicioFinal;
            ViewState["importe"] = tramite.Adeudo;
            btnPrepago.Visible = false; 
            btnImprimir.Visible = true;


            Boolean isAdeudo = false;            
            //if (Decimal.Compare(Convert.ToDecimal(tramite.Adeudo), 0.00) == 1)
            if (tramite.Adeudo < 1 )
            {
                isAdeudo = true;
            }
            //int tamaño = tramite.Periodo.Length;
            //int periodoInicial = Convert.ToInt32(tramite.Periodo.Substring(0, 1));
            //int ejercicioIncial = Convert.ToInt32(tramite.Periodo.Substring(2, 4));
            //int periodoFinal = Convert.ToInt32(tramite.Periodo.Substring(10, 1));
            //int ejercicioFinal = Convert.ToInt32(tramite.Periodo.Substring(12, 4));


            lblDirectorPredial.Text = "EL QUE SUSCRIBE " + new cParametroSistemaBL().GetValorByClave("DirectorPredial") +
                ", " + new cParametroSistemaBL().GetValorByClave("PuestoPredial") + ", DE "
                + new cParametroSistemaBL().GetValorByClave("NOMBRE_MUNICIPIO") + ", MORELOS";
            lblFechaEncabezado.Text = "QUE EN LOS ARCHIVOS DE ESTA OFICINA, AL " + DateTime.Now.Day + " DE " + DateTime.Now.ToString("MMMM") + " DEL " + DateTime.Now.ToString("yyyy") +
                ", SE ENCUENTRA REGISTRADO UN PREDIO, CON LOS SIGUIENTES DATOS:";
            cContribuyente contribuyente = predio.cContribuyente;
            lblNombreText.Text =  contribuyente.ApellidoPaterno.ToUpper() + " " + contribuyente.ApellidoMaterno.ToUpper()+ " "+ contribuyente.Nombre.ToUpper();
            lblCuentaCatastral.Text = "CLAVE CATASTRAL: ";
            lblCuentaCatastralText.Text = predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3);
            if (predio.ClavePredial.Substring(0, 1) == "0")
            {
                lblCuentaCatastral.Text = "CUENTA PREDIAL: ";
                lblCuentaCatastralText.Text = predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClaveAnterior.Substring(9, 3);
            }
            lblUbicacionText.Text = (predio.Calle + " " + predio.Numero + " Col. " + predio.cColonia.NombreColonia + " " +
                                    predio.CP + " " + predio.Localidad + ", MORELOS").ToUpper();
            lblPeriodoPagoText.Text = tramite.BimestreInicial + "/" + tramite.EjercicioInicial;       
            lblMensajePie.Text = "SE EXPIDE EL PRESENTE CERTIFICADO Y PARA LOS FINES LEGALES QUE CORRESPONDAN";

            if (isAdeudo)
            {
                tRecibo recibos = new tReciboBL().GetByIdTramite(tramite.Id);
                if (recibos != null)
                {
                    lblFechaPagoText.Text = (recibos.FechaPago.ToString("dd") + " DE " + recibos.FechaPago.ToString("MMMM") + " DE " +
                    recibos.FechaPago.ToString("yyyy")).ToUpper();
                    lblMensajePie.Text = lblMensajePie.Text + ", HABIENDO CUBIERTO LOS DERECHOS CAUSADOS, CON RECIBO OFICIAL No. " + recibos.Id.ToString().PadLeft(6,'0');
                    lblREciboText.Text = recibos.Id.ToString();
                }
                else
                {
                    lblFechaPagoText.Text = "";
                     lblMensajePie.Text = lblMensajePie.Text + ", HABIENDO CUBIERTO LOS DERECHOS CAUSADOS.";
                }
                
            }
            else
            {
                lblFechaPagoText.Text = "";
                lblMensajePie.Text = lblMensajePie.Text + ".";
            }
            
            if (tramite.Tipo == "IP")
            {
                visibleLabel(true, 1);
                if (!isAdeudo)
                {
                    lblMensajeAdeudo.Text = "CERTIFICADO DE NO ADEUDO DEL IMPUESTO PREDIAL";
                }
                else
                {
                    lblMensajeAdeudo.Text = "CERTIFICADO DE ADEUDO DEL IMPUESTO PREDIAL";
                    lblMensajeAdeudo1.Text = "EL CUAL PRESENTA UN ADEUDO DEL BIMESTRE " + tramite.Periodo + " DE " + Convert.ToDouble(tramite.Adeudo).ToString("C");
                }

                lblSuperficiePredioText.Text = Convert.ToDouble(tramite.SuperficieTerreno).ToString("N") +" M2 ";//+ " M&#178; "
                lblSuperficiePredioLetra.Text = Letras(tramite.SuperficieTerreno.ToString(), 1).ToUpper(); 
                lblSuperficieConsText.Text = Convert.ToDouble(tramite.SuperficieConstruccion).ToString("N") + " M2 ";// " M&#178; 
                lblSuperficieConsLetra.Text= Letras(tramite.SuperficieConstruccion.ToString(), 1).ToUpper();

                lblValorCastText.Text = predio.ValorCatastral.ToString("C");
                lblValorCastLetra.Text = " " + Letras(predio.ValorCatastral.ToString(), 2).ToUpper();      

            }
            else 
            {
                visibleLabel(true, 2);
                if (isAdeudo)
                {
                    lblMensajeAdeudo.Text = "CERTIFICADO DE NO ADEUDO DE SERVICIOS MUNICIPALES";
                }
                else
                {
                    lblMensajeAdeudo.Text = "CERTIFICADO DE ADEUDO DE SERVICIOS MUNICIPALES";
                    lblMensajeAdeudo1.Text = "EL CUAL PRESENTA UN ADEUDO DEL BIMESTRE " + tramite.Periodo + " AL" +
                        " DE " + tramite.Adeudo;
                }
                lblMetrosLinealesText.Text = Convert.ToDouble(predio.MetrosFrente).ToString("N") + " M2 ";// " M&#178; " + "(" + 
                    lblMetrosLinealesLetra.Text= Letras(predio.MetrosFrente.ToString(), 1).ToUpper() ;
                lblZonaText.Text = predio.Zona.ToString();                               
            }
        }

        protected void limpiarDatos()
        {

            lblMensajeAdeudo.Text = "";
            lblDirectorPredial.Text = "";
            lblFechaEncabezado.Text = "";
            lblNombreText.Text = "";
            lblCuentaCatastralText.Text = "";
            lblUbicacionText.Text = "";
            lblPeriodoPagoText.Text = "";
            lblREciboText.Text = "";
            lblFechaPagoText.Text = "";
            lblSuperficiePredioText.Text = "";
            lblSuperficiePredioLetra.Text = "";
            lblSuperficieConsText.Text = "";
            lblSuperficieConsLetra.Text = "";
            lblValorCastText.Text = "";
            lblValorCastLetra.Text = "";
            lblMetrosLinealesText.Text = "";
            lblMetrosLinealesLetra.Text = "";
            lblZonaText.Text = "";
            lblMensajeAdeudo1.Text = "";
            lblMensajePie.Text = "";
            lblMensajeTPendiente.Text = "";
            ViewState["idTramite"] = 0;
            ViewState["bimestreI"] = 0;
            ViewState["bimestreF"] = 0;
            ViewState["ejercicioI"] = 0;
            ViewState["ejercicioF"] = 0;
            ViewState["importe"] = 0;

        }

        protected void visibleLabel(Boolean activo, int tipoServicio)
        {            
            lblMensajeAdeudo.Visible = activo;
            lblDirectorPredial.Visible = activo;
            lblCertifica.Visible = activo;
            lblFechaEncabezado.Visible = activo;
            lblNombreText.Visible = activo;
            lblNombre.Visible = activo;
            lblCuentaCatastral.Visible = activo;
            lblCuentaCatastralText.Visible = activo;
            lblUbicacion.Visible = activo;
            lblUbicacionText.Visible = activo;
            lblPeriodoPago.Visible = activo;
            lblPeriodoPagoText.Visible = activo;
            lblRecibo.Visible = activo;
            lblREciboText.Visible = activo;
            lblFechaPago.Visible = activo;
            lblFechaPagoText.Visible = activo;
            lblMensajeAdeudo1.Visible = activo;
            lblMensajePie.Visible = activo;

            switch (tipoServicio)
            {
                case 1:
                    {//Para el Predial
                        lblSuperficiePredio.Visible = activo;
                        lblSuperficiePredioText.Visible = activo;
                        lblSuperficiePredioLetra.Visible = activo;
                        lblSuperficieCons.Visible = activo;
                        lblSuperficieConsText.Visible = activo;
                        lblSuperficieConsLetra.Visible = activo;
                        lblValorCast.Visible = activo;
                        lblValorCastText.Visible = activo;
                        lblValorCastLetra.Visible = activo;

                        lblMetrosLineales.Visible = !activo;
                        lblMetrosLinealesText.Visible = !activo;
                        lblMetrosLinealesLetra.Visible = !activo;
                        lblZona.Visible = !activo;
                        lblZonaText.Visible = !activo;
                        break;
                    }
                case 2:
                    { //Para el caso de servicio
                        lblMetrosLineales.Visible = activo;
                        lblMetrosLinealesLetra.Visible = activo;
                        lblMetrosLinealesText.Visible = activo;
                        lblZona.Visible = activo;
                        lblZonaText.Visible = activo;

                        lblSuperficiePredio.Visible = !activo;                        
                        lblSuperficiePredioText.Visible = !activo;
                        lblSuperficiePredioLetra.Visible = !activo;
                        lblSuperficieCons.Visible = !activo;
                        lblSuperficieConsText.Visible = !activo;
                        lblSuperficieConsLetra.Visible = !activo;
                        lblValorCast.Visible = !activo;
                        lblValorCastText.Visible = !activo;
                        lblValorCastLetra.Visible = !activo;
                        break;
                    }
                default:
                    {
                        lblSuperficiePredio.Visible = activo;
                        lblSuperficiePredioText.Visible = activo;
                        lblSuperficiePredioLetra.Visible = activo;
                        lblSuperficieCons.Visible = activo;
                        lblSuperficieConsText.Visible = activo;
                        lblSuperficieConsLetra.Visible = activo;
                        lblValorCast.Visible = activo;
                        lblValorCastText.Visible = activo;
                        lblValorCastLetra.Visible = activo;

                        lblMetrosLineales.Visible = activo;
                        lblMetrosLinealesLetra.Visible = activo;
                        lblMetrosLinealesText.Visible = activo;
                        lblZona.Visible = activo;
                        lblZonaText.Visible = activo;

                        break;
                    }


            }
        }

        protected void buscarClaveCatastral(object sender, System.EventArgs e)
        {
            limpiarDatos();
            visibleLabel(false, 0);

            int eActual = DateTime.Now.Year;
            double mesActual = Utileria.Redondeo(DateTime.Now.Month / 2.0);
            int bActual = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(DateTime.Now.Month / 2.0)));

            if (txtClvCastatral.Text.Length == 12)
            {
                cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
                if (Predio == null)
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);                    
                    txtClvCastatral.Text = "";
                }
                else
                {
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

                    else if (Predio.cStatusPredio.Descripcion == "A")
                    {
                        if (chkImprimirCopia.Checked)
                        {
                            chkImprimirCopia_CheckedChanged(null, null);
                        }
                        else
                        {
                            llenarDatos(Predio);
                        }
                    }
                    else
                    {
                        txtClvCastatral.Text = "";
                        vtnModal.ShowPopup(new Utileria().GetDescription(Predio.cStatusPredio.Descripcion == "B" ? MensajesInterfaz.sTatusPredioBaja : MensajesInterfaz.sTatusPredioSuspendido
                            ), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);               
                txtClvCastatral.Text = "";
            }
        }

        public string Letras(string numero,int tipo)
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

        protected void chkImprimirCopia_CheckedChanged(object sender, EventArgs e)
        {
            if (chkImprimirCopia.Checked)
            {
                if (txtClvCastatral.Text != "")
                {
                    grd.Visible = true;
                    llenagrid(txtClvCastatral.Text);
                    btnImprimir.Visible = false;
                    btnPrepago.Visible = false;
                    limpiarDatos();
                    visibleLabel(false, 0);
                }                
            }
            else
            {
                grd.Visible = false;
                limpiarDatos();
                visibleLabel(false, 0);
            }
        }

        protected void chkBimestreAnterior_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBimestreAnterior.Checked)
            {
                int mes = (Convert.ToInt16(ViewState["bActual"])) * 2;

                string dato = 01 + "/" + (mes - 1) + "/" + DateTime.Now.Year;
                txtFechaAdeudo.Text = Convert.ToDateTime(dato).AddDays(-1).ToString("dd/MM/yyyy");
            }
            else
            {
                txtFechaAdeudo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        protected void rdbPredial_CheckedChanged(object sender, EventArgs e)
        {
            txtClvCastatral.Text = "";
            chkImprimirCopia.Checked = false;
            chkBimestreAnterior.Checked = false;
            chkGratuito.Checked = false;
            lblMensajeAdeudo.BorderStyle = BorderStyle.None;
            txtFechaAdeudo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            limpiarDatos();
            visibleLabel(false, 0);
        }       

        private void llenagrid(String clavePredio)
        {
            String tipoTramite = new cParametroSistemaBL().GetValorByClave("Certificado");
            grd.DataSource = new vTramiteBL().GetFilterVTramiteCertificado(clavePredio, Convert.ToInt32(tipoTramite), "'I'", "'Id'", "asc");
            grd.DataBind();
            
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ConsultarRegistro")
            {
                string id = e.CommandArgument.ToString();
                llenarDatosCopia(Convert.ToInt32(id));
            }
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void chkGratuito_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGratuito.Checked)
            {
                btnImprimir.Visible = true;
                btnPrepago.Visible = false;
                lblMensajeTPendiente.Text = "";
                lblMensajeTPendiente.Visible = false;
            }
            else
            {
                btnImprimir.Visible = false;
                // btnPrepago.Visible = true; se deshabilita para no generar certificados 
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            imprimirCertificado();  
            if (!chkImprimirCopia.Checked)
            {
                if (!chkGratuito.Checked)
                {
                    tTramite tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idTramite"]));
                    tramite.Status = "I";
                    MensajesInterfaz msg = new tTramiteBL().Update(tramite);
                    btnImprimir.Visible = false;
                }
                else
                {
                    
                    cPredio predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
                    switch (validaTramite(predio)) {
                        case 1:case 2: {
                            tTramite tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idTramite"]));
                            tramite.Status = "I";
                            MensajesInterfaz msg = new tTramiteBL().Update(tramite);
                            btnImprimir.Visible = false;
                            break;                        
                        }
                        case 3:
                            {
                                btnPrepago_Click(null, null);
                                btnImprimir.Visible = false;
                                btnPrepago.Visible = false;
                                break;
                            }
                    }
                }
            }            
        }

        protected void imprimirCertificado()
        {
             fillPDF();
            //String urlPath = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
            //urlPath += "/Temporales/" + ViewState["pdf"].ToString();
            //String parametros = "','Certificados','height=600,width=550,toolbar=no,directories=no,menubar=no,scrollbars=no,resizable=no'";
            //String clienScript = "window.open('" + urlPath + parametros + ")";
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", clienScript, true);
             frameRecibo.Src = "~/Temporales/" + ViewState["pdf"].ToString();
             modalRecibo.Show();
             //frameRecibo.Attributes["src"] = ConfigurationManager.AppSettings["NombreSitioWeb"] + "/Temporales/" + ViewState["pdf"].ToString();
             String Clientscript = "printPdf();";
             ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", Clientscript, true);
        }

        protected void fillPDF()
        {

            String paht = Server.MapPath("~/");
            String formOriginal = paht + "Documentos/CertificadosPDF/" + obtenerPdf();
            String ruta = lblCuentaCatastralText.Text.Trim() +"_"+ obtenerPdf();
            String formImprimir = paht + "/Temporales/" + ruta;
            ViewState["pdf"] = ruta;
            int idPredio = Convert.ToInt32(ViewState["idPredio"].ToString());
            cPredio predio = new cPredioBL().GetByConstraint(idPredio);
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];

            PdfReader reader = new PdfReader(formOriginal);
            PdfStamper stam = new PdfStamper(reader,new FileStream(formImprimir,FileMode.Create));

            if (lblMensajeAdeudo.Text.ToUpper().Contains("NO"))
            {
                stam.AcroFields.SetField("Titulo", "CERTIFICADO DE NO ADEUDO");
            }
            else
            {
                stam.AcroFields.SetField("Titulo", "CERTIFICADO DE ADEUDO");
                stam.AcroFields.SetField("Adeudo", lblMensajeAdeudo1.Text);
            }
           // cCoPropietario cop = new cCoPropietario

            //stam.AcroFields.SetField("{{directorPredial}}", HttpUtility.HtmlDecode(new cParametroSistemaBL().GetValorByClave("DirectorPredial")));            
            //stam.AcroFields.SetField("{{subtitulo}}", HttpUtility.HtmlDecode(lblDirectorPredial.Text));
            //stam.AcroFields.SetField("{{dia}}", DateTime.Now.ToString("dd"));
            //stam.AcroFields.SetField("{{mes}}", DateTime.Now.ToString("MMMM").ToUpper());            
            //stam.AcroFields.SetField("{{ejercicio}}", DateTime.Now.ToString("yyyy"));
            stam.AcroFields.SetField("Propietario", HttpUtility.HtmlDecode(lblNombreText.Text));
            stam.AcroFields.SetField("Copropietario", "XXXXX");
            stam.AcroFields.SetField("Clavecatastral", lblCuentaCatastralText.Text);
            stam.AcroFields.SetField("txtClave", lblCuentaCatastral.Text);
            stam.AcroFields.SetField("Calle", predio.Calle);// HttpUtility.HtmlDecode(lblUbicacionText.Text));
            stam.AcroFields.SetField("Numero", predio.Numero.ToString()); //HttpUtility.HtmlDecode(lblUbicacionText.Text));
            stam.AcroFields.SetField("CP", predio.CP.ToString());
            stam.AcroFields.SetField("Colonia", predio.cColonia.NombreColonia.ToString());
            stam.AcroFields.SetField("Referencia", predio.Referencia.ToString());
            DateTime fecha = DateTime.Now;
            stam.AcroFields.SetField("Fecha",( Convert.ToDateTime(fecha).ToString("dd") + " DE "  +   (Convert.ToDateTime(fecha).ToString("MMMM")).ToUpper() +
                            " DE " + Convert.ToDateTime(fecha).ToString("yyyy"))); 
            if (rdbPredial.Checked)
            {
                stam.AcroFields.SetField("Superficie", predio.SuperficieTerreno.ToString() + " m2  A.C. +" + predio.TerrenoComun.ToString()+ " m2.");
                //stam.AcroFields.SetField("{{superficiePredioLetra}}", HttpUtility.HtmlDecode(lblSuperficiePredioLetra.Text)); 
                stam.AcroFields.SetField("Construccion", lblSuperficieConsText.Text);
                //stam.AcroFields.SetField("{{superficieConstruccionLetra}}", HttpUtility.HtmlDecode(lblSuperficieConsLetra.Text));
                stam.AcroFields.SetField("Valortotal", lblValorCastText.Text);
                stam.AcroFields.SetField("Valorcatastral", lblValorCastText.Text);

                //stam.AcroFields.SetField("{{valorCatastralLetra}}", HttpUtility.HtmlDecode(lblValorCastLetra.Text));

            }
            //else
            //{
            //    stam.AcroFields.SetField("{{metrosFrente}}", lblMetrosLinealesText.Text);
            //    stam.AcroFields.SetField("{{metrosFrenteLetra}}", HttpUtility.HtmlDecode(lblMetrosLinealesLetra.Text));
            //    stam.AcroFields.SetField("{{zona}}", HttpUtility.HtmlDecode(lblZonaText.Text));
            //}

            
            tRecibo reci = new tReciboBL().GetReciboCertificado(txtClvCastatral.Text.Trim());
            //if (chkGratuito.Checked == false && reci!=null)
            //stam.AcroFields.SetField("{{mensajePie}}", "Con el Recibo Oficial " + reci.Serie + reci.Id.ToString());
            
            stam.AcroFields.SetField("Periodopagado",lblPeriodoPagoText.Text +"   Fecha de pago:  "+ lblFechaPagoText.Text);
            stam.AcroFields.SetField("Recibo", lblREciboText.Text);
            stam.AcroFields.SetField("Adeudo", Convert.ToDecimal(ViewState["importe"]).ToString("C"));
            stam.AcroFields.SetField("Elaboro", HttpUtility.HtmlDecode(U.Usuario + " " + DateTime.Today.ToString()));
            //stam.AcroFields.SetField("{{fechaActual}}","A "+DateTime.Now.ToString("dd")+" DE "+ DateTime.Now.ToString("MMMM")+" DE "+DateTime.Now.ToString("yyyy") );  
            //stam.AcroFields.SetField("{{puestoPredio}}", new cParametroSistemaBL().GetValorByClave("PuestoPredial"));
            stam.FormFlattening = true;
            stam.Close();                                               
        }

        protected void imCerrarRecibo_Click(object sender, ImageClickEventArgs e)
        {
            modalRecibo.Hide();
        }

        protected String obtenerPdf()
        {
            Boolean copia = chkImprimirCopia.Checked;
            if (rdbPredial.Checked)
            {
                if (copia)
                {
                    return "Certificado_Predio_Copia_" + new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor + ".pdf";
                }
                else
                {
                    return "Certificado_Predio_" + new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor + ".pdf";
                }
            }
            else
            {
                if (copia)
                {
                    return "Certificado_Servicio_Copia_" + new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor + ".pdf";
                }
                else
                {
                    return "Certificado_Servicio_" + new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor + ".pdf";
                }
            }
        }

    }

}