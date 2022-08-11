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
	 public class cTarifaZonaBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cTarifaZonaBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cTarifaZona obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cTarifaZona.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTarifaZonaBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cTarifaZonaBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTarifaZonaBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cTarifaZona obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cTarifaZona objOld = Predial.cTarifaZona.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Zona = obj.Zona;
				 objOld.Ejercicio = obj.Ejercicio;
                 objOld.IdTipoCobro = obj.IdTipoCobro;
				 objOld.Tarifa = obj.Tarifa;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTarifaZonaBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cTarifaZonaBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTarifaZonaBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cTarifaZona GetByConstraint(int id)
		 {
			 cTarifaZona obj = null;
			 try
			 {
				 obj = Predial.cTarifaZona.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cTarifaZonaBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cTarifaZona obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cTarifaZona objOld = Predial.cTarifaZona.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTarifaZonaBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cTarifaZonaBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTarifaZonaBL.Delete.Exception", ex);
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
		 public List<cTarifaZona> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cTarifaZona> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cTarifaZona.SqlQuery("Select Id,Zona,Ejercicio,Tarifa,Activo,IdUsuario,FechaModificacion,IdTipoCobro from cTarifaZona where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.cTarifaZona.SqlQuery("Select Id,Zona,Ejercicio,Tarifa,Activo,IdUsuario,FechaModificacion,IdTipoCobro from cTarifaZona where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cTarifaZona.SqlQuery("Select Id,Zona,Ejercicio,Tarifa,Activo,IdUsuario,FechaModificacion,IdTipoCobro from cTarifaZona where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
                          objList = Predial.cTarifaZona.SqlQuery("Select Id,Zona,Ejercicio,Tarifa,Activo,IdUsuario,FechaModificacion,IdTipoCobro from cTarifaZona where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cTarifaZonaBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cTarifaZona> GetAll()
		 {
			 List<cTarifaZona> objList = null;
			 try
			 {
				 objList = Predial.cTarifaZona.Where(o => o.Activo==true).OrderBy(o => o.Ejercicio).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTarifaZonaBL.GetAll.Exception", ex);
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
                 cTarifaZona pObject = new cTarifaZona();
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
                 new Utileria().logError("cTarifaZonaBL.ListaCampos.Exception", ex);
             }
			 
		 return propertyList;
		 }

         public List<cTarifaZona> GetByPeriodo(int eInicial, int eFinal)
         {
             List<cTarifaZona> objList = null;
             try
             {
                 objList = Predial.cTarifaZona.Where(o => o.Ejercicio >= eInicial && o.Ejercicio <= eFinal && o.Activo == true).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cTarifaZonaBL.GetByPeriodo.Exception", ex , "--Parámetros eInicial:" + eInicial + ", eFinal:" + eFinal);
             }
             return objList;
         }

	 }//class

}
