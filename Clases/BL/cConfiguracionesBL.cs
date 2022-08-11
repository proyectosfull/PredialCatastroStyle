using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core;
using Clases.Models;
using System.Data;
using Clases.Utilerias;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace Clases.BL
{
	 /// <summary>
	 /// 
	 /// </summary>
	 public class cConfiguracionesBL
	 {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public cConfiguracionesBL()
		 {
            Predial = new PredialEntities();
        } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cConfiguraciones obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cConfiguraciones.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cConfiguraciones.Insert.UpdateException", ex.ToString());
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cConfiguraciones.Insert.Exception", ex.ToString());
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cConfiguraciones obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cConfiguraciones objOld = Predial.cConfiguraciones.FirstOrDefault(c => c.Id == obj.Id);
                 objOld.Clave = obj.Clave;
				 objOld.valor = obj.valor;
				 objOld.Descripcion = obj.Descripcion;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cConfiguraciones.Update.UpdateException", ex.ToString());
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cConfiguraciones.Update.Exception", ex.ToString());
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cConfiguraciones GetByConstraint(int id)
		 {
			 cConfiguraciones obj = null;
			 try
			 {
				 obj = Predial.cConfiguraciones.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cConfiguraciones.GetByConstraint.Exception", ex.ToString());
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cConfiguraciones obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cConfiguraciones objOld = Predial.cConfiguraciones.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.valor = obj.valor;
				 objOld.Descripcion = obj.Descripcion;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cConfiguraciones.Delete.UpdateException", ex.ToString());
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cConfiguraciones.Delete.Exception", ex.ToString());
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
		 public List<cConfiguraciones> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cConfiguraciones> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cConfiguraciones.SqlQuery("Select Id,Clave,valor,Descripcion,Activo,IdUsuario,FechaModificacion from cConfiguraciones where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.cConfiguraciones.SqlQuery("Select Id,Clave,valor,Descripcion,Activo,IdUsuario,FechaModificacion from cConfiguraciones where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cConfiguraciones.SqlQuery("Select Id,Clave,valor,Descripcion,Activo,IdUsuario,FechaModificacion from cConfiguraciones where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("p", valorFiltro)).ToList();
					  else
                          objList = Predial.cConfiguraciones.SqlQuery("Select Id,Clave,valor,Descripcion,Activo,IdUsuario,FechaModificacion from cConfiguraciones where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cConfiguraciones.GetAll.Exception", ex.ToString());
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cConfiguraciones> GetAll()
		 {
			 List<cConfiguraciones> objList = null;
			 try
			 {
				 objList = Predial.cConfiguraciones.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cConfiguraciones.GetAll.Exception", ex.ToString());
			 }
			 return objList;
		 }

         public cConfiguraciones GetByClave(string clave)
         {
             cConfiguraciones objList = null;
             try
             {
                 objList = Predial.cConfiguraciones.FirstOrDefault(o => o.Activo == true && o.Clave == clave);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cConfiguraciones.GetAll.Exception", ex.ToString());
             }
             return objList;
         }

         public cConfiguraciones GetByValor(string valorConf)
         {
             cConfiguraciones objList = null;
             try
             {
                 objList = Predial.cConfiguraciones.FirstOrDefault(o => o.Activo == true && o.valor == valorConf);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cConfiguraciones.GetAll.Exception", ex.ToString());
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
			 cConfiguraciones pObject = new cConfiguraciones();
			 if (pObject != null)
			 {
				 foreach (var prop in pObject.GetType().GetProperties())
				 {
					 if (prop.Name.ToUpper() != "ID" && prop.Name.ToUpper() != "IDUSUARIO" && prop.Name.ToUpper() != "ACTIVO" && prop.Name.ToUpper() != "FECHAMODIFICACION" && prop.Name.Substring(0, 1) != "c" && prop.Name.Substring(0, 1) != "t" && prop.Name.Substring(0, 1) != "m" && prop.Name.Substring(0, 1) != "I")
						 propertyList.Add(prop.Name);
				 }
			 }
		 return propertyList;
		 }
	 }

}
