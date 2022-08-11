using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using Clases;
using Clases.Utilerias;


namespace Clases.BL
{
    /// <summary>
    /// 
    /// </summary>
    public class tCorteCajaDetalleBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public tCorteCajaDetalleBL()
        {
            Predial = new PredialEntities();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Insert(tCorteCajaDetalle obj)
        {
            MensajesInterfaz Insert;
            try
            {
                Predial.tCorteCajaDetalle.Add(obj);
                Predial.SaveChanges();
                Insert = MensajesInterfaz.Ingreso;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tCorteCajaDetalleBL.Insert.UpdateException", ex);
                Insert = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tCorteCajaDetalleBL.Insert.DataException", ex);
                Insert = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tCorteCajaDetalleBL.Insert.Exception", ex);
                Insert = MensajesInterfaz.ErrorGeneral;
            }
            return Insert;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Update(tCorteCajaDetalle obj)
        {
            MensajesInterfaz Update;
            try
            {
                tCorteCajaDetalle objOld = Predial.tCorteCajaDetalle.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdCorteCaja = obj.IdCorteCaja;
                objOld.IdRecibo = obj.IdRecibo;
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Update = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tCorteCajaDetalleBL.Update.UpdateException", ex);
                Update = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tCorteCajaDetalleBL.Update.DataException", ex);
                Update = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tCorteCajaDetalleBL.Update.Exception", ex);
                Update = MensajesInterfaz.ErrorGeneral;
            }
            return Update;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tCorteCajaDetalle GetByConstraint(int id)
        {
            tCorteCajaDetalle obj = null;
            try
            {
                obj = Predial.tCorteCajaDetalle.FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                new Utileria().logError("tCorteCajaDetalleBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
            }
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Delete(tCorteCajaDetalle obj)
        {
            MensajesInterfaz Delete;
            try
            {
                tCorteCajaDetalle objOld = Predial.tCorteCajaDetalle.FirstOrDefault(c => c.Id == obj.Id);
                objOld.IdCorteCaja = obj.IdCorteCaja;
                objOld.IdRecibo = obj.IdRecibo;
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Delete = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tCorteCajaDetalleBL.Delete.UpdateException", ex);
                Delete = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tCorteCajaDetalleBL.Delete.DataException", ex);
                Delete = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tCorteCajaDetalleBL.Delete.Exception", ex);
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
        public List<tCorteCajaDetalle> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<tCorteCajaDetalle> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.tCorteCajaDetalle.SqlQuery("Select Id,IdCorteCaja,IdRecibo,Activo,IdUsuario,FechaModificacion from tCorteCajaDetalle where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.tCorteCajaDetalle.SqlQuery("Select Id,IdCorteCaja,IdRecibo,Activo,IdUsuario,FechaModificacion from tCorteCajaDetalle where activo=0 order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.tCorteCajaDetalle.SqlQuery("Select Id,IdCorteCaja,IdRecibo,Activo,IdUsuario,FechaModificacion from tCorteCajaDetalle where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.tCorteCajaDetalle.SqlQuery("Select Id,IdCorteCaja,IdRecibo,Activo,IdUsuario,FechaModificacion from tCorteCajaDetalle where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tCorteCajaDetalleBL.GetFilter.Exception", ex ,
                    "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<tCorteCajaDetalle> GetAll()
        {
            List<tCorteCajaDetalle> objList = null;
            try
            {
                objList = Predial.tCorteCajaDetalle.Where(o => o.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tCorteCajaDetalleBL.GetAll.Exception", ex);
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
                tCorteCajaDetalle pObject = new tCorteCajaDetalle();
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
                new Utileria().logError("tCorteCajaDetalleBL.ListaCampos.Exception", ex);
            }

            return propertyList;

        }
    }
}
