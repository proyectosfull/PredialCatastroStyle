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
	 public class cCuotasPredioBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cCuotasPredioBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cCuotasPredio obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cCuotasPredio.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cCuotasPredioBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cCuotasPredioBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cCuotasPredioBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cCuotasPredio obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cCuotasPredio objOld = Predial.cCuotasPredio.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Ejercicio = obj.Ejercicio;
				 objOld.IdTipoPredio = obj.IdTipoPredio;
				 objOld.CuotasCobro = obj.CuotasCobro;
				 objOld.IdTipoCobro = obj.IdTipoCobro;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cCuotasPredioBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cCuotasPredioBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cCuotasPredioBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cCuotasPredio GetByConstraint(int id)
		 {
			 cCuotasPredio obj = null;
			 try
			 {
				 obj = Predial.cCuotasPredio.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cCuotasPredioBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cCuotasPredio obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cCuotasPredio objOld = Predial.cCuotasPredio.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cCuotasPredioBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cCuotasPredioBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cCuotasPredioBL.Delete.Exception", ex);
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
		 public List<cCuotasPredio> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cCuotasPredio> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cCuotasPredio.SqlQuery("Select Id,Ejercicio,IdTipoPredio,CuotasCobro,IdTipoCobro,Activo,IdUsuario,FechaModificacion from cCuotasPredio where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.cCuotasPredio.SqlQuery("Select Id,Ejercicio,IdTipoPredio,CuotasCobro,IdTipoCobro,Activo,IdUsuario,FechaModificacion from cCuotasPredio where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cCuotasPredio.SqlQuery("Select Id,Ejercicio,IdTipoPredio,CuotasCobro,IdTipoCobro,Activo,IdUsuario,FechaModificacion from cCuotasPredio where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
                          objList = Predial.cCuotasPredio.SqlQuery("Select Id,Ejercicio,IdTipoPredio,CuotasCobro,IdTipoCobro,Activo,IdUsuario,FechaModificacion from cCuotasPredio where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }             

			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cCuotasPredioBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cCuotasPredio> GetAll()
		 {
			 List<cCuotasPredio> objList = null;
			 try
			 {
				 objList = Predial.cCuotasPredio.Where(o => o.Activo==true).OrderBy(o => o.Ejercicio).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cCuotasPredioBL.GetAll.Exception", ex);
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
                 cCuotasPredio pObject = new cCuotasPredio();
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
                 new Utileria().logError("cCuotasPredioBL.ListaCampos.Exception", ex);
             }
			 
		 return propertyList;
		 }

         public cCuotasPredio GetByTipoPredio(int ejercicio, int tipo)
         {
             cCuotasPredio obj = null;
             try
             {
                 obj = Predial.cCuotasPredio.FirstOrDefault(o => o.Ejercicio == ejercicio && o.IdTipoPredio == tipo && o.Activo == true);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cCuotasPredioBL.GetByTipoPredio.Exception", ex , "--Parámetros ejercicio:" + ejercicio + ", tipo:" + tipo);
             }
             return obj;
         }

         public List<cCuotasPredio> GetAllByTipoPredio(int ejercicio, int tipo)
         {
             List<cCuotasPredio> objList = null;
             try
             {
                 objList = Predial.cCuotasPredio.Where(o => o.IdTipoPredio == tipo  && o.Activo == true).ToList();

             }
             catch (Exception ex)
             {
                 new Utileria().logError("cCuotasPredioBL.GetAllByTipoPredio.Exception", ex , "--Parámetros ejercicio:" + ejercicio + ", tipo:" + tipo);
             }
             return objList;
         }

         
	 }

}
