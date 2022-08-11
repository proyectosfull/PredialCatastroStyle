using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Web;
using System.Xml;
using ZXing;
using ZXing.QrCode;
using System.Web;

namespace Catastro.ModelosFactura
{
    class TimbradoEx
    {
        public ResultDAO cancelarTimbre(string idRecibo, string motivo)
        {
            ResultDAO result = new ResultDAO();

            string folio = "";
            string total = "";
            string rfc = "";
            try
            {
                SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                cnx.Open();
                SqlCommand command = new SqlCommand("select f.FolioFiscal, r.Rfc, r.ImportePagado from tRecibo r join tReciboFactura f on r.Id=f.IdRecibo where r.Id='" + idRecibo + "';")
                {
                    Connection = cnx
                }; 
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    folio = reader["FolioFiscal"].ToString();
                    rfc = reader["Rfc"].ToString();
                    total = reader["ImportePagado"].ToString();
                }
                reader.Close();

                if (!folio.Equals(""))
                {
                    string refUUID = "";
                    TimbradoEx tEx = new TimbradoEx();
                    ServiceFacturaXpress.RespuestaCancelar rc = tEx.CancelarRecibo(folio, rfc, total, motivo, idRecibo, ref refUUID);
                    if (rc.code.Equals("201") || rc.code.Equals("202"))
                    {
                        result = new ResultDAO(true, "El recibo fue cancelado correctamente junto a su factura.\n" + refUUID);
                    }
                    else
                    {
                        result = new ResultDAO(true, "El recibo fue cancelado correctamente pero la factura no pudo cancelarse.\n" + rc.message);
                    }

                }
                else
                {
                    result = new ResultDAO(true, "El recibo fue cancelado correctamente pero la factura no pudo cancelarse porque no tiene un folio fiscal registrado.");
                }
            }
            catch (Exception ex)
            {
                result = new ResultDAO(true, "El recibo fue cancelado correctamente pero la factura no pudo cancelarse.\n" + ex.Message);
            }
            return result;
        }

