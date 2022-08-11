using Clases.Utilerias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases.BL
{
    public class cTipoMovAvaluoBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public cTipoMovAvaluoBL()
        {
            Predial = new PredialEntities();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Insert(cTipoMovAvaluo obj)
        {
            MensajesInterfaz Insert;
            try
            {
                Predial.cTipoMovAvaluo.Add(obj);
                Predial.SaveChanges();
                Insert = MensajesInterfaz.Ingreso;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("cTipoMovAvaluoBL.Insert.DbUpdateException", ex);
                Insert = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("cTipoMovAvaluoBL.Insert.DataException", ex);
                Insert = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cTipoMovAvaluoBL.Insert.Exception", ex);
                Insert = MensajesInterfaz.ErrorGeneral;
            }
            return Insert;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Update(cTipoMovAvaluo obj)
        {
            MensajesInterfaz Update;
            try
            {
                cTipoMovAvaluo objOld = Predial.cTipoMovAvaluo.FirstOrDefault(c => c.Id == obj.Id);
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
                new Utileria().logError("cTipoMovAvaluoBL.Update.DbUpdateException", ex);
                Update = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("cTipoMovAvaluoBL.Update.DataException", ex);
                Update = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cTipoMovAvaluoBL.Update.Exception", ex);
                Update = MensajesInterfaz.ErrorGeneral;
            }
            return Update;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public cTipoMovAvaluo GetByConstraint(int id)
        {
            cTipoMovAvaluo obj = null;
            try
            {
                obj = Predial.cTipoMovAvaluo.FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                new Utileria().logError("cTipoMovAvaluoBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
            }
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Delete(cTipoMovAvaluo obj)
        {
            MensajesInterfaz Delete;
            try
            {
                cTipoMovAvaluo objOld = Predial.cTipoMovAvaluo.FirstOrDefault(c => c.Id == obj.Id);
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Delete = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("cTipoMovAvaluoBL.Delete.DbUpdateException", ex);
                Delete = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("cTipoMovAvaluoBL.Delete.DataException", ex);
                Delete = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cTipoMovAvaluoBL.Delete.Exception", ex);
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
        public List<cTipoMovAvaluo> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<cTipoMovAvaluo> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.cTipoMovAvaluo.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cTipoMovAvaluo where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.cTipoMovAvaluo.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cTipoMovAvaluo where activo=0 order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.cTipoMovAvaluo.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cTipoMovAvaluo where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.cTipoMovAvaluo.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cTipoMovAvaluo where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cTipoMovAvaluoBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<cTipoMovAvaluo> GetAll()
        {
            List<cTipoMovAvaluo> objList = null;
            try
            {
                objList = Predial.cTipoMovAvaluo.Where(o => o.Activo == true).OrderBy(o => o.Descripcion).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cTipoMovAvaluoBL.GetAll.Exception", ex);
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
                cTipoMovAvaluo pObject = new cTipoMovAvaluo();
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
                new Utileria().logError("cTipoMovAvaluoBL.ListaCampos.Exception", ex);
            }
           
            return propertyList;
        }
    }

}

