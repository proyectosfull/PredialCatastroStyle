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
    public class cCondominioBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public cCondominioBL()
        {
            Predial = new PredialEntities();
        }
        public MensajesInterfaz Insert(cCondominio obj)
        {
            MensajesInterfaz Insert;
            try
            {
                Predial.cCondominio.Add(obj);
                Predial.SaveChanges();
                Insert = MensajesInterfaz.Ingreso;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("cCondominioBL.Insert.DbUpdateException", ex);
                Insert = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("cCondominioBL.Insert.DataException", ex);
                Insert = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cCondominioBL.Insert.Exception", ex);
                Insert = MensajesInterfaz.ErrorGeneral;
            }
            return Insert;
        }
        public MensajesInterfaz Update(cCondominio obj)
        {
            MensajesInterfaz Update;
            try
            {
                cCondominio objOld = Predial.cCondominio.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Descripcion = obj.Descripcion;
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Update = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("cCondominioBL.Update.DbUpdateException", ex);
                Update = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("cCondominioBL.Update.DataException", ex);
                Update = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cCondominioBL.Update.Exception", ex);
                Update = MensajesInterfaz.ErrorGeneral;
            }
            return Update;
        }
        public MensajesInterfaz Delete(cCondominio obj)
        {
            MensajesInterfaz Delete;
            try
            {
                cCondominio objOld = Predial.cCondominio.FirstOrDefault(c => c.Id == obj.Id);
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Delete = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("cCondominioBL.Delete.DbUpdateException", ex);
                Delete = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("cCondominioBL.Delete.DataException", ex);
                Delete = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cCondominioBL.Delete.Exception", ex);
                Delete = MensajesInterfaz.ErrorGeneral;
            }
            return Delete;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public cCondominio GetByConstraint(int id)
        {
            cCondominio obj = null;
            try
            {
                obj = Predial.cCondominio.FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                new Utileria().logError("cCondominioBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
            }
            return obj;
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
        public List<cCondominio> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<cCondominio> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.cCondominio.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cCondominio where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.cCondominio.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cCondominio where activo=0 order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.cCondominio.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cCondominio where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.cCondominio.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cCondominio where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cCondominioBL.GetFilter.Exception", ex ,
                    "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }

        public List<cCondominio> GetAll()
        {
            List<cCondominio> objList = null;
            try
            {
                objList = Predial.cCondominio.Where(o => o.Activo == true).OrderBy(o=> o.Descripcion).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cCondominio.GetAll.Exception", ex);
            }
            return objList;
        }

        public List<string> ListaCampos()
        {
            List<string> propertyList = new List<string>();
            try
            {
                cCondominio pObject = new cCondominio();
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
                new Utileria().logError("cCondominioBL.ListaCampos.Exception", ex);
            }

            return propertyList;
        }
    }
}
