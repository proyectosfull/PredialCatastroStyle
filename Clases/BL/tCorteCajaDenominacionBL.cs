using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using Clases;
using Clases.Utilerias;


namespace Clases.BL
{
	 /// <summary>
	 /// 
	 /// </summary>
	 public class tCorteCajaDenominacionBL
	 {
         PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public tCorteCajaDenominacionBL()
		 {
             Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(tCorteCajaDenominacion obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.tCorteCajaDenominacion.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tCorteCajaDenominacionBL.Insert.UpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tCorteCajaDenominacionBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tCorteCajaDenominacionBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(tCorteCajaDenominacion obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 tCorteCajaDenominacion objOld = Predial.tCorteCajaDenominacion.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdCorteCaja = obj.IdCorteCaja;
				 objOld.Denominacion = obj.Denominacion;
				 objOld.Cantidad = obj.Cantidad;
				 objOld.Importe = obj.Importe;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tCorteCajaDenominacionBL.Update.UpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tCorteCajaDenominacionBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tCorteCajaDenominacionBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public tCorteCajaDenominacion GetByConstraint(int id)
		 {
			 tCorteCajaDenominacion obj = null;
			 try
			 {
				 obj = Predial.tCorteCajaDenominacion.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tCorteCajaDenominacionBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(tCorteCajaDenominacion obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 tCorteCajaDenominacion objOld = Predial.tCorteCajaDenominacion.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.IdCorteCaja = obj.IdCorteCaja;
				 objOld.Denominacion = obj.Denominacion;
				 objOld.Cantidad = obj.Cantidad;
				 objOld.Importe = obj.Importe;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tCorteCajaDenominacionBL.Delete.UpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tCorteCajaDenominacionBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tCorteCajaDenominacionBL.Delete.Exception", ex);
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
		 public List<tCorteCajaDenominacion> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<tCorteCajaDenominacion> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.tCorteCajaDenominacion.SqlQuery("Select Id,IdCorteCaja,Denominacion,Cantidad,Importe,Activo,IdUsuario,FechaModificacion from tCorteCajaDenominacion where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.tCorteCajaDenominacion.SqlQuery("Select Id,IdCorteCaja,Denominacion,Cantidad,Importe,Activo,IdUsuario,FechaModificacion from tCorteCajaDenominacion where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.tCorteCajaDenominacion.SqlQuery("Select Id,IdCorteCaja,Denominacion,Cantidad,Importe,Activo,IdUsuario,FechaModificacion from tCorteCajaDenominacion where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.tCorteCajaDenominacion.SqlQuery("Select Id,IdCorteCaja,Denominacion,Cantidad,Importe,Activo,IdUsuario,FechaModificacion from tCorteCajaDenominacion where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tCorteCajaDenominacionBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<tCorteCajaDenominacion> GetAll()
		 {
			 List<tCorteCajaDenominacion> objList = null;
			 try
			 {
				 objList = Predial.tCorteCajaDenominacion.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tCorteCajaDenominacionBL.GetAll.Exception", ex);
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
                 tCorteCajaDenominacion pObject = new tCorteCajaDenominacion();
			     if (pObject != null)
			     {
				     foreach (var prop in pObject.GetType().GetProperties())
				     {
					     if (prop.Name.ToUpper() != "ID" && prop.Name.ToUpper() != "IDUSUARIO" && prop.Name.ToUpper() != "ACTIVO" && prop.Name.ToUpper() != "FECHAMODIFICACION" && prop.Name.Substring(0, 1) != "c" && prop.Name.Substring(0, 1) != "t" && prop.Name.Substring(0, 1) != "m" && prop.Name.Substring(0, 1) != "I")
						     propertyList.Add(prop.Name);
				     }
			     }
             }catch (Exception ex)
			 {
				 new Utileria().logError("tCorteCajaDenominacionBL.ListaCampos.Exception", ex);
			 }
			 
		 return propertyList;
		 }
	 }

}
