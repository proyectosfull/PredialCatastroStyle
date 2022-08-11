using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core;
using Clases;
using System.Data;
using Clases.Utilerias;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Data.Entity;

namespace Clases.BL
{
    /// <summary>
    /// 
    /// </summary>
    public class tRequerimientoBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public tRequerimientoBL()
        {
            Predial = new PredialEntities();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Insert(tRequerimiento obj)
        {
            MensajesInterfaz Insert;
            try
            {
                Predial.tRequerimiento.Add(obj);
                Predial.SaveChanges();
                Insert = MensajesInterfaz.Ingreso;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tReciboDetalleBL.Insert.DbUpdateException", ex);
                Insert = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tReciboDetalleBL.Insert.DataException", ex);
                Insert = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.Insert.Exception", ex);
                Insert = MensajesInterfaz.ErrorGeneral;
            }
            return Insert;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Update(tRequerimiento obj)
        {
            MensajesInterfaz Update;
            try
            {
                tRequerimiento objOld = Predial.tRequerimiento.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdPredio = obj.IdPredio;
                objOld.Folio = obj.Folio;
                objOld.TipoImpuesto = obj.TipoImpuesto;
                objOld.FechaEmision = obj.FechaEmision;
                objOld.TipoFase = obj.TipoFase;
                objOld.FechaLimite = obj.FechaLimite;
                objOld.BimestreInicial = obj.BimestreInicial;
                objOld.EjercicioInicial = obj.EjercicioInicial;
                objOld.BimestreFinal = obj.BimestreFinal;
                objOld.EjercicioFinal = obj.EjercicioFinal;
                objOld.IdDiferencias = obj.IdDiferencias;
                objOld.Impuesto = obj.Impuesto;
                objOld.Adicional = obj.Adicional;
                objOld.Recargo = obj.Recargo;
                objOld.Rezago = obj.Rezago;
                objOld.Diferencia = obj.Diferencia;
                objOld.RecargoDiferencia = obj.RecargoDiferencia;
                objOld.Recoleccion = obj.Recoleccion;
                objOld.Limpieza = obj.Limpieza;
                objOld.Dap = obj.Dap;
                objOld.Ejecucion = obj.Ejecucion;
                objOld.Multa = obj.Multa;
                objOld.Importe = obj.Importe;
                objOld.Status = obj.Status;
                objOld.StatusEnvio = obj.StatusEnvio;
                objOld.IdAgenteFiscal = obj.IdAgenteFiscal;
                objOld.FechaRecepcion = obj.FechaRecepcion;
                objOld.CaracterFirmante = obj.CaracterFirmante;
                objOld.Oficio = obj.Oficio;
                objOld.FechaOficio = obj.FechaOficio;
                objOld.Observaciones = obj.Observaciones;
                objOld.Recibo = obj.Recibo;
                objOld.FechaPago = obj.FechaPago;
                objOld.FechaCancelacion = obj.FechaCancelacion;
                objOld.Activo = obj.Activo;
                objOld.IdPredio = obj.IdPredio;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                objOld.IdTipoFaseDoc = obj.IdTipoFaseDoc;
                Predial.SaveChanges();
                Update = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tReciboDetalleBL.Update.DbUpdateException", ex);
                Update = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tReciboDetalleBL.Update.DataException", ex);
                Update = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.Update.Exception", ex);
                Update = MensajesInterfaz.ErrorGeneral;
            }
            return Update;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tRequerimiento GetByConstraint(int id)
        {
            tRequerimiento obj = null;
            try
            {
                obj = Predial.tRequerimiento.FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.GetByConstraint.Exception", ex, "--Parámetros id:" + id);
            }
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Delete(tRequerimiento obj)
        {
            MensajesInterfaz Delete;
            try
            {
                tRequerimiento objOld = Predial.tRequerimiento.FirstOrDefault(c => c.Id == obj.Id);
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Delete = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tReciboDetalleBL.Delete.DbUpdateException", ex);
                Delete = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tReciboDetalleBL.Delete.DataException", ex);
                Delete = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.Delete.Exception", ex);
                Delete = MensajesInterfaz.ErrorGeneral;
            }
            return Delete;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<tRequerimiento> GetAll()
        {
            List<tRequerimiento> objList = null;
            try
            {
                objList = Predial.tRequerimiento.Where(o => o.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.GetAll.Exception", ex);
            }
            return objList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<string> ListaCampos()
        {
            List<string> propertyList = new List<string>();
            try
            {
                tRequerimiento pObject = new tRequerimiento();
                if (pObject != null)
                {
                    foreach (var prop in pObject.GetType().GetProperties())
                    {
                        if (prop.Name.ToUpper() != "ID" && prop.Name.ToUpper() != "IDUSUARIO" && prop.Name.ToUpper() != "ACTIVO" && prop.Name.ToUpper() != "FECHAMODIFICACION" && prop.Name.Substring(0, 1) != "c" && prop.Name.Substring(0, 1) != "t" && prop.Name.Substring(0, 1) != "m" && prop.Name.Substring(0, 1) != "I")
                            propertyList.Add(prop.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.ListaCampos.Exception", ex);
            }
            return propertyList;
        }
        /// <param name="parametros">
        /// 0-Id Predio
        /// 1-NO
        /// 2-Bimestre Inicial
        /// 3-Ejerciio Inicial
        /// 4-Bimestr Final
        /// 5-Ejerciio Final
        /// 6-Id Descuento
        /// 7-Rezago
        /// </param>
        /// <returns></returns>
        public DataTable GenerarEdoCuenta(List<List<string>> parametros, cConcepto conHonorarios, cConcepto conEjecucion, cConcepto conMulta, cConcepto conHonorariosSM, cConcepto conEjecucionSM, cConcepto conMultaSM, decimal sm, int IdTipoFaseIp)
        {
            cPredio obj = null;
            DataTable dt = new DataTable();
            MensajesInterfaz msg = new MensajesInterfaz();
            #region definicion datatable
            dt.Columns.Add("Consecutivo");
            dt.Columns.Add("IdPredio");
            dt.Columns.Add("ClavePredial");
            dt.Columns.Add("Ubicacion");
            dt.Columns.Add("Contribuyente");
            dt.Columns.Add("Periodo");
            dt.Columns.Add("Fase");

            dt.Columns.Add("Impuesto", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Adicionales", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Rezagos", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Recargos", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Diferencias", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Rec", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Honorarios", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Ejecucion", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Multas", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Importe", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Base", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("SM");
            dt.Columns.Add("Infraestructura", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Limpieza", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("DAP", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Recoleccion", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("ImpuestoSM", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("AdicionalesSM", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("RezagosSM", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("RecargosSM", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("HonorariosSM", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("EjecucionSM", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("MultasSM", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("ImporteSM", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Folio");
            dt.Columns.Add("Fecha");
            dt.Columns.Add("Error");
            dt.Columns.Add("Bimestres");
            dt.Columns.Add("Agente");

            #endregion

            string mesaSM = "MESASM";
            string mesa = "MESAIP";

            String strFase = "";
            if (IdTipoFaseIp != 0)
            {
                int faseSiguiente = Predial.cTipoFase.Where(f => f.Activo == true && f.Id == IdTipoFaseIp).FirstOrDefault().FaseSiguiente;
                cTipoFase fase = Predial.cTipoFase.Where(f => f.Activo == true && f.Fase == faseSiguiente).FirstOrDefault();
                if (fase != null)
                    strFase = fase.DoctoImprimir;
                else
                    strFase = "Sin Documento";
            }
            else
            {
                cTipoFase fase = Predial.cTipoFase.Where(f => f.Activo == true && f.Fase == obj.cTipoFase1.FaseSiguiente).FirstOrDefault();
                if (fase != null)
                    strFase = fase.DoctoImprimir;
                else
                    strFase = "Sin Documento";
            }
            String smReq = Predial.cParametroSistema.Where(p => p.Activo == true && p.Clave.ToUpper() == "SMENREQUERIMIENTO").FirstOrDefault() != null ? Predial.cParametroSistema.Where(p => p.Activo == true && p.Clave.ToUpper() == "SMENREQUERIMIENTO").FirstOrDefault().Valor : "NO";
            decimal porcAdicional = new cParametroCobroBL().GetByClave("PorcentajeAdicional") / 100;
            string recalcularMandamiento = "SI";
            if (new cParametroSistemaBL().GetByClave("RECALCULARMANDAMIENTO") != null) recalcularMandamiento = new cParametroSistemaBL().GetByClave("RECALCULARMANDAMIENTO").Valor.ToUpper();

            SaldosC saldos = new SaldosC();

            foreach (List<string> param in parametros)
            {
                DataRow dr = dt.NewRow();
                try
                {
                    int id = Int32.Parse(param[0]);
                    obj = Predial.cPredio.Where(p => p.Id == id).FirstOrDefault();
                    if (obj != null)
                    {
                        dr["IdPredio"] = obj.Id;
                        //dr["ClavePredial"] = obj.ClavePredial;
                        string cpred = obj.ClavePredial.Substring(0, 4) + "-" + obj.ClavePredial.Substring(4, 2) + "-" + obj.ClavePredial.Substring(6, 3) + "-" + obj.ClavePredial.Substring(9, 3);
                        dr["ClavePredial"] = cpred;
                        dr["Ubicacion"] = obj.Calle + " NÚM. " + obj.Numero + " COL. " + obj.cColonia.NombreColonia + " C.P. " + obj.CP + " LOCALIDAD " + obj.Localidad;
                        dr["Contribuyente"] = obj.cContribuyente.Nombre + " " + obj.cContribuyente.ApellidoPaterno + " " + obj.cContribuyente.ApellidoMaterno;
                        dr["Fecha"] = param[7];
                        dr["Fase"] = strFase;

                        #region Servicios Municipales
                        if (smReq == "SI")
                        {
                            Servicio servicio = saldos.CalculaCobroSM(id, obj.BimestreFinSm, obj.AaFinalSm, Int32.Parse(param[4]), Int32.Parse(param[5]), 0, 0, "Rezago", obj.Zona, obj.IdTipoPredio, Convert.ToDouble(obj.SuperficieConstruccion));
                            if (servicio.TextError != null)
                            {
                                dr["Error"] = dr["Error"].ToString() == "" ? servicio.TextError : dr["Error"].ToString() + ", " + servicio.TextError;
                                dr["SM"] = "";
                                dr["ImpuestoSM"] = 0;
                                dr["AdicionalesSM"] = 0;
                                dr["RezagosSM"] = 0;
                                dr["RecargosSM"] = 0;
                                dr["HonorariosSM"] = 0;
                                dr["EjecucionSM"] = 0;
                                dr["MultasSM"] = 0;
                                dr["Limpieza"] = 0;
                                dr["DAP"] = 0;
                                dr["Recoleccion"] = 0;
                                dr["ImporteSM"] = 0;
                                dr["Error"] = dr["Error"].ToString() == "" ? "Calculo de SM Nulo" : dr["Error"].ToString() + ", " + "Calculo de SM Nulo";

                            }
                            else
                            {
                                if (servicio.Estado != null)
                                {
                                    if (recalcularMandamiento == "NO")
                                    {
                                        tRequerimiento r = new tRequerimientoBL().obtenerRequerimientoporIdPredio(id, "SM");
                                        if (r != null)
                                        {
                                            dr["Error"] = "";
                                            dr["SM"] = r.BimestreInicial.ToString() + " " + r.EjercicioInicial.ToString() + " / " + r.BimestreFinal.ToString() + " " + r.EjercicioFinal.ToString();
                                            dr["ImpuestoSM"] = 0;
                                            dr["AdicionalesSM"] = r.Adicional;
                                            dr["RezagosSM"] = r.Rezago;
                                            dr["RecargosSM"] = r.Recargo;
                                            //dr["HonorariosSM"] = r.Honorarios;
                                            //dr["EjecucionSM"] = r.Ejecucion;
                                            //dr["MultasSM"] = r.Multa;
                                            dr["Infraestructura"] = r.Impuesto;
                                            dr["Limpieza"] = r.Limpieza;
                                            dr["DAP"] = r.Dap;
                                            dr["Recoleccion"] = r.Recoleccion;
                                            dr["ImporteSM"] = r.Adicional + r.Rezago + r.Recargo + r.Impuesto + r.Limpieza + r.Recoleccion + r.Dap;
                                        }
                                    }
                                    else
                                    {
                                        dr["ImpuestoSM"] = servicio.Estado.Infraestructura > 0 ? servicio.Estado.Infraestructura : 0;
                                        dr["AdicionalesSM"] = servicio.Estado.Adicional > 0 ? servicio.Estado.Adicional : 0;
                                        dr["RezagosSM"] = servicio.Estado.Rezagos > 0 ? servicio.Estado.Rezagos : 0;
                                        dr["RecargosSM"] = servicio.Estado.Recargo > 0 ? servicio.Estado.Recargo : 0;
                                        dr["Limpieza"] = servicio.Estado.Limpieza > 0 ? servicio.Estado.Limpieza : 0;
                                        dr["DAP"] = servicio.Estado.Dap > 0 ? servicio.Estado.Dap : 0;
                                        dr["Recoleccion"] = servicio.Estado.Recoleccion > 0 ? servicio.Estado.Recoleccion : 0;
                                        dr["ImporteSM"] = servicio.Estado.Importe;
                                        dr["SM"] = servicio.Estado.PeriodoGral;
                                    }

                                    if (mesaSM != "")
                                    {
                                        #region honorarios     
                                        if (conHonorariosSM != null)
                                        {
                                            if (smReq == "SI")
                                                dr["HonorariosSM"] = 0;
                                            else
                                            {
                                                switch (conHonorariosSM.TipoCobro)
                                                {
                                                    case "I":
                                                        dr["HonorariosSM"] = Utileria.Redondeo(conHonorariosSM.Importe);
                                                        break;
                                                    case "P":
                                                        //dr["EjecuciónSM"] = 0;
                                                        dr["HonorariosSM"] = Utileria.Redondeo((servicio.Estado.Rezagos + servicio.Estado.Infraestructura + servicio.Estado.Limpieza +
                                                            servicio.Estado.Recoleccion + servicio.Estado.Dap + servicio.Estado.Adicional) * conHonorariosSM.Importe / 100);
                                                        break;
                                                    case "S":
                                                        //Decimal sm = Predial.cSalarioMinimo.Where(s => s.Ejercicio == DateTime.Today.Year && s.Activo == true).FirstOrDefault().Importe;
                                                        dr["HonorariosSM"] = Utileria.Redondeo(sm * conHonorariosSM.SalarioMin);
                                                        break;
                                                    default:
                                                        dr["HonorariosSM"] = 0;
                                                        break;
                                                }
                                            }
                                        }
                                        else
                                            dr["HonorariosSM"] = 0;

                                        #endregion

                                        #region  ejecucion
                                        if (conEjecucionSM != null)
                                        {
                                            if (smReq == "SI")
                                                dr["EjecucionSM"] = 0;
                                            else
                                            {
                                                switch (conEjecucionSM.TipoCobro)
                                                {
                                                    case "I":
                                                        dr["EjecucionSM"] = Utileria.Redondeo(conEjecucionSM.Importe);
                                                        break;
                                                    case "P":
                                                        //dr["EjecuciónSM"] = 0;
                                                        dr["EjecucionSM"] = Utileria.Redondeo((servicio.Estado.Rezagos + servicio.Estado.Infraestructura + servicio.Estado.Limpieza +
                                                                servicio.Estado.Recoleccion + servicio.Estado.Dap + servicio.Estado.Adicional) * conEjecucionSM.Importe / 100);
                                                        break;
                                                    case "S":
                                                        //Decimal sm = Predial.cSalarioMinimo.Where(s => s.Ejercicio == DateTime.Today.Year && s.Activo == true).FirstOrDefault().Importe;
                                                        dr["EjecucionSM"] = Utileria.Redondeo(sm * conEjecucionSM.SalarioMin);
                                                        break;
                                                    default:
                                                        dr["EjecucionSM"] = 0;
                                                        break;
                                                }
                                            }
                                        }
                                        else
                                            dr["EjecucionSM"] = 0;

                                        #endregion

                                        #region multas
                                        if (conMultaSM != null)
                                        {
                                            dr["MultasSM"] = 0;
                                            switch (conMultaSM.TipoCobro)
                                            {
                                                case "I":
                                                    dr["MultasSM"] = Utileria.Redondeo(conMultaSM.Importe);
                                                    break;
                                                case "P":
                                                    dr["MultasSM"] = Utileria.Redondeo((servicio.Estado.Rezagos + servicio.Estado.Infraestructura + servicio.Estado.Limpieza + servicio.Estado.Recargo +
                                                        servicio.Estado.Recoleccion + servicio.Estado.Dap + servicio.Estado.Adicional + Convert.ToDecimal(dr["EjecucionSM"]) + Convert.ToDecimal(dr["HonorariosSM"])) * conMultaSM.Importe / 100);
                                                    break;
                                                case "S":
                                                    //Decimal sm = Predial.cSalarioMinimo.Where(s => s.Ejercicio == DateTime.Today.Year && s.Activo == true).FirstOrDefault().Importe;
                                                    dr["MultasSM"] = Utileria.Redondeo(sm * conMultaSM.SalarioMin);
                                                    break;
                                                default:
                                                    dr["MultasSM"] = 0;
                                                    break;
                                            }
                                        }
                                        else
                                            dr["MultasSM"] = 0;

                                        #endregion
                                    }
                                    else
                                    {
                                        dr["HonorariosSM"] = 0;
                                        dr["EjecucionSM"] = 0;
                                        dr["MultasSM"] = 0;
                                    }
                                    dr["ImporteSM"] = servicio.Estado.Importe + Convert.ToDecimal(dr["MultasSM"]) + Convert.ToDecimal(dr["EjecucionSM"]) + Convert.ToDecimal(dr["HonorariosSM"]);

                                }
                                else
                                {
                                    dr["Error"] = dr["Error"].ToString() == "" ? servicio.TextError : dr["Error"].ToString() + ", " + servicio.TextError;
                                    dr["SM"] = "";
                                    dr["ImpuestoSM"] = 0;
                                    dr["AdicionalesSM"] = 0;
                                    dr["RezagosSM"] = 0;
                                    dr["RecargosSM"] = 0;
                                    dr["HonorariosSM"] = 0;
                                    dr["EjecucionSM"] = 0;
                                    dr["MultasSM"] = 0;
                                    dr["Infraestructura"] = 0;
                                    dr["Limpieza"] = 0;
                                    dr["DAP"] = 0;
                                    dr["Recoleccion"] = 0;
                                    dr["ImporteSM"] = 0;
                                    dr["Error"] = dr["Error"].ToString() == "" ? "Calculo de SM Nulo" : dr["Error"].ToString() + ", " + "Calculo de SM Nulo";
                                }
                            }
                        }
                        else 
                        {  ///   smReq == "NO"
                            dr["Error"] = "";
                            dr["SM"] = "";
                            dr["ImpuestoSM"] = 0;
                            dr["AdicionalesSM"] = 0;
                            dr["RezagosSM"] = 0;
                            dr["RecargosSM"] = 0;
                            dr["HonorariosSM"] = 0;
                            dr["EjecucionSM"] = 0;
                            dr["MultasSM"] = 0;
                            dr["Infraestructura"] = 0;
                            dr["Limpieza"] = 0;
                            dr["DAP"] = 0;
                            dr["Recoleccion"] = 0;
                            dr["ImporteSM"] = 0;                            
                        }         
                        #endregion

                        #region Impuesto
                        Impuesto impuesto = new SaldosC().CalculaCobro(id, param[1], obj.BimestreFinIp, obj.AaFinalIp, Int32.Parse(param[4]), Int32.Parse(param[5]), Int32.Parse(param[6]), 0,"Rezago");

                        if (impuesto.TextError != null)
                        {
                            dr["Error"] = dr["Error"].ToString() == "" ? impuesto.TextError : dr["Error"].ToString() + ", " + impuesto.TextError;
                        }
                        else
                        {
                            if (impuesto.Estado != null)
                            {
                               
                                if (recalcularMandamiento == "NO")
                                {
                                    tRequerimiento r = new tRequerimientoBL().obtenerRequerimientoporIdPredio(id, "IP");
                                    if (r != null)
                                    {
                                        dr["Error"] = "";
                                        dr["SM"] =
                                        dr["Periodo"] = r.BimestreInicial.ToString() + " " + r.EjercicioInicial.ToString() + " / " + r.BimestreFinal.ToString() + " " + r.EjercicioFinal.ToString();
                                        dr["Impuesto"] = r.Impuesto;
                                        dr["Adicionales"] = r.Adicional;
                                        dr["Rezagos"] = r.Rezago;
                                        dr["Recargos"] = r.Recargo;
                                        dr["Diferencias"] = r.Diferencia;
                                        dr["Rec"] = r.RecargoDiferencia;
                                        dr["Importe"] = r.Impuesto + r.Adicional + r.Rezago + r.Recargo + r.Diferencia + r.RecargoDiferencia;
                                        int bim = new SaldosC().CuentaBimestre(r.BimestreInicial, r.EjercicioInicial, r.BimestreFinal, r.EjercicioFinal, ref msg);
                                        dr["Bimestres"] = bim;
                                    }
                                    else
                                    {
                                        dr["Periodo"] = impuesto.Estado.PeriodoGral;
                                        dr["Impuesto"] = impuesto.Estado.Impuesto > 0 ? impuesto.Estado.Impuesto : 0;
                                        dr["Adicionales"] = impuesto.Estado.Adicional > 0 ? impuesto.Estado.Adicional : 0;
                                        dr["Rezagos"] = impuesto.Estado.Rezagos > 0 ? impuesto.Estado.Rezagos : 0;
                                        dr["Recargos"] = impuesto.Estado.Recargos > 0 ? impuesto.Estado.Recargos : 0;
                                        dr["Diferencias"] = impuesto.Estado.Diferencias > 0 ? impuesto.Estado.Diferencias : 0;
                                        dr["Rec"] = impuesto.Estado.RecDiferencias > 0 ? impuesto.Estado.RecDiferencias : 0;
                                        dr["Importe"] = impuesto.Estado.Importe > 0 ? impuesto.Estado.Importe : 0;
                                        dr["Bimestres"] = (impuesto.ActBimestre + impuesto.AntBimestre + impuesto.RezBimestre);
                                    }
                                }
                                else
                                {
                                    dr["Periodo"] = impuesto.Estado.PeriodoGral;
                                    dr["Impuesto"] = impuesto.Estado.Impuesto > 0 ? impuesto.Estado.Impuesto : 0;
                                    dr["Adicionales"] = impuesto.Estado.Adicional > 0 ? impuesto.Estado.Adicional : 0;
                                    dr["Rezagos"] = impuesto.Estado.Rezagos > 0 ? impuesto.Estado.Rezagos : 0;
                                    dr["Recargos"] = impuesto.Estado.Recargos > 0 ? impuesto.Estado.Recargos : 0;
                                    dr["Diferencias"] = impuesto.Estado.Diferencias > 0 ? impuesto.Estado.Diferencias : 0;
                                    dr["Rec"] = impuesto.Estado.RecDiferencias > 0 ? impuesto.Estado.RecDiferencias : 0;
                                    dr["Importe"] = impuesto.Estado.Importe > 0 ? impuesto.Estado.Importe : 0;
                                    dr["Bimestres"] = (impuesto.ActBimestre + impuesto.AntBimestre + impuesto.RezBimestre);
                                }


                                if (mesa != "")
                                {
                                    #region honorarios                           
                                    if (conHonorarios != null)
                                    {
                                        switch (conHonorarios.TipoCobro)
                                        {
                                            case "I":
                                                dr["Honorarios"] = Utileria.Redondeo(conHonorarios.Importe);
                                                break;
                                            case "P":
                                                dr["Honorarios"] = Utileria.Redondeo((impuesto.Estado.Rezagos + impuesto.Estado.Impuesto + impuesto.Estado.Diferencias + impuesto.Estado.Adicional + impuesto.Estado.Recargos) * conHonorarios.Importe / 100);
                                                break;
                                            case "S":

                                                dr["Honorarios"] = Utileria.Redondeo(sm * conHonorarios.SalarioMin);
                                                break;
                                            default:
                                                dr["Honorarios"] = 0;
                                                break;
                                        }
                                    }
                                    else
                                    //{
                                        dr["Honorarios"] = 0;
                                    //    dr["Error"] = dr["Error"].ToString() == "" ? "Clave HONORARIOSIP inexistente" : dr["Error"].ToString() + "\r\n" + "Clave HONORARIOS inexistente";
                                    //}
                                    #endregion

                                    #region ejecucion                           
                                    if (conEjecucion != null)
                                    {
                                        switch (conEjecucion.TipoCobro)
                                        {
                                            case "I":
                                                dr["Ejecucion"] = Utileria.Redondeo(conEjecucion.Importe);
                                                break;
                                            case "P":
                                                dr["Ejecucion"] = Utileria.Redondeo((impuesto.Estado.Rezagos + impuesto.Estado.Impuesto + impuesto.Estado.Diferencias + impuesto.Estado.Adicional+impuesto.Estado.Recargos) * conEjecucion.Importe / 100);
                                                break;
                                            case "S":                                                
                                                dr["Ejecucion"] = Utileria.Redondeo(sm * conEjecucion.SalarioMin);
                                                break;
                                            default:
                                                dr["Ejecucion"] = 0;
                                                break;
                                        }
                                    }
                                    else
                                    //{
                                        dr["Ejecucion"] = 0;
                                    //    dr["Error"] = dr["Error"].ToString() == "" ? "Clave EJECUCIONIP inexistente" : dr["Error"].ToString() + "\r\n" + "Clave EJECUCIONIP inexistente";
                                    //}
                                    #endregion

                                    //Int32 bimestre = Predial.cBaseGravable.Where(b => b.IdPredio == obj.Id && b.Activo == true && b.Ejercicio == DateTime.Today.Year).Max(b => b.Bimestre);
                                    //cBaseGravable Base = Predial.cBaseGravable.Where(b => b.IdPredio == obj.Id && b.Activo == true && b.Ejercicio == DateTime.Today.Year && b.Bimestre == bimestre).FirstOrDefault();
                                    cBaseGravable Base = Predial.cBaseGravable.Where(b => b.IdPredio == obj.Id && b.Activo == true && b.Ejercicio == DateTime.Today.Year).FirstOrDefault();
                                    dr["Base"] = Base != null ? Base.Valor : 0;

                                    if (Base == null)
                                    {
                                        dr["Error"] = dr["Error"].ToString() == "" ? "La Base Gravable no esta definido para el año " + DateTime.Today.Year.ToString() : dr["Error"].ToString() + ", " + "La Base Gravable no esta definido para el año " + DateTime.Today.Year.ToString();
                                    }
                                    #region multa
                                    if (conMulta != null)
                                    {
                                        switch (conMulta.TipoCobro)
                                        {
                                            case "I":
                                                dr["Multas"] = Utileria.Redondeo(conMulta.Importe);
                                                break;
                                            case "P":
                                                dr["Multas"] = Utileria.Redondeo((impuesto.Estado.Rezagos + impuesto.Estado.Impuesto + impuesto.Estado.Diferencias + impuesto.Estado.RecDiferencias +
                                                     impuesto.Estado.Recargos + impuesto.Estado.Adicional + Convert.ToDecimal(dr["Honorarios"]) + Convert.ToDecimal(dr["Ejecucion"])) * conMulta.Importe / 100);
                                                break;
                                            case "S":                                              
                                                dr["Multas"] = Utileria.Redondeo(sm * conMulta.SalarioMin);
                                                break;
                                            default:
                                                dr["Multas"] = 0;
                                                break;
                                        }
                                    }
                                    else
                                        dr["Multas"] = 0;
                                    //    dr["Error"] = dr["Error"].ToString() == "" ? "Clave MULTASIP inexistente" : dr["Error"].ToString() + ", " + "Clave MULTASIP inexistente";
                                    #endregion

                                }
                                else
                                {
                                    dr["Honorarios"] = 0;
                                    dr["Ejecucion"] = 0;
                                    dr["Multas"] = 0;
                                    dr["Error"] = dr["Error"].ToString() == "" ? "Mesa IP no configurada" : dr["Error"].ToString() + ", " + "Mesa IP no configurada";
                                }
                                dr["Importe"] = impuesto.Estado.Importe + Convert.ToDecimal(dr["Ejecucion"]) + Convert.ToDecimal(dr["Multas"])+ Convert.ToDecimal(dr["Honorarios"]);
                                //se suman los smen los impuestos

                            }
                            else
                            {
                                dr["Periodo"] = "";
                                dr["Impuesto"] = 0;
                                dr["Adicionales"] = 0;
                                dr["Rezagos"] = 0;
                                dr["Recargos"] = 0;
                                dr["Diferencias"] = 0;
                                dr["Rec"] = 0;
                                //(i.rezagos + i.impuesto + i.diferencias + i.recargos_diferencias + i.adicionales+ i.recargos + i.multas) * pocentaje_ejecucion  
                                dr["Honorarios"] = 0;
                                dr["Ejecucion"] = 0;
                                dr["Base"] = 0;
                                dr["Multas"] = 0;
                                dr["Importe"] = 0;
                                dr["Bimestres"] = 0;
                                dr["Error"] = dr["Error"].ToString() == "" ? "Calculo de IP Nulo" : dr["Error"].ToString() + "\n" + "Calculo de IP Nulo";
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    new Utileria().logError("tRrequerimietoBL.GenerarEdoCuenta.Exception", ex);
                    dr["ClavePredial"] = dr["ClavePredial"].ToString() == "" ? "Sin Clave Predial" : dr["ClavePredial"].ToString();
                    dr["Error"] = dr["Error"].ToString() == "" ? ex.ToString() : dr["Error"].ToString() + ", " + ex.ToString();
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public tRequerimiento obtenerRequerimientoporIdPredio(Int32 idPredio, String strTipoImpuesto)
        {
            tRequerimiento obj = null;
            List<tRequerimiento> objList = new List<tRequerimiento>();

            try
            {
                objList = Predial.tRequerimiento.Where(o => o.Status == "A" && o.Activo == true && o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto).ToList();                
                if (objList.Count() > 0)
                {                    
                    int idFase = objList.Max(o => o.TipoFase);
                    //int idFase = Predial.tRequerimiento.Where(o => o.Activo == true && o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto).Max(o => o.TipoFase);
                    obj = Predial.tRequerimiento.Where(o => o.Activo == true && o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto && o.TipoFase == idFase).OrderByDescending(t => t.Id).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.obtenerRequerimientoporIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio + ", strTipoImpuesto:" + strTipoImpuesto);
            }
            return obj;
        }

        public tRequerimiento obtenerRequerimientoIdPredio(Int32 idPredio, Int32 folio, String strTipoImpuesto)
        {
            tRequerimiento obj = null;
            List<tRequerimiento> objList = new List<tRequerimiento>();

            try
            {                
                objList = Predial.tRequerimiento.Where(o =>  o.IdPredio == idPredio && o.Folio==folio && o.TipoImpuesto == strTipoImpuesto).OrderByDescending(i => i.Id).ToList();                
                
                if (objList.Count() > 0)
                {
                    //int idFase = objList.Max(o => o.TipoFase);
                    //obj = objList.Where(o => o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto && o.TipoFase == idFase).FirstOrDefault();
                    obj = objList.Where(o => o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto ).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.obtenerRequerimientoIdPredio.Exception", ex, "--Parámetros idPredio:" + idPredio + ", strTipoImpuesto:" + strTipoImpuesto);
            }
            return obj;
        }

        public tRequerimiento obtenerRequerimientoIdPredioFecha(Int32 idPredio, Int32 folio, String strTipoImpuesto,DateTime fechaEmision)
        {
            tRequerimiento obj = null;
            List<tRequerimiento> objList = new List<tRequerimiento>();
           
            //string format = "MM/dd/yyyy HH:mm:ss";           
            try
            {
                objList = Predial.tRequerimiento.Where(o => o.IdPredio == idPredio && o.Folio == folio && o.TipoImpuesto == strTipoImpuesto && DbFunctions.CreateDateTime(o.FechaEmision.Year, o.FechaEmision.Month, o.FechaEmision.Day, o.FechaEmision.Hour, o.FechaEmision.Minute, o.FechaEmision.Second).Value == DbFunctions.CreateDateTime(fechaEmision.Year, fechaEmision.Month, fechaEmision.Day, fechaEmision.Hour, fechaEmision.Minute, fechaEmision.Second).Value).ToList();
                if (objList.Count() > 0)
                {
                    int idFase = objList.Max(o => o.TipoFase);
                    //int idFase = Predial.tRequerimiento.Where(o => o.Activo == true && o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto).Max(o => o.TipoFase);
                    // obj = Predial.tRequerimiento.Where(o => o.Activo == true && o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto && o.TipoFase == idFase).FirstOrDefault();
                    obj = objList.Where(o => o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto && o.TipoFase == idFase).FirstOrDefault();
                    //obj = objList.Where(o => o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto ).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.obtenerRequerimientoIdPredio.Exception", ex, "--Parámetros idPredio:" + idPredio + ", strTipoImpuesto:" + strTipoImpuesto);
            }
            return obj;
        }

        public tRequerimiento obtenerRequerimientoporIdPredioPagado(Int32 idPredio, String strTipoImpuesto)
        {
            tRequerimiento obj = null;
            List<tRequerimiento> objList = new List<tRequerimiento>();

            try
            {
                objList = Predial.tRequerimiento.Where(o => o.Status == "P" && o.Activo == true && o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto).ToList();
                if (objList.Count() > 0)
                {
                    int idFase = objList.Max(o => o.TipoFase);
                    //int idFase = Predial.tRequerimiento.Where(o => o.Activo == true && o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto).Max(o => o.TipoFase);
                    obj = Predial.tRequerimiento.Where(o => o.Activo == true && o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto && o.TipoFase == idFase).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.obtenerRequerimientoporIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio + ", strTipoImpuesto:" + strTipoImpuesto);
            }
            return obj;
        }


        public List<tRequerimiento> GetAllbyIdPredio(Int32 idPredio)
        {
            List<tRequerimiento> objList = new List<tRequerimiento>();
            try
            {
                objList = Predial.tRequerimiento.Where(o => o.IdPredio == idPredio).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.obtenerRequerimientoporIdPredio.Exception", ex, "--Parámetros idPredio:" + idPredio );
            }
            return objList;
        }
        
        public MensajesInterfaz CancelAnteriorPorPredioPagado(Int32 idPredio, String strTipoImpuesto, int id)
        {
            // tRequerimiento obj = null;
            MensajesInterfaz msg = new MensajesInterfaz();
            List<tRequerimiento> objList = new List<tRequerimiento>();
            msg = MensajesInterfaz.Actualizacion;

            try
            {
                objList = Predial.tRequerimiento.Where(o => o.Status == "A" && o.Activo == true && o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto && o.Id != id ).ToList();
                if (objList.Count() > 0)
                {
                    foreach(tRequerimiento obj in objList )
                    {
                        obj.Status = "C"; 
                        obj.FechaCancelacion = DateTime.Now;
                        obj.Activo = false;
                        msg = new tRequerimientoBL().Update(obj);
                        if (msg != MensajesInterfaz.Actualizacion)
                            msg = MensajesInterfaz.ErrorGuardar;
                    }
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.CancelAnteriorPorPredioPagado.Exception", ex, "--Parámetros idPredio:" + idPredio + ", strTipoImpuesto:" + strTipoImpuesto +" id: " + id);
            }
            return msg;
        }

        public Boolean validarRequerimientoActivo(Int32 idPredio, String strTipoImpuesto, DateTime fechaEmision)
        {
            List<tRequerimiento> objList = new List<tRequerimiento>();
            Boolean continua = true;
            try
            {
                objList = Predial.tRequerimiento.Where(o => o.Status == "A" && o.Activo == true && o.IdPredio == idPredio && o.TipoImpuesto == strTipoImpuesto && o.FechaEmision >= fechaEmision ).ToList();
                if (objList.Count() > 0)
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                continua = false;
                new Utileria().logError("tReciboDetalleBL.obtenerRequerimientoporIdPredio.Exception", ex, "--Parámetros idPredio:" + idPredio + ", strTipoImpuesto:" + strTipoImpuesto);
            }
            return continua;
        }

        public MensajesInterfaz CancelaRequerimiento(Int32 idPredio, int fase, int idUsuario, bool todos)
        {
            // tRequerimiento obj = null;
            MensajesInterfaz msg = new MensajesInterfaz();
            List<tRequerimiento> objList = new List<tRequerimiento>();
            msg = MensajesInterfaz.Actualizacion;

            try
            {
                if (todos == true)
                    objList = Predial.tRequerimiento.Where(o => o.Status != "C" && o.Activo == true  && o.IdPredio == idPredio).ToList();
                else
                    objList = Predial.tRequerimiento.Where(o => o.Status == "A" && o.Activo == true && o.TipoFase == fase && o.IdPredio == idPredio).ToList();

                if (objList.Count() > 0)
                {
                    foreach (tRequerimiento obj in objList)
                    {
                        obj.Status = "C";
                        obj.FechaCancelacion = DateTime.Now;
                        obj.IdUsuario = idUsuario;
                        msg = new tRequerimientoBL().Update(obj);
                        if (msg != MensajesInterfaz.Actualizacion)
                            msg = MensajesInterfaz.ErrorGuardar;
                    }
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.CancelaRequerimiento.Exception", ex, "--Parámetros idPredio:" + idPredio + ", IdTipoFase:" + fase);
            }
            return msg;
        }

        public MensajesInterfaz ActivaRequerimientoEnFaseSig(Int32 idPredio, int fase, int idUsuario)
        {
            // tRequerimiento obj = null;
            MensajesInterfaz msg = new MensajesInterfaz();
            List<tRequerimiento> objList = new List<tRequerimiento>();
            msg = MensajesInterfaz.Actualizacion;

            try
            {
                objList = Predial.tRequerimiento.Where(o => o.Status == "S" && o.Activo == true && o.TipoFase == fase && o.IdPredio == idPredio).ToList();
                if (objList.Count() > 0)
                {
                    foreach (tRequerimiento obj in objList)
                    {
                        obj.Status = "A";
                        obj.FechaCancelacion = DateTime.Now;
                        obj.IdUsuario = idUsuario;
                        msg = new tRequerimientoBL().Update(obj);
                        if (msg != MensajesInterfaz.Actualizacion)
                            msg = MensajesInterfaz.ErrorGuardar;
                    }
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.ActivaRequerimientoEnFaseSig.Exception", ex, "--Parámetros idPredio:" + idPredio + ", Status: F , Fase" + fase);
            }
            return msg;
        }

        public MensajesInterfaz CambiaRequerimientoAfaseSig(Int32 idPredio, int idFase, string tipoImpuesto, int idUsuario)
        {
            // tRequerimiento obj = null;
            MensajesInterfaz msg = new MensajesInterfaz();
            List<tRequerimiento> objList = new List<tRequerimiento>();
            msg = MensajesInterfaz.Actualizacion;

            try
            {
                objList = Predial.tRequerimiento.Where(o => o.Status == "A" && o.Activo == true && o.TipoFase == idFase && o.IdPredio == idPredio && o.TipoImpuesto == tipoImpuesto).ToList();
                if (objList.Count() > 0)
                {
                    foreach (tRequerimiento obj in objList)
                    {
                        obj.Status = "S";
                        obj.FechaModificacion = DateTime.Now;
                        obj.IdUsuario = idUsuario;
                        msg = new tRequerimientoBL().Update(obj);
                        if (msg != MensajesInterfaz.Actualizacion)
                            msg = MensajesInterfaz.ErrorGuardar;
                    }
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboDetalleBL.CambiaRequerimientoAfaseSig.Exception", ex, "--Parámetros idPredio:" + idPredio + " Fase" + idFase  + "TipoImpuesto " + tipoImpuesto + "idUsuario" + idUsuario);
            }
            return msg;
        }

    }

}
