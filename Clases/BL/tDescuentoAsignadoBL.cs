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
	 public class tDescuentoAsignadoBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
         public tDescuentoAsignadoBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Insert(tDescuentoAsignado obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
                 Predial.tDescuentoAsignado.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("tDescuentoAsignadoBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("tDescuentoAsignadoBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tDescuentoAsignadoBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Update(tDescuentoAsignado obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {

                 tDescuentoAsignado objOld = Predial.tDescuentoAsignado.FirstOrDefault(c => c.Id == obj.Id);
                 Utilerias.Utileria.Compare(obj, objOld);
                 objOld.IdDescuento = obj.IdDescuento;               
                 objOld.IdPredio = obj.IdPredio;               
                 objOld.Vigencia = obj.Vigencia;
                 objOld.Estado = obj.Estado;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("tDescuentoAsignadoBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("tDescuentoAsignadoBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tDescuentoAsignadoBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
         public tDescuentoAsignado GetByConstraint(int id)
		 {
             tDescuentoAsignado obj = null;
			 try
			 {
                 obj = Predial.tDescuentoAsignado.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tDescuentoAsignadoBL.GetByConstraint.Exception", ex);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
         public MensajesInterfaz Delete(tDescuentoAsignado obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
                 tDescuentoAsignado objOld = Predial.tDescuentoAsignado.FirstOrDefault(c => c.Id == obj.Id);
                 obj.Estado = "CANCELADO";
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("tDescuentoAsignadoBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("tDescuentoAsignadoBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tDescuentoAsignadoBL.Delete.Exception", ex);
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
         //public List<cCaja> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
         //{
         //    List<cCaja> objList = null;
         //    try
         //    {
         //        if (campoFiltro == string.Empty)
         //        {
         //             if (activos.ToUpper()=="TRUE")
         //                objList = Predial.cCaja.SqlQuery("Select Id,Caja,Ubicacion,Activo,IdUsuario,FechaModificacion from cCaja where activo=1 order by " + campoSort + " " + tipoSort).ToList();
         //             else
         //                objList = Predial.cCaja.SqlQuery("Select Id,Caja,Ubicacion,Activo,IdUsuario,FechaModificacion from cCaja where activo=0 order by " + campoSort + " " + tipoSort).ToList();
         //        }
         //        else
         //        {
         //             valorFiltro = "%" + valorFiltro + "%";
         //             if (activos.ToUpper()=="TRUE")
         //                objList = Predial.cCaja.SqlQuery("Select Id,Caja,Ubicacion,Activo,IdUsuario,FechaModificacion from cCaja where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
         //             else
         //                objList = Predial.cCaja.SqlQuery("Select Id,Caja,Ubicacion,Activo,IdUsuario,FechaModificacion from cCaja where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
         //        }
         //    }
         //    catch (Exception ex)
         //    {
         //        new Utileria().logError("cCajaBL.GetFilter.Exception", ex ,
         //            "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
         //    }
         //    return objList;
         //}
		
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
         //public List<string> ListaCampos()
         //{
         //    List<string> propertyList = new List<string>();
         //    try
         //    {
         //        tDescuentoAsignado pObject = new tDescuentoAsignado();
         //        if (pObject != null)
         //        {
         //            foreach (var prop in pObject.GetType().GetProperties())
         //            {
         //                if (prop.Name.ToUpper() != "ID" && prop.Name.ToUpper() != "IDUSUARIO" && prop.Name.ToUpper() != "ACTIVO" && prop.Name.ToUpper() != "FECHAMODIFICACION" && prop.Name.Substring(0, 1) != "c" && prop.Name.Substring(0, 1) != "t" && prop.Name.Substring(0, 1) != "m" && prop.Name.Substring(0, 1) != "I")
         //                    propertyList.Add(prop.Name);
         //            }
         //        }
         //    }
         //    catch (Exception ex)
         //    {
         //        new Utileria().logError("tDescuentoAsignadoBL.ListaCampos.Exception", ex);
         //    }
			 
         //return propertyList;
         //}

         public tDescuentoAsignado GetDescuentoByIdPredio(int idPredio)
         {
            tDescuentoAsignado obj = new tDescuentoAsignado();
            try
            {
                obj = Predial.tDescuentoAsignado.FirstOrDefault(o => o.IdPredio == idPredio && o.Vigencia >= DateTime.Today && o.Activo == true);
                if (obj == null)
                    obj = new tDescuentoAsignado();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tDescuentoAsignadoBL.GetDescuentoByIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio );
                obj = new tDescuentoAsignado();
            }
            return obj;
         }

         public string RegresaClaveDesctoByIdPredio(int idPredio)
         {
             tDescuentoAsignado obj = new tDescuentoAsignado();
             cDescuento d = new cDescuento();
             try
             {
                 obj = Predial.tDescuentoAsignado.FirstOrDefault(o => o.IdPredio == idPredio && o.Vigencia >= DateTime.Today && o.Activo == true);
                 if (obj == null)
                     return "";

                 d = new cDescuentoBL().GetByIdClave(obj.IdDescuento, DateTime.Today);
                 if (d == null)
                     return "";

             }
             catch (Exception ex)
             {
                 new Utileria().logError("tDescuentoAsignadoBL.GetDescuentoByIdPredio.Exception", ex , "--Parámetros idPredio:" + idPredio);
                 obj = new tDescuentoAsignado();
             }
             return d.Clave;
         }

        public MensajesInterfaz CancelaVigentesPorIdPredio(int idPredio, int idUsuario)
        {
            List<tDescuentoAsignado> objList = null;
            MensajesInterfaz msg = new MensajesInterfaz();
            try
            {
                objList = Predial.tDescuentoAsignado.Where(o => o.IdPredio == idPredio  && o.Activo == true).ToList();
                if (objList.Count > 0)
                {
                    foreach (tDescuentoAsignado t in objList)
                    {
                        t.Estado = "CANCELADO";
                        t.Activo = false;
                        t.IdUsuario = idUsuario;
                        t.FechaModificacion = DateTime.Now;
                        msg =  Delete( t);
                        if (msg != MensajesInterfaz.Actualizacion)
                            return msg;
                    }
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cDescuentoBL.CancelaVigentesPorIdPredio.Exception", ex , " Parametros: idPredio " + idPredio.ToString() );
            }
            return msg;
        }


        public tDescuentoAsignado BusqPorIdPredioIdDescuento(int idPredio, int idDescuento)
        {
            tDescuentoAsignado obj = new tDescuentoAsignado();
            cDescuento d = new cDescuento();
            try
            {
                obj = Predial.tDescuentoAsignado.FirstOrDefault(o => o.IdPredio == idPredio && o.IdDescuento == idDescuento && o.Vigencia >= DateTime.Today && o.Activo == true);
                if (obj == null)
                    return null;

                d = new cDescuentoBL().GetByIdClave(obj.IdDescuento, DateTime.Today);
                if (d == null)
                    return null;

            }
            catch (Exception ex)
            {
                new Utileria().logError("tDescuentoAsignadoBL.GetDescuentoByIdPredio.Exception", ex, "--Parámetros idPredio:" + idPredio);
                obj = new tDescuentoAsignado();
            }
            return obj;
        }

    }

}
