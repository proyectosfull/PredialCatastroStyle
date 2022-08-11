using Clases.BL;
using System;
using System.Transactions;


namespace Clases.Utilerias
{
    public class TramiteInternet
    {

        public MensajesInterfaz GuardaInternet(int idPredio, ref string idOrden, decimal ImporteTotal, Impuesto i, Servicio sm)
        {
            DateTime fecha = DateTime.Today;           
            int idMesa = 0, idMesaSM = 0;
            string periodoIP = string.Empty, periodoSM = string.Empty;
            ConceptoGral cgral = new ConceptoGral();
            SaldosC s = new SaldosC();
            MensajesInterfaz msg;

#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    if (i != null && (i.Estado.PeriodoGral != null && i.Estado.PeriodoGral != "- ---- / - ----") )
                    {
                        string mesa = new cParametroSistemaBL().GetValorByClave("MesaIP").ToString();
                        if (mesa == "")
                        {
                            return MensajesInterfaz.DefinirMesaIP;
                        }
                        idMesa = new cMesaBL().GetByNombre(mesa);
                        periodoIP  = i.Estado.PeriodoGral;
                    }
                    if (sm != null && (sm.PeriodoGral != "- ---- / - ----" && sm.PeriodoGral != null) )
                    {
                        string mesaSM = new cParametroSistemaBL().GetValorByClave("MesaSM").ToString();
                        if (mesaSM == "")
                        {
                            return MensajesInterfaz.DefinirMesaIP;
                        }
                        idMesaSM = new cMesaBL().GetByNombre(mesaSM);
                        periodoSM = sm.Estado.PeriodoGral;
                    }

                    cPredio predio = new cPredioBL().GetByConstraint(idPredio);
                    string usuario = new cParametroSistemaBL().GetValorByClave("USUARIOINTERNET").ToString();
                    cUsuarios U = new cUsuariosBL().GetByUsuario(usuario);

                    tInternet internet = new tInternet();
                    //internet.Id =
                    internet.IdPredio = idPredio;
                    internet.IdOrden = idOrden;
                    internet.Ejercicio = fecha.Year;
                    internet.Mes = fecha.Month;
                    internet.NoAutorizacion = "";
                    internet.FechaPago = DateTime.Now;
                    internet.Estado = "T"; // T tramite, P pagado
                    internet.ImportePagado = ImporteTotal;
                    if (idMesa > 0)
                        internet.IdMesaIP = idMesa;
                    if (idMesaSM > 0)
                        internet.IdMesaSM = idMesaSM;
                    internet.IdTipoPago = 4;
                    if (i.IdDiferencia > 0)
                        internet.IdDiferencia = i.IdDiferencia;
                    if (i.IdRequerimiento > 0)
                        internet.IdRequerimiento = i.IdRequerimiento;
                    internet.PeriodoPagadoIP = periodoIP;
                    internet.PeriodoPagadoSM = periodoSM;
                    internet.Activo = true;
                    internet.IdUsuario = U.Id;
                    internet.FechaModificacion = DateTime.Now; ;

                    msg = new tInternetBL().Insert(internet);
                    if (msg != MensajesInterfaz.Ingreso)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");


                    if ( i != null)
                    {
                        string complemento = ", " + "Tipo predio " + predio.cTipoPredio.Descripcion + ", Super. Terreno " + predio.SuperficieTerreno + ", Super. Const. " + predio.SuperficieConstruccion + ", Base gravable " + predio.ValorCatastral;
                        cgral = s.ConceptosP(i, predio.ClavePredial, "" );

                        if (cgral.Error != null)
                        {
                            throw new System.ArgumentException(new Utileria().GetDescription(cgral.Error), "Error");                           
                        }

                        //RECIBO DETALLE  
                        foreach (ConceptosRec conceptos in cgral.conceptos)
                        {
                            tInternetDetalle detalle = new tInternetDetalle();
                            detalle.IdInternet = internet.Id;
                            detalle.IdMesa = idMesa;
                            detalle.Descripcion = conceptos.Descripcion;
                            detalle.Unidad = conceptos.Unidad;
                            detalle.IdConcepto = Convert.ToInt32(conceptos.IdCri);
                            detalle.ImporteNeto = Convert.ToDecimal(conceptos.Importe);
                            detalle.ImporteDescuento = Convert.ToDecimal(conceptos.Descuento);
                            detalle.ImporteAdicional = Convert.ToDecimal(conceptos.Adicional);
                            detalle.ImporteTotal = Convert.ToDecimal(conceptos.Importe - conceptos.Descuento);
                            detalle.Cantidad = 1;
                            detalle.DescuentoDecretoPorcentaje = 0;
                            detalle.IdConceptoRef = conceptos.IdConceptoRef;
                            detalle.DescuentoEspecialPorcentaje = 0;
                            detalle.Activo = true;
                            detalle.FechaModificacion = DateTime.Now; ;
                            detalle.IdUsuario = U.Id;                           

                            msg = msg = new tInternetDetalleBL().Insert(detalle);
                            if (msg != MensajesInterfaz.Ingreso)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        }
                    }

                    cParametroSistema municipio = new cParametroSistemaBL().GetByClave("COBROSERVICIOSINTERNET");
                    if (municipio == null) municipio = new cParametroSistema();
                    
                    if (sm != null && municipio.Valor == "SI")
                    {
                        cgral = s.ConceptosS(sm, predio.ClavePredial);

                        if (cgral.Error != null)
                        {
                            throw new System.ArgumentException(new Utileria().GetDescription(cgral.Error), "Error");
                        }

                        //RECIBO DETALLE  
                        foreach (ConceptosRec conceptos in cgral.conceptos)
                        {
                            tInternetDetalle detalle = new tInternetDetalle();
                            detalle.IdInternet = internet.Id;
                            detalle.IdMesa = idMesaSM;
                            detalle.IdConcepto = Convert.ToInt32(conceptos.IdCri);
                            detalle.ImporteNeto = Convert.ToDecimal(conceptos.Importe);
                            detalle.ImporteDescuento = Convert.ToDecimal(conceptos.Descuento);
                            detalle.ImporteAdicional = Convert.ToDecimal(conceptos.Adicional);
                            detalle.ImporteTotal = Convert.ToDecimal(conceptos.Importe - conceptos.Descuento);
                            detalle.Cantidad = 1;
                            detalle.DescuentoDecretoPorcentaje = 0;
                            detalle.IdConceptoRef = conceptos.IdConceptoRef;
                            detalle.DescuentoEspecialPorcentaje = 0;
                            detalle.Activo = true;
                            detalle.FechaModificacion = DateTime.Now; 
                            detalle.IdUsuario = U.Id;
                            detalle.Descripcion = conceptos.Descripcion;
                            detalle.Unidad = conceptos.Unidad;

                            msg = new tInternetDetalleBL().Insert(detalle);
                            if (msg != MensajesInterfaz.Ingreso)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        }

                    }
                    idOrden = idOrden.Trim()+ "-" + internet.Id;
                    scope.Complete();

                }//scope
            }
            catch (Exception ex)
            {
                return MensajesInterfaz.PagoEnLineaFail;
            }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa


            return MensajesInterfaz.TramiteGuardado;
        }


    }
}
