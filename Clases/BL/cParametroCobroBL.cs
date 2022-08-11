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
	 public class cParametroCobroBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
         /// 
         
		 public cParametroCobroBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cParametroCobro obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cParametroCobro.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cParametroCobroBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cParametroCobroBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cParametroCobroBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cParametroCobro obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cParametroCobro objOld = Predial.cParametroCobro.FirstOrDefault(c => c.Id == obj.Id);
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
				 new Utileria().logError("cParametroCobroBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cParametroCobroBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cParametroCobroBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cParametroCobro GetByConstraint(int id)
		 {
			 cParametroCobro obj = null;
			 try
			 {
				 obj = Predial.cParametroCobro.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cParametroCobroBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cParametroCobro obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cParametroCobro objOld = Predial.cParametroCobro.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cParametroCobroBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cParametroCobroBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cParametroCobroBL.Delete.Exception", ex);
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
		 public List<cParametroCobro> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cParametroCobro> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cParametroCobro.SqlQuery("Select Id,Clave,Valor,Descripcion,Activo,IdUsuario,FechaModificacion from cParametroCobro where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cParametroCobro.SqlQuery("Select Id,Clave,Valor,Descripcion,Activo,IdUsuario,FechaModificacion from cParametroCobro where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cParametroCobro.SqlQuery("Select Id,Clave,Valor,Descripcion,Activo,IdUsuario,FechaModificacion from cParametroCobro where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cParametroCobro.SqlQuery("Select Id,Clave,Valor,Descripcion,Activo,IdUsuario,FechaModificacion from cParametroCobro where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cParametroCobroBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cParametroCobro> GetAll()
		 {
			 List<cParametroCobro> objList = null;
			 try
			 {
				 objList = Predial.cParametroCobro.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cParametroCobroBL.GetAll.Exception", ex);
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
                 cParametroCobro pObject = new cParametroCobro();
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
                 new Utileria().logError("cParametroCobroBL.ListaCampos.Exception", ex);
             }
		    return propertyList;
		 }

         public decimal  GetByClave(string Clave)
         {
             cParametroCobro obj = null;
             try
             {
                 obj = Predial.cParametroCobro.FirstOrDefault(o => o.Clave == Clave);
                 if (obj == null) return -1;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cParametroCobroBL.GetByClave.Exception", ex , "--Parámetros Clave:" + Clave);
             }
             return obj.Valor;
         }

         public object GetDiccionaryValor()
         {
             List<cParametroCobro> objList = null;            
             Dictionary<string, decimal> dCobro = new Dictionary<string, decimal>();
             try
             {
                 objList = GetAll();

                    foreach (var prop in objList)
                    {                        
                        dCobro.Add(prop.Clave,Convert.ToDecimal(prop.Valor));                         
                    }
                    
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cParametroCobroBL.GetDiccionaryValor.Exception", ex);
             }
             return dCobro;
         }

	 }     

}
