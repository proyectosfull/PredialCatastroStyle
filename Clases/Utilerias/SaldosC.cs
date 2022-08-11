using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases.BL;
using Clases.Utilerias;
using System.Data;
using System.ComponentModel;
using System.Globalization;



namespace Clases.Utilerias
{
    public class SaldosC
    {
        List<cSalarioMinimo> salMin;
        List<cTarifaZona> tZona;
        List<cTarifaLimpieza> tLimpieza;
        List<cTarifaRecoleccion> tRecoleccion;
        List<cTarifaDap> tDap;
        decimal indiceActual;
        string AplicaActualizacion;
        decimal porcAdicional;
        List<cTarifaRecargo> listRecargos;
        List<cIndicePrecio> listIndice;
        decimal cobroMulta, cobroEjecucion;
        string cobroDapSinConst;
        public SaldosC()
        {
            salMin = new cSalarioMinimoBL().GetAll();
            tZona = new cTarifaZonaBL().GetAll();
            tLimpieza = new cTarifaLimpiezaBL().GetAll();
            tRecoleccion = new cTarifaRecoleccionBL().GetAll();
            tDap  = new cTarifaDapBL().GetAll();
            indiceActual = new cIndicePrecioBL().ValorIndiceActual();
            AplicaActualizacion = new cParametroSistemaBL().GetValorByClave("APLICARACTUALIZACIONINP").ToString();

            porcAdicional = new cParametroCobroBL().GetByClave("PorcentajeAdicional");
            if (porcAdicional > 0)
                porcAdicional = porcAdicional / 100;

            listRecargos = new cTarifaRecargoBL().GetAll();
            listIndice = new cIndicePrecioBL().GetAll();
            cobroMulta = new cParametroCobroBL().GetByClave("ImporteMulta");
            cobroEjecucion = new cParametroCobroBL().GetByClave("ImporteEjecucion");
            cobroDapSinConst = new cParametroSistemaBL().GetValorByClave("DAP_SINCONSTRUCCION").ToString();

        }
     

        #region Basicas

        //Dictionary<string, decimal> dCobro = (Dictionary<string, decimal>)new cParametroCobroBL().GetDiccionaryValor();

