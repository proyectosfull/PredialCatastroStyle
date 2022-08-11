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
	 public class cFIELBL
	 {
         PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cFIELBL()
		 {
             Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cFIEL obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
                 Predial.cFIEL.Add(obj);
                 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cFIELBL.Insert.UpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cFIELBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cFIELBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Update(cFIEL obj)
         {
             MensajesInterfaz Update;
             try
             {
                 cFIEL objOld = Predial.cFIEL.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Nombre = obj.Nombre;
                 //objOld.Descripcion = obj.Descripcion;
                 objOld.Activo = obj.Activo;
                 objOld.IdUsuario = obj.IdUsuario;
                 objOld.FechaModificacion = obj.FechaModificacion;
                 Predial.SaveChanges();
                 Update = MensajesInterfaz.Actualizacion;
             }
             catch (DbUpdateException ex)
             {
                 new Utileria().logError("cFIELBL.Update.UpdateException", ex);
                 Update = MensajesInterfaz.ErrorGuardar;
             }
             catch (DataException ex)
             {
                 new Utileria().logError("cFIELBL.Update.DataException", ex);
                 Update = MensajesInterfaz.ErrorDB;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cFIELBL.Update.Exception", ex);
                 Update = MensajesInterfaz.ErrorGeneral;
             }
             return Update;
         }
         /// <summary>
         /// 
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
         public cFIEL GetByConstraint(int id)
         {
             cFIEL obj = null;
             try
             {
                 obj = Predial.cFIEL.FirstOrDefault(o => o.Id == id);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cFIELBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
             }
             return obj;
         }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cFIEL GetByActive()
		 {
			 cFIEL obj = null;
			 try
			 {
				 obj = Predial.cFIEL.FirstOrDefault(o => o.Activo==true);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cFIELBL.GetByActive.Exception", ex);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cFIEL obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
                 cFIEL objOld = Predial.cFIEL.FirstOrDefault(c => c.Id == obj.Id);
				 
				 objOld.IdUsuario = obj.IdUsuario;
                 objOld.Activo = obj.Activo;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cFIELBL.Delete.UpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cFIELBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cFIELBL.Delete.Exception", ex);
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
         public List<cFIEL> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
         {
             List<cFIEL> objList = null;
             try
             {
                 if (campoFiltro == string.Empty)
                 {
                     if (activos.ToUpper() == "TRUE")
                         objList = Predial.cFIEL.SqlQuery("Select Id,Nombre,RFC,Calle,Municipio,Estado,Pais,CodigoPostal,NoExterior,NoInterior,Colonia,Localidad,Referencia,KeyFile,CerFile,KeyPass,Logo,Activo,IdUsuario,FechaModificacion from cFIEL where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                     else
                         objList = Predial.cFIEL.SqlQuery("Select Id,Nombre,RFC,Calle,Municipio,Estado,Pais,CodigoPostal,NoExterior,NoInterior,Colonia,Localidad,Referencia,KeyFile,CerFile,KeyPass,Logo,Activo,IdUsuario,FechaModificacion from cFIEL where activo=0 order by " + campoSort + " " + tipoSort).ToList();
                 }
                 else
                 {
                     valorFiltro = "%" + valorFiltro + "%";
                     if (activos.ToUpper() == "TRUE")
                         objList = Predial.cFIEL.SqlQuery("Select Id,Nombre,RFC,Calle,Municipio,Estado,Pais,CodigoPostal,NoExterior,NoInterior,Colonia,Localidad,Referencia,KeyFile,CerFile,KeyPass,Logo,Activo,IdUsuario,FechaModificacion from cFIEL where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                     else
                         objList = Predial.cFIEL.SqlQuery("Select Id,Nombre,RFC,Calle,Municipio,Estado,Pais,CodigoPostal,NoExterior,NoInterior,Colonia,Localidad,Referencia,KeyFile,CerFile,KeyPass,Logo,Activo,IdUsuario,FechaModificacion from cFIEL where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                 }
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cFIELBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
             }
             return objList;
         }
         /// <summary>
         /// 
         /// </summary>
         /// <param name=""></param>
         /// <returns></returns>
         public List<cFIEL> GetAll()
         {
             List<cFIEL> objList = null;
             try
             {
                 objList = Predial.cFIEL.Where(o => o.Activo == true).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cFIELBL.GetAll.Exception", ex);
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
                 cFIEL pObject = new cFIEL();
                 if (pObject != null)
                 {
                     foreach (var prop in pObject.GetType().GetProperties())
                     {
                         if (prop.Name.ToUpper() != "ID" && prop.Name.ToUpper() != "IDUSUARIO" && prop.Name.ToUpper() != "ACTIVO" && prop.Name.ToUpper() != "FECHAMODIFICACION" && prop.Name.ToUpper() != "KEYFILE" && prop.Name.ToUpper() != "CERFILE" && prop.Name.ToUpper() != "LOGO" && prop.Name.Substring(0, 1) != "c" && prop.Name.Substring(0, 1) != "t" && prop.Name.Substring(0, 1) != "m" && prop.Name.Substring(0, 1) != "I")
                             propertyList.Add(prop.Name);
                     }
                 }
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cFIELBL.ListaCampos.Exception", ex);
             }
             return propertyList;
         }
	 }

}
