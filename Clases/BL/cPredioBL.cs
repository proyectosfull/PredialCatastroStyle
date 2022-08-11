using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core;
using Clases;
using System.Data;
using Clases.Utilerias;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace Clases.BL
{
    /// <summary>
    /// 
    /// </summary>
    public class cPredioBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public cPredioBL()
        {
            Predial = new PredialEntities();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Insert(cPredio obj)
        {
            MensajesInterfaz Insert;
            try
            {
                Predial.cPredio.Add(obj);
                Predial.SaveChanges();
                Insert = MensajesInterfaz.Ingreso;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("cPredioBL.Insert.DbUpdateException", ex);
                Insert = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("cPredioBL.Insert.DataException", ex);
                Insert = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.Insert.Exception", ex);
                Insert = MensajesInterfaz.ErrorGeneral;
            }
            return Insert;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Update(cPredio obj)
        {
            MensajesInterfaz Update;
            try
            {
                cPredio objOld = Predial.cPredio.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.ClavePredial = obj.ClavePredial;
                objOld.Calle = obj.Calle;
                objOld.Numero = obj.Numero;
                objOld.IdColonia = obj.IdColonia;
                objOld.CP = obj.CP;
                objOld.Localidad = obj.Localidad;
                objOld.Referencia = obj.Referencia;
                objOld.SuperficieTerreno = obj.SuperficieTerreno;
                objOld.TerrenoPrivativo = obj.TerrenoPrivativo;
                objOld.TerrenoComun = obj.TerrenoComun;
                objOld.ValorTerreno = obj.ValorTerreno;
                objOld.SuperficieConstruccion = obj.SuperficieConstruccion;
                objOld.ConstruccionPrivativa = obj.ConstruccionPrivativa;
                objOld.ConstruccionComun = obj.ConstruccionComun;
                objOld.ValorConstruccion = obj.ValorConstruccion;
                objOld.ValorCatastral = obj.ValorCatastral;
                objOld.ValorComercial = obj.ValorComercial;
                objOld.ValorFiscal = obj.ValorFiscal;
                objOld.ValorOperacion = obj.ValorOperacion;
                objOld.FechaAlta = obj.FechaAlta;
                objOld.BaseTributacion = obj.BaseTributacion;
                objOld.FechaAvaluo = obj.FechaAvaluo;
                objOld.FechaTraslado = obj.FechaTraslado;
                objOld.Zona = obj.Zona;
                objOld.MetrosFrente = obj.MetrosFrente;
                objOld.IdUsoSuelo = obj.IdUsoSuelo;
                objOld.IdExentoPago = obj.IdExentoPago;
                objOld.IdStatusPredio = obj.IdStatusPredio;
                objOld.FechaBaja = obj.FechaBaja;
                objOld.IdTipoPredio = obj.IdTipoPredio;
                objOld.IdContribuyente = obj.IdContribuyente;
                objOld.IdTipoFaseIp = obj.IdTipoFaseIp;
                objOld.Nivel = obj.Nivel;
                objOld.UbicacionExpediente = obj.UbicacionExpediente;
                objOld.IdCartografia = obj.IdCartografia;
                objOld.BimestreFinIp = obj.BimestreFinIp;
                objOld.AaFinalIp = obj.AaFinalIp;
                objOld.ReciboIp = obj.ReciboIp;
                objOld.IdReciboIp = obj.IdReciboIp;
                objOld.FechaPagoIp = obj.FechaPagoIp;
                objOld.BimestreFinSm = obj.BimestreFinSm;
                objOld.AaFinalSm = obj.AaFinalSm;
                objOld.ReciboSm = obj.ReciboSm;
                objOld.IdReciboSm = obj.IdReciboSm;
                objOld.FechaPagoSm = obj.FechaPagoSm;
                objOld.IdTipoFaseSm = obj.IdTipoFaseSm;
                objOld.IdTipoMovAvaluo = obj.IdTipoMovAvaluo;
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                objOld.IdTipoMovAvaluo = obj.IdTipoMovAvaluo;
                objOld.IdCondominio = obj.IdCondominio;
                objOld.Observacion = obj.Observacion;
                List<cPredioObservacion> SpeParamToRemove = new List<cPredioObservacion>();
                foreach (cPredioObservacion param in objOld.cPredioObservacion)
                {
                    cPredioObservacion oldParam = obj.cPredioObservacion.FirstOrDefault(p => p.Id == param.Id && p.Observacion == param.Observacion);
                    if (oldParam == null)
                    {
                        SpeParamToRemove.Add(param);
                    }
                }
                foreach (cPredioObservacion param in obj.cPredioObservacion)
                {
                    cPredioObservacion newParam = objOld.cPredioObservacion.FirstOrDefault(p => p.Id == param.Id && p.Observacion == param.Observacion);
                    if (newParam == null)
                    {
                        objOld.cPredioObservacion.Add(param);
                    }
                }
                List<cContratoAgua> cContratoAguaToRemove = new List<cContratoAgua>();
                foreach (cContratoAgua param in objOld.cContratoAgua)
                {
                    cContratoAgua oldParam = obj.cContratoAgua.FirstOrDefault(p => p.Id == param.Id && p.NoContrato == param.NoContrato);
                    if (oldParam == null)
                    {
                        cContratoAguaToRemove.Add(param);
                    }
                }
                foreach (cContratoAgua param in obj.cContratoAgua)
                {
                    cContratoAgua newParam = objOld.cContratoAgua.FirstOrDefault(p => p.Id == param.Id && p.NoContrato == param.NoContrato);
                    if (newParam == null)
                    {
                        objOld.cContratoAgua.Add(param);
                    }
                }
                if ( SpeParamToRemove.Count() > 0 )
                    Predial.cPredioObservacion.RemoveRange(SpeParamToRemove);
                if (cContratoAguaToRemove.Count() > 0 )
                    Predial.cContratoAgua.RemoveRange(cContratoAguaToRemove);
                Predial.SaveChanges();
                Update = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("cPredioBL.Update.DbUpdateException", ex);
                Update = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("cPredioBL.Update.DataException", ex);
                Update = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.Update.Exception", ex);
                Update = MensajesInterfaz.ErrorGeneral;
            }
            return Update;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public cPredio GetByConstraint(int id)
        {
            cPredio obj = null;
            try
            {
                obj = Predial.cPredio.FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
            }
            return obj;
        }
        public cPredio GetPredioByIdContribuyente(int id)
        {
            cPredio obj = null;
            try
            {
                obj = Predial.cPredio.FirstOrDefault(o => o.IdContribuyente == id);
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.GetPredioByIdContribuyente.Exception", ex, "--Parámetros id:" + id);
            }
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Delete(cPredio obj)
        {
            MensajesInterfaz Delete;
            try
            {
                cPredio objOld = Predial.cPredio.FirstOrDefault(c => c.Id == obj.Id);
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Delete = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("cPredioBL.Delete.DbUpdateException", ex);
                Delete = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("cPredioBL.Delete.DataException", ex);
                Delete = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.Delete.Exception", ex);
                Delete = MensajesInterfaz.ErrorGeneral;
            }
            return Delete;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<cPredio> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<cPredio> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.cPredio.SqlQuery("Select Id,ClavePredial,Calle,Numero,IdColonia,CP,Localidad,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,FechaAlta,BaseTributacion,FechaAvaluo,FechaTraslado,Zona,MetrosFrente,IdUsoSuelo,IdExentoPago,IdStatusPredio,FechaBaja,IdTipoPredio,IdContribuyente,IdTipoFaseIp,Nivel,UbicacionExpediente,IdCartografia,BimestreFinIp,AaFinalIp,ReciboIp,IdReciboIp,FechaPagoIp,BimestreFinSm,AaFinalSm,ReciboSm,IdReciboSm,FechaPagoSm,IdTipoFaseSm,Activo,IdUsuario,FechaModificacion from cPredio where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.cPredio.SqlQuery("Select Id,ClavePredial,Calle,Numero,IdColonia,CP,Localidad,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,FechaAlta,BaseTributacion,FechaAvaluo,FechaTraslado,Zona,MetrosFrente,IdUsoSuelo,IdExentoPago,IdStatusPredio,FechaBaja,IdTipoPredio,IdContribuyente,IdTipoFaseIp,Nivel,UbicacionExpediente,IdCartografia,BimestreFinIp,AaFinalIp,ReciboIp,IdReciboIp,FechaPagoIp,BimestreFinSm,AaFinalSm,ReciboSm,IdReciboSm,FechaPagoSm,IdTipoFaseSm,Activo,IdUsuario,FechaModificacion from cPredio where activo=0 order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.cPredio.SqlQuery("Select Id,ClavePredial,Calle,Numero,IdColonia,CP,Localidad,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,FechaAlta,BaseTributacion,FechaAvaluo,FechaTraslado,Zona,MetrosFrente,IdUsoSuelo,IdExentoPago,IdStatusPredio,FechaBaja,IdTipoPredio,IdContribuyente,IdTipoFaseIp,Nivel,UbicacionExpediente,IdCartografia,BimestreFinIp,AaFinalIp,ReciboIp,IdReciboIp,FechaPagoIp,BimestreFinSm,AaFinalSm,ReciboSm,IdReciboSm,FechaPagoSm,IdTipoFaseSm,Activo,IdUsuario,FechaModificacion from cPredio where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.cPredio.SqlQuery("Select Id,ClavePredial,Calle,Numero,IdColonia,CP,Localidad,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,FechaAlta,BaseTributacion,FechaAvaluo,FechaTraslado,Zona,MetrosFrente,IdUsoSuelo,IdExentoPago,IdStatusPredio,FechaBaja,IdTipoPredio,IdContribuyente,IdTipoFaseIp,Nivel,UbicacionExpediente,IdCartografia,BimestreFinIp,AaFinalIp,ReciboIp,IdReciboIp,FechaPagoIp,BimestreFinSm,AaFinalSm,ReciboSm,IdReciboSm,FechaPagoSm,IdTipoFaseSm,Activo,IdUsuario,FechaModificacion from cPredio where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<cPredio> GetAll()
        {
            List<cPredio> objList = null;
            try
            {
                objList = Predial.cPredio.Where(o => o.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.GetAll.Exception", ex);
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
                cPredio pObject = new cPredio();
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
                new Utileria().logError("cPredioBL.ListaCampos.Exception", ex);
            }
            return propertyList;
        }
        public cPredio GetByClavePredial(string ClavePredial)
        {
            cPredio obj = new cPredio();
            try
            {
                obj = Predial.cPredio.FirstOrDefault(o => o.ClavePredial == ClavePredial && o.Activo==true);
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.GetByClavePredial.Exception", ex , "--Parámetros ClavePredial:" + ClavePredial);
            }
            return obj;
        }

        public cPredio GetByCuentaPredial(string CuentaPredial)
        {
            cPredio obj = new cPredio();
            try
            {
                obj = Predial.cPredio.FirstOrDefault(o => o.ClaveAnterior == CuentaPredial && o.Activo == true);
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.GetByClavePredial.Exception", ex, "--Parámetros ClavePredial:" + CuentaPredial);
            }
            return obj;
        }

        /// <summary>
        /// Obtiene la lista de claves a las cuales se le generará su estado de cuenta
        /// </summary>
        /// <param name="strParam">
        /// 0-Año Periodo, 1-Bimestre Periodo, 
        /// 2-Ignorar Baldíos
        /// 3-Solo Exento
        /// 4-Ignorar Exento
        /// 5-Baldios
        /// 6-Rango Incial, 7-Rango Final
        /// 8-Contribuyente
        /// 9-Claves</param>
        /// <returns></returns>
        /// 
        public DataTable ObtenerPrediosRequerimientos(String[] strParam)
        {
            List<cPredio> obj = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("ClavePredial");
            dt.Columns.Add("Tipo");
            dt.Columns.Add("Periodo");
            dt.Columns.Add("IdPredio");
            dt.Columns.Add("IdTipoPredio");
            try
            {
                int anio = DateTime.Today.Year;
                Int32 bimestre = 0;
                //obj = Predial.cPredio.Where(p => p.AaFinalIp < anio).ToList();                                
                //if (strParam[2].ToString().ToUpper() != "FALSE")
                //    obj = obj.Where(p => p.cTipoPredio.Descripcion != "BALDIO").ToList();
                //if (strParam[3].ToString().ToUpper() != "FALSE")
                //    obj = obj.Where(p => p.cExentoPago.Descripcion == "EXENTO").ToList();
                //if (strParam[4].ToString().ToUpper() != "FALSE")
                //    obj = obj.Where(p => p.cExentoPago.Descripcion != "EXENTO").ToList();
                //if (strParam[5].ToString().ToUpper() != "FALSE")
                //    obj = obj.Where(p => p.cTipoPredio.Descripcion == "BALDIO").ToList();
                //if (strParam[6].ToString()!="" && strParam[7].ToString() != "")
                //{
                //    Int64 rangoini = Convert.ToInt64(strParam[6].ToString()), rangofin = Convert.ToInt64(strParam[7].ToString());
                //    obj = obj.Where(p => Convert.ToInt64(p.ClavePredial) >= rangoini && Convert.ToInt64(p.ClavePredial)<=rangofin).ToList();
                //}
                //if (strParam[8].ToString().ToUpper() != "")
                //{
                //    Int32 idContribuyente = Convert.ToInt32(strParam[8].ToString());
                //    obj = obj.Where(p => p.IdContribuyente == idContribuyente).ToList();
                //}
                //if (strParam[9].ToString().ToUpper() != "")
                //{
                //    String[] claves = strParam[9].Split(',');
                //    obj = obj.Where(p => claves.Contains(p.ClavePredial)).ToList();
                //}
                //foreach (cPredio predio in obj)
                //{
                //    DataRow row = dt.NewRow();
                //    row["IdPredio"] = predio.Id.ToString();
                //    row["IdTipoPredio"] = predio.IdTipoPredio.ToString();
                //    row["ClavePredial"] = predio.ClavePredial;
                //    row["Tipo"] = "Impuesto Predial";
                //    row["Periodo"] = predio.AaFinalIp.ToString()+" - "+predio.BimestreFinIp.ToString();
                //    dt.Rows.Add(row);
                //}      

                bimestre = Convert.ToInt32(strParam[0]) * 10 + Convert.ToInt32(strParam[1]);
                if (strParam[6].ToString() != "" && strParam[7].ToString() != "")
                {
                    obj = Predial.cPredio.SqlQuery("Select * from cPredio where isnumeric(ClavePredial)=1 And Activo=1 And (AaFinalIp != 0 And AaFinalIp * 10 + BimestreFinIp < " + bimestre + ") ").ToList();
                    //obj = Predial.cPredio.SqlQuery("Select * from cPredio where isnumeric(ClavePredial)=1 And Activo=1 And ((AaFinalSm * 10 + BimestreFinSm <= " + bimestre + " And AaFinalSm != 0) Or (AaFinalIp != 0 And AaFinalIp * 10 + BimestreFinIp <= " + bimestre + ")) ").ToList();
                    Int64 rangoini = Convert.ToInt64(strParam[6].ToString()), rangofin = Convert.ToInt64(strParam[7].ToString());
                    obj = obj.Where(p => (p.cStatusPredio.Descripcion == "A" || p.cStatusPredio.Descripcion == "S") && Convert.ToInt64(p.ClavePredial.Trim().Replace(" ", "")) >= rangoini && Convert.ToInt64(p.ClavePredial.Trim().Replace(" ", "")) <= rangofin).ToList();
                }
                else
                    obj = Predial.cPredio.Where(p => ( (p.AaFinalIp != 0 && p.AaFinalIp * 10 + p.BimestreFinIp < bimestre)) && p.Activo == true && (p.cStatusPredio.Descripcion == "A" || p.cStatusPredio.Descripcion == "S")).ToList();
                    //obj = Predial.cPredio.Where(p => ((p.AaFinalSm * 10 + p.BimestreFinSm <= bimestre && p.AaFinalSm != 0) || (p.AaFinalIp != 0 && p.AaFinalIp * 10 + p.BimestreFinIp < bimestre)) && p.Activo == true && (p.cStatusPredio.Descripcion == "A" || p.cStatusPredio.Descripcion == "S")).ToList();
                
                //obj = Predial.cPredio.Where(p => p.AaFinalIp * 10 + p.BimestreFinIp <= bimestre).ToList();

                //obj = Predial.cPredio.Where(p => (p.AaFinalSm < anio || p.AaFinalIp < anio) && p.Activo == true && (p.cStatusPredio.Descripcion == "A" || p.cStatusPredio.Descripcion == "S")).ToList();
                //if (strParam[0].ToString() != "0" && strParam[1].ToString() != "0")
                //{
                //    bimestre = Convert.ToInt32(strParam[0] + strParam[1]);
                //    obj = obj.Where(p => (Convert.ToInt32(p.AaFinalSm.ToString() + p.BimestreFinSm.ToString()) <= bimestre && p.AaFinalSm != 0) || (p.AaFinalIp != 0 && Convert.ToInt32(p.AaFinalIp.ToString() + p.BimestreFinIp.ToString()) <= bimestre)).ToList();
                //}
                if (strParam[2].ToString().ToUpper() != "FALSE")
                    obj = obj.Where(p => p.cTipoPredio.Descripcion != "BALDIO").ToList();
                if (strParam[3].ToString().ToUpper() != "FALSE")
                    obj = obj.Where(p => p.cExentoPago.Descripcion == "EXENTO").ToList();
                if (strParam[4].ToString().ToUpper() != "FALSE")
                    obj = obj.Where(p => p.cExentoPago.Descripcion != "EXENTO").ToList();
                if (strParam[5].ToString().ToUpper() != "FALSE")
                    obj = obj.Where(p => p.cTipoPredio.Descripcion == "BALDIO").ToList();
                //if (strParam[6].ToString() != "" && strParam[7].ToString() != "")
                //{
                //    Int64 rangoini = Convert.ToInt64(strParam[6].ToString()), rangofin = Convert.ToInt64(strParam[7].ToString());
                //    obj = obj.Where(p => Convert.ToInt64(p.ClavePredial.Trim().Replace(" ", "")) >= rangoini && Convert.ToInt64(p.ClavePredial.Trim().Replace(" ", "")) <= rangofin).ToList();
                   
                //}
                if (strParam[8].ToString().ToUpper() != "")
                {
                    Int32 idContribuyente = Convert.ToInt32(strParam[8].ToString());
                    obj = obj.Where(p => p.IdContribuyente == idContribuyente).ToList();
                }
                if (strParam[9].ToString().ToUpper() != "")
                {
                    String[] claves = strParam[9].Split(',');
                    obj = obj.Where(p => claves.Contains(p.ClavePredial.Replace(" ", ""))).ToList();
                }
                if (strParam[10].ToString() != "0")
                {
                    Int32 idDocumento = Convert.ToInt32(strParam[10].ToString());
                    obj = obj.Where(p => p.IdTipoFaseSm == idDocumento || p.IdTipoFaseIp == idDocumento).ToList();
                }
                if (strParam[11].ToString() != "")
                {
                    Int32 idCondominio = Convert.ToInt32(strParam[11].ToString());
                    obj = obj.Where(p => p.IdCondominio == idCondominio).ToList();
                }
                if (strParam[12].ToString() != "")
                {
                    Int32 idColonia = Convert.ToInt32(strParam[12].ToString());
                    obj = obj.Where(p => p.IdColonia == idColonia).ToList();
                }
                foreach (cPredio predio in obj)
                {
                    DataRow row = dt.NewRow();
                    row["IdPredio"] = predio.Id.ToString();
                    row["IdTipoPredio"] = predio.IdTipoPredio.ToString();
                    row["ClavePredial"] = predio.ClavePredial;
                    row["Tipo"] = "Impuesto Predial";
                    row["Periodo"] = predio.AaFinalIp.ToString() == "0" ? predio.AaFinalSm.ToString() + " - " + predio.BimestreFinSm.ToString() : predio.AaFinalIp.ToString() + " - " + predio.BimestreFinIp.ToString();
                    dt.Rows.Add(row);
                }
                if (dt.Rows.Count == 0)
                {
                    DataRow row = dt.NewRow();
                    row["IdPredio"] = "";
                    row["IdTipoPredio"] = "";
                    row["ClavePredial"] = "";
                    row["Tipo"] = "";
                    row["Periodo"] = "";
                    dt.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.ObtenerPrediosRequerimientos.Exception", ex , "--Parámetros strParam:" + strParam);
                DataRow row = dt.NewRow();
                row["ClavePredial"] = "";
                row["Tipo"] = "";
                row["Periodo"] = "";
                dt.Rows.Add(row);
            }
            return dt;
        }

        public DataTable ObtenerPrediosRezago(String[] strParam)
        {
            List<cPredio> obj = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("ClavePredial");
            dt.Columns.Add("Tipo");
            dt.Columns.Add("Periodo");
            dt.Columns.Add("IdPredio");
            dt.Columns.Add("IdTipoPredio");
            try
            {
                int anio = DateTime.Today.Year;
                Int32 bimestre = 0;
                

                bimestre = Convert.ToInt32(strParam[0]) * 10 + Convert.ToInt32(strParam[1]);
                if (strParam[6].ToString() != "" && strParam[7].ToString() != "")
                {
                    obj = Predial.cPredio.SqlQuery("Select * from cPredio where isnumeric(ClavePredial)=1 And Activo=1 And ((AaFinalSm * 10 + BimestreFinSm < " + bimestre + " And AaFinalSm != 0) Or (AaFinalIp != 0 And AaFinalIp * 10 + BimestreFinIp < " + bimestre + ")) ").ToList();
                    obj = obj.Where(p => (p.cStatusPredio.Descripcion == "A" || p.cStatusPredio.Descripcion == "S")).ToList();
                    Int64 rangoini = Convert.ToInt64(strParam[6].ToString()), rangofin = Convert.ToInt64(strParam[7].ToString());
                     obj = obj.Where(p => Convert.ToInt64(p.ClavePredial.Trim().Replace(" ", "")) >= rangoini && Convert.ToInt64(p.ClavePredial.Trim().Replace(" ", "")) <= rangofin).ToList();

                }
                else
                    obj = Predial.cPredio.Where(p => ((p.AaFinalSm * 10 + p.BimestreFinSm <= bimestre && p.AaFinalSm != 0) || (p.AaFinalIp != 0 && p.AaFinalIp * 10 + p.BimestreFinIp <= bimestre)) && p.Activo == true && (p.cStatusPredio.Descripcion == "A" || p.cStatusPredio.Descripcion == "S")).ToList();
                
                if (strParam[2].ToString().ToUpper() != "FALSE")
                    obj = obj.Where(p => p.cTipoPredio.Descripcion != "BALDIO").ToList();
                if (strParam[3].ToString().ToUpper() != "FALSE")
                    obj = obj.Where(p => p.cExentoPago.Descripcion == "EXENTO").ToList();
                if (strParam[4].ToString().ToUpper() != "FALSE")
                    obj = obj.Where(p => p.cExentoPago.Descripcion != "EXENTO").ToList();
                if (strParam[5].ToString().ToUpper() != "FALSE")
                    obj = obj.Where(p => p.cTipoPredio.Descripcion == "BALDIO").ToList();
                //if (strParam[6].ToString() != "" && strParam[7].ToString() != "")
                //{
                //    Int64 rangoini = Convert.ToInt64(strParam[6].ToString()), rangofin = Convert.ToInt64(strParam[7].ToString());
                //    obj = obj.Where(p => Convert.ToInt64(p.ClavePredial.Trim().Replace(" ", "")) >= rangoini && Convert.ToInt64(p.ClavePredial.Trim().Replace(" ", "")) <= rangofin).ToList();

                //}
                if (strParam[8].ToString().ToUpper() != "")
                {
                    Int32 idContribuyente = Convert.ToInt32(strParam[8].ToString());
                    obj = obj.Where(p => p.IdContribuyente == idContribuyente).ToList();
                }
                if (strParam[9].ToString().ToUpper() != "")
                {
                    String[] claves = strParam[9].Split(',');
                    obj = obj.Where(p => claves.Contains(p.ClavePredial.Replace(" ", ""))).ToList();
                }
                if (strParam[10].ToString() != "0")
                {
                    Int32 idDocumento = Convert.ToInt32(strParam[10].ToString());
                    obj = obj.Where(p => p.IdTipoFaseSm == idDocumento || p.IdTipoFaseIp == idDocumento).ToList();
                }
                foreach (cPredio predio in obj)
                {
                    DataRow row = dt.NewRow();
                    row["IdPredio"] = predio.Id.ToString();
                    row["IdTipoPredio"] = predio.IdTipoPredio.ToString();
                    row["ClavePredial"] = predio.ClavePredial;
                    row["Tipo"] = "Impuesto Predial";
                    row["Periodo"] = predio.AaFinalIp != 0 ? predio.AaFinalIp.ToString() + " - " + predio.BimestreFinIp.ToString(): "0 - 0";
                    dt.Rows.Add(row);
                }
                if (dt.Rows.Count == 0)
                {
                    DataRow row = dt.NewRow();
                    row["IdPredio"] = "";
                    row["IdTipoPredio"] = "";
                    row["ClavePredial"] = "";
                    row["Tipo"] = "";
                    row["Periodo"] = "";
                    dt.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.ObtenerPrediosRequerimientos.Exception", ex , "--Parámetros strParam:" + strParam);
                DataRow row = dt.NewRow();
                row["ClavePredial"] = "";
                row["Tipo"] = "";
                row["Periodo"] = "";
                dt.Rows.Add(row);
            }
            return dt;
        }

        public List<cPredio> ObtenerPrediosCalculoRezago(int ejFinal, int bimFinal, bool sinBaldio, bool soloExento, bool sinExento, bool soloBaldio, Int64 rangoDe, Int64 rangoA, int idContribuyente, string claves, int idFase, int idCondominio, int idColonia)
        {
            List<cPredio> obj = null;
            
            try
            {
                int anio = DateTime.Today.Year;
                Int32 bimestre = 0;

                string incluirSM = new cParametroSistemaBL().GetValorByClave("SMENREQUERIMIENTO");         

                bimestre = ejFinal* 10 + bimFinal;
                if (rangoDe > 0 && rangoA > 0)
                {
                    if (incluirSM == "SI")
                        obj = Predial.cPredio.SqlQuery("Select * from cPredio where isnumeric(ClavePredial)=1 And Activo=1 And ((AaFinalSm * 10 + BimestreFinSm < " + bimestre + " And AaFinalSm != 0) Or (AaFinalIp != 0 And AaFinalIp * 10 + BimestreFinIp < " + bimestre + ")) ").ToList();
                    else
                        obj = Predial.cPredio.SqlQuery("Select * from cPredio where isnumeric(ClavePredial)=1 And Activo=1 AND (AaFinalIp != 0 And AaFinalIp * 10 + BimestreFinIp < " + bimestre + ")").ToList();
                    
                    obj = obj.Where(p => (p.cStatusPredio.Descripcion == "A" || p.cStatusPredio.Descripcion == "S")).ToList();                    
                    obj = obj.Where(p => Convert.ToInt64(p.ClavePredial.Trim().Replace(" ", "")) >= rangoDe && Convert.ToInt64(p.ClavePredial.Trim().Replace(" ", "")) <= rangoA).ToList();
                }
                else
                {
                    if (incluirSM == "SI")
                        obj = Predial.cPredio.Where(p => ((p.AaFinalSm * 10 + p.BimestreFinSm <= bimestre && p.AaFinalSm != 0) || (p.AaFinalIp != 0 && p.AaFinalIp * 10 + p.BimestreFinIp <= bimestre)) && p.Activo == true && (p.cStatusPredio.Descripcion == "A" || p.cStatusPredio.Descripcion == "S")).ToList();
                    else
                        obj = Predial.cPredio.Where(p =>  p.AaFinalIp != 0 && p.AaFinalIp * 10 + p.BimestreFinIp <= bimestre  && p.Activo == true && (p.cStatusPredio.Descripcion == "A" || p.cStatusPredio.Descripcion == "S")).ToList();
                    
                }
                    

                if ( sinBaldio == true)
                    obj = obj.Where(p => p.cTipoPredio.Descripcion != "BALDIO").ToList();
                if (soloExento == true)
                    obj = obj.Where(p => p.cExentoPago.Descripcion == "EXENTO").ToList();
                if (sinExento == true)
                    obj = obj.Where(p => p.cExentoPago.Descripcion != "EXENTO").ToList();
                if (soloBaldio == true)
                    obj = obj.Where(p => p.cTipoPredio.Descripcion == "BALDIO").ToList();
                
                if (idContribuyente > 0)
                    obj = obj.Where(p => p.IdContribuyente == idContribuyente).ToList();
               
                if (claves != "") 
                {
                    string clav = new Utileria().RemoverCaracteresEspeciales(claves);
                   // String[] lclaves = claves.Split(',');
                    obj = obj.Where(p => clav.Contains(p.ClavePredial.Replace(" ", ""))).ToList();
                }
                if (idFase > 0)
                {                    
                    obj = obj.Where(p => p.IdTipoFaseSm == idFase || p.IdTipoFaseIp == idFase).ToList();
                }
                if (idCondominio > 0)                
                    obj = obj.Where(p => p.IdCondominio == idCondominio).ToList();
               
                if (idColonia > 0)               
                    obj = obj.Where(p => p.IdColonia == idColonia).ToList();
               

            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.ObtenerPrediosRequerimientos.Exception", ex, "--Parámetros:" + ejFinal +" "+ bimFinal + " " + sinBaldio + " " + soloExento + " " + 
                    sinExento + " " + soloBaldio + " " + rangoDe + " " + rangoA + " " + idContribuyente + " " + claves + " " + idFase + " " + idCondominio + " " + idColonia);
                obj = null;
            }
            return obj;
        }

        public List<cPredio> GetByCondominio(int idcon)
        {
            List<cPredio> obj = new List<cPredio>();
            try
            {
                obj = Predial.cPredio.Where(o => o.IdCondominio == idcon).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.GetByClavePredial.Exception", ex, "--Parámetros Id:" + idcon);
            }
            return obj;
        }
        public List<cPredio> GetByColonia(int idcol)
        {
            List<cPredio> obj = new List<cPredio>();
            try
            {
                obj = Predial.cPredio.Where(o => o.IdColonia == idcol).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cPredioBL.GetByClavePredial.Exception", ex, "--Parámetros IdColonia:" + idcol);
            }
            return obj;
        }
    }

}
