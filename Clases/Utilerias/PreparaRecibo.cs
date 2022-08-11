using Clases.BL;
using GeneraRecibo33;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Transactions;

namespace Clases.Utilerias
{
    public class PreparaRecibo
    {
        public List<tInternetDetalle> BuscaDetalleInternet(int idInternet,int  idMesa, ref decimal subtotal, ref decimal descuento, ref decimal importe)
        {
            List<tInternetDetalle> lConceptos = new tInternetDetalleBL().GetDetallePorMesa( idInternet, idMesa);

            foreach (tInternetDetalle d in lConceptos)
            {
                descuento += d.ImporteDescuento;
                subtotal += d.ImporteNeto;
                importe += d.ImporteTotal;         
            }
            //MensajesInterfaz mgg =  new cErrorBL().insertcError("comprobante => ProcesaRecibo => BuscaDetalleInternet ", "idInternet " + idInternet + " idMesa " + idMesa + " subtotal " + subtotal + " descuento " + descuento + " descuento " + descuento);
            return lConceptos;
        }

        public MensajesInterfaz ComprobanteInternet(string idOrden, string noAutorizacion, ref string rutaPDF,ref string rutaPDFSm)        
        {  //TramiteGuardado OK, PagoEnLineaFailDeposito , r

            string[] orden = idOrden.Split('-');
            string tipoImpuesto;
            MensajesInterfaz mensaje = new MensajesInterfaz();

            tInternet internet = new tInternetBL().GetByConstraint(Convert.ToInt32(orden[2]));
            //mensaje = new cErrorBL().insertcError("ComprobanteInternet: ", " idOrden " + idOrden + " noAutorizacion " + noAutorizacion);

            #region  pretramite
            if (internet == null)
               return MensajesInterfaz.PagoEnLineaFailDeposito;            
           
            if (internet.PeriodoPagadoIP != null)//PREDIAL
            {
                //IMPUESTO PREDIAL
                tipoImpuesto = "IP";
                mensaje = ProcesaRecibo(tipoImpuesto, idOrden, noAutorizacion, ref rutaPDF, "");
                if (mensaje != MensajesInterfaz.TramiteGuardado)
                    return MensajesInterfaz.ReciboDigitalError;
            }
            if (internet.PeriodoPagadoSM != null && internet.PeriodoPagadoSM != "- ---- / - ----" && internet.PeriodoPagadoSM != "") //SERVICIOS
            {
                tipoImpuesto = "SM";
                mensaje = ProcesaRecibo(tipoImpuesto, idOrden, noAutorizacion,  ref rutaPDFSm,"");
                if (mensaje != MensajesInterfaz.TramiteGuardado)
                    return MensajesInterfaz.ReciboDigitalError;
            }
            #endregion
           
            return mensaje;
        }

        public MensajesInterfaz ProcesaRecibo(string tipoImpuesto ,string idOrden, string noAutorizacion, ref string rutaPDF, string observacion)
        {  //TramiteGuardado OK, PagoEnLineaFailDeposito , r

            string[] orden = idOrden.Split('-');
           
            //Recupera conceptos de InternetDetalle
            SaldosC s = new SaldosC(); 
            ConceptoGral cgral = new ConceptoGral();   
            tTramite tramite = new tTramite();           
            string vSerie = string.Empty;
            string mesa = string.Empty;
            int idMesa;
            DateTime fechaPago = DateTime.Now;
            decimal vSubtotal= 0, vDescuento = 0, vImporte = 0;           
            List<tInternetDetalle> lConceptos = new List<tInternetDetalle>();
            MensajesInterfaz mensaje = new MensajesInterfaz();
            Exception exception = new Exception();

            //para ambos tipos
            vSerie = new cParametroSistemaBL().GetValorByClave("SERIE").ToString();
            if (vSerie == "")
                throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.DefinirSerie), "Error");
           
            cPredio predio = new cPredioBL().GetByClavePredial(orden[1].ToString());

            tInternet internet = new tInternetBL().GetByConstraint(Convert.ToInt32(orden[2]));

