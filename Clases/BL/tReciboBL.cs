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
	 public class tReciboBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public tReciboBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(tRecibo obj)
		 {
            string path_Name_File = System.Web.HttpContext.Current.Server.MapPath("~/") + "/Log/" + string.Format("{0:yyyy_MM_dd}", DateTime.Now) + ".txt";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path_Name_File, true);
            file.WriteLine("**************************");
            file.WriteLine("Fecha Hora de la insercion del recibo: " + DateTime.Now.ToString());
            file.WriteLine("idtramite=" + obj.IdTramite);
            file.WriteLine("Contribuyente=" + obj.Contribuyente);
            file.WriteLine("DatosPredio=" + obj.DatosPredio);
            file.WriteLine("**************************");
            file.Close();
            MensajesInterfaz Insert;
			 try
			 {              
                 Predial.tRecibo.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (ex.Message != null)
                {
                    sb.AppendLine("Message=> " + ex.Message.ToString());
                }
                if (ex.InnerException != null)
                {
                    sb.AppendLine(" <------------> ");
                    sb.AppendLine("InnerException=> " + ex.InnerException.ToString());
                }
                if (ex.GetType() != null)
                {
                    sb.AppendLine(" <------------> ");
                    sb.AppendLine("GetType=> " + ex.GetType().Name.ToString());
                }
                if (ex.StackTrace != null)
                {
                    sb.AppendLine(" <------------> ");
                    sb.AppendLine("StackTrace=> " + ex.StackTrace.ToString());
                }              
                sb.AppendLine(" Exception: " + ex.ToString());
                file = new System.IO.StreamWriter(path_Name_File, true);
                file.WriteLine("**************************");
                file.WriteLine("Fecha Hora de la insercion del recibo: " + DateTime.Now.ToString());
                file.WriteLine("idtramite=" + obj.IdTramite);
                file.WriteLine("Contribuyente=" + obj.Contribuyente);
                file.WriteLine("DatosPredio=" + obj.DatosPredio);
                file.WriteLine("**************************");
                file.WriteLine(sb.ToString());
                file.Close();
                new Utileria().logError("tReciboBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (ex.Message != null)
                {
                    sb.AppendLine("Message=> " + ex.Message.ToString());
                }
                if (ex.InnerException != null)
                {
                    sb.AppendLine(" <------------> ");
                    sb.AppendLine("InnerException=> " + ex.InnerException.ToString());
                }
                if (ex.GetType() != null)
                {
                    sb.AppendLine(" <------------> ");
                    sb.AppendLine("GetType=> " + ex.GetType().Name.ToString());
                }
                if (ex.StackTrace != null)
                {
                    sb.AppendLine(" <------------> ");
                    sb.AppendLine("StackTrace=> " + ex.StackTrace.ToString());
                }                
                sb.AppendLine(" Exception: " + ex.ToString());
                file = new System.IO.StreamWriter(path_Name_File, true);
                file.WriteLine("**************************");
                file.WriteLine("Fecha Hora de la insercion del recibo: " + DateTime.Now.ToString());
                file.WriteLine("idtramite=" + obj.IdTramite);
                file.WriteLine("Contribuyente=" + obj.Contribuyente);
                file.WriteLine("DatosPredio=" + obj.DatosPredio);
                file.WriteLine("**************************");
                file.WriteLine(sb.ToString());
                file.Close();
                new Utileria().logError("tReciboBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (ex.Message != null)
                {
                    sb.AppendLine("Message=> " + ex.Message.ToString());
                }
                if (ex.InnerException != null)
                {
                    sb.AppendLine(" <------------> ");
                    sb.AppendLine("InnerException=> " + ex.InnerException.ToString());
                }
                if (ex.GetType() != null)
                {
                    sb.AppendLine(" <------------> ");
                    sb.AppendLine("GetType=> " + ex.GetType().Name.ToString());
                }
                if (ex.StackTrace != null)
                {
                    sb.AppendLine(" <------------> ");
                    sb.AppendLine("StackTrace=> " + ex.StackTrace.ToString());
                }
                sb.AppendLine(" Exception: " + ex.ToString());
                file = new System.IO.StreamWriter(path_Name_File, true);
                file.WriteLine("**************************");
                file.WriteLine("Fecha Hora de la insercion del recibo: " + DateTime.Now.ToString());
                file.WriteLine("idtramite=" + obj.IdTramite);
                file.WriteLine("Contribuyente=" + obj.Contribuyente);
                file.WriteLine("DatosPredio=" + obj.DatosPredio);
                file.WriteLine("**************************");
                file.WriteLine(sb.ToString());
                file.Close();
                new Utileria().logError("tReciboBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(tRecibo obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 tRecibo objOld = Predial.tRecibo.FirstOrDefault(c => c.Id == obj.Id);
                if (objOld.Ruta.Length > 1)
                {
                    Utilerias.Utileria.Compare(obj, objOld);
                }
                 objOld.IdCaja = obj.IdCaja;
				 objOld.FechaPago = obj.FechaPago;
                 objOld.EstadoRecibo = obj.EstadoRecibo;
				 objOld.Contribuyente = obj.Contribuyente;
				 objOld.Rfc = obj.Rfc;
				 objOld.Domicilio = obj.Domicilio;
				 objOld.ImportePagado = obj.ImportePagado;
				 objOld.ImporteNeto = obj.ImporteNeto;
				 objOld.ImporteDescuento = obj.ImporteDescuento;
				 objOld.MaquinaPago = obj.MaquinaPago;
				 objOld.IdUsuarioCancela = obj.IdUsuarioCancela;
				 objOld.MotivoCancelacion = obj.MotivoCancelacion;
				 objOld.IdUsuarioCobra = obj.IdUsuarioCobra;
				 objOld.IdDescuento = obj.IdDescuento;
				 objOld.IdMesaCobro = obj.IdMesaCobro;
				 objOld.IdTipoPago = obj.IdTipoPago;
				 objOld.IdTramite = obj.IdTramite;
                 objOld.FechaCancelacion = obj.FechaCancelacion;				 
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
                 objOld.Ruta = obj.Ruta;
                 objOld.RutaFactura = obj.RutaFactura;
                 objOld.FechaFactura = obj.FechaFactura;
                 objOld.CodigoSeguridad = obj.CodigoSeguridad;
                 objOld.Facturado = obj.Facturado;
                 objOld.RND = obj.RND;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tReciboBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tReciboBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tReciboBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public tRecibo GetByConstraint(int id)
		 {
			 tRecibo obj = null;
			 try
			 {
				 obj = Predial.tRecibo.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tReciboBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(tRecibo obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 tRecibo objOld = Predial.tRecibo.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tReciboBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tReciboBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tReciboBL.Delete.Exception", ex);
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
		 public List<tRecibo> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<tRecibo> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.tRecibo.SqlQuery("Select Id,NoRecibo,FolioPago,IdCaja,FechaPago,IdEstadoRecibo,Contribuyente,Rfc,Domicilio,ImportePagado,ImporteNeto,ImporteDescuento,MaquinaPago,IdUsuarioCancela,MotivoCancelacion,FechaCancelacion,IdUsuarioCobra,IdDescuento,IdMesaCobro,IdTipoPago,IdTramite,Activo,IdUsuario,FechaModificacion from tRecibo where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.tRecibo.SqlQuery("Select Id,NoRecibo,FolioPago,IdCaja,FechaPago,IdEstadoRecibo,Contribuyente,Rfc,Domicilio,ImportePagado,ImporteNeto,ImporteDescuento,MaquinaPago,IdUsuarioCancela,MotivoCancelacion,FechaCancelacion,IdUsuarioCobra,IdDescuento,IdMesaCobro,IdTipoPago,IdTramite,Activo,IdUsuario,FechaModificacion from tRecibo where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.tRecibo.SqlQuery("Select Id,NoRecibo,FolioPago,IdCaja,FechaPago,IdEstadoRecibo,Contribuyente,Rfc,Domicilio,ImportePagado,ImporteNeto,ImporteDescuento,MaquinaPago,IdUsuarioCancela,MotivoCancelacion,FechaCancelacion,IdUsuarioCobra,IdDescuento,IdMesaCobro,IdTipoPago,IdTramite,Activo,IdUsuario,FechaModificacion from tRecibo where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
                          objList = Predial.tRecibo.SqlQuery("Select Id,NoRecibo,FolioPago,IdCaja,FechaPago,IdEstadoRecibo,Contribuyente,Rfc,Domicilio,ImportePagado,ImporteNeto,ImporteDescuento,MaquinaPago,IdUsuarioCancela,MotivoCancelacion,FechaCancelacion,IdUsuarioCobra,IdDescuento,IdMesaCobro,IdTipoPago,IdTramite,Activo,IdUsuario,FechaModificacion from tRecibo where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tReciboBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<tRecibo> GetAll()
		 {
			 List<tRecibo> objList = null;
			 try
			 {
				 objList = Predial.tRecibo.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tReciboBL.GetAll.Exception", ex);
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
                 tRecibo pObject = new tRecibo();
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
                 new Utileria().logError("tReciboBL.ListaCampos.Exception", ex);
             } return propertyList;
		 }

         public List<tRecibo> GetRecibosCajero(int IdUsuario, DateTime FechaInicial, DateTime FechaFinal)
         {
             List<tRecibo> objList = null;
             try
             {
                 objList = Predial.tRecibo.Where(o => o.IdUsuario == IdUsuario && o.FechaPago >= FechaInicial && o.FechaPago <= FechaFinal).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("tReciboBL.GetRecibosCajero.Exception", ex , "--Parámetros IdUsuario:" + IdUsuario + ", FechaInicial:" + FechaInicial + ", FechaFinal:" + FechaFinal);
             }
             return objList;
         }

         public tRecibo GetByIdTramite(int id)
         {
             tRecibo obj = null;
             try
             {
                 obj = Predial.tRecibo.FirstOrDefault(o => o.IdTramite == id && o.Activo == true);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("tReciboBL.GetByIdTramite.Exception", ex , "--Parámetros id:" + id);
             }
             return obj;
         }

         public tRecibo GetReciboCertificado(string clave)
         {           
             tRecibo obj = null;
             try
             {
                obj = Predial.tRecibo.FirstOrDefault(o => o.FechaPago <= DateTime.Now && o.tTramite.IdTipoTramite==1 && o.tTramite.cPredio.ClavePredial==clave);
                
            }
             catch (Exception ex)
             {
                 new Utileria().logError("tReciboBL.GetRecibosCajero.Exception", ex , "--Clave:" + clave);
             }
             return obj;
         }

        public tRecibo GetReciboPredialbyIPredio(int idpredio)
        {
            tRecibo obj = null;
            try
            {
                obj = Predial.tRecibo.Where(o => o.FechaPago <= DateTime.Now && o.tTramite.IdTipoTramite == 5 && o.EstadoRecibo == "P" && o.tTramite.cPredio.Id == idpredio).OrderByDescending(x => x.FechaPago).FirstOrDefault();

            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboBL.GetRecibosCajero.Exception", ex, "--idpredio:" + idpredio.ToString());
            }
            return obj;
        }

        public tRecibo GetReciboCatastrobyIPredio(int idpredio)
        {
            tRecibo obj = null;
            try
            {
                obj = Predial.tRecibo.Where(o => o.FechaPago <= DateTime.Now && o.tTramite.IdTipoTramite == 4 && o.EstadoRecibo == "P" && o.tTramite.cPredio.Id == idpredio).OrderByDescending(x => x.FechaPago).FirstOrDefault();

            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboBL.GetRecibosCajero.Exception", ex, "--idpredio:" + idpredio.ToString());
            }
            return obj;
        }

        public tRecibo GetByCodigoSeguridad(string codigoseguridad)
        {
            tRecibo obj = null;
            try
            {
                obj = Predial.tRecibo.FirstOrDefault(o => o.CodigoSeguridad == codigoseguridad);
            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboBL.GetByCodigoSeguridad.Exception", ex, "--Parámetros id:" + codigoseguridad);
            }
            return obj;
        }
    }

}
