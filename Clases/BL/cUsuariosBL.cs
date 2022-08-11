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
	 public class cUsuariosBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cUsuariosBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cUsuarios obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cUsuarios.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cUsuariosBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cUsuariosBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cUsuariosBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cUsuarios obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cUsuarios objOld = Predial.cUsuarios.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Nombre = obj.Nombre;
				 objOld.ApellidoPaterno = obj.ApellidoPaterno;
				 objOld.ApellidoMaterno = obj.ApellidoMaterno;
				 objOld.Usuario = obj.Usuario;
				 objOld.Contrasenia = obj.Contrasenia;
				 objOld.Direccion = obj.Direccion;
				 objOld.Area = obj.Area;
				 objOld.NoEmpleado = obj.NoEmpleado;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cUsuariosBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cUsuariosBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cUsuariosBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cUsuarios GetByConstraint(int id)
		 {
			 cUsuarios obj = null;
			 try
			 {
				 obj = Predial.cUsuarios.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cUsuariosBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cUsuarios obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cUsuarios objOld = Predial.cUsuarios.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cUsuariosBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cUsuariosBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cUsuariosBL.Delete.Exception", ex);
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
		 public List<cUsuarios> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cUsuarios> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cUsuarios.SqlQuery("Select Id,Nombre,ApellidoPaterno,ApellidoMaterno,Usuario,Contrasenia,Direccion,Area,NoEmpleado,Activo,IdUsuario,FechaModificacion from cUsuarios where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cUsuarios.SqlQuery("Select Id,Nombre,ApellidoPaterno,ApellidoMaterno,Usuario,Contrasenia,Direccion,Area,NoEmpleado,Activo,IdUsuario,FechaModificacion from cUsuarios where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cUsuarios.SqlQuery("Select Id,Nombre,ApellidoPaterno,ApellidoMaterno,Usuario,Contrasenia,Direccion,Area,NoEmpleado,Activo,IdUsuario,FechaModificacion from cUsuarios where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cUsuarios.SqlQuery("Select Id,Nombre,ApellidoPaterno,ApellidoMaterno,Usuario,Contrasenia,Direccion,Area,NoEmpleado,Activo,IdUsuario,FechaModificacion from cUsuarios where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cUsuariosBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cUsuarios> GetAll()
		 {
			 List<cUsuarios> objList = null;
			 try
			 {
				 objList = Predial.cUsuarios.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cUsuariosBL.GetAll.Exception", ex);
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
                 cUsuarios pObject = new cUsuarios();
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
                 new Utileria().logError("cUsuariosBL.ListaCampos.Exception", ex);
             }
			 
		 return propertyList;
		 }

         public cUsuarios GetByUsuarioContrasenia(string usuario, string contrasenia)
         {
             cUsuarios obj = null;
             try
             {
                 obj = Predial.cUsuarios.FirstOrDefault(o => o.Usuario == usuario && o.Contrasenia == contrasenia);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cUsuariosBL.GetByUsuarioContrasenia.Exception", ex , "--Parámetros usuario:" + usuario + ", contrasenia:" + contrasenia);
             }
             return obj;
         }
         
         public cUsuarios GetByNoEmpleado(int id)
         {
             cUsuarios obj = null;
             try
             {
                 obj = Predial.cUsuarios.FirstOrDefault(o => o.NoEmpleado == id && o.Activo == true);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cUsuariosBL.GetByNoEmpleado.Exception", ex , "--Parámetros id:" + id);
             }
             return obj;
         }

         public cUsuarios GetByNoEmpleadoTodos(int id)
         {
             cUsuarios obj = null;
             try
             {
                 obj = Predial.cUsuarios.FirstOrDefault(o => o.NoEmpleado == id);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cUsuariosBL.GetByNoEmpleadoTodos.Exception", ex , "--Parámetros id:" + id);
             }
             return obj;
         }
         public cUsuarios GetByNoEmpleadoUsuario(int NoEmpleado, string usuario)
         {
             cUsuarios obj = null;
             try
             {
                 obj = Predial.cUsuarios.FirstOrDefault(o => o.NoEmpleado == NoEmpleado || o.Usuario == usuario);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cUsuariosBL.GetByNoEmpleadoUsuario.Exception", ex , "--Parámetros usuario:" + usuario + ", NoEmpleado:" + NoEmpleado);
             }
             return obj;
         }        
         public cUsuarios GetByUsuario(string usuario)
         {
             cUsuarios obj = null;
             try
             {
                 obj = Predial.cUsuarios.FirstOrDefault(o => o.Usuario == usuario);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cUsuariosBL.GetByUsuarioContrasenia.Exception", ex , "--Parámetros usuario:" + usuario);
             }
             return obj;
         }
         public List<cUsuarios> GetFilterConBotones(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
         {
             List<cUsuarios> lista = GetFilter(campoFiltro, valorFiltro, activos, campoSort, tipoSort);
             List<cUsuarios> listaConBotones = new List<cUsuarios>();
             try
             {               
                 foreach (cUsuarios usu in lista)
                 {
                     bool bandera = false;
                     if (usu.mUsuBot.Count() > 0)
                     {
                         foreach (mUsuBot mub in usu.mUsuBot)
                         {
                             if (mub.Activo)
                             {
                                 bandera = true;
                                 break;
                             }
                         }
                         usu.Activo = bandera;
                         listaConBotones.Add(usu);
                     }
                 }
                 
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cUsuariosBL.GetFilterConBotones.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
             }
             return listaConBotones;
         }
	 }

}
