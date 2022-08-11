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
	 public class cColoniaBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cColoniaBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cColonia obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cColonia.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cColoniaBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cColoniaBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cColoniaBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cColonia obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cColonia objOld = Predial.cColonia.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.NombreColonia = obj.NombreColonia;
				 objOld.IdTipoAsentamiento = obj.IdTipoAsentamiento;
				 objOld.Cp = obj.Cp;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cColoniaBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cColoniaBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cColoniaBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cColonia GetByConstraint(int id)
		 {
			 cColonia obj = null;
			 try
			 {
				 obj = Predial.cColonia.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cColoniaBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cColonia obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cColonia objOld = Predial.cColonia.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cColoniaBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cColoniaBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cColoniaBL.Delete.Exception", ex);
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
		 public List<cColonia> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cColonia> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cColonia.SqlQuery("Select Id,NombreColonia,IdTipoAsentamiento,Cp,Activo,IdUsuario,FechaModificacion from cColonia where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cColonia.SqlQuery("Select Id,NombreColonia,IdTipoAsentamiento,Cp,Activo,IdUsuario,FechaModificacion from cColonia where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cColonia.SqlQuery("Select Id,NombreColonia,IdTipoAsentamiento,Cp,Activo,IdUsuario,FechaModificacion from cColonia where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cColonia.SqlQuery("Select Id,NombreColonia,IdTipoAsentamiento,Cp,Activo,IdUsuario,FechaModificacion from cColonia where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cColoniaBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cColonia> GetAll()
		 {
			 List<cColonia> objList = null;
			 try
			 {
				 objList = Predial.cColonia.Where(o => o.Activo==true).OrderBy(i=> i.NombreColonia).ToList();
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cColoniaBL.GetAll.Exception", ex);
			 }
			 return objList;
		 }

        public List<cColonia> GetAutoCompleteByName(string prefix)
        {
            List<cColonia> objList = null;
            try
            {
                objList = Predial.cColonia.Where(o => o.Activo == true && o.NombreColonia.Contains(prefix)).OrderBy(i => i.NombreColonia).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cColoniaBL.GetAutoCompleteByName.Exception", ex);
            }
            return objList;
        }
        public cColonia GetByName(string prefix)
        {
            cColonia  objList = null;
            try
            {
                objList = Predial.cColonia.FirstOrDefault(o => o.Activo == true && o.NombreColonia.Equals(prefix));
            }
            catch (Exception ex)
            {
                new Utileria().logError("cColoniaBL.GetByName.Exception", ex);
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
                 cColonia pObject = new cColonia();
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
                 new Utileria().logError("cColoniaBL.ListaCampos.Exception", ex);
             }
			 
		 return propertyList;
		 }
	 }

}
