using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using Catastro.Controles;
using GeneraRecibo;
using GeneraRecibo33;
using System.Configuration;
using System.Globalization;
using System.Text;
using Catastro.ModelosFactura;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;

namespace Catastro.Recibos
{
    public partial class AdministrarRecibos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarControles();
                LimpiarControles();

            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void LimpiarControles()
        {
            txtFechaInicio.Visible = false;
            txtFechaFin.Visible = false;
            txtBusqueda.Visible = false;
            txtBusqueda.Text = "";
            rfvFechaInicio.Enabled = false;
            rfvFechaFin.Enabled = false;
            rfvBusqueda.Enabled = false;
            txtFechaInicio.Text = DateTime.Now.ToShortDateString();
            txtFechaFin.Text = DateTime.Now.ToShortDateString();
        }
        public void CargarControles()
        {

        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            llenarGrid();
        }

        public void llenarGrid()
        {
            int NoRecibo = 0;
            string clavepredial = string.Empty;
            //int folio = 0;
            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;
            string estado = "";


            if (ddlEstado.SelectedItem.Value.ToString() != "0")
            {
                switch (ddlEstado.SelectedItem.Value.ToString())
                {
                    case "P":
                        estado = "PAGADO";
                        break;
                    case "C":
                        estado = "CANCELADO";
                        break;
                }
            }

            if (Convert.ToInt32(ddlTipoBusqueda.SelectedItem.Value) == 1)
            {
                NoRecibo = Convert.ToInt32(txtBusqueda.Text);
            }
            if (Convert.ToInt32(ddlTipoBusqueda.SelectedItem.Value) == 2)
            {

                fechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
                fechaFin = Convert.ToDateTime(txtFechaFin.Text + " 23:59:59");
            }
            if (Convert.ToInt32(ddlTipoBusqueda.SelectedItem.Value) == 3)
            {
                clavepredial = txtBusqueda.Text.Trim();
            }
            List<vRecibo> recibos = new List<vRecibo>();
            if (estado != "")
            {
                recibos = new vVistasBL().ObtieneRecibo(NoRecibo, clavepredial,fechaInicio, fechaFin, estado);
            }
            else
            {
                recibos = new vVistasBL().ObtieneRecibo(NoRecibo, clavepredial ,fechaInicio, fechaFin);
            }
            
            List<vRecibo> recibosFinal = new List<vRecibo>();
            if (ddlEstado.SelectedItem.Value.ToString() == "0" && NoRecibo==0)
            {
                if (recibos.Count > 0)
                {
                    int rMin;
                    int rMax = recibos.Max(x => x.Id);
                    for (rMin = recibos.Min(x => x.Id); rMin <= rMax; rMin++)
                    {
                        vRecibo rec = recibos.FirstOrDefault(lrec => lrec.Id == rMin);
                        if (rec == null)
                        {
                            vRecibo rnew = new vRecibo();
                            rnew.Id = rMin;
                            rnew.EstadoRecibo = "INHABILITADO";
                            rnew.Contribuyente = "INHABILITADO POR EL SISTEMA";
                            rnew.Tipo = "-";
                            rnew.UsuarioCobra = "-";
                            rnew.UsuarioCancela = "-";
                            recibosFinal.Add(rnew);
                        }
                        else
                        {
                            recibosFinal.Add(rec);
                        }
                    }
                    grdRecibo.DataSource = recibosFinal;
                    grdRecibo.DataBind();
                }
                else
                {
                    grdRecibo.DataSource = null;
                    grdRecibo.DataBind();
                }
            }
            else
            {
                grdRecibo.DataSource = recibos;
                grdRecibo.DataBind();
            }            
        }
        protected void grdRecibo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            string id = e.CommandArgument.ToString();

            if (e.CommandName == "ConsultarRegistro")
            {
                //Dictionary<string, string> parametro = new Dictionary<string, string>();
                //tRecibo recibo = new tRecibo();
                //recibo = new tReciboBL().GetByConstraint(Convert.ToInt32(id));
                //Reporte(recibo);
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                string estatus = grdRecibo.DataKeys[gvr.RowIndex].Values[1].ToString();
                DatosFactura dat = new DatosFactura();
                dat.Get(id);
                if (dat.status)
                {
                    string QRPath = "";
                    string hoy = DateTime.Now.ToString(Constantes.formatoFechaArchivos);
                    string docURL = Server.MapPath("~/Temporales");
                    docURL += Constantes.recibosReimpresosSistemaFolder;

                    if (!System.IO.Directory.Exists(docURL))
                    {
                        System.IO.Directory.CreateDirectory(docURL);
                    }
                    docURL += U.Usuario+"-"+dat.pago.NumRecibo+"-"+ hoy + "-REIMPRESION.pdf";

                    dat.factura.FolioFiscal = dat.factura.FolioFiscal ?? "";
                    if (!dat.factura.FolioFiscal.Equals(""))
                    {
                        TimbradoEx tEx = new TimbradoEx();
                        QRPath = tEx.generarSelloQR(dat.factura.FolioFiscal, tEx.getEmisor().Rfc, dat.factura.RFCReceptor, dat.factura.Total, dat.factura.SelloComprobante,U.Usuario+"-"+ dat.pago.NumRecibo);
                    }
                    
                    DateTime fecha = DateTime.ParseExact(dat.factura.HoraFecha, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.CurrentCulture);
                    dat.factura.HoraFecha = fecha.ToString();
                    dat.pago.EsCancelado = (estatus.Equals("CANCELADO")); // Para mostrar mensaje de cancelado en el recibo.
                    dat.pago.CantidadLetra = Letras(dat.pago.PagoTotal.ToString(),0);
                    if (PDF.crearRecibo(dat.cliente, dat.pago, dat.factura, !dat.factura.FolioFiscal.Equals(""), docURL, QRPath, null))
                    {
                        //FileStream newFile = new FileStream(docURL, FileMode.Create);
                        //newFile.Write(rec.pdfBytes, 0, rec.pdfBytes.Length);
                        //newFile.Close();
                        //frameRecibo.Src = docURL;
                        //modalRecibo.Show();
                        frameRecibo.Src = "~/Temporales/sipred-files/Recibos expedidos/Reimpresos/" + U.Usuario + "-" + dat.pago.NumRecibo + "-" + hoy + "-REIMPRESION.pdf";
                        //btnSiguiente.Visible = true;
                        modalRecibo.Show();

                    }



                }
            }
            
