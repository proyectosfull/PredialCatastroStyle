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
	 public class cUsoSueloBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cUsoSueloBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cUsoSuelo obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cUsoSuelo.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cUsoSueloBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cUsoSueloBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cUsoSueloBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cUsoSuelo obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cUsoSuelo objOld = Predial.cUsoSuelo.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Clave = obj.Clave;
				 objOld.Descripcion = obj.Descripcion;
				 objOld.Densidad = obj.Densidad;
				 objOld.LoteTipo = obj.LoteTipo;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cUsoSueloBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cUsoSueloBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cUsoSueloBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cUsoSuelo GetByConstraint(int id)
		 {
			 cUsoSuelo obj = null;
			 try
			 {
				 obj = Predial.cUsoSuelo.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cUsoSueloBL.GetByConstraint.Exception", ex , "--Par?metros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cUsoSuelo obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cUsoSuelo objOld = Predial.cUsoSuelo.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cUsoSueloBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cUsoSueloBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cUsoSueloBL.Delete.Exception", ex);
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
		 public List<cUsoSuelo> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cUsoSuelo> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cUsoSuelo.SqlQuery("Select Id,Clave,Descripcion,Densidad,LoteTipo,Activo,IdUsuario,FechaModificacion from cUsoSuelo where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cUsoSuelo.SqlQuery("Select Id,Clave,Descripcion,Densidad,LoteTipo,Activo,IdUsuario,FechaModificacion from cUsoSuelo where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cUsoSuelo.SqlQuery("Select Id,Clave,Descripcion,Densidad,LoteTipo,Activo,IdUsuario,FechaModificacion from cUsoSuelo where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cUsoSuelo.SqlQuery("Select Id,Clave,Descripcion,Densidad,LoteTipo,Activo,IdUsuario,FechaModificacion from cUsoSuelo where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cUsoSueloBL.GetFilter.Exception", ex ,
                     "--Par?metros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cUsoSuelo> GetAll()
		 {
			 List<cUsoSuelo> objList = null;
			 try
			 {
				 objList = Predial.cUsoSuelo.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cUsoSueloBL.GetAll.Exception", ex);
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
                 cUsoSuelo pObject = new cUsoSuelo();
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
                 new Utileria().logError("cUsoSueloBL.ListaCampos.Exception", ex);
             }
		 return propertyList;
		 }
	 }

}
