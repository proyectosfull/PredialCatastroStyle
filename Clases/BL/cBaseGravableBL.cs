using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core;
using Clases;
using System.Data;
using Clases.Utilerias;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using Clases.BL;

namespace Clases.BL
{
	 /// <summary>
	 /// 
	 /// </summary>
	 public class cBaseGravableBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cBaseGravableBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cBaseGravable obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cBaseGravable.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cBaseGravableBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cBaseGravableBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cBaseGravableBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }

		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cBaseGravable obj)
		 {
			 MensajesInterfaz Update;
            try
            {
                cBaseGravable objOld = Predial.cBaseGravable.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdPredio = obj.IdPredio;
                objOld.Ejercicio = obj.Ejercicio;
                objOld.Bimestre = obj.Bimestre;
                objOld.Valor = obj.Valor;
                objOld.FechaAvaluo = obj.FechaAvaluo;
                objOld.SuperficieTerreno = obj.SuperficieTerreno;
                objOld.TerrenoPrivativo = obj.TerrenoPrivativo;
                objOld.TerrenoComun = obj.TerrenoComun;
                objOld.ValorTerreno = obj.ValorTerreno;
                objOld.SuperficieConstruccion = obj.SuperficieConstruccion;
                objOld.ConstruccionPrivativa = obj.ConstruccionPrivativa;
                objOld.ConstruccionComun = obj.ConstruccionComun;
                objOld.ValorConstruccion = obj.ValorConstruccion;
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Update = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("cBaseGravableBL.Update.DbUpdateException", ex);
                Update = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("cBaseGravableBL.Update.DataException", ex);
                Update = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cBaseGravableBL.Update.Exception", ex);
                Update = MensajesInterfaz.ErrorGeneral;
            }
			 return Update;
		 }

		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cBaseGravable GetByConstraint(int id)
		 {
			 cBaseGravable obj = null;
			 try
			 {
				 obj = Predial.cBaseGravable.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cBaseGravableBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }

		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cBaseGravable obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cBaseGravable objOld = Predial.cBaseGravable.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cBaseGravableBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cBaseGravableBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cBaseGravableBL.Delete.Exception", ex);
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
		 public List<cBaseGravable> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cBaseGravable> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cBaseGravable.SqlQuery("Select Id,IdPredio,Ejercicio,Bimestre,Valor,FechaAvaluo,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,Activo,IdUsuario,FechaModificacion from cBaseGravable where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cBaseGravable.SqlQuery("Select Id,IdPredio,Ejercicio,Bimestre,Valor,FechaAvaluo,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,Activo,IdUsuario,FechaModificacion from cBaseGravable where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cBaseGravable.SqlQuery("Select Id,IdPredio,Ejercicio,Bimestre,Valor,FechaAvaluo,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,Activo,IdUsuario,FechaModificacion from cBaseGravable where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cBaseGravable.SqlQuery("Select Id,IdPredio,Ejercicio,Bimestre,Valor,FechaAvaluo,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,Activo,IdUsuario,FechaModificacion from cBaseGravable where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cBaseGravableBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }

		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cBaseGravable> GetAll()
		 {
			 List<cBaseGravable> objList = null;
			 try
			 {
				 objList = Predial.cBaseGravable.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cBaseGravableBL.GetAll.Exception", ex);
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
                 cBaseGravable pObject = new cBaseGravable();
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
                 new Utileria().logError("cBaseGravableBL.ListaCampos.Exception", ex);
             }
			 
		 return propertyList;
		 }

         public List<cBaseGravable> GetByPeriodo(int idPredio, int EjercicioInicial, int EjercicioFinal)
         {
             List<cBaseGravable> objList = null;
             try
             {
                 objList = Predial.cBaseGravable.Where(o => o.IdPredio == idPredio && o.Ejercicio >= EjercicioInicial && o.Ejercicio <= EjercicioFinal && o.Activo == true).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cBaseGravableBL.GetByPeriodo.Exception", ex ,
                    "--Parámetros idPredio:" + idPredio + ", EjercicioInicial: " + EjercicioInicial + ", EjercicioFinal:" + EjercicioFinal);
             }
             return objList;
         }

         public cBaseGravable GetLastByIdPredio(int idPredio)
        {
            cBaseGravable obj = null;
            try
            {
                obj = Predial.cBaseGravable.Where(o => o.IdPredio == idPredio && o.Ejercicio>=DateTime.Now.Year && o.Activo==true).OrderByDescending(o => o.Ejercicio).Take(1).SingleOrDefault();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cBaseGravableBL.GetLastByIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio);
            }
            return obj;
        }

         public List<cBaseGravable> GetListByIdPredio(int idPredio)
        {
            List<cBaseGravable> obj = null;
            try
            {
                obj = Predial.cBaseGravable.Where(o => o.IdPredio == idPredio).OrderByDescending(o => o.Ejercicio).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cBaseGravableBL.GetLastByIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio);
            }
            return obj;
        }

         public bool EsUnicaAnual(string clavePredio, int anio)
        {
            bool obj = true;
            try
            {
                obj = !(Predial.cBaseGravable.Where(o => o.cPredio.ClavePredial == clavePredio && o.Ejercicio == anio).Count() > 0);
            }
            catch (Exception ex)
            {
                new Utileria().logError("cBaseGravableBL.EsUnicaAnual.Exception", ex , "--Parámetros clavePredio:" + clavePredio + ", anio:" + anio.ToString());
            }
            return obj;
        }

         public MensajesInterfaz UpdateOtrasBases(string clavePredio, int anio)
        {
            MensajesInterfaz Update;
            try
            {
                var friends = Predial.cBaseGravable.Where(o => o.cPredio.ClavePredial == clavePredio && o.Ejercicio == anio).ToList();
                friends.ForEach(a => a.Activo = false);
                Predial.SaveChanges();
                Update = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("cBaseGravableBL.UpdateOtrasBases.DbUpdateException", ex);
                Update = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("cBaseGravableBL.UpdateOtrasBases.DataException", ex);
                Update = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cBaseGravableBL.UpdateOtrasBases.Exception", ex);
                Update = MensajesInterfaz.ErrorGeneral;
            }
            return Update;
        }

         public cBaseGravable GetByPredAnio(int idPredio, int anio)
         {
            cBaseGravable obj = null;
            try
            {
                obj = Predial.cBaseGravable.FirstOrDefault(o => o.IdPredio == idPredio && o.Ejercicio == anio && o.Activo==true);
            }
            catch (Exception ex)
            {
                new Utileria().logError("cBaseGravableBL.GetLastByIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio);
            }
            return obj;
         }

         public List<cBaseGravable> GetListByIdPredioAct(int idPredio)
        {
            List<cBaseGravable> obj = null;
            try
            {
                obj = Predial.cBaseGravable.Where(o => o.IdPredio == idPredio && o.Activo==true).OrderByDescending(o => o.Ejercicio).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cBaseGravableBL.GetLastByIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio);
            }
            return obj;
        }

         public cBaseGravable GetLastByIdPredioAct(int idPredio)
        {
            cBaseGravable obj = null;
            try
            {
                obj = Predial.cBaseGravable.Where(o => o.IdPredio == idPredio && o.Activo==true).OrderByDescending(o => o.Ejercicio).ThenByDescending(o => o.Bimestre).Take(1).SingleOrDefault();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cBaseGravableBL.GetLastByIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio);
            }
            return obj;
        }

         public cBaseGravable GetUltByIdPredioAct(int idPredio)
        {
            cBaseGravable obj = null;
            try
            {
                obj = Predial.cBaseGravable.Where(o => o.IdPredio == idPredio && o.Activo == true).OrderBy(o => o.Ejercicio).ThenByDescending(o => o.Bimestre).Take(1).SingleOrDefault();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cBaseGravableBL.GetLastByIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio);
            }
            return obj;
        }

         public List<cBaseGravable> GetAllforAnio(int anio)
        {
            List<cBaseGravable> objList = null;
            try
            {
                objList = Predial.cBaseGravable.Where(o => o.Activo == true && o.Ejercicio==anio).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cBaseGravableBL.GetAll.Exception", ex);
            }
            return objList;
        }

        public decimal GetByBasePredEjercicio(int idPredio, int ejercicio)
        {
            cBaseGravable obj = null;
            decimal valor = 0;
            try
            {
                obj = Predial.cBaseGravable.FirstOrDefault(o => o.IdPredio == idPredio && o.Ejercicio == ejercicio && o.Activo == true);
                if (obj == null)
                    valor = 0;
                else
                    valor = obj.Valor;
            }
            catch (Exception ex)
            {
                //new Utileria().logError("cBaseGravableBL.GetLastByIdPredio.Exception", ex, "--Parámetros idPredio:" + idPredio);
                valor = 0;
            }
            
            return valor;
        }

    }

}
