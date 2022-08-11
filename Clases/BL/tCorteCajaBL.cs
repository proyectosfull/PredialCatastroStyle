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
	 public class tCorteCajaBL
	 {
         PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public tCorteCajaBL()
		 {
             Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(tCorteCaja obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
                 Predial.tCorteCaja.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tCorteCajaBL.Insert.UpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tCorteCajaBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tCorteCajaBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(tCorteCaja obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 tCorteCaja objOld = Predial.tCorteCaja.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Fecha = obj.Fecha;
				 objOld.NoCaja = obj.NoCaja;
				 objOld.IdUsuarioCajero = obj.IdUsuarioCajero;
				 objOld.IdMesa = obj.IdMesa;
				 objOld.Equipo = obj.Equipo;
				 objOld.ImporteTotal = obj.ImporteTotal;
				 objOld.RecibosCobrados = obj.RecibosCobrados;
				 objOld.RecibosCancelados = obj.RecibosCancelados;
				 objOld.Turno = obj.Turno;
				 objOld.FechaInicial = obj.FechaInicial;
				 objOld.FechaFinal = obj.FechaFinal;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tCorteCajaBL.Update.UpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tCorteCajaBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tCorteCajaBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public tCorteCaja GetByConstraint(int id)
		 {
			 tCorteCaja obj = null;
			 try
			 {
				 obj = Predial.tCorteCaja.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tCorteCajaBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(tCorteCaja obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 tCorteCaja objOld = Predial.tCorteCaja.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Fecha = obj.Fecha;
				 objOld.NoCaja = obj.NoCaja;
				 objOld.IdUsuarioCajero = obj.IdUsuarioCajero;
				 objOld.IdMesa = obj.IdMesa;
				 objOld.Equipo = obj.Equipo;
				 objOld.ImporteTotal = obj.ImporteTotal;
				 objOld.RecibosCobrados = obj.RecibosCobrados;
				 objOld.RecibosCancelados = obj.RecibosCancelados;
				 objOld.Turno = obj.Turno;
				 objOld.FechaInicial = obj.FechaInicial;
				 objOld.FechaFinal = obj.FechaFinal;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tCorteCajaBL.Delete.UpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tCorteCajaBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tCorteCajaBL.Delete.Exception", ex);
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
		 public List<tCorteCaja> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<tCorteCaja> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.tCorteCaja.SqlQuery("Select Id,Fecha,NoCaja,IdUsuarioCajero,IdMesa,Equipo,ImporteTotal,RecibosCobrados,RecibosCancelados,Turno,FechaInicial,FechaFinal,Activo,IdUsuario,FechaModificacion from tCorteCaja where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.tCorteCaja.SqlQuery("Select Id,Fecha,NoCaja,IdUsuarioCajero,IdMesa,Equipo,ImporteTotal,RecibosCobrados,RecibosCancelados,Turno,FechaInicial,FechaFinal,Activo,IdUsuario,FechaModificacion from tCorteCaja where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.tCorteCaja.SqlQuery("Select Id,Fecha,NoCaja,IdUsuarioCajero,IdMesa,Equipo,ImporteTotal,RecibosCobrados,RecibosCancelados,Turno,FechaInicial,FechaFinal,Activo,IdUsuario,FechaModificacion from tCorteCaja where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.tCorteCaja.SqlQuery("Select Id,Fecha,NoCaja,IdUsuarioCajero,IdMesa,Equipo,ImporteTotal,RecibosCobrados,RecibosCancelados,Turno,FechaInicial,FechaFinal,Activo,IdUsuario,FechaModificacion from tCorteCaja where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tCorteCajaBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<tCorteCaja> GetAll()
		 {
			 List<tCorteCaja> objList = null;
			 try
			 {
				 objList = Predial.tCorteCaja.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tCorteCajaBL.GetAll.Exception", ex);
			 }
			 return objList;
		 }
         public List<vCortes> GetByFechas(DateTime fechaInicio, DateTime fechaFin)
          {
              List<vCortes> objList = null;
			 try
			 {
				 objList = Predial.vCortes.Where(o => o.Activo==true && o.FechaInicial >= fechaInicio && o.FechaFinal <= fechaFin).ToList();
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tCorteCajaBL.GetByFechas.Exception", ex , "--Parámetros fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
			 }
			 return objList;
		 }
         public List<vCortes> GetByFolio(int folio)
         {
             List<vCortes> objList = null;
             try
             {
                 objList = Predial.vCortes.Where(o => o.Activo == true && o.Id==folio).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("tCorteCajaBL.GetByFolio.Exception", ex , "--Parámetros folio:" + folio);
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
                 tCorteCaja pObject = new tCorteCaja();
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
                 new Utileria().logError("tCorteCajaBL.ListaCampos.Exception", ex);
             }
			
		 return propertyList;
		 }
	 }

}
