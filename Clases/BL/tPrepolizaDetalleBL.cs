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
	 public class tPrepolizaDetalleBL
	 {
         PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
         public tPrepolizaDetalleBL()
		 {
             Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Insert(tPrepolizaDetalle obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.tPrepolizaDetalle.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("tPrepolizaDetalleBL.Insert.UpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tPrepolizaDetalleBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaDetalleBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Update(tPrepolizaDetalle obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
                 tPrepolizaDetalle objOld = Predial.tPrepolizaDetalle.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                //objOld.IdDetalle = obj.IdDetalle;
                //objOld.Activo = obj.Activo;
                //objOld.IdUsuario = obj.IdUsuario;
                //objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("tPrepolizaDetalleBL.Update.UpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tPrepolizaDetalleBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaDetalleBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
         public tPrepolizaDetalle GetByConstraint(int id)
		 {
             tPrepolizaDetalle obj = null;
			 try
			 {
                 obj = Predial.tPrepolizaDetalle.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaDetalleBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Delete(tPrepolizaDetalle obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
                 tPrepolizaDetalle objOld = Predial.tPrepolizaDetalle.FirstOrDefault(c => c.Id == obj.Id);                
                 objOld.Activo = obj.Activo;
                 objOld.IdUsuario = obj.IdUsuario;
                 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("tPrepolizaDetalleBL.Delete.UpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tPrepolizaDetalleBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaDetalleBL.Delete.Exception", ex);
				 Delete = MensajesInterfaz.ErrorGeneral;
			 }
			 return Delete;
		 }
		
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
         public List<tPrepolizaDetalle> GetAll()
		 {
             List<tPrepolizaDetalle> objList = null;
			 try
			 {
                 objList = Predial.tPrepolizaDetalle.Where(o => o.Activo == true).ToList();
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaDetalleBL.GetAll.Exception", ex);
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
                 tPrepolizaDetalle pObject = new tPrepolizaDetalle();
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
                 new Utileria().logError("tPrepolizaDetalleBL.ListaCampos.Exception", ex);
             } return propertyList;
		 }

       

	 }

}
