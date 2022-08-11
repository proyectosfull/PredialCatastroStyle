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
	 /// </summary>1
	 public class cDescuentoBL
	 {
		 PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        
		 public cDescuentoBL()
		 { 
			Predial = new PredialEntities();           

		 }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Insert(cDescuento obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cDescuento.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cDescuentoBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cDescuentoBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cDescuentoBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cDescuento obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cDescuento objOld = Predial.cDescuento.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Clave = obj.Clave;
				 objOld.Ejercicio = obj.Ejercicio;
				 objOld.Descripcion = obj.Descripcion;
				 objOld.Autorizacion = obj.Autorizacion;
				 objOld.FechaInicio = obj.FechaInicio;
				 objOld.FechaFin = obj.FechaFin;
				 objOld.AnticipadoImpuesto = obj.AnticipadoImpuesto;
				 objOld.AnticipadoAdicional = obj.AnticipadoAdicional;
				 objOld.AnticipadoLimpieza = obj.AnticipadoLimpieza;
				 objOld.AnticipadoRecoleccion = obj.AnticipadoRecoleccion;
				 objOld.AnticipadoDap = obj.AnticipadoDap;
				 objOld.ActualImpuesto = obj.ActualImpuesto;
				 objOld.ActualAdicional = obj.ActualAdicional;
				 objOld.ActualRecargo = obj.ActualRecargo;
				 objOld.ActualLImpieza = obj.ActualLImpieza;
				 objOld.ActualRecoleccion = obj.ActualRecoleccion;
				 objOld.ActualDap = obj.ActualDap;
				 objOld.Diferencia = obj.Diferencia;
				 objOld.DiferenciaRecargo = obj.DiferenciaRecargo;
				 objOld.Rezago = obj.Rezago;
				 objOld.RezagoRecargo = obj.RezagoRecargo;
				 objOld.RezagoAdicional = obj.RezagoAdicional;
				 objOld.Basegravable = obj.Basegravable;
				 objOld.Importe = obj.Importe;
				 objOld.Multas = obj.Multas;
				 objOld.Ejecucion = obj.Ejecucion;
                 objOld.Honorarios = obj.Honorarios;
                 objOld.AnticipadoInfraestructura = obj.AnticipadoInfraestructura;
                 objOld.Infraestructura = obj.Infraestructura;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;


			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cDescuentoBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cDescuentoBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cDescuentoBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cDescuento GetByConstraint(int id)
		 {
			 cDescuento obj = null;
			 try
			 {
				 obj = Predial.cDescuento.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cDescuentoBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cDescuento obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cDescuento objOld = Predial.cDescuento.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cDescuentoBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cDescuentoBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cDescuentoBL.Delete.Exception", ex);
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
		 public List<cDescuento> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cDescuento> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cDescuento.SqlQuery("SELECT Id,Clave,Ejercicio,Descripcion,Autorizacion,FechaInicio,FechaFin,AnticipadoImpuesto,AnticipadoAdicional,AnticipadoLimpieza,AnticipadoRecoleccion,AnticipadoDap,ActualImpuesto,ActualAdicional,ActualRecargo,ActualLImpieza,ActualRecoleccion,ActualDap,Diferencia,DiferenciaRecargo,Rezago,RezagoRecargo,RezagoAdicional,Basegravable,Importe,Multas,Honorarios,Ejecucion,AnticipadoInfraestructura,Infraestructura,Activo,IdUsuario,FechaModificacion,Honorarios FROM cDescuento where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.cDescuento.SqlQuery("SELECT Id,Clave,Ejercicio,Descripcion,Autorizacion,FechaInicio,FechaFin,AnticipadoImpuesto,AnticipadoAdicional,AnticipadoLimpieza,AnticipadoRecoleccion,AnticipadoDap,ActualImpuesto,ActualAdicional,ActualRecargo,ActualLImpieza,ActualRecoleccion,ActualDap,Diferencia,DiferenciaRecargo,Rezago,RezagoRecargo,RezagoAdicional,Basegravable,Importe,Multas,Honorarios,Ejecucion,AnticipadoInfraestructura,Infraestructura,Activo,IdUsuario,FechaModificacion,Honorarios FROM cDescuento where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cDescuento.SqlQuery("SELECT Id,Clave,Ejercicio,Descripcion,Autorizacion,FechaInicio,FechaFin,AnticipadoImpuesto,AnticipadoAdicional,AnticipadoLimpieza,AnticipadoRecoleccion,AnticipadoDap,ActualImpuesto,ActualAdicional,ActualRecargo,ActualLImpieza,ActualRecoleccion,ActualDap,Diferencia,DiferenciaRecargo,Rezago,RezagoRecargo,RezagoAdicional,Basegravable,Importe,Multas,Honorarios,Ejecucion,AnticipadoInfraestructura,Infraestructura,Activo,IdUsuario,FechaModificacion,Honorarios FROM cDescuento where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
                          objList = Predial.cDescuento.SqlQuery("SELECT Id,Clave,Ejercicio,Descripcion,Autorizacion,FechaInicio,FechaFin,AnticipadoImpuesto,AnticipadoAdicional,AnticipadoLimpieza,AnticipadoRecoleccion,AnticipadoDap,ActualImpuesto,ActualAdicional,ActualRecargo,ActualLImpieza,ActualRecoleccion,ActualDap,Diferencia,DiferenciaRecargo,Rezago,RezagoRecargo,RezagoAdicional,Basegravable,Importe,Multas,Honorarios,Ejecucion,AnticipadoInfraestructura,Infraestructura,Activo,IdUsuario,FechaModificacion,Honorarios FROM cDescuento where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cDescuentoBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cDescuento> GetAll()
		 {
			 List<cDescuento> objList = null;
			 try
			 {
				 objList = Predial.cDescuento.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cDescuentoBL.GetAll.Exception", ex);
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
                 cDescuento pObject = new cDescuento();
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
                 new Utileria().logError("cDescuentoBL.ListaCampos.Exception", ex);
             }
			
		 return propertyList;
		 }

         public List<cDescuento> GetVigentes()
         {
             List<cDescuento> objList = null;
             try
             {
                 objList = Predial.cDescuento.Where(o => o.FechaFin >=DateTime.Today && o.Activo == true).ToList();                 
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cDescuentoBL.GetVigentes.Exception", ex);
             }
             return objList;
         }
         //Descuento_Automatico
         public cDescuento GetByClave(string clave, DateTime fechaPago)
         {
             cDescuento obj = null;
             try
             {
                 obj = Predial.cDescuento.FirstOrDefault(o => o.Clave == clave && o.FechaInicio <= fechaPago && o.FechaFin >= fechaPago && o.Activo == true);
                 if (obj == null)
                     obj = InicializaVarDescuento();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cDescuentoBL.GetByClave.Exception", ex , "--Parámetros clave:" + clave + ", fechaPago" + fechaPago);
             }
             return obj;
         }

         public cDescuento GetByIdClave(int id, DateTime fechaPago)
         {
             cDescuento obj = null;
             try
             {
                 obj = Predial.cDescuento.FirstOrDefault(o => o.Id == id && o.FechaInicio <= fechaPago && o.FechaFin >= fechaPago && o.Activo == true);
                 if (obj == null)
                     obj = InicializaVarDescuento();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cDescuentoBL.GetByIdClave.Exception", ex , "--Parámetros id:" + id + ", fechaPago" + fechaPago);
             }
             return obj;
         }

         public cDescuento InicializaVarDescuento ()
         {
             cDescuento d = new cDescuento();
             try
             {
                 d.Clave = "";
                 d.Ejercicio = 0;
                 d.Descripcion = "";
                 d.Autorizacion = "";
                 d.FechaInicio = Convert.ToDateTime("1900-01-01");
                 d.FechaFin = Convert.ToDateTime("1900-01-01");
                 d.AnticipadoImpuesto = 0;
                 d.AnticipadoAdicional = 0;
                 d.AnticipadoLimpieza = 0;
                 d.AnticipadoRecoleccion = 0;
                 d.AnticipadoDap = 0;
                 d.ActualImpuesto = 0;
                 d.ActualAdicional = 0;
                 d.ActualRecargo = 0;
                 d.ActualLImpieza = 0;
                 d.ActualRecoleccion = 0;
                 d.ActualDap = 0;
                 d.Diferencia = 0;
                 d.DiferenciaRecargo = 0;
                 d.Rezago = 0;
                 d.RezagoRecargo = 0;
                 d.RezagoAdicional = 0;
                 d.Basegravable = 0;
                 d.Importe = 0;
                 d.Multas = 0;
                 d.Ejecucion = 0;
                 d.Honorarios = 0;
                 d.AnticipadoInfraestructura = 0;
                 d.Infraestructura = 0;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cDescuentoBL.InicializaVarDescuento.Exception", ex);
             }
            
             return d;
         }

	 }

}
