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
	 public class cSalarioMinimoBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cSalarioMinimoBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cSalarioMinimo obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cSalarioMinimo.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cSalarioMinimoBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cSalarioMinimoBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cSalarioMinimoBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cSalarioMinimo obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cSalarioMinimo objOld = Predial.cSalarioMinimo.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Ejercicio = obj.Ejercicio;
				 objOld.Importe = obj.Importe;
				 objOld.Descripcion = obj.Descripcion;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cSalarioMinimoBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cSalarioMinimoBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cSalarioMinimoBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cSalarioMinimo GetByConstraint(int id)
		 {
			 cSalarioMinimo obj = null;
			 try
			 {
				 obj = Predial.cSalarioMinimo.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cSalarioMinimoBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cSalarioMinimo obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cSalarioMinimo objOld = Predial.cSalarioMinimo.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cSalarioMinimoBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cSalarioMinimoBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cSalarioMinimoBL.Delete.Exception", ex);
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
		 public List<cSalarioMinimo> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cSalarioMinimo> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cSalarioMinimo.SqlQuery("Select Id,Ejercicio,Importe,Descripcion,Activo,IdUsuario,FechaModificacion from cSalarioMinimo where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cSalarioMinimo.SqlQuery("Select Id,Ejercicio,Importe,Descripcion,Activo,IdUsuario,FechaModificacion from cSalarioMinimo where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cSalarioMinimo.SqlQuery("Select Id,Ejercicio,Importe,Descripcion,Activo,IdUsuario,FechaModificacion from cSalarioMinimo where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cSalarioMinimo.SqlQuery("Select Id,Ejercicio,Importe,Descripcion,Activo,IdUsuario,FechaModificacion from cSalarioMinimo where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cSalarioMinimoBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cSalarioMinimo> GetAll()
		 {
			 List<cSalarioMinimo> objList = null;
			 try
			 {
				 objList = Predial.cSalarioMinimo.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cSalarioMinimoBL.GetAll.Exception", ex);
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
                 cSalarioMinimo pObject = new cSalarioMinimo();
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
                 new Utileria().logError("cSalarioMinimoBL.ListaCampos.Exception", ex);
             }
		 return propertyList;
		 }

         public cSalarioMinimo GetByEjercicio(int Ejercicio)
         {
             cSalarioMinimo obj = null;
             try
             {
                 obj = Predial.cSalarioMinimo.FirstOrDefault(o => o.Ejercicio == Ejercicio && o.Activo==true);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cSalarioMinimoBL.GetByEjercicio.Exception", ex , "--Parámetros Ejercicio:" + Ejercicio);
             }
             return obj;
         }

         public List<cSalarioMinimo> GetByPeriodo(int EjercicioInicial, int EjercicioFinal)
         {
             List<cSalarioMinimo> objList = null;
             try
             {
                 objList = Predial.cSalarioMinimo.Where(o => o.Ejercicio >= EjercicioInicial && o.Ejercicio <= EjercicioFinal && o.Activo == true).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cSalarioMinimoBL.GetByPeriodo.Exception", ex , "--Parámetros EjercicioInicial:" + EjercicioInicial + ", EjercicioFinal" + EjercicioFinal);
             }
             return objList;
         }

         public decimal GetSMbyEjercicio(int Ejercicio)
         {
            cSalarioMinimo obj = null;
            decimal sm = 0;
            try
            {
                obj = Predial.cSalarioMinimo.FirstOrDefault(o => o.Ejercicio == Ejercicio && o.Activo == true);
                if (obj == null)
                    sm = 0;
                else
                    sm = obj.Importe;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cSalarioMinimoBL.GetByEjercicio.Exception", ex, "--Parámetros Ejercicio:" + Ejercicio);
            }
            return sm;
         }


    }

}
