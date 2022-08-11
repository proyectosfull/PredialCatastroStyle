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
	 public class tTramiteDetalleBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
         public tTramiteDetalleBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(tTramiteDetalle obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
                 
				 Predial.tTramiteDetalle.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tTramiteDetalleBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tTramiteDetalleBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tTramiteDetalleBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(tTramiteDetalle obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 tTramiteDetalle objOld = Predial.tTramiteDetalle.FirstOrDefault(c => c.Id == obj.Id);
                 Utilerias.Utileria.Compare(obj, objOld);
                 objOld.IdConcepto = obj.IdConcepto;
                 objOld.IdConceptoRef = obj.IdConceptoRef;
                 objOld.IdTramite = obj.IdTramite;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
                 objOld.Importe = obj.Importe;
				 objOld.FechaModificacion = obj.FechaModificacion;
                 
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tTramiteDetalleBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tTramiteDetalleBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tTramiteDetalleBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public tTramiteDetalle GetByConstraint(int id)
		 {
			 tTramiteDetalle obj = null;
			 try
			 {
				 obj = Predial.tTramiteDetalle.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tTramiteDetalleBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(tTramiteDetalle obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 tTramiteDetalle objOld = Predial.tTramiteDetalle.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tTramiteDetalleBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tTramiteDetalleBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tTramiteDetalleBL.Delete.Exception", ex);
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
		 public List<tTramiteDetalle> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<tTramiteDetalle> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.tTramiteDetalle.SqlQuery("Select Id,IdRecibo,IdConcepto,ImporteNeto,ImporteDescuento,ImporteAdicional,ImporteTotal,Cantidad,Activo,IdUsuario,FechaModificacion,IdDescuentoDecretoConcepto,IdDescuentoDecretoGlobal from tTramiteDetalle where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.tTramiteDetalle.SqlQuery("Select Id,IdRecibo,IdConcepto,ImporteNeto,ImporteDescuento,ImporteAdicional,ImporteTotal,Cantidad,Activo,IdUsuario,FechaModificacion,IdDescuentoDecretoConcepto,IdDescuentoDecretoGlobal from tTramiteDetalle where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.tTramiteDetalle.SqlQuery("Select Id,IdRecibo,IdConcepto,ImporteNeto,ImporteDescuento,ImporteAdicional,ImporteTotal,Cantidad,Activo,IdUsuario,FechaModificacion,IdDescuentoDecretoConcepto,IdDescuentoDecretoGlobal from tTramiteDetalle where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.tTramiteDetalle.SqlQuery("Select Id,IdRecibo,IdConcepto,ImporteNeto,ImporteDescuento,ImporteAdicional,ImporteTotal,Cantidad,Activo,IdUsuario,FechaModificacion,IdDescuentoDecretoConcepto,IdDescuentoDecretoGlobal from tTramiteDetalle where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tTramiteDetalleBL.GGetFilteretAll.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<tTramiteDetalle> GetAll()
		 {
			 List<tTramiteDetalle> objList = null;
			 try
			 {
				 objList = Predial.tTramiteDetalle.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tTramiteDetalleBL.GetAll.Exception", ex);
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
                 tTramiteDetalle pObject = new tTramiteDetalle();
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
                 new Utileria().logError("tTramiteDetalleBL.ListaCampos.Exception", ex);
             }
		 return propertyList;
		 }

         public List<tTramiteDetalle> GetAllIdPredio(int idPredio, String status, Int32 idTipoTramite)
         {
             List<tTramiteDetalle> objList = null;
             try
             {
                 objList = Predial.tTramiteDetalle.Where(o => o.tTramite.cPredio.Id == idPredio && o.tTramite.Status == status && o.tTramite.IdTipoTramite == idTipoTramite).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("tTramiteDetalleBL.GetAllIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio + ", status:" + status);
             }
             return objList;
         }

         public List<tTramiteDetalle> GetAllIdTramite(int idTramite)
         {
             List<tTramiteDetalle> objList = null;
             //List<cConcepto> listConcepto = new List<cConcepto>();
             try
             {
                //objList = Predial.tTramiteDetalle.Where(o => o.IdTramite == idTramite && o.Activo==true && o.tTramite.Status=="A").ToList();
                objList = Predial.tTramiteDetalle.Where(o => o.IdTramite == idTramite && o.Activo == true ).ToList();
                //if (objList.Count > 0)
                //{   
                //    foreach (tTramiteDetalle td in objList)
                //    {
                //        listConcepto.Add(td.cConcepto);
                //    }
                //}
            }
             catch (Exception ex)
             {
                 new Utileria().logError("tTramiteDetalleBL.GetAllIdTramite.Exception", ex , "--Parámetros idTramite:" + idTramite);
             }
             return objList;
         }

         public tTramiteDetalle getIdTramiteByIdConcepto(int idTramite, int idConcepto)
         {
             tTramiteDetalle objList = null;
             try
             {
                 objList = Predial.tTramiteDetalle.FirstOrDefault(o => o.IdTramite == idTramite && o.IdConcepto == idConcepto);
                 
             }
             catch (Exception ex)
             {
                 new Utileria().logError("tTramiteDetalleBL.getIdTramiteByIdConcepto.Exception", ex, "--Parámetros idTramite:" + idTramite);
             }
             return objList;
         }

         public MensajesInterfaz InactivaPorIdTramite(int idTramite)
         {
            List<tTramiteDetalle> objList = null;
            MensajesInterfaz msg = new MensajesInterfaz();
            //List<cConcepto> listConcepto = new List<cConcepto>();
            try
            {
                objList = Predial.tTramiteDetalle.Where(o => o.IdTramite == idTramite && o.Activo == true ).ToList();
                if (objList.Count > 0)
                {
                    foreach (tTramiteDetalle td in objList)
                    {
                        td.Activo = false;
                        msg = new tTramiteDetalleBL().Update(td);
                        if (msg != MensajesInterfaz.Actualizacion)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                    }
                }
                else
                    msg = MensajesInterfaz.Actualizacion;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteDetalleBL.InactivaPorIdTramite.Exception", ex, "--Parámetros idTramite:" + idTramite);
                return msg;
            }
            return msg ;
         }


        public MensajesInterfaz BuscaPorIdTramite(int idTramite)
        {
            List<tTramiteDetalle> objList = null;
            MensajesInterfaz msg = new MensajesInterfaz();
            //List<cConcepto> listConcepto = new List<cConcepto>();
            try
            {
                objList = Predial.tTramiteDetalle.Where(o => o.IdTramite == idTramite && o.Activo == true).ToList();
                if (objList.Count > 0)
                    msg = MensajesInterfaz.ListConvenioDesglozado;
                else
                    msg = MensajesInterfaz.NoEncontrados;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tTramiteDetalleBL.BuscaPorIdTramite.Exception", ex, "--Parámetros idTramite:" + idTramite);
                return msg;
            }
            return msg;
        }


    }
}
