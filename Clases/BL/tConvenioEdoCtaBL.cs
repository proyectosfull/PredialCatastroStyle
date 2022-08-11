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
    public class tConvenioEdoCtaBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public tConvenioEdoCtaBL()
        {
            Predial = new PredialEntities();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tConvenioEdoCta GetByConstraint(int id)
        {
            tConvenioEdoCta obj = null;
            try
            {
                obj = Predial.tConvenioEdoCta.FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioEdoCtaBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
            }
            return obj;
        }

        public List<tConvenioEdoCta> GetByClavePredial(string ClavePredial)
        {
            List<tConvenioEdoCta> obj = null;
            try
            {
                obj = Predial.tConvenioEdoCta.Where(o => o.cPredio.ClavePredial == ClavePredial && o.Activo==true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioEdoCtaBL.GetByClavePredial.Exception", ex , "--Parámetros ClavePredial:" + ClavePredial);
            }
            return obj;
        }

        public tConvenioEdoCta GetByIdPredial(int IdPredio)

        {//obtines el estado ed cuenta por predio y  mes tipo
            tConvenioEdoCta obj = null;
            try
            {
                obj = Predial.tConvenioEdoCta.Where(o => o.IdPredio == IdPredio  && o.FechaEmision.Month == DateTime.Today.Month  && o.Activo == true && o.Status != "C").FirstOrDefault();

            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioEdoCtaBL.GetByIdPredial.Exception", ex , "--Parámetros IdPredio:" + IdPredio);
            }
            return obj;
        }

        public List<tConvenioEdoCta> GetByFolio(Int32 Folio)
        {
            List<tConvenioEdoCta> obj = null;
            try
            {
                obj = Predial.tConvenioEdoCta.Where(o => o.Folio == Folio  ).ToList(); // && o.Activo == true;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioEdoCtaBL.GetByFolio.Exception", ex , "--Parámetros Folio:" + Folio);
            }
            return obj;
        }


        public MensajesInterfaz Insert(tConvenioEdoCta obj)
        {
            MensajesInterfaz Insert;
            try
            {
                Predial.tConvenioEdoCta.Add(obj);
                Predial.SaveChanges();
                Insert = MensajesInterfaz.Ingreso;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tConvenioEdoCtaBL.Insert.DbUpdateException", ex);
                Insert = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tConvenioEdoCtaBL.Insert.DataException", ex);
                Insert = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioEdoCtaBL.Insert.Exception", ex);
                Insert = MensajesInterfaz.ErrorGeneral;
            }
            return Insert;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Update(tConvenioEdoCta obj)
        {
            MensajesInterfaz Update;
            try
            {
                tConvenioEdoCta objOld = Predial.tConvenioEdoCta.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdPredio = obj.IdPredio;
                objOld.Folio = obj.Folio;
                objOld.Status = obj.Status;
                objOld.FechaEmision = obj.FechaEmision;
                objOld.PeriodoIP = obj.PeriodoIP;
                objOld.TipoImpuesto = obj.TipoImpuesto;            
                objOld.IdDiferencias = obj.IdDiferencias;
                objOld.IdDescuento = obj.IdDescuento;
                objOld.IdRequerimiento = obj.IdRequerimiento;
                objOld.Impuesto = obj.Impuesto;
                objOld.Adicional = obj.Adicional;
                objOld.Recargo = obj.Recargo;
                objOld.Rezago = obj.Rezago;
                objOld.Diferencia = obj.Diferencia;
                objOld.RecargoDiferencia = obj.RecargoDiferencia;
                objOld.Honorarios = obj.Honorarios;
                objOld.Ejecucion = obj.Ejecucion;
                objOld.Multa = obj.Multa;
                objOld.DescuentoIP = obj.DescuentoIP;
                objOld.PeriodoSM = obj.PeriodoSM;
                objOld.Infraestructura = obj.Infraestructura;
                objOld.AdicionalSM = obj.AdicionalSM;
                objOld.RezagoSM = obj.RezagoSM;
                objOld.RecargoSM = obj.RecargoSM;
                objOld.Recoleccion = obj.Recoleccion;
                objOld.DAP = obj.DAP;
                objOld.MultaSM = obj.MultaSM;
                objOld.EjecucionSM = obj.EjecucionSM;
                objOld.HonorariosSM = obj.HonorariosSM;
                objOld.Importe = obj.Importe;
                objOld.DescImpuesto = obj.DescImpuesto;
                objOld.DescAdicional = obj.DescAdicional;
                objOld.DescRecargo = obj.DescRecargo;
                objOld.DescRezago = obj.DescRezago;
                objOld.DescDiferencia = obj.DescDiferencia;
                objOld.DescRecargoDiferencia = obj.DescRecargoDiferencia;
                objOld.DescRecoleccion = obj.DescRecoleccion;
                objOld.DescLimpieza = obj.DescLimpieza;
                objOld.DescDAP = obj.DescDAP;
                objOld.DescHonorarios = obj.DescHonorarios;
                objOld.DescEjecucion = obj.DescEjecucion;
                objOld.DescMulta = obj.DescMulta;
                objOld.DescInfraestructura = obj.DescInfraestructura;
                objOld.DescAdicionalSM = obj.DescAdicionalSM;
                objOld.DescRecargoSM = obj.DescRecargoSM;
                objOld.DescRezagoSM = obj.DescRezagoSM;
                objOld.DescHonorariosSM = obj.DescHonorariosSM;
                objOld.DescEjecucionSM = obj.DescEjecucionSM;
                objOld.DescMultaSM = obj.DescMultaSM;
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;

                Predial.SaveChanges();
                Update = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tConvenioEdoCtaBL.Update.DbUpdateException", ex);
                Update = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tConvenioEdoCtaBL.Update.DataException", ex);
                Update = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenio.Update.Exception", ex);
                Update = MensajesInterfaz.ErrorGeneral;
            }
            return Update;
        }

        public  DataTable GetAllForConvenio()
        {//obtines los predios activos que no esten en un convenio activo 
            List<tConvenioEdoCta> obj = new List<tConvenioEdoCta>();
            List<int> obj2 = new List<int>();
            string clavePredianAnt = "";
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("id");
            dtResult.Columns.Add("ClavePredial");
            dtResult.Columns.Add("Nombre");
            try
            {
                obj2 = Predial.tConvenio.Where(o => o.Activo == true).Select(c => c.IdConvenioEdoCta).ToList();
                obj = Predial.tConvenioEdoCta.Where(o => o.Activo == true && !obj2.Contains(o.Folio)).OrderBy(c=>c.cPredio.ClavePredial).ToList();
                foreach (tConvenioEdoCta item in obj)
                {
                    if (!clavePredianAnt.Equals(item.cPredio.ClavePredial))
                    { 
                    DataRow dr = dtResult.NewRow();
                    dr["id"] = item.Id;
                    dr["ClavePredial"] = item.cPredio.ClavePredial;
                    dr["Nombre"] = item.cPredio.cContribuyente.Nombre;
                    dtResult.Rows.Add(dr);
                        clavePredianAnt= item.cPredio.ClavePredial;
                    }
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioEdoCtaBL.GetAllForConvenio.Exception", ex);
            }
            return dtResult;
        }

        public DataTable GetAllForConveniosDelMes()
        {//obtines los predios activos que no esten en un convenio activo 
            List<tConvenioEdoCta> obj = new List<tConvenioEdoCta>();
            List<int> obj2 = new List<int>();
            string clavePredianAnt = "";
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("id");
            dtResult.Columns.Add("ClavePredial");
            dtResult.Columns.Add("Nombre");
            try
            {
                obj2 = Predial.tConvenio.Where(o =>  o.Activo == true).Select(c => c.IdConvenioEdoCta).ToList();
                obj = Predial.tConvenioEdoCta.Where(o => o.FechaEmision.Month == DateTime.Today.Month && o.Activo == true && !obj2.Contains(o.Folio)).OrderBy(c => c.cPredio.ClavePredial).ToList();
                foreach (tConvenioEdoCta item in obj)
                { 
                    if (!clavePredianAnt.Equals(item.cPredio.ClavePredial))
                    {
                        DataRow dr = dtResult.NewRow();
                        dr["id"] = item.Id;
                        dr["ClavePredial"] = item.cPredio.ClavePredial;
                        dr["Nombre"] = item.cPredio.cContribuyente.Nombre;
                        dtResult.Rows.Add(dr);
                        clavePredianAnt = item.cPredio.ClavePredial;
                    }
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioEdoCtaBL.GetAllForConveniosDelMes.Exception", ex);
            }
            return dtResult;
        }

        public List<tConvenioEdoCta> GetByClavePredialByMonth(string ClavePredial, Int32 idConvenio )
        {
            List<tConvenioEdoCta> obj = null;
            try
            {
                if( idConvenio> 0 )
                    obj = Predial.tConvenioEdoCta.Where(o => o.cPredio.ClavePredial == ClavePredial && o.Id == idConvenio && o.Activo == true).ToList();
                else
                    obj = Predial.tConvenioEdoCta.Where(o => o.cPredio.ClavePredial == ClavePredial && o.FechaEmision.Month == DateTime.Today.Month && o.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioEdoCtaBL.GetByClavePredial.Exception", ex, "--Parámetros ClavePredial:" + ClavePredial);
            }
            return obj;
        }


    }
}
