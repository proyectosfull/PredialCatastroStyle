using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using Clases.Utilerias;

namespace Catastro.ModelosFactura
{
    public class DatosFactura
    {
        public ClienteBean cliente;
        public PagoBean pago;
        public FacturaBean factura;
        //public bool usarDatosFiscales;

        public bool status;
        public string message;

        public DatosFactura()
        {
            this.cliente = new ClienteBean();
            this.pago = new PagoBean();
            this.factura = new FacturaBean();
            //this.usarDatosFiscales = usarDF;
            this.status = false;
            this.message = "";
        }

        public ResultDAO Get(string idRecibo)
        {
            SqlConnection cnx = null;
            SqlDataReader reader = null;
            SqlCommand command = null;
            SaldosC s = new SaldosC();
            Periodo per = new Periodo();
            MensajesInterfaz msg = new MensajesInterfaz();

            try
            {

                cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                cnx.Open();

                command = new SqlCommand
                {
                    Connection = cnx
                }; // Create a object of SqlCommand class



                int ii = 0;
                int total = 0;
                List<List<string>> Mconceptos = new List<List<string>>();
                string query = "Select cc.Cri as clavePublica, (select ClaveProdServ from cProdServ where Id=cc.IdProdServ) as idProdServ, (select ClaveUnidad from cUnidadMedida where Id=cc.IdUnidadMedida) as idUnidadMed, cc.Nombre as describe, c.IdConcepto as cvconcepto, c.ImporteDescuento as descuento, c.ImporteTotal as costo, c.ImporteNeto, tt.Id as idTramite, tt.Periodo as Periodo  from tReciboDetalle c join cConcepto cc on c.IdConcepto=cc.Id join tRecibo tr on tr.id = c.IdRecibo join tTramite tt on tr.IdTramite = tt.Id  where c.IdRecibo='" + idRecibo + "'  order by ImporteTotal desc";
                command.CommandText = query;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["describe"].ToString() != "")
                    {

                        List<string> aux = new List<string>();
                        aux.Add(reader["clavePublica"].ToString());
                        aux.Add(reader["idProdServ"].ToString());
                        aux.Add(reader["idUnidadMed"].ToString());
                        //aux.Add(reader["describe"].ToString());
                        
                        string idTramite = "";
                        string periodo = reader["Periodo"].ToString();
                        string concepto = reader["cvconcepto"].ToString();


                        
                        string obs = "";
                        string ms = "";
                        int bmstre;

                        if (concepto =="17")
                        {
                            //per = s.DevuelvePeriodoPago(Convert.ToInt32(periodo.Substring(0,1)), Convert.ToInt32(periodo.Substring( 2, 4)), Convert.ToInt32(periodo.Substring( 9, 1)), Convert.ToInt32(periodo.Substring( 12, 4)),"REZAGO");
                            //if(periodo is null)
                            per = s.DevuelvePeriodoPago(Convert.ToInt32(periodo.Substring(0, 1)), Convert.ToInt32(periodo.Substring(2, 4)), Convert.ToInt32(periodo.Substring(9, 1)), Convert.ToInt32(periodo.Substring(11, 4)), "REZAGO");
                            bmstre = s.CuentaBimestre(per.bInicial,per.eInicial,per.bFinal,per.eFinal, ref msg);
                            obs =" Bimestree pagados "+bmstre.ToString()+" , "+"Periodo pagado "+ per.bInicial.ToString() + " " + per.eInicial.ToString() + " al " + per.bFinal.ToString() + " " + per.eFinal.ToString();                            
                        }
                        if(concepto =="12" || concepto == "11")
                        {
                            per = s.DevuelvePeriodoPago(Convert.ToInt32(periodo.Substring(0, 1)), Convert.ToInt32(periodo.Substring(2, 4)), Convert.ToInt32(periodo.Substring(9, 1)), Convert.ToInt32(periodo.Substring(11, 4)), "ACTUAL");
                            bmstre = s.CuentaBimestre(per.bInicial, per.eInicial, per.bFinal, per.eFinal, ref msg);
                            obs = " Bimestree pagados " + bmstre.ToString() + " , " + "Periodo pagado " + per.bInicial.ToString() + " " + per.eInicial.ToString() + " al " + per.bFinal.ToString() + " " + per.eFinal.ToString();

                        }

                        aux.Add(reader["describe"].ToString() +" "+obs+" Subtotal $ "+ reader["ImporteNeto"].ToString());
                        
                        aux.Add(reader["costo"].ToString());
                        aux.Add(reader["descuento"].ToString());
                        aux.Add(reader["ImporteNeto"].ToString());


                        Mconceptos.Add(aux);
                        ii++;
                    }
                }
                total = ii;
                reader.Close();



