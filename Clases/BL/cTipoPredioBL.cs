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
	 public class cTipoPredioBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cTipoPredioBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cTipoPredio obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cTipoPredio.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTipoPagoBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cTipoPagoBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoPagoBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cTipoPredio obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cTipoPredio objOld = Predial.cTipoPredio.FirstOrDefault(c => c.Id == obj.Id);
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
				 new Utileria().logError("cTipoPagoBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cTipoPagoBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoPagoBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cTipoPredio GetByConstraint(int id)
		 {
			 cTipoPredio obj = null;
			 try
			 {
				 obj = Predial.cTipoPredio.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cTipoPagoBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cTipoPredio obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cTipoPredio objOld = Predial.cTipoPredio.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTipoPagoBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cTipoPagoBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoPagoBL.Delete.Exception", ex);
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
		 public List<cTipoPredio> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cTipoPredio> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cTipoPredio.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cTipoPredio where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cTipoPredio.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cTipoPredio where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cTipoPredio.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cTipoPredio where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cTipoPredio.SqlQuery("Select Id,Descripcion,Activo,IdUsuario,FechaModificacion from cTipoPredio where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cTipoPagoBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cTipoPredio> GetAll()
		 {
			 List<cTipoPredio> objList = null;
			 try
			 {
				 objList = Predial.cTipoPredio.Where(o => o.Activo==true).OrderBy(o => o.Descripcion).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoPagoBL.GetAll.Exception", ex);
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
                 cTipoPredio pObject = new cTipoPredio();
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
                 new Utileria().logError("cTipoPagoBL.ListaCampos.Exception", ex);
             } 
		 return propertyList;
		 }
	 }

}