            else if (e.CommandName == "TipoPago")
            {
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                string tipo = grdRecibo.DataKeys[gvr.RowIndex].Values[2].ToString();
                if (tipo.ToUpper() == "RECIBO")
                {
                    ddlTipoPago.Items.Clear();
                    ddlTipoPago.DataValueField = "Id";
                    ddlTipoPago.DataTextField = "Descripcion";
                    ddlTipoPago.DataSource = new cTipoPagoBL().GetAll();
                    ddlTipoPago.DataBind();
                    ddlTipoPago.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", ""));
                    tRecibo recibo = new tRecibo();
                    recibo = new tReciboBL().GetByConstraint(Convert.ToInt32(id));
                    ddlTipoPago.SelectedValue = recibo.IdTipoPago.ToString();
                    ViewState["idRecibo"] = id;
                    modalTipoPago.Show();
                }
                else
                {
                    vtnModal.ShowPopup("En facturas no esta permitido actualizar el tipo de pago.", ModalPopupMensaje.TypeMesssage.Alert);
                }                        
            }
            else if (e.CommandName == "FacturarPendiente")
            {
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                string tipo = grdRecibo.DataKeys[gvr.RowIndex].Values[2].ToString();

                ViewState["idRecibo"] = id;
                if (tipo.ToUpper() == "RECIBO")
                {
                    tRecibo FacturaCancelar = new tRecibo();
                    FacturaCancelar = new tReciboBL().GetByConstraint(Convert.ToInt32(id));
                    if (FacturaCancelar.EstadoRecibo == "P")
                    {
                        vtnModal.DysplayCancelar = true;
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfirmacionFacturaPendiente), ModalPopupMensaje.TypeMesssage.Confirm);
                    }
                }
            }
            else if (e.CommandName == "EliminarRegistro")
            {

                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                string tipo = grdRecibo.DataKeys[gvr.RowIndex].Values[2].ToString();

                ViewState["idRecibo"] = id;
                if (tipo.ToUpper() == "RECIBO" || tipo.ToUpper() == "CFDI")
                {
                    int prepoliza = new tPrepolizaReciboBL().GetByCountRecibo(Convert.ToInt32(id));
                    if (prepoliza == 0)
                    {
                        tRecibo reciboCancelar = new tRecibo();
                        reciboCancelar = new tReciboBL().GetByConstraint(Convert.ToInt32(id));
                        if (reciboCancelar.EstadoRecibo == "P")
                        {
                            vtnModal.DysplayCancelar = true;
                            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
                        }
                        else if (reciboCancelar.EstadoRecibo == "C")
                            vtnModal.ShowPopup("El recibo está cancelado", ModalPopupMensaje.TypeMesssage.Alert);
                    }
                    else
                    {
                        vtnModal.ShowPopup("El recibo no puede ser cancelado, ya se encuentra en el Reporte del Ingreso", ModalPopupMensaje.TypeMesssage.Alert);
                    }
                }
                //else
                //{
                //    tRecibo FacturaCancelar = new tRecibo();
                //    FacturaCancelar = new tReciboBL().GetByConstraint(Convert.ToInt32(id));
                //    if (FacturaCancelar.EstadoRecibo == "P")
                //    {
                //        vtnModal.DysplayCancelar = true;
                //        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfirmacionCancelaFactura), ModalPopupMensaje.TypeMesssage.Confirm);
                //    }
                //}
            }
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                pnlCancelar.Show();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.IngresoRFC))
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
            //cancela facturas
            //else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfirmacionCancelaFactura))
            //{
            //    tRecibo FacturaCancelar = new tRecibo();
            //    FacturaCancelar = new tReciboBL().GetByConstraint(Convert.ToInt32(ViewState["idRecibo"]));
            //    ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
            //    cUsuarios U = (cUsuarios)Session["usuario"];
            //    string usuarioFactura = ConfigurationManager.AppSettings["usuarioFactura"].ToString();
            //    string passwordFactura = ConfigurationManager.AppSettings["passwordFactura"].ToString();
            //    string respuestaCancelacion = reciboCFDI.CancelacionFactura(FacturaCancelar, U.Id, "", usuarioFactura, passwordFactura);
            //    if (respuestaCancelacion == "")
            //    {
            //        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.Actualizacion), ModalPopupMensaje.TypeMesssage.Alert);
            //        llenarGrid();
            //    }
            //    else
            //    {
            //        vtnModal.ShowPopup(new Utileria().GetDescription(respuestaCancelacion), ModalPopupMensaje.TypeMesssage.Alert);
            //        llenarGrid();

            //        DatosFactura dat = new DatosFactura();
            //        dat.Get(ViewState["idRecibo"].ToString());
            //        if (dat.status)
            //        {
            //            string QRPath = "";
            //            string hoy = DateTime.Now.ToString(Constantes.formatoFechaArchivos);
            //            string docURL = Constantes.recibosReimpresosSistemaFolder;
            //            if (!System.IO.Directory.Exists(docURL))
            //            {
            //                System.IO.Directory.CreateDirectory(docURL);
            //            }
            //            docURL += "/" + U.Usuario + "-" + dat.pago.NumRecibo + "-" + hoy + "-REIMPRESION.pdf";

            //            if ((dat.pago.Facturar && !dat.factura.FolioFiscal.Equals("")))
            //            {
            //                TimbradoEx tEx = new TimbradoEx();
            //                QRPath = tEx.generarSelloQR(dat.factura.FolioFiscal, dat.factura.RFCEmisor, dat.factura.RFCReceptor, dat.factura.Total, dat.factura.SelloComprobante);

            //                dat.pago.EsCancelado = true; // Para mostrar mensaje de cancelado en el recibo.
            //                if (PDF.crearRecibo(dat.cliente, dat.pago, dat.factura, (dat.pago.Facturar && !dat.factura.FolioFiscal.Equals("")), docURL, QRPath, null))
            //                {

            //                }
            //            }
            //        }
            //    }
            //}
            // factura pendiente
            


            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfirmacionFacturaPendiente))
            {
                tRecibo FacturaCancelar = new tRecibo();
                FacturaCancelar = new tReciboBL().GetByConstraint(Convert.ToInt32(ViewState["idRecibo"]));
                ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
                cUsuarios U = (cUsuarios)Session["usuario"];
                string usuarioFactura = ConfigurationManager.AppSettings["usuarioFactura"].ToString();
                string passwordFactura = ConfigurationManager.AppSettings["passwordFactura"].ToString();
                string idRecibo = ViewState["idRecibo"].ToString();
                
                DatosFactura dat = new DatosFactura();
                dat.Get(idRecibo);
                if (dat.status)
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                    con.Open();

                    SqlDataReader leer;
                    SqlCommand comm;
                    TimbradoEx tEx = new TimbradoEx();
                    ModelosFactura.Factura factura = new ModelosFactura.Factura();
                    ModelosFactura.Comprobante comprobante = new ModelosFactura.Comprobante();

                    List<Catastro.ModelosFactura.Concepto> listaConceptos = new List<Catastro.ModelosFactura.Concepto>();
                    string query = "select r.IdConcepto, c.Nombre, r.ImporteNeto, r.Cantidad, c.Cri, (select u.ClaveUnidad from cUnidadMedida u where u.Id=c.IdUnidadMedida) as unidadMedida, (select p.ClaveProdServ from cProdServ p where p.Id=c.IdProdServ) as idProdServ, r.ImporteDescuento from tReciboDetalle r join cConcepto c on r.IdConcepto=c.id where IdRecibo='" + idRecibo + "' order by r.ImporteTotal desc";
                    comm = con.CreateCommand();
                    comm.CommandTimeout = 0;
                    comm.CommandText = query;
                    leer = comm.ExecuteReader();
                    while (leer.Read())
                    {
                        Catastro.ModelosFactura.Concepto concepto = new Catastro.ModelosFactura.Concepto();
                        concepto.Cantidad = Convert.ToInt64(leer["Cantidad"].ToString());
                        concepto.Descripcion = leer["Nombre"].ToString();
                        concepto.Importe = this.formatearDecimales(leer["ImporteNeto"].ToString());
                        concepto.ValorUnitario = this.formatearDecimales(leer["ImporteNeto"].ToString());
                        concepto.NoIdentificacion = leer["Cri"].ToString();
                        concepto.ClaveUnidad = leer["unidadMedida"].ToString();
                        concepto.ClaveProdServ = leer["idProdServ"].ToString();
                        concepto.Descuento = this.formatearDecimales(leer["ImporteDescuento"].ToString());
                        listaConceptos.Add(concepto);
                        
                    }

                    comprobante.Conceptos = listaConceptos;
                    
                    query = "select r.Contribuyente, r.Rfc, (select Clave from cTipoPago tp where tp.Id=r.IdTipoPago) as formaPago, r.ImporteDescuento,r.ImporteNeto, r.ImportePagado, r.usoCfdi from tRecibo r where r.Id='" + idRecibo + "' ";
                    comm = con.CreateCommand();
                    comm.CommandTimeout = 0;
                    comm.CommandText = query;
                    leer = comm.ExecuteReader();
                    while (leer.Read())
                    {
                        //comprobante.Exportacion = "01";
                        //comprobante.Version = leer["VersionCDFI"].ToString();
                        comprobante.Emisor = tEx.getEmisor();
                        comprobante.Receptor = tEx.getReceptor(leer["Contribuyente"].ToString(), leer["Rfc"].ToString()); //tEx.getReceptor(leer["ReNombre"].ToString(), leer["ReRFC"].ToString(), leer["Otros"].ToString(), leer["UsoCDFI"].ToString(), leer["ReCodpostal"].ToString());
                        comprobante.CondicionesDePago = "Contado";
                        comprobante.FormaPago = tEx.getCvFormaPago(leer["formaPago"].ToString());
                        comprobante.MetodoPago = "PUE"; // en una sola exhibición
                        comprobante.TipoDeComprobante = "I"; // ingreso
                        comprobante.Fecha = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
                        comprobante.LugarExpedicion = Constantes.cpSistema; // cp tezoyuca
                        comprobante.Moneda = "MXN";
                        comprobante.TipoCambio = 1;
                        comprobante.NoCertificado = Constantes.noCertificadoEmisor;
                        //comprobante.Serie = "FW09";
                        dat.factura.UsoCfdi = leer["usoCfdi"].ToString() is null || leer["usoCfdi"].ToString() =="" ? "G03" : leer["usoCfdi"].ToString();
                        comprobante.Receptor.UsoCfdi = leer["usoCfdi"].ToString() is null || leer["usoCfdi"].ToString() == "" ? "G03" : leer["usoCfdi"].ToString();
                        comprobante.Serie = "U";
                        comprobante.SubTotal = this.formatearDecimales(decimal.Parse(leer["ImporteNeto"].ToString()).ToString());
                        comprobante.Descuento = this.formatearDecimales(decimal.Parse(leer["ImporteDescuento"].ToString()).ToString());
                        comprobante.Total = this.formatearDecimales(decimal.Parse(leer["ImportePagado"].ToString()).ToString());
                    }
                    factura.Comprobante = comprobante;



                    var json = factura.ToJson();
                    string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

                    ServiceFacturaXpress.ServicioTimbradoWSPortTypeClient te = new ServiceFacturaXpress.ServicioTimbradoWSPortTypeClient();
                    ServiceFacturaXpress.RespuestaTimbrado rt;
                    if (Constantes.modoPruebas)
                    {
                        rt = te.timbrarJSON(Constantes.apikey, base64, txtKeyDev.Text, txtCerDev.Text);
                    }
                    else
                    {
                        rt = te.timbrarJSON(Constantes.apikey, base64, txtKey.Text, txtCer.Text);
                    }

                    if (rt.code.Equals("200"))
                    {
                        string xmlPath = tEx.crearXMLTimbrado(rt.data,U.Usuario+"-"+ dat.pago.NumRecibo);
                        SqlXml newXml = new SqlXml(new XmlTextReader(xmlPath));
                        DateTime fechaF = DateTime.Parse(factura.Comprobante.Fecha);

                        // Actualizar cantidad de timbres restantes

                        try
                        {
                            Dictionary<string, string> xmlFF = ManejadorXML.obtenerFacturaTimbradaXML(xmlPath);

                            string folioFiscal = xmlFF["UUID"];
                            string noCertificado = xmlFF["NoCertificado"];
                            string noCertificadoSAT = xmlFF["NoCertificadoSAT"];
                            string fechaTimbrado = xmlFF["FechaTimbrado"];
                            string selloCFDI = xmlFF["SelloCFD"];
                            string selloSAT = xmlFF["SelloSAT"];
                            string selloComprobante = xmlFF["Sello"];

                            string cadenaOriginalCFDI = "||1.0|" + folioFiscal + "|" + factura.Comprobante.Fecha + "|" + selloCFDI + "|" + selloSAT;

                            string selloPath = tEx.generarSelloQR(folioFiscal, factura.Comprobante.Emisor.Rfc, factura.Comprobante.Receptor.Rfc, factura.Comprobante.Total.Trim(), selloComprobante,U.Usuario+"-"+ dat.pago.NumRecibo);

                            SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                            SqlTransaction sqlTran = con.BeginTransaction();
                            try
                            {
                                cmd = con.CreateCommand();
                                cmd.Transaction = sqlTran;
                                query = "INSERT INTO tReciboFactura (IdPredio,FolioFiscal,CertificadoEmisor,CertificadoSAT,FechahoraCertificado,SelloCDFI,selloSAT,Cadena,Fechafactura,NumCaja,selloDigital,xml,IdRecibo)  VALUES " +
                                    "(@IdPredio, @folioFiscal, @CertificadoEmisor, @CertificadoSAT, @FechahoraCertificado, @SelloCDFI, @selloSAT, @Cadena, @Fechafactura, @NumCaja, @selloDigital, @xml, @IdRecibo) ; ";
                                cmd.CommandText = query;
                                cmd.Parameters.AddWithValue("@IdPredio", dat.cliente.IdPredio);
                                cmd.Parameters.AddWithValue("@folioFiscal", folioFiscal);
                                cmd.Parameters.AddWithValue("@CertificadoEmisor", noCertificado);
                                cmd.Parameters.AddWithValue("@CertificadoSAT", noCertificadoSAT);
                                cmd.Parameters.AddWithValue("@FechahoraCertificado", fechaTimbrado);
                                cmd.Parameters.AddWithValue("@SelloCDFI", selloCFDI);
                                cmd.Parameters.AddWithValue("@selloSAT", selloSAT);
                                cmd.Parameters.AddWithValue("@Cadena", cadenaOriginalCFDI);
                                cmd.Parameters.AddWithValue("@Fechafactura", fechaF);
                                cmd.Parameters.AddWithValue("@NumCaja", U.IdUsuario);
                                cmd.Parameters.AddWithValue("@selloDigital", selloComprobante);
                                cmd.Parameters.AddWithValue("@xml", newXml.Value);
                                cmd.Parameters.AddWithValue("@IdRecibo", idRecibo);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();

                                
                                query = "update tRecibo set Facturado=@Facturado where Id=@IdRecibo; ";
                                cmd.CommandText = query;
                                cmd.Parameters.AddWithValue("@Facturado", "1");
                                cmd.Parameters.AddWithValue("@IdRecibo", idRecibo);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();


                                if (sqlTran != null)
                                {
                                    sqlTran.Commit();
                                }

                                dat.GetFactura(idRecibo);
                                if (dat.status)
                                {
                                    string QRPath = "";
                                    string hoy = DateTime.Now.ToString(Constantes.formatoFechaArchivos);
                                    string docURL = Server.MapPath("~/Temporales"); // Constantes.recibosReimpresosSistemaFolder;
                                    docURL += Constantes.recibosReimpresosSistemaFolder;

                                    if (!System.IO.Directory.Exists(docURL))
                                    {
                                        System.IO.Directory.CreateDirectory(docURL);
                                    }
                                    docURL += U.Usuario + "-" + dat.pago.NumRecibo + "-" + hoy + "-REIMPRESION.pdf";

                                    dat.pago.CantidadLetra = Letras(dat.pago.PagoTotal.ToString(),0);

                                    if (!dat.factura.FolioFiscal.Equals(""))
                                    {
                                        QRPath = tEx.generarSelloQR(dat.factura.FolioFiscal, tEx.getEmisor().Rfc, dat.factura.RFCReceptor, dat.factura.Total, selloPath,U.Usuario+"-"+ dat.pago.NumRecibo);

                                        DateTime fecha = DateTime.ParseExact(dat.factura.HoraFecha, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.CurrentCulture);
                                        dat.factura.HoraFecha = fecha.ToString();
                                        dat.pago.EsCancelado = false; // Para mostrar mensaje de cancelado en el recibo.
                                        if (PDF.crearRecibo(dat.cliente, dat.pago, dat.factura, (!dat.factura.FolioFiscal.Equals("")), docURL, QRPath, null))
                                        {
                                            frameRecibo.Src = "~/Temporales/sipred-files/Recibos expedidos/Reimpresos/" + U.Usuario + "-" + dat.pago.NumRecibo + "-" + hoy + "-REIMPRESION.pdf";
                                            //btnSiguiente.Visible = true;
                                            modalRecibo.Show();
                                        }

                                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ExitoFacturaPendiente), ModalPopupMensaje.TypeMesssage.Alert);
                                        llenarGrid();
                                    }
                                    else
                                    {
                                        vtnModal.ShowPopup("El recibo fue facturado pero no se pudo generar el archivo, intente una reimpresión.\n*El folio n se registró.", ModalPopupMensaje.TypeMesssage.Alert);
                                        llenarGrid();
                                    }
                                }  else
                                {
                                    vtnModal.ShowPopup("El recibo fue facturado pero no se pudo generar el archivo, intente una reimpresión.", ModalPopupMensaje.TypeMesssage.Alert);
                                    llenarGrid();
                                }
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ErrorFacturaPendiente) + "\n " + ex.Message, ModalPopupMensaje.TypeMesssage.Alert);
                                    llenarGrid();
                                    sqlTran.Rollback();
                                }
                                catch (Exception exRollback)
                                {
                                }
                            }
                            finally
                            {
                                if (con != null)
                                {
                                    cmd.Dispose();
                                    sqlTran.Dispose();
                                    con.Close();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ErrorFacturaPendiente) + "\n " + ex.Message, ModalPopupMensaje.TypeMesssage.Alert);
                            llenarGrid();
                        }
                    }
                    else
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ErrorFacturaPendiente) + "\n " + rt.message, ModalPopupMensaje.TypeMesssage.Alert);
                        llenarGrid();
                    }
                }
            }
        }

        
        //protected void grdRecibo_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //}

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

        protected void ddlTipoBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlTipoBusqueda.SelectedItem.Value)
            {
                case "1":
                    txtFechaInicio.Visible = false;
                    txtFechaFin.Visible = false;
                    txtBusqueda.Visible = true;
                    txtBusqueda.Text = "";
                    rfvFechaInicio.Enabled = false;
                    rfvFechaFin.Enabled = false;
                    rfvBusqueda.Enabled = true;
                    meeFiltro.Enabled = false;
                    break;
                case "2":
                    txtFechaInicio.Text = DateTime.Now.ToShortDateString();
                    txtFechaFin.Text = DateTime.Now.ToShortDateString();
                    txtFechaInicio.Visible = true;
                    txtFechaFin.Visible = true;
                    txtBusqueda.Visible = false;
                    rfvFechaInicio.Enabled = true;
                    rfvFechaFin.Enabled = true;
                    rfvBusqueda.Enabled = false;
                    meeFiltro.Enabled = false;
                    break;
                case "3":
                    txtFechaInicio.Visible = false;
                    txtFechaFin.Visible = false;
                    txtBusqueda.Visible = true;
                    txtBusqueda.Text = "";
                    rfvFechaInicio.Enabled = false;
                    rfvFechaFin.Enabled = false;
                    rfvBusqueda.Enabled = true;
                    meeFiltro.Enabled = true;
                    break;
                case "0":
                    txtFechaInicio.Visible = false;
                    txtFechaFin.Visible = false;
                    txtBusqueda.Visible = false;
                    txtBusqueda.Text = "";
                    rfvFechaInicio.Enabled = false;
                    rfvFechaFin.Enabled = false;
                    rfvBusqueda.Enabled = false;
                    break;
            }
            //if (ddlTipoBusqueda.SelectedItem.Value == "1") // por No. de recibo
            //{
            //    txtFechaInicio.Visible = false;
            //    txtFechaFin.Visible = false;
            //    txtBusqueda.Visible = true;
            //    txtBusqueda.Text = "";
            //    rfvFechaInicio.Enabled = false;
            //    rfvFechaFin.Enabled = false;
            //    rfvBusqueda.Enabled = true;
            //}
            //else if (ddlTipoBusqueda.SelectedItem.Value == "2") // por fechas
            //{
            //    txtFechaInicio.Text = DateTime.Now.ToShortDateString();
            //    txtFechaFin.Text = DateTime.Now.ToShortDateString();
            //    txtFechaInicio.Visible = true;
            //    txtFechaFin.Visible = true;
            //    txtBusqueda.Visible = false;
            //    rfvFechaInicio.Enabled = true;
            //    rfvFechaFin.Enabled = true;
            //    rfvBusqueda.Enabled = false;
            //}
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            tRecibo R = new tReciboBL().GetByConstraint(Convert.ToInt32(ViewState["idRecibo"]));
            int IdTipoTramite = R.tTramite.IdTipoTramite;

            cUsuarios U = (cUsuarios)Session["usuario"];
            R.MotivoCancelacion = txtMotivo.Text.Trim();
            R.IdUsuarioCancela = U.Id;
            R.FechaCancelacion = DateTime.Today;
            //MensajesInterfaz msgprim = new tReciboBL().Update(R);
            // PREDIAL
            if (IdTipoTramite == 5 || IdTipoTramite == 6)
            {
                SaldosC s = new SaldosC();
                MensajesInterfaz msg = s.CancelaRecibo(R.tTramite.Id, R);
                if (msg.Equals(MensajesInterfaz.Actualizacion))
                {
                    TimbradoEx tEx = new TimbradoEx();

                    ResultDAO result = tEx.cancelarTimbre(ViewState["idRecibo"].ToString(), "02");

                    vtnModal.ShowPopup(new Utileria().GetDescription(result.MESSAGE), ModalPopupMensaje.TypeMesssage.Alert);
                } else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            //CATASTRO
            else
            {
                MensajesInterfaz msg = new tTramiteBL().cancelarCobroOtrosConceptos(Convert.ToInt32( R.IdTramite), R);
                //vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                if (msg.Equals(MensajesInterfaz.Actualizacion))
                {
                    TimbradoEx tEx = new TimbradoEx();
                    ResultDAO result = tEx.cancelarTimbre(ViewState["idRecibo"].ToString(), "02");

                    vtnModal.ShowPopup(new Utileria().GetDescription(result.MESSAGE), ModalPopupMensaje.TypeMesssage.Alert);
                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }

            }



            //using (TransactionScope scope = new TransactionScope())
            //{
            //int idRecibo = Convert.ToInt32(ViewState["idRecibo"]);               
            //cUsuarios usuario = new cUsuarios();               
            //usuario = (cUsuarios)(Session["usuario"]);               
            //MensajesInterfaz msg = new tTramiteBL().CancelaReciboTramite(idRecibo, usuario.Id, txtMotivo.Text.Trim());                
            //vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            //}
            llenarGrid();
        }
        private void Reporte(tRecibo recibo)
        {
            string FechaFact33 = ConfigurationManager.AppSettings["FechaFact33"].ToString();
            string path = Server.MapPath("~/");
            if (recibo.FechaPago < DateTime.ParseExact(FechaFact33, "dd/MM/yyyy", CultureInfo.InvariantCulture))
            {
                ReciboCFDI reciboCFDI = new ReciboCFDI();
                GeneraRecibo.Recibo rec;
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
            }
            else
            {
                ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
                GeneraRecibo33.Recibo rec;
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
            }
            
            
            ViewState["IdRecibo"] = recibo.Id;
            if (recibo.Facturado)
            {
                //tbFacturar.Visible = false;
               // divCerrarFactura.Visible = true;
            }
            else
            {
                //tbFacturar.Visible = true;
                //divCerrarFactura.Visible = false;
            }
        }

        protected void grdRecibo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string estado = grdRecibo.DataKeys[e.Row.RowIndex].Values[1].ToString();
                string Tipo = grdRecibo.DataKeys[e.Row.RowIndex].Values[2].ToString();
                if (estado.ToUpper() == "CANCELADO")
                {
                    ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    imgDelete.Visible = false;
                }
                if (estado.ToUpper() == "INHABILITADO")
                {
                    ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    imgDelete.Visible = false;
                    ImageButton imgConsulta = (ImageButton)e.Row.FindControl("imgConsulta");
                    imgConsulta.Visible = false;
                }
                if (Tipo.ToUpper().Equals("CFDI") || !estado.ToUpper().Equals("PAGADO"))
                {
                    ImageButton imgFP = (ImageButton)e.Row.FindControl("imgFacturaPendiente");
                    imgFP.Visible = false;
                }
            }
        }

        protected void grdRecibo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdRecibo.PageIndex = e.NewPageIndex;
            llenarGrid();
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
        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            modalFactura.Show();
            
        }

        protected void btnCancelarTodo_Click(object sender, EventArgs e)
        {
            modalRecibo.Show();
           // Response.Redirect("~/Recibos/AdministrarRecibos.aspx", false);
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

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            btnGuardar.Visible = true;
            btnGeneraFactura.Visible = false;
            btnEditar.Visible = false;
            activaTxt(true);
            txtRFC.Enabled = false;

            modalFactura.Show();
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

        protected void btnGeneraFactura_Click(object sender, EventArgs e)
        {
            string RFC = ViewState["RFC"].ToString();
            int IdRecibo = (int)ViewState["IdRecibo"];
            string usuarioFactura = ConfigurationManager.AppSettings["usuarioFactura"].ToString();
            string passwordFactura = ConfigurationManager.AppSettings["passwordFactura"].ToString();
            bool productivoFactura = Convert.ToBoolean(ConfigurationManager.AppSettings["productivoFactura"]);

            ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
            GeneraRecibo33.Factura fact = reciboCFDI.generaFacturaRecibo(RFC, IdRecibo, usuarioFactura, passwordFactura, productivoFactura,ddlUsuCFDI.SelectedItem.Value);

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

                cRFC rfcObj = new cRfcBL().GetByRfc(RFC); 
                if(rfcObj.RFC.Length>1)
                {
                    byte[] arrayXml = Encoding.UTF8.GetBytes(fact.xml);
                    MensajesInterfaz correo = new Utileria().sendEMail(rfcObj.Email, "SERVICIO DE ENTREGA DE FACTURA.", "SERVICIO DE ENTREGA DE FACTURA. <br> Favor de no contestar este correo, debido a que es un servicio automatico.", new MemoryStream(arrayXml), new MemoryStream(fact.pdfBytes));
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription(correo), ModalPopupMensaje.TypeMesssage.Confirm);
                    llenarGrid();
                }
                else
                {
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FacturaCorreoError), ModalPopupMensaje.TypeMesssage.Confirm);
                    llenarGrid();
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
                // frameRecibo.Attributes["src"] = ConfigurationManager.AppSettings["NombreSitioWeb"] + "/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf";
            }


        }


        //private void creaXMLrecibo(int folioIni, int folioFin)
        //{
        //    tRecibo R;
        //    for (int i = folioIni; i <= folioFin; i++)
        //    {
        //        R = new tReciboBL().GetByConstraint(i);

        //        Comprobante comprobante = new Comprobante();
        //        Receptor receptor = new Receptor();
        //        DatosRecibo datosRecibo = new DatosRecibo();

        //        ////COMPROBANTE using GeneraRecibo;
        //        //comprobante.cajero = R. U.Nombre + " " + U.ApellidoPaterno + " " + U.ApellidoMaterno;
        //        //comprobante.FormaDePago = ddlMetodoPago.SelectedItem.Text; //"EN UNA SOLA EXHIBICIÓN";                    
        //        //comprobante.MetodoDePago = ddlMetodoPago.SelectedItem.Text; ;
        //        //comprobante.motivoDescuento = "";
        //        //comprobante.Serie = vSerie;
        //        //comprobante.Mesa = vMesa;
        //        //comprobante.TipoDeComprobante = "ingreso";
        //        //comprobante.Folio = r.Id.ToString().PadLeft(6, '0');
        //        //comprobante.SubTotal = Convert.ToDouble(vSubtotal);
        //        //comprobante.descuento = Convert.ToDouble(vDescuento);
        //        //comprobante.Total = Convert.ToDouble(vImporte);

        //        ////DATOS RECEPTOR   
        //        //receptor.Nombre = predio.cContribuyente.Nombre + ' ' + predio.cContribuyente.ApellidoPaterno + ' ' + predio.cContribuyente.ApellidoMaterno;
        //        //receptor.Calle = predio.Calle + ' ' + predio.Numero + ' ' + predio.cColonia.NombreColonia + ' ' + predio.Localidad;
        //        //receptor.CodigoPostal = predio.CP;
        //        //receptor.Colonia = predio.cColonia.NombreColonia;
        //        //receptor.Estado = new cParametroSistemaBL().GetValorByClave("ESTADO").ToString();
        //        //receptor.Localidad = predio.Localidad;
        //        //receptor.Municipio = new cParametroSistemaBL().GetValorByClave("NOMBRE_MUNICIPIO").ToString();
        //        //receptor.NoExterior = predio.Numero;
        //        //receptor.NoInterior = " ";
        //        //receptor.Pais = "MEXICO";
        //        //receptor.Referencia = predio.ClavePredial;
        //        //receptor.RFC = "XAXX010101000";
        //        //receptor.email = predio.cContribuyente.Email;
        //        //receptor.Id = predio.Id;

        //        ////LISTA DE CONCEPTOS             
        //        //datosRecibo.Conceptos_ = lConceptos;
        //        //datosRecibo.Comprobante_ = comprobante;
        //        //datosRecibo.Receptor_ = receptor;

        //    }

        //}

        protected void btnCancelarBusqueda_Click(object sender, EventArgs e)
        {

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

        }

        //protected void imbDescargar_Click(object sender, ImageClickEventArgs e)
        //{
        //    int IdRecibo = Convert.ToInt32(ViewState["IdRecibo"]);

        //    ReciboCFDI reciboCFDI = new ReciboCFDI();
        //    tRecibo recibo = new tReciboBL().GetByConstraint(Convert.ToInt32(IdRecibo));
        //    string path = Server.MapPath("~/");
        //    //Recibo RF = reciboCFDI.consultaFacturaPDF(recibo, recibo.IdFIEL, path);
        //    //Response.Clear();
        //    //MemoryStream msPdf = new MemoryStream(RF.pdfBytes);
        //    //Response.ContentType = "application/pdf";
        //    //Response.AddHeader("content-disposition", "attachment;filename=Factura.pdf");
        //    //Response.Buffer = true;
        //    //msPdf.WriteTo(Response.OutputStream);
        //    //Response.End();

        //    //byte[] arrayXml = Encoding.UTF8.GetBytes(RF.xml);
        //    byte[] arrayXml =  File.ReadAllBytes(path + recibo.RutaFactura);
        //    Response.Clear();
        //    MemoryStream ms = new MemoryStream(arrayXml);
        //    Response.ContentType = "text/xml";
        //    Response.AddHeader("content-disposition", "attachment;filename=Factura.xml");
        //    Response.Buffer = true;
        //    ms.WriteTo(Response.OutputStream);
        //    Response.End();

        //}

        protected void btnDescargaXML_Click(object sender, EventArgs e)
        {
            int IdRecibo = Convert.ToInt32(ViewState["IdRecibo"]);

            ReciboCFDI reciboCFDI = new ReciboCFDI();
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
            if (recibo.FechaPago < DateTime.ParseExact(FechaFact33, "dd/MM/yyyy", CultureInfo.InvariantCulture))
            {
                ReciboCFDI reciboCFDI = new ReciboCFDI();
                GeneraRecibo.Recibo RF = reciboCFDI.consultaFacturaPDF(recibo, recibo.IdFIEL, path);
                Response.Clear();
                MemoryStream msPdf = new MemoryStream(RF.pdfBytes);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Factura.pdf");
                Response.Buffer = true;
                msPdf.WriteTo(Response.OutputStream);
                Response.End();
            }
            else
            {
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
            if (recibo.FechaPago < DateTime.ParseExact(FechaFact33, "dd/MM/yyyy", CultureInfo.InvariantCulture))
            {
                ReciboCFDI reciboCFDI = new ReciboCFDI();
                GeneraRecibo.Recibo RF = reciboCFDI.consultaFacturaPDF(recibo, recibo.IdFIEL, path);
                byte[] arrayXml = Encoding.UTF8.GetBytes(RF.xml);
                MensajesInterfaz correo = new Utileria().sendEMail(txtCorreoEnvio.Text, "SERVICIO DE ENTREGA DE FACTURA.", "SERVICIO DE ENTREGA DE FACTURA. <br> Favor de no contestar este correo, debido a que es un servicio automatico.", new MemoryStream(arrayXml), new MemoryStream(RF.pdfBytes));
                vtnModal.ShowPopup(new Utileria().GetDescription(correo), ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
                GeneraRecibo33.Recibo RF = reciboCFDI.consultaFacturaPDF(recibo, recibo.IdFIEL, path);
                byte[] arrayXml = Encoding.UTF8.GetBytes(RF.xml);
                MensajesInterfaz correo = new Utileria().sendEMail(txtCorreoEnvio.Text, "SERVICIO DE ENTREGA DE FACTURA.", "SERVICIO DE ENTREGA DE FACTURA. <br> Favor de no contestar este correo, debido a que es un servicio automatico.", new MemoryStream(arrayXml), new MemoryStream(RF.pdfBytes));
                vtnModal.ShowPopup(new Utileria().GetDescription(correo), ModalPopupMensaje.TypeMesssage.Alert);
            }            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            modalRecibo.Show();
        }

        protected void btnGuardarTipoPago_Click(object sender, EventArgs e)
        {
            int IdRecibo = Convert.ToInt32(ViewState["idRecibo"]);
            tRecibo recibo = new tReciboBL().GetByConstraint(IdRecibo);
            recibo = new tReciboBL().GetByConstraint(Convert.ToInt32(IdRecibo));
            cTipoPago tipopago = new cTipoPagoBL().GetByConstraint(Convert.ToInt32(ddlTipoPago.SelectedValue.ToString()));

            recibo.IdTipoPago = tipopago.Id;
            MensajesInterfaz msg = new MensajesInterfaz();            
            Boolean cambioTipoPago = new ReciboCFDI33().cambioTipoPago(recibo.Ruta, tipopago.Clave);
            if (cambioTipoPago)
            {
                msg = new tReciboBL().Update(recibo);
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ErrorActualizar), ModalPopupMensaje.TypeMesssage.Alert);
            }
            ViewState["IdRecibo"] = null;
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
    }
}