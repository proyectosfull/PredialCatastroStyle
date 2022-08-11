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
	 public class mRolBotBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public mRolBotBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Insert(mRolBot obj)
         {
             MensajesInterfaz Insert;
             try
             {
                 Predial.mRolBot.Add(obj);
                 Predial.SaveChanges();
                 Insert = MensajesInterfaz.Ingreso;
             }
             catch (DbUpdateException ex)
             {
                 new Utileria().logError("mRolBotBL.Insert.UpdateException", ex);
                 Insert = MensajesInterfaz.ErrorGuardar;
             }
             catch (DataException ex)
             {
                 new Utileria().logError("mRolBotBL.Insert.DataException", ex);
                 Insert = MensajesInterfaz.ErrorDB;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("mRolBotBL.Insert.Exception", ex);
                 Insert = MensajesInterfaz.ErrorGeneral;
             }
             return Insert;
         }

         public MensajesInterfaz InsertLista(List<mRolBot> objList, int IdRol, int IdUsuario, DateTime fecha)
         {
             MensajesInterfaz Insert;
             try
             {
                 List<mRolBot> bbaja = Predial.mRolBot.Where(m => m.IdRol == IdRol).ToList();
                 foreach (mRolBot bb in bbaja)
                 {
                     bb.Activo = false;
                     bb.IdUsuario = IdUsuario;
                     bb.FechaModificacion = fecha;
                 }
                 foreach (mRolBot b in objList)
                 {
                     Predial.mRolBot.Add(b);
                 }
                 Predial.SaveChanges();
                 Insert = MensajesInterfaz.Ingreso;
             }
             catch (DbUpdateException ex)
             {
                 new Utileria().logError("mRolBotBL.InsertLista.UpdateException", ex);
                 Insert = MensajesInterfaz.ErrorGuardar;
             }
             catch (DataException ex)
             {
                 new Utileria().logError("mRolBotBL.InsertLista.UpdateException", ex);
                 Insert = MensajesInterfaz.ErrorDB;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("mRolBotBL.InsertLista.Exception", ex);
                 Insert = MensajesInterfaz.ErrorGeneral;
             }
             return Insert;
         }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="obj"></param>
         /// <returns></returns>
         public MensajesInterfaz Update(mRolBot obj)
         {
             MensajesInterfaz Update;
             try
             {
                 mRolBot objOld = Predial.mRolBot.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdBoton = obj.IdBoton;
                 objOld.IdRol = obj.IdRol;
                 objOld.Activo = obj.Activo;
                 objOld.IdUsuario = obj.IdUsuario;
                 objOld.FechaModificacion = obj.FechaModificacion;
                 Predial.SaveChanges();
                 Update = MensajesInterfaz.Actualizacion;
             }
             catch (DbUpdateException ex)
             {
                 new Utileria().logError("mRolBotBL.Update.UpdateException", ex);
                 Update = MensajesInterfaz.ErrorGuardar;
             }
             catch (DataException ex)
             {
                 new Utileria().logError("mRolBotBL.Update.DataException", ex);
                 Update = MensajesInterfaz.ErrorDB;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("mRolBotBL.Update.Exception", ex);
                 Update = MensajesInterfaz.ErrorGeneral;
             }
             return Update;
         }
         /// <summary>
         /// 
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
         public mRolBot GetByConstraint(int id)
         {
             mRolBot obj = null;
             try
             {
                 obj = Predial.mRolBot.FirstOrDefault(o => o.Id == id);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("mRolBotBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
             }
             return obj;
         }
         /// <summary>
         /// 
         /// </summary>
         /// <param name="obj"></param>
         /// <returns></returns>
         public MensajesInterfaz Delete(mRolBot obj)
         {
             MensajesInterfaz Delete;
             try
             {
                 mRolBot objOld = Predial.mRolBot.FirstOrDefault(c => c.Id == obj.Id);
                 objOld.IdBoton = obj.IdBoton;
                 objOld.IdRol = obj.IdRol;
                 objOld.Activo = obj.Activo;
                 objOld.IdUsuario = obj.IdUsuario;
                 objOld.FechaModificacion = obj.FechaModificacion;
                 Predial.SaveChanges();
                 Delete = MensajesInterfaz.Actualizacion;
             }
             catch (DbUpdateException ex)
             {
                 new Utileria().logError("mRolBotBL.Delete.UpdateException", ex);
                 Delete = MensajesInterfaz.ErrorGuardar;
             }
             catch (DataException ex)
             {
                 new Utileria().logError("mRolBotBL.Delete.DataException", ex);
                 Delete = MensajesInterfaz.ErrorDB;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("mRolBotBL.Delete.Exception", ex);
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
         public List<mRolBot> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
         {
             List<mRolBot> objList = null;
             try
             {
                 if (campoFiltro == string.Empty)
                 {
                     if (activos.ToUpper() == "TRUE")
                         objList = Predial.mRolBot.SqlQuery("Select Id,IdBoton,IdRol,Activo,IdUsuario,FechaModificacion from mRolBot where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                     else
                         objList = Predial.mRolBot.SqlQuery("Select Id,IdBoton,IdRol,Activo,IdUsuario,FechaModificacion from mRolBot where activo=0 order by " + campoSort + " " + tipoSort).ToList();
                 }
                 else
                 {
                     valorFiltro = "%" + valorFiltro + "%";
                     if (activos.ToUpper() == "TRUE")
                         objList = Predial.mRolBot.SqlQuery("Select Id,IdBoton,IdRol,Activo,IdUsuario,FechaModificacion from mRolBot where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                     else
                         objList = Predial.mRolBot.SqlQuery("Select Id,IdBoton,IdRol,Activo,IdUsuario,FechaModificacion from mRolBot where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                 }
             }
             catch (Exception ex)
             {
                 new Utileria().logError("mRolBotBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
             }
             return objList;
         }
         /// <summary>
         /// 
         /// </summary>
         /// <param name=""></param>
         /// <returns></returns>
         public List<mRolBot> GetAll()
         {
             List<mRolBot> objList = null;
             try
             {
                 objList = Predial.mRolBot.Where(o => o.Activo == true).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("mRolBotBL.GetAll.Exception", ex);
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
                 mRolBot pObject = new mRolBot();
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
                 new Utileria().logError("mRolBotBL.ListaCampos.Exception", ex);
             } return propertyList;
         }

         public List<mRolBot> GetAllByIdRoll(int IdRol)
         {
             List<mRolBot> objList = null;
             try
             {
                 objList = Predial.mRolBot.Where(o => o.IdRol == IdRol && o.Activo == true).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("mRolBotBL.GetAllByIdRoll.Exception", ex , "--Parámetros IdRol:" + IdRol);
             }
             return objList;
         }
	 }

}