        public Periodo ValidaPeriodoPago(int bimestreInicial, int ejercicioInicial, int bimestreFinal, int ejercicioFinal, string tramite)
        {
            Periodo per = new Periodo();
            int eAnterior = 0;
            try
            {   //se incrementa en el bimestre siguiente
                bimestreInicial++;
                if (bimestreInicial == 7)
                {
                    bimestreInicial = 1;
                    ejercicioInicial++;
                }

                int ejercicioCobro = Convert.ToInt32(new cParametroCobroBL().GetByClave("EjercicioInicialCobro"));
                if (ejercicioCobro == -1)
                {
                    per.mensaje = MensajesInterfaz.EjercicioInicialCobro;
                    return per;
                }
                string aplicarPrescripcion = new cParametroSistemaBL().GetValorByClave("APLICARPRESCRIPCION");
                if (aplicarPrescripcion == "" || aplicarPrescripcion == null)
                {
                    per.mensaje = MensajesInterfaz.AplicarPrescripcion;
                    return per;
                }
                if (aplicarPrescripcion == "SI")
                {
                    int aaPrescripcion = Convert.ToInt32(new cParametroCobroBL().GetByClave("EjercicioDePrescripcion"));
                    if (aaPrescripcion <= 0)
                    {
                        per.mensaje = MensajesInterfaz.AniosPrescripcion;
                        return per;
                    }

                    eAnterior = (DateTime.Today).Year - aaPrescripcion;
                }
                else
                {
                    eAnterior = ejercicioCobro;
                }

                if (ejercicioInicial < eAnterior)
                {
                    ejercicioInicial = eAnterior;
                    bimestreInicial = 1;
                }

                per.bInicial = bimestreInicial;
                per.eInicial = ejercicioInicial;

                int EjercicioAnticipado = Convert.ToInt32(new cParametroCobroBL().GetByClave("EjercicioAnticipado"));
                if (ejercicioInicial >= DateTime.Today.Year + 2)
                {
                    per.bInicial = bimestreInicial;
                    per.eInicial = ejercicioInicial;
                    per.bFinal = bimestreInicial;
                    per.eFinal = ejercicioInicial;
                    return per;
                }

                per.bFinal = bimestreFinal;
                per.eFinal = ejercicioFinal;

                if (tramite == "CalculaPredial" || tramite == "CalculaServicios")
                {

                    int mesCobroAnt = Convert.ToInt32(new cParametroCobroBL().GetByClave("MesPagoAnticipado"));
                    if (mesCobroAnt == -1) mesCobroAnt = 12;

                    if (mesCobroAnt <= DateTime.Today.Month)
                    {
                        decimal baseAnt = new cBaseImpuestoBL().GetByEjercicio(DateTime.Today.Year + 1);
                        if (baseAnt > 0)
                        {
                            per.bFinal = 6;
                            per.eFinal = DateTime.Today.Year + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("Saldos.ValidaPeriodoPago.Exception", ex, " -- Parametros:" + bimestreInicial + "," + ejercicioInicial + "," + bimestreFinal + "," + ejercicioFinal);
                per.mensaje = MensajesInterfaz.EjercicioIniFin;
            }

            return per;
        }

        public Periodo DevuelvePeriodoPago(int bInicial, int eInicial, int bApagar, int eApagar, string tipo)
        {
            Periodo per = null;
            int bActual = 0;
            int eAnterior = 0;
            DateTime ldt_hoy = DateTime.Today;
            int eActual = ldt_hoy.Year;

            try
            {
                double mesActual = Utileria.Redondeo(ldt_hoy.Month / 2.0);
                bActual = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ldt_hoy.Month / 2.0)));

                //validar el limite de años para cobro
                eAnterior = Convert.ToInt32((new cParametroCobroBL().GetByClave("EjercicioInicialCobro")));
                if (eAnterior == -1)
                {
                    per.mensaje = MensajesInterfaz.EjercicioInicialCobro;
                    return per;
                }
                if (eInicial < (eActual - eAnterior))
                {
                    bInicial = 1;
                    eInicial = Convert.ToInt32(ldt_hoy.Year - eAnterior);
                }
                switch (tipo)
                {
                    case "ANTICIPADO":
                        if (eInicial > eActual || eApagar > eActual)
                        {
                            per = new Periodo();
                            per.bInicial = bInicial;
                            per.eInicial = eInicial;
                            per.bFinal = bApagar;
                            per.eFinal = eApagar;
                            if (eInicial <= eActual)
                            {
                                per.bInicial = 1;
                                per.eInicial = eActual + 1;
                            }
                        }
                        break;
                    case "REZAGO":
                        if ((eInicial < eActual))//&& ( (eApagar >= eActual) || (eApagar))
                        {
                            per = new Periodo();
                            per.bInicial = bInicial;
                            per.eInicial = eInicial;
                            per.bFinal = bApagar;
                            per.eFinal = eApagar;
                            if (eApagar >= eActual)
                            {
                                per.bFinal = 6;
                                per.eFinal = eActual - 1;
                            }
                        }
                        break;
                    case "ACTUAL":
                        if (eInicial <= eActual && (eApagar >= eActual))
                        {
                            per = new Periodo();
                            per.bInicial = bInicial;
                            per.eInicial = eInicial;
                            per.bFinal = bApagar;
                            per.eFinal = eApagar;

                            if (eInicial < eActual)
                            {
                                per.bInicial = 1;
                                per.eInicial = eActual;
                            }
                            if (eApagar > eActual)
                            {
                                per.bFinal = 6;
                                per.eFinal = eActual;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("Saldos.DevuelvePeriodoPago.Exception", ex, "Parametros --- " + bInicial.ToString() + ", " +
                                            eInicial.ToString() + ", " + bApagar.ToString() + ", " + eApagar.ToString() + ", " + tipo.ToString());
            }
            return per;

        }//DevuelvePeriodoPago

        public int CuentaBimestre(int bimestreInicial, int ejercicioInicial, int bimestreFinal, int ejercicioFinal, ref MensajesInterfaz msg)
        {
            int bimestres = 0;
            Boolean flag = true;

            if (ejercicioInicial > ejercicioFinal)
            {
                msg = MensajesInterfaz.PeriodoIncorrecto;
                return 0;
            }

            if (bimestreInicial == bimestreFinal && ejercicioInicial == ejercicioFinal)
            {
                return 1;
            }

            if (ejercicioInicial > ejercicioFinal && bimestreInicial > bimestreFinal)
            {
                return 0;
            }

            bimestres++;
            do
            {
                if (bimestreInicial == bimestreFinal && ejercicioInicial == ejercicioFinal)
                    break;
                else
                {
                    bimestreInicial++;

                    if (bimestreInicial == 7)
                    {
                        bimestreInicial = 1;
                        ejercicioInicial++;
                    }
                    bimestres++;
                }

            } while (flag == true);

            return bimestres;
        }

        public Impuesto InicializaIP(Impuesto i)
        {
            i.PerAnticipado = null;
            i.AntImpuesto = 0;
            i.AntImpuestoPri = 0;
            i.AntImpuestoExc = 0;
            i.AntDescImpuesto = 0;
            i.AntAdicional = 0;
            i.AntDescAdicional = 0;
            i.AntBimestre = 0;
            //Actual             
            i.PerActual = null;
            i.ActImpuesto = 0;
            i.ActImpuestoPri = 0;
            i.ActImpuestoExc = 0;
            i.ActDescImpuesto = 0;
            i.ActRecargo = 0;
            i.ActDescRecargo = 0;
            i.ActAdicional = 0;
            i.ActDescAdicional = 0;
            i.ActDiferencias = 0;
            i.ActDescDiferencias = 0;
            i.ActRecDiferencias = 0;
            i.ActDescRecDiferencias = 0;
            i.ActBimestre = 0;
            //rezago         
            i.PerRezago = null;
            i.Rezagos = 0;
            i.RezDescRezagos = 0;
            i.RezRecargo = 0;
            i.RezDescRecargos = 0;
            i.RezAdicional = 0;
            i.RezDescAdicional = 0;
            i.RezDiferencias = 0;
            i.RezDescDiferencias = 0;
            i.RezRecDiferencias = 0;
            i.RezDescRecDiferencias = 0;
            //Generales
            i.Multa = 0;
            i.MultaDesc = 0;
            i.Ejecucion = 0;
            //sumatorias         
            i.DescuentoGral = 0;
            i.Importe = 0;
            i.IdRequerimiento = 0;
            i.IdDiferencia = 0;
            //estado de cuenta           
            ImpuestoEdo Estado = new ImpuestoEdo();
            Estado.AntAdicional = 0;
            Estado.AntImpuesto = 0;
            Estado.Descuentos = 0;
            Estado.Diferencias = 0;
            Estado.Ejecucion = 0;
            Estado.Importe = 0;
            Estado.Impuesto = 0;
            Estado.Multas = 0;
            Estado.PeriodoGral = "- ---- / - ----";
            Estado.Recargos = 0;
            Estado.RecDiferencias = 0;
            Estado.Rezagos = 0;
            Estado.Adicional = 0;
            //error
            i.Estado = Estado;
            i.TextError = "";
            //i.mensaje = null;

            return i;
        }

        public Servicio InicializaSM(Servicio s)
        {
            //Anticipado            
            s.PerAnticipado = null;
            s.AntBimestre = 0;
            s.AntInfraestructura = 0;
            s.AntDescInfraestructura = 0;
            s.AntRecoleccion = 0;
            s.AntDescRecoleccion = 0;
            s.AntLimpieza = 0;
            s.AntDescLimpieza = 0;
            s.AntDap = 0;
            s.AntDescDap = 0;
           

            //Actual             
            s.PerActual = null;
            s.ActBimestre = 0;
            s.ActInfraestructura = 0;
            s.ActDescInfraestructura = 0;
            s.ActRecoleccion = 0;
            s.ActDescRecoleccion = 0;
            s.ActLimpieza = 0;
            s.ActDescLimpieza = 0;
            s.ActDap = 0;
            s.ActDescDap = 0;
           
            //Rezago  
            s.PerRezago = null;
            s.RezBimestre = 0;
            s.Rezagos = 0;
            s.RezDescRezagos = 0;
            s.RezRecargo = 0;
            s.RezDescRecargos = 0;
            s.RezAdicional = 0;
            s.RezDescAdicional = 0;
            //Generales  
            s.Multa = 0;
            s.MultaDesc = 0;
            s.Ejecucion = 0;
            //sumatorias         
            s.DescuentoGral = 0;
            s.Importe = 0;
            s.PeriodoGral = "- ---- / - ----";
            //estado
            ServicioEdo Estado = new ServicioEdo();
            Estado.Adicional = 0;
            Estado.AntAdicional = 0;
            Estado.AntInfraestructura = 0;
            Estado.Dap = 0;
            Estado.Descuentos = 0;
            Estado.Ejecucion = 0;
            Estado.Importe = 0;
            Estado.Infraestructura = 0;
            Estado.Limpieza = 0;
            Estado.Multa = 0;
            Estado.PeriodoGral = "- ---- / - ----";
            Estado.Recargo = 0;
            Estado.Recoleccion = 0;
            Estado.Rezagos = 0;
            s.Estado = Estado;
            s.Importe = 0;
            //public
            s.TextError = "";
            //public MensajesInterfaz mensaje;
            return s;
        }

        #endregion

        #region Impuesto Predial

        public decimal ImpuestoPorBimestre(decimal baseGravable, decimal baseImpuesto, decimal SM, int tipoPredio, int ejercicio, ref string textError)
        {
            decimal impuestoAnual = 0;
            decimal bimestre = 0;
            try
            {
                cCuotasPredio c = new cCuotasPredioBL().GetByTipoPredio(ejercicio, tipoPredio);
                if (c == null)
                {
                    textError = new Utileria().GetDescription(MensajesInterfaz.CuotaTipoPredio) + ejercicio.ToString();
                    return -1;
                }

                decimal smAnualizado = new cParametroCobroBL().GetByClave("SmAnualizado");
                if (smAnualizado == -1)
                {
                    textError = new Utileria().GetDescription(MensajesInterfaz.SalarioMinumoAnualizado);
                    return -1;
                }
                smAnualizado = (smAnualizado * SM) * (c.CuotasCobro / 100);
                decimal dosMillar = new cParametroCobroBL().GetByClave("DosMillar");
                if (dosMillar == -1)
                {
                    textError = new Utileria().GetDescription(MensajesInterfaz.DosMillar);
                    return -1;
                }

                decimal tresMillar = new cParametroCobroBL().GetByClave("TresMillar");
                if (tresMillar == -1)
                {
                    textError = new Utileria().GetDescription(MensajesInterfaz.TresMillar);
                    return -1;
                }

                switch (tipoPredio)
                {
                    case 1://urbano,baldio,interes social,
                    case 3: //baldio
                    case 4: //Bienes publicos
                    case 7: //tasa5
                    case 8: //popular 
                        if (baseGravable > baseImpuesto)  
                            impuestoAnual = baseImpuesto * dosMillar + ((baseGravable - baseImpuesto) * tresMillar);
                            if (ejercicio == 2017)
                            {                                
                                if (impuestoAnual < smAnualizado)
                                    impuestoAnual = smAnualizado;
                            }
                            else
                            {
                                if (impuestoAnual < (baseImpuesto * dosMillar))
                                        impuestoAnual = baseImpuesto * dosMillar;                                    
                            }   
                        break;
                    case 2: //rustico
                    case 5://comunal
                    case 6://ejidal
                        impuestoAnual = baseGravable * dosMillar;
                        if (ejercicio == 2017)
                        {
                            //impuestoAnual = baseImpuesto * dosMillar + ((baseGravable - baseImpuesto) * tresMillar);
                            if (impuestoAnual < smAnualizado)
                                impuestoAnual = smAnualizado;
                        }
                        else
                        {
                            if (impuestoAnual < (baseImpuesto * dosMillar))
                                impuestoAnual = baseImpuesto * dosMillar;
                        }
                        //if (impuestoAnual < smAnualizado)
                        //    impuestoAnual = smAnualizado;
                        //if (impuestoAnual < baseImpuesto * dosMillar)  
                        //    impuestoAnual = baseImpuesto * dosMillar;
                        break;
                    default: //otros
                        impuestoAnual = baseGravable * dosMillar;
                        //if (impuestoAnual < smAnualizado)
                        //    impuestoAnual = smAnualizadoimpuestoAnual = baseGravable * dosMillar;
                        if (ejercicio == 2017)
                        {
                            //impuestoAnual = baseImpuesto * dosMillar + ((baseGravable - baseImpuesto) * tresMillar);
                            if (impuestoAnual < smAnualizado)
                                impuestoAnual = smAnualizado;
                        }
                        else
                        {
                            if (impuestoAnual < (baseImpuesto * dosMillar))
                                impuestoAnual = baseImpuesto * dosMillar;
                        }
                        //if (impuestoAnual < baseImpuesto * dosMillar)
                        //    impuestoAnual = baseImpuesto * dosMillar;
                        break;
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("Saldos.ImpuestoPorBimestre.Exception", ex, "par ametros-- " + baseGravable.ToString() + "," + baseImpuesto.ToString() + "," + SM.ToString() + "," + tipoPredio.ToString() + "," + ejercicio.ToString());
                return bimestre = 0;
            }
            bimestre = impuestoAnual / 6;
            return bimestre;
        }

        public decimal ImpuestoPorBimestreCuota(decimal baseGravable, decimal baseImpuesto, decimal SM, int tipoPredio, int ejercicio, ref string textError)
        {
            decimal impuestoAnual = 0;
            decimal bimestre = 0;
            try
            {
                cCuotasPredio c = new cCuotasPredioBL().GetByTipoPredio(ejercicio, tipoPredio);
                if (c == null)
                {
                    textError = new Utileria().GetDescription(MensajesInterfaz.CuotaTipoPredio) + ejercicio.ToString();
                    return -1;
                }

                decimal smAnualizado = new cParametroCobroBL().GetByClave("SmAnualizado");
                if (smAnualizado == -1)
                {
                    textError = new Utileria().GetDescription(MensajesInterfaz.SalarioMinumoAnualizado);
                    return -1;
                }
                smAnualizado = (smAnualizado * SM) * (c.CuotasCobro / 100);


                decimal dosMillar = new cParametroCobroBL().GetByClave("DosMillar");
                if (dosMillar == -1)
                {
                    textError = new Utileria().GetDescription(MensajesInterfaz.DosMillar);
                    return -1;
                }

                decimal tresMillar = new cParametroCobroBL().GetByClave("TresMillar");
                if (tresMillar == -1)
                {
                    textError = new Utileria().GetDescription(MensajesInterfaz.TresMillar);
                    return -1;
                }

                switch (tipoPredio)
                {
                    case 1://urbano,baldio,interes social,
                    case 3://baldio
                    case 4://bien publico
                    case 7://tasa5
                    case 8://popular  
                        if (baseGravable > baseImpuesto)
                            impuestoAnual = baseImpuesto * dosMillar + ((baseGravable - baseImpuesto) * tresMillar);

                        if (impuestoAnual < smAnualizado)
                            impuestoAnual = smAnualizado;
                        break;
                    case 2://rustico
                    case 5://comunal
                    case 6://ejidal   
                        impuestoAnual = baseGravable * dosMillar;
                        if (impuestoAnual < smAnualizado)
                            impuestoAnual = smAnualizado;
                        break;
                    default: //otros
                        impuestoAnual = baseGravable * dosMillar;
                        if (impuestoAnual < smAnualizado)
                            impuestoAnual = smAnualizado;
                        break;
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("Saldos.ImpuestoPorBimestre.Exception", ex, "par ametros-- " + baseGravable.ToString() + "," + baseImpuesto.ToString() + "," + SM.ToString() + "," + tipoPredio.ToString() + "," + ejercicio.ToString());
                return bimestre = 0;
            }
            bimestre = impuestoAnual / 6;
            return bimestre;
        }

        public List<ImpuestoBimestral> CalculaImpuestoBimestral(int idClavePredial, int bInicial, int eInicial, int bFinal, int eFinal, int tipoPredio)
        {
            List<ImpuestoBimestral> impuestoB = new List<ImpuestoBimestral>();
            if (bInicial == 0 && bFinal == 0 && eInicial == 0 && eFinal == 0) return impuestoB;
            decimal indiceAnt = 0;
            int restaBim = 0;
            DateTime fPago = DateTime.Today;
            cBaseImpuesto baseI = new cBaseImpuesto();
            cBaseGravable bg = new cBaseGravable();
            cSalarioMinimo sm = new cSalarioMinimo();
            ImpuestoBimestral ip = new ImpuestoBimestral();
            MensajesInterfaz msg = new MensajesInterfaz();
            try
            {

                string error = string.Empty;
                decimal bimAnual = 0;
                int bimActual = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(fPago.Month / 2)));
                int bimestres = CuentaBimestre(bInicial, eInicial, bFinal, eFinal, ref msg);
                if (msg > 0)
                {
                    impuestoB = new List<ImpuestoBimestral>();
                    ip.mensaje = msg;
                    ip.TextError = new Utileria().GetDescription(ip.mensaje) + "Para el periodo " + eInicial.ToString() + " " + eFinal.ToString();
                    impuestoB.Add(ip);
                    return impuestoB;
                }

                List<cBaseImpuesto> bi = new cBaseImpuestoBL().GetByPeriodo(eInicial, eFinal);
                List<cBaseGravable> baseG = new cBaseGravableBL().GetByPeriodo(idClavePredial, eInicial, eFinal);
                List<cSalarioMinimo> salMin = new cSalarioMinimoBL().GetByPeriodo(eInicial, eFinal);

                if (baseG == null)
                {
                    impuestoB = new List<ImpuestoBimestral>();
                    ip.mensaje = MensajesInterfaz.BaseGravable;
                    ip.TextError = new Utileria().GetDescription(ip.mensaje) + "Para el periodo " + eInicial.ToString() + " " + eFinal.ToString();
                    impuestoB.Add(ip);
                    return impuestoB;
                }
                if (baseI == null)
                {
                    impuestoB = new List<ImpuestoBimestral>();
                    ip.mensaje = MensajesInterfaz.BaseDelImpuesto;
                    ip.TextError = new Utileria().GetDescription(ip.mensaje) + "Para el periodo " + eInicial.ToString() + " " + eFinal.ToString();
                    impuestoB.Add(ip);
                    return impuestoB;
                }
                if (sm == null)
                {
                    impuestoB = new List<ImpuestoBimestral>();
                    ip.mensaje = MensajesInterfaz.SalarioMinimo;
                    ip.TextError = new Utileria().GetDescription(ip.mensaje) + "Para el periodo " + eInicial.ToString() + " " + eFinal.ToString();
                    impuestoB.Add(ip);
                    return impuestoB;
                }

                //string AplicaActualizacion = new cParametroSistemaBL().GetValorByClave("AplicarActualizacionINP").ToString();
                if (AplicaActualizacion == "")
                {
                    impuestoB = new List<ImpuestoBimestral>();
                    ip.mensaje = MensajesInterfaz.AplicarAtualizacionINP;
                    impuestoB.Add(ip);
                    return impuestoB;
                }
                indiceActual = 0;
                if (AplicaActualizacion == "SI")
                {
                    decimal indiceActual = new cIndicePrecioBL().ValorIndiceActual();
                    if (indiceActual == 0)
                    {
                        indiceActual = new cIndicePrecioBL().ValorIndiceActualSinCambio();
                        if (indiceActual == 0)
                        {
                            impuestoB = new List<ImpuestoBimestral>();
                            ip.mensaje = MensajesInterfaz.INPactual;
                            ip.TextError = new Utileria().GetDescription(ip.mensaje) + eInicial.ToString() + "-" + DateTime.Today.Month.ToString();
                            impuestoB.Add(ip);
                            return impuestoB;
                        }
                    }
                }



                decimal porcAdicional = (new cParametroCobroBL().GetByClave("PorcentajeAdicional"));
                if (porcAdicional == -1)
                {
                    impuestoB = new List<ImpuestoBimestral>();
                    ip.mensaje = MensajesInterfaz.PorcAdicional;
                    impuestoB.Add(ip);
                    return impuestoB;
                }
                porcAdicional = porcAdicional / 100;

                restaBim = bimestres;

                for (int i = 1; i <= bimestres; i++)
                {
                    ip = new ImpuestoBimestral();
                    if (bInicial == 7 || i == 1)
                    {
                        #region revisa cambio de año
                        if (bInicial == 7)
                        {
                            bInicial = 1;
                            eInicial++;
                        }

                        ip = new ImpuestoBimestral();

                        baseI = bi.FirstOrDefault(o => o.Ejercicio == eInicial && o.Activo == true);
                        bg = baseG.FirstOrDefault(o => o.Ejercicio == eInicial && o.Activo == true);
                        sm = salMin.FirstOrDefault(o => o.Ejercicio == eInicial && o.Activo == true);

                        //validaciones
                        if (bg == null)
                        {
                            impuestoB = new List<ImpuestoBimestral>();
                            ip.mensaje = MensajesInterfaz.BaseGravable;
                            ip.TextError = new Utileria().GetDescription(ip.mensaje) + eInicial.ToString();
                            impuestoB.Add(ip);
                            return impuestoB;
                        }
                        if (baseI == null)
                        {
                            impuestoB = new List<ImpuestoBimestral>();
                            ip.mensaje = MensajesInterfaz.BaseDelImpuesto;
                            ip.TextError = new Utileria().GetDescription(ip.mensaje) + eInicial.ToString();
                            impuestoB.Add(ip);
                            return impuestoB;
                        }
                        if (sm == null)
                        {
                            impuestoB = new List<ImpuestoBimestral>();
                            ip.mensaje = MensajesInterfaz.SalarioMinimo;
                            ip.TextError = new Utileria().GetDescription(ip.mensaje) + eInicial.ToString();
                            impuestoB.Add(ip);
                            return impuestoB;
                        }
                        cParametroSistema municipio = new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO");
                        if (municipio.Valor == "ZAPATA")
                            bimAnual = ImpuestoPorBimestreCuota(bg.Valor, baseI.Valor, sm.Importe, tipoPredio, eInicial, ref error);
                        else
                            bimAnual = ImpuestoPorBimestreCuota(bg.Valor, baseI.Valor, sm.Importe, tipoPredio, eInicial, ref error);
                            //se comenta esta linea porq  yautepec dice que deben tener cuota minima
                            //bimAnual = ImpuestoPorBimestre(bg.Valor, baseI.Valor, sm.Importe, tipoPredio, eInicial, ref error);

                        #endregion cambio año
                    }
                    ip.Ejercicio = eInicial;
                    ip.BaseGravable = bg.Valor;
                    ip.BaseImpuesto = baseI.Valor;
                    ip.Uma = sm.Importe;

                    ip.Impuesto = bimAnual;
                    if (ip.Impuesto == -1)
                    {
                        impuestoB = new List<ImpuestoBimestral>();
                        ip.TextError = error;
                        ip.mensaje = MensajesInterfaz.ImpuestoPorBimestre;
                        impuestoB.Add(ip);
                        return impuestoB;
                    }
                    ip.PorcentajeRecargo = new cTarifaRecargoBL().PorcentajeRecargo(bInicial, eInicial, bimActual, fPago.Year,ref listRecargos);

                    ip.Adicional = ip.Impuesto * porcAdicional;
                    ip.Recargo = 0;

                    if (Convert.ToInt32(eInicial.ToString() + bInicial.ToString()) <= Convert.ToInt32(fPago.Year.ToString() + bimActual.ToString()))
                    {
                        if (AplicaActualizacion == "SI")
                        {
                            indiceAnt = new cIndicePrecioBL().ValorIndiceAnterior(eInicial, bInicial);
                            if (indiceAnt == 0)
                            {
                                impuestoB = new List<ImpuestoBimestral>();
                                ip.mensaje = MensajesInterfaz.INPanterior;
                                ip.TextError = new Utileria().GetDescription(ip.mensaje) + eInicial.ToString();
                                impuestoB.Add(ip);
                                return impuestoB;
                            }
                            ip.IndAnterior = indiceAnt;
                            ip.IndActual = indiceActual;
                            ip.PorcentajeINP = (indiceActual / indiceAnt) - 1;
                            if (indiceAnt > indiceActual) ip.PorcentajeINP = 0;

                            ip.ImpuestoINP = (ip.Impuesto * ip.PorcentajeINP);
                            ip.AdicionalINP = ip.ImpuestoINP * porcAdicional;
                        }
                        ip.Recargo = (ip.Impuesto + ip.Adicional + ip.ImpuestoINP + ip.AdicionalINP) * ip.PorcentajeRecargo;
                    }

                    ip.Bimestre = restaBim;
                    bInicial++;
                    restaBim = restaBim - 1;
                    impuestoB.Add(ip);
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("Saldos.CalculaImpuesto.Exception", ex, "Parametros--- " + idClavePredial.ToString() + "," + bInicial.ToString() + "," + eInicial.ToString() + "," + bFinal.ToString() + "," + eFinal.ToString() + ", tipo predio: " + tipoPredio + ", bg:" + bg.Valor + ", base impuesto: " + baseI.Valor + ", sm: " + sm.Importe);
                impuestoB = new List<ImpuestoBimestral>();
                ip.mensaje = MensajesInterfaz.EjercicioIncorrecto;
                impuestoB.Add(ip);
                return impuestoB;
            }
            return impuestoB;
        }

        public Impuesto CalculaCobro(int idClavePredial, string soloDiferencia, int bInicial, int eInicial, int bFinal, int eFinal, int descJyP, int descRez, string tramite)
        {
            Impuesto ip = new Impuesto();
            List<ImpuestoEdo> impAnual = new List<ImpuestoEdo>();
            ImpuestoEdo impA = new ImpuestoEdo();

            DateTime fPago = DateTime.Today;
            string ClaveDescuento = string.Empty;
            Periodo per = new Periodo();
            int descGral = 0;
            ip.mensaje = 0;
            string noBim = string.Empty;
            int ejercicioAdicional = Convert.ToInt16(new cParametroSistemaBL().GetValorByClave("EJERCICIO COBRO ADICIONAL").ToString());
            int j = 0; // contador para el impuestoanulizado

            try
            {
                int convenio = new tConvenioBL().ObtenerConvenioPorIdPredio(idClavePredial);
                if (convenio > 0)
                {
                    ip.mensaje = MensajesInterfaz.PredioEnConvenio;
                    ip.TextError = new Utileria().GetDescription(ip.mensaje) + " Para aclaraciones pasar a Rezagos. ";
                    return ip;
                }

                List<Diferencias> dif = new cDiferenciaBL().CalculaDiferencias(idClavePredial,ref listRecargos);
                if (dif.Count > 0)
                {
                    if (dif[0].mensaje != 0)
                    {
                        ip.mensaje = dif[0].mensaje;
                        return ip;
                    }
                    else
                    {
                        cDiferencia d = new cDiferenciaBL().GetByClaveCatastral(idClavePredial);
                        ip.IdDiferencia = d.Id;
                        d = null;
                    }
                }
                if (tramite != "CalculaPredial") //En donde se llame a calcula cobro deben poner el nombre de la ventana
                {
                    per = ValidaPeriodoPago(bInicial, eInicial, bFinal, eFinal, tramite);
                    if (per.mensaje != 0)
                    {
                        ip.mensaje = MensajesInterfaz.EjercicioIncorrecto;
                        return ip;
                    }
                    bInicial = per.bInicial;
                    eInicial = per.eInicial;
                    bFinal = per.bFinal;
                    eFinal = per.eFinal;
                }

    
                cPredio pred = new cPredioBL().GetByConstraint(idClavePredial);
                int bimActual = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(fPago.Month / 2)));
                List<ImpuestoBimestral> impBim = new List<ImpuestoBimestral>();

                Descuento descAplicar = ValidaDescuentoClave(pred.Id, "IP", descJyP, descRez, ref descGral);

                if (((bInicial == 0 && bFinal == 0 || eInicial == 0 && eFinal == 0 ) && ip.IdDiferencia > 0) || (eInicial > eFinal && ip.IdDiferencia > 0)  )
                {
                    soloDiferencia = "SI";
                }
               
                if (((bInicial == 0 && bFinal == 0) || (eInicial == 0 && eFinal == 0)) && (dif.Count == 0 || dif == null))
                {
                    InicializaIP(ip);
                    return ip;
                }
                
                if (soloDiferencia.ToUpper() == "NO") //ok
                {
                    #region Todo los impuestos incluyendo diferencias  si las hay

                    if (eInicial > eFinal)
                    {
                        ip.TextError = "Periodo de cobro incorrecto, el Año inicial es mayor al año final";
                        return ip;
                    }

                    impBim = CalculaImpuestoBimestral(pred.Id, bInicial, eInicial, bFinal, eFinal, pred.IdTipoPredio);
                    if (impBim[0].mensaje != 0)
                    {
                        ip.mensaje = MensajesInterfaz.ImpuestoPorBimestre;
                        ip.TextError = impBim[0].TextError;
                        return ip;
                    }

                    //Anticipado
                    ip.PerAnticipado = DevuelvePeriodoPago(bInicial, eInicial, bFinal, eFinal, "ANTICIPADO");
                    if (ip.PerAnticipado != null)
                    {
                        decimal b = new cBaseImpuestoBL().GetByEjercicio(fPago.Year + 1);
                        decimal dos = new cParametroCobroBL().GetByClave("DosMillar");
                        ip.AntImpuesto = Utileria.Redondeo((from i in impBim where i.Ejercicio >= fPago.Year + 1 select i.Impuesto).Sum());
                        ip.AntImpuestoPri = Utileria.Redondeo(b * dos);
                        ip.AntImpuestoExc = Utileria.Redondeo(ip.AntImpuesto - ip.AntImpuestoPri);
                        ip.AntAdicional = Utileria.Redondeo(ip.AntImpuesto * porcAdicional);
                        if (ip.PerAnticipado.bInicial == 1 && ip.PerAnticipado.bFinal == 6)
                        {//aplica descuento por año completo para campañas
                            ip.AntDescImpuesto = Utileria.Redondeo(ip.AntImpuesto * (descAplicar.AnticipadoImpuesto / 100));
                            ip.AntDescAdicional = Utileria.Redondeo(ip.AntAdicional * (descAplicar.AnticipadoAdicional / 100));
                        }
                        ip.AntBimestre = (from i in impBim where i.Ejercicio >= fPago.Year + 1 select i.Impuesto).Count();// Utileria.Redondeo(ip.AntImpuesto / 6);
                    }
                    //Actual 
                    ip.PerActual = DevuelvePeriodoPago(bInicial, eInicial, bFinal, eFinal, "ACTUAL");
                    if (ip.PerActual != null)
                    {
                        decimal b = new cBaseImpuestoBL().GetByEjercicio(fPago.Year);
                        decimal dos = new cParametroCobroBL().GetByClave("DosMillar");
                        ip.ActBimestre = (from i in impBim where i.Ejercicio == fPago.Year select i.Impuesto).Count();
                        ip.ActImpuesto = Utileria.Redondeo((from i in impBim where i.Ejercicio == fPago.Year select i.Impuesto).Sum());
                        ip.ActImpuestoINP = Utileria.Redondeo((from i in impBim where i.Ejercicio == fPago.Year select i.ImpuestoINP).Sum());
                        ip.ActImpuesto += ip.ActImpuestoINP; // SE AGREGA ESTE VALOR AL IMPUESTO
                        ip.ActImpuestoINP = 0;
                        ip.ActImpuestoPri = Utileria.Redondeo(b * dos);
                        ip.ActImpuestoExc = Utileria.Redondeo(ip.ActImpuesto - ip.ActImpuestoPri);
                        string claveG = new cParametroSistemaBL().GetValorByClave("DescuentoGlobalIP");
                        string claveJYP = new cParametroSistemaBL().GetValorByClave("DescuentoJyP");
                        if (ip.PerActual.bInicial == 1 && ip.PerActual.bFinal == 6 && (descAplicar.Clave == claveG || descAplicar.Clave == claveJYP))
                        {
                            if ((descAplicar != null) && (descAplicar.ActualImpuesto > 0))
                                ip.ActDescImpuesto = Utileria.Redondeo(ip.ActImpuesto * (descAplicar.ActualImpuesto / 100));
                        }
                        else
                            if (descAplicar.Clave != claveG && descAplicar.Clave != claveJYP)
                                ip.ActDescImpuesto = Utileria.Redondeo(ip.ActImpuesto * (descAplicar.ActualImpuesto / 100));
                       
                        ip.ActRecargo = 0;
                        ip.ActDescRecargo = 0;
                        decimal mesRecProntoPago = new cParametroCobroBL().GetByClave("MESSINRECARGOPORPRONTOPAGO");
                        if (mesRecProntoPago == -1)
                        {
                            mesRecProntoPago = 0;
                        }

                        if (fPago.Month > mesRecProntoPago)
                        {
                            ip.ActRecargo = Utileria.Redondeo((from i in impBim where i.Ejercicio == fPago.Year select i.Recargo).Sum());
                            ip.ActDescRecargo = Utileria.Redondeo(ip.ActRecargo * (descAplicar.ActualRecargo / 100));
                        }
                        ip.ActAdicional = Utileria.Redondeo((ip.ActImpuesto + ip.ActImpuestoINP) * porcAdicional);
                        ip.ActDescAdicional = Utileria.Redondeo(ip.ActAdicional * (descAplicar.ActualAdicional / 100));

                        ip.ActDiferencias = 0;
                        ip.ActDescDiferencias = 0;
                        ip.ActRecDiferencias = 0;
                        ip.ActDescRecDiferencias = 0;
                        if (dif.Count > 0)
                        {
                            ip.ActDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio >= fPago.Year select d.Diferencia).Sum());
                            ip.ActDiferenciasINP = Utileria.Redondeo((from d in dif where d.Ejercicio >= fPago.Year select d.ActualizacionINP).Sum()); // dif.Sum(i => i.Diferencia);                            
                            ip.ActDiferencias += ip.ActDiferenciasINP;
                            ip.ActDiferenciasINP = 0;
                            ip.ActDescDiferencias = Utileria.Redondeo(ip.ActDiferencias * (descAplicar.Diferencia / 100));
                            ip.ActRecDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio >= fPago.Year select d.Recargo).Sum());
                            ip.ActDescRecDiferencias = Utileria.Redondeo(ip.ActRecDiferencias * (descAplicar.DiferenciaRecargo / 100));
                            ip.ActAdicDiferencia = Utileria.Redondeo((ip.ActDiferencias + ip.ActDiferenciasINP) * porcAdicional);
                            ip.ActDescAdicDiferencia = Utileria.Redondeo(ip.ActAdicDiferencia * (descAplicar.ActualAdicional / 100));
                        }

                        // Se ingresa variable para anualizado
                        if (ip.ActImpuesto > 0)
                        {
                            impA = new ImpuestoEdo();
                            impA.PeriodoGral = ip.ActBimestre.ToString() +" " + ip.PerActual.eFinal.ToString();                            
                            impA.AntImpuesto = 0;
                            impA.AntAdicional = 0;
                            impA.Impuesto = ip.ActImpuesto + ip.ActImpuestoINP;
                            impA.Diferencias = ip.ActDiferencias + ip.ActDiferenciasINP;
                            impA.RecDiferencias = ip.ActRecDiferencias;
                            impA.Adicional = ip.ActAdicional + ip.ActAdicDiferencia;
                            impA.Rezagos = 0;
                            impA.Recargos = ip.ActRecargo;
                            impA.Ejecucion = 0;
                            impA.Honorarios = 0;
                            impA.Multas = 0;
                            impA.ActINP = 0;
                            impA.RezagoINP = 0;
                            impA.Descuentos = ip.ActDescImpuesto + ip.ActDescDiferencias + ip.ActDescAdicDiferencia +
                               ip.ActDescAdicional + ip.ActDescRecargo + ip.ActDescRecDiferencias;
                            impA.Importe = (impA.Impuesto + impA.Recargos) - impA.Descuentos;
                            impAnual.Add(impA);
                        }

                    }
                    //rezago         
                    ip.PerRezago = DevuelvePeriodoPago(bInicial, eInicial, bFinal, eFinal, "REZAGO");
                    if (ip.PerRezago != null)
                    {

                        ip.Rezagos = Utileria.Redondeo((from i in impBim where i.Ejercicio < fPago.Year select i.Impuesto).Sum());
                        ip.RezBimestre = (from i in impBim where i.Ejercicio < fPago.Year select i.Impuesto).Count();
                        ip.RezagoINP = Utileria.Redondeo((from i in impBim where i.Ejercicio < fPago.Year select i.ImpuestoINP).Sum());
                        ip.Rezagos += ip.RezagoINP;
                        ip.RezagoINP = 0;
                        ip.RezDescRezagos = Utileria.Redondeo(ip.Rezagos * (descAplicar.Rezago / 100));
                        ip.RezRecargo = Utileria.Redondeo((from i in impBim where i.Ejercicio < fPago.Year select i.Recargo).Sum());
                        ip.RezDescRecargos = Utileria.Redondeo(ip.RezRecargo * (descAplicar.RezagoRecargo / 100));
                        ip.RezAdicional = Utileria.Redondeo((ip.Rezagos + ip.RezagoINP) * porcAdicional);
                        //validar el año de cobro adicionales                        
                        if ((fPago.Year >= ejercicioAdicional && ip.PerRezago.eInicial <= ejercicioAdicional) && ejercicioAdicional > 0)
                        {
                            decimal impuestoAdicional = Utileria.Redondeo((from i in impBim where i.Ejercicio <= ejercicioAdicional select i.Impuesto).Sum())+
                                                        Utileria.Redondeo((from i in impBim where i.Ejercicio <= ejercicioAdicional select i.ImpuestoINP).Sum()); ;
                            ip.RezAdicional = Utileria.Redondeo((impuestoAdicional * Convert.ToDecimal(0.25))) ;
                        }
                        
                        ip.RezDescAdicional = Utileria.Redondeo(ip.RezAdicional * (descAplicar.RezagoAdicional / 100));
                        ip.RezDiferencias = 0;
                        ip.RezDescDiferencias = 0;
                        ip.RezDescRecDiferencias = 0;
                        if (dif.Count > 0)
                        {
                            ip.RezDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio < fPago.Year select d.Diferencia).Sum());
                            ip.RezDiferenciasINP = Utileria.Redondeo((from d in dif where d.Ejercicio < fPago.Year select d.ActualizacionINP).Sum());
                            ip.RezDiferencias += ip.RezDiferenciasINP;
                            ip.RezDiferenciasINP = 0;
                            ip.RezDescDiferencias = Utileria.Redondeo(ip.RezDiferencias * (descAplicar.Diferencia / 100));
                            ip.RezRecDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio < fPago.Year select d.Recargo).Sum());
                            ip.RezDescRecDiferencias = Utileria.Redondeo(ip.RezRecDiferencias * (descAplicar.DiferenciaRecargo / 100));
                            ip.RezAdicDiferencias = Utileria.Redondeo(( ip.RezDiferencias + ip.RezDiferenciasINP) * porcAdicional);

