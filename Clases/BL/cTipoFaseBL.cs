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
	 public class cTipoFaseBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cTipoFaseBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cTipoFase obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cTipoFase.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTipoFaseBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cTipoFaseBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoFaseBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cTipoFase obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cTipoFase objOld = Predial.cTipoFase.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Fase = obj.Fase;
				objOld.Descripcion = obj.Descripcion;
				objOld.FaseSiguiente = obj.FaseSiguiente;
				objOld.PorcentajeEjecucion = obj.PorcentajeEjecucion;
				objOld.SalarioMinimoMultas = obj.SalarioMinimoMultas;
				objOld.DoctoImprimir = obj.DoctoImprimir;
                objOld.Cobranza = obj.Cobranza;
				objOld.Activo = obj.Activo;
				objOld.IdUsuario = obj.IdUsuario;
				objOld.FechaModificacion = obj.FechaModificacion;
				Predial.SaveChanges();
				Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTipoFaseBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cTipoFaseBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoFaseBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cTipoFase GetByConstraint(int id)
		 {
			 cTipoFase obj = null;
			 try
			 {
				 obj = Predial.cTipoFase.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cTipoFaseBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cTipoFase obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cTipoFase objOld = Predial.cTipoFase.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTipoFaseBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cTipoFaseBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoFaseBL.Delete.Exception", ex);
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
		 public List<cTipoFase> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cTipoFase> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cTipoFase.SqlQuery("SELECT Id,Fase,Descripcion,FaseSiguiente,PorcentajeEjecucion,SalarioMinimoMultas,DoctoImprimir,Cobranza,Activo,IdUsuario,FechaModificacion FROM cTipoFase where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.cTipoFase.SqlQuery("SELECT Id,Fase,Descripcion,FaseSiguiente,PorcentajeEjecucion,SalarioMinimoMultas,DoctoImprimir,Cobranza,Activo,IdUsuario,FechaModificacion FROM cTipoFase where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cTipoFase.SqlQuery("SELECT Id,Fase,Descripcion,FaseSiguiente,PorcentajeEjecucion,SalarioMinimoMultas,DoctoImprimir,Cobranza,Activo,IdUsuario,FechaModificacion FROM cTipoFase where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
                          objList = Predial.cTipoFase.SqlQuery("SELECT Id,Fase,Descripcion,FaseSiguiente,PorcentajeEjecucion,SalarioMinimoMultas,DoctoImprimir,Cobranza,Activo,IdUsuario,FechaModificacion FROM cTipoFase where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cTipoFaseBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
             }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cTipoFase> GetAll()
		 {
			 List<cTipoFase> objList = null;
			 try
			 {
				 objList = Predial.cTipoFase.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTipoFaseBL.GetAll.Exception", ex);
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
                 cTipoFase pObject = new cTipoFase();
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
                 new Utileria().logError("cTipoFaseBL.ListaCampos.Exception", ex);
             }
			 
		 return propertyList;
		 }
                 
        public cTipoFase GetByFase(int fase)
        {
            cTipoFase obj = null;
            try
            {
                obj = Predial.cTipoFase.FirstOrDefault(o => o.Fase == fase && o.Activo == true);
                if (obj == null)
                    obj = new cTipoFase();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cTipoFaseBL.GetByFase.Exception", ex, "--Parámetros fase:" + fase);
            }
            return obj;
        }

        public cTipoFase GetByFase(string fase)
        {
            cTipoFase obj = null;
            try
            {
                obj = Predial.cTipoFase.FirstOrDefault(o => o.Descripcion == fase && o.Activo == true);
                if (obj == null)
                    obj = new cTipoFase();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cTipoFaseBL.GetByFase.Exception", ex, "--Parámetros fase:" + fase);
            }
            return obj;
        }

        public cTipoFase GetByFaseSiguiente(int faseSig)
        {
            cTipoFase obj = null;
            try
            {
                obj = Predial.cTipoFase.FirstOrDefault(o => o.FaseSiguiente == faseSig && o.Activo == true);
                if (obj == null)
                    obj = new cTipoFase();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cTipoFaseBL.GetByFaseSiguiente.Exception", ex, "--Parámetros faseSiguiente:" + faseSig);
            }
            return obj;
        }


    }
}


