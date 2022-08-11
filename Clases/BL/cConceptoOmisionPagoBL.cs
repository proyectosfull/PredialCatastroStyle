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
	 public class cConceptoOmisionPagoBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cConceptoOmisionPagoBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cConceptoOmisionPago obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cConceptoOmisionPago.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cConceptoOmisionPagoBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cConceptoOmisionPagoBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cConceptoOmisionPagoBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cConceptoOmisionPago obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cConceptoOmisionPago objOld = Predial.cConceptoOmisionPago.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdPredio = obj.IdPredio;
				 objOld.Activo = obj.Activo;
                 objOld.FechaInicio = obj.FechaInicio;
                 objOld.FechaFin = obj.FechaFin;
                 objOld.ClaveParametro = obj.ClaveParametro;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cConceptoOmisionPagoBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cConceptoOmisionPagoBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cConceptoOmisionPagoBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cConceptoOmisionPago GetByConstraint(int id)
		 {
			 cConceptoOmisionPago obj = null;
			 try
			 {
				 obj = Predial.cConceptoOmisionPago.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cConceptoOmisionPagoBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cConceptoOmisionPago obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cConceptoOmisionPago objOld = Predial.cConceptoOmisionPago.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cConceptoOmisionPagoBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cConceptoOmisionPagoBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cConceptoOmisionPagoBL.Delete.Exception", ex);
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
		 public List<cConceptoOmisionPago> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cConceptoOmisionPago> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cConceptoOmisionPago.SqlQuery("Select Id,IdPredio,Activo,IdUsuario,FechaModificacion from cConceptoOmisionPago where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cConceptoOmisionPago.SqlQuery("Select Id,IdPredio,Activo,IdUsuario,FechaModificacion from cConceptoOmisionPago where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cConceptoOmisionPago.SqlQuery("Select Id,IdPredio,Activo,IdUsuario,FechaModificacion from cConceptoOmisionPago where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cConceptoOmisionPago.SqlQuery("Select Id,IdPredio,Activo,IdUsuario,FechaModificacion from cConceptoOmisionPago where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cConceptoOmisionPagoBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cConceptoOmisionPago> GetAll()
		 {
			 List<cConceptoOmisionPago> objList = null;
			 try
			 {
				 objList = Predial.cConceptoOmisionPago.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cConceptoOmisionPagoBL.GetAll.Exception", ex);
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
                 cConceptoOmisionPago pObject = new cConceptoOmisionPago();
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
                 new Utileria().logError("cConceptoOmisionPagoBL.ListaCampos.Exception", ex);
             }
			 
		 return propertyList;
		 }

         /// <summary>
         /// 
         /// </summary>
         /// <param name=""></param>
         /// <returns></returns>
         public List<cConceptoOmisionPago> GetAllIdPredio(int idPredio)
         {
             List<cConceptoOmisionPago> objList = null;
             try
             {
                 objList = Predial.cConceptoOmisionPago.Where(o => o.IdPredio==idPredio).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cConceptoOmisionPagoBL.GetAllIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio);
             }
             return objList;
         }
	 }

}
