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
	 public class mUsuBotBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public mUsuBotBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(mUsuBot obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.mUsuBot.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("mUsuBotBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("mUsuBotBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("mUsuBotBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(mUsuBot obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 mUsuBot objOld = Predial.mUsuBot.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdUsuario = obj.IdUsuario;
				 objOld.IdBoton = obj.IdBoton;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuarioMod = obj.IdUsuarioMod;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("mUsuBotBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("mUsuBotBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("mUsuBotBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public mUsuBot GetByConstraint(int id)
		 {
			 mUsuBot obj = null;
			 try
			 {
				 obj = Predial.mUsuBot.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("mUsuBotBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(mUsuBot obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 mUsuBot objOld = Predial.mUsuBot.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.Activo = obj.Activo;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("mUsuBotBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("mUsuBotBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("mUsuBotBL.Delete.Exception", ex);
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
		 public List<mUsuBot> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<mUsuBot> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.mUsuBot.SqlQuery("Select Id,IdUsuario,IdBoton,Activo,IdUsuarioMod,FechaModificacion from mUsuBot where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.mUsuBot.SqlQuery("Select Id,IdUsuario,IdBoton,Activo,IdUsuarioMod,FechaModificacion from mUsuBot where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.mUsuBot.SqlQuery("Select Id,IdUsuario,IdBoton,Activo,IdUsuarioMod,FechaModificacion from mUsuBot where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.mUsuBot.SqlQuery("Select Id,IdUsuario,IdBoton,Activo,IdUsuarioMod,FechaModificacion from mUsuBot where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("mUsuBotBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<mUsuBot> GetAll()
		 {
			 List<mUsuBot> objList = null;
			 try
			 {
				 objList = Predial.mUsuBot.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("mUsuBotBL.GetAll.Exception", ex);
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
                 mUsuBot pObject = new mUsuBot();
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
                 new Utileria().logError("mUsuBotBL.ListaCampos.Exception", ex);
             }
			
		 return propertyList;
		 }
         /// <summary>
         /// 
         /// </summary>
         /// <param name="idUsuario"></param>
         /// <param name="idUsuarioMod"></param>
         /// <param name="activa"></param>
         /// <returns></returns>
         public MensajesInterfaz ActivaDesactiva(int idUsuario, int idUsuarioMod, bool activa)
         {
             MensajesInterfaz Delete;
             try
             {
                 List<mUsuBot> lista = GetAllUsuario(idUsuario);
                 foreach (mUsuBot objOld in lista)
                 {
                     objOld.Activo = activa;
                     objOld.IdUsuarioMod = idUsuarioMod;
                     objOld.FechaModificacion = DateTime.Now;
                 }
                 Predial.SaveChanges();
                 Delete = MensajesInterfaz.Actualizacion;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("mUsuBotBL.ActivaDesactiva.Exception", ex , "--Parámetros idUsuario:" + idUsuario + ", idUsuarioMod:" + idUsuarioMod + ", activa:" + activa);
                 Delete = MensajesInterfaz.ErrorGeneral;
             }
             return Delete;
         }
         /// <summary>
         /// 
         /// </summary>
         /// <param name="obj"></param>
         /// <returns></returns>
         public MensajesInterfaz ActivaDesactivaBoton(int idUsuario, int idBoton, int idUsuarioMod, bool activa)
         {
             MensajesInterfaz Delete;
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
             try
             {
                 mUsuBot objOld = GetByUsuarioBoton(idUsuario, idBoton);
                 objOld.Activo = activa;
                 objOld.IdUsuarioMod = idUsuarioMod;
                 objOld.FechaModificacion = DateTime.Now;
                 Predial.SaveChanges();
                 Delete = MensajesInterfaz.Actualizacion;
             }
             catch (DbUpdateException ex)
             {
                 new Utileria().logError("mUsuBotBL.ActivaDesactivaBoton.UpdateException", ex , "--Parámetros idUsuario:" + idUsuario + ", idUsuarioMod:" + idUsuarioMod + ", activa:" + activa);
                 Delete = MensajesInterfaz.ErrorGuardar;
             }
             catch (DataException ex)
             {
                 Delete = MensajesInterfaz.ErrorDB;
             }
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
             return Delete;
         }
         public List<mUsuBot> GetAllUsuario(int IdUsuario)
         {
             List<mUsuBot> objList = null;
             try
             {
                 objList = Predial.mUsuBot.Where(o => o.IdUsuario == IdUsuario).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("mUsuRol.GetAllUsuario.Exception", ex , "--Parámetros IdUsuario:" + IdUsuario);
             }
             return objList;
         }
         public bool ExisteBoton(int IdUsuario, int idBoton)
         {
             bool existe = true;
             try
             {
                 existe = Predial.mUsuBot.Where(o => o.IdUsuario == IdUsuario && o.IdBoton == idBoton).Count() == 0 ? false : true;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("mUsuRol.ExisteBoton.Exception", ex , "--Parámetros IdUsuario:" + IdUsuario+", idBoton:"+idBoton);
             }
             return existe;
         }
         public mUsuBot GetByUsuarioBoton(int IdUsuario, int idBoton)
         {
             mUsuBot objList = new mUsuBot();
             try
             {
                 objList = Predial.mUsuBot.First(o => o.IdUsuario == IdUsuario && o.IdBoton == idBoton);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("mUsuRol.GetByUsuarioBoton.Exception", ex , "--Parámetros IdUsuario:" + IdUsuario + ", idBoton:" + idBoton);
             }
             return objList;
         }
	 }

}
