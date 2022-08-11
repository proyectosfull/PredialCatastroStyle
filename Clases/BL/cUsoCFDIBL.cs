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
	 public class cUsoCFDIBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cUsoCFDIBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cUsoCFDI obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cUsoCFDI.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cUsoCFDIBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cUsoCFDIBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cUsoCFDIBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cUsoCFDI obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cUsoCFDI objOld = Predial.cUsoCFDI.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);               
				 objOld.Descripcion = obj.Descripcion;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cUsoCFDIBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cUsoCFDIBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cUsoCFDIBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cUsoCFDI GetByConstraint(int id)
		 {
			 cUsoCFDI obj = null;
			 try
			 {
				 obj = Predial.cUsoCFDI.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cUsoCFDIBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cUsoCFDI obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cUsoCFDI objOld = Predial.cUsoCFDI.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cUsoCFDIBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cUsoCFDIBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cUsoCFDIBL.Delete.Exception", ex);
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
		 //public List<cUsoCFDI> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 //{
			// List<cUsoCFDI> objList = null;
			// try
			// {
			//	 if (campoFiltro == string.Empty)
			//	 {
			//		  if (activos.ToUpper()=="TRUE")
			//			 objList = Predial.cUsoCFDI.SqlQuery("Select Id,Nombre,Descripcion,Activo,IdUsuario,FechaModificacion from cUsoCFDI where activo=1 order by " + campoSort + " " + tipoSort).ToList();
			//		  else
			//			 objList = Predial.cUsoCFDI.SqlQuery("Select Id,Nombre,Descripcion,Activo,IdUsuario,FechaModificacion from cUsoCFDI where activo=0 order by " + campoSort + " " + tipoSort).ToList();
			//	 }
			//	 else
			//	 {
			//		  valorFiltro = "%" + valorFiltro + "%";
			//		  if (activos.ToUpper()=="TRUE")
			//			 objList = Predial.cUsoCFDI.SqlQuery("Select Id,Nombre,Descripcion,Activo,IdUsuario,FechaModificacion from cUsoCFDI where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
			//		  else
			//			 objList = Predial.cUsoCFDI.SqlQuery("Select Id,Nombre,Descripcion,Activo,IdUsuario,FechaModificacion from cUsoCFDI where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
			//	 }
			// }
			// catch (Exception ex)
			// {
   //              new Utileria().logError("cUsoCFDIBL.GetFilter.Exception", ex ,
   //                  "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			// }
			// return objList;
		 //}
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cUsoCFDI> GetAll()
		 {
			 List<cUsoCFDI> objList = null;
			 try
			 {
				 objList = Predial.cUsoCFDI.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cUsoCFDIBL.GetAll.Exception", ex);
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
                 cUsoCFDI pObject = new cUsoCFDI();
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
                 new Utileria().logError("cUsoCFDIBL.ListaCampos.Exception", ex);
             }
			
		 return propertyList;
		 }

        public List<cUsoCFDI> GetByTipoPersona(string TipoPersona)
         {
            List<cUsoCFDI> objList = null;

            try
             {
                if (TipoPersona == "FISICA")
                {
                    objList = Predial.cUsoCFDI.Where(o => o.AplicaFisica == true && o.Activo == true).ToList();
                }
                else if (TipoPersona == "MORAL")
                {
                    objList = Predial.cUsoCFDI.Where(o => o.AplicaMoral == true && o.Activo == true).ToList();
                }
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cUsoCFDIBL.GetByTipoPersona.Exception", ex , "--Parámetros TipoPersona:" + TipoPersona);
             }
             return objList;
         }

         public cUsoCFDI GetTipoPagoByClave(string clave)
         {
             cUsoCFDI obj = null;
             try
             {
                 obj = Predial.cUsoCFDI.FirstOrDefault(o => o.Clave == clave && o.Activo == true);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cUsoCFDIBL.GetByClave.Exception", ex , "--Parámetros clave:" + clave);
             }
             return obj;
         }


	 }

}
