using Catastro.Controles;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeneraRecibo33;
using System.IO;
using System.Text;

namespace CatastroPago.Recibos
{
    public partial class Facturacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                cParametroSistema municipio = new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO");
                if (municipio.Valor == "YAUTEPEC")
                {
                    ImagenLogo.ImageUrl = "~/Img/logo_yaute.jpg";
                    ViewState["colordiv"] = "#ff3399";
                    ImagenLogo.Height = 143;
                    ImagenLogo.Width = 239;
                }
                else
                {
                    ImagenLogo.ImageUrl = "~/Img/tlaltizapan.jpg";
                    ImagenLogo.Height = 123;
                    ImagenLogo.Width = 239;
                    ViewState["colordiv"] = "#ff1000";
                }

            }
        }
        public string colorDiv { get { return ViewState["colordiv"].ToString(); } }

        protected void btnCodigo_Click(object sender, EventArgs e)
        {
            tRecibo recibo = new tRecibo();
            recibo = new tReciboBL().GetByCodigoSeguridad(txtCodigo.Text);
            Reporte(recibo);
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
            cUsuarios U = new cUsuariosBL().GetByUsuario("INTERNET");
           

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
                modalFactura.Hide();
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);
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
                modalFactura.Hide();
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);
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
                //modalRecibo.Show();
                String Clientscript = "printPdf();";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", Clientscript, true);

                cRFC rfcObj = new cRfcBL().GetByRfc(RFC);
                if (rfcObj.RFC.Length > 1)
                {
                    byte[] arrayXml = Encoding.UTF8.GetBytes(fact.xml);
                    MensajesInterfaz correo = new Utileria().sendEMail(rfcObj.Email, "SERVICIO DE ENTREGA DE FACTURA.", "SERVICIO DE ENTREGA DE FACTURA. <br> Favor de no contestar este correo, debido a que es un servicio automatico.", new MemoryStream(arrayXml), new MemoryStream(fact.pdfBytes));
                    vtnModal.ShowPopup(new Utileria().GetDescription(correo), ModalPopupMensaje.TypeMesssage.Alert);
                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FacturaCorreoError), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            if (recibo.Facturado)
            {
                tbFacturar.Visible = false;
                divCerrarFactura.Visible = true;
            }
            else
            {
                tbFacturar.Visible = true;
                divCerrarFactura.Visible = false;
            }


        }

        protected void btnCancelarTodo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Recibos/Facturacion.aspx", false);
        }


        #endregion

        private void Reporte(tRecibo recibo)
        {

            ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
            string path = Server.MapPath("~/");
            Recibo rec;
            if (recibo.Facturado)
            {
                rec = reciboCFDI.consultaFacturaPDF(recibo, recibo.IdFIEL, path);
            }
            else
            {
                rec = reciboCFDI.consultaRecibo(recibo, recibo.IdFIEL, path);
            }


            // string urlpath = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
            FileStream newFile = new FileStream(path + "/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf", FileMode.Create);
            newFile.Write(rec.pdfBytes, 0, rec.pdfBytes.Length);
            newFile.Close();
            frameRecibo.Src = "~/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf";
            //lblCodigo1.Text = frameRecibo.Src;
            //frameRecibo.Attributes["src"] = "~/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf";

            pnlRecibo.Visible = true;
            //modalRecibo.Show();

            //entorno de desarrollo

            //entorno de produccion
            //frameRecibo.Attributes["src"] = ConfigurationManager.AppSettings["NombreSitioWeb"] + "/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf";
            ///Session["urlpdf"] = "/Recibos/Pdf/" + recibo.CodigoSeguridad + ".pdf";
            ViewState["IdRecibo"] = recibo.Id;
            if (recibo.Facturado)
            {
                tbFacturar.Visible = false;
                divCerrarFactura.Visible = true;
            }
            else
            {
                tbFacturar.Visible = true;
                divCerrarFactura.Visible = false;
            }
        }

        protected void btnCancelarBusqueda_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Recibos/Facturacion.aspx", false);
        }

        protected void btnDescargaXML_Click(object sender, EventArgs e)
        {
            int IdRecibo = Convert.ToInt32(ViewState["IdRecibo"]);

            ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
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

            ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
            tRecibo recibo = new tReciboBL().GetByConstraint(Convert.ToInt32(IdRecibo));
            string path = Server.MapPath("~/");
            Recibo RF = reciboCFDI.consultaFacturaPDF(recibo, recibo.IdFIEL, path);
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

            ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
            tRecibo recibo = new tReciboBL().GetByConstraint(Convert.ToInt32(IdRecibo));
            string path = Server.MapPath("~/");
            Recibo RF = reciboCFDI.consultaFacturaPDF(recibo, recibo.IdFIEL, path);


            byte[] arrayXml = Encoding.UTF8.GetBytes(RF.xml);
            MensajesInterfaz correo = new Utileria().sendEMail(txtCorreoEnvio.Text, "SERVICIO DE ENTREGA DE FACTURA.", "SERVICIO DE ENTREGA DE FACTURA. <br> Favor de no contestar este correo, debido a que es un servicio automatico.", new MemoryStream(arrayXml), new MemoryStream(RF.pdfBytes));
            vtnModal.ShowPopup(new Utileria().GetDescription(correo), ModalPopupMensaje.TypeMesssage.Alert);
        }
    }
}