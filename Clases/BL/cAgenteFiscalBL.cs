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
	 public class cAgenteFiscalBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cAgenteFiscalBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cAgenteFiscal obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cAgenteFiscal.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cAgenteFiscalBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cAgenteFiscalBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cAgenteFiscalBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cAgenteFiscal obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cAgenteFiscal objOld = Predial.cAgenteFiscal.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Nombre = obj.Nombre;
				 objOld.ApellidoPaterno = obj.ApellidoPaterno;
				 objOld.ApellidoMaterno = obj.ApellidoMaterno;
				 objOld.Direccion = obj.Direccion;
				 objOld.Ciudad = obj.Ciudad;
				 objOld.Estado = obj.Estado;
				 objOld.Sexo = obj.Sexo;
				 objOld.Telefono = obj.Telefono;
				 objOld.Email = obj.Email;
				 objOld.Rfc = obj.Rfc;
				 objOld.Curp = obj.Curp;
				 objOld.FechaIngreso = obj.FechaIngreso;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cAgenteFiscalBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cAgenteFiscalBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cAgenteFiscalBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cAgenteFiscal GetByConstraint(int id)
		 {
			 cAgenteFiscal obj = null;
			 try
			 {
				 obj = Predial.cAgenteFiscal.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cAgenteFiscalBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cAgenteFiscal obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cAgenteFiscal objOld = Predial.cAgenteFiscal.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cAgenteFiscalBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cAgenteFiscalBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cAgenteFiscalBL.Delete.Exception", ex);
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
		 public List<cAgenteFiscal> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cAgenteFiscal> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cAgenteFiscal.SqlQuery("Select Id,Nombre,ApellidoPaterno,ApellidoMaterno,Direccion,Ciudad,Estado,Sexo,Telefono,Email,Rfc,Curp,FechaIngreso,Activo,IdUsuario,FechaModificacion from cAgenteFiscal where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cAgenteFiscal.SqlQuery("Select Id,Nombre,ApellidoPaterno,ApellidoMaterno,Direccion,Ciudad,Estado,Sexo,Telefono,Email,Rfc,Curp,FechaIngreso,Activo,IdUsuario,FechaModificacion from cAgenteFiscal where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cAgenteFiscal.SqlQuery("Select Id,Nombre,ApellidoPaterno,ApellidoMaterno,Direccion,Ciudad,Estado,Sexo,Telefono,Email,Rfc,Curp,FechaIngreso,Activo,IdUsuario,FechaModificacion from cAgenteFiscal where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cAgenteFiscal.SqlQuery("Select Id,Nombre,ApellidoPaterno,ApellidoMaterno,Direccion,Ciudad,Estado,Sexo,Telefono,Email,Rfc,Curp,FechaIngreso,Activo,IdUsuario,FechaModificacion from cAgenteFiscal where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cAgenteFiscalBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cAgenteFiscal> GetAll()
		 {
			 List<cAgenteFiscal> objList = null;
			 try
			 {
				 objList = Predial.cAgenteFiscal.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cAgenteFiscalBL.GetAll.Exception", ex);
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
                 cAgenteFiscal pObject = new cAgenteFiscal();
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
                 new Utileria().logError("cAgenteFiscalBL.ListaCampos.Exception", ex);
             }
			 
		 return propertyList;
		 }
	 }

}
