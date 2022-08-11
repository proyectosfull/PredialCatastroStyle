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
	 public class tTramiteConceptoBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public tTramiteConceptoBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(tTramiteConcepto obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.tTramiteConcepto.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tTramiteConcepto.Insert.DbUpdateException", ex.ToString());
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tTramiteConcepto.Insert.Exception", ex.ToString());
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(tTramiteConcepto obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 tTramiteConcepto objOld = Predial.tTramiteConcepto.FirstOrDefault(c => c.Id == obj.Id);
                 objOld.IdConcepto = obj.IdConcepto;                 				
				 objOld.IdTipoTramite = obj.IdTipoTramite;
                 objOld.IdUsuario = obj.IdUsuario;
				 objOld.Activo = obj.Activo;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tTramiteConcepto.Update.DbUpdateException", ex.ToString());
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tTramiteConcepto.Update.Exception", ex.ToString());
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public tTramiteConcepto GetByConstraint(int id)
		 {
			 tTramiteConcepto obj = null;
			 try
			 {
				 obj = Predial.tTramiteConcepto.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tTramiteConcepto.GetByConstraint.Exception", ex.ToString());
			 }
			 return obj;
		 }
         /// <summary>
         /// 
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
         public List<tTramiteConcepto> GetListByConstraint(int id, int Ejercicio)
         {
             List<tTramiteConcepto> obj = null;
             try
             {
                 obj = Predial.tTramiteConcepto.Where(o => o.IdTipoTramite == id && o.Activo==true && o.cConcepto.Ejercicio==Ejercicio)
                     .OrderByDescending(u=> u.cConcepto.Adicional).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("tTramiteConcepto.GetByConstraint.Exception", ex.ToString());
             }
             return obj;
         }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(tTramiteConcepto obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 tTramiteConcepto objOld = Predial.tTramiteConcepto.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tTramiteConcepto.Delete.DbUpdateException", ex.ToString());
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tTramiteConcepto.Delete.Exception", ex.ToString());
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
		 public List<tTramiteConcepto> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<tTramiteConcepto> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.tTramiteConcepto.SqlQuery("SELECT Id,IdTipoTramite,IdConcepto,Activo,IdUsuario,FechaModificacion FROM tTramiteConcepto where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.tTramiteConcepto.SqlQuery("SELECT Id,IdTipoTramite,IdConcepto,Activo,IdUsuario,FechaModificacion FROM tTramiteConcepto where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.tTramiteConcepto.SqlQuery("SELECT Id,IdTipoTramite,IdConcepto,Activo,IdUsuario,FechaModificacion FROM tTramiteConcepto where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("p", valorFiltro)).ToList();
					  else
                          objList = Predial.tTramiteConcepto.SqlQuery("SELECT Id,IdTipoTramite,IdConcepto,Activo,IdUsuario,FechaModificacion FROM tTramiteConcepto where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tTramiteConcepto.GetAll.Exception", ex.ToString());
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<tTramiteConcepto> GetAll()
		 {
			 List<tTramiteConcepto> objList = null;
			 try
			 {
				 objList = Predial.tTramiteConcepto.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tTramiteConcepto.GetAll.Exception", ex.ToString());
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
			 tTramiteConcepto pObject = new tTramiteConcepto();
			 if (pObject != null)
			 {
				 foreach (var prop in pObject.GetType().GetProperties())
				 {
					 if (prop.Name.ToUpper() != "ID" && prop.Name.ToUpper() != "IDUSUARIO" && prop.Name.ToUpper() != "ACTIVO" && prop.Name.ToUpper() != "FECHAMODIFICACION"  && prop.Name.Substring(0, 1) != "c" && prop.Name.Substring(0, 1) != "t" && prop.Name.Substring(0, 1) != "m" && prop.Name.Substring(0, 1) != "I")
						 propertyList.Add(prop.Name);
				 }
			 }
		 return propertyList;
		 }

        
    }

}