            //USUARIO
            string usuario = new cParametroSistemaBL().GetValorByClave("USUARIOINTERNET").ToString();
            if (usuario == "")
                throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.UsuarioNoExiste), "Error");
            cUsuarios u = new cUsuariosBL().GetByUsuario(usuario);

            #region  pretramite

            if (tipoImpuesto =="IP")//PREDIAL
            {
                //IMPUESTO PREDIAL             
                mesa = new cParametroSistemaBL().GetValorByClave("MesaIP").ToString();
                if (mesa == "")
                    throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.DefinirMesaIP), "Error");

                idMesa = new cMesaBL().GetByNombre(mesa);

                lConceptos = BuscaDetalleInternet(internet.Id, idMesa,ref vSubtotal,ref vDescuento,ref vImporte);

                tramite.BimestreInicial = Convert.ToInt32(internet.PeriodoPagadoIP.Substring(0,1));
                tramite.EjercicioInicial = Convert.ToInt32(internet.PeriodoPagadoIP.Substring(2,4));
                tramite.BimestreFinal = Convert.ToInt32(internet.PeriodoPagadoIP.Substring(9, 1));
                tramite.EjercicioFinal = Convert.ToInt32(internet.PeriodoPagadoIP.Substring(11, 4));
                
                if (internet.IdRequerimiento != null)
                    tramite.IdRequerimiento = internet.IdRequerimiento;
                if (internet.IdDiferencia != null)
                    tramite.IdDiferencia = internet.IdDiferencia;
                tramite.Tipo = "IP";
                tramite.Periodo = internet.PeriodoPagadoIP;
                tramite.IdTipoTramite = 5;

            }
            else //SERVICIOS
            {               
                mesa = new cParametroSistemaBL().GetValorByClave("MESASM").ToString();
                if (mesa == "")
                    throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.DefinirMesaSM), "Error");
                
                idMesa = new cMesaBL().GetByNombre(mesa);

                //TRAMITE
                lConceptos = BuscaDetalleInternet(internet.Id, idMesa, ref vSubtotal, ref vDescuento, ref vImporte);

                tramite.BimestreInicial = Convert.ToInt32(internet.PeriodoPagadoSM.Substring(0, 1));
                tramite.EjercicioInicial = Convert.ToInt32(internet.PeriodoPagadoSM.Substring(2, 4));
                tramite.BimestreFinal = Convert.ToInt32(internet.PeriodoPagadoSM.Substring(9, 1));
                tramite.EjercicioFinal = Convert.ToInt32(internet.PeriodoPagadoSM.Substring(11, 4));               
                tramite.Tipo = "SM";
                tramite.Periodo = internet.PeriodoPagadoSM;
                tramite.IdTipoTramite = 6;                
            }
            #endregion

            //CAJACAJERO
            tConfiguracionMesa config = new tConfiguracionMesaBL().ValidaConfiguracion(u.Id, idMesa);
            if (config == null)
                throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.SinCajaActiva), "Error");
            
            ////GUARDA TRAMITE
            try
            {
               using (TransactionScope scope = new TransactionScope())
                {
                    tramite.IdPredio = predio.Id;
                    tramite.Fecha = DateTime.Now;
                    tramite.Status = "P";
                    tramite.BaseGravable = predio.ValorCatastral;
                    tramite.SuperficieTerreno = predio.SuperficieTerreno;
                    tramite.TerrenoPrivativo = predio.TerrenoPrivativo;
                    tramite.TerrenoComun = predio.TerrenoComun;
                    tramite.SuperficieConstruccion = predio.SuperficieConstruccion;
                    tramite.ConstruccionPrivativa = predio.ConstruccionPrivativa;
                    tramite.ConstruccionComun = predio.ConstruccionComun;
                    tramite.Observacion = "";
                    tramite.IdUsuario = u.Id;
                    tramite.Activo = true;
                    tramite.FechaModificacion = DateTime.Now;

                    MensajesInterfaz msg = new tTramiteBL().Insert(tramite);
                    if (msg != MensajesInterfaz.Ingreso)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                   // mensaje = new cErrorBL().insertcError(" ProcesaRecibo => Insert tramite", " Parámetros--- Tipo: ");
                    //param[0] -- idpredio           //param[1] -- idInternet       //param[2] -- idCaja
                    //param[3] -- fechaPago          //param[4] -- contribuyente    //param[5] -- domicilio
                    //param[6] -- vImporte           //param[7] -- vSubtotal        //param[8] -- vDescuento
                    //param[9] -- idUsuario          //param[10] -- idMesa          //param[11] -- idTipoPago
                    //param[12] -- idTramite         //param[13] -- noAutorizacion  //param[14] -- vSerie
                    //param[15] -- observaciones     //param[16] -- tipoImpuesto

                    cTipoPago tipoPago = new cTipoPagoBL().GetByConstraint(internet.IdTipoPago >= 0 ? 5 : internet.IdTipoPago);

                    if (tipoPago == null)
                        throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.TipoPago), "Error");

                    string contribuyente = predio.cContribuyente.ApellidoPaterno.Trim() + " " + predio.cContribuyente.ApellidoMaterno.Trim() + " " + predio.cContribuyente.Nombre.Trim();
                    string domicilio = predio.Calle.Trim() + " " + predio.Numero.Trim() + " " + predio.cColonia.NombreColonia.Trim() + " " + predio.Localidad.Trim();

                    string[] param = { predio.Id.ToString(), internet.Id.ToString(),config.IdCaja.ToString(), fechaPago.ToString(), contribuyente, domicilio,
                                       vImporte.ToString(),vSubtotal.ToString(),vDescuento.ToString(),u.Id.ToString(),idMesa.ToString() ,
                                       tipoPago.Id.ToString(),tramite.Id.ToString(),noAutorizacion,vSerie,observacion, tipoImpuesto };

                    mensaje = Recibo(param,  ref rutaPDF, cgral, tipoImpuesto);
                    if (mensaje != MensajesInterfaz.TramiteGuardado)
                        throw new System.ArgumentException(new Utileria().GetDescription(mensaje), "Error");
                                        
                    scope.Complete();

                }//scope
            }//try
            catch (Exception ex)
            {                
                new Utileria().logError("Pago en linea Clases: PreparaRecibo.Exception", ex, " Parámetros--- tipo: " + tipoImpuesto +" ordenId: "+ idOrden+" no auto: "+ noAutorizacion + " usuario: " + u.Id.ToString() + " idmesa: " + idMesa.ToString());
                return MensajesInterfaz.PagoEnLineaFail;
            }
           
            return MensajesInterfaz.TramiteGuardado ;
        }

        private MensajesInterfaz Recibo(String[] strParam, ref string rutaPDF, ConceptoGral cgral, string tipoImpuesto)
        {
            #region parametros
            //strParam[0] -- idPredio
            //strParam[1] -- idInternet
            //strParam[2] -- idCaja
            //strParam[3] -- fechaPago
            //strParam[4] -- contribuyente
            //strParam[5] -- domicilio
            //strParam[6] -- vImporte
            //strParam[7] -- vSubtotal
            //strParam[8] -- vDescuento
            //strParam[9] -- idUsuario
            //strParam[10] -- idMesa
            //strParam[11] -- idTipoPago
            //strParam[12] -- idTramite
            //strParam[13] -- noAutorizacion
            //strParam[14] -- vSerie
            //strParam[15] -- observacion
            //strParam[16] -- tipoImpuesto


            //param{idPredio,idInternet, idCaja, fechaPago,contribuyente,domicilio,vImporte,vSultotal,vDescuento,
            //idUsuario,idMesa,idTipoPago,idTramite, noAutorizacion, vSerie, observacion, tipoimpuesto}
            #endregion
            cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(strParam[0].ToString()));

            MensajesInterfaz msg = new MensajesInterfaz();
            List<tInternetDetalle> list = new List<tInternetDetalle>();
            cUsuarios u = new cUsuariosBL().GetByConstraint(Convert.ToInt32(strParam[9].ToString()));
            cMesa  mesa = new cMesaBL().GetByConstraint(Convert.ToInt32(strParam[10].ToString()));
            Exception exception = new Exception();

            try
            {
                if (strParam[2].ToString() == "")                    
                        throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.SinCajaActiva), "Error");

                // GUARDANDO RECIBO
                tRecibo r = new tRecibo();
                #region recibo
                r.IdCaja = Convert.ToInt32(strParam[2].ToString());
                r.FechaPago = Convert.ToDateTime(strParam[3].ToString());
                r.EstadoRecibo = "P";
                r.Contribuyente = strParam[4].ToString();
                r.Rfc = "XAXX010101000";
                r.Domicilio = strParam[5].ToString();
                r.ImportePagado = Convert.ToDecimal(strParam[6].ToString());
                r.ImporteNeto = Convert.ToDecimal(strParam[7].ToString());//not null
                r.ImporteDescuento = Convert.ToDecimal(strParam[8].ToString()); //not null
                r.MaquinaPago = "-"; //not null                                        
                r.IdUsuarioCobra = u.Id; //not null                                            
                r.IdMesaCobro = mesa.Id;
                r.IdTipoPago = Convert.ToInt32(strParam[11].ToString());
                r.IdTramite = Convert.ToInt32(strParam[12].ToString()); //not null                                             
                r.NoAutorizacion = strParam[13].ToString();
                r.IdFIEL = new cFIELBL().GetByActive().Id; //not null     
                r.Serie = strParam[14].ToString(); //not null
                r.Ruta = "-"; //se asigna al generar el recibo//not null
                r.IdMesa = mesa.Id; //
                r.CodigoSeguridad = "-"; //se asigna al generar el recibo//null
                r.RND = 0; //al generar el recibo //null
                r.DatosPredio = "Clave Catastral: " + predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3) + "     Tipo predio: " + predio.cTipoPredio.Descripcion.Trim() + "     Super. Terreno: " + predio.SuperficieTerreno + "m2.     Super. Const.: " + predio.SuperficieConstruccion + "m2.    Base gravable: $" + predio.ValorCatastral.ToString("#.##");
                r.Observaciones = "";// "Clave Catastral: " + predio.ClavePredial.Trim() + "     Tipo predio: " + predio.cTipoPredio.Descripcion.Trim() + "     Super. Terreno: " + predio.SuperficieTerreno + "m2.     Super. Const.: " + predio.SuperficieConstruccion + "m2.    Base gravable: $" + predio.ValorCatastral.ToString("#.##") ;
                r.Activo = true; //not null
                r.IdUsuario = u.Id;
                r.FechaModificacion = DateTime.Now;

                //msg = new tReciboBL().Insert(r);
                //if (msg != MensajesInterfaz.Ingreso)
                //    throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");           

                if (strParam[1].ToString() != "") //idInternet != ""
                {
                    list = new tInternetDetalleBL().GetDetallePorMesa(Convert.ToInt32(strParam[1].ToString()), Convert.ToInt32(strParam[10].ToString()));

                    //RECIBO DETALLE  
                    foreach (tInternetDetalle concepto in list)
                    {
                        tReciboDetalle reciboDetalle = new tReciboDetalle();
                        reciboDetalle.IdRecibo = r.Id;
                        reciboDetalle.IdConcepto = concepto.IdConcepto;
                        reciboDetalle.ImporteNeto = concepto.ImporteNeto;
                        reciboDetalle.ImporteDescuento = concepto.ImporteDescuento;
                        reciboDetalle.ImporteAdicional = concepto.ImporteAdicional;
                        reciboDetalle.ImporteTotal = concepto.ImporteTotal;
                        reciboDetalle.Cantidad = 1;
                        reciboDetalle.DescuentoDecretoPorcentaje = 0;
                        reciboDetalle.IdConceptoRef = concepto.IdConceptoRef;
                        reciboDetalle.DescuentoEspecialPorcentaje = 0;
                        reciboDetalle.IdControlSistema = 1;
                        reciboDetalle.Activo = true;
                        reciboDetalle.FechaModificacion =DateTime.Now;
                        reciboDetalle.IdUsuario = Convert.ToInt32(strParam[9].ToString());

                        //msg = msg = new tReciboDetalleBL().Insert(reciboDetalle);
                        //if (msg != MensajesInterfaz.Ingreso)
                        //    throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        r.tReciboDetalle.Add(reciboDetalle);                       
                    }
                    msg = new tReciboBL().Insert(r);
                    if (msg != MensajesInterfaz.Ingreso)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                }

                #endregion

                //ACTUALIZACION DE PREDIOS
                tTramite tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(strParam[12].ToString()));

                tInternet internet = new tInternetBL().GetByConstraint(Convert.ToInt32(strParam[1].ToString()));                

                #region actualizacion despues de guardar

                if (strParam[16].ToString() == "IP")//PREDIAL
                {
                    if (tramite.BimestreInicial > 0 && tramite.BimestreFinal > 0)
                    {
                        predio.BimestreFinIp = Convert.ToInt32(tramite.BimestreFinal);
                        predio.AaFinalIp = Convert.ToInt32(tramite.EjercicioFinal);
                        predio.ReciboIp = r.Id;
                        predio.IdReciboIp = r.Id;
                        predio.FechaPagoIp = DateTime.Now;

                        msg = new cPredioBL().Update(predio);
                        if (msg != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                      
                    }

                    //DIFERENCIAS
                    if (internet.IdDiferencia > 0)
                    {
                        cDiferencia dif = new cDiferenciaBL().GetByConstraint(Convert.ToInt32(internet.IdDiferencia));

                        dif.Status = "P";
                        msg = new cDiferenciaBL().Update(dif);
                        if (msg != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                    }
                    if (internet.IdRequerimiento > 0)
                    {
                        tRequerimiento req = new tRequerimientoBL().GetByConstraint(Convert.ToInt32(internet.IdRequerimiento));
                        req.Status = "P";
                        req.FechaPago = DateTime.Now;
                        req.Recibo = r.Id;

                        msg = new tRequerimientoBL().Update(req);
                        if (msg != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                    }
                    if (internet.Id > 0  )
                    {
                        internet.IdReciboIP = r.Id;
                        if (internet.IdReciboSM != null)
                        {
                            internet.Estado = "P";  //estado P predial S servicios F finalizado
                        }
                        msg = new tInternetBL().Update(internet);
                        if (msg != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                    }
                }
                
                if (strParam[16].ToString() == "SM")
                {
                    predio.BimestreFinSm = Convert.ToInt32(tramite.BimestreFinal);
                    predio.AaFinalSm = Convert.ToInt32(tramite.EjercicioFinal);
                    predio.ReciboSm = r.Id;
                    predio.IdReciboSm = r.Id;
                    predio.FechaPagoSm = DateTime.Now;
                    msg = new cPredioBL().Update(predio);
                    if (msg != MensajesInterfaz.Actualizacion)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                    if (internet.Id > 0  )
                    {
                        internet.IdReciboSM = r.Id;
                        if (internet.IdReciboIP != null)
                        {
                            internet.Estado = "P";
                        }
                        msg = new tInternetBL().Update(internet);
                        if (msg != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                    }
                }
                #endregion

                rutaPDF = string.Empty;
                
                List<Conceptos> cg = ConceptosDigital(internet.IdOrden, cgral, list, predio.ClavePredial);
                if (cg == null)
                    throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.ConceptoDigitalError), "Error");
                                
                MensajesInterfaz mensaje = ComprobanteDigital(predio.Id, ref rutaPDF, cg, r.IdTipoPago, u, strParam[14].ToString(), mesa.Descripcion, r.ImporteNeto, r.ImporteDescuento, r.ImportePagado, r.Id);
                                
                if (mensaje != MensajesInterfaz.TramiteGuardado)
                    throw new System.ArgumentException(new Utileria().GetDescription(mensaje), "Error");

            }//try
            catch (Exception ex)
            {                
                new Utileria().logError("Comprobante Digital", ex, " Parámetros---  idPredio" + strParam[0].ToString()+ " idUsuario: "+strParam[9].ToString() + " "+tipoImpuesto);
                return MensajesInterfaz.PagoEnLineaFailDeposito;
            }           
            return MensajesInterfaz.TramiteGuardado;
        }
       
        private List<Conceptos> ConceptosDigital (string idOrden, ConceptoGral cGral, List<tInternetDetalle> list,  string claveCatastral)
        {
            List<Conceptos> listC = new List<Conceptos>();
            try
            {
                if (list != null)
                {
                    foreach (tInternetDetalle concepto in list)
                    {
                        Conceptos c = new Conceptos();
                        c.Cantidad = 1;
                        c.CuentaPredial = claveCatastral;
                        c.Descripcion = concepto.Descripcion;
                        c.Id = (concepto.IdConcepto).ToString();
                        c.Importe = Convert.ToDouble(concepto.ImporteNeto);
                        c.Unidad = "PIEZA";                        
                        c.ClaveProdServ = "01010101";
                        c.ClaveUnidad = "C81";
                        c.Descuento = Convert.ToDouble(concepto.ImporteDescuento);
                        c.ValorUnitario = Convert.ToDouble(concepto.ImporteNeto);
                        listC.Add(c);
                    }
                }
                else
                {
                    foreach (ConceptosRec cr in cGral.conceptos)
                    {
                        Conceptos c = new Conceptos();
                        c.Cantidad = cr.Cantidad;
                        c.CuentaPredial = cr.CuentaPredial;
                        c.Descripcion = cr.Descripcion;
                        c.Id = cr.Id;
                        c.Importe = cr.Importe;
                        c.Unidad = cr.Unidad;
                        c.Unidad = "PIEZA";
                        c.ClaveProdServ = "01010101";
                        c.ClaveUnidad = "C81";
                        c.Descuento = Convert.ToDouble(cr.Descuento);
                        c.ValorUnitario = cr.ValorUnitario;
                        listC.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("PreparaRecibo.Exception", ex);
                listC = null;
            }
            return listC;
        }

        public MensajesInterfaz ComprobanteDigital (int idPredio, ref string rutaPDF,List<Conceptos> lConceptos,int metodoPago, cUsuarios usuario, string serie, string mesa, decimal subTotal, decimal descuento, decimal importe, int idRecibo )
        {
            Comprobante comprobante = new Comprobante();
            Receptor receptor = new Receptor();
            DatosRecibo datosRecibo = new DatosRecibo();
            tRecibo r = new tRecibo();

            MensajesInterfaz msg =new MensajesInterfaz();        

            try
            {               
                    cPredio predio = new cPredioBL().GetByConstraint(idPredio);
                    cTipoPago tipoPago = new cTipoPagoBL().GetByConstraint(metodoPago);

                    #region digital
                    // SI LA TRANSACCION ESTA COMPLETA GENERA EL RECIBO
                    //COMPROBANTE using GeneraRecibo;
                    comprobante.cajero = usuario.Usuario;
                    comprobante.FormaDePago = "04";
                    comprobante.MetodoDePago = "PUE";
                    comprobante.motivoDescuento = "";
                    comprobante.Serie = serie;
                    comprobante.Mesa = mesa;
                    comprobante.TipoDeComprobante = "I";
                    comprobante.Folio = idRecibo.ToString().PadLeft(6, '0');
                    comprobante.SubTotal = Convert.ToDouble(subTotal);
                    comprobante.descuento = Convert.ToDouble(descuento);
                    comprobante.Total = Convert.ToDouble(importe);
                    comprobante.DatosPredio = "Clave Catastral: " + predio.ClavePredial.Trim() + "     Tipo predio: " + predio.cTipoPredio.Descripcion.Trim() + "     Super. Terreno: " + predio.SuperficieTerreno + "m2.     Super. Const.: " + predio.SuperficieConstruccion + "m2.    Base gravable: $" + predio.ValorCatastral.ToString("#.##");
                    comprobante.Observaciones = "";              

                //DATOS RECEPTOR   
                    receptor.Nombre = predio.cContribuyente.ApellidoPaterno.Trim() + ' ' + predio.cContribuyente.ApellidoMaterno.Trim() + ' ' + predio.cContribuyente.Nombre.Trim();
                    receptor.UsoCFDI = "P01";
                    receptor.Domicilio = predio.Calle.Trim() + ' ' + predio.Numero.Trim() + ' ' + predio.cColonia.NombreColonia.Trim() + ' ' + predio.Localidad.Trim();
                    receptor.RFC = "XAXX010101000";
                    receptor.email = predio.cContribuyente.Email;
                    receptor.Id = predio.Id;
         
                //LISTA DE CONCEPTOS             
                    datosRecibo.Conceptos_ = lConceptos;
                    datosRecibo.Comprobante_ = comprobante;
                    datosRecibo.Receptor_ = receptor;

                    //GENERA RECIBO 
                    Recibo rec = new Recibo();
                    string path;
                    ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
                    path = HttpContext.Current.Server.MapPath("~/");                    

                    rec = reciboCFDI.generaRecibo(datosRecibo, path);
                   
                    if (rec.Errores != null)
                    {
                        if (rec.Errores.Length > 0)
                        {
                            throw new System.ArgumentException(new Utileria().GetDescription(rec.Errores), "Error");
                        }
                    }
                    #endregion

                    r = new tReciboBL().GetByConstraint(idRecibo);
                    //ACTUALIZA EL RECIBO CON LOS DATOS GENERADOS POR LA FUNCION GENERA RECIBO
                    r.CodigoSeguridad = rec.codigoReimpresion;
                    r.Ruta = rec.Ruta;
                    r.RND = Convert.ToByte(rec.RND);

                    msg = new tReciboBL().Update(r);
                    if (msg != MensajesInterfaz.Actualizacion)
                    {
                        string usuarioFactura = ConfigurationManager.AppSettings["usuarioFactura"].ToString();
                        string passwordFactura = ConfigurationManager.AppSettings["passwordFactura"].ToString();
                        string respuestaCancelacion = reciboCFDI.CancelacionFactura(r, usuario.Id, "", usuarioFactura, passwordFactura);

                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    }
             
                    //GENERA PDF         
                    string ruta = path + "/Recibos/Pdf/" + r.CodigoSeguridad + ".pdf";
                    FileStream newFile = new FileStream(ruta, FileMode.Create);
                    newFile.Write(rec.pdfBytes, 0, rec.pdfBytes.Length);
                    newFile.Close();

                    rutaPDF = "~/Recibos/Pdf/" + r.CodigoSeguridad + ".pdf";
              
            }             
            catch (Exception ex)
            {               
                new Utileria().logError("ComprobanteDigital ", ex);
                return MensajesInterfaz.PagoEnLineaFailDeposito;
            }            
            return MensajesInterfaz.TramiteGuardado;            
        }

        private string CancelaDigital (tRecibo r , int idUsuario)
        {
            ReciboCFDI33 reciboCFDI = new ReciboCFDI33();
            string usuarioFactura = System.Configuration.ConfigurationManager.AppSettings["usuarioFactura"].ToString();
            string passwordFactura = ConfigurationManager.AppSettings["passwordFactura"].ToString();
            string  respuestaCancelacion = reciboCFDI.CancelacionFactura(r, idUsuario, "", usuarioFactura, passwordFactura);

            return respuestaCancelacion;
        }


        public MensajesInterfaz ActualizaTipoPago(string idOrden, string noAutorizacion, string tipopago)
        {  
            string[] orden = idOrden.Split('-');
            int idTipoPago = 0;
           
            MensajesInterfaz mensaje = new MensajesInterfaz();
            tInternet internet = new tInternetBL().GetByConstraint(Convert.ToInt32(orden[2]));
            
            #region  pretramite
            if (internet == null)
                return MensajesInterfaz.PagoEnLineaFailDeposito;

            switch (tipopago)
            {
                case "CLABE,clabe":
                case "CLABE":
                case "clabe":
                    idTipoPago =  4 ;//new cParametroSistemaBL().GetByClave("PAGO_INTERNET_CLABE").Id;
                        break;
                   
                case "CIE,cie":
                case "CIE":
                case "cie":
                        idTipoPago =  3 ; // new cParametroSistemaBL().GetByClave("PAGO_INTERNET_CHEQUE").Id;
                        break;

                //case "TDX,tdx":
                //case "TDX":
                //case "tdx":
                //    idTipoPago = 5;// new cParametroSistemaBL().GetByClave("PAGO_INTERNET_TC").Id;
                //    break;

                default:
                    idTipoPago = 5;
                    break;
            }            

            if (internet != null)//PREDIAL
            {               
                System.Web.HttpContext.Current.Session["usuario"] = new cUsuariosBL().GetByConstraint(internet.IdUsuario);

                internet.IdTipoPago = idTipoPago;
                internet.NoAutorizacion = noAutorizacion;
                mensaje = new tInternetBL().Update(internet);
                if (mensaje != MensajesInterfaz.Actualizacion)
                    return mensaje;
            }
            #endregion
           
            return MensajesInterfaz.TramiteGuardado; 
        }


    }//class
}
