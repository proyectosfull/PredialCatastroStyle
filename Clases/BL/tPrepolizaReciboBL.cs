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
	 public class tPrepolizaReciboBL
	 {
         PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
         public tPrepolizaReciboBL()
		 {
             Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Insert(tPrepolizaRecibo obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.tPrepolizaRecibo.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("tPrepolizaReciboBL.Insert.UpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tPrepolizaReciboBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaReciboBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Update(tPrepolizaRecibo obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
                 tPrepolizaRecibo objOld = Predial.tPrepolizaRecibo.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdRecibo = obj.IdRecibo;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("tPrepolizaReciboBL.Update.UpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tPrepolizaReciboBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaReciboBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
         public tPrepolizaRecibo GetByConstraint(int id)
		 {
             tPrepolizaRecibo obj = null;
			 try
			 {
                 obj = Predial.tPrepolizaRecibo.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaReciboBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Delete(tPrepolizaRecibo obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
                 tPrepolizaRecibo objOld = Predial.tPrepolizaRecibo.FirstOrDefault(c => c.Id == obj.Id);
                 objOld.IdRecibo = obj.IdRecibo;
                 objOld.Activo = obj.Activo;
                 objOld.IdUsuario = obj.IdUsuario;
                 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("tPrepolizaReciboBL.Delete.UpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tPrepolizaReciboBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaReciboBL.Delete.Exception", ex);
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
         //public List<tPrepolizaRecibo> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
         //{
         //    List<tPrepolizaRecibo> objList = null;
         //    try
         //    {
         //        if (campoFiltro == string.Empty)
         //        {
         //             if (activos.ToUpper()=="TRUE")
         //                objList = Predial.tPrepolizaRecibo.SqlQuery("Select Id,FolioPago,NoCaja,FechaPago,IdEstadoRecibo,Contribuyente,Rfc,Domicilio,ImportePagado,ImporteNeto,ImporteDescuento,MaquinaPago,IdUsuarioCancela,MotivoCancelacion,FechaCancelacion,IdUsuarioCobra,IdMesaCobro,IdTipoPago,Activo,IdUsuario,FechaModificacion from tRecibo where activo=1 order by " + campoSort + " " + tipoSort).ToList();
         //             else
         //                objList = Predial.tPrepolizaRecibo.SqlQuery("Select Id,FolioPago,NoCaja,FechaPago,IdEstadoRecibo,Contribuyente,Rfc,Domicilio,ImportePagado,ImporteNeto,ImporteDescuento,MaquinaPago,IdUsuarioCancela,MotivoCancelacion,IdUsuarioCobra,IdMesaCobro,IdTipoPago,Activo,IdUsuario,FechaModificacion from tRecibo where activo=0 order by " + campoSort + " " + tipoSort).ToList();
         //        }
         //        else
         //        {
         //             valorFiltro = "%" + valorFiltro + "%";
         //             if (activos.ToUpper()=="TRUE")
         //                objList = Predial.tPrepolizaRecibo.SqlQuery("Select Id,FolioPago,NoCaja,FechaPago,IdEstadoRecibo,Contribuyente,Rfc,Domicilio,ImportePagado,ImporteNeto,ImporteDescuento,MaquinaPago,IdUsuarioCancela,MotivoCancelacion,IdUsuarioCobra,IdMesaCobro,IdTipoPago,Activo,IdUsuario,FechaModificacion from tRecibo where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
         //             else
         //                 objList = Predial.tPrepolizaRecibo.SqlQuery("Select Id,FolioPago,NoCaja,FechaPago,IdEstadoRecibo,Contribuyente,Rfc,Domicilio,ImportePagado,ImporteNeto,ImporteDescuento,MaquinaPago,IdUsuarioCancela,MotivoCancelacion,FechaCancelacion,IdUsuarioCobra,IdMesaCobro,IdTipoPago,Activo,IdUsuario,FechaModificacion from tRecibo where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
         //        }
         //    }
         //    catch (Exception ex)
         //    {
         //        new Utileria().logError("tPrepolizaReciboBL.GetAll.Exception", ex);
         //    }
         //    return objList;
         //}
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
         public List<tPrepolizaRecibo> GetAll()
		 {
             List<tPrepolizaRecibo> objList = null;
			 try
			 {
                 objList = Predial.tPrepolizaRecibo.Where(o => o.Activo == true).ToList();
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaReciboBL.GetAll.Exception", ex);
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
                 tPrepolizaRecibo pObject = new tPrepolizaRecibo();
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
                 new Utileria().logError("tPrepolizaReciboBL.ListaCampos.Exception", ex);
             } return propertyList;
		 }

         public int GetByCountRecibo(int IdRecibo)
         {
             int obj = 0;
             try
             {
                 obj = Predial.tPrepolizaRecibo.Count(o => o.IdRecibo == IdRecibo && o.Activo == true);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("tPrepolizaReciboBL.GetByRecibo.Exception", ex , "--Parámetros IdRecibo:" + IdRecibo);
             }
             return obj;
         }

	 }

}