                query = "select r.Id, r.Contribuyente, IdPredio, r.Rfc, r.Domicilio, p.ClavePredial, p.ClaveAnterior, r.usocfdi, (select Nombre from cTipoPago where Id=r.IdTipoPago) as formaPagoo, (select tp.Descripcion from cTipoPredio tp where tp.Id=p.IdTipoPredio) as tipoPredio, p.IdTipoPredio, r.ImportePagado, concat(p.Calle, ' #', p.Numero,', COL.',(select NombreColonia from cColonia where id=p.IdColonia),', C.P.',p.CP,', LOC. ',p.Localidad) as direccionPredio, p.Referencia, p.SuperficieTerreno, p.SuperficieConstruccion, p.TerrenoComun as areaComun, p.ValorCatastral as baseImpuesto, (select Usuario from cUsuarios where Id=r.IdUsuario) as usuario  from tRecibo r join tTramite t on r.IdTramite=t.Id join cPredio p on t.IdPredio=p.Id  where r.Id='" + idRecibo + "'";
                command.CommandText = query;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cliente.Giro = reader["tipoPredio"].ToString();
                    cliente.ClaveGiro = reader["IdTipoPredio"].ToString();

                    cliente.NombreCompleto = reader["Contribuyente"].ToString();
                    cliente.Direccion = reader["Domicilio"].ToString();
                    cliente.Rfc = reader["Rfc"].ToString();
                    cliente.CorreoElectronico = "S/N";

                    //factura.RFCEmisor = reader["RFCEmitio"].ToString();
                    factura.RFCReceptor = reader["Rfc"].ToString();
                    factura.Total = reader["ImportePagado"].ToString();
                    cliente.ClaveCatastral = reader["ClavePredial"].ToString() == "" || reader["ClavePredial"].ToString() == " " || reader["ClavePredial"].ToString()== null ? " ":reader["ClavePredial"].ToString().Substring(0, 1) == "0" ? "" : reader["ClavePredial"].ToString().Substring(0, 4) + "-" + reader["ClavePredial"].ToString().Substring(4, 2) + "-" + reader["ClavePredial"].ToString().Substring(6, 3) + "-" + reader["ClavePredial"].ToString().Substring(9, 3); ; //Validar con si tiene 0 vacia;
                    cliente.CuentaCatastral = reader["ClaveAnterior"].ToString() =="" || reader["ClaveAnterior"].ToString() ==" "|| reader["ClaveAnterior"].ToString()== null ? " " : reader["ClaveAnterior"].ToString().Substring(0, 4) + "-" + reader["ClaveAnterior"].ToString().Substring(4, 2) + "-" + reader["ClaveAnterior"].ToString().Substring(6, 3) + "-" + reader["ClaveAnterior"].ToString().Substring(9, 3);

                    // Datos predio creo falta laclave catastral 
                    cliente.IdPredio = reader["IdPredio"].ToString();
                    cliente.DireccionPredio = reader["direccionPredio"].ToString();
                    cliente.Referencia = reader["Referencia"].ToString();
                    cliente.SuperficieTerreno = reader["SuperficieTerreno"].ToString();
                    cliente.SuperficieAreaConstruccion = reader["SuperficieConstruccion"].ToString();
                    cliente.AreaComun = reader["areaComun"].ToString();
                    cliente.BaseImpuesto = reader["baseImpuesto"].ToString();
                    cliente.UsuarioFacturo = reader["usuario"].ToString();

                    decimal impbase = Convert.ToDecimal(cliente.BaseImpuesto);

                    cliente.BaseImpuesto = impbase.ToString("C");


                    factura.UsoCfdi = reader["usocfdi"].ToString() is null || reader["usocfdi"].ToString() == "" ? "G03" : reader["usocfdi"].ToString(); 

                    pago.NumRecibo = reader["Id"].ToString();
                    pago.ConceptosPago = Mconceptos;
                    pago.FechaRealizo = DateTime.Now.ToString("yyyy-MM-dd");
                    //pago.MesAnio = reader["mesconayo"].ToString();
                    //pago.LectorActual = reader["lecactual"].ToString();
                    //pago.LectorAnterior = reader["lecantes"].ToString();
                    pago.PagoTotal = reader["ImportePagado"].ToString();
                    //pago.CantidadLetra = reader["cantletra"].ToString();
                    //pago.Meses = reader["nmeses"].ToString();
                    //pago.ConsumoMedidor = reader["consumomed"].ToString();

