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
    public class tTramiteBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public tTramiteBL()
        {
            Predial = new PredialEntities();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Insert(tTramite obj)
        {
            string path_Name_File = System.Web.HttpContext.Current.Server.MapPath("~/") + "/Log/" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".txt";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path_Name_File, true);
            file.WriteLine("**************************");
            file.WriteLine("Fecha Hora de la insercion del recibo: " + DateTime.Now.ToString());
            file.WriteLine("IdPredio=" + obj.IdPredio);
            file.WriteLine("Periodo  =" + obj.Periodo);
            file.WriteLine("IdTipoTramite=" + obj.IdTipoTramite);
            file.WriteLine("tipo=" + obj.Tipo);
            file.WriteLine("**************************");
            file.Close();
            MensajesInterfaz Insert;
            try
            {
                Predial.tTramite.Add(obj);
                Predial.SaveChanges();
                Insert = MensajesInterfaz.Ingreso;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tTramiteBL.Insert.DbUpdateException", ex);
                Insert = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tTramiteBL.Insert.DataException", ex);
                Insert = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.Insert.Exception", ex);
                Insert = MensajesInterfaz.ErrorGeneral;
            }
            return Insert;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Update(tTramite obj)
        {
            MensajesInterfaz Update;
            try
            {
                tTramite objOld = Predial.tTramite.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdPredio = obj.IdPredio;
                objOld.IdConvenio = obj.IdConvenio;
                objOld.Fecha = obj.Fecha;
                objOld.Status = obj.Status;
                objOld.BaseGravable = obj.BaseGravable;
                objOld.BimestreInicial = obj.BimestreInicial;
                objOld.EjercicioInicial = obj.EjercicioInicial;
                objOld.BimestreFinal = obj.BimestreFinal;
                objOld.EjercicioFinal = obj.EjercicioFinal;
                objOld.Tipo = obj.Tipo;
                objOld.Adeudo = obj.Adeudo;
                objOld.Periodo = obj.Periodo;
                objOld.SuperficieTerreno = obj.SuperficieTerreno;
                objOld.TerrenoPrivativo = obj.TerrenoPrivativo;
                objOld.TerrenoComun = obj.TerrenoComun;
                objOld.SuperficieConstruccion = obj.SuperficieConstruccion;
                objOld.ConstruccionPrivativa = obj.ConstruccionPrivativa;
                objOld.ConstruccionComun = obj.ConstruccionComun;
                objOld.FechaEstimado = obj.FechaEstimado;
                objOld.NombreAdquiriente = obj.NombreAdquiriente;
                objOld.IdTipoAvaluo = obj.IdTipoAvaluo;
                objOld.FechaOperacion = obj.FechaOperacion;
                objOld.NumeroEscritura = obj.NumeroEscritura;
                objOld.ValorMasAlto = obj.ValorMasAlto;
                objOld.PorMinimos = obj.PorMinimos;
                objOld.NoSalarioMinimo = obj.NoSalarioMinimo;
                objOld.IdValuador = obj.IdValuador;
                objOld.Notaria = obj.Notaria;
                objOld.IdTipoTramite = obj.IdTipoTramite;
                objOld.Observacion = obj.Observacion;
                objOld.IdDiferencia = obj.IdDiferencia;
                objOld.IdRequerimiento = obj.IdRequerimiento;
                objOld.NoParcialidad = obj.NoParcialidad;
                objOld.Mensualidad = obj.Mensualidad;
                objOld.Interes = obj.Interes;
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                objOld.IsabiForaneo = obj.IsabiForaneo;
                objOld.valorOperacion = obj.valorOperacion;
                objOld.valorComercial = obj.valorComercial;
                Predial.SaveChanges();
                Update = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tTramiteBL.Update.DbUpdateException", ex);
                Update = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tTramiteBL.Update.DataException", ex);
                Update = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.Update.Exception", ex);
                Update = MensajesInterfaz.ErrorGeneral;
            }
            return Update;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tTramite GetByConstraint(int id)
        {
            tTramite obj = null;
            try
            {
                obj = Predial.tTramite.FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
            }
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tTramite GettTramiteByPredio(int idP, int idtT)
        {
            tTramite obj = null;
            try
            {
                obj = Predial.tTramite.SqlQuery("SELECT Id,IdPredio,IdConvenio,Fecha,Status,BaseGravable,BimestreInicial,EjercicioInicial,BimestreFinal,EjercicioFinal,Tipo,Adeudo,Periodo,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,FechaEstimado,NombreAdquiriente,IdTipoAvaluo,FechaOperacion,NumeroEscritura,ValorMasAlto,PorMinimos,NoSalarioMinimo,IdValuador,Notaria,IdTipoTramite,Observacion,IdDiferencia,IdRequerimiento,NoParcialidad,Mensualidad,Interes,Activo,IdUsuario,FechaModificacion,IsabiForaneo,valorComercial,valorOperacion from tTramite where activo=1 AND IdPredio = " + idP + " AND IdTipoTramite = " + idtT + " order by Id desc").FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.GetByConstraint.Exception", ex , "--Parámetros idP:" + idP + ", idtT:" + idtT);
            }
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Delete(tTramite obj)
        {
            MensajesInterfaz Delete;
            try
            {
                tTramite objOld = Predial.tTramite.FirstOrDefault(c => c.Id == obj.Id);
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Delete = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tTramiteBL.Delete.DbUpdateException", ex);
                Delete = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tTramiteBL.Delete.DataException", ex);
                Delete = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.Delete.Exception", ex);
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
        public List<tTramite> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<tTramite> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.tTramite.SqlQuery("SELECT Id,IdPredio,IdConvenio,Fecha,Status,BaseGravable,BimestreInicial,EjercicioInicial,BimestreFinal,EjercicioFinal,Tipo,Adeudo,Periodo,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,FechaEstimado,NombreAdquiriente,IdTipoAvaluo,FechaOperacion,NumeroEscritura,ValorMasAlto,PorMinimos,NoSalarioMinimo,IdValuador,Notaria,IdTipoTramite,Observacion,IdDiferencia,IdRequerimiento,NoParcialidad,Mensualidad,Interes,Activo,IdUsuario,FechaModificacion,IsabiForaneo,valorComercial,valorOperacion FROM tTramite where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.tTramite.SqlQuery("SELECT Id,IdPredio,IdConvenio,Fecha,Status,BaseGravable,BimestreInicial,EjercicioInicial,BimestreFinal,EjercicioFinal,Tipo,Adeudo,Periodo,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,FechaEstimado,NombreAdquiriente,IdTipoAvaluo,FechaOperacion,NumeroEscritura,ValorMasAlto,PorMinimos,NoSalarioMinimo,IdValuador,Notaria,IdTipoTramite,Observacion,IdDiferencia,IdRequerimiento,NoParcialidad,Mensualidad,Interes,Activo,IdUsuario,FechaModificacion,IsabiForaneo,valorComercial,valorOperacion FROM tTramite where activo=0 order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    if (campoFiltro != "IdConvenio")
                        valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.tTramite.SqlQuery("SELECT Id,IdPredio,IdConvenio,Fecha,Status,BaseGravable,BimestreInicial,EjercicioInicial,BimestreFinal,EjercicioFinal,Tipo,Adeudo,Periodo,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,FechaEstimado,NombreAdquiriente,IdTipoAvaluo,FechaOperacion,NumeroEscritura,ValorMasAlto,PorMinimos,NoSalarioMinimo,IdValuador,Notaria,IdTipoTramite,Observacion,IdDiferencia,IdRequerimiento,NoParcialidad,Mensualidad,Interes,Activo,IdUsuario,FechaModificacion,IsabiForaneo,valorComercial,valorOperacion FROM tTramite where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.tTramite.SqlQuery("SELECT Id,IdPredio,IdConvenio,Fecha,Status,BaseGravable,BimestreInicial,EjercicioInicial,BimestreFinal,EjercicioFinal,Tipo,Adeudo,Periodo,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,FechaEstimado,NombreAdquiriente,IdTipoAvaluo,FechaOperacion,NumeroEscritura,ValorMasAlto,PorMinimos,NoSalarioMinimo,IdValuador,Notaria,IdTipoTramite,Observacion,IdDiferencia,IdRequerimiento,NoParcialidad,Mensualidad,Interes,Activo,IdUsuario,FechaModificacion,IsabiForaneo,valorComercial,valorOperacion FROM tTramite where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<tTramite> GetAll()
        {
            List<tTramite> objList = null;
            try
            {
                objList = Predial.tTramite.Where(o => o.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.GetAll.Exception", ex);
            }
            return objList;
        }

        public List<tTramite> GetTiposTramiteIdPredio(int idPredio, int idTipoTramite, String status)
        {
            List<tTramite> objList = null;
            try
            {
                objList = Predial.tTramite.Where(o => o.IdPredio == idPredio && o.IdTipoTramite == idTipoTramite && o.Status == status && o.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.GetTiposTramiteIdPredio.Exception", ex , "--Parámetros iidPrediod:" + idPredio + ", idTipoTramite:" + idTipoTramite + ", status:" + status);
            }
            return objList;
        }

        public List<tTramite> GetTiposTramiteIdPredioPlano(int idPredio, int idTipoTramite, String status)
        { 
            List<tTramite> objList = null;
            DateTime valida = DateTime.Today.Date.AddDays(-31);
            
            try
            {
                objList = Predial.tTramite.Where(o => o.IdPredio == idPredio && o.IdTipoTramite == idTipoTramite && o.Status == status && o.Fecha > valida && o.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.GetTiposTramiteIdPredio.Exception", ex, "--Parámetros iidPrediod:" + idPredio + ", idTipoTramite:" + idTipoTramite + ", status:" + status);
            }
            return objList;
        }

        public MensajesInterfaz cancelarCobroOtrosConceptos(int? idTramite, tRecibo r)
        {
            throw new NotImplementedException();
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
                tTramite pObject = new tTramite();
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
                new Utileria().logError("tTramiteBL.ListaCampos.Exception", ex);
            }
            
            return propertyList;
        }

        public tTramite GetByNoPago(int idConvenio, int noPago)
        {
            tTramite objList = null;
            try
            {
                objList = Predial.tTramite.Where(o => o.IdConvenio == idConvenio && o.NoParcialidad == noPago && o.Activo == true).FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.GetByNoPago.Exception", ex , "--Parámetros idConvenio:" + idConvenio + ", noPago:" + noPago);
            }
            return objList;
        }

        public List<tTramite> GetTiposTramiteIdPredio(int idPredio, int idTipoTramite, String status, String tipo)
        {
            List<tTramite> objList = null;
            try
            {
                objList = Predial.tTramite.Where(o => o.IdPredio == idPredio && o.IdTipoTramite == idTipoTramite
                    && o.Status == status && o.Tipo == tipo && o.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.GetTiposTramiteIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio + ", idTipoTramite:" + idTipoTramite + ", status:" + status + ", tipo:" + tipo);
            }
            return objList;
        }

        public List<tTramite> GetTramiteFechaIniFin(DateTime fechaInicio, DateTime fechaFin, int idPredio, int idTipoTramite, String status, String tipo)
        {
            List<tTramite> objList = null;
            try
            {
                objList = Predial.tTramite.Where(o => o.Fecha >= fechaInicio && o.Fecha <= fechaFin && o.IdPredio == idPredio && o.IdTipoTramite == idTipoTramite
                    && o.Status == status && o.Tipo == tipo && o.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.GetTramiteFechaIniFin.Exception", ex , "--Parámetros fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin + ", idPredio:" + idPredio +
                    ", idTipoTramite:" + idTipoTramite + ", status:" + status + ", tipo:" + tipo);
            }
            return objList;
        }

        /// <summary>
        /// Método para obtener el interes por falta de pago de convenios
        /// </summary>
        /// <param name="strClvCastatral"></param>
        /// <returns></returns>
        /// 
        public Decimal interesFaltaPagoConvenio(String strClvCastatral)
        {
            if (strClvCastatral.Length == 12)
            {
                tConvenioEdoCta predio = new tConvenioEdoCtaBL().GetByClavePredial(strClvCastatral).Where(p => p.Status == "A").FirstOrDefault();
                if (predio != null)
                {
                    List<tTramite> objList = null;
                    tTramite obj = null;
                    try
                    {
                        objList = Predial.tTramite.Where(t => t.tConvenio.IdConvenioEdoCta == predio.Folio && t.tConvenio.Activo == true && t.Activo == true && t.Status == "A" && t.Fecha <= DateTime.Today).ToList();
                        obj = objList.Where(t => t.Fecha <= (objList.Min(o => o.Fecha))).FirstOrDefault();
                        TimeSpan dias = DateTime.Today - obj.Fecha;
                        Int32 meses = Convert.ToInt32(Utileria.Redondeo(Convert.ToDouble(dias.Days / 30)));
                        //Int32 meses = Math.Abs(obj.Fecha.Month - DateTime.Today.Month) + 12 * (obj.Fecha.Year - DateTime.Today.Year);
                        Decimal total = 0, interes = 0, sumainteres = 0;
                        Decimal interesSistema = Predial.cParametroCobro.Where(pc => pc.Clave == "PorcOmisConv" && pc.Activo == true).FirstOrDefault().Valor;
                        total = Convert.ToDecimal(obj.Mensualidad) + Convert.ToDecimal(obj.Interes);
                        for (Int32 i = 1; i <= meses; i++)
                        {
                            interes = Utileria.Redondeo(total * interesSistema / 100);
                            total = total + interes;
                            sumainteres = sumainteres + interes;
                        }

                        //tPrediosDescuento predioDescuento = Predial.tPrediosDescuento.Where(pd => pd.Activo == true && pd.cPredio.Id == predio.cPredio.Id && pd.IdTramite == obj.Id).FirstOrDefault();
                        //if (predioDescuento != null)
                        //{
                        //    Decimal descuento = predioDescuento.Porcentaje;
                        //    if (descuento > 0)
                        //        sumainteres = sumainteres - Utileria.Redondeo(sumainteres * descuento / 100, 2);
                        //}
                        return sumainteres;
                    }
                    catch (Exception ex)
                    {
                        new Utileria().logError("tTramiteBL.interesFaltaPagoConvenio.Exception", ex , "--Parámetros strClvCastatral:" + strClvCastatral);
                        return -1;
                    }
                }
                else
                    return -1;
            }
            else
                return -1;
        }

        /// <summary>
        /// Método para obtener si es el último pago de un convenio
        /// </summary>
        /// <param name="strClvCastatral"></param>
        /// <returns>-1 - Error al ejecutar método
        ///           0 - No es último pago
        ///           1 - Es último pago</returns>
        ///           
        public Int32 ultimoPagoConvenio(Int32 idConvenio,Int32 idTramite)
        {
            try
            {
                List<tTramite> objList = null;
                objList = Predial.tTramite.Where(t => t.IdConvenio == idConvenio && t.Id != idTramite && t.Activo == true && t.Status == "A").ToList();
                if (objList.Count > 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }        
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.ultimoPagoConvenio.Exception", ex , "--Parámetros idConvenio:" + idConvenio + ", idTramite:" + idTramite);
                return -1;
            }
                
        }

        public int validaMensualidadPagoConvenio(Int32 idTramite, Nullable<int> idConvenio, Nullable<int> noParcialidad)
        {
            try
            {
                List<tTramite> objList = null;
                objList = Predial.tTramite.Where(t => t.IdConvenio == idConvenio && t.Activo == true && t.Status == "A" && t.NoParcialidad < noParcialidad && t.Id != idTramite).ToList();
                if (objList.Count > 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.validaMensualidadPagoConvenio.Exception", ex , "--Parámetros idTramite:" + idTramite + ", idConvenio:" + idConvenio + ", noParcialidad:" + noParcialidad);
                return -1;
            }           
        }

        public MensajesInterfaz cancelarCobroOtrosConceptos(Int32 idTramite,tRecibo rec)
        {
            tTramite tramite = Predial.tTramite.FirstOrDefault(t => t.Id == idTramite);
            //cUsuarios U = new cUsuarios();
            cUsuarios U = (cUsuarios)System.Web.HttpContext.Current.Session["usuario"];// new cUsuarios(); HttpContext.Current.Session
            MensajesInterfaz Update = new MensajesInterfaz();
            //tRecibo rec = new tReciboBL().GetByConstraint(idrecibo);
            switch (tramite.IdTipoTramite)
            {

                case 2://2.- Isabis
                    try
                    {
                        cPredio predio = Predial.cPredio.FirstOrDefault(p => p.Id == tramite.IdPredio);
                        predio.FechaModificacion = DateTime.Now;
                        predio.IdUsuario = U.Id;
                        predio.IdStatusPredio = 3; //Estado Suspendido
                        tramite.FechaModificacion = DateTime.Now;
                        tramite.IdUsuario = U.Id;
                        tramite.Status = "C";
                        Predial.SaveChanges();
                        Update = MensajesInterfaz.Actualizacion;
                    }
                    catch (DbUpdateException ex)
                    {
                        new Utileria().logError("tTramiteBL.cancelarCobroOtrosConceptos.DbUpdateException", ex , "--Parámetros idTramite:" + idTramite);
                        Update = MensajesInterfaz.ErrorGuardar;
                    }
                    catch (DataException ex)
                    {
                        new Utileria().logError("tTramiteBL.cancelarCobroOtrosConceptos.DataException", ex , "--Parámetros idTramite:" + idTramite);
                        Update = MensajesInterfaz.ErrorDB;
                    }
                    catch (Exception ex)
                    {
                        new Utileria().logError("tTramiteBL.cancelarCobroOtrosConceptos.Exception", ex , "--Parámetros idTramite:" + idTramite);
                        Update = MensajesInterfaz.ErrorGeneral;
                    }
                        rec.EstadoRecibo = "C";
                        Update = new tReciboBL().Update(rec);
                        if (Update != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(Update), "Error");
                    break;
                case 3://3.- Convenios
                    try
                    {
                        if (tramite.tConvenio.Status == "P")
                            {
                                tConvenio convenio = Predial.tConvenio.FirstOrDefault(c => c.Id == tramite.tConvenio.Id);
                                convenio.FechaModificacion = DateTime.Now;
                                convenio.IdUsuario = U.Id;
                                convenio.Status = "A";
                            }
                    
                        tramite.FechaModificacion = DateTime.Now;
                        tramite.IdUsuario = U.Id;
                        tramite.Status = "A";
                        Predial.SaveChanges();
                        Update = MensajesInterfaz.Actualizacion;
                    }
                    catch (DbUpdateException ex)
                    {
                        new Utileria().logError("tTramiteBL.cancelarCobroOtrosConceptos.DbUpdateException", ex , "--Parámetros idTramite:" + idTramite);
                        Update = MensajesInterfaz.ErrorGuardar;
                    }
                    catch (DataException ex)
                    {
                        new Utileria().logError("tTramiteBL.cancelarCobroOtrosConceptos.DataException", ex , "--Parámetros idTramite:" + idTramite);
                        Update = MensajesInterfaz.ErrorDB;
                    }
                    catch (Exception ex)
                    {
                        new Utileria().logError("tTramiteBL.cancelarCobroOtrosConceptos.Exception", ex , "--Parámetros idTramite:" + idTramite);
                        Update = MensajesInterfaz.ErrorGeneral;
                    }
                        rec.EstadoRecibo = "C";
                        Update = new tReciboBL().Update(rec);
                        if (Update != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(Update), "Error");
                    break;
                case 1://1.-Certificados 
                case 4://4.-Catastro 
                case 7://7.- Plano
                    try
                    {
                        tramite.FechaModificacion = DateTime.Now;
                        tramite.IdUsuario = U.Id;
                        tramite.Status = "C";
                        Predial.SaveChanges();
                        Update = MensajesInterfaz.Actualizacion;
                    }
                    catch (DbUpdateException ex)
                    {
                        new Utileria().logError("tTramiteBL.cancelarCobroOtrosConceptos.DbUpdateException", ex , "--Parámetros idTramite:" + idTramite);
                        Update = MensajesInterfaz.ErrorGuardar;
                    }
                    catch (DataException ex)
                    {
                        new Utileria().logError("tTramiteBL.cancelarCobroOtrosConceptos.DataException", ex , "--Parámetros idTramite:" + idTramite);
                        Update = MensajesInterfaz.ErrorDB;
                    }
                    catch (Exception ex)
                    {
                        new Utileria().logError("tTramiteBL.cancelarCobroOtrosConceptos.Exception", ex , "--Parámetros idTramite:" + idTramite);
                        Update = MensajesInterfaz.ErrorGeneral;
                    }
                        rec.EstadoRecibo = "C";
                        Update = new tReciboBL().Update(rec);
                        if (Update != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(Update), "Error");
                    break;
                default:
                    break;
            }
            return Update;
        }

        public int ObtenerBimestre(int mes)
        {
            int bimestre = 0;
            try
            {
                switch (mes)
                {
                    case 1:
                    case 2: { bimestre = 1; break; }
                    case 3:
                    case 4: { bimestre = 2; break; }
                    case 5:
                    case 6: { bimestre = 3; break; }
                    case 7:
                    case 8: { bimestre = 4; break; }
                    case 9:
                    case 10: { bimestre = 5; break; }
                    case 11:
                    case 12: { bimestre = 6; break; }
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.ObtenerBimestre.Exception", ex , "--Parámetros mess:" + mes);
            }
           
            return bimestre;
        }

        public tTramite GetTramiteReciboInternet(int idPredio, int idTipoTramite, string periodo)
        {
            tTramite obj = null;
            try
            {
                obj = Predial.tTramite.Where(o => o.IdPredio == idPredio && o.IdTipoTramite == idTipoTramite && o.Periodo == periodo
                    && o.Status != "C" && o.Activo == true).OrderByDescending(o => o.Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.GetByTramiteReciboInternet.Exception", ex, "--Parámetros idPredio:" + idPredio);
            }
            return obj;
        }

        public List<tTramite> GetTramiteIdPredio(int idPredio)
        {
            List<tTramite> ListObj = null;
            try
            {
                ListObj = Predial.tTramite.Where(o => o.IdPredio == idPredio).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.GetTramiteIdPredio.Exception", ex, "--Parámetros idPredio:" + idPredio);
            }
            return ListObj;
        }

        public Decimal interesFaltaPagoConvenioIdTramite(int idTramite)
        {
            tConvenio c = new tConvenio();
            Decimal total = 0, interes = 0, sumainteres = 0;
            tTramite t = new tTramiteBL().GetByConstraint(idTramite);
            int conv = t.IdConvenio == null ? 0 : Convert.ToInt32(t.IdConvenio);
            if (t.IdConvenio > 0)
            {
                c = new tConvenioBL().GetByConstraint(conv );
            }
            if (c != null)
            {
                try
                {
                    TimeSpan dias = DateTime.Today - t.Fecha;
                    Int32 meses = Convert.ToInt32(Utileria.Redondeo(Convert.ToDouble(dias.Days / 30)));
                    if (meses > 0)
                    {
                        Decimal interesSistema = Predial.cParametroCobro.Where(pc => pc.Clave == "PorcOmisConv" && pc.Activo == true).FirstOrDefault().Valor;
                        total = Convert.ToDecimal(t.Mensualidad) + Convert.ToDecimal(t.Interes);
                        for (Int32 i = 1; i <= meses; i++)
                        {
                            interes = Utileria.Redondeo(total * interesSistema / 100);
                            total = total + interes;
                            sumainteres = sumainteres + interes;
                        }
                    }
                }
                catch (Exception ex)
                {
                    new Utileria().logError("tTramiteBL.interesFaltaPagoConvenioIdTramite.Exception",ex , "--Parámetros idtramite:" + idTramite);
                    return -1;
                }

            }
            return sumainteres;
        }
        
        public tTramite BuscaTramitePagado(int idPredio, int idTipoTramite, string periodo)
        {
            tTramite obj = null;
            try
            {
                obj = Predial.tTramite.Where(o => o.IdPredio == idPredio && o.IdTipoTramite == idTipoTramite && o.Periodo == periodo
                    && o.Status == "P" && o.Activo == true).OrderByDescending(o => o.Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteBL.BuscaTramitePagado.Exception", ex, "--Parámetros idPredio:" + idPredio + " idtipotramite: " + idTipoTramite +
                                        " periodo " + periodo + " status: P");
            }
            return obj;
        }


    }

}
