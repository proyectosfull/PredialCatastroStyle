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
	 public class tPrediosDescuentoBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
         public tPrediosDescuentoBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(tPrediosDescuento obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.tPrediosDescuento.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tPrediosDescuentoBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tPrediosDescuentoBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tPrediosDescuentoBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(tPrediosDescuento obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
                tPrediosDescuento objOld = Predial.tPrediosDescuento.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdConcepto = obj.IdConcepto;
                objOld.IdPredio = obj.IdPredio;
                objOld.IdTramite = obj.IdTramite;
                objOld.Concepto = obj.Concepto;
                objOld.FechaInicio = obj.FechaInicio;
                objOld.FechaFin = obj.FechaFin;
                objOld.Porcentaje = obj.Porcentaje;
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tPrediosDescuentoBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tPrediosDescuentoBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tPrediosDescuentoBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public tPrediosDescuento GetByConstraint(int id)
		 {
			 tPrediosDescuento obj = null;
			 try
			 {
				 obj = Predial.tPrediosDescuento.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrediosDescuentoBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
         /// <summary>
         /// 
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>       

        public tPrediosDescuento GetForCobro(int idConcepto, int idPredio, int idTramite, DateTime d)
        {
            tPrediosDescuento obj = null;
            try
            {
                obj = Predial.tPrediosDescuento
                    .FirstOrDefault(o => o.Activo == true && o.IdConcepto == idConcepto && o.IdPredio == idPredio && o.IdTramite== idTramite & o.FechaInicio <= d && o.FechaFin > d);
                //if(obj.FechaInicio<=d && obj.FechaFin>d)
                //{
                //    obj = obj;
                //}
                //else
                //{
                //    obj = null;
                //}
            }
            catch (Exception ex)
            {
                new Utileria().logError("tPrediosDescuentoBL.GetForCobro.Exception", ex, "--Parámetros idPredio:" + idPredio + ", d:" + d);
            }
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Delete(tPrediosDescuento obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 tPrediosDescuento objOld = Predial.tPrediosDescuento.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("tPrediosDescuentoBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("tPrediosDescuentoBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tPrediosDescuentoBL.Delete.Exception", ex);
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
		 public List<tPrediosDescuento> GetForDescuentos(int IdPredio, int IdTramite)
		 {
			 List<tPrediosDescuento> objList = null;
			 try
			 {
                if (IdTramite > 0)
                    objList = Predial.tPrediosDescuento.SqlQuery("SELECT Id,idConcepto,IdPredio,idtramite,Concepto,FechaInicio,FechaFin,Porcentaje,Activo,IdUsuario,FechaModificacion from tPrediosDescuento where IdPredio=" + IdPredio + " and IdTramite=" + IdTramite).ToList();
                else
                    objList = Predial.tPrediosDescuento.SqlQuery("SELECT Id,idConcepto,IdPredio,idtramite,Concepto,FechaInicio,FechaFin,Porcentaje,Activo,IdUsuario,FechaModificacion from tPrediosDescuento where IdPredio=" + IdPredio + " and IdTramite is null ").ToList();                    
            }
			 catch (Exception ex)
			 {
                 new Utileria().logError("tPrediosDescuentoBL.GetForDescuentos.Exception", ex , "--Parámetros IdPredio:" + IdPredio + ", IdTramite:" + IdTramite);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<tPrediosDescuento> GetAll()
		 {
			 List<tPrediosDescuento> objList = null;
			 try
			 {
				 objList = Predial.tPrediosDescuento.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("tPrediosDescuentoBL.GetAll.Exception", ex);
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
                 tPrediosDescuento pObject = new tPrediosDescuento();
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
                 new Utileria().logError("tPrediosDescuentoBL.ListaCampos.Exception", ex);
             }
			 
		 return propertyList;
		 }

        public tPrediosDescuento GetForDescuento(int IdConcepto,int IdPredio, int IdTramite)
        {
            tPrediosDescuento obj = null;
            try
            {
                if(IdTramite>0)
                    obj = Predial.tPrediosDescuento.FirstOrDefault(o => o.IdTramite == IdTramite && o.IdConcepto == IdConcepto && o.IdPredio == IdPredio);
                else
                    obj = Predial.tPrediosDescuento.FirstOrDefault(o => o.IdTramite == null && o.IdConcepto == IdConcepto && o.IdPredio == IdPredio);
            }
            catch (Exception ex)
            {
                new Utileria().logError("tPrediosDescuentoBL.GetForDescuento.Exception", ex , "--Parámetros IdConcepto:" + IdConcepto + ", IdPredio:" + IdPredio + ", IdTramite:" + IdTramite);
            }
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>

        public List<tPrediosDescuento> GetDescuentoCobro(int idTramite, int idPredio, DateTime d)
        {
            List<tPrediosDescuento> obj = null;
            try
            {
                obj = Predial.tPrediosDescuento.Where(o => o.Activo == true && o.IdTramite == idTramite && o.IdPredio == idPredio &&
                    o.FechaInicio < d && o.FechaFin > d).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tPrediosDescuentoBL.GetDescuentoCobro.Exception", ex , "--Parámetros idTramite:" + idTramite + ", idPredio:" + idPredio + ", d:"+d);
            }
            return obj;
        }


        public List<tPrediosDescuento> GetDescuentoPorPredio( int idPredio, string concepto)
        {
            List<tPrediosDescuento> objList = null;           
            try
            {                
                objList = Predial.tPrediosDescuento.Where(o => o.Activo == true && o.IdPredio == idPredio && o.Concepto == concepto && o.FechaInicio <= DateTime.Today && o.FechaFin >= DateTime.Today).ToList();

                if (objList == null)
                {
                    objList = new List<tPrediosDescuento>();
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("tPrediosDescuentoBL.GetDescuentoPorPredio.Exception", ex , "--Parámetros idPredio:" + idPredio + ", concepto:" + concepto);
            }
            return objList;
        }

        public int  GetIdConceptoPorPredio(int idConcepto, int idPredio)
        {
            tPrediosDescuento obj = null;
            try
            {
                obj = Predial.tPrediosDescuento
                    .FirstOrDefault(o => o.Activo == true && o.IdConcepto == idConcepto && o.IdPredio == idPredio);
                if (obj != null)
                {
                    return obj.Id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tPrediosDescuentoBL.GetForCobro.Exception", ex , "--Parámetros idPredio:" + idPredio );
            }
            return obj.Id;
        }

        public int GuardaConcepto(int idPredio,int  idConcepto, string concepto, DateTime fin, decimal porcentaje,int idUsuario )
        {          
            MensajesInterfaz msg = new MensajesInterfaz();
            tPrediosDescuento desc = new tPrediosDescuento();
            
            try
            {
                int d = 0;
                d = GetIdConceptoPorPredio(idPredio, idConcepto);

                if (d == 0) //Inserta
                {
                    desc.IdConcepto = idConcepto;//Convert.ToInt32(grd.DataKeys[0].Values[0].ToString());
                    desc.IdPredio = idPredio;
                    desc.IdTramite = null;
                    desc.Concepto = concepto;//ddlTramite.DataTextField;
                    desc.FechaInicio = DateTime.Now;
                    desc.FechaFin = fin;
                    desc.Porcentaje = porcentaje;
                    desc.Activo = true;
                    desc.IdUsuario = idUsuario;
                    desc.FechaModificacion = DateTime.Today;
                    msg = new tPrediosDescuentoBL().Insert(desc);
                    if (msg != MensajesInterfaz.Ingreso)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                }
                else // "ActualizarRegistro"
                {
                    desc = new tPrediosDescuentoBL().GetByConstraint(d);
                    desc.Concepto = concepto;
                    desc.FechaInicio = DateTime.Now;
                    desc.FechaFin = fin;
                    desc.Porcentaje = porcentaje;
                    if (porcentaje < 0)
                        desc.Activo = true;
                    else
                        desc.Activo = false;
                    desc.IdUsuario = idUsuario;
                    desc.FechaModificacion = DateTime.Today;
                    msg = new tPrediosDescuentoBL().Update(desc);
                    if (msg != MensajesInterfaz.Actualizacion)
                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                }


            }
            catch (Exception ex)
            {
                new Utileria().logError("tPrediosDescuentoBL.GuardaConcepto.Exception", ex , "--Parámetros idPredio:" + idPredio + ", idConcepto:" + idConcepto + ", concepto "+ concepto);
                return -1;
            }
            return 1;
        }

        public int InactivaDescuento (int idPredio)
        {
            MensajesInterfaz msg = new MensajesInterfaz();
            List <tPrediosDescuento> ld = new List<tPrediosDescuento>();           
            try 
            {
                ld = GetForDescuentos(idPredio,0);
                foreach (tPrediosDescuento d in ld)
                {
                    d.IdTramite = null;
                    d.Concepto = "";
                    d.Porcentaje = 0;
                    d.Activo = false;
                    msg = new tPrediosDescuentoBL().Update(d);
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("tPrediosDescuentoBL.GuardaConcepto.Exception", ex , "--Parámetros idPredio:" + (idPredio).ToString() );
                return -1;
            }
            return 1;
        }

        public tPrediosDescuento GetbyIdConceptoPredioISABI(int idConcepto, int idPredio, int idTram)
        {
            tPrediosDescuento obj = null;
            try
            {
                obj = Predial.tPrediosDescuento
                    .FirstOrDefault(o => o.Activo == true && o.IdConcepto == idConcepto && o.IdPredio == idPredio && o.Concepto == 
                        "Isabis" && o.FechaFin >= DateTime.Now && o.IdTramite==idTram);
            }
            catch (Exception ex)
            {
                new Utileria().logError("tPrediosDescuentoBL.GetForCobro.Exception", ex, "--Parámetros idPredio:" + idPredio);
            }
            return obj;
        }




    }

}
