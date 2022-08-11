using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core;
using System.Data;
using Clases.Utilerias;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;


namespace Clases.BL
{
	 /// <summary>
	 /// 
	 /// </summary>
	 public class cBaseImpuestoBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cBaseImpuestoBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cBaseImpuesto obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cBaseImpuesto.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cBaseImpuestoBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cBaseImpuestoBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cBaseImpuestoBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cBaseImpuesto obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cBaseImpuesto objOld = Predial.cBaseImpuesto.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Ejercicio = obj.Ejercicio;
				 objOld.Valor = obj.Valor;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cBaseImpuestoBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cBaseImpuestoBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cBaseImpuestoBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cBaseImpuesto GetByConstraint(int id)
		 {
			 cBaseImpuesto obj = null;
			 try
			 {
				 obj = Predial.cBaseImpuesto.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cBaseImpuestoBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cBaseImpuesto obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cBaseImpuesto objOld = Predial.cBaseImpuesto.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cBaseImpuestoBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cBaseImpuestoBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cBaseImpuestoBL.Delete.Exception", ex);
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
		 public List<cBaseImpuesto> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cBaseImpuesto> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cBaseImpuesto.SqlQuery("Select Id,Ejercicio,Valor,Activo,IdUsuario,FechaModificacion from cBaseImpuesto where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cBaseImpuesto.SqlQuery("Select Id,Ejercicio,Valor,Activo,IdUsuario,FechaModificacion from cBaseImpuesto where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cBaseImpuesto.SqlQuery("Select Id,Ejercicio,Valor,Activo,IdUsuario,FechaModificacion from cBaseImpuesto where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cBaseImpuesto.SqlQuery("Select Id,Ejercicio,Valor,Activo,IdUsuario,FechaModificacion from cBaseImpuesto where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cBaseImpuestoBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cBaseImpuesto> GetAll()
		 {
			 List<cBaseImpuesto> objList = null;
			 try
			 {
				 objList = Predial.cBaseImpuesto.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cBaseImpuestoBL.GetAll.Exception", ex);
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
                 cBaseImpuesto pObject = new cBaseImpuesto();
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
                 new Utileria().logError("cBaseImpuestoBL.ListaCampos.Exception", ex);
             }
			 
		 return propertyList;
		 }

         public List<cBaseImpuesto> GetByPeriodo(int EjercicioInicial, int EjercicioFinal)
         {
             List<cBaseImpuesto> objList = null;
             try
             {
                 objList = Predial.cBaseImpuesto.Where(o => o.Ejercicio >= EjercicioInicial && o.Ejercicio <= EjercicioFinal && o.Activo == true).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cBaseImpuestoBL.GetByPeriodo.Exception", ex , "--Parámetros EjercicioInicial:" + EjercicioInicial +
                     ", EjercicioFinal:" + EjercicioFinal);
             }
             return objList;
         }

         public decimal GetByEjercicio(int ejercicio)
         {
             cBaseImpuesto obj = new cBaseImpuesto();
             try
             {
                obj = Predial.cBaseImpuesto.FirstOrDefault(o => o.Ejercicio == ejercicio && o.Activo == true);
                if (obj == null) return 0;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cBaseImpuestoBL.GetByEjercicio.Exception", ex , "--Parámetros ejercicio:" + ejercicio);
             }
            
             return obj.Valor;
         }


         public List<cBaseImpuesto> GetByEjercicioInicial(int ejercicio)
         {
             List<cBaseImpuesto> objList = null;
             try
             {
                 objList = Predial.cBaseImpuesto.Where(o => o.Ejercicio >= ejercicio &&  o.Activo == true).OrderBy(r=> r.Ejercicio).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cBaseImpuestoBL.GetByEjercicioInicial.Exception", ex , "--Parámetros ejercicio:" + ejercicio);
             }
             return objList;
         }

     }
    
}
