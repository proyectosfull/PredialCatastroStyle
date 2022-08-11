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
    public class tPrepolizaBL
    {
        PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
        public tPrepolizaBL()
		 {
             Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Insert(tPrepoliza obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
                 Predial.tPrepoliza.Add(obj);
                 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("tPrepolizaBL.Insert.UpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tPrepolizaBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Update(tPrepoliza obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
                 tPrepoliza objOld = Predial.tPrepoliza.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("tPrepolizaBL.Update.UpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tPrepolizaBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
         public tPrepoliza GetByConstraint(int id)
		 {
             tPrepoliza obj = null;
			 try
			 {
                 obj = Predial.tPrepoliza.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Delete(int idPrepoliza, int idUsuario)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
                 tPrepoliza objOld = Predial.tPrepoliza.FirstOrDefault(c => c.Id == idPrepoliza);
                 objOld.Activo = false;
                 objOld.IdUsuario = idUsuario;
                 objOld.FechaModificacion = DateTime.Now;
                 foreach (tPrepolizaDetalle pd in objOld.tPrepolizaDetalle)
                 {
                     pd.Activo = false;
                     pd.IdUsuario = idUsuario;
                     pd.FechaModificacion = DateTime.Now;
                 }
                 foreach (tPrepolizaRecibo pr in objOld.tPrepolizaRecibo)
                 {
                     pr.Activo = false;
                     pr.IdUsuario = idUsuario;
                     pr.FechaModificacion = DateTime.Now;
                 }
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Eliminacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("tPrepolizaBL.Delete.UpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tPrepolizaBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaBL.Delete.Exception", ex);
				 Delete = MensajesInterfaz.ErrorGeneral;
			 }
			 return Delete;
		 }
		 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
         public List<tPrepoliza> GetAll()
		 {
             List<tPrepoliza> objList = null;
			 try
			 {
                 objList = Predial.tPrepoliza.Where(o => o.Activo == true).ToList();
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrepolizaBL.GetAll.Exception", ex);
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
                 tPrepoliza pObject = new tPrepoliza();
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
                 new Utileria().logError("tPrepolizaBL.ListaCampos.Exception", ex);
             }
		 return propertyList;
		 }
 

	 }

}