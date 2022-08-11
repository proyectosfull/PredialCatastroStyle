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
	 public class cContribuyenteBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cContribuyenteBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cContribuyente obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cContribuyente.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cContribuyenteBLBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cContribuyenteBLBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cContribuyenteBLBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cContribuyente obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				cContribuyente objOld = Predial.cContribuyente.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Nombre = obj.Nombre;
				objOld.ApellidoPaterno = obj.ApellidoPaterno;
				objOld.ApellidoMaterno = obj.ApellidoMaterno;
				objOld.Calle = obj.Calle;
				objOld.Numero = obj.Numero;
				objOld.Colonia = obj.Colonia;
				objOld.Localidad = obj.Localidad;
				objOld.Municipio = obj.Municipio;
				objOld.Estado = obj.Estado;
				objOld.CP = obj.CP;
				objOld.Email = obj.Email;
				objOld.Telefono = obj.Telefono;
				objOld.Curp = obj.Curp;			
				objOld.Activo = obj.Activo;
				objOld.IdUsuario = obj.IdUsuario;
				objOld.FechaModificacion = obj.FechaModificacion;
                objOld.AdultoMayor = obj.AdultoMayor;
                objOld.RazonSocial = obj.RazonSocial;
                objOld.IdPropietarioTitular = obj.IdPropietarioTitular;
                objOld.PorcCoPropietario = obj.PorcCoPropietario;

                 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cContribuyenteBLBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cContribuyenteBLBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cContribuyenteBLBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cContribuyente GetByConstraint(int id)
		 {
			 cContribuyente obj = null;
			 try
			 {
				 obj = Predial.cContribuyente.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cContribuyenteBL.GetByConstraint.Exception", ex);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cContribuyente obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cContribuyente objOld = Predial.cContribuyente.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cContribuyenteBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cContribuyenteBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cContribuyenteBL.Delete.Exception", ex);
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
		 public List<cContribuyente> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cContribuyente> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cContribuyente.SqlQuery("Select Id,Nombre +' '+ApellidoPaterno+' '+ApellidoMaterno+' '+RazonSocial as Nombre,Calle,Numero,Colonia,Localidad,Municipio,Estado,CP,Email,Telefono,Curp,Activo,IdUsuario,FechaModificacion,AdultoMayor, IdPropietarioTitular,PorcCoPropietario,IdTipoContribuyente, Referencia  from cContribuyente where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cContribuyente.SqlQuery("Select Id,Nombre +' '+ApellidoPaterno+' '+ApellidoMaterno+' '+RazonSocial as Nombre,Calle,Numero,Colonia,Localidad,Municipio,Estado,CP,Email,Telefono,Curp,Activo,IdUsuario,FechaModificacion,AdultoMayor,IdPropietarioTitular,PorcCoPropietario,IdTipoContribuyente, Referencia  from cContribuyente where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
                    //string sql = "SELECT * from YourTable
                    //       where SomeValue = @p0 and SomeOtherValue = @p1";
                    //        List<object> parameters = new List<object>(){
                    //            new "Value 1",
                    //            new "Value 2"
                    //        }.ToArray();

                    valorFiltro = "'%" + valorFiltro + "%'";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.cContribuyente.SqlQuery("Select Id,Nombre +' '+ApellidoPaterno+' '+ApellidoMaterno+' '+RazonSocial as Nombre,ApellidoPaterno, ApellidoMaterno, RazonSocial,Calle, Numero,Colonia,Localidad,Municipio,Estado,CP,Email,Telefono,Curp,Activo,IdUsuario,FechaModificacion,AdultoMayor,IdPropietarioTitular,PorcCoPropietario,IdTipoContribuyente, Referencia from cContribuyente where activo=1 and " + campoFiltro + " like " + valorFiltro + " order by " + campoSort + " " + tipoSort).ToList(); //, new SqlParameter("p", valorFiltro)).ToList();
                    else
                        objList = Predial.cContribuyente.SqlQuery("Select Id,Nombre +' '+ApellidoPaterno+' '+ApellidoMaterno+' '+RazonSocial as Nombre,ApellidoPaterno, ApellidoMaterno, RazonSocial,Calle,Numero,Colonia,Localidad,Municipio,Estado,CP,Email,Telefono,Curp,Activo,IdUsuario,FechaModificacion,AdultoMayor ,IdPropietarioTitular,PorcCoPropietario,IdTipoContribuyente, Referencia from cContribuyente where activo=0  order by " + campoSort + " " + tipoSort).ToList(); //, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cContribuyenteBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cContribuyente> GetAll()
		 {
			 List<cContribuyente> objList = null;
			 try
			 {
				 objList = Predial.cContribuyente.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cContribuyenteBL.GetAll.Exception", ex);
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
             string concat = string.Empty;
             try
             {
                 cContribuyente pObject = new cContribuyente();
                 if (pObject != null)
                 {
                     foreach (var prop in pObject.GetType().GetProperties())
                     {
                        if (prop.Name.ToUpper() != "ID" && prop.Name.ToUpper() != "IDUSUARIO" && prop.Name.ToUpper() != "ACTIVO" && prop.Name.ToUpper() != "FECHAMODIFICACION" && prop.Name.Substring(0, 1) != "c" && prop.Name.Substring(0, 1) != "t" && prop.Name.Substring(0, 1) != "m" && prop.Name.Substring(0, 1) != "I" && prop.Name.Substring(0, 1) != "P")
                            //if ((prop.Name == "Nombre") || (prop.Name == "ApellidoPaterno") || (prop.Name == "ApellidoMaterno") || (prop.Name == "RazonSocial") ||
                            //   (prop.Name == "Calle")|| (prop.Name == "IdColonia"))
                            //{
                            //    concat = "Nombre del Contribuyente";
                            //    concat2 = "Domicilio"
                            //}
                            propertyList.Add(prop.Name);
                     }
                 }
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cContribuyenteBL.ListaCampos.Exception", ex);
             }
			
		 return propertyList;
		 }

        public List<cContribuyente> GetAutoCompleteByName(string prefix)
        {
            List<cContribuyente> objList = null;
            try
            {
                objList = Predial.cContribuyente.Where(o => o.Activo == true && (o.RazonSocial.Trim() + " " + o.ApellidoPaterno.Trim() + " " + o.ApellidoMaterno.Trim() + " " + o.Nombre.Trim()).Contains(prefix)).OrderBy(i => i.Nombre).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cContribuyenteBL.GetAutoCompleteByName.Exception", ex);
            }
            return objList;
        }
        public string GetByName(string name)
        {
            string obj = "";
            string filtro = name.Trim();
            try
            {
                cContribuyente objList = Predial.cContribuyente.FirstOrDefault(o => o.Activo == true && o.ApellidoPaterno.Trim() + " " + o.ApellidoMaterno.Trim() + " " + o.Nombre.Trim() + " " + o.RazonSocial == filtro);
                if (objList != null)
                    obj = objList.Id.ToString();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cContribuyenteBL.GetAutoCompleteByName.Exception", ex);
            }
            return obj;
        }
    }

}
