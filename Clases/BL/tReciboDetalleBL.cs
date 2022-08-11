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
	 public class tReciboDetalleBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public tReciboDetalleBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(tReciboDetalle obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.tReciboDetalle.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tReciboDetalleBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tReciboDetalleBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tReciboDetalleBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(tReciboDetalle obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 tReciboDetalle objOld = Predial.tReciboDetalle.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdRecibo = obj.IdRecibo;
				 objOld.IdConcepto = obj.IdConcepto;
				 objOld.ImporteNeto = obj.ImporteNeto;
				 objOld.ImporteDescuento = obj.ImporteDescuento;
				 objOld.ImporteAdicional = obj.ImporteAdicional;
				 objOld.ImporteTotal = obj.ImporteTotal;
				 objOld.Cantidad = obj.Cantidad;
                 objOld.IdConceptoRef = obj.IdConceptoRef;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
                 objOld.DescuentoDecretoPorcentaje = obj.DescuentoDecretoPorcentaje;
                 objOld.DescuentoEspecialPorcentaje = obj.DescuentoEspecialPorcentaje;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tReciboDetalleBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tReciboDetalleBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tReciboDetalleBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public tReciboDetalle GetByConstraint(int id)
		 {
			 tReciboDetalle obj = null;
			 try
			 {
				 obj = Predial.tReciboDetalle.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tReciboDetalleBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(tReciboDetalle obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 tReciboDetalle objOld = Predial.tReciboDetalle.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tReciboDetalleBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tReciboDetalleBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tReciboDetalleBL.Delete.Exception", ex);
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
		 public List<tReciboDetalle> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<tReciboDetalle> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.tReciboDetalle.SqlQuery("Select Id,IdRecibo,IdConcepto,ImporteNeto,ImporteDescuento,ImporteAdicional,ImporteTotal,Cantidad,IdConceptoRef,Activo,IdUsuario,FechaModificacion,IdDescuentoDecretoConcepto,IdDescuentoDecretoGlobal from tReciboDetalle where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.tReciboDetalle.SqlQuery("Select Id,IdRecibo,IdConcepto,ImporteNeto,ImporteDescuento,ImporteAdicional,ImporteTotal,Cantidad,IdConceptoRef,Activo,IdUsuario,FechaModificacion,IdDescuentoDecretoConcepto,IdDescuentoDecretoGlobal from tReciboDetalle where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.tReciboDetalle.SqlQuery("Select Id,IdRecibo,IdConcepto,ImporteNeto,ImporteDescuento,ImporteAdicional,ImporteTotal,Cantidad,IdConceptoRef,Activo,IdUsuario,FechaModificacion,IdDescuentoDecretoConcepto,IdDescuentoDecretoGlobal from tReciboDetalle where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
                          objList = Predial.tReciboDetalle.SqlQuery("Select Id,IdRecibo,IdConcepto,ImporteNeto,ImporteDescuento,ImporteAdicional,ImporteTotal,Cantidad,IdConceptoRef,Activo,IdUsuario,FechaModificacion,IdDescuentoDecretoConcepto,IdDescuentoDecretoGlobal from tReciboDetalle where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tReciboDetalleBL.GGetFilteretAll.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<tReciboDetalle> GetAll()
		 {
			 List<tReciboDetalle> objList = null;
			 try
			 {
				 objList = Predial.tReciboDetalle.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tReciboDetalleBL.GetAll.Exception", ex);
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
                 tReciboDetalle pObject = new tReciboDetalle();
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
                 new Utileria().logError("tReciboDetalleBL.ListaCampos.Exception", ex);
             }
		 return propertyList;
		 }
	 }

}
