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
	 public class cParametroSistemaBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cParametroSistemaBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cParametroSistema obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cParametroSistema.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cParametroSistemaBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cParametroSistemaBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cParametroSistemaBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cParametroSistema obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cParametroSistema objOld = Predial.cParametroSistema.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Clave = obj.Clave;
				 objOld.Valor = obj.Valor;
				 objOld.Descripcion = obj.Descripcion;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cParametroSistemaBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cParametroSistemaBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cParametroSistemaBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cParametroSistema GetByConstraint(int id)
		 {
			 cParametroSistema obj = null;
			 try
			 {
				 obj = Predial.cParametroSistema.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cParametroSistemaBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cParametroSistema obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cParametroSistema objOld = Predial.cParametroSistema.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cParametroSistemaBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cParametroSistemaBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cParametroSistemaBL.Delete.Exception", ex);
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
		 public List<cParametroSistema> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cParametroSistema> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cParametroSistema.SqlQuery("Select Id,Clave,Valor,Descripcion,TipoCampo,Activo,IdUsuario,FechaModificacion from cParametroSistema where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.cParametroSistema.SqlQuery("Select Id,Clave,Valor,Descripcion,TipoCampo,Activo,IdUsuario,FechaModificacion from cParametroSistema where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cParametroSistema.SqlQuery("Select Id,Clave,Valor,Descripcion,TipoCampo,Activo,IdUsuario,FechaModificacion from cParametroSistema where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
                          objList = Predial.cParametroSistema.SqlQuery("Select Id,Clave,Valor,Descripcion,TipoCampo,Activo,IdUsuario,FechaModificacion from cParametroSistema where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cParametroSistemaBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cParametroSistema> GetAll()
		 {
			 List<cParametroSistema> objList = null;
			 try
			 {
				 objList = Predial.cParametroSistema.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cParametroSistemaBL.GetAll.Exception", ex);
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
                 cParametroSistema pObject = new cParametroSistema();
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
                 new Utileria().logError("cParametroSistemaBL.ListaCampos.Exception", ex);
             }
		 return propertyList;
		 }

         public string GetValorByClave(string Clave)
         {
             cParametroSistema obj = null;
             try
             {
                 obj = Predial.cParametroSistema.FirstOrDefault(o => o.Clave == Clave && o.Activo == true);
                 if (obj == null) return "";
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cParametroSistemaBL.GetValorByClave.Exception", ex , "--Parámetros Clave:" + Clave);
             }
             return obj.Valor;
         }

        public List<cParametroSistema> GetFilterFin(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<cParametroSistema> objList = null;
            try
            {
                if ( !string.IsNullOrEmpty(campoFiltro) )
                {
                    valorFiltro = valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.cParametroSistema.SqlQuery("Select Id,Clave,Valor,Descripcion,TipoCampo,Activo,IdUsuario,FechaModificacion from cParametroSistema where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.cParametroSistema.SqlQuery("Select Id,Clave,Valor,Descripcion,TipoCampo,Activo,IdUsuario,FechaModificacion from cParametroSistema where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cParametroSistemaBL.GetFilterFin.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }

        public cParametroSistema GetByClave(string clave)
        {
            cParametroSistema obj = null;
            try
            {
                obj = Predial.cParametroSistema.FirstOrDefault(o => o.Clave == clave);
            }
            catch (Exception ex)
            {
                new Utileria().logError("cParametroSistemaBL.GetByClave.Exception", ex, "--Parámetros Clave:" + clave);
            }
            return obj;
        }

        public String GetByTipoCampoCobro(string clave)
        {
            String obj = "";
            try
            {
                obj = Predial.cParametroSistema.Where(o => o.TipoCampo == clave && o.Activo== true && o.Valor=="1").FirstOrDefault().Clave;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cParametroSistemaBL.GetByTipoCampoCobro.Exception", ex , "--Parámetros Clave:" + clave);
            }
            return obj;
        }
    }

}
