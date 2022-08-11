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
	 public class cUnidadMedidaBL
	 {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public cUnidadMedidaBL()
        {
            Predial = new PredialEntities();
        }

        public List<cUnidadMedida> GetAutoCompleteByName(string prefix)
        {
            List<cUnidadMedida> objList = null;
            try
            {
                objList = Predial.cUnidadMedida.Where(o => o.Activo == true && (o.ClaveUnidad.Contains(prefix) || o.Nombre.Contains(prefix))).OrderBy(i => i.ClaveUnidad).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cUnidadMedidaBL.GetAutoCompleteByName.Exception", ex);
            }
            return objList;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public MensajesInterfaz Insert(cUnidadMedida obj)
        //{
        //    MensajesInterfaz Insert;
        //    try
        //    {
        //        Predial.cUnidadMedida.Add(obj);
        //        Predial.SaveChanges();
        //        Insert = MensajesInterfaz.Ingreso;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.Insert.DbUpdateException", ex);
        //        Insert = MensajesInterfaz.ErrorGuardar;
        //    }
        //    catch (DataException ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.Insert.DataException", ex);
        //        Insert = MensajesInterfaz.ErrorDB;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.Insert.Exception", ex);
        //        Insert = MensajesInterfaz.ErrorGeneral;
        //    }
        //    return Insert;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public MensajesInterfaz Update(cUnidadMedida obj)
        //{
        //    MensajesInterfaz Update;
        //    try
        //    {
        //        cUnidadMedida objOld = Predial.cUnidadMedida.FirstOrDefault(c => c.Id == obj.Id);
        //        Utilerias.Utileria.Compare(obj, objOld);               
        //        objOld.Activo = obj.Activo;
        //        objOld.IdUsuario = obj.IdUsuario;
        //        objOld.FechaModificacion = obj.FechaModificacion;
        //        Predial.SaveChanges();
        //        Update = MensajesInterfaz.Actualizacion;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.Update.DbUpdateException", ex);
        //        Update = MensajesInterfaz.ErrorGuardar;
        //    }
        //    catch (DataException ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.Update.DataException", ex);
        //        Update = MensajesInterfaz.ErrorDB;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.Update.Exception", ex);
        //        Update = MensajesInterfaz.ErrorGeneral;
        //    }
        //    return Update;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public cUnidadMedida GetByConstraint(int id)
        //{
        //    cUnidadMedida obj = null;
        //    try
        //    {
        //        obj = Predial.cUnidadMedida.FirstOrDefault(o => o.Id == id);
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.GetByConstraint.Exception", ex, "--Parámetros id:" + id);
        //    }
        //    return obj;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public MensajesInterfaz Delete(cUnidadMedida obj)
        //{
        //    MensajesInterfaz Delete;
        //    try
        //    {
        //        cUnidadMedida objOld = Predial.cUnidadMedida.FirstOrDefault(c => c.Id == obj.Id);
        //        objOld.Activo = obj.Activo;
        //        objOld.IdUsuario = obj.IdUsuario;
        //        objOld.FechaModificacion = obj.FechaModificacion;
        //        Predial.SaveChanges();
        //        Delete = MensajesInterfaz.Actualizacion;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.Delete.DbUpdateException", ex);
        //        Delete = MensajesInterfaz.ErrorGuardar;
        //    }
        //    catch (DataException ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.Delete.DataException", ex);
        //        Delete = MensajesInterfaz.ErrorDB;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.Delete.Exception", ex);
        //        Delete = MensajesInterfaz.ErrorGeneral;
        //    }
        //    return Delete;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name=""></param>
        ///// <param name=""></param>
        ///// <param name=""></param>
        ///// <param name=""></param>
        ///// <param name=""></param>
        ///// <returns></returns>
        //public List<cUnidadMedida> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        //{
        //    List<cUnidadMedida> objList = null;
        //    try
        //    {
        //        if (campoFiltro == string.Empty)
        //        {
        //            if (activos.ToUpper() == "TRUE")
        //                objList = Predial.cUnidadMedida.SqlQuery("Select Id,NombreColonia,IdTipoAsentamiento,Cp,Activo,IdUsuario,FechaModificacion from cUnidadMedida where activo=1 order by " + campoSort + " " + tipoSort).ToList();
        //            else
        //                objList = Predial.cUnidadMedida.SqlQuery("Select Id,NombreColonia,IdTipoAsentamiento,Cp,Activo,IdUsuario,FechaModificacion from cUnidadMedida where activo=0 order by " + campoSort + " " + tipoSort).ToList();
        //        }
        //        else
        //        {
        //            valorFiltro = "%" + valorFiltro + "%";
        //            if (activos.ToUpper() == "TRUE")
        //                objList = Predial.cUnidadMedida.SqlQuery("Select Id,NombreColonia,IdTipoAsentamiento,Cp,Activo,IdUsuario,FechaModificacion from cUnidadMedida where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
        //            else
        //                objList = Predial.cUnidadMedida.SqlQuery("Select Id,NombreColonia,IdTipoAsentamiento,Cp,Activo,IdUsuario,FechaModificacion from cUnidadMedida where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.GetFilter.Exception", ex,
        //            "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
        //    }
        //    return objList;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns></returns>
        //public List<cUnidadMedida> GetAll()
        //{
        //    List<cUnidadMedida> objList = null;
        //    try
        //    {
        //        objList = Predial.cUnidadMedida.Where(o => o.Activo == true).OrderBy(i => i.NombreColonia).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.GetAll.Exception", ex);
        //    }
        //    return objList;
        //}


        //public cUnidadMedida GetByName(string prefix)
        //{
        //    cUnidadMedida objList = null;
        //    try
        //    {
        //        objList = Predial.cUnidadMedida.FirstOrDefault(o => o.Activo == true && o.NombreColonia.Equals(prefix));
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.GetByName.Exception", ex);
        //    }
        //    return objList;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns></returns>
        //public List<string> ListaCampos()
        //{
        //    List<string> propertyList = new List<string>();
        //    try
        //    {
        //        cUnidadMedida pObject = new cUnidadMedida();
        //        if (pObject != null)
        //        {
        //            foreach (var prop in pObject.GetType().GetProperties())
        //            {
        //                if (prop.Name.ToUpper() != "ID" && prop.Name.ToUpper() != "IDUSUARIO" && prop.Name.ToUpper() != "ACTIVO" && prop.Name.ToUpper() != "FECHAMODIFICACION" && prop.Name.Substring(0, 1) != "c" && prop.Name.Substring(0, 1) != "t" && prop.Name.Substring(0, 1) != "m" && prop.Name.Substring(0, 1) != "I")
        //                    propertyList.Add(prop.Name);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cUnidadMedidaBL.ListaCampos.Exception", ex);
        //    }

        //    return propertyList;
        //}
    }

}
