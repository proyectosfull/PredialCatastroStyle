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
	 public class cProdServBL
	 {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public cProdServBL()
        {
            Predial = new PredialEntities();
        }

        public List<cProdServ> GetAutoCompleteByName(string prefix)
        {
            List<cProdServ> objList = null;
            try
            {
                objList = Predial.cProdServ.Where(o => o.Activo == true && (o.ClaveProdServ.Contains(prefix) || o.Descripcion.Contains(prefix))).OrderBy(i => i.ClaveProdServ).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cProdServBL.GetAutoCompleteByName.Exception", ex);
            }
            return objList;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public MensajesInterfaz Insert(cProdServ obj)
        //{
        //    MensajesInterfaz Insert;
        //    try
        //    {
        //        Predial.cProdServ.Add(obj);
        //        Predial.SaveChanges();
        //        Insert = MensajesInterfaz.Ingreso;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        new Utileria().logError("cProdServBL.Insert.DbUpdateException", ex);
        //        Insert = MensajesInterfaz.ErrorGuardar;
        //    }
        //    catch (DataException ex)
        //    {
        //        new Utileria().logError("cProdServBL.Insert.DataException", ex);
        //        Insert = MensajesInterfaz.ErrorDB;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cProdServBL.Insert.Exception", ex);
        //        Insert = MensajesInterfaz.ErrorGeneral;
        //    }
        //    return Insert;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public MensajesInterfaz Update(cProdServ obj)
        //{
        //    MensajesInterfaz Update;
        //    try
        //    {
        //        cProdServ objOld = Predial.cProdServ.FirstOrDefault(c => c.Id == obj.Id);
        //        Utilerias.Utileria.Compare(obj, objOld);               
        //        objOld.Activo = obj.Activo;
        //        objOld.IdUsuario = obj.IdUsuario;
        //        objOld.FechaModificacion = obj.FechaModificacion;
        //        Predial.SaveChanges();
        //        Update = MensajesInterfaz.Actualizacion;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        new Utileria().logError("cProdServBL.Update.DbUpdateException", ex);
        //        Update = MensajesInterfaz.ErrorGuardar;
        //    }
        //    catch (DataException ex)
        //    {
        //        new Utileria().logError("cProdServBL.Update.DataException", ex);
        //        Update = MensajesInterfaz.ErrorDB;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cProdServBL.Update.Exception", ex);
        //        Update = MensajesInterfaz.ErrorGeneral;
        //    }
        //    return Update;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public cProdServ GetByConstraint(int id)
        //{
        //    cProdServ obj = null;
        //    try
        //    {
        //        obj = Predial.cProdServ.FirstOrDefault(o => o.Id == id);
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cProdServBL.GetByConstraint.Exception", ex, "--Parámetros id:" + id);
        //    }
        //    return obj;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public MensajesInterfaz Delete(cProdServ obj)
        //{
        //    MensajesInterfaz Delete;
        //    try
        //    {
        //        cProdServ objOld = Predial.cProdServ.FirstOrDefault(c => c.Id == obj.Id);
        //        objOld.Activo = obj.Activo;
        //        objOld.IdUsuario = obj.IdUsuario;
        //        objOld.FechaModificacion = obj.FechaModificacion;
        //        Predial.SaveChanges();
        //        Delete = MensajesInterfaz.Actualizacion;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        new Utileria().logError("cProdServBL.Delete.DbUpdateException", ex);
        //        Delete = MensajesInterfaz.ErrorGuardar;
        //    }
        //    catch (DataException ex)
        //    {
        //        new Utileria().logError("cProdServBL.Delete.DataException", ex);
        //        Delete = MensajesInterfaz.ErrorDB;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cProdServBL.Delete.Exception", ex);
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
        //public List<cProdServ> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        //{
        //    List<cProdServ> objList = null;
        //    try
        //    {
        //        if (campoFiltro == string.Empty)
        //        {
        //            if (activos.ToUpper() == "TRUE")
        //                objList = Predial.cProdServ.SqlQuery("Select Id,NombreColonia,IdTipoAsentamiento,Cp,Activo,IdUsuario,FechaModificacion from cProdServ where activo=1 order by " + campoSort + " " + tipoSort).ToList();
        //            else
        //                objList = Predial.cProdServ.SqlQuery("Select Id,NombreColonia,IdTipoAsentamiento,Cp,Activo,IdUsuario,FechaModificacion from cProdServ where activo=0 order by " + campoSort + " " + tipoSort).ToList();
        //        }
        //        else
        //        {
        //            valorFiltro = "%" + valorFiltro + "%";
        //            if (activos.ToUpper() == "TRUE")
        //                objList = Predial.cProdServ.SqlQuery("Select Id,NombreColonia,IdTipoAsentamiento,Cp,Activo,IdUsuario,FechaModificacion from cProdServ where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
        //            else
        //                objList = Predial.cProdServ.SqlQuery("Select Id,NombreColonia,IdTipoAsentamiento,Cp,Activo,IdUsuario,FechaModificacion from cProdServ where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cProdServBL.GetFilter.Exception", ex,
        //            "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
        //    }
        //    return objList;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns></returns>
        //public List<cProdServ> GetAll()
        //{
        //    List<cProdServ> objList = null;
        //    try
        //    {
        //        objList = Predial.cProdServ.Where(o => o.Activo == true).OrderBy(i => i.NombreColonia).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cProdServBL.GetAll.Exception", ex);
        //    }
        //    return objList;
        //}


        //public cProdServ GetByName(string prefix)
        //{
        //    cProdServ objList = null;
        //    try
        //    {
        //        objList = Predial.cProdServ.FirstOrDefault(o => o.Activo == true && o.NombreColonia.Equals(prefix));
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("cProdServBL.GetByName.Exception", ex);
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
        //        cProdServ pObject = new cProdServ();
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
        //        new Utileria().logError("cProdServBL.ListaCampos.Exception", ex);
        //    }

        //    return propertyList;
        //}
    }

}