        private ServiceFacturaXpress.RespuestaCancelar CancelarRecibo(string folioFiscal, string receptorRFC, string total, string motivo, string idRecibo, ref string refUUID)
        {
            //String ruta = HttpContext.Current.Server.MapPath("~/");
            Byte[] kbytes = File.ReadAllBytes(@"C:\sipred-files\csd\" + Constantes.keyName);
            String bkey = Convert.ToBase64String(kbytes);
            Byte[] cbytes = File.ReadAllBytes(@"C:\sipred-files\csd\" + Constantes.cerName);
            String bcer = Convert.ToBase64String(cbytes);

            SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

            ServiceFacturaXpress.ServicioTimbradoWSPortTypeClient te = new ServiceFacturaXpress.ServicioTimbradoWSPortTypeClient();
            ServiceFacturaXpress.RespuestaCancelar rc = te.cancelar2(Constantes.apikey, bkey, bcer, Constantes.pw, folioFiscal, Constantes.rfcEmisor, receptorRFC, double.Parse(total), motivo, "");

            if (rc.code.Equals("201") || rc.code.Equals("202"))
            {
                TimbradoEx tEx = new TimbradoEx();
                JObject json = JObject.Parse(rc.data);
                string xmlPath = new TimbradoEx().crearXMLAcuseCancelacion(json.GetValue("acuse").ToString(),"");

                string fecha = ManejadorXML.obtenerValorXML("Acuse", "Fecha", xmlPath);
                string emisorRFC = ManejadorXML.obtenerValorXML("Acuse", "RfcEmisor", xmlPath);
                string estatusUUID = ManejadorXML.obtenerInnerXML("EstatusUUID", xmlPath);
                string UUID = ManejadorXML.obtenerInnerXML("UUID", xmlPath);
                string modulus = ManejadorXML.obtenerInnerXML("Modulus", xmlPath);
                refUUID = UUID;


                SqlXml newXml = new SqlXml(new XmlTextReader(xmlPath));
                cnx.Open();
                string query = "update tReciboFactura set FechaCancelar='" + DateTime.Now.ToString("dd/MM/yyyy") + "' where IdRecibo='" + idRecibo + "';";
                SqlCommand comm = new SqlCommand(query, cnx);
                comm.ExecuteNonQuery();
                
                query = "insert into tReciboFacturaCancelada (idRecibo, motivo, fecha, emisorRFC, receptorRFC, total, estatusUUID, UUID, modulus, xml) ";
                query += " values(@idRecibo, @motivo, @fecha, @emisorRFC, @receptorRFC, @total, @estatusUUID, @UUID, @modulus, @xmlacuse);";
                SqlCommand command = new SqlCommand(query, cnx);
                command.Parameters.AddWithValue("@idRecibo", idRecibo);
                command.Parameters.AddWithValue("@motivo", motivo);
                command.Parameters.AddWithValue("@fecha", fecha);
                command.Parameters.AddWithValue("@emisorRFC", emisorRFC);
                command.Parameters.AddWithValue("@receptorRFC", receptorRFC);
                command.Parameters.AddWithValue("@total", total);
                command.Parameters.AddWithValue("@estatusUUID", estatusUUID);
                command.Parameters.AddWithValue("@UUID", UUID);
                command.Parameters.AddWithValue("@modulus", modulus);
                command.Parameters.AddWithValue("@xmlacuse", newXml.Value);

                command.ExecuteNonQuery();
                command.Parameters.Clear();
                command.Dispose();
                cnx.Close();
            }
            return rc;
        }



        public string crearXMLTimbrado(string xml, string usuario)
        {
            string hoy = DateTime.Now.ToString(Constantes.formatoFechaArchivos);
            string reciboPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Temporales"); //Server.MapPath("~/Temporales");
            reciboPath += Constantes.xmlSistemaFolder;

            if (!System.IO.Directory.Exists(reciboPath))
            {
                System.IO.Directory.CreateDirectory(reciboPath);
            }
            reciboPath += usuario +"-"+ hoy + "-XMLTIMBRADO.xml";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.PreserveWhitespace = true;
            doc.Save(reciboPath);
            return reciboPath;
        }

        public string crearXMLAcuseCancelacion(string xml, string usuario)
        {
            string hoy = DateTime.Now.ToString(Constantes.formatoFechaArchivos);
            string reciboPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Temporales"); //Server.MapPath("~/Temporales");
            reciboPath += Constantes.xmlSistemaFolder;
            if (!System.IO.Directory.Exists(reciboPath))
            {
                System.IO.Directory.CreateDirectory(reciboPath);
            }
            reciboPath += usuario +"-"+ hoy + "-XMLCANCELACION.xml";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.PreserveWhitespace = true;
            doc.Save(reciboPath);
            return reciboPath;
            //doc.Save(Environment.CurrentDirectory + "/Reports/xml-acuse.xml");
            //return Environment.CurrentDirectory + "/Reports/xml-acuse.xml";
        }

        public string generarSelloQR(string folioFiscal, string rfcEmisor, string rfcReceptor, string total, string sello, string usuario)
        {
            try
            {
                //String var = xmlFF["Sello"];
                //selloComprobante = var;
                int tam_var = sello.Length;
                String Var_Sub = sello.Substring((tam_var - 8), 8);
                //Gma.QrCodeNet.Encoding.Windows.Forms.QrCodeImgControl qrCode = new Gma.QrCodeNet.Encoding.Windows.Forms.QrCodeImgControl();
                //qrCode.Text = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?" + "&id=" + folioFiscal + "&re=" + factura.Comprobante.Emisor.Rfc + "&rr=" + factura.Comprobante.Receptor.Rfc + "&tt=" + factura.Comprobante.Total.Trim() + "&fe=" + Var_Sub;
                //qrCode.Image.Save(Constantes.xmlSistemaFolder + "/" + factura.Comprobante.Fecha + "-QR.jpg");
                string contenidoQR = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?" + "&id=" + folioFiscal + "&re=" + rfcEmisor + "&rr=" + rfcReceptor + "&tt=" + total + "&fe=" + Var_Sub;

                var hints = new Dictionary<EncodeHintType, object>
                {
                    [EncodeHintType.MARGIN] = 0
                };

                var qr = new QRCodeWriter();
                var matrix = qr.encode(contenidoQR, BarcodeFormat.QR_CODE, 120, 120, hints);

                var writerx = new BarcodeWriter { Options = { Margin = 0 } };

                string hoy = DateTime.Now.ToString(Constantes.formatoFechaArchivos);
                string selloPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Temporales");  //Server.MapPath("~/Temporales");
                selloPath += Constantes.xmlSistemaFolder;
                if (!System.IO.Directory.Exists(selloPath))
                {
                    System.IO.Directory.CreateDirectory(selloPath);
                }
                selloPath += usuario+"-"+hoy + "-SELLO-DIGITAL.png";

                using (var bitmap = writerx.Write(matrix))
                {
                    bitmap.MakeTransparent(System.Drawing.Color.White);
                    bitmap.Save(selloPath);
                }
                return selloPath;
            }
            catch (Exception)
            {
                return "";
            }


        }

        public Emisor getEmisor()
        {

            Emisor emisor = new Emisor();
            emisor.Rfc = Constantes.rfcEmisor;
            emisor.RegimenFiscal = Constantes.regimenFiscalEmisor;
            emisor.Nombre = Constantes.nombreEmisor;
            return emisor;
        }

        public Receptor getReceptorGeneral()
        {
            Receptor receptor = new Receptor();
            receptor.Nombre = "MUNICIPIO DE EMILIANO ZAPATA";
            receptor.Rfc = "XAXX010101000";
            receptor.UsoCfdi = "G03";
            return receptor;
        }

        public Receptor getReceptor(string nombre, string rfc)
        {
            Receptor receptor = new Receptor();
            receptor.Nombre = nombre;
            receptor.Rfc = rfc;
            receptor.UsoCfdi = "G03";
            return receptor;
        }

        public string getCvFormaPago(string formPago)
        {
            if (formPago != null)
            {
                List<List<string>> list = new List<List<string>>();
                list.Add(new List<string> { "01", "Efectivo" });
                list.Add(new List<string> { "02", "Cheque nominativo" });
                list.Add(new List<string> { "03", "Transferencia electrónica de fondos" });
                list.Add(new List<string> { "04", "Tarjeta de crédito" });
                list.Add(new List<string> { "06", "Dinero electrónico" });
                list.Add(new List<string> { "28", "Tarjeta de débito" });
                //01  Efectivo
                //02  Cheque nominativo
                //03  Transferencia electrónica de fondos
                //04  Tarjeta de crédito
                //05  Monedero electrónico
                //06  Dinero electrónico
                //08  Vales de despensa
                //12  Dación en pago
                //13  Pago por subrogación
                //14  Pago por consignación
                //15  Condonación
                //17  Compensación
                //23  Novación
                //24  Confusión
                //25  Remisión de deuda
                //26  Prescripción o caducidad
                //27  A satisfacción del acreedor
                //28  Tarjeta de débito
                //29  Tarjeta de servicios
                //30  Aplicación de anticipos
                //31  Intermediario pagos
                //99  Por definir
                foreach (List<string> f in list)
                {
                    if (f[1].ToLower().Equals(formPago.ToLower()))
                    {
                        return f[0];
                    }
                }
            }
            return "01";
        }

        public string getFormaPago(string cvFormaPAgo)
        {
            if (cvFormaPAgo != null)
            {
                List<List<string>> list = new List<List<string>>();
                list.Add(new List<string> { "01", "Efectivo" });
                list.Add(new List<string> { "02", "Cheque nominativo" });
                list.Add(new List<string> { "03", "Transferencia electrónica de fondos" });
                list.Add(new List<string> { "04", "Tarjeta de crédito" });
                list.Add(new List<string> { "06", "Dinero electrónico" });
                list.Add(new List<string> { "28", "Tarjeta de débito" });
                //01  Efectivo
                //02  Cheque nominativo
                //03  Transferencia electrónica de fondos
                //04  Tarjeta de crédito
                //05  Monedero electrónico
                //06  Dinero electrónico
                //08  Vales de despensa
                //12  Dación en pago
                //13  Pago por subrogación
                //14  Pago por consignación
                //15  Condonación
                //17  Compensación
                //23  Novación
                //24  Confusión
                //25  Remisión de deuda
                //26  Prescripción o caducidad
                //27  A satisfacción del acreedor
                //28  Tarjeta de débito
                //29  Tarjeta de servicios
                //30  Aplicación de anticipos
                //31  Intermediario pagos
                //99  Por definir
                foreach (List<string> f in list)
                {
                    if (f[0].Equals(cvFormaPAgo))
                    {
                        return f[1];
                    }
                }
            }
            return "Efectivo";
        }

        public string getMetodoPago(string cvMetodoPago)
        {
            if (cvMetodoPago != null)
            {
                List<List<string>> list = new List<List<string>>();
                list.Add(new List<string> { "PUE", "Pago en una sola exhibición" });
                list.Add(new List<string> { "PPD", "Pago en parcialidades o diferido" });
                foreach (List<string> f in list)
                {
                    if (f[0].Equals(cvMetodoPago))
                    {
                        return f[1];
                    }
                }
            }
            return "Pago en una sola exhibición";
        }

        public string getCvMetodoPago(string metodoPago)
        {
            if (metodoPago != null)
            {
                List<List<string>> list = new List<List<string>>();
                list.Add(new List<string> { "PUE", "Pago en una sola exhibición" });
                list.Add(new List<string> { "PPD", "Pago en parcialidades o diferido" });
                foreach (List<string> f in list)
                {
                    if (f[1].ToLower().Equals(metodoPago.ToLower()))
                    {
                        return f[0];
                    }
                }
            }
            return "PUE";
        }
    }
}