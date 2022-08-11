using System;
using System.Collections.Generic;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ZXing;
using ZXing.QrCode;
using DocumentFormat.OpenXml.Office2013.Excel;
using System.Web.UI;
using Clases.Utilerias;

namespace Catastro.ModelosFactura
{
    public class PDF
    {



        /// <summary> 
        ///     FORMATO PDF DEL RECIBO DE PAGO (itext)
        ///     </summary>
        public static bool crearRecibo(ClienteBean clienteBean, PagoBean pagoBean, FacturaBean facturaBean, bool facturar, string docURL, string selloPath, List<ConceptosRec> lConceptos)
        {
            try
            {
                // Creamos el documento con el tamaño de página tradicional
                Document doc = new Document(PageSize.A4);
                // Indicamos donde vamos a guardar el documento
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(docURL, FileMode.Create));

                

                // Abrimos el archivo
                doc.Open();

                // Creamos el tipo de Font que vamos utilizar
                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 9, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK);
                iTextSharp.text.Font _standardFontRED = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.RED);
                String ruta = HttpContext.Current.Server.MapPath("~/");
                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(ruta+"/Documentos/logo.png");
                imagen.BorderWidth = 0;
                imagen.Alignment = Element.ALIGN_LEFT;
                float percentage2 = 0.0f;
                percentage2 = 150 / imagen.Width;
                imagen.ScalePercent(percentage2 * 90);
                imagen.SetAbsolutePosition(30, 740);
                doc.Add(imagen);

                iTextSharp.text.Paragraph p = new Paragraph();
                p.Alignment = Element.ALIGN_RIGHT;
                p.Font = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
                p.Add(Constantes.nombreSistema);
                doc.Add(p);
                p.Clear();

                p.Alignment = Element.ALIGN_RIGHT;
                p.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
                p.Add(Constantes.nombreOficina);
                doc.Add(p);
                p.Clear();

