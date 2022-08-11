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
	 public class cExentoPagoBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cExentoPagoBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cExentoPago obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cExentoPago.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cExentoPagoBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cExentoPagoBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cExentoPagoBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cExentoPago obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cExentoPago objOld = Predial.cExentoPago.FirstOrDefault(c => c.Id == obj.Id);
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
				 new Utileria().logError("cExentoPagoBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cExentoPagoBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cExentoPagoBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cExentoPago GetByConstraint(int id)
		 {
			 cExentoPago obj = null;
			 try
			 {
				 obj = Predial.cExentoPago.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cExentoPagoBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cExentoPago obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cExentoPago objOld = Predial.cExentoPago.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cExentoPagoBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cExentoPagoBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cExentoPagoBL.Delete.Exception", ex);
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
		 public List<cExentoPago> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cExentoPago> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cExentoPago.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cExentoPago where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cExentoPago.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cExentoPago where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cExentoPago.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cExentoPago where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cExentoPago.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cExentoPago where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cExentoPagoBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cExentoPago> GetAll()
		 {
			 List<cExentoPago> objList = null;
			 try
			 {
				 objList = Predial.cExentoPago.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cExentoPagoBL.GetAll.Exception", ex);
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
                 cExentoPago pObject = new cExentoPago();
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
                 new Utileria().logError("cExentoPagoBL.ListaCampos.Exception", ex);
             }
		 return propertyList;
		 }
	 }

}
