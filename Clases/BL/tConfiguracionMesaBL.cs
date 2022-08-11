using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Clases;
using Clases.Utilerias;
using System.Data.Entity.Infrastructure;

namespace Clases.BL
{
    public class tConfiguracionMesaBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public tConfiguracionMesaBL()
        {
            Predial = new PredialEntities();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Insert(tConfiguracionMesa obj)
        {
            MensajesInterfaz Insert;
            try
            {
                Predial.tConfiguracionMesa.Add(obj);
                Predial.SaveChanges();
                Insert = MensajesInterfaz.Ingreso;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.Insert.UpdateException", ex);
                Insert = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.Insert.DataException", ex);
                Insert = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.Insert.Exception", ex);
                Insert = MensajesInterfaz.ErrorGeneral;
            }
            return Insert;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Update(tConfiguracionMesa obj)
        {
            MensajesInterfaz Update;
            try
            {
                tConfiguracionMesa objOld = Predial.tConfiguracionMesa.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdMesa = obj.IdMesa;
                objOld.IdCajero = obj.IdCajero;
                objOld.IdCaja = obj.IdCaja;
                objOld.Turno = obj.Turno;
                objOld.Lugar = obj.Lugar;
                objOld.Maquina = obj.Maquina;
                objOld.FechaApertura = obj.FechaApertura;
                objOld.FechaCierre = obj.FechaCierre;
                objOld.Activo = obj.Activo;
                objOld.IdUsuario1 = obj.IdUsuario1;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Update = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.Update.UpdateException", ex);
                Update = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.Update.DataException", ex);
                Update = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.Update.Exception", ex);
                Update = MensajesInterfaz.ErrorGeneral;
            }
            return Update;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tConfiguracionMesa GetByConstraint(int id)
        {
            tConfiguracionMesa obj = null;
            try
            {
                obj = Predial.tConfiguracionMesa.FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
            }
            return obj;
        }


        public tConfiguracionMesa GetByIdCajero(int id)
        {
            tConfiguracionMesa obj = null;
            try
            {
                obj = Predial.tConfiguracionMesa.Where(c => c.IdCajero == id && c.Activo == true).FirstOrDefault(); ;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.GetByIdCajero.Exception", ex , "--Parámetros id:" + id);
            }
            return obj;
        }

        public tConfiguracionMesa GetByIdCaja(int id)
        {
            tConfiguracionMesa obj = null;
            try
            {
                obj = Predial.tConfiguracionMesa.Where(c => c.IdCaja == id && c.Activo == true).FirstOrDefault(); ;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.GetByIdCaja.Exception", ex , "--Parámetros id:" + id);
            }
            return obj;
        }

        public tConfiguracionMesa GetByCajeroCaja(int cajero, int caja)
        {
            tConfiguracionMesa obj = null;
            try
            {
                obj = Predial.tConfiguracionMesa.Where(c => c.IdCajero == cajero && c.IdCaja == caja && c.Activo == true).FirstOrDefault(); ;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.GetByCajeroCaja.Exception", ex , "--Parámetros cajero:" + cajero + ", caja:" + caja);
            }
            return obj;
        }

        public tConfiguracionMesa GetByCajeroMesa(int cajero, int Mesa)
        {
            tConfiguracionMesa obj = null;
            try
            {
                obj = Predial.tConfiguracionMesa.Where(c => c.IdCajero == cajero && c.IdMesa == Mesa && c.Activo == true).FirstOrDefault(); ;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.GetByCajeroMesa.Exception", ex , "--Parámetros cajero:" + cajero + ", mesa:" + Mesa);
            }
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Delete(tConfiguracionMesa obj)
        {
            MensajesInterfaz Delete;
            try
            {
                tConfiguracionMesa objOld = Predial.tConfiguracionMesa.FirstOrDefault(c => c.Id == obj.Id);
                objOld.Activo = obj.Activo;
                objOld.IdUsuario1 = obj.IdUsuario1;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Delete = MensajesInterfaz.CajaCerrada;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.Delete.DbUpdateException", ex);
                Delete = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.Delete.DataException", ex);
                Delete = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.Delete.Exception", ex);
                Delete = MensajesInterfaz.ErrorGeneral;
            }
            return Delete;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Delete(int id)
        {
            MensajesInterfaz Delete;
            try
            {
                tConfiguracionMesa objOld = Predial.tConfiguracionMesa.FirstOrDefault(c => c.Id == id);
                Predial.tConfiguracionMesa.Remove(objOld);
                Predial.SaveChanges();
                Delete = MensajesInterfaz.Eliminacion;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.Delete.DataException", ex , "--Parámetros id:" + id);
                Delete = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.Delete.Exception", ex , "--Parámetros id:" + id);
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
        public List<tConfiguracionMesa> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<tConfiguracionMesa> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.tConfiguracionMesa.SqlQuery("SELECT [Id],[IdMesa],[IdCajero],[IdCaja],[Turno],[Lugar],[Maquina],[FechaApertura],[FechaCierre],[Activo],[IdUsuario1],[FechaModificacion] FROM [tConfiguracionMesa] where Activo=1 order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.tConfiguracionMesa.SqlQuery("SELECT [Id],[IdMesa],[IdCajero],[IdCaja],[Turno],[Lugar],[Maquina],[FechaApertura],[FechaCierre],[Activo],[IdUsuario1],[FechaModificacion] FROM [tConfiguracionMesa] where Activo=0 order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.tConfiguracionMesa.SqlQuery("SELECT [Id],[IdMesa],[IdCajero],[IdCaja],[Turno],[Lugar],[Maquina],[FechaApertura],[FechaCierre],[Activo],[IdUsuario1],[FechaModificacion] FROM [tConfiguracionMesa] where " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.tConfiguracionMesa.SqlQuery("SELECT [Id],[IdMesa],[IdCajero],[IdCaja],[Turno],[Lugar],[Maquina],[FechaApertura],[FechaCierre],[Activo],[IdUsuario1],[FechaModificacion] FROM [tConfiguracionMesa] where " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<tConfiguracionMesa> GetAll()
        {
            List<tConfiguracionMesa> objList = null;
            try
            {
                objList = Predial.tConfiguracionMesa.ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.GetAll.Exception", ex);
            }
            return objList;
        }

        public tConfiguracionMesa GetByIdUsuario(int idUsuario)
        {
            tConfiguracionMesa objList = null;
            try
            {
                objList = Predial.tConfiguracionMesa.FirstOrDefault(o => o.IdCajero == idUsuario && o.Activo == true);
                if (objList == null)
                    objList = null;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.GetAll.Exception", ex , "--Parámetros idUsuario:" + idUsuario);
                
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
                tConfiguracionMesa pObject = new tConfiguracionMesa();
                if (pObject != null)
                {
                    foreach (var prop in pObject.GetType().GetProperties())
                    {
                        if (prop.Name.ToUpper() != "ID" && prop.Name.ToUpper() != "IDUSUARIO" && prop.Name.ToUpper() != "ACTIVO" && prop.Name.ToUpper() != "FECHAMODIFICACION" && prop.Name.Substring(0, 1) != "c")
                            propertyList.Add(prop.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.ListaCampos.Exception", ex);

            }
           
            return propertyList;
        }


        public tConfiguracionMesa ValidaConfiguracion (int idUsuario, int idMesa)
        {
            tConfiguracionMesa obj = null;
            try
            {
                obj = Predial.tConfiguracionMesa.FirstOrDefault(o => o.IdCajero == idUsuario && o.IdMesa == idMesa && o.Activo == true);
                if (obj == null)
                    obj = null;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConfiguracionMesaBL.ValidaConfiguracion.Exception", ex , "--Parámetros idUsuario:" + idUsuario + ", idMesa:" + idMesa);

            }
            return obj;
        }

    }

}