                p.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);
                p.Add(Constantes.direccionSistema + "  RFC: " + Constantes.rfcEmisor);
                doc.Add(p);
                p.Clear();

                p.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);
                p.Add(Constantes.correoSistema+ " "+ Constantes.telefonoSistema+" "+  (facturaBean.HoraFecha is null || facturaBean.HoraFecha =="" ?  DateTime.Now.ToString() : facturaBean.HoraFecha )+ " \n ");
                doc.Add(p);
                p.Clear();

                //p.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);
                //p.Add(Constantes.telefonoSistema);
                //doc.Add(p);
                //p.Clear();

                //p.Font = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL);
                //p.Add("      " + DateTime.Now.ToString() + " \n ");
                //doc.Add(p);
                //p.Clear();
                //ENCABEZADO END========================


                iTextSharp.text.Font fontWhite = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.WHITE);
                iTextSharp.text.Font fontBlack = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
                iTextSharp.text.Font fontBold = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK);

                // INFORMACIÒN FISCAL ==================================================================================================
                if (facturar)
                {

                    PdfPTable tableInfo = new PdfPTable(2);
                    tableInfo.WidthPercentage = 55f;
                    tableInfo.HorizontalAlignment = Element.ALIGN_RIGHT;

                    PdfPCell header = new PdfPCell(new Phrase(" Informacion Fiscal:", fontWhite));
                    header.BackgroundColor = iTextSharp.text.Color.BLACK;
                    header.Colspan = 2;
                    tableInfo.AddCell(header);

                    PdfPCell cellInfo = new PdfPCell(new Phrase("   Folio Fiscal ", fontBold));
                    cellInfo.Colspan = 1;
                    cellInfo.PaddingTop = 6;
                    cellInfo.Border = 4;
                    cellInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tableInfo.AddCell(cellInfo);

                    cellInfo = new PdfPCell(new Phrase(facturaBean.FolioFiscal, fontBlack));
                    cellInfo.Colspan = 1;
                    cellInfo.PaddingTop = 6;
                    cellInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellInfo.Border = 8;
                    tableInfo.AddCell(cellInfo);

                    cellInfo = new PdfPCell(new Phrase("   No. de Serie del Certificado SAT ", fontBold));
                    cellInfo.Colspan = 1;
                    cellInfo.Border = 4;
                    cellInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tableInfo.AddCell(cellInfo);

                    cellInfo = new PdfPCell(new Phrase(facturaBean.CertificadoSAT, fontBlack));
                    cellInfo.Colspan = 1;
                    cellInfo.Border = 8;
                    cellInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tableInfo.AddCell(cellInfo);

                    cellInfo = new PdfPCell(new Phrase("   Fecha y Hora de Certificacion ", fontBold));
                    cellInfo.Colspan = 1;
                    cellInfo.Border = 4;

                    cellInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tableInfo.AddCell(cellInfo);

                    cellInfo = new PdfPCell(new Phrase(facturaBean.HoraFecha, fontBlack));
                    cellInfo.Colspan = 1;
                    cellInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellInfo.Border = 8;
                    tableInfo.AddCell(cellInfo);

                    cellInfo = new PdfPCell(new Phrase("   No. de Certificado CSD ", fontBold));
                    cellInfo.Colspan = 1;
                    cellInfo.Border = 6;
                    cellInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tableInfo.AddCell(cellInfo);

                    cellInfo = new PdfPCell(new Phrase(facturaBean.CertificadoEmisor, fontBlack));
                    cellInfo.Colspan = 1;
                    cellInfo.Border = 10;
                    cellInfo.PaddingBottom = 6;
                    cellInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tableInfo.AddCell(cellInfo);

                    doc.Add(tableInfo);
                }

                // INFORMACIÒN FISCAL===== END ===========================================================================================


                if (pagoBean.EsCancelado)
                {
                    p = new Paragraph();
                    p.Alignment = Element.ALIGN_RIGHT;
                    p.Font = _standardFontRED;
                    p.Add(" RECIBO CANCELADO**");
                    doc.Add(p);
                    p.Clear();
                }

                p = new Paragraph();
                p.Alignment = Element.ALIGN_LEFT;
                p.Font = _standardFont;
                p.SpacingBefore = 1;
                p.Add("No. recibo:    " + pagoBean.NumRecibo +"   "+"Cajero: " + clienteBean.UsuarioFacturo+ "\n ");
                doc.Add(p);
                p.Clear();


                // DATOS FISCALES DEL USUARIO =============================================================================================
                PdfPTable table = new PdfPTable(5);
                table.TotalWidth = 550f;
                table.LockedWidth = true;

                PdfPCell cell = new PdfPCell(new Phrase("  Datos Fiscales del Usuario", fontWhite));
                cell.Colspan = 5;
                cell.BackgroundColor = iTextSharp.text.Color.BLACK;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase("RFC: ", fontBlack));
                cell.Colspan = 1;
                cell.PaddingTop = 4;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(" "+clienteBean.Rfc, fontBlack));
                cell.Colspan = 2;
                cell.PaddingTop = 4;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Clave catastral: ", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                cell.PaddingTop = 4;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("  " + clienteBean.ClaveCatastral, fontBold));
                cell.Colspan = 1;
                cell.PaddingTop = 4;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Nombre: ", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(" "+clienteBean.NombreCompleto.Trim(), fontBlack));
                cell.Colspan = 2;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Cuenta: ", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(" "+clienteBean.CuentaCatastral, fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Dirección: ", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(clienteBean.Direccion, fontBlack));
                cell.Colspan = 2;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Tipo predio: ", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("  "+ clienteBean.ClaveGiro + " - " + clienteBean.Giro, fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                table.AddCell(cell);

                //cell = new PdfPCell(new Phrase("Referencia: ", fontBlack));
                //cell.Colspan = 1;
                //cell.Border = 0;     
                //table.AddCell(cell);

                //cell = new PdfPCell(new Phrase("  " + clienteBean.Referencia, fontBlack));
                //cell.Colspan = 1;
                //cell.Border = 0;
                //table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Área común: ", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("  " + clienteBean.AreaComun + " m2", fontBlack));
                cell.Colspan = 2;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(" Superficie terreno: " , fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("  " + clienteBean.SuperficieTerreno+" m2", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                table.AddCell(cell);

               

                cell = new PdfPCell(new Phrase("Base de impuesto: ", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("  "+clienteBean.BaseImpuesto, fontBlack));
                cell.Colspan = 2;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(" Superficie construcción: ", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("  " + clienteBean.SuperficieAreaConstruccion + " m2", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                table.AddCell(cell);
                

                cell = new PdfPCell(new Phrase("Uso de CFDI: ", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(facturaBean.UsoCfdi, fontBlack));
                cell.Colspan = 2;
                cell.Border = 0;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                table.AddCell(cell);

                doc.Add(table);

                //======================================= DIRECCION DE PREDIO

                table = new PdfPTable(12);
                table.TotalWidth = 550f;
                table.LockedWidth = true;

                cell = new PdfPCell(new Phrase("  Dirección de predio", fontWhite));
                cell.Colspan = 12;
                cell.BackgroundColor = iTextSharp.text.Color.BLACK;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase("Dirección: ", fontBlack));
                cell.Colspan = 1;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(clienteBean.DireccionPredio, fontBlack));
                cell.Colspan = 5;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Referencia: ", fontBlack));
                cell.Colspan = 1;
                cell.Border = 0;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("  " + clienteBean.Referencia, fontBlack));
                cell.Colspan = 5;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                cell.Border = 0;
                table.AddCell(cell);

                doc.Add(table);

                //======================================= FORMA DE PAGO
                table = new PdfPTable(12);
                table.TotalWidth = 550f;
                table.LockedWidth = true;

                cell = new PdfPCell(new Phrase("Forma de Pago", fontBold));
                cell.Colspan = 2;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                cell.Border = 1;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("PAGO EN UNA SOLA EXHIBICIÓN", fontBlack));
                cell.Colspan = 2;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Método de Pago", fontBold));
                cell.Colspan = 2;
                cell.Border = 1;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(pagoBean.FormaPago.ToUpper(), fontBlack));
                cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase("Condición de Pago", fontBold));
                cell.Border = 1;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("CONTADO", fontBlack));
                cell.Colspan = 2;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                table.AddCell(cell);

                doc.Add(table);

                //======================================= CONCEPTOS DE PAGO

                table = new PdfPTable(10);
                table.TotalWidth = 550f;
                table.LockedWidth = true;

                cell = new PdfPCell(new Phrase("Cuenta pública", fontWhite));
                cell.Colspan = 2;
                cell.BackgroundColor = iTextSharp.text.Color.BLACK;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Clave", fontWhite));
                cell.Colspan = 1;
                cell.BackgroundColor = iTextSharp.text.Color.BLACK;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Clave unidad", fontWhite));
                cell.Colspan = 1;
                cell.BackgroundColor = iTextSharp.text.Color.BLACK;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Concepto de pago", fontWhite));
                cell.Colspan = 3;
                cell.BackgroundColor = iTextSharp.text.Color.BLACK;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);
                
                cell = new PdfPCell(new Phrase("Importe Neto", fontWhite));
                cell.Colspan = 1;
                cell.BackgroundColor = iTextSharp.text.Color.BLACK;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Descuento", fontWhite));
                cell.Colspan = 1;
                cell.BackgroundColor = iTextSharp.text.Color.BLACK;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Total", fontWhite));
                cell.Colspan = 1;
                cell.BackgroundColor = iTextSharp.text.Color.BLACK;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                if (lConceptos != null)
                {
                    List<ConceptosRec> conceptos = lConceptos;
                    for (int i = 0; i < conceptos.Count; i++)
                    {
                        cell = new PdfPCell(new Phrase(conceptos[i].Id.ToString(), fontBlack));
                        cell.Colspan = 2;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(conceptos[i].ClaveProdServ, fontBlack));
                        cell.Colspan = 1;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(conceptos[i].ClaveUnidadMedida, fontBlack));
                        cell.Colspan = 1;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(conceptos[i].Descripcion, fontBlack));
                        cell.Colspan = 3;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        table.AddCell(cell);
                        
                        decimal descuento = decimal.Parse(conceptos[i].Descuento.ToString());
                        decimal importeNeto = decimal.Parse(conceptos[i].Importe.ToString());//importeTotal;
                        decimal importeTotal = importeNeto-descuento;

                        cell = new PdfPCell(new Phrase(" $ " + importeNeto.ToString("#,0.00", CultureInfo.InvariantCulture), fontBlack));
                        cell.Colspan = 1;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell);

                        
                        cell = new PdfPCell(new Phrase(" $ " + descuento.ToString("#,0.00", CultureInfo.InvariantCulture), fontBlack));
                        cell.Colspan = 1;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell);

                        
                        cell = new PdfPCell(new Phrase(" $ " + importeTotal.ToString("#,0.00", CultureInfo.InvariantCulture), fontBlack));
                        cell.Colspan = 1;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell);

                    }
                } else
                {
                    List<List<string>> conceptos = pagoBean.ConceptosPago;
                    for (int i = 0; i < conceptos.Count; i++)
                    {

                        cell = new PdfPCell(new Phrase(conceptos[i][0], fontBlack));
                        cell.Colspan = 2;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(conceptos[i][1], fontBlack));
                        cell.Colspan = 1;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(conceptos[i][2], fontBlack));
                        cell.Colspan = 1;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(conceptos[i][3], fontBlack));
                        cell.Colspan = 3;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        table.AddCell(cell);

                        decimal importeTotal = decimal.Parse(conceptos[i][4]);
                        decimal descuento = decimal.Parse(conceptos[i][5]);
                        decimal importeNeto = decimal.Parse(conceptos[i][6]);

                        cell = new PdfPCell(new Phrase(" $ " + importeNeto.ToString("#,0.00", CultureInfo.InvariantCulture), fontBlack));
                        cell.Colspan = 1;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(" $ " + descuento.ToString("#,0.00", CultureInfo.InvariantCulture), fontBlack));
                        cell.Colspan = 1;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell);
                        
                        cell = new PdfPCell(new Phrase(" $ " + importeTotal.ToString("#,0.00", CultureInfo.InvariantCulture), fontBlack));
                        cell.Colspan = 1;
                        cell.Border = 0;
                        cell.PaddingTop = 8;
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell);

                    }
                }


                cell = new PdfPCell(new Phrase("   " + pagoBean.Observaciones, fontBlack));
                cell.Colspan = 10;
                cell.Border = 0;
                cell.PaddingTop = 4;
                cell.PaddingBottom = 4;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Importe en Letra: ", fontBold));
                cell.Colspan = 2;
                cell.Border = 1;
                cell.PaddingTop = 8;
                cell.PaddingBottom = 8;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(pagoBean.CantidadLetra, fontBlack));
                cell.Colspan = 4;
                cell.Border = 1;
                cell.PaddingTop = 8;
                cell.PaddingBottom = 8;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Total     $" + decimal.Parse(pagoBean.PagoTotal).ToString("#,0.00", CultureInfo.InvariantCulture), _standardFont));
                cell.Colspan = 4;
                cell.Border = 1;
                cell.PaddingTop = 8;
                cell.PaddingBottom = 8;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

               

                cell = new PdfPCell(new Phrase("", fontBold));
                cell.Colspan = 10;
                cell.Border = 1;
                cell.PaddingTop = 8;
                table.AddCell(cell);

                

                doc.Add(table);

                //== SELLOS =====================================================

                //INICIO DE IMAGEN
                if (facturar)
                {


                    try
                    {
                        //iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(Constantes.xmlSistemaFolder + "/" + factura.Comprobante.Fecha + "-QR.jpg");
                        //iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(Environment.CurrentDirectory + "/Reports/selloDigital.png");
                        iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(selloPath);
                        imagen2.BorderWidth = 0;
                        imagen2.Alignment = Element.ALIGN_LEFT;
                        float percentage = 0.0f;
                        percentage = 100 / imagen2.Width;
                        imagen2.ScalePercent(percentage * 85);
                        imagen2.SetAbsolutePosition(30, 120);
                        doc.Add(imagen2);
                        //END
                    }
                    catch (Exception ex) { }


                    table = new PdfPTable(6);
                    table.TotalWidth = 550f;
                    table.LockedWidth = true;

                    cell = new PdfPCell(new Phrase(""));
                    cell.Colspan = 1;
                    cell.Border = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(" Sello digital CFDI:", fontBold));
                    cell.Colspan = 5;
                    cell.Border = 1;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(""));
                    cell.Colspan = 1;
                    cell.Border = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(" " + facturaBean.SelloDigitalCFDI, fontBlack));
                    cell.Colspan = 5;
                    cell.Border = 0;
                    cell.PaddingBottom = 8;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(""));
                    cell.Colspan = 1;
                    cell.Border = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(" Sello del SAT:", fontBold));
                    cell.Colspan = 5;
                    cell.Border = 1;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(""));
                    cell.Colspan = 1;
                    cell.Border = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(" " + facturaBean.SelloSAT, fontBlack));
                    cell.Colspan = 5;
                    cell.PaddingBottom = 8;
                    cell.Border = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(""));
                    cell.Colspan = 1;
                    cell.Border = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(" Cadena Original:", fontBold));
                    cell.Colspan = 5;
                    cell.Border = 1;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(""));
                    cell.Colspan = 1;
                    cell.Border = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(" " + facturaBean.CadenaOriginal, fontBlack));
                    cell.Colspan = 5;
                    cell.PaddingBottom = 17;
                    cell.Border = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(""));
                    cell.Colspan = 1;
                    cell.Border = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Este documento es una representación impresa de un CFDI", fontBold));
                    cell.Colspan = 3;
                    cell.PaddingBottom = 8;
                    cell.Border = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("C.c.p Archivo / Contabilidad", fontBold));
                    cell.Colspan = 2;
                    cell.PaddingBottom = 8;
                    cell.Border = 0;
                    table.AddCell(cell);

                    doc.Add(table);
                }



                doc.Close(); //cierra el archivo de codigo
                writer.Close();//cierra el archivo de codigo
                Process.Start(docURL); //ver el archivo final
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}