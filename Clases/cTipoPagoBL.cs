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
	 public class cTipoPagoBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cTipoPagoBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cTipoPago obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cTipoPago.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTipoPago.Insert.DbUpdateException", ex.ToString());
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoPago.Insert.Exception", ex.ToString());
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cTipoPago obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cTipoPago objOld = Predial.cTipoPago.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Nombre = obj.Nombre;
				 objOld.Descripcion = obj.Descripcion;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTipoPago.Update.DbUpdateException", ex.ToString());
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoPago.Update.Exception", ex.ToString());
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cTipoPago GetByConstraint(int id)
		 {
			 cTipoPago obj = null;
			 try
			 {
				 obj = Predial.cTipoPago.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoPago.GetByConstraint.Exception", ex.ToString());
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cTipoPago obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cTipoPago objOld = Predial.cTipoPago.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTipoPago.Delete.DbUpdateException", ex.ToString());
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoPago.Delete.Exception", ex.ToString());
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
		 public List<cTipoPago> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cTipoPago> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cTipoPago.SqlQuery("Select Id,Nombre,Descripcion,Activo,IdUsuario,FechaModificacion from cTipoPago where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cTipoPago.SqlQuery("Select Id,Nombre,Descripcion,Activo,IdUsuario,FechaModificacion from cTipoPago where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cTipoPago.SqlQuery("Select Id,Nombre,Descripcion,Activo,IdUsuario,FechaModificacion from cTipoPago where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("p", valorFiltro)).ToList();
					  else
						 objList = Predial.cTipoPago.SqlQuery("Select Id,Nombre,Descripcion,Activo,IdUsuario,FechaModificacion from cTipoPago where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoPago.GetAll.Exception", ex.ToString());
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cTipoPago> GetAll()
		 {
			 List<cTipoPago> objList = null;
			 try
			 {
				 objList = Predial.cTipoPago.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoPago.GetAll.Exception", ex.ToString());
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
			 cTipoPago pObject = new cTipoPago();
			 if (pObject != null)
			 {
				 foreach (var prop in pObject.GetType().GetProperties())
				 {
					 if (prop.Name.ToUpper() != "ID" && prop.Name.ToUpper() != "IDUSUARIO" && prop.Name.ToUpper() != "ACTIVO" && prop.Name.ToUpper() != "FECHAMODIFICACION"  && prop.Name.Substring(0, 1) != "c" && prop.Name.Substring(0, 1) != "t" && prop.Name.Substring(0, 1) != "m" && prop.Name.Substring(0, 1) != "I")
						 propertyList.Add(prop.Name);
				 }
			 }
		 return propertyList;
		 }

         public int GetByClave(string clave)
         {
             cTipoPago obj = null;
             try
             {
                 obj = Predial.cTipoPago.FirstOrDefault(o => o.Clave == clave && o.Activo == true);
                                      
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cTipoPago.GetByConstraint.Exception", ex.ToString());
             }
             return obj.Id;
         }

	 }

}