                            int dEjerIni = (from d in dif select d.Ejercicio).Min();
                            int dEjerFin = (from d in dif select d.Ejercicio).Max();
                            if (( dEjerIni <= ejercicioAdicional) && ejercicioAdicional > 0)
                            {
                                decimal impuestoAdicional = Utileria.Redondeo((from d in dif where d.Ejercicio < ejercicioAdicional select d.Diferencia).Sum())  +
                                                            Utileria.Redondeo((from d in dif where d.Ejercicio < ejercicioAdicional select d.ActualizacionINP).Sum());

                                ip.RezAdicDiferencias = Utileria.Redondeo((impuestoAdicional * Convert.ToDecimal(0.25)));
                            }
                            ip.RezDescAdicDiferencias = Utileria.Redondeo(ip.RezAdicDiferencias * ( descAplicar.RezagoAdicional / 100 ));
                        }

                        int ejercicioAnual = ip.PerRezago.eInicial;
                       
                        if (ip.Rezagos > 0) //se ocupa para el llenado del anualizado del edo cta
                        {                            
                            for (int i = j; ejercicioAnual <=  ip.PerRezago.eFinal; i++)
                            {
                                impA = new ImpuestoEdo();
                                noBim = ((from k in impBim where k.Ejercicio == ejercicioAnual select k.Impuesto).Count()).ToString();
                                impA.PeriodoGral = noBim + " " + ejercicioAnual.ToString();
                                impA.AntImpuesto = 0;
                                impA.AntAdicional = 0;
                                impA.Impuesto = Utileria.Redondeo((from k in impBim where k.Ejercicio == ejercicioAnual select k.Impuesto + k.ImpuestoINP).Sum());
                                impA.Diferencias = 0;
                                impA.RecDiferencias = 0;
                                impA.Adicional = 0;
                                impA.Rezagos = 0;
                                impA.Recargos = Utileria.Redondeo((from k in impBim where k.Ejercicio == ejercicioAnual select k.Recargo).Sum());
                                impA.Ejecucion = 0;
                                impA.Honorarios = 0;
                                impA.Multas = 0;
                                impA.ActINP = 0;
                                impA.RezagoINP = 0;
                                impA.Descuentos = Utileria.Redondeo(impA.Recargos * (descAplicar.RezagoRecargo / 100)) +
                                                        Utileria.Redondeo(impA.Impuesto * (descAplicar.Rezago / 100));
                                impA.Importe = (impA.Impuesto + impA.Recargos) - impA.Descuentos;
                                impAnual.Add(impA);
                                ejercicioAnual++;
                            }
                        }
                    }
                   
                    //calcula diferencias si las hay.
                    if (ip.PerActual == null && ip.PerRezago == null && dif.Count > 0)
                    {
                        ip.ActDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio == fPago.Year select d.Diferencia).Sum());
                        ip.ActDiferenciasINP = Utileria.Redondeo((from d in dif where d.Ejercicio == fPago.Year select d.ActualizacionINP).Sum()); // dif.Sum(i => i.Diferencia);                            
                        ip.ActDiferencias += ip.ActDiferenciasINP;
                        ip.ActDiferenciasINP = 0;
                        ip.ActDescDiferencias = Utileria.Redondeo(ip.ActDiferencias * (descAplicar.Diferencia / 100));
                        ip.ActRecDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio == fPago.Year select d.Recargo).Sum());
                        ip.ActDescRecDiferencias = Utileria.Redondeo(ip.ActRecDiferencias * (descAplicar.DiferenciaRecargo / 100));

                        ip.RezDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio < fPago.Year select d.Diferencia).Sum());
                        ip.RezDiferenciasINP = Utileria.Redondeo((from d in dif where d.Ejercicio < fPago.Year select d.ActualizacionINP).Sum());
                        ip.RezDiferencias += ip.RezDiferenciasINP;
                        ip.RezDiferenciasINP = 0;
                        ip.RezDescDiferencias = Utileria.Redondeo(ip.RezDiferencias * (descAplicar.Diferencia / 100));
                        ip.RezRecDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio < fPago.Year select d.Recargo).Sum());
                        ip.RezDescRecDiferencias = Utileria.Redondeo(ip.RezRecDiferencias * (descAplicar.DiferenciaRecargo / 100));

                        ip.RezAdicional = Utileria.Redondeo((ip.Rezagos + ip.RezagoINP + ip.RezDiferencias + ip.RezDiferenciasINP) * porcAdicional);
                        ip.RezDescAdicional = Utileria.Redondeo(ip.RezAdicional * (descAplicar.RezagoAdicional / 100));

                        ip.ActAdicional = Utileria.Redondeo((ip.ActImpuesto + ip.ActImpuestoINP + ip.ActDiferencias + ip.ActDiferenciasINP) * porcAdicional);
                        ip.ActDescAdicional = Utileria.Redondeo(ip.ActAdicional * (descAplicar.ActualAdicional / 100));
                    }

                    if (ip.PerActual != null && dif.Count > 0 && ip.PerRezago == null)
                    {
                        ip.RezDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio < fPago.Year select d.Diferencia).Sum());
                        ip.RezDiferenciasINP = Utileria.Redondeo((from d in dif where d.Ejercicio < fPago.Year select d.ActualizacionINP).Sum());
                        ip.RezDiferencias += ip.RezDiferenciasINP;
                        ip.RezDiferenciasINP = 0;
                        ip.RezDescDiferencias = Utileria.Redondeo(ip.RezDiferencias * (descAplicar.Diferencia / 100));
                        ip.RezRecDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio < fPago.Year select d.Recargo).Sum());
                        ip.RezDescRecDiferencias = Utileria.Redondeo(ip.RezRecDiferencias * (descAplicar.DiferenciaRecargo / 100));

                        ip.RezAdicional = Utileria.Redondeo((ip.Rezagos + ip.RezagoINP + ip.RezDiferencias + ip.RezDiferenciasINP) * porcAdicional);
                        ip.RezDescAdicional = Utileria.Redondeo(ip.RezAdicional * (descAplicar.RezagoAdicional / 100));
                    }

                    if (tramite != "Rezago")
                    {
                        //Obtenemos los apercibimientos si es que los tiene el predio
                        tRequerimiento q = new tRequerimientoBL().obtenerRequerimientoporIdPredio(idClavePredial, "IP");
                        if (q != null)
                        {
                            ip.Multa = Convert.ToDecimal(q.Multa);
                            ip.MultaDesc = Utileria.Redondeo(ip.Multa * (descAplicar.Multas / 100));
                            ip.IdRequerimiento = q.Id;

                            ip.Ejecucion = Convert.ToDecimal(q.Ejecucion);
                            ip.EjecucionDesc = Utileria.Redondeo(ip.Ejecucion * (descAplicar.Ejecucion / 100));
                            ip.Honorarios = Convert.ToDecimal(q.Honorarios);
                            ip.HonorariosDesc = Utileria.Redondeo(ip.Honorarios * (descAplicar.Honorarios / 100)); ;

                        }
                        else
                        { //incluimos las cuotas fijas para estos conceptos
                            ip.Multa = 0;
                            ip.MultaDesc = 0;
                            ip.Ejecucion = 0;
                            ip.EjecucionDesc = 0;
                            ip.IdRequerimiento = 0;
                            ip.Honorarios = 0;
                            ip.HonorariosDesc = 0;

                            if (tramite != "Rezago")
                            {
                                int periodoValido = 0;
                                string periodoValida = new cParametroSistemaBL().GetValorByClave("PeriodoCobroMultasYEjec");
                                if (periodoValida == "")
                                    periodoValido = 0;
                                else
                                {
                                    periodoValida = periodoValida.Substring(1, 4) + periodoValida.Substring(0, 1);
                                    periodoValido = Convert.ToInt32(periodoValida);
                                }
                                int periodoInicial = Convert.ToInt32(eInicial.ToString() + bInicial.ToString());
                                if (periodoValido >= periodoInicial)//validar si el periodo es menor al de la tabla cparametros
                                {
                                    ip.Multa = Utileria.Redondeo(cobroMulta);
                                    ip.MultaDesc = Utileria.Redondeo(ip.Multa * (descAplicar.Multas / 100));
                                    ip.Ejecucion = Utileria.Redondeo(cobroEjecucion);
                                    ip.EjecucionDesc = Utileria.Redondeo(ip.Ejecucion * (descAplicar.Ejecucion / 100));
                                    ip.Honorarios = 0;
                                    ip.HonorariosDesc = 0;
                                    ip.IdRequerimiento = 0;
                                }
                            }
                        }
                    }//tramite difererente de "Rezago"
                    //sumatorias  
                    ip.Importe =   ip.AntAdicional + ip.AntImpuesto +
                                   ip.ActImpuesto + ip.ActAdicional + ip.ActRecargo + ip.ActImpuestoINP +
                                   ip.ActDiferencias + ip.ActRecDiferencias + ip.ActDiferenciasINP + ip.ActAdicDiferencia +
                                   ip.ActRezago + ip.Rezagos + ip.RezAdicional + ip.RezRecargo + ip.RezagoINP +
                                   ip.RezDiferencias + ip.RezRecDiferencias + ip.RezDiferenciasINP +ip.RezAdicDiferencias +
                                   ip.Multa + ip.Ejecucion + ip.Honorarios ;
                    
                    
                    ip.DescuentoGral = ip.AntDescImpuesto + ip.AntDescAdicional +
                                       ip.ActDescImpuesto + ip.ActDescAdicional + ip.ActDescRecargo + ip.ActDescRezago +
                                       ip.ActDescDiferencias + ip.ActDescRecDiferencias + ip.ActDescAdicDiferencia +
                                       ip.RezDescAdicional + ip.RezDescRezagos + ip.RezDescRecargos + 
                                       ip.RezDescDiferencias + ip.RezDescRecDiferencias + ip.RezDescAdicDiferencias+
                                       ip.MultaDesc +ip.EjecucionDesc + ip.HonorariosDesc;

                    ip.Importe = ip.Importe - ip.DescuentoGral;

                    #endregion Impuesto Completo
                } // NO   
               

                if (soloDiferencia.ToUpper() == "SI" )//|| (ip.PerActual != null && dif != null && ip.PerRezago == null) || (ip.PerActual == null && ip.PerRezago == null && dif != null))
                {
                    #region Solo diferencias
                    //ingresar todos los campos en ceros menos las diferencias
                    //Anticipado
                    ip.PerAnticipado = null;
                    ip.AntBimestre = 0;
                    ip.AntImpuesto = 0;
                    ip.AntImpuestoPri = 0;
                    ip.AntImpuestoExc = 0;
                    ip.AntDescImpuesto = 0;
                    ip.AntAdicional = 0;
                    ip.AntDescAdicional = 0;

                    //Actual 
                    ip.PerActual = null;
                    ip.ActBimestre = 0;
                    ip.ActImpuesto = 0;
                    ip.ActImpuestoPri = 0;
                    ip.ActImpuestoExc = 0;
                    ip.ActDescImpuesto = 0;
                    ip.ActRecargo = 0;
                    ip.ActDescAdicional = 0;
                    ip.ActDiferencias = 0;
                    ip.ActDescDiferencias = 0;
                    ip.ActRecDiferencias = 0;
                    ip.ActAdicional = 0;
                    ip.ActRezago = 0;
                    ip.ActImpuestoINP = 0;
                    if (dif.Count > 0)
                    {
                        //diferencias actuales
                        ip.ActDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio >= fPago.Year select d.Diferencia).Sum());
                        ip.ActDiferenciasINP = Utileria.Redondeo((from d in dif where d.Ejercicio >= fPago.Year select d.ActualizacionINP).Sum()); // dif.Sum(i => i.Diferencia);                            
                        ip.ActDiferencias += ip.ActDiferenciasINP;
                        ip.ActDiferenciasINP = 0;
                        ip.ActDescDiferencias = Utileria.Redondeo(ip.ActDiferencias * (descAplicar.Diferencia/ 100));
                        ip.ActRecDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio >= fPago.Year select d.Recargo).Sum());
                        ip.ActDescRecDiferencias = Utileria.Redondeo(ip.ActRecDiferencias * (descAplicar.DiferenciaRecargo / 100));
                        ip.ActAdicDiferencia = Utileria.Redondeo((ip.ActDiferencias + ip.ActDiferenciasINP) * porcAdicional);
                        ip.ActDescAdicDiferencia = Utileria.Redondeo(ip.ActAdicDiferencia * (descAplicar.ActualAdicional / 100));
                        //diferencias rezago
                        ip.RezDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio < fPago.Year select d.Diferencia).Sum());
                        ip.RezDiferenciasINP = Utileria.Redondeo((from d in dif where d.Ejercicio < fPago.Year select d.ActualizacionINP).Sum());
                        ip.RezDiferencias += ip.RezDiferenciasINP;
                        ip.RezDiferenciasINP = 0;
                        ip.RezDescDiferencias = Utileria.Redondeo(ip.RezDiferencias * (descAplicar.Diferencia / 100));
                        ip.RezRecDiferencias = Utileria.Redondeo((from d in dif where d.Ejercicio < fPago.Year select d.Recargo).Sum());
                        ip.RezDescRecDiferencias = Utileria.Redondeo(ip.RezRecDiferencias * (descAplicar.DiferenciaRecargo / 100));
                        ip.RezAdicDiferencias = Utileria.Redondeo((ip.RezDiferencias + ip.RezDiferenciasINP) * porcAdicional);

                        int dEjerIni = (from d in dif select d.Ejercicio).Min();
                        int dEjerFin = (from d in dif select d.Ejercicio).Max();
                        if ((dEjerIni <= ejercicioAdicional) && ejercicioAdicional > 0)
                        {
                            decimal impuestoAdicional = Utileria.Redondeo((from d in dif where d.Ejercicio < ejercicioAdicional select d.Diferencia).Sum()) +
                                                        Utileria.Redondeo((from d in dif where d.Ejercicio < ejercicioAdicional select d.ActualizacionINP).Sum());

                            ip.RezAdicDiferencias = Utileria.Redondeo((impuestoAdicional * Convert.ToDecimal( porcAdicional /100)));
                        }
                        ip.RezDescAdicDiferencias = Utileria.Redondeo(ip.RezAdicDiferencias * (descAplicar.RezagoAdicional / 100));
                    }

                    //rezago         
                    ip.PerRezago = null;
                    ip.Rezagos = 0;
                    ip.RezDescRezagos = 0;
                    ip.RezRecargo = 0;
                    ip.RezDescRecargos = 0;
                    ip.RezAdicional = 0;
                    ip.Multa = 0;
                    ip.MultaDesc = 0;
                    ip.Ejecucion = 0;
                    //sumatorias            
                    ip.Importe = ip.ActDiferencias + ip.ActDiferenciasINP+ ip.ActRecDiferencias + ip.ActAdicDiferencia  +
                                 ip.RezDiferencias+ ip.RezDiferenciasINP+ ip.RezRecDiferencias  + ip.RezAdicDiferencias ;
                    ip.DescuentoGral = ip.ActDescDiferencias + ip.ActDescRecDiferencias + ip.ActDescAdicDiferencia + 
                                       ip.RezDescDiferencias + ip.RezDescRecDiferencias + ip.RezDescAdicDiferencias ;
                    ip.Importe = ip.Importe - ip.DescuentoGral;
                    ip.Estado = EstadoResumido(ip, 0, 0000, bFinal, eFinal);
                    return ip;
                    #endregion
                }
                //se guarda el id del descuento de JYP
                if (descJyP > 0)                
                    ip.idDescuentoJYP = descJyP;
                
                //se guarda el id del descuento de rezago asignado
                if (descRez > 0)               
                    ip.idDescuentoRez = descRez;
               
                if (descGral > 0)
                    ip.idDescuentoGral = descGral;

                ip.Estado = EstadoResumido(ip, bInicial, eInicial, bFinal, eFinal);
                ip.Anual = impAnual;
            }
            catch (Exception ex)
            {
                new Utileria().logError("Saldos.CalculaCobro.Exception", ex, " Parámetros--- " + idClavePredial.ToString() + "," + soloDiferencia.ToString() + "," + bInicial.ToString() + "," + eInicial.ToString() + "," + bFinal.ToString() + "," + eFinal.ToString() + "," + descJyP.ToString() + " " + descRez);
                ip.mensaje = MensajesInterfaz.CalculoImpuesto;
            }
            return ip;
        }//CalculaCobro

        private ImpuestoEdo EstadoResumido(Impuesto i, int bInicial, int eInicial, int bFinal, int eFinal)
        {
            ImpuestoEdo e = new ImpuestoEdo();

            e.AntImpuesto = i.AntImpuesto;
            e.DescAntImpuesto = i.AntDescImpuesto;
            e.AntAdicional = i.AntAdicional;
            e.Impuesto = i.ActImpuesto;
            e.DescImpuesto = i.ActDescImpuesto;            
            e.Diferencias = (i.ActDiferencias + i.RezDiferencias);
            e.DescDiferencias = i.ActDescDiferencias + i.RezDescDiferencias;
            e.RecDiferencias = (i.ActRecDiferencias + i.RezRecDiferencias);
            e.DescRecDiferencias = i.ActDescRecDiferencias + i.RezDescRecDiferencias;
            e.Rezagos = i.Rezagos;
            e.DescRezagos = i.RezDescRezagos;
            e.Recargos = (i.RezRecargo + i.ActRecargo);
            e.DescRecargos = i.ActDescRecargo + i.RezDescRecargos;
            e.Adicional = (i.ActAdicional + i.RezAdicional+i.ActAdicDiferencia+i.RezAdicDiferencias);
            e.Multas = i.Multa;
            e.DescMultas = i.MultaDesc;
            e.Ejecucion = i.Ejecucion;
            e.DescEjecucion = i.EjecucionDesc;
            e.Honorarios = i.Honorarios;
            e.DescHonorarios = i.HonorariosDesc;
            e.PeriodoGral = bInicial.ToString() + " " + eInicial.ToString() + " - " + bFinal.ToString() + " " + eFinal.ToString(); ;
            e.Descuentos = i.DescuentoGral;
            e.Importe = i.Importe;
            e.ImporteNeto =  i.Importe + i.DescuentoGral;

            return e;
        }

        #endregion

        #region Servicios Municipales

        public decimal DapBimestral(decimal salMin, decimal valor, int tipoCalculo)
        {
            decimal resultado = 0;

            cTipoCobro tipo = new cTipoCobroBL().GetByConstraint(tipoCalculo);
            if (tipo == null) return -1;

            switch (tipo.Codigo)
            {
                case "S":
                    resultado = (salMin * valor) / 6;
                    break;
                case "P":
                case "T":
                    resultado = (salMin * 365) * valor / 6;
                    break;
                case "I":
                    resultado = valor / 6;
                    break;
            }
            return resultado;
        }

        public decimal LimpiezaBimestral(decimal salMin, decimal valor, int tipoCalculo)
        {
            decimal resultado = 0;
            cTipoCobro tipo = new cTipoCobroBL().GetByConstraint(tipoCalculo);
            if (tipo == null) return -1;

            switch (tipo.Codigo)
            {
                case "S":
                    resultado = (salMin * valor) / 6;
                    break;
                case "P":
                case "T":
                    resultado = (salMin * 365) * valor / 6;
                    break;
                case "I":
                    resultado = valor / 6;
                    break;
            }
            return resultado;
        }

        public decimal RecoleccionBimestral(decimal salMin, decimal valor, int tipoCalculo)
        {
            decimal resultado = 0;
            cTipoCobro tipo = new cTipoCobroBL().GetByConstraint(tipoCalculo);
            if (tipo == null) return -1;

            switch (tipo.Codigo)
            {
                case "S":
                    resultado = (salMin * valor) / 6;
                    break;
                case "P":
                case "T":
                    resultado = (salMin * 365) * valor / 6;
                    break;
                case "I":
                    resultado = valor / 6;
                    break;
            }
            return resultado;
        }

        public decimal InfraestructuraBimestral(decimal salMin, decimal valor, int tipoCalculo)
        {
            decimal resultado = 0;
            cTipoCobro tipo = new cTipoCobroBL().GetByConstraint(tipoCalculo);
            if (tipo == null) return -1;

            switch (tipo.Codigo)
            {
                case "S":
                    resultado = (salMin * valor) / 6;
                    break;
                case "P":
                case "T":
                    resultado = (salMin * 365) * valor / 6;
                    break;
                case "I":
                    resultado = valor / 6;
                    break;
            }
            return resultado;
        }

        public List<ServicioBimestral> ServicioBimestral(int idClavePredial, int bInicial, int eInicial, int bFinal, int eFinal, decimal zona,int tipoPredio, string cobroDapSinConst, double supConstruccion)
        {
            int restaBim = 0;
            List<ServicioBimestral> serviciosB = new List<ServicioBimestral>();
            if (bInicial == 0 && bFinal == 0 && eInicial == 0 && eFinal == 0) return serviciosB;
            DateTime fPago = DateTime.Today;
            cSalarioMinimo sm = new cSalarioMinimo();
            cTarifaZona tz = new cTarifaZona();
            cTarifaLimpieza tl = new cTarifaLimpieza();
            cTarifaRecoleccion tr = new cTarifaRecoleccion();
            cTarifaDap td = new cTarifaDap();
            ServicioBimestral sb = new ServicioBimestral();
            MensajesInterfaz msg = new MensajesInterfaz();

            string cobroServiciosCaja = new cParametroSistemaBL().GetValorByClave("COBROSERVICIOSCAJAS").ToString();
            if (cobroServiciosCaja == "NO")
            {
                return serviciosB;
            }


            try
            {
                int bimestres = CuentaBimestre(bInicial, eInicial, bFinal, eFinal, ref msg);
                if (msg > 0)
                {
                    sb.mensaje = msg;
                    sb.TextError = new Utileria().GetDescription(sb.mensaje) + eInicial.ToString() + " " + eFinal.ToString();
                    serviciosB.Add(sb);
                    return serviciosB;
                }

                int bimActual = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(fPago.Month / 2)));

                //decimal indiceActual = new cIndicePrecioBL().ValorIndiceActual();
                if (indiceActual == 0)
                {
                    indiceActual = new cIndicePrecioBL().ValorIndiceActualSinCambio();
                    if (indiceActual == 0)
                    {
                        sb.mensaje = MensajesInterfaz.INPactual;
                        sb.TextError = new Utileria().GetDescription(sb.mensaje) + eInicial.ToString();
                        serviciosB.Add(sb);
                        return serviciosB;
                    }
                }
                string AplicaActualizacion = new cParametroSistemaBL().GetValorByClave("AplicarActualizacionINP").ToString();
                if (AplicaActualizacion == "")
                {
                    sb.mensaje = MensajesInterfaz.INPactual;
                    sb.TextError = new Utileria().GetDescription(sb.mensaje) + eInicial.ToString();
                    serviciosB.Add(sb);
                    return serviciosB;
                }

                //decimal porcAdicional = new cParametroCobroBL().GetByClave("PorcentajeAdicional");
                if (porcAdicional == -1)
                {
                    sb.mensaje = MensajesInterfaz.PorcAdicional;
                    serviciosB.Add(sb);
                    return serviciosB;
                }
                //porcAdicional = porcAdicional / 100;
                restaBim = bimestres;

                for (int i = 1; i <= bimestres; i++)
                {
                    if (bInicial == 7 || i == 1)
                    {
                        #region cambio de año
                        if (bInicial == 7)
                        {
                            bInicial = 1;
                            eInicial++;
                        }
                        sm = salMin.FirstOrDefault(o => o.Ejercicio == eInicial);
                        tz = tZona.FirstOrDefault(o => o.Ejercicio == eInicial && o.Zona == zona);
                        tl = tLimpieza.FirstOrDefault(o => o.Ejercicio == eInicial);
                        tr = tRecoleccion.FirstOrDefault(o => o.Ejercicio == eInicial);
                        td = tDap.FirstOrDefault(o => o.Ejercicio == eInicial);

                        if (sm == null)
                        {
                            serviciosB = new List<ServicioBimestral>();
                            sb.mensaje = MensajesInterfaz.SalarioMinimo;
                            sb.TextError = new Utileria().GetDescription(sb.mensaje) + eInicial.ToString();
                            serviciosB.Add(sb);
                            return serviciosB;
                        }

                        if (tz == null)
                        {
                            serviciosB = new List<ServicioBimestral>();
                            sb.mensaje = MensajesInterfaz.TarifaZona;
                            sb.TextError = new Utileria().GetDescription(sb.mensaje) + eInicial.ToString();
                            serviciosB.Add(sb);
                            return serviciosB;
                        }

                        if (tl == null)
                        {
                            serviciosB = new List<ServicioBimestral>();
                            sb.mensaje = MensajesInterfaz.TarifaLimpieza;
                            sb.TextError = new Utileria().GetDescription(sb.mensaje) + eInicial.ToString();
                            serviciosB.Add(sb);
                            return serviciosB;
                        }

                        if (tr == null)
                        {
                            serviciosB = new List<ServicioBimestral>();
                            sb.mensaje = MensajesInterfaz.TarifaRecoleccion;
                            sb.TextError = new Utileria().GetDescription(sb.mensaje) + eInicial.ToString();
                            serviciosB.Add(sb);
                            return serviciosB;
                        }

                        if (td == null)
                        {
                            serviciosB = new List<ServicioBimestral>();
                            sb.mensaje = MensajesInterfaz.TarifaDAP;
                            sb.TextError = new Utileria().GetDescription(sb.mensaje) + eInicial.ToString();
                            serviciosB.Add(sb);
                            return serviciosB;
                        }
                        #endregion  cambio de año
                    }
                    sb = new ServicioBimestral();

                    sb.Ejercicio = eInicial;
                    
                    if (tz.Tarifa > 0) sb.Infraestructura = InfraestructuraBimestral(sm.Importe, tz.Tarifa, tz.IdTipoCobro);
                   
                    if (tl.Tarifa > 0) sb.Limpieza = LimpiezaBimestral(sm.Importe, tl.Tarifa, tl.TipoCobro); // tl.Tarifa, tl.TipoCobro); 
                    
                    if (tr.Tarifa > 0) sb.Recoleccion = RecoleccionBimestral(sm.Importe, tr.Tarifa, tr.IdTipoCobro);

                    sb.Dap = 0;
                    if ((tipoPredio == 3) || (cobroDapSinConst == "SI" && supConstruccion > 0))
                        if (td.Tarifa > 0) sb.Dap = DapBimestral(sm.Importe, td.Tarifa, td.TipoCobro);

                    sb.Adicional = (sb.Infraestructura + sb.Limpieza + sb.Recoleccion + sb.Dap) * porcAdicional;

                    if (Convert.ToInt32(eInicial.ToString() + bInicial.ToString()) <= Convert.ToInt32(fPago.Year.ToString() + bimActual.ToString()))
                    {
                        sb.PorcentajeRecargo = new cTarifaRecargoBL().PorcentajeRecargo(bInicial, eInicial, bimActual, fPago.Year,ref listRecargos);
                        if (sb.PorcentajeRecargo == -1)
                        {
                            serviciosB = new List<ServicioBimestral>();
                            sb.mensaje = MensajesInterfaz.TarifaRecargos;
                            sb.TextError = new Utileria().GetDescription(sb.mensaje) + eInicial.ToString();
                            serviciosB.Add(sb);
                            return serviciosB;
                        }

                        if (AplicaActualizacion == "SI")
                        {
                            //decimal indiceAnt = new cIndicePrecioBL().ValorIndiceAnterior(eInicial, bInicial);
                            decimal indiceAnt = new cIndicePrecioBL().ValorIndiceAnterior(eInicial, bInicial,ref listIndice);
                            //listIndiceAnt
                            if (indiceAnt == 0)
                            {
                                serviciosB = new List<ServicioBimestral>();
                                sb.mensaje = MensajesInterfaz.INPactual;
                                sb.TextError = new Utileria().GetDescription(sb.mensaje) + eInicial.ToString();
                                serviciosB.Add(sb);
                                return serviciosB;
                            }
                            sb.IndAnterior = indiceAnt;
                            sb.IndActual = indiceActual;
                            sb.PorcentajeINP = (indiceActual / indiceAnt) - 1;
                            if (indiceAnt > indiceActual) sb.PorcentajeINP = 0;

                            sb.InfraestructuraIPN = (sb.Infraestructura * sb.PorcentajeINP);
                            sb.LimpiezaIPN = sb.Limpieza * sb.PorcentajeINP;
                            sb.RecoleccionIPN = sb.Recoleccion * sb.PorcentajeINP;
                            sb.DapIPN = sb.Dap * sb.PorcentajeRecargo;

                            sb.AdicionalIPN = ((sb.InfraestructuraIPN) +
                                (sb.RecoleccionIPN) + (sb.LimpiezaIPN) + (sb.DapIPN)) * porcAdicional;
                            
                            //sb.Recargo = ((sb.Infraestructura + sb.InfraestructuraIPN) +
                            //    (sb.Recoleccion + sb.RecoleccionIPN) + (sb.Limpieza + sb.LimpiezaIPN) + (sb.Dap + sb.DapIPN)) * sb.PorcentajeRecargo;
                            sb.InfraestructuraRec = (sb.Infraestructura + sb.InfraestructuraIPN) * sb.PorcentajeRecargo;
                            sb.RecoleccionRec = (sb.Recoleccion + sb.RecoleccionIPN) * sb.PorcentajeRecargo;
                            sb.LimpiezaRec = (sb.Limpieza + sb.LimpiezaIPN) * sb.PorcentajeRecargo;
                            sb.DapRec = (sb.Dap + sb.DapIPN) * sb.PorcentajeRecargo;
                        }
                        else
                        {
                            //sb.Recargo = (sb.Infraestructura + sb.Limpieza + sb.Recoleccion + sb.Adicional) * sb.PorcentajeRecargo;
                            sb.InfraestructuraRec = sb.Infraestructura  * sb.PorcentajeRecargo;
                            sb.RecoleccionRec = sb.Recoleccion  * sb.PorcentajeRecargo;
                            sb.LimpiezaRec = sb.Limpieza  * sb.PorcentajeRecargo;
                            sb.DapRec = sb.Dap  * sb.PorcentajeRecargo;
                            sb.LimpiezaIPN = 0;
                            sb.RecoleccionIPN = 0;
                            sb.DapIPN = 0;
                            sb.AdicionalIPN = 0;
                            sb.InfraestructuraIPN = 0;
                        }
                    }

                    sb.NoBimestre = restaBim;
                    bInicial++;

                    serviciosB.Add(sb);
                    restaBim--;
                }
            }

            catch (Exception ex)
            {
                new Utileria().logError("Saldos.CalculaImpuesto.Exception", ex);
            }
            return serviciosB;
        }

        public Servicio CalculaCobroSM(int idClavePredial, int bInicial, int eInicial, int bFinal, int eFinal, int descJyP, int descRez, string tramite, int zona, int IdTipoPredio, double supConstruccion)
        {
            Servicio sm = new Servicio();
            int descGral = 0;
            int ejercicioAdicional = Convert.ToInt16(new cParametroSistemaBL().GetValorByClave("EJERCICIO COBRO ADICIONAL").ToString());

            //validar si hay cobro de servicios en caja
            string cobroServiciosCaja = new cParametroSistemaBL().GetValorByClave("COBROSERVICIOSCAJAS").ToString();
            if (cobroServiciosCaja == "NO")
            {
                sm = InicializaSM(sm);
                return sm;
            }



            //Si el año esta vacio se regresa
            if (((bInicial == 0 && bFinal == 0) || (eInicial == 0 && eFinal == 0)) || (Convert.ToInt32(eInicial.ToString() + bInicial.ToString()) > Convert.ToInt32(eFinal.ToString() + bFinal.ToString())))
            {
                sm = InicializaSM(sm);
                return sm;
            }
            try
            {
                DateTime fPago = DateTime.Today;
                Periodo per = new Periodo();

                if (tramite != "CalculaServicios")//no entra cuando viene de requerimientos
                {
                    per = ValidaPeriodoPago(bInicial, eInicial, bFinal, eFinal, tramite);
                    if (per.mensaje != 0)
                    {
                        sm.mensaje = MensajesInterfaz.EjercicioIncorrecto;
                        sm.TextError = "";
                        return sm;
                    }
                    bInicial = per.bInicial;
                    eInicial = per.eInicial;
                    bFinal = per.bFinal;
                    eFinal = per.eFinal;
                }
                //decimal porcAdicional = new cParametroCobroBL().GetByClave("PorcentajeAdicional") / 100;
                if (porcAdicional == -1)
                {
                    sm.mensaje = MensajesInterfaz.PorcAdicional;
                    sm.TextError = "";
                    return sm;
                }

                //cPredio pred = new cPredioBL().GetByConstraint(idClavePredial);
                int bimActual = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(fPago.Month / 2)));
                List<ServicioBimestral> serBim = new List<ServicioBimestral>();

                //descuento global
                Descuento descAplicar = new Descuento();
                if (tramite == "CalculaServicios")// entra cuando viene del estado de cuenta
                    descAplicar = ValidaDescuentoClave(idClavePredial, "SM", descJyP, descRez, ref descGral);

                #region Servicios Municipales

                //Calcula Servicio Bimestral
                serBim = ServicioBimestral(idClavePredial, bInicial, eInicial, bFinal, eFinal, zona, IdTipoPredio, cobroDapSinConst, supConstruccion);
                if (serBim.Count == 1 && serBim[0].TextError != null)
                {
                    sm.mensaje = serBim[0].mensaje;
                    sm.TextError = serBim[0].TextError;
                    return sm;
                }
                #region  ANTICIPADO
                sm.PerAnticipado = DevuelvePeriodoPago(bInicial, eInicial, bFinal, eFinal, "ANTICIPADO");
                if (sm.PerAnticipado != null)
                {
                    sm.AntBimestre = (from i in serBim where i.Ejercicio == fPago.Year select i.Infraestructura).Count();
                    //INFRAESTRUCTURA   
                    sm.AntInfraestructura = Utileria.Redondeo((from i in serBim where i.Ejercicio >= fPago.Year + 1 select i.Infraestructura).Sum());
                    if (descAplicar != null && descAplicar.AnticipadoInfraestructura > 0 && sm.PerAnticipado.bInicial == 1 && sm.PerAnticipado.bFinal == 6)
                        sm.AntDescInfraestructura = Utileria.Redondeo(sm.AntInfraestructura * (descAplicar.AnticipadoInfraestructura / 100));
                    sm.AntAdicInfraestructura = Utileria.Redondeo(sm.AntInfraestructura * porcAdicional);
                    if (descAplicar != null && descAplicar.AnticipadoAdicional > 0 && sm.PerAnticipado.bInicial == 1 && sm.PerAnticipado.bFinal == 6)
                        sm.AntDescAdicInfraestructura = Utileria.Redondeo(sm.AntAdicInfraestructura * (descAplicar.AnticipadoAdicional / 100));

                    //RECOLECCION
                    sm.AntRecoleccion = Utileria.Redondeo((from i in serBim where i.Ejercicio >= fPago.Year + 1 select i.Recoleccion).Sum());
                    if (descAplicar != null && descAplicar.AnticipadoRecoleccion > 0 && sm.PerAnticipado.bInicial == 1 && sm.PerAnticipado.bFinal == 6)
                        sm.AntDescRecoleccion = Utileria.Redondeo(sm.AntRecoleccion * (descAplicar.AnticipadoRecoleccion / 100));
                    sm.AntAdicRecoleccion = Utileria.Redondeo(sm.AntRecoleccion * porcAdicional);
                    if (descAplicar != null && descAplicar.AnticipadoAdicional > 0 && sm.PerAnticipado.bInicial == 1 && sm.PerAnticipado.bFinal == 6)
                        sm.AntDescAdicRecoleccion = Utileria.Redondeo(sm.AntAdicRecoleccion * (descAplicar.AnticipadoAdicional / 100));


                    //LIMPIEZA
                    sm.AntLimpieza = Utileria.Redondeo((from i in serBim where i.Ejercicio >= fPago.Year + 1 select i.Limpieza).Sum());
                    if (descAplicar != null && descAplicar.AnticipadoLimpieza > 0 && sm.PerAnticipado.bInicial == 1 && sm.PerAnticipado.bFinal == 6)
                        sm.AntDescLimpieza = Utileria.Redondeo(sm.AntLimpieza * (descAplicar.AnticipadoLimpieza / 100));
                    sm.AntAdicLimpieza = Utileria.Redondeo(sm.AntLimpieza * porcAdicional);
                    if (descAplicar != null && descAplicar.AnticipadoAdicional > 0 && sm.PerAnticipado.bInicial == 1 && sm.PerAnticipado.bFinal == 6)
                        sm.AntDescAdicLimpieza = Utileria.Redondeo(sm.AntAdicLimpieza * (descAplicar.AnticipadoAdicional / 100));

                    //DAP
                    if (IdTipoPredio == 3)
                    {
                        sm.AntDap = Utileria.Redondeo((from i in serBim where i.Ejercicio >= fPago.Year + 1 select i.Dap).Sum());
                        if (descAplicar != null && descAplicar.AnticipadoDap > 0 && sm.PerAnticipado.bInicial == 1 && sm.PerAnticipado.bFinal == 6)
                            sm.AntDescDap = Utileria.Redondeo(sm.AntDap * (descAplicar.AnticipadoDap / 100));
                        sm.AntAdicDap = Utileria.Redondeo(sm.AntDap * porcAdicional);
                        if (descAplicar != null && descAplicar.AnticipadoAdicional > 0 && sm.PerAnticipado.bInicial == 1 && sm.PerAnticipado.bFinal == 6)
                            sm.AntDescAdicDap = Utileria.Redondeo(sm.AntAdicDap * (descAplicar.AnticipadoAdicional / 100));
                    }
                    else
                        sm.AntDap = 0;
                    //sm.AntAdicional = Utileria.Redondeo((sm.AntInfraestructura + sm.AntRecoleccion + sm.AntLimpieza + sm.AntDap) * porcAdicional);
                    //ADICIONAL
                    //sm.AntAdicional = Utileria.Redondeo((from i in serBim where i.Ejercicio >= fPago.Year + 1 select i.Adicional).Sum());
                    //if (descAplicar != null && descAplicar.AnticipadoAdicional > 0 && sm.PerAnticipado.bInicial == 1 && sm.PerAnticipado.bFinal == 6)
                    //    sm.AntDescAdicional = Utileria.Redondeo(sm.AntAdicional * (descAplicar.AnticipadoAdicional / 100));
                }
                #endregion anticipado

                #region ACTUAL
                sm.PerActual = DevuelvePeriodoPago(bInicial, eInicial, bFinal, eFinal, "ACTUAL");
                if (sm.PerActual != null)
                {
                    sm.ActBimestre = (from i in serBim where i.Ejercicio == fPago.Year select i.Infraestructura).Count();
                    //INFRAESTRUCTRA 
                    sm.ActInfraestructura = Utileria.Redondeo((from i in serBim where i.Ejercicio == fPago.Year select i.Infraestructura).Sum()) +
                    Utileria.Redondeo((from i in serBim where i.Ejercicio < fPago.Year select i.InfraestructuraIPN).Sum());
                    if (descAplicar != null && descAplicar.Infraestructura > 0 && sm.PerActual.bInicial == 1 && sm.PerActual.bFinal == 6)
                        sm.ActDescInfraestructura = Utileria.Redondeo(sm.ActInfraestructura * (descAplicar.Infraestructura / 100));
                    sm.ActRecInfraestructura = Utileria.Redondeo((from i in serBim where i.Ejercicio == fPago.Year select i.InfraestructuraRec).Sum());
                    if (descAplicar != null && descAplicar.ActualRecargo > 0)  //Dap
                        sm.ActDescRecInfraestructura = Utileria.Redondeo(sm.ActRecInfraestructura * (descAplicar.ActualRecargo / 100));
                    sm.ActAdicInfraestructura = Utileria.Redondeo(sm.ActInfraestructura * porcAdicional);
                    if (descAplicar != null && descAplicar.ActualAdicional > 0 && sm.PerActual.bInicial == 1 && sm.PerActual.bFinal == 6)  //Adicional
                        sm.ActDescAdicInfraestructura = Utileria.Redondeo(sm.ActAdicInfraestructura * (descAplicar.ActualAdicional / 100));
                    sm.ActINP = 0;

                    //RECOLECCION
                    sm.ActRecoleccion = Utileria.Redondeo((from i in serBim where i.Ejercicio == fPago.Year select i.Recoleccion).Sum()) +
                     Utileria.Redondeo((from i in serBim where i.Ejercicio == fPago.Year select i.RecoleccionIPN).Sum());
                    if (descAplicar != null && descAplicar.ActualRecoleccion > 0 && sm.PerActual.bInicial == 1 && sm.PerActual.bFinal == 6)
                        sm.ActDescRecoleccion = Utileria.Redondeo(sm.ActRecoleccion * (descAplicar.ActualRecoleccion / 100));

                    sm.ActRecRecoleccion = Utileria.Redondeo((from i in serBim where i.Ejercicio == fPago.Year select i.RecoleccionRec).Sum());
                    if (descAplicar != null && descAplicar.ActualRecargo > 0)  //Dap
                        sm.ActDescRecRecoleccion = Utileria.Redondeo(sm.ActRecRecoleccion * (descAplicar.ActualRecargo / 100));

                    sm.ActAdicRecoleccion = Utileria.Redondeo(sm.ActRecoleccion * porcAdicional);
                    if (descAplicar != null && descAplicar.ActualAdicional > 0 && sm.PerActual.bInicial == 1 && sm.PerActual.bFinal == 6)  //Adicional
                        sm.ActDescAdicRecoleccion = Utileria.Redondeo(sm.ActAdicRecoleccion * (descAplicar.ActualAdicional / 100));

                    //LIMPIEZA
                    sm.ActLimpieza = Utileria.Redondeo((from i in serBim where i.Ejercicio == fPago.Year select i.Limpieza).Sum()) +
                     Utileria.Redondeo((from i in serBim where i.Ejercicio < fPago.Year select i.LimpiezaIPN).Sum());
                    if (descAplicar != null && descAplicar.ActualLImpieza > 0 && sm.PerActual.bInicial == 1 && sm.PerActual.bFinal == 6)
                        sm.ActDescLimpieza = Utileria.Redondeo(sm.ActLimpieza * (descAplicar.ActualLImpieza / 100));

                    sm.ActRecLimpieza = Utileria.Redondeo((from i in serBim where i.Ejercicio == fPago.Year select i.LimpiezaRec).Sum());
                    if (descAplicar != null && descAplicar.ActualRecargo > 0)  //Dap
                        sm.ActDescRecLimpieza = Utileria.Redondeo(sm.ActRecLimpieza * (descAplicar.ActualRecargo / 100));

                    sm.ActAdicLimpieza = Utileria.Redondeo(sm.ActLimpieza * porcAdicional);
                    if (descAplicar != null && descAplicar.ActualAdicional > 0 && sm.PerActual.bInicial == 1 && sm.PerActual.bFinal == 6)  //Adicional
                        sm.ActDescAdicLimpieza = Utileria.Redondeo(sm.ActAdicLimpieza * (descAplicar.ActualAdicional / 100));

                    //DAP
                    //if ( (IdTipoPredio == 3) || (cobroDapSinConst == "SI" && mtsConstrucion > 0 ) )
                    //{
                    sm.ActDap = Utileria.Redondeo((from i in serBim where i.Ejercicio == fPago.Year select i.Dap).Sum()) +
                    Utileria.Redondeo((from i in serBim where i.Ejercicio == fPago.Year select i.DapIPN).Sum());
                    if (descAplicar != null && descAplicar.ActualDap > 0 && sm.PerActual.bInicial == 1 && sm.PerActual.bFinal == 6)  //Dap
                        sm.ActDescDap = Utileria.Redondeo(sm.ActDap * (descAplicar.ActualDap / 100));

                    sm.ActRecDap = Utileria.Redondeo((from i in serBim where i.Ejercicio == fPago.Year select i.DapRec).Sum());
                    if (descAplicar != null && descAplicar.ActualRecargo > 0)  //Dap
                        sm.ActDescRecDap = Utileria.Redondeo(sm.ActRecDap * (descAplicar.ActualRecargo / 100));

                    sm.ActAdicDap = Utileria.Redondeo(sm.ActDap * porcAdicional);
                    if (descAplicar != null && descAplicar.ActualAdicional > 0 && sm.PerActual.bInicial == 1 && sm.PerActual.bFinal == 6)  //Adicional
                        sm.ActDescAdicLimpieza = Utileria.Redondeo(sm.ActAdicDap * (descAplicar.ActualAdicional / 100));
                                        
                    #endregion actual
                    
                }
                #region REZAGO
                sm.PerRezago = DevuelvePeriodoPago(bInicial, eInicial, bFinal, eFinal, "REZAGO");
                if (sm.PerRezago != null)
                {
                    sm.RezBimestre = (from i in serBim where i.Ejercicio < fPago.Year select i.Infraestructura).Count();
                    sm.Rezagos = Utileria.Redondeo((from i in serBim where i.Ejercicio < fPago.Year select i.Infraestructura).Sum() +
                                 (from i in serBim where i.Ejercicio < fPago.Year select i.Recoleccion).Sum() +
                                 (from i in serBim where i.Ejercicio < fPago.Year select i.Limpieza).Sum() +
                                 (from i in serBim where i.Ejercicio < fPago.Year select i.Dap).Sum());
                    sm.RezINP = Utileria.Redondeo((from i in serBim where i.Ejercicio < fPago.Year select i.InfraestructuraIPN).Sum() +
                                 (from i in serBim where i.Ejercicio < fPago.Year select i.RecoleccionIPN).Sum() +
                                 (from i in serBim where i.Ejercicio < fPago.Year select i.LimpiezaIPN).Sum() +
                                 (from i in serBim where i.Ejercicio < fPago.Year select i.DapIPN).Sum());
                    sm.Rezagos = sm.Rezagos + sm.RezINP;
                    if (descAplicar != null && descAplicar.Rezago > 0) //Rezago
                        sm.RezDescRezagos = Utileria.Redondeo(sm.Rezagos * (descAplicar.Rezago / 100));
                    
                    sm.RezRecargo = Utileria.Redondeo((from i in serBim where i.Ejercicio < fPago.Year select i.InfraestructuraRec).Sum() +
                                  (from i in serBim where i.Ejercicio < fPago.Year select i.RecoleccionRec).Sum() +
                                  (from i in serBim where i.Ejercicio < fPago.Year select i.LimpiezaRec).Sum() +
                                  (from i in serBim where i.Ejercicio < fPago.Year select i.DapRec).Sum());

                    if (descAplicar != null && descAplicar.RezagoRecargo > 0) //Recargo
                        sm.RezDescRecargos = Utileria.Redondeo(sm.RezRecargo * (descAplicar.RezagoRecargo / 100));

                    sm.RezAdicional = Utileria.Redondeo((from i in serBim where i.Ejercicio < fPago.Year select i.Adicional).Sum()) +
                     Utileria.Redondeo((from i in serBim where i.Ejercicio < fPago.Year select i.AdicionalIPN).Sum());
                    //validar el año de cobro adicionales                        
                    if (( sm.PerRezago.eInicial <= ejercicioAdicional) && ejercicioAdicional > 0)
                    {
                        decimal impuestoAdicional = (from i in serBim where i.Ejercicio <= ejercicioAdicional select i.Infraestructura).Sum() +
                                 (from i in serBim where i.Ejercicio <= ejercicioAdicional select i.Recoleccion).Sum() +
                                 (from i in serBim where i.Ejercicio <= ejercicioAdicional select i.Limpieza).Sum() +
                                 (from i in serBim where i.Ejercicio <= ejercicioAdicional select i.Dap).Sum() +
                                (from i in serBim where i.Ejercicio  <= ejercicioAdicional select i.InfraestructuraIPN).Sum() +
                                 (from i in serBim where i.Ejercicio <= ejercicioAdicional select i.RecoleccionIPN).Sum() +
                                 (from i in serBim where i.Ejercicio <= ejercicioAdicional select i.LimpiezaIPN).Sum() +
                                 (from i in serBim where i.Ejercicio <= ejercicioAdicional select i.DapIPN).Sum();
                        sm.RezAdicional = Utileria.Redondeo((impuestoAdicional * Convert.ToDecimal(0.25)));
                    }
                    if (descAplicar != null && descAplicar.RezagoAdicional > 0) //Recargo
                        sm.RezDescAdicional = Utileria.Redondeo(sm.RezAdicional * (descAplicar.RezagoAdicional / 100));
                }
                #endregion REZAGOS

                #region REQUERIMIENTOS

                if (tramite != "Rezago")
                {
                    //Obtenemos los apercibimientos si es que los tiene el predio
                    tRequerimiento q = new tRequerimientoBL().obtenerRequerimientoporIdPredio(idClavePredial, "SM");
                    if (q != null)
                    {
                        sm.Multa = Convert.ToDecimal(q.Multa);
                        sm.MultaDesc = Utileria.Redondeo(sm.Multa * (descAplicar.Multas / 100));
                        sm.Ejecucion = Convert.ToDecimal(q.Ejecucion);
                        sm.IdRequerimiento = q.Id;
                    }
                    else
                    {
                        //incluimos las cuotas fijas pra estos conceptos
                        sm.Multa = sm.Multa;
                        sm.MultaDesc = Utileria.Redondeo(sm.Multa * (descAplicar.Multas / 100));
                        sm.Ejecucion = sm.Ejecucion;
                        sm.EjecucionDesc = Utileria.Redondeo(sm.Ejecucion * (descAplicar.Ejecucion / 100));
                        sm.IdRequerimiento = 0;
                    }
                }
           
                #endregion REQUERIMIENTOS

                sm.Importe = sm.AntInfraestructura + sm.AntAdicInfraestructura + sm.AntRecoleccion + sm.AntAdicRecoleccion + 
                             sm.AntLimpieza + sm.AntAdicLimpieza + sm.AntDap + sm.AntAdicDap + 
                             sm.ActInfraestructura + sm.ActRecInfraestructura + sm.ActAdicInfraestructura +
                             sm.ActRecoleccion + sm.ActRecRecoleccion + sm.ActAdicRecoleccion +
                             sm.ActLimpieza + sm.ActRecLimpieza + sm.ActAdicLimpieza +
                             sm.ActDap + sm.ActRecDap + sm.ActAdicDap +
                             sm.Rezagos + sm.RezRecargo + sm.RezAdicional +
                             sm.Multa + sm.Ejecucion + sm.Honorarios;

                sm.DescuentoGral = sm.AntDescInfraestructura + sm.AntDescRecoleccion + sm.AntDescLimpieza + sm.AntDescDap + 
                                   sm.AntDescAdicInfraestructura + sm.AntDescAdicRecoleccion + sm.AntDescAdicLimpieza + sm.AntDescAdicDap +
                                   sm.ActDescInfraestructura + sm.ActDescRecInfraestructura + sm.ActDescAdicInfraestructura + 
                                   sm.ActDescRecoleccion + sm.ActDescRecRecoleccion + sm.ActDescAdicRecoleccion +
                                   sm.ActDescLimpieza + sm.ActDescRecLimpieza + sm.ActDescAdicLimpieza +
                                   sm.ActDescDap + sm.ActDescRecDap + sm.ActDescAdicDap +                                   
                                   sm.RezDescRezagos + sm.RezDescRecargos + sm.RezDescAdicional +
                                   sm.MultaDesc+ sm.HonorariosDesc;
                sm.PeriodoGral = bInicial.ToString() + " " + eInicial.ToString() + " - " + bFinal.ToString() + " " + eFinal.ToString();

                sm.Importe = sm.Importe - sm.DescuentoGral;
                
                #endregion IMPUESTO

                sm.Estado = EstadoResumidoSM(sm);
            }
            catch (Exception ex)
            {
                new Utileria().logError("Saldos.CalculaCobroSM.Exception", ex, " Parametros--- " + idClavePredial.ToString() + "," + bInicial.ToString() + "," + eInicial.ToString() + "," + bFinal.ToString() + "," + eFinal.ToString() + "," + descJyP.ToString() + " " + descRez.ToString());
            }
            return sm;

        }//CalculaCobroSM

        private ServicioEdo EstadoResumidoSM(Servicio sm)
        {
            ServicioEdo e = new ServicioEdo();
            e.AntInfraestructura = sm.AntInfraestructura;
            e.AntAdicional = sm.AntAdicInfraestructura +sm.AntAdicRecoleccion+sm.AntAdicLimpieza+sm.AntAdicDap;
            e.Infraestructura = sm.ActInfraestructura;
            e.Recoleccion = (sm.AntRecoleccion + sm.ActRecoleccion);
            e.Limpieza = (sm.AntLimpieza + sm.ActLimpieza);
            e.Dap = (sm.AntDap + sm.ActDap);           
            e.Recargo = (sm.ActRecInfraestructura + sm.ActRecRecoleccion +sm.ActRecLimpieza + sm.ActRecDap + sm.RezRecargo);
            e.Rezagos = sm.Rezagos;
            e.Adicional = (sm.ActAdicInfraestructura +sm.ActAdicRecoleccion+sm.ActAdicLimpieza+sm.ActAdicDap + sm.RezAdicional);
            e.Ejecucion = sm.Ejecucion;
            e.Multa = sm.Multa;
            e.Idrequerimiento = sm.IdRequerimiento;

            e.PeriodoGral = sm.PeriodoGral;
            e.Descuentos = sm.DescuentoGral;
            e.Importe = sm.Importe;
            return e;
        }
        #endregion  servicios

        #region Generales

        public ConceptoGral ConceptosP(Impuesto i, string claveCatastral, string complementoConcepto)
        {
            ConceptosRec c = new ConceptosRec();
            List<ConceptosRec> r = new List<ConceptosRec>();
            ConceptoGral cGral = new ConceptoGral();
            string descDiferencia = string.Empty;


            string sMesa = new cParametroSistemaBL().GetValorByClave("MesaIP");
            if (sMesa == "")//validar si la mesa esta definida en la tabla cParametroSistema
            {
                cGral.Error = new Utileria().GetDescription(MensajesInterfaz.DefinirMesaIP);
                cGral.conceptos = null;
                return cGral;
            }
            int mesa = new cMesaBL().GetByNombre(sMesa);
            if (sMesa == "") //validar si la mesa esta en la tabla cMesa
            {
                cGral.Error = new Utileria().GetDescription(MensajesInterfaz.DefinirMesaCatalogo);
                cGral.conceptos = null;
                return cGral;
            }

            double porcAdicional = Convert.ToDouble(new cParametroCobroBL().GetByClave("PorcentajeAdicional")) / 100;
            if (porcAdicional == -1)
            {
                cGral.Error = new Utileria().GetDescription(MensajesInterfaz.PorcAdicional);
                cGral.conceptos = null;
                return cGral;
            }
            //Ingresando conceptos
            #region ANTICIPADO
            int IdConceptoRef = 0;

            if (i.AntImpuesto > 0)
            {
                IdConceptoRef = 0;
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AntImpuestoIP");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Impuesto Anticipado en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Impuesto Anticipado " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }   
                string descripcion = ccon.Descripcion + ". Bimestres pagados " + i.AntBimestre.ToString() + " Periodo pag. " + i.PerAnticipado.bInicial.ToString() + " " + i.PerAnticipado.eInicial.ToString() + " al " +
                                i.PerAnticipado.bFinal.ToString() + " " + i.PerAnticipado.eFinal.ToString();
                
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.AntImpuesto, i.AntDescImpuesto, descripcion, porcAdicional, 2, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //ant impuesto
                IdConceptoRef = ccon.Id;
            }

            if (i.AntAdicional > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AntAdicionalIP");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional Anticipado en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catlago Parámetros de Sistema: CRI de Adicional Anticipado " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.AntAdicional, i.AntDescAdicional, ccon.Descripcion, porcAdicional, 5, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //ant adicional
            }
            #endregion ANTICIPADO

            #region ACTUAL
            if (i.ActImpuesto > 0)
            {
                IdConceptoRef = 0;
                c = new ConceptosRec();
                c.Id = (new cParametroSistemaBL().GetValorByClave("ImpuestoIP")).ToString();
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Impuesto Anticipado en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Impuesto registrado " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                string descripcion = ccon.Descripcion + ". Bimestres pagados " + i.ActBimestre.ToString() + ", Periodo pag. " + i.PerActual.bInicial.ToString() + " " + i.PerActual.eInicial.ToString() + " al " +
                                i.PerActual.bFinal.ToString() + " " + i.PerActual.eFinal.ToString() + complementoConcepto;               
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.ActImpuesto, i.ActDescImpuesto, descripcion, porcAdicional, 6, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //impuesto
                IdConceptoRef = ccon.Id;
            }
            
            if (i.ActRecargo > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("RecargoIP");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Recargo del año atual en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalgo Parámetros de Sistema: CRI de Recargo del año atual " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.ActRecargo, i.ActDescRecargo, ccon.Descripcion, porcAdicional, 10, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad));  //recargo
            }

            if (i.ActAdicional > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AdicionalIP");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Adicional " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }         
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.ActAdicional, i.ActDescAdicional, ccon.Descripcion, porcAdicional, 9, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //adicional
            }
            #endregion impuestos
                        
            #region DIFERENCIAS
            
            if (i.ActDiferencias > 0)
            {
                IdConceptoRef = 0;
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("DIFERENCIAS");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Diferencias en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Diferencias  " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                descDiferencia = new cDiferenciaBL().GetDiferenciaPeriodoByPredio(claveCatastral);
                ccon.Descripcion = descDiferencia;//+ ccon.Descripcion;
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.ActDiferencias, i.ActDescDiferencias, ccon.Descripcion, porcAdicional,17, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad));
                IdConceptoRef = ccon.Id;
            }

            if (i.ActRecDiferencias > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("RecargoIP");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Recargo Diferencias en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Recargo Diferencias " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.ActRecDiferencias, i.ActDescRecDiferencias, ccon.Descripcion, porcAdicional,10, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //recargo
            }

            if ( i.ActAdicDiferencia > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AdicionalIP");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Adicional " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                r.Add(LlenaConcepto(claveCatastral ,c.Id, ccon.Id, IdConceptoRef, i.ActAdicDiferencia, i.ActDescAdicDiferencia, ccon.Descripcion, porcAdicional, 17, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //diferencias act 
            }
            #endregion diferencias

            #region REZAGOS
            
            #region rezago             
            if (i.Rezagos > 0)
            {
                IdConceptoRef = 0;
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("RezagosIP");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Rezagos en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Rezagos " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                c.Importe = Convert.ToDouble(i.Rezagos);
                string descripcion = ccon.Descripcion + ". Bimestres pagados " + i.RezBimestre.ToString() + "Periodo pag. " + i.PerRezago.bInicial.ToString() + " " + i.PerRezago.eInicial.ToString() + " al " +
                                i.PerRezago.bFinal.ToString() + " " + i.PerRezago.eFinal.ToString() + complementoConcepto;                
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.Rezagos, i.RezDescRezagos, descripcion, porcAdicional, 11, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad));//rezagos
                IdConceptoRef = ccon.Id;
            }            

            if (i.RezRecargo > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("REZAGORECARGO");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Recargos de rezago en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Recargos de rezago " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.RezRecargo, i.RezDescRecargos, ccon.Descripcion, porcAdicional, 13, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad));  //recargo              
            }

            if (i.RezAdicional > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("REZAGOADICIONAL");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional de rezago en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Adicional de Rezago " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.RezAdicional, i.RezDescAdicional, ccon.Descripcion, porcAdicional,12, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //adicional
            }
            #endregion rezago

            #region rezago diferencia
            int IdConceptoRefdIF = 0;
            if (i.RezDiferencias > 0)
            {                
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("DIFERENCIAREZ");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Diferencias en rezagos en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo ¨Parámetros de Sistema: CRI de Diferencias Rezago " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                if (i.ActDiferencias <= 0)
                {
                    descDiferencia = new cDiferenciaBL().GetDiferenciaPeriodoByPredio(claveCatastral);
                    ccon.Descripcion = descDiferencia;// +" " + ccon.Descripcion;
                }
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRefdIF, i.RezDiferencias, i.RezDescDiferencias, ccon.Descripcion, porcAdicional,18, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //rezago diferencias
                IdConceptoRefdIF = ccon.Id;
            }

            if (i.RezRecDiferencias > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("REZAGORECARGO");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Recargos de diferencias en rezago en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Recargo de diferencias Rezago " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }     
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRefdIF, i.RezRecDiferencias, i.RezDescRecDiferencias, ccon.Descripcion, porcAdicional, 13, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad));//rezago recargo
            }

            if (i.RezAdicDiferencias > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("REZAGOADICIONAL");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional del Rezago en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Adicional del Rezago " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                ccon.Descripcion = new cDiferenciaBL().GetDiferenciaPeriodoByPredio( claveCatastral); 
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRefdIF, i.RezAdicDiferencias, i.RezDescAdicDiferencias, ccon.Descripcion, porcAdicional, 12, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //rezago adicional
            }
            IdConceptoRefdIF = 0;

            #endregion diferencias

            #endregion REZAGOS

            #region REQUERIMIENTOS

            if (i.Multa > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("MultasIP");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Multas en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Multas " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.Multa, i.MultaDesc, ccon.Descripcion, porcAdicional,16, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //multas
            }

            if (i.Honorarios > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("HONORARIOSIP");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Honorarios en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Honorarios " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.Honorarios, i.HonorariosDesc, ccon.Descripcion, porcAdicional,14, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //honorarios
            }

            if (i.Ejecucion > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("EjecucionIP");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Ejecución en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 5);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Ejecución " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }            
                r.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, i.Ejecucion, i.EjecucionDesc, ccon.Descripcion, porcAdicional, 15, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //ejecucion
            }
            #endregion

            cGral.conceptos = r;
            cGral.Error = null;

            return cGral;
        }

        public ConceptoGral ConceptosS(Servicio s, string claveCatastral)
        {
            ConceptosRec c = new ConceptosRec();
            List<ConceptosRec> lc = new List<ConceptosRec>();
            ConceptoGral cGral = new ConceptoGral();

            string sMesa = new cParametroSistemaBL().GetValorByClave("MESASM");
            if (sMesa == "")//validar si la mesa esta definida en la tabla cParametroSistema
            {
                cGral.Error = new Utileria().GetDescription(MensajesInterfaz.DefinirMesaIP);
                cGral.conceptos = null;
                return cGral;
            }
            int mesa = new cMesaBL().GetByNombre(sMesa);
            if (sMesa == "") //validar si la mesa esta en la tabla cMesa
            {
                cGral.Error = new Utileria().GetDescription(MensajesInterfaz.DefinirMesaCatalogo);
                cGral.conceptos = null;
                return cGral;
            }

            double porcAdicional = Convert.ToDouble(new cParametroCobroBL().GetByClave("PorcentajeAdicional")) / 100;
            if (porcAdicional == -1)
            {
                cGral.Error = new Utileria().GetDescription(MensajesInterfaz.PorcAdicional);
                cGral.conceptos = null;
                return cGral;
            }
            int IdConceptoRef = 0;

            #region ANTICIPADO

            #region anticipado infraestrutura
            if (s.AntInfraestructura > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AntInfraestructuraSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Infraestructura Anticipado en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Infraestructura Anticipado " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }         
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef , s.AntInfraestructura, s.AntDescInfraestructura, ccon.Descripcion, porcAdicional,19, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad));
                IdConceptoRef = ccon.Id;
            }

            if (s.AntAdicInfraestructura > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AntAdicionalSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional Anticipado en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetos de Sistema: CRI de Adicional Anticipado " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.AntAdicInfraestructura, s.AntDescAdicInfraestructura, ccon.Descripcion, porcAdicional,5, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad));
            }
            #endregion anticipado infraestructura

            #region anticipado recoleccion
            
            if (s.AntRecoleccion > 0)
            {
                IdConceptoRef = 0;
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AntRecoleccionSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Recolección Anticipado en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Recolección Anticipado " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.AntRecoleccion, s.AntDescRecoleccion, ccon.Descripcion, porcAdicional,21, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad));
                IdConceptoRef = ccon.Id;
            }

            if (s.AntAdicRecoleccion > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AntAdicionalSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional Anticipado en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Adicional Anticipado " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.AntAdicRecoleccion, s.AntDescAdicRecoleccion, ccon.Descripcion, porcAdicional,5, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad));
            }

            #endregion anticipado recoleccion

            #region anticipado limpieza 
            
            if (s.AntLimpieza > 0)
            {
                IdConceptoRef = 0;
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AntLimpiezaSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Limpieza Anticipado en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Limpieza Anticipado " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }               
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.AntLimpieza, s.AntDescLimpieza, ccon.Descripcion, porcAdicional,20, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //limpieza
                IdConceptoRef = ccon.Id;
            }

            if (s.AntAdicLimpieza > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AntAdicionalSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional Anticipado en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Adicional Anticipado " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.AntAdicLimpieza, s.AntDescAdicLimpieza, ccon.Descripcion, porcAdicional, 5, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //adic ant limpieza
            }

            #endregion anticipado limpieza

            #region anticipado dap
            
            if (s.AntDap > 0)
            {
                IdConceptoRef = 0;
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AntDapSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Dap Anticipado en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Dap Anticipado " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
              
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.AntDap, s.AntDescDap, ccon.Descripcion, porcAdicional,22, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //ant dap
                IdConceptoRef = ccon.Id;
            }

            if (s.AntAdicDap > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AntAdicionalSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional Anticipado en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Adicional Anticipado  " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.AntAdicDap, s.AntDescAdicDap, ccon.Descripcion, porcAdicional,5, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //ant adic dap
            }

            #endregion dap

            #endregion anticipado

            #region ACTUAL
            #region actual infraestructura
           
            if (s.ActInfraestructura > 0)
            {
                IdConceptoRef = 0;
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("InfraestructuraSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Infraestructura en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Infraestructura " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.ActInfraestructura, s.ActDescInfraestructura, ccon.Descripcion, porcAdicional, 23, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //infraestructura
                IdConceptoRef = ccon.Id;
            }
            if (s.ActRecInfraestructura > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("RecargoSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Recargo en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Recargo " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }               
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.ActRecInfraestructura, s.ActDescRecInfraestructura, ccon.Descripcion, porcAdicional, 10, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //act recargo
            }

            if (s.ActAdicInfraestructura > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AdicionalSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalago de Parámetros de Sistema: CRI de Adicional " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.ActAdicInfraestructura, s.ActDescAdicInfraestructura, ccon.Descripcion, porcAdicional, 9, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //adic infra
            }
            #endregion actual infraestrutura

            #region actual recoleccion
            
            if (s.ActRecoleccion > 0)
            {
                IdConceptoRef = 0;
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("RecoleccionSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Recolección en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Recolección " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }                
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.ActRecoleccion, s.ActDescRecoleccion, ccon.Descripcion, porcAdicional,24, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad));//recoleccion
                IdConceptoRef = ccon.Id;
            }
            if (s.ActRecRecoleccion > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("RecargoSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Recargo en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Recargo " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.ActRecRecoleccion, s.ActDescRecRecoleccion, ccon.Descripcion, porcAdicional, 10, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //act recargo 
            }

            if (s.ActAdicRecoleccion > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AdicionalSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI Adicional SM "+ c.Id +", no existe en el catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.ActAdicRecoleccion, s.ActDescAdicRecoleccion, ccon.Descripcion, porcAdicional, 9, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //adicional
            }

            #endregion recoleccion 

            #region actual limpieza
            
            if (s.ActLimpieza > 0)
            {
                IdConceptoRef = 0;
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("LimpiezaSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Limpieza en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Limpieza " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }              
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.ActLimpieza, s.ActDescLimpieza, ccon.Descripcion, porcAdicional, 25, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //limpieza
                IdConceptoRef = ccon.Id;
            }
            if (s.ActRecLimpieza > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("RecargoSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Recargo en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Recargo " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.ActRecLimpieza, s.ActDescRecLimpieza, ccon.Descripcion, porcAdicional, 10, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //recargo limpieza
            }

            if (s.ActAdicLimpieza > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AdicionalSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Adicional " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.ActAdicLimpieza, s.ActDescAdicLimpieza, ccon.Descripcion, porcAdicional, 9, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //adicional limpieza
            }
            #endregion limpieza

            #region actual dap
            
            if (s.ActDap > 0)
            {
                IdConceptoRef = 0;
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("DapSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Dap en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogos de Parámetros de Sistema: CRI de Dap " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }             
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.ActDap, s.ActDescDap, ccon.Descripcion, porcAdicional, 26, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //dap
                IdConceptoRef = ccon.Id;
            }

            if (s.ActRecDap > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("RecargoSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Recargo en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Recargo " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.ActRecDap, s.ActDescRecDap, ccon.Descripcion, porcAdicional, 10, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //recargo dap
            }

            if (s.ActAdicDap > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("AdicionalSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Cata,ogo de Parámetros de Sistema: CRI de Adicional " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.ActAdicDap, s.ActDescAdicDap, ccon.Descripcion, porcAdicional,9, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //adicional dap
            }
            #endregion

            #endregion

            #region REZAGO
            
            if (s.Rezagos > 0)
            {
                IdConceptoRef = 0;
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("RezagoSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Rezago en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Rezago " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }              
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.Rezagos, s.RezDescRezagos, ccon.Descripcion, porcAdicional, 11, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //rezago
                IdConceptoRef = ccon.Id;
            }

            if (s.RezRecargo > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("RecargoSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Recargos de Rezago en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Recargos de Rezago " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.RezRecargo, s.RezDescRecargos, ccon.Descripcion, porcAdicional, 13, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //rezago recargo
            }

            if (s.RezAdicional > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("ADICIONALSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Adicional de Rezago en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Adicional de Rezago " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.RezAdicional, s.RezDescAdicional, ccon.Descripcion, porcAdicional, 12, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //rezago adicional
            }
            #endregion

            #region REQUERIMIENTOS
            //IdConceptoRef = 0; NO SE REINICIA PARA ENLAZAR LOS
            if (s.Multa > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("MULTASSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Multas en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo de Parámetros de Sistema: CRI de Multas " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }              
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.Multa, s.MultaDesc, ccon.Descripcion, porcAdicional,16, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //multas
            }

            if (s.Honorarios > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("HONORARIOSSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Honorarios en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Honorarios " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.Honorarios, s.HonorariosDesc, ccon.Descripcion, porcAdicional,14, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //honorarios
            }

            if (s.Ejecucion > 0)
            {
                c = new ConceptosRec();
                c.Id = new cParametroSistemaBL().GetValorByClave("EjecucionSM");
                if (c.Id == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    cGral.Error = "No se ha definido el CRI de Ejecución en Parámetros de Sistema, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }
                cConcepto ccon = new cConceptoBL().RegistroByCriIdTramite(c.Id, DateTime.Today.Year, mesa, 6);
                if (ccon == null)
                {
                    cGral.Error = "Catalogo Parámetros de Sistema: CRI de Ejecución " + c.Id + ", no existe en el Catalogo de Conceptos, consulte al Administrador.";
                    cGral.conceptos = null;
                    return cGral;
                }               
                lc.Add(LlenaConcepto(claveCatastral, c.Id, ccon.Id, IdConceptoRef, s.Ejecucion, s.EjecucionDesc, ccon.Descripcion, porcAdicional, 15, ccon.cProdServ.ClaveProdServ, ccon.cUnidadMedida.ClaveUnidad)); //ejecucion
            }
            #endregion

            cGral.conceptos = lc;
            cGral.Error = null;
            return cGral;
        }

        private ConceptosRec LlenaConcepto(string clave, string cri, int idConcepto, int idRef, decimal neto, decimal descuento, string descripcion, double porcAdicional, int idControl, string ClaveProdServ, string ClaveUnidadMedida)
        {
            ConceptosRec c = new ConceptosRec();
            c.Id = cri;
            c.IdCri = idConcepto;            
            c.Importe = Convert.ToDouble(neto);
            c.Descripcion = descripcion + ". Subtotal $ " + c.Importe;
            c.Cantidad = 1;
            c.Unidad = "N/A";
            c.CuentaPredial = clave;
            c.Adicional = Utileria.Redondeo(c.Importe * porcAdicional);
            c.ValorUnitario = Convert.ToDouble(c.Importe);
            c.IdConceptoRef = idRef;
            c.IdControlSistema = idControl;
            c.ClaveProdServ = ClaveProdServ;
            c.ClaveUnidadMedida = ClaveUnidadMedida;
            if (descuento > 0)
            {
                c.Descuento = Convert.ToDouble(descuento);
                c.PorcentajeDescto = Convert.ToInt32(Utileria.Redondeo((Convert.ToDouble(c.Descuento * 100) / c.Importe)));
                c.Descripcion = c.Descripcion + " " + c.PorcentajeDescto + "%  Descuento aplicado $ " + (c.Descuento).ToString();
            }
            return c;
        }
        
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection propiedades = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in propiedades)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in propiedades)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public DataTable ConvertToDatatableIP(List<ImpuestoBimestral> l)
        {
            decimal totalImp = 0;
            decimal totalAdi = 0;
            decimal totalImpINP = 0;
            decimal totalAdiINP = 0;
            decimal totalRec = 0;
            decimal total = 0;
            DataTable dt = new DataTable();

            dt.Columns.Add("Ejercicio");
            dt.Columns.Add("Bimestre");
            dt.Columns.Add("BaseImpuesto");
            dt.Columns.Add("BaseGravable");
            dt.Columns.Add("Uma");
            dt.Columns.Add("Impuesto");
            dt.Columns.Add("Adicional");
            dt.Columns.Add("IndActual");
            dt.Columns.Add("IndAnterior");
            dt.Columns.Add("PorcentajeINP");
            dt.Columns.Add("PorcentajeRecargo");
            dt.Columns.Add("ImpuestoINP");
            dt.Columns.Add("AdicionalINP");
            dt.Columns.Add("Recargo");
            dt.Columns.Add("Total");

            foreach (ImpuestoBimestral item in l)
            {
                var row = dt.NewRow();

                row["Ejercicio"] = item.Ejercicio;
                row["Bimestre"] = item.Bimestre;
                row["BaseImpuesto"] = item.BaseImpuesto.ToString("N2", CultureInfo.CurrentCulture);
                row["BaseGravable"] = item.BaseGravable.ToString("N2", CultureInfo.CurrentCulture);
                row["Uma"] = item.Uma.ToString("N2", CultureInfo.CurrentCulture);
                row["Impuesto"] = item.Impuesto.ToString("N2", CultureInfo.CurrentCulture);
                row["Adicional"] = item.Adicional.ToString("N2", CultureInfo.CurrentCulture);
                row["IndActual"] = item.IndActual.ToString("N4", CultureInfo.CurrentCulture);
                row["IndAnterior"] = item.IndAnterior.ToString("N4", CultureInfo.CurrentCulture);
                row["PorcentajeINP"] = item.PorcentajeINP.ToString("N4", CultureInfo.CurrentCulture);
                row["PorcentajeRecargo"] = item.PorcentajeRecargo.ToString("N4", CultureInfo.CurrentCulture);
                row["ImpuestoINP"] = (item.Impuesto + item.ImpuestoINP).ToString("N2", CultureInfo.CurrentCulture);
                row["AdicionalINP"] = (item.Adicional + item.AdicionalINP).ToString("N2", CultureInfo.CurrentCulture);
                row["Recargo"] = item.Recargo.ToString("N2", CultureInfo.CurrentCulture);
                row["Total"] = (item.Impuesto + item.ImpuestoINP + item.Adicional + item.AdicionalINP + item.Recargo).ToString("N", CultureInfo.CurrentCulture);

                totalImp += item.Impuesto;
                totalAdi += item.Adicional;
                totalImpINP += (item.Impuesto + item.ImpuestoINP);
                totalAdiINP += (item.Adicional + item.AdicionalINP);
                totalRec += item.Recargo;
                total += (item.Impuesto + item.ImpuestoINP + item.Adicional + item.AdicionalINP + item.Recargo);

                dt.Rows.Add(row);
            }

            //decimal totalImp = Convert.ToDecimal(dt.Compute("SUM(Impuesto)", ""));
            //decimal totalAdi = Convert.ToDecimal(dt.Compute("SUM(Adicional)", ""));
            //decimal totalImpINP = Convert.ToDecimal( dt.Compute("SUM(ImpuestoINP)", ""));
            //decimal totalAdiINP = Convert.ToDecimal(dt.Compute("SUM(AdicionalINP)", ""));
            //decimal totalRec = Convert.ToDecimal(dt.Compute("SUM(Recargo)", ""));
            //decimal total = Convert.ToDecimal(dt.Compute("SUM(Total)", ""));

            DataRow d = dt.NewRow();
            d["Impuesto"] = totalImp.ToString("C", CultureInfo.CurrentCulture);
            d["Adicional"] = totalAdi.ToString("C", CultureInfo.CurrentCulture);
            d["ImpuestoINP"] = totalImpINP.ToString("C", CultureInfo.CurrentCulture);
            d["AdicionalINP"] = totalAdiINP.ToString("C", CultureInfo.CurrentCulture);
            d["Recargo"] = totalRec.ToString("C", CultureInfo.CurrentCulture);
            d["Total"] = total.ToString("C", CultureInfo.CurrentCulture);

            dt.Rows.Add(d);

            return dt;
        }

        public DataTable ConvertToDatatableSM(List<ServicioBimestral> l)
        {            
            decimal total = 0;
            decimal totalInfra = 0, totalRecolecc = 0, totalDap = 0, totalLimpieza = 0, totalInfraINP = 0, totalRecolecINP= 0;
            decimal totalDapINP = 0, totalLimpiezaINP = 0, totalInfraRec = 0, totalRecolecRec = 0, totalDapRec = 0, totalLimpiezaRec = 0;
            DataTable dt = new DataTable();

            dt.Columns.Add("Ejercicio");
            dt.Columns.Add("Bimestre");
            dt.Columns.Add("Infra");
            dt.Columns.Add("Recolecc");
            dt.Columns.Add("Dap");
            dt.Columns.Add("Limpieza");   
            dt.Columns.Add("IndActual");
            dt.Columns.Add("IndAnterior");
            dt.Columns.Add("PorcINP");
            dt.Columns.Add("PorcRecargo");
            dt.Columns.Add("InfraINP");
            dt.Columns.Add("RecoleccINP");
            dt.Columns.Add("DapINP");
            dt.Columns.Add("LimpiezaINP");
            dt.Columns.Add("InfraRec");
            dt.Columns.Add("RecoleccRec");
            dt.Columns.Add("DapRec");
            dt.Columns.Add("LimpiezaRec");
            dt.Columns.Add("Total");

            foreach (ServicioBimestral item in l)
            {
                var row = dt.NewRow();

                row["Ejercicio"] = item.Ejercicio.ToString("N", CultureInfo.CurrentCulture);
                row["Bimestre"] = item.NoBimestre.ToString("N", CultureInfo.CurrentCulture);
                row["Infra"] = item.Infraestructura.ToString("N2", CultureInfo.CurrentCulture);
                row["Recolecc"] = item.Recoleccion.ToString("N2", CultureInfo.CurrentCulture);
                row["Dap"] = item.Dap.ToString("N2", CultureInfo.CurrentCulture);
                row["Limpieza"] = item.Limpieza.ToString("N2", CultureInfo.CurrentCulture);
                row["IndActual"] = item.IndActual.ToString("N4", CultureInfo.CurrentCulture);
                row["IndAnterior"] = item.IndAnterior.ToString("N4", CultureInfo.CurrentCulture);
                row["PorcINP"] = item.PorcentajeINP.ToString("N4", CultureInfo.CurrentCulture);
                row["PorcRecargo"] = item.PorcentajeRecargo.ToString("N4", CultureInfo.CurrentCulture);
                row["InfraINP"] = item.InfraestructuraIPN.ToString("N2", CultureInfo.CurrentCulture);
                row["RecoleccINP"] = item.RecoleccionIPN.ToString("N2", CultureInfo.CurrentCulture);
                row["DapINP"] = item.DapIPN.ToString("N2", CultureInfo.CurrentCulture);
                row["LimpiezaINP"] = item.LimpiezaIPN.ToString("N2", CultureInfo.CurrentCulture);
                row["InfraRec"] = item.InfraestructuraRec.ToString("N2", CultureInfo.CurrentCulture);
                row["RecoleccRec"] = item.RecoleccionRec.ToString("N2", CultureInfo.CurrentCulture);
                row["DapRec"] = item.DapRec.ToString("N2", CultureInfo.CurrentCulture);
                row["LimpiezaRec"] = item.LimpiezaRec.ToString("N2", CultureInfo.CurrentCulture);
                row["Total"] = item.Infraestructura + item.Recoleccion + item.Dap + item.Limpieza +
                        item.InfraestructuraIPN + item.RecoleccionIPN + item.DapIPN + item.LimpiezaIPN +
                        item.InfraestructuraRec + item.RecoleccionRec + item.DapRec + item.LimpiezaRec;
                dt.Rows.Add(row);

                totalInfra += item.Infraestructura;
                totalRecolecc += item.Recoleccion;
                totalDap += item.Dap;
                totalLimpieza += item.Limpieza;
                totalInfraINP += item.InfraestructuraIPN;
                totalRecolecINP += item.RecoleccionIPN;
                totalDapINP += item.DapIPN;
                totalLimpiezaINP += item.LimpiezaIPN;
                totalInfraRec += item.InfraestructuraRec;
                totalRecolecRec += item.RecoleccionRec;
                totalDapRec += item.DapRec;
                totalLimpiezaRec += item.LimpiezaRec;
                total += item.Infraestructura + item.Recoleccion + item.Dap + item.Limpieza +
                        item.InfraestructuraIPN + item.RecoleccionIPN + item.DapIPN + item.LimpiezaIPN +
                        item.InfraestructuraRec + item.RecoleccionRec + item.DapRec + item.LimpiezaRec;
            }

            DataRow d = dt.NewRow();
           
            d["Infra"] = totalInfra.ToString("C", CultureInfo.CurrentCulture);
            d["Recolecc"] = totalRecolecc.ToString("C", CultureInfo.CurrentCulture);
            d["Dap"] = totalDap.ToString("C", CultureInfo.CurrentCulture);
            d["Limpieza"] = totalLimpieza.ToString("C", CultureInfo.CurrentCulture);
            d["InfraINP"] = totalInfraINP.ToString("C", CultureInfo.CurrentCulture);
            d["RecoleccINP"] = totalRecolecINP.ToString("C", CultureInfo.CurrentCulture);
            d["DapINP"] = totalDapINP.ToString("C", CultureInfo.CurrentCulture);
            d["LimpiezaINP"] = totalLimpiezaINP.ToString("C", CultureInfo.CurrentCulture);
            d["InfraRec"] = totalInfraRec.ToString("C", CultureInfo.CurrentCulture);
            d["RecoleccRec"] = totalRecolecRec.ToString("C", CultureInfo.CurrentCulture);
            d["DapRec"] = totalDapRec.ToString("C", CultureInfo.CurrentCulture);
            d["LimpiezaRec"] = totalLimpiezaRec.ToString("C", CultureInfo.CurrentCulture);
            d["Total"] = total.ToString("C", CultureInfo.CurrentCulture);

            dt.Rows.Add(d);

            return dt;
        }

        public DataTable ConvertToDatatableDif(List<Diferencias> l)
        {
            decimal totalDif = 0;
            decimal totalAdi = 0;
            decimal totalDifINP = 0;
            decimal totalRec = 0;
            decimal total = 0;

            DataTable dt = new DataTable();
            dt.Columns.Add("Tipo");
            dt.Columns.Add("Ejercicio");
            dt.Columns.Add("PorcentajeINP");
            dt.Columns.Add("PorcentajeRecargo");            
            dt.Columns.Add("Bimestre");
            dt.Columns.Add("Diferencia");
            dt.Columns.Add("DiferenciaINP");            
            dt.Columns.Add("Adicional");
            dt.Columns.Add("Recargo");            
            dt.Columns.Add("Total");

            foreach (Diferencias item in l)
            {
                var row = dt.NewRow();
                row["Tipo"] = item.Tipo;
                row["Ejercicio"] = item.Ejercicio;
                row["PorcentajeRecargo"] = item.PorcentajeRecargo.ToString("N4", CultureInfo.CurrentCulture);
                row["PorcentajeINP"] = item.PorcentajeINP.ToString("N4", CultureInfo.CurrentCulture);
                row["Bimestre"] = item.Bimestre;
                row["Diferencia"] = item.Diferencia.ToString("N2", CultureInfo.CurrentCulture);
                row["DiferenciaINP"] = item.ActualizacionINP.ToString("N2", CultureInfo.CurrentCulture);                
                row["Adicional"] = item.Adicional.ToString("N2", CultureInfo.CurrentCulture);
                row["Recargo"] = item.Recargo.ToString("N2", CultureInfo.CurrentCulture);
                row["Total"] =  item.Total.ToString("N", CultureInfo.CurrentCulture);

                totalDif += item.Diferencia;
                totalAdi += item.Adicional;
                totalDifINP += item.ActualizacionINP;                
                totalRec += item.Recargo;
                total += item.Total;
                dt.Rows.Add(row);
            }

            DataRow d = dt.NewRow();
            d["Diferencia"] = totalDif.ToString("N2", CultureInfo.CurrentCulture);
            d["DiferenciaINP"] = totalDifINP.ToString("N2", CultureInfo.CurrentCulture);
            d["Adicional"] = totalAdi.ToString("N2", CultureInfo.CurrentCulture);
            d["Recargo"] = totalRec.ToString("N2", CultureInfo.CurrentCulture);
            d["Total"] = total.ToString("C", CultureInfo.CurrentCulture);
            dt.Rows.Add(d);

            return dt;
        }

        public Descuento ValidaDescuentoClave(int idClavePredial, string tipo, int jyp, int rez, ref int gral)
        {
            string clave = string.Empty;
            cDescuento global = new cDescuentoBL().InicializaVarDescuento();
            cDescuento desc = new cDescuentoBL().InicializaVarDescuento();
            cDescuento descjyp = new cDescuentoBL().InicializaVarDescuento();
            cDescuento descrez = new cDescuentoBL().InicializaVarDescuento();
            Descuento concentrado = new Descuento();

            if (tipo == "IP") //campaña
            {
                clave = new cParametroSistemaBL().GetValorByClave("DescuentoGlobalIP");
                if (clave != "")
                {
                    desc = new cDescuentoBL().GetByClave(clave, DateTime.Today);
                    if (desc.Id > 0)
                        gral = desc.Id;
                    else
                        clave = "";
                }          
            }
            else
            {
                clave = new cParametroSistemaBL().GetValorByClave("DescuentoGlobalSM");
                if (clave != "")
                {
                    desc = new cDescuentoBL().GetByClave(clave, DateTime.Today);
                    if (desc.Id > 0)
                        gral = desc.Id;
                    else
                        clave = "";
                }
            }
            
            if (jyp != 0)
                descjyp = new cDescuentoBL().GetByIdClave(jyp, DateTime.Today);//jyp

            global = ComparaDescuentos(desc, descjyp);

            if (rez != 0)
                descrez = new cDescuentoBL().GetByIdClave(rez, DateTime.Today);//rezago
            
            if (rez == 0 && jyp == 0 && clave == "")
            {
                clave  = new tDescuentoAsignadoBL().RegresaClaveDesctoByIdPredio(idClavePredial);
                if (clave != "")
                {
                    descrez = new cDescuentoBL().GetByClave(clave, DateTime.Today);
                }
            }

            global = ComparaDescuentos(global, descrez);

            concentrado.AnticipadoImpuesto = global.AnticipadoImpuesto.Value;
            concentrado.AnticipadoAdicional = global.AnticipadoAdicional.Value;
            concentrado.ActualImpuesto = global.ActualImpuesto.Value;
            concentrado.ActualAdicional = global.ActualAdicional.Value;
            concentrado.ActualRecargo = global.ActualRecargo.Value;
            concentrado.Diferencia = global.Diferencia.Value;
            concentrado.DiferenciaRecargo = global.DiferenciaRecargo.Value;
            concentrado.Rezago = global.Rezago.Value;
            concentrado.RezagoRecargo = global.RezagoRecargo.Value;
            concentrado.RezagoAdicional = global.RezagoAdicional.Value;
            concentrado.Multas = global.Multas.Value;
            concentrado.Ejecucion = global.Ejecucion.Value;
            concentrado.Honorarios = global.Honorarios.Value;
            concentrado.AnticipadoInfraestructura = global.AnticipadoInfraestructura.Value;
            concentrado.AnticipadoAdicional = global.AnticipadoAdicional.Value;
            concentrado.AnticipadoLimpieza = global.AnticipadoLimpieza.Value;
            concentrado.AnticipadoRecoleccion = global.AnticipadoRecoleccion.Value;
            concentrado.AnticipadoDap = global.AnticipadoDap.Value;
            concentrado.Infraestructura = global.Infraestructura.Value;
            concentrado.ActualAdicional = global.ActualAdicional.Value;
            concentrado.ActualLImpieza = global.ActualLImpieza.Value;
            concentrado.ActualRecoleccion = global.ActualRecoleccion.Value;
            concentrado.ActualDap = global.ActualDap.Value;
            concentrado.ActualRecargo = global.ActualRecargo.Value;
            concentrado.Rezago = global.Rezago.Value;
            concentrado.RezagoRecargo = global.RezagoRecargo.Value;
            concentrado.RezagoAdicional = global.RezagoAdicional.Value;
            concentrado.Ejecucion = global.Ejecucion.Value;
            concentrado.Multas = global.Multas.Value;
            concentrado.Clave = global.Clave;

            return concentrado;

        }

        public cDescuento ComparaDescuentos(cDescuento d, cDescuento desc)
        {
            cDescuento global = new cDescuento();


            //ANTIMPUESTOIP	           
            global.AnticipadoImpuesto = d.AnticipadoImpuesto.Value >= desc.AnticipadoImpuesto.Value ? d.AnticipadoImpuesto.Value : desc.AnticipadoImpuesto.Value;
            if ( d.AnticipadoImpuesto.Value  > desc.AnticipadoImpuesto.Value && d.Clave!= "")
                global.Clave = d.Clave;
            else
                if ( desc.Clave != "")
                    global.Clave = desc.Clave;

            //ADICIONAL ANTICIPADO  	
            global.AnticipadoAdicional = d.AnticipadoAdicional.Value >= desc.AnticipadoAdicional.Value ? d.AnticipadoAdicional.Value : desc.AnticipadoAdicional.Value;
            //IMPUESTOIP	1.2.1.1.1.1
            global.ActualImpuesto = d.ActualImpuesto.Value >= desc.ActualImpuesto.Value ? d.ActualImpuesto.Value : desc.ActualImpuesto.Value;
            if (d.ActualImpuesto.Value > desc.ActualImpuesto.Value && d.Clave != "")
                global.Clave = d.Clave;
            else 
               if (desc.Clave != "")
                    global.Clave = desc.Clave;
            //ADICIONALIP	1.8.1.1
            global.ActualAdicional = d.ActualAdicional.Value >= desc.ActualAdicional.Value ? d.ActualAdicional.Value : desc.ActualAdicional.Value;
            //RECARGOIP	6.1.15.3.1
            global.ActualRecargo = d.ActualRecargo.Value >= desc.ActualRecargo.Value ? d.ActualRecargo.Value : desc.ActualRecargo.Value;
            //DIFERENCIAS	1.2.1.1.1.1
            global.Diferencia = d.Diferencia.Value >= desc.Diferencia.Value ? d.Diferencia.Value : desc.Diferencia.Value;
            //DIFERENCIAREC 6.1.15.3.1
            global.DiferenciaRecargo = d.DiferenciaRecargo.Value >= desc.DiferenciaRecargo.Value ? d.DiferenciaRecargo.Value : desc.DiferenciaRecargo.Value;
            //REZAGOSIP	1.2.1.3.1
            global.Rezago = d.Rezago.Value >= desc.Rezago.Value ? d.Rezago.Value : desc.Rezago.Value;
            //REZAGORECARGOIP	6.1.15.3.1
            global.RezagoRecargo = d.RezagoRecargo.Value >= desc.RezagoRecargo.Value ? d.RezagoRecargo.Value : desc.RezagoRecargo.Value;
            global.RezagoAdicional = d.RezagoAdicional.Value >= desc.RezagoAdicional.Value ? d.RezagoAdicional.Value : desc.RezagoAdicional.Value;
            
            global.AnticipadoInfraestructura = d.AnticipadoInfraestructura.Value >= desc.AnticipadoInfraestructura.Value ? d.AnticipadoInfraestructura.Value : desc.AnticipadoInfraestructura.Value;
            //ANTADICIONALSM	1.8.1.1
            global.AnticipadoAdicional = d.AnticipadoAdicional.Value >= desc.AnticipadoAdicional.Value ? d.AnticipadoAdicional.Value : desc.AnticipadoAdicional.Value;
            //ANTLIMPIEZASM	1.8.1.3
            global.AnticipadoLimpieza = d.AnticipadoLimpieza.Value >= desc.AnticipadoLimpieza.Value ? d.AnticipadoLimpieza.Value : desc.AnticipadoLimpieza.Value;
            //ANTRECOLECCIONSM	1.8.1.4
            global.AnticipadoRecoleccion = d.AnticipadoRecoleccion.Value >= desc.AnticipadoRecoleccion.Value ? d.AnticipadoRecoleccion.Value : desc.AnticipadoRecoleccion.Value;
            //ANTDAPSM	4.3.5.2.2
            global.AnticipadoDap = d.AnticipadoDap.Value >= desc.AnticipadoDap.Value ? d.AnticipadoDap.Value : desc.AnticipadoDap.Value;
            //INFRAESTRUCTURASM	4.3.5.1.1
            global.Infraestructura = d.Infraestructura.Value >= desc.Infraestructura.Value ? d.Infraestructura.Value : desc.Infraestructura.Value;
            //global.ActualAdicional = d.ActualAdicional.Value >= desc.ActualAdicional.Value ? d.ActualAdicional.Value : desc.ActualAdicional.Value;
            //LIMPIEZASM	4.3.5.5.1
            global.ActualLImpieza = d.ActualLImpieza.Value >= desc.ActualLImpieza.Value ? d.ActualLImpieza.Value : desc.ActualLImpieza.Value;
            //RECOLECCIONSM	4.3.5.4.1
            global.ActualRecoleccion = d.ActualRecoleccion.Value >= desc.ActualRecoleccion.Value ? d.ActualRecoleccion.Value : desc.ActualRecoleccion.Value;
            //DAPSM	4.3.5.2.1
            global.ActualDap = d.ActualDap.Value >= desc.ActualDap.Value ? d.ActualDap.Value : desc.ActualDap.Value;

            global.Multas = d.Multas.Value >= desc.Multas.Value ? d.Multas.Value : desc.Multas.Value;
            global.Ejecucion = d.Ejecucion >= desc.Ejecucion.Value ? d.Ejecucion.Value : desc.Ejecucion.Value;
            global.Honorarios = d.Honorarios >= desc.Honorarios.Value ? d.Honorarios.Value : desc.Honorarios.Value;


            return global;

        }
        
        public MensajesInterfaz CancelaRecibo(int idtramite, tRecibo rec)
        {
            MensajesInterfaz msg = new MensajesInterfaz();
            try
            {
                tTramite t = new tTramiteBL().GetByConstraint(idtramite);
                cPredio predio = new cPredioBL().GetByConstraint(t.cPredio.Id);

                //ACTUALIZACION DE PREDIOS
                if (t.Tipo == "IP")//PREDIAL
                {
                    if (t.BimestreInicial > 0 && t.BimestreFinal > 0)
                    {
                        if (t.BimestreInicial == 1)
                        {
                            predio.BimestreFinIp = 6;
                            predio.AaFinalIp = t.EjercicioInicial.Value - 1;
                        }
                        else
                        {
                            predio.BimestreFinIp = t.BimestreInicial.Value - 1;
                            predio.AaFinalIp = t.EjercicioInicial.Value;
                        }
                        predio.ReciboIp = 0;
                        predio.IdReciboIp = null;
                        predio.FechaPagoIp = null;
                        msg = new cPredioBL().Update(predio);
                        if (msg != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                    }


                    //DIFERENCIAS
                    if (t.IdDiferencia != null)
                    {
                        cDiferencia dif = new cDiferenciaBL().GetByConstraint(t.IdDiferencia.Value);
                        if (dif != null)
                        { 
                            dif.Status = "A";
                            msg = new cDiferenciaBL().Update(dif);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        }
                    }

                    if (t.IdRequerimiento != null)
                    {
                        tRequerimiento req = new tRequerimientoBL().obtenerRequerimientoporIdPredioPagado(predio.Id, "IP");

                        if (req != null)
                        {
                            req.Status = "A";
                            req.FechaPago = null;
                            req.Recibo = null;

                            msg = new tRequerimientoBL().Update(req);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        }
                    }

                    t.Status = "C";
                    msg = new tTramiteBL().Update(t);
                    if (msg != MensajesInterfaz.Actualizacion)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    //tRecibo rec = new tReciboBL().GetByConstraint(idrecibo);
                    rec.EstadoRecibo = "C";
                    msg = new tReciboBL().Update(rec);
                    if (msg != MensajesInterfaz.Actualizacion)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                }

                if (t.Tipo == "SM")
                {
                    if (t.BimestreInicial == 1)
                    {
                        predio.BimestreFinSm = 6;
                        predio.AaFinalSm = t.EjercicioInicial.Value - 1;
                    }
                    else
                    {
                        predio.BimestreFinSm = t.BimestreInicial.Value - 1;
                        predio.AaFinalSm = t.EjercicioInicial.Value;
                    }
                    //predio.BimestreFinSm = t.BimestreFinal.Value;
                    //predio.AaFinalSm = t.EjercicioFinal.Value;
                    predio.ReciboSm = 0;
                    predio.IdReciboSm = null;
                    predio.FechaPagoSm = null;
                    msg = new cPredioBL().Update(predio);
                    if (msg != MensajesInterfaz.Actualizacion)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                    if (t.IdRequerimiento != null)
                    {
                        tRequerimiento reqs = new tRequerimientoBL().obtenerRequerimientoporIdPredio(predio.Id, "SM");
                        if (reqs != null)
                        { 
                            reqs.Status = "A";
                            reqs.FechaPago = null;
                            reqs.Recibo = null;
                            msg = new tRequerimientoBL().Update(reqs);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        }

                    }

                    t.Status = "C";
                    msg = new tTramiteBL().Update(t);
                    if (msg != MensajesInterfaz.Actualizacion)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    //tRecibo rec = new tReciboBL().GetByConstraint(idrecibo);
                    rec.EstadoRecibo = "C";
                    msg = new tReciboBL().Update(rec);
                    if (msg != MensajesInterfaz.Actualizacion)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                }

            } //try
            catch (Exception ex)
            {
                new Utileria().logError("SaldosC.CancelaRecibo", ex);
                return msg;

            }
            return msg;
        }

        public void GetExcelFromDataTable(DataTable dt, string fileName)
        {
            //string attachment = "attachment; filename=" + fileName;
            //HttpContext.Current.Response.ClearContent();
            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xls", fileName));
            ////HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"; //Excel 2003
            //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; //Excel 2007
            //string tab = "";
            //foreach (DataColumn dc in dt.Columns)
            //{
            //    HttpContext.Current.Response.Write(tab + dc.ColumnName);
            //    tab = "\t";
            //}
            //System.Web.HttpContext.Current.Response.Write("\n");

            //int i;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    tab = "";
            //    for (i = 0; i < dt.Columns.Count; i++)
            //    {
            //        if (dr[i].ToString().Trim() == "\"")
            //        {
            //            dr[i] = " \" ";
            //        }
            //        // HttpContext.Current.Response.Write(tab + dr[i].ToString());
            //        if (dr[i].ToString().StartsWith("0"))
            //        {
            //            HttpContext.Current.Response.Write(tab + @"=""" + dr[i].ToString().Trim() + @"""");
            //        }
            //        else
            //        {
            //            HttpContext.Current.Response.Write(tab + dr[i].ToString().Trim());
            //        }
            //        tab = "\t";
            //    }
            //    HttpContext.Current.Response.Write("\n");
            //}
            //HttpContext.Current.Response.End();
        }

       
        #endregion


    }//class EstadoDeCuenta   
}
