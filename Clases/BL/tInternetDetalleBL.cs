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
	 public class tInternetDetalleBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public tInternetDetalleBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(tInternetDetalle obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.tInternetDetalle.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tInternetDetalleBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tInternetDetalleBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tInternetDetalleBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(tInternetDetalle obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 tInternetDetalle objOld = Predial.tInternetDetalle.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdInternet = obj.IdInternet;
				 objOld.IdConcepto = obj.IdConcepto;
                 objOld.IdMesa = obj.IdMesa;
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
				 new Utileria().logError("tInternetDetalleBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tInternetDetalleBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tInternetDetalleBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public tInternetDetalle GetByConstraint(int id)
		 {
			 tInternetDetalle obj = null;
			 try
			 {
				 obj = Predial.tInternetDetalle.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tInternetDetalleBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(tInternetDetalle obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 tInternetDetalle objOld = Predial.tInternetDetalle.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tInternetDetalleBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tInternetDetalleBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tInternetDetalleBL.Delete.Exception", ex);
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
		 public List<tInternetDetalle> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<tInternetDetalle> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.tInternetDetalle.SqlQuery("Select Id,IdInternet,IdMesa,IdConcepto,ImporteNeto,ImporteDescuento,ImporteAdicional,ImporteTotal,Cantidad,IdConceptoRef,Activo,IdUsuario,FechaModificacion,IdDescuentoDecretoConcepto,IdDescuentoDecretoGlobal from tInternetDetalle where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.tInternetDetalle.SqlQuery("Select Id,IdInternret,IdMesa,IdConcepto,ImporteNeto,ImporteDescuento,ImporteAdicional,ImporteTotal,Cantidad,IdConceptoRef,Activo,IdUsuario,FechaModificacion,IdDescuentoDecretoConcepto,IdDescuentoDecretoGlobal from tInternetDetalle where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.tInternetDetalle.SqlQuery("Select Id,IdInternet,IdMesa,IdConcepto,ImporteNeto,ImporteDescuento,ImporteAdicional,ImporteTotal,Cantidad,IdConceptoRef,Activo,IdUsuario,FechaModificacion,IdDescuentoDecretoConcepto,IdDescuentoDecretoGlobal from tInternetDetalle where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
                          objList = Predial.tInternetDetalle.SqlQuery("Select Id,IdInternet,IdMesa,IdConcepto,ImporteNeto,ImporteDescuento,ImporteAdicional,ImporteTotal,Cantidad,IdConceptoRef,Activo,IdUsuario,FechaModificacion,IdDescuentoDecretoConcepto,IdDescuentoDecretoGlobal from tInternetDetalle where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tInternetDetalleBL.GGetFilteretAll.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<tInternetDetalle> GetAll()
		 {
			 List<tInternetDetalle> objList = null;
			 try
			 {
				 objList = Predial.tInternetDetalle.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tInternetDetalleBL.GetAll.Exception", ex);
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
                 tInternetDetalle pObject = new tInternetDetalle();
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
                 new Utileria().logError("tInternetDetalleBL.ListaCampos.Exception", ex);
             }
		 return propertyList;
		 }

        public List<tInternetDetalle> GetDetallePorMesa(int idInternet, int idMesa)
        {
            List<tInternetDetalle> objList = null;
            try
            {
                objList = Predial.tInternetDetalle.Where(o => o.IdInternet == idInternet && o.IdMesa == idMesa && o.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tInternetDetalleBL.GetAll.Exception", ex);
            }
            return objList;
        }

        public List<tInternetDetalle> GetDetalleCobro(int idInternet, Int32 idMesa)
        {
            List<tInternetDetalle> objList = null;
            try
            {
                if (idMesa != -1)
                {
                    objList = Predial.tInternetDetalle.Where(o => o.IdInternet == idInternet && o.Activo == true && o.IdMesa != idMesa && o.tInternet.Estado == "T").ToList();
                }
                else
                {
                    objList = Predial.tInternetDetalle.Where(o => o.IdInternet == idInternet && o.Activo == true  && o.tInternet.Estado == "T").ToList();
                }
                
                if (objList != null)
                {
                    objList.OrderBy(i => i.Id);
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tInternetDetalleBL.GetDetalleCobro.Exception", ex);
            }
            return objList;
        }
    }

}
