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
	 public class cRfcBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cRfcBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cRFC obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cRFC.Add(obj); 
				 Predial.SaveChanges();
                 Insert = MensajesInterfaz.IngresoRFC;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cRfcBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cRfcBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cRfcBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cRFC obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cRFC objOld = Predial.cRFC.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.RFC = obj.RFC;
				 objOld.Nombre = obj.Nombre;
				 objOld.Calle = obj.Calle ;
                objOld.Municipio = obj.Municipio;
                objOld.Estado = obj.Estado;
                objOld.Pais = obj.Pais;
                objOld.CodigoPostal = obj.CodigoPostal;
                objOld.NoExterior = obj.NoExterior;
                objOld.NoInterior = obj.NoInterior;
                objOld.Colonia = obj.Colonia;
                objOld.Localidad = obj.Localidad;
                objOld.Referencia = obj.Referencia;
                objOld.Email = obj.Email;
                objOld.IdContribuyente = obj.IdContribuyente;
                objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.ActualizacionRFC;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cRfcBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cRfcBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cRfcBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cRFC GetByConstraint(int id)
		 {
			 cRFC obj = null;
			 try
			 {
				 obj = Predial.cRFC.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cRfcBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cRFC obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cRFC objOld = Predial.cRFC.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = false;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cRfcBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cRfcBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cRfcBL.Delete.Exception", ex);
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
		 public List<cRFC> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cRFC> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cRFC.SqlQuery("SELECT Id, RFC, Nombre, Calle, Municipio, Estado, Pais, CodigoPostal, NoExterior, NoInterior, Colonia, Localidad, Referencia, Email, Activo, IdUsuario, FechaModificacion, IdPredio, IdContribuyente, RegimenFiscal, UsoCFDI  FROM cRFC where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.cRFC.SqlQuery("SELECT Id, RFC, Nombre, Calle, Municipio, Estado, Pais, CodigoPostal, NoExterior, NoInterior, Colonia, Localidad, Referencia, Email, Activo, IdUsuario, FechaModificacion, IdPredio, IdContribuyente, RegimenFiscal, UsoCFDI  FROM cRFC where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cRFC.SqlQuery("SELECT Id, RFC, Nombre, Calle, Municipio, Estado, Pais, CodigoPostal, NoExterior, NoInterior, Colonia, Localidad, Referencia, Email, Activo, IdUsuario, FechaModificacion, IdPredio, IdContribuyente, RegimenFiscal, UsoCFDI  FROM cRFC where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
                          objList = Predial.cRFC.SqlQuery("SELECT Id, RFC, Nombre, Calle, Municipio, Estado, Pais, CodigoPostal, NoExterior, NoInterior, Colonia, Localidad, Referencia, Email, Activo, IdUsuario, FechaModificacion, IdPredio, IdContribuyente, RegimenFiscal, UsoCFDI FROM cRFC where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cRfcBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cRFC> GetAll()
		 {
			 List<cRFC> objList = null;
			 try
			 {
				 objList = Predial.cRFC.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cRfcBL.GetAll.Exception", ex);
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
                 cRFC pObject = new cRFC();
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
                 new Utileria().logError("cRfcBL.ListaCampos.Exception", ex);
             }
			 
		 return propertyList;
		 }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="rfc"></param>
         /// <returns></returns>
         public cRFC GetByRfc(string rfc)
         {
             cRFC obj = null;
             try
             {
                 obj = Predial.cRFC.FirstOrDefault(o => o.RFC == rfc.Trim());
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cRfcBL.GetByRfc.Exception", ex , "--Parámetros rfc:" + rfc);
             }
             return obj;
         }

         /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cRFC GetActivo()
		 {
			 cRFC obj = null;
			 try
			 {
				 obj = Predial.cRFC.FirstOrDefault(o => o.Activo == true);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cRfcBL.GetActivo.Exception", ex);
			 }
			 return obj;
		 }


        public cRFC GetRfcByIdpredio(int idContribuyente)
        {
            cRFC obj = null;
            try
            {
                obj = Predial.cRFC.FirstOrDefault(o => o.IdContribuyente == idContribuyente);
                if (obj == null)
                {
                    obj = new cRFC();
                    obj.IdContribuyente = idContribuyente;
                    obj.Localidad = "";
                    obj.Nombre = "";
                    obj.Calle = "";
                    obj.Municipio = "";
                    obj.Estado = "";
                    obj.Pais = "";
                    obj.CodigoPostal = "";
                    obj.NoExterior = "";
                    obj.NoInterior = "";
                    obj.Colonia = "";
                    obj.Localidad = "";
                    obj.Referencia = "";
                    obj.Email = "";
                    obj.RegimenFiscal = "";
                    obj.UsoCFDI = "";
                    //obj.Activo = "";
                    //obj.IdUsuario = "";
                    //obj.FechaModificacion = obj.FechaModificacion;
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cRfcBL.GetByRfc.Exception", ex, "GetRfcByIdpredio--Parámetros rfc: idContribuyente" + idContribuyente);
            }
            return obj;
        }

        public cRFC GetRfcByIdcontribuyente(int idContribuyente)
        {
            cRFC obj = null;
            try
            {
                obj = Predial.cRFC.Where(o => o.Activo == true).FirstOrDefault(o => o.IdContribuyente == idContribuyente); //Predial.cRFC.firsto("SELECT Id, RFC, Nombre, Calle, Municipio, Estado, Pais, CodigoPostal, NoExterior, NoInterior, Colonia, Localidad, Referencia, Email, Activo, IdUsuario, FechaModificacion, IdPredio, IdContribuyente, RegimenFiscal, UsoCFDI  FROM cRFC where activo=1 and IdContribuyente =  " + idContribuyente);
                //obj = Predial.cRFC.FirstOrDefault(o => o.IdContribuyente == idContribuyente);
                if (obj == null)
                {
                    obj = new cRFC();
                    obj.IdContribuyente = idContribuyente;
                    obj.Localidad = "";
                    obj.Nombre = "";
                    obj.Calle = "";
                    obj.Municipio = "";
                    obj.Estado = "";
                    obj.Pais = "";
                    obj.CodigoPostal = "";
                    obj.NoExterior = "";
                    obj.NoInterior = "";
                    obj.Colonia = "";
                    obj.Localidad = "";
                    obj.Referencia = "";
                    obj.Email = "";
                    obj.RegimenFiscal = "";
                    obj.UsoCFDI = "";
                    //obj.Activo = "";
                    //obj.IdUsuario = "";
                    //obj.FechaModificacion = obj.FechaModificacion;
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cRfcBL.GetByRfc.Exception", ex, "GetRfcByIdpredio--Parámetros rfc: idContribuyente" + idContribuyente);
            }
            return obj;
        }


    }

}