                    //pago.Observaciones = reader["observa"].ToString();

                    pago.FormaPago = reader["formaPagoo"].ToString();
                    break;
                }
                reader.Close();
                
                query = "select FolioFiscal, CertificadoEmisor, CertificadoSAT, FechahoraCertificado, SelloCDFI, selloSAT, Cadena, selloDigital from tReciboFactura where IdRecibo='" + idRecibo + "'";
                command.CommandText = query;
                reader = command.ExecuteReader();
                while (reader.Read())
                {

                    pago.CondicionPago = "CONTADO";
                    //pago.FormaPago = reader["FormaPago"].ToString();
                    //pago.MetodoPago = reader["metodoPago"].ToString();
                    factura.HoraFecha = reader["FechahoraCertificado"].ToString();
                    //p.Observaciones = reader["observaciones"].ToString();
                    //pago.CantidadLetra = reader["cantletra"].ToString();
                    factura.FolioFiscal = reader["FolioFiscal"].ToString();
                    factura.CertificadoEmisor = reader["CertificadoEmisor"].ToString();
                    factura.CertificadoSAT = reader["CertificadoSAT"].ToString();
                    factura.SelloDigitalCFDI = reader["SelloCDFI"].ToString();
                    factura.SelloSAT = reader["selloSAT"].ToString();
                    factura.CadenaOriginal = reader["Cadena"].ToString();
                    factura.SelloComprobante = reader["selloDigital"].ToString();
                    //factura.RFCEmisor = reader["RFCEmitio"].ToString();
                    //factura.RFCReceptor = reader["ReRFC"].ToString();
                    //factura.Total = reader["total"].ToString();
                    pago.Facturar = true;

                }
                reader.Close();

                this.status = true;
                this.message = "El recibo se generó correctamente.";

            }
            catch (Exception ex)
            {
                this.status = false;
                this.message = "No se pudo generar el recibo.\n" + ex;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }

                if (cnx != null)
                {
                    command.Dispose();
                    cnx.Close();
                }

            }
            return new ResultDAO(this.status, this.message);

        }

        public ResultDAO GetFactura(string idRecibo)
        {
            SqlConnection cnx = null;
            SqlDataReader reader = null;
            SqlCommand command = null;

            try
            {

                cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                cnx.Open();

                command = new SqlCommand
                {
                    Connection = cnx
                }; // Create a object of SqlCommand class

                

                string query = "select FolioFiscal, CertificadoEmisor, CertificadoSAT, FechahoraCertificado, SelloCDFI, selloSAT, Cadena from tReciboFactura where IdRecibo='" + idRecibo + "'";
                command.CommandText = query;
                reader = command.ExecuteReader();
                while (reader.Read())
                {

                    pago.CondicionPago = "CONTADO";
                    //pago.FormaPago = reader["FormaPago"].ToString();
                    //pago.MetodoPago = reader["metodoPago"].ToString();
                    factura.HoraFecha = reader["FechahoraCertificado"].ToString();
                    //p.Observaciones = reader["observaciones"].ToString();
                    //pago.CantidadLetra = reader["cantletra"].ToString();
                    factura.FolioFiscal = reader["FolioFiscal"].ToString();
                    factura.CertificadoEmisor = reader["CertificadoEmisor"].ToString();
                    factura.CertificadoSAT = reader["CertificadoSAT"].ToString();
                    factura.SelloDigitalCFDI = reader["SelloCDFI"].ToString();
                    factura.SelloSAT = reader["selloSAT"].ToString();
                    factura.CadenaOriginal = reader["Cadena"].ToString();
                    //factura.SelloComprobante = reader["selloCFD"].ToString();
                    //factura.RFCEmisor = reader["RFCEmitio"].ToString();
                    //factura.RFCReceptor = reader["ReRFC"].ToString();
                    //factura.Total = reader["total"].ToString();
                    pago.Facturar = true;

                }
                reader.Close();

                this.status = true;
                this.message = "El recibo se generó correctamente.";

            }
            catch (Exception ex)
            {
                this.status = false;
                this.message = "No se pudo generar el recibo.\n" + ex;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }

                if (cnx != null)
                {
                    command.Dispose();
                    cnx.Close();
                }

            }
            return new ResultDAO(this.status, this.message);

        }
    }
}