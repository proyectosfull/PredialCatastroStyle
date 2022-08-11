using Catastro.Controles;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using FindAndReplace;
using iTextSharp.text.pdf;
using SharpMap.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace Catastro.Servicios
{
    public partial class ImprimePlano : System.Web.UI.Page
    {
        private SharpMap.Map myMap;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["parametro"] = null;
                visibleLabel(false);
                btnGenera.Visible = false;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);

            if (ViewState["mapZoom"] != null)
            {
                //Set up the map
                myMap = InitializeMap(new System.Drawing.Size((int)imgMap.Width.Value, (int)imgMap.Height.Value));
                if (Page.IsPostBack)
                {
                    //Page is post back. Restore center and zoom-values from viewstate
                    myMap.Center = (GeoAPI.Geometries.Coordinate)ViewState["mapCenter"];
                    myMap.Zoom = (double)ViewState["mapZoom"];
                }
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfirmacionGuardado), ModalPopupMensaje.TypeMesssage.Confirm);
        }

        protected void btnRecargar_Click(object sender, EventArgs e)
        {

            Response.Redirect("ImprimePlano.aspx");
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

        protected void btnGenera_Click(object sender, EventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            tTramite Tramite = new tTramite();
            cPredio predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
            string validacionFechaAvaluo = new cParametroSistemaBL().GetValorByClave("VALIDACIONFECHAAVALUO");
            if (validacionFechaAvaluo == "SI")
            {
                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan span = DateTime.Now - predio.FechaAvaluo;
                int years = (zeroTime + span).Year - 1;
                if (years < 2)
                {
                    MensajesInterfaz msgA = MensajesInterfaz.FechaAvaluoMenorDosAnios;
                    vtnModal.ShowPopup(new Utileria().GetDescription(msgA), ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }
            }

            Tramite.Fecha = DateTime.Now;
            Tramite.IdPredio = predio.Id;
            Tramite.IdTipoTramite = Convert.ToInt32(new cParametroSistemaBL().GetValorByClave("Plano"));
            Tramite.Status = "A";
            Tramite.IdUsuario = U.Id;
            Tramite.Activo = true;
            Tramite.FechaModificacion = DateTime.Now;
            Tramite.FechaOperacion = DateTime.Now;
            Tramite.SuperficieConstruccion = predio.SuperficieConstruccion;
            Tramite.SuperficieTerreno = predio.SuperficieTerreno;
            Tramite.Tipo = "IP";
            
            MensajesInterfaz msg = new tTramiteBL().Insert(Tramite);
            vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            buscarClaveCatastral(null, null);
           
        }

        protected void llenarDatos(cPredio predio)
        {
            visibleLabel(true);
            lblMunicipio.Text = predio.cContribuyente.Municipio;
            lblLocalidad.Text = predio.cContribuyente.Localidad;
            lblUbicacion.Text = predio.Calle + " No." + predio.Numero + " COL. " + predio.cColonia.NombreColonia;
            lblNombreCausante.Text =  predio.cContribuyente.ApellidoPaterno + " " + predio.cContribuyente.ApellidoMaterno + " "+ predio.cContribuyente.Nombre;
            lblDomicilioCausante.Text = predio.cContribuyente.Calle + " " + predio.cContribuyente.Numero + " " + predio.cContribuyente.Colonia;
            lblSuperficieTerreno.Text = predio.SuperficieTerreno.ToString() + " M2";
            lblValorTerreno.Text =  ((double)predio.ValorTerreno).ToString("C");
            lblSuperficieConstruccion.Text = predio.SuperficieConstruccion.ToString() + " M2";
            lblValorConstruccion.Text = ((double)predio.ValorConstruccion).ToString("C");
            lblValorTotal.Text = ((double)(predio.ValorTerreno + predio.ValorConstruccion)).ToString("C");
            int pendiente = validaTramite(predio);
            switch (pendiente)
            {
                case 1:
                    myMap = InitializeMap(new System.Drawing.Size((int)imgMap.Width.Value, (int)imgMap.Height.Value));
                    if (myMap.Layers.Count() > 0)
                    {
                        btnImprimir.Visible = true;
                        pnlMapa.Visible = true;
                        CreateMap();
                    }
                    btnImprimir.Visible = true;
                    btnGenera.Visible = false;
                    pnlMapa.Visible = true;
                    break;
                case 2:
                    lblMensajeTPendiente.Visible = true;
                    lblMensajeTPendiente.Text = ("TRAMITE PENDIENTE DE PAGO");
                    btnGenera.Visible = false;
                    btnImprimir.Visible = false;
                    break;
                case 3:
                    //btnGenera.Visible = true; se comenta para deshabilitar la generacion del tramite
                    btnImprimir.Visible = false;
                    break;
            }
            
        }

        protected int validaTramite(cPredio predio)
        {
            String tipoTramite = new cParametroSistemaBL().GetValorByClave("Plano");
            List<tTramite> lisTramite = new tTramiteBL().GetTiposTramiteIdPredioPlano(predio.Id, Convert.ToInt32(tipoTramite), "P");
            if (lisTramite.Count > 0)//Tramite pendiene para imprimir
            {
                ViewState["idTramite"] = lisTramite.FirstOrDefault().Id;
                return 1;
            }
            else
            {
                lisTramite = new tTramiteBL().GetTiposTramiteIdPredioPlano(predio.Id, Convert.ToInt32(tipoTramite), "A");
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

        protected void limpiarDatos()
        {

            lblMunicipio.Text = "";
            lblLocalidad.Text = "";
            lblUbicacion.Text = "";
            lblNombreCausante.Text = "";
            lblDomicilioCausante.Text = "";
            lblSuperficieTerreno.Text = "";
            lblValorTerreno.Text = "";
            lblSuperficieConstruccion.Text = "";
            lblValorConstruccion.Text = "";
            lblValorTotal.Text = "";
            //lblTPrivativo.Text = "";
            //lblTComun.Text = "";
            //lblCPrivativa.Text = "";
            //lblCComun.Text = "";
            lblMensajeTPendiente.Text = "";
            ViewState["idTramite"] = 0;
            pnlMapa.Visible = false;
            btnImprimir.Visible = false;

        }

        protected void visibleLabel(Boolean activo)
        {
            lblMunicipio.Visible = activo;
            lblLocalidad.Visible = activo;
            lblUbicacion.Visible = activo;
            lblNombreCausante.Visible = activo;
            lblDomicilioCausante.Visible = activo;
            lblSuperficieTerreno.Visible = activo;
            lblValorTerreno.Visible = activo;
            lblSuperficieConstruccion.Visible = activo;
            lblValorConstruccion.Visible = activo;
            lblValorTotal.Visible = activo;
            lblMunicipioL.Visible = activo;
            lblLocalidadL.Visible = activo;
            lblUbicacionL.Visible = activo;
            lblNombreCausanteL.Visible = activo;
            lblDomicilioCausanteL.Visible = activo;
            lblSuperficieTerrenoL.Visible = activo;
            lblValorTerrenoL.Visible = activo;
            lblSuperficieConstruccionL.Visible = activo;
            lblValorConstruccionL.Visible = activo;
            lblValorTotalL.Visible = activo;
            //lblTPrivativo.Visible = activo;
            //lblTComun.Visible = activo;
            //lblCPrivativa.Visible = activo;
            //lblCComun.Visible = activo;
        }

        protected void buscarClaveCatastral(object sender, System.EventArgs e)
        {
            ViewState["mapZoom"] = null;
            limpiarDatos();
            visibleLabel(false);

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
                    if (!Predio.Activo)
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInactivo), ModalPopupMensaje.TypeMesssage.Alert);
                        txtClvCastatral.Text = "";
                    }
                    else if (eActual * 10 + bActual > Predio.AaFinalIp * 10 + Predio.BimestreFinIp)
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("El predio tiene un adeudo, no es posible generar tramites catastrales"), ModalPopupMensaje.TypeMesssage.Alert);
                        txtClvCastatral.Text = "";
                    }
                    else if (Predio.cStatusPredio.Descripcion == "S")
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("El predio esta suspendido, favor de revisar la clave con el administrador"), ModalPopupMensaje.TypeMesssage.Alert);
                        txtClvCastatral.Text = "";
                    }
                    else if (Predio.cStatusPredio.Descripcion == "B")
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("El predio esta dado de baja, favor de revisar la clave con el administrador"), ModalPopupMensaje.TypeMesssage.Alert);
                        txtClvCastatral.Text = "";
                    }
                    else
                    {
                        //if (Predio.IdCartografia == null)
                        //    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioSinPlano), ModalPopupMensaje.TypeMesssage.Alert);
                        llenarDatos(Predio);
                    }
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);
                txtClvCastatral.Text = "";

            }
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
        
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            //imprimirCertificado();

            cPredio predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
            tTramite tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idTramite"]));
            if (predio != null)
            {
                string Path = Server.MapPath("~/");

                string inputFile = Path + "Documentos/PlanosPDF/PlanoCatastral_" + new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor + ".docx";
                //string outputFile = Path + "/Temporales/PlanoCatastral" + "-" + predio.ClavePredial.Trim() + "-" + DateTime.Now.Second.ToString() + ".docx";
                string nombre = "-" + predio.ClavePredial.Trim() + "-" + DateTime.Now.Second.ToString() + ".docx";
                string outputFile = Path + "/Temporales/" + nombre;
                File.Copy(inputFile, outputFile, true);

                #region reemplazar
                using (var flatDocument = new FlatDocument(outputFile))
                {

                    //valida si es clave o cuenta
                    if (predio.ClavePredial == "" || predio.ClavePredial == null)
                    {
                        flatDocument.FindAndReplace("«CuentaPredial»", predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? " " : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                        flatDocument.FindAndReplace("«ClaveCatastra»", " ");
                    }
                    else
                    {
                        if (predio.ClavePredial.Substring(0, 1) == "0") //cuentapredial
                        {
                            flatDocument.FindAndReplace("«CuentaPredial»", predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? " " : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                            flatDocument.FindAndReplace("«ClaveCatastra»", " ");
                        }
                        else//clavecatastral
                        {
                            flatDocument.FindAndReplace("«ClaveCatastral»", predio.ClavePredial == "" || predio.ClavePredial == null ? " " : predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                            flatDocument.FindAndReplace("«CuentaPredial»", predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? " " : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClaveAnterior.Substring(9, 3));
                        }
                    }

                    flatDocument.FindAndReplace("«Municipio»", "EMILIANO ZAPATA");
                    flatDocument.FindAndReplace("«Localidad»", predio.cColonia.NombreColonia + ", " + predio.Localidad);
                    flatDocument.FindAndReplace("«UbicacionPredio»", predio.Calle + " No." + predio.Numero + " COL. " + predio.cColonia.NombreColonia);
                    flatDocument.FindAndReplace("«Contribuyente»", predio.cContribuyente.ApellidoPaterno + " " + predio.cContribuyente.ApellidoMaterno + " " + predio.cContribuyente.Nombre + " " + predio.cContribuyente.RazonSocial);
                    flatDocument.FindAndReplace("«DomicilioContribuyente»", predio.cContribuyente.Calle + " " + predio.cContribuyente.Numero + " " + predio.cContribuyente.Colonia);
                    flatDocument.FindAndReplace("«Referencia»", predio.Referencia is null || predio.Referencia == "" ?"SN/R" : predio.Referencia);
                    flatDocument.FindAndReplace("«SupTerreno»", predio.SuperficieTerreno.ToString() + " M2" );
                    flatDocument.FindAndReplace("«ValTerreno»", ((double)predio.ValorTerreno).ToString("C"));
                    flatDocument.FindAndReplace("«SupConstruccion»", predio.SuperficieConstruccion.ToString() + " M2" + " m2  A.C. +" + predio.TerrenoComun.ToString() + " m2.");
                    flatDocument.FindAndReplace("«ValConstruccion»", ((double)predio.ValorConstruccion).ToString("C"));
                    flatDocument.FindAndReplace("«ValorCatastral»", ((double)(predio.ValorTerreno + predio.ValorConstruccion)).ToString("C"));
                    // tRecibo recibos = new tReciboBL().GetByIdTramite(Convert.ToInt32(ViewState["idTramite"]));
                    tRecibo recibos = new tReciboBL().GetByIdTramite(tramite.IdTramiteRef);
                    if (recibos != null)
                    {
                        flatDocument.FindAndReplace("«Recibo»", recibos.Id.ToString().PadLeft(6, '0'));
                        flatDocument.FindAndReplace("«Importe»", ((decimal)recibos.ImportePagado).ToString("C"));
                    }
                    cUsuarios U = new cUsuarios();
                    U = (cUsuarios)Session["usuario"];
                    flatDocument.FindAndReplace("«FechaAvaluo»", predio.FechaAvaluo.ToString("dd/MM/yyyy"));
                    flatDocument.FindAndReplace("«Usuario»", U.Usuario);
                    flatDocument.FindAndReplace("«FechaElaboracion»", DateTime.Now.ToString("dd/MMMM/yyyy"));
                    flatDocument.FindAndReplace("«DirectorPredial»", new cParametroSistemaBL().GetValorByClave("DIRECTORPREDIAL"));
                    flatDocument.FindAndReplace("«PuestoDirector»", new cParametroSistemaBL().GetValorByClave("PUESTOPREDIAL"));

                }
                #endregion

               
                tramite.Status = "I";
                MensajesInterfaz msg = new tTramiteBL().Update(tramite);
                btnImprimir.Visible = false;
                buscarClaveCatastral(null, null);

                Response.Redirect("~/Temporales/" + nombre);
                Response.Close();

                
            }

            
        }

        protected void imprimirCertificado()
        {
            //Llena un word
            fillPlanoDocx();
            
            ////Lena un pdf
            //fillPDF();
            ////insertaImg(); //se quita esta linea porque el mpio no entrego la cargtografia
            //String urlPath = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
            //urlPath += "/Temporales/"+ ViewState["pdf"].ToString();// + "result.pdf"; //+ ViewState["pdf"].ToString();            
            //String parametros = "','Planos','height=600,width=550,toolbar=no,directories=no,menubar=no,scrollbars=yes,resizable=yes'";
            //String clienScript = "window.open('" + urlPath + parametros + ")";
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", clienScript, true);
        }

        protected void fillPDF()
        {

            String paht = Server.MapPath("~/");
            String formOriginal = paht + "Documentos/PlanosPDF/" + obtenerPdf();
            String ruta = "Planos.pdf";
            String formImprimir = paht + "/Temporales/" + ruta;
            ViewState["pdf"] = ruta;

            PdfReader reader = new PdfReader(formOriginal);
            PdfStamper stam = new PdfStamper(reader, new FileStream(formImprimir, FileMode.Create));
            // stam.AcroFields.SetField("{{titulo}}", "COPIA CERTIFICADA DEL PLANO CATASTRAL");
            cPredio predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);

            if (predio.ClavePredial == "" || predio.ClavePredial == null)
            {
                stam.AcroFields.SetField("CuentaAnterior", predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? "" : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));

            }
            else
            {
                if (predio.ClavePredial.Substring(0, 1) == "0") //cuentapredial
                {
                    stam.AcroFields.SetField("ClaveCatastral", predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? "" : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                    stam.AcroFields.SetField("CuentaAnterior", predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? "" : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));

                }
                else//clavecatastral
                {
                    stam.AcroFields.SetField("ClaveCatastral", predio.ClavePredial == "" || predio.ClavePredial == null ? "  " : predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                    stam.AcroFields.SetField("CuentaAnterior", predio.ClavePredial == "" || predio.ClavePredial == null ? "  " : predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                }
            }

            //stam.AcroFields.SetField("ClaveCatastral", predio.ClavePredial);
            stam.AcroFields.SetField("Municipio", "EMILIANO ZAPATA");
            stam.AcroFields.SetField("Localidad", predio.Localidad + ", " + predio.cColonia.NombreColonia);
            stam.AcroFields.SetField("Ubicacion", predio.Calle + " No." + predio.Numero + " COL. " + predio.cColonia.NombreColonia);
            //stam.AcroFields.SetField("{{DIA}}", DateTime.Now.ToString("dd"));
            stam.AcroFields.SetField("Referencia", predio.Referencia);
            //DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            // stam.AcroFields.SetField("{{MES}}", formatoFecha.GetMonthName(int.Parse(DateTime.Now.ToString("MM"))).ToUpper());
            //stam.AcroFields.SetField("{{ANIO}}", DateTime.Now.ToString("yyyy"));
            stam.AcroFields.SetField("Contribuyente", predio.cContribuyente.RazonSocial + " " + predio.cContribuyente.ApellidoPaterno + " " + predio.cContribuyente.ApellidoMaterno + " " + predio.cContribuyente.Nombre);
            stam.AcroFields.SetField("Domicilio ", predio.cContribuyente.Calle + " " + predio.cContribuyente.Numero + " " + predio.cContribuyente.Colonia);
            stam.AcroFields.SetField("SupTerreno", predio.SuperficieTerreno.ToString() + " M2    A.C. " +  predio.TerrenoComun.ToString() + " M2.");
            stam.AcroFields.SetField("ValTerreno", ((double)predio.ValorTerreno).ToString("C"));
            stam.AcroFields.SetField("SupConstruccion", predio.SuperficieConstruccion.ToString() + " M2");
            stam.AcroFields.SetField("ValConstruccion", ((double)predio.ValorConstruccion).ToString("C"));
            stam.AcroFields.SetField("ValorCatastral", ((double)(predio.ValorTerreno + predio.ValorConstruccion)).ToString("C"));
            stam.AcroFields.SetField("FechaAvaluo", predio.FechaAvaluo.ToString("dd/MM/yyyy"));
            //stam.AcroFields.SetField("{{TPRIVATIVO}}", "T.Privativo " + predio.TerrenoPrivativo.ToString() + " m2");
            //stam.AcroFields.SetField("{{TCOMUN}}", "T.Común " + predio.TerrenoComun.ToString() + " m2");
            //stam.AcroFields.SetField("{{CPRIVATIVA}}", "C.Privativo " + predio.ConstruccionPrivativa.ToString() + " m2");
            //stam.AcroFields.SetField("{{CCOMUN}}", "C.Común " + predio.ConstruccionComun.ToString() + " m2");
            //stam.AcroFields.SetField("{{FIRMA}}", new cParametroSistemaBL().GetValorByClave("DIRECTORPREDIAL"));
            tRecibo recibos = new tReciboBL().GetByIdTramite(Convert.ToInt32(ViewState["idTramite"]));
            if (recibos != null)
            {
                stam.AcroFields.SetField("Recibo", recibos.Id.ToString().PadLeft(6, '0'));
                stam.AcroFields.SetField("Importe", recibos.ImportePagado.ToString("N2"));
            }
            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            stam.AcroFields.SetField("FechaAvaluo", predio.FechaAvaluo.ToString());
            stam.AcroFields.SetField("FechaAvaluo", predio.FechaAvaluo.ToString());
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            stam.AcroFields.SetField("Elaboro", U.Usuario + " " + DateTime.Now.ToString("dd/MMMM/yyyy"));


            stam.FormFlattening = true;
            stam.Close();
        }

        protected void fillPlanoDocx()
        {
            cPredio predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
            if (predio != null)
            {
                string Path = Server.MapPath("~/");

                string inputFile = Path + "Documentos/PlanosPDF/PlanoCatastral_" + new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor + ".docx";
                //string outputFile = Path + "/Temporales/PlanoCatastral" + "-" + predio.ClavePredial.Trim() + "-" + DateTime.Now.Second.ToString() + ".docx";
                string nombre = "-" + predio.ClavePredial.Trim() + "-" + DateTime.Now.Second.ToString() + ".docx";
                string outputFile = Path + "/Temporales/" + nombre;
                File.Copy(inputFile, outputFile, true);

                #region reemplazar
                using (var flatDocument = new FlatDocument(outputFile))
                {

                    //valida si es clave o cuenta
                    if (predio.ClavePredial=="" || predio.ClavePredial==null)
                    {
                        flatDocument.FindAndReplace("«CuentaPredial»", predio.ClaveAnterior =="" || predio.ClaveAnterior==null ? " " : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                        flatDocument.FindAndReplace("«ClaveCatastra»", " ");
                    }
                    else
                    {
                        if (predio.ClavePredial.Substring(0, 1) == "0") //cuentapredial
                        {
                            flatDocument.FindAndReplace("«CuentaPredial»", predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? " ": predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                            flatDocument.FindAndReplace("«ClaveCatastra»", " ");
                        }
                        else//clavecatastral
                        {
                            flatDocument.FindAndReplace("«ClaveCatastral»", predio.ClavePredial == "" || predio.ClavePredial == null ? " " : predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                            flatDocument.FindAndReplace("«CuentaPredial»", predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? " " : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClaveAnterior.Substring(9, 3));
                        }
                    }
                    
                    flatDocument.FindAndReplace("«Municipio»", "EMILIANO ZAPATA");
                    flatDocument.FindAndReplace("«Localidad»", predio.cColonia.NombreColonia+", "+ predio.Localidad);
                    flatDocument.FindAndReplace("«UbicacionPredio»", predio.Calle + " No." + predio.Numero + " COL. " + predio.cColonia.NombreColonia);
                    flatDocument.FindAndReplace("«Contribuyente»", predio.cContribuyente.ApellidoPaterno + " " + predio.cContribuyente.ApellidoMaterno + " " + predio.cContribuyente.Nombre +" "+ predio.cContribuyente.RazonSocial);   
                    flatDocument.FindAndReplace("«DomicilioContribuyente»", predio.cContribuyente.Calle + " " + predio.cContribuyente.Numero + " " + predio.cContribuyente.Colonia);
                    flatDocument.FindAndReplace("«Referencia»", predio.Referencia);
                    flatDocument.FindAndReplace("«SupTerreno»", predio.SuperficieTerreno.ToString() + " M2");
                    flatDocument.FindAndReplace("«ValTerreno»", ((double)predio.ValorTerreno).ToString("C"));
                    flatDocument.FindAndReplace("«SupConstruccion»", predio.SuperficieConstruccion.ToString() + " M2");
                    flatDocument.FindAndReplace("«ValConstruccion»", ((double)predio.ValorConstruccion).ToString("C"));
                    flatDocument.FindAndReplace("«ValorCatastral»", ((double)(predio.ValorTerreno + predio.ValorConstruccion)).ToString("C"));

                    tRecibo recibos;
                    
                    if (predio.Id >= 46233 && predio.Id <= 46399 && predio.Id != 46327)
                    {
                        string clv = new cParametroSistemaBL().GetValorByClave("PREDIO MASIVAS").ToString();
                        cPredio rPredio = new cPredioBL().GetByClavePredial(clv);
                        recibos = new tReciboBL().GetReciboCatastrobyIPredio(rPredio.Id);                       
                    }
                    else
                    {
                        recibos = new tReciboBL().GetByIdTramite(Convert.ToInt32(ViewState["idTramite"]));
                    }
                    if (recibos != null)
                    {
                        flatDocument.FindAndReplace("«Recibo»", recibos.Id.ToString().PadLeft(6, '0'));
                        flatDocument.FindAndReplace("«Importe»", recibos.ImportePagado.ToString());
                    }
                    cUsuarios U = new cUsuarios();
                    U = (cUsuarios)Session["usuario"];
                    flatDocument.FindAndReplace("«FechaAvaluo»", predio.FechaAvaluo.ToString("dd/MM/yyyy"));
                    flatDocument.FindAndReplace("«Usuario»", U.Usuario);
                    flatDocument.FindAndReplace("«FechaElaboracion»", DateTime.Now.ToString("dd/MMMM/yyyy"));
                    flatDocument.FindAndReplace("«DirectorPredial»", new cParametroSistemaBL().GetValorByClave("DIRECTORPREDIAL"));
                    flatDocument.FindAndReplace("«PuestoDirector»", new cParametroSistemaBL().GetValorByClave("PUESTOPREDIAL"));
                                                           
                }
                #endregion
                
                Response.Redirect("~/Temporales/" + nombre);
                Response.Close();
            }
        }

        protected void insertaImg()
        {
            String urlPath = Server.MapPath("~/");
            urlPath = urlPath + "/Temporales/";
            using (Stream inputPdfStream = new FileStream(urlPath + ViewState["pdf"].ToString(), FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageStream = new FileStream(urlPath + "planoImg.png", FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream outputPdfStream = new FileStream(urlPath + "result.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var reader = new PdfReader(inputPdfStream);
                var stamper = new PdfStamper(reader, outputPdfStream);
                var pdfContentByte = stamper.GetOverContent(1);

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
                image.SetAbsolutePosition(45, 330);
                pdfContentByte.AddImage(image);
                stamper.Close();
            }
        }

        protected String obtenerPdf()
        {
            return "Plano_Plantilla_" + new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor + ".pdf";
           
        }

        protected void imgMap_Click(object sender, ImageClickEventArgs e)
        {
            //Set center of the map to where the client clicked
            myMap.Center = SharpMap.Utilities.Transform.MapToWorld(new System.Drawing.Point(e.X, e.Y), myMap);

            //SharpMap
            //Set zoom value if any of the zoom tools were selected
            if (rblMapTools.SelectedValue == "0") //Zoom in
                myMap.Zoom = myMap.Zoom * 0.5;
            else if (rblMapTools.SelectedValue == "1") //Zoom out
                myMap.Zoom = myMap.Zoom * 2;
            //Save the new map's zoom and center in the viewstate
            ViewState.Add("mapCenter", myMap.Center);
            ViewState.Add("mapZoom", myMap.Zoom);
            //Create the map
            CreateMap();


        }

        /// <summary>
        /// Sets up the map, add layers and sets styles
        /// </summary>
        /// <param name="outputsize">Initiatial size of output image</param>
        /// <returns>Map object</returns>
        private SharpMap.Map InitializeMap(System.Drawing.Size outputsize)
        {
            //Initialize a new map of size 'imagesize'
            SharpMap.Map map = new SharpMap.Map(outputsize);
            String ruta = new cParametroSistemaBL().GetValorByClave("RutaShapes");
            cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
            if (Predio != null)
            {

                try {
                    //SharpMap.Layers.VectorLayer layArcos = new SharpMap.Layers.VectorLayer("arcos");
                    //layArcos.DataSource = new SharpMap.Data.Providers.ShapeFile(ruta + "predios" + Predio.IdCartografia + "\\arcos" + Predio.IdCartografia + ".shp", true);
                    //layArcos.Style.Fill = new SolidBrush(Color.Green);
                    //layArcos.Style.Outline = System.Drawing.Pens.Black;
                    //layArcos.Style.EnableOutline = true;

                    //SharpMap.Layers.VectorLayer layConstrucciones = new SharpMap.Layers.VectorLayer("construcciones");
                    //layConstrucciones.DataSource = new SharpMap.Data.Providers.ShapeFile(ruta + "predios" + Predio.IdCartografia + "\\construcciones" + Predio.IdCartografia + ".shp", true);
                    //layConstrucciones.Style.Line = new Pen(Color.Blue, 1);

                    //SharpMap.Layers.VectorLayer layPredios = new SharpMap.Layers.VectorLayer("predios");
                    //layPredios.DataSource = new SharpMap.Data.Providers.ShapeFile(ruta + "predios" + Predio.IdCartografia + "\\predios" + Predio.IdCartografia + ".shp", true);
                    //layPredios.Style.Line = new Pen(Color.Blue, 1);


                    //SharpMap.Layers.VectorLayer layArcosB = new SharpMap.Layers.VectorLayer("arcosB");
                    //layArcosB.DataSource = new SharpMap.Data.Providers.ShapeFile(ruta + "\\arcos03.shp", true);
                    //layArcosB.Style.Fill = new SolidBrush(Color.Green);
                    //layArcosB.Style.Outline = System.Drawing.Pens.Black;
                    //layArcosB.Style.EnableOutline = true;

                    //SharpMap.Layers.VectorLayer layConstruccionesB = new SharpMap.Layers.VectorLayer("construccionesB");
                    //layConstruccionesB.DataSource = new SharpMap.Data.Providers.ShapeFile(ruta + "\\construcciones03.shp", true);
                    //layConstruccionesB.Style.Line = new Pen(Color.Blue, 1);

                    SharpMap.Layers.VectorLayer layPrediosB = new SharpMap.Layers.VectorLayer("prediosB");
                    layPrediosB.DataSource = new SharpMap.Data.Providers.ShapeFile(ruta + "\\predios03.shp", true);
                    layPrediosB.Style.Line = new Pen(Color.Blue, 1);
                    GeoAPI.Geometries.Coordinate centro = new GeoAPI.Geometries.Coordinate();
                    for (int i=0; i< layPrediosB.DataSource.GetFeatureCount(); i++)
                    {
                        FeatureDataRow row = layPrediosB.DataSource.GetFeature((uint)i);
                        if ((row.ItemArray[20].ToString()) == Predio.IdCartografia) {
                            centro = row.Geometry.Centroid.Coordinate;
                            break;
                        }
                    }
                    //layPrediosB.DataSource.Open();
                    //GeoAPI.Geometries.Envelope ext = layPrediosB.DataSource..GetExtents();
                    //SharpMap.Data.Providers.ShapeFileProviderConfiguration sf = null;
                    //sf = new SharpMap.Data.Providers.ShapeFileProviderConfiguration(ruta + "\\predios03.shp");
                    //sf.Open(false);
                    //BoundingBox ext = sf.GetExtents();
                    //FeatureDataSet ds = new FeatureDataSet();
                    //layPrediosB.ExecuteIntersectionQuery(ext, ds);
                    //SharpMap.Data.FeatureDataTable table = ds.Tables[0] as FeatureDataTable;//TODO:
                    ////Read the .dbf data from the row.

                    //foreach (FeatureDataRow row in table.Rows)
                    //{
                    //    Polygon polygon = row.Geometry as NetTopologySuite.Geometries.Polygon;
                    //    if (polygon != null)
                    //    {
                            
                    //    }
                    //}
                    //layPrediosB.DataSource.Close();
                    //layPrediosB.DataSource.Dispose();
                    //SharpMap.Layers.VectorLayer layCallesB = new SharpMap.Layers.VectorLayer("callesB");
                    //layCallesB.DataSource = new SharpMap.Providers.ShapeFile(ruta + "\\calles03.shp", true);
                    //layCallesB.Style.Line = new Pen(Color.Blue, 1);

                    SharpMap.Layers.VectorLayer layNodosB = new SharpMap.Layers.VectorLayer("nodosB");
                    layNodosB.DataSource = new SharpMap.Data.Providers.ShapeFile(ruta + "\\nodos03.shp", true);
                    layNodosB.Style.Line = new Pen(Color.Blue, 1);

                    //map.Layers.Add(layArcos);
                    //map.Layers.Add(layConstrucciones);
                    //map.Layers.Add(layPredios);
                    //map.Layers.Add(layArcosB);
                    //map.Layers.Add(layConstruccionesB);
                    map.Layers.Add(layPrediosB);
                    // map.Layers.Add(layCallesB);
                    //map.Layers.Add(layNodosB);
                    if (ViewState["mapZoom"] == null)
                    {
                        map.Center = centro;
                        map.Zoom = 50;
                        ViewState.Add("mapCenter", map.Center);
                        ViewState.Add("mapZoom", map.Zoom);
                    }
                    
                }
                catch(Exception ex)
                {

                }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            }
            return map;
        }

        /// <summary>
        /// Creates the map, inserts it into the cache and sets the ImageButton Url
        /// </summary>
        private void CreateMap()
        {
            System.Drawing.Image img = myMap.GetMap();
            String urlPath = Server.MapPath("~/");
            urlPath = urlPath + "/Temporales/planoImg.png";
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                string base64String = Convert.ToBase64String(stream.ToArray(), 0, stream.ToArray().Length);
                imgMap.ImageUrl = "data:image/jpeg;base64," + base64String;
                using (var fileStream = new FileStream(urlPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    stream.Position = 0;
                    stream.WriteTo(fileStream); // fileStream is not populated
                }
                stream.Close();
            }
            img.Dispose();            
        }
                
        //protected void buscarClaveCatastral(object sender, ImageClickEventArgs e)
        //{
        //    txtClvCastatral.Text = "";
        //    limpiarDatos();
        //    btnGenera.Visible = false;
        //}        
    }

}