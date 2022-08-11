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
	 public class tInternetBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public tInternetBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(tInternet obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.tInternet.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tInternetBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tInternetBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tInternetBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(tInternet Obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 tInternet objOld = Predial.tInternet.FirstOrDefault(c => c.Id == Obj.Id);
                 Utilerias.Utileria.Compare(Obj, objOld); 
                objOld.Id = Obj.Id;
                objOld.IdPredio = Obj.IdPredio;
                objOld.IdOrden = Obj.IdOrden;
                objOld.Ejercicio = Obj.Ejercicio;
                objOld.Mes = Obj.Mes;
                objOld.NoAutorizacion = Obj.NoAutorizacion;
                objOld.FechaPago = Obj.FechaPago;
                objOld.Estado = Obj.Estado;
                objOld.ImportePagado = Obj.ImportePagado;
                objOld.IdMesaIP = Obj.IdMesaIP;
                objOld.IdMesaSM = Obj.IdMesaSM;
                objOld.IdTipoPago = Obj.IdTipoPago;
                objOld.IdDiferencia = Obj.IdDiferencia;
                objOld.IdRequerimiento = Obj.IdRequerimiento;
                objOld.PeriodoPagadoIP = Obj.PeriodoPagadoIP;
                objOld.PeriodoPagadoSM = Obj.PeriodoPagadoSM;
                objOld.IdReciboIP = Obj.IdReciboIP;
                objOld.IdReciboSM = Obj.IdReciboSM;
                objOld.Activo = Obj.Activo;
                objOld.IdUsuario = Obj.IdUsuario;
                objOld.FechaModificacion = Obj.FechaModificacion;
                Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tInternetBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tInternetBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tInternetBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public tInternet GetByConstraint(int id)
		 {
			 tInternet obj = null;
			 try
			 {
				 obj = Predial.tInternet.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tInternetBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(tInternet obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 tInternet objOld = Predial.tInternet.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tInternetBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tInternetBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tInternetBL.Delete.Exception", ex);
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
		 public List<tInternet> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<tInternet> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.tInternet.SqlQuery("Select Id,IdPredio,IdOrden,Ejercicio,Mes,NoAutorizacion,FechaPago,Estado,ImportePagado,IdMesaIP,IdMesaSM,IdTipoPago,IdDiferencia,IdRequerimiento,PeriodoPagadoIP,PeriodoPagadoSM,IdReciboIP,IdReciboSM, Activo,IdUsuario,FechaModificacion   from tInternet where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.tInternet.SqlQuery("Select Id,IdPredio,IdOrden,Ejercicio,Mes,NoAutorizacion,FechaPago,Estado,ImportePagado,IdMesaIP,IdMesaSM,IdTipoPago,IdDiferencia,IdRequerimiento,PeriodoPagadoIP,PeriodoPagadoSM,IdReciboIP,IdReciboSM ,Activo,IdUsuario,FechaModificacion   from tInternet where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.tInternet.SqlQuery("Select Id,IdPredio,IdOrden,Ejercicio,Mes,NoAutorizacion,FechaPago,Estado,ImportePagado,IdMesaIP,IdMesaSM,IdTipoPago,IdDiferencia,IdRequerimiento,PeriodoPagadoIP,PeriodoPagadoSM,IdReciboIP,IdReciboSM, Activo,IdUsuario,FechaModificacion  from tInternet where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
                          objList = Predial.tInternet.SqlQuery("Select Id,IdPredio,IdOrden,Ejercicio,Mes,NoAutorizacion,FechaPago,Estado,ImportePagado,IdMesaIP,IdMesaSM,IdTipoPago,IdDiferencia,IdRequerimiento,PeriodoPagadoIP,PeriodoPagadoSM,IdReciboIP,IdReciboSM, Activo,IdUsuario,FechaModificacion  from tInternet where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tInternetBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }

         public List<tInternet> GetFilterPago(string campoFiltro, string valorFiltro,  string campoSort, string tipoSort)
         {
             List<tInternet> objList = null;
             try
             {
                 String ejercicio = DateTime.Now.Year.ToString();
                 String mes = DateTime.Now.Month.ToString();
                 
                              
                 if (mes == "1")
                 {
                     mes = mes + ",12";
                     ejercicio = ejercicio + "," +( DateTime.Now.Year - 1);
                 }
                 else
                 {
                     mes = mes + "," + (DateTime.Now.Month - 1);
                 }

                 if (campoFiltro == string.Empty)
                 {

                     objList = Predial.tInternet.SqlQuery("Select Id,IdPredio,IdOrden,Ejercicio,Mes,NoAutorizacion,FechaPago,Estado,ImportePagado,IdMesaIP,IdMesaSM,IdTipoPago,IdDiferencia,IdRequerimiento,PeriodoPagadoIP,PeriodoPagadoSM,IdReciboIP,IdReciboSM, Activo,IdUsuario,FechaModificacion   from tInternet where activo=1 and Estado ='T' and Ejercicio in ("+ejercicio+ ") and Mes in ("+mes+") order by " + campoSort + " " + tipoSort).ToList();
                 }
                 else
                 {

                     objList = Predial.tInternet.SqlQuery("Select Id,IdPredio,IdOrden,Ejercicio,Mes,NoAutorizacion,FechaPago,Estado,ImportePagado,IdMesaIP,IdMesaSM,IdTipoPago,IdDiferencia,IdRequerimiento,PeriodoPagadoIP,PeriodoPagadoSM,IdReciboIP,IdReciboSM, Activo,IdUsuario,FechaModificacion  from tInternet where activo=1 and Estado ='T' and Ejercicio in (" + ejercicio + ") and Mes in (" + mes + ") and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                 }
             }
             catch (Exception ex)
             {
                 new Utileria().logError("tInternetBL.GetFilterPago.Exception", ex,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
             }
             return objList;
         }

		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<tInternet> GetAll()
		 {
			 List<tInternet> objList = null;
			 try
			 {
				 objList = Predial.tInternet.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tInternetBL.GetAll.Exception", ex);
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
                 tInternet pObject = new tInternet();
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
                 new Utileria().logError("tInternetBL.ListaCampos.Exception", ex);
             } return propertyList;
		 }

         public List<tInternet> GetInternetsCajero(int IdUsuario, DateTime FechaInicial, DateTime FechaFinal)
         {
             List<tInternet> objList = null;
             try
             {
                 objList = Predial.tInternet.Where(o => o.IdUsuario == IdUsuario && o.FechaPago >= FechaInicial && o.FechaPago <= FechaFinal).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("tInternetBL.GetInternetsCajero.Exception", ex , "--Parámetros IdUsuario:" + IdUsuario + ", FechaInicial:" + FechaInicial + ", FechaFinal:" + FechaFinal);
             }
             return objList;
         }

         public tInternet BuscaOrdenIdPagado(string OrderId, string NoAutorizacion)
         {
            tInternet obj = null;
            DateTime fechaInicial = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            fechaInicial = Convert.ToDateTime(fechaInicial.ToShortDateString() + " 00:00:01");
            //DateTime fechaFinal = new DateTime(DateTime.Today.Year, DateTime.Today.Month + 1, 1).AddDays(-1);
            DateTime fechaFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            fechaFinal = Convert.ToDateTime(fechaFinal.ToShortDateString() + " 23:59:59");
           
            try
            {
                obj = Predial.tInternet.Where(o => o.IdOrden == OrderId && o.NoAutorizacion == NoAutorizacion && o.FechaModificacion >= fechaInicial && o.FechaModificacion <= fechaFinal ).FirstOrDefault();

                if (obj != null)
                {
                    if (obj.PeriodoPagadoIP != null && obj.IdReciboIP == null)
                        obj = null;
                }

                
            }
            catch (Exception ex)
            {
                new Utileria().logError("tInternetBL.BuscaOrdenIdPagado.Exception", ex, "--Parámetros OrderId:" + OrderId + ", No autorizacion: " + NoAutorizacion); // FechaInicial:" + FechaInicial + ", FechaFinal:" + FechaFinal);
            }
            return obj;
        }


        public string BuscaOrdenIdPagadoBnt(string OrderId, string NoAutorizacion)
        {
            string[] orden = OrderId.Split('-');
           
            tInternet obj = null;

            if (orden[2] != "")
            {
                int id = Convert.ToInt32(orden[2]);
                obj = Predial.tInternet.Where(o => o.Id == id && o.NoAutorizacion == NoAutorizacion).FirstOrDefault();
                //si lo encuentra es que ya esta pagada.
                if (obj != null)
                    return "PAGADA";
            }
            return "";
        }



    }

}
