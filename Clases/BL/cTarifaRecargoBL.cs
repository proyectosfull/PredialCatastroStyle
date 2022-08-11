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
	 public class cTarifaRecargoBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cTarifaRecargoBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cTarifaRecargo obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cTarifaRecargo.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTarifaRecargoBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cTarifaRecargoBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTarifaRecargoBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cTarifaRecargo obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cTarifaRecargo objOld = Predial.cTarifaRecargo.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Ejercicio = obj.Ejercicio;
				 objOld.Bimestre = obj.Bimestre;
				 objOld.Porcentaje = obj.Porcentaje;
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTarifaRecargoBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cTarifaRecargoBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTarifaRecargoBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cTarifaRecargo GetByConstraint(int id)
		 {
			 cTarifaRecargo obj = null;
			 try
			 {
				 obj = Predial.cTarifaRecargo.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cTarifaRecargoBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cTarifaRecargo obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cTarifaRecargo objOld = Predial.cTarifaRecargo.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cTarifaRecargoBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cTarifaRecargoBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTarifaRecargoBL.Delete.Exception", ex);
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
		 public List<cTarifaRecargo> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cTarifaRecargo> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cTarifaRecargo.SqlQuery("Select Id,Ejercicio,Bimestre,Porcentaje,Activo,IdUsuario,FechaModificacion from cTarifaRecargo where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cTarifaRecargo.SqlQuery("Select Id,Ejercicio,Bimestre,Porcentaje,Activo,IdUsuario,FechaModificacion from cTarifaRecargo where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cTarifaRecargo.SqlQuery("Select Id,Ejercicio,Bimestre,Porcentaje,Activo,IdUsuario,FechaModificacion from cTarifaRecargo where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cTarifaRecargo.SqlQuery("Select Id,Ejercicio,Bimestre,Porcentaje,Activo,IdUsuario,FechaModificacion from cTarifaRecargo where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cTarifaRecargoBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cTarifaRecargo> GetAll()
		 {
			 List<cTarifaRecargo> objList = null;
			 try
			 {
				 objList = Predial.cTarifaRecargo.Where(o => o.Activo==true).OrderBy(o => o.Ejercicio).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cTarifaRecargoBL.GetAll.Exception", ex);
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
                 cTarifaRecargo pObject = new cTarifaRecargo();
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
                 new Utileria().logError("cTarifaRecargoBL.ListaCampos.Exception", ex);
             }
			
		 return propertyList;
		 }

         public List<cTarifaRecargo> GetByPeriodo(int BimInicial, int EjInicial, int BimFinal , int EjFinal)
         {
             List<cTarifaRecargo> objList = null;
             try
             {
                 objList = Predial.cTarifaRecargo.Where(o => o.Ejercicio == EjInicial && o.Bimestre == BimInicial  && o.Ejercicio == EjFinal && o.Bimestre == BimFinal && o.Activo == true).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cTarifaRecargoBL.GetByPeriodo.Exception", ex , "--Parámetros BimInicial:" + BimInicial + ", EjInicial:" + EjInicial + ", BimFinal:" + BimFinal + ", EjFinal:" + EjFinal);
             }
             return objList;
         }

         public decimal tarifaRecargoMes(int bimestre, int ejercicio)
         {
             try
             {
                 return Predial.cTarifaRecargo.Where(o => o.Ejercicio == ejercicio && o.Bimestre == bimestre && o.Activo == true).FirstOrDefault().Porcentaje;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cTarifaRecargoBL.GetByPeriodo.Exception", ex , "--Parámetros bimestre:" + bimestre + ", ejercicio:" + ejercicio);
                 return -1;
             }
         }

         public decimal PorcentajeRecargo(int BimInicial, int EjInicial, int BimFinal, int EjFinal,ref List<cTarifaRecargo> listRecargos)
         {
             List<cTarifaRecargo> objList = null;
             List<TarifaRecargo> tarifa = new List<TarifaRecargo>();
             List<TarifaRecargo> t = new List<TarifaRecargo>();
             decimal porcentaje = 0;
             int mesFinal = 0;
             int mes = DateTime.Today.Month;// BimFinal * 2;
             int mesInicial = 0;

             //Sacando el mes Inicial
             if (mes.ToString().Length == 1) 
                 mesFinal = Convert.ToInt32( EjFinal.ToString() + '0' + mes);
             else
                 mesFinal = Convert.ToInt32(EjFinal.ToString() + mes );
             //Sacando el mes Final
             //mes = (BimInicial * 2) -1;//se comenta apr ano restar la unidad
             mes = (BimInicial * 2) ;
             if (mes.ToString().Length == 1)
                 mesInicial = Convert.ToInt32(EjInicial.ToString() + '0' + mes);
             else
                 mesInicial = Convert.ToInt32(EjInicial.ToString() + mes);
             //Si es mes par solo se cobra al mes anteior
             if (DateTime.Today.Month % 2 == 0)
             {
                 //mesFinal = mesFinal -1;  //no se aplica
             }

             try
             {
                 objList = listRecargos.Where(o => o.Ejercicio >= EjInicial && o.Ejercicio <= EjFinal && o.Activo == true).ToList();
                //objList = Predial.cTarifaRecargo.Where(o => o.Ejercicio >= EjInicial && o.Ejercicio <= EjFinal && o.Activo == true).ToList();
                 foreach (var o in objList)
                 {
                     string  bim = o.Bimestre.ToString();
                     if (bim.Length == 1) bim =  '0' + bim;
                     tarifa.Add(new TarifaRecargo(o.Ejercicio, o.Bimestre, Convert.ToDecimal(o.Porcentaje), Convert.ToInt32( o.Ejercicio.ToString() + bim ) ));                          
                 }
                 t = tarifa.Where(o => o.periodo >= mesInicial && o.periodo <= mesFinal).ToList();
                 porcentaje = t.Sum(o => o.porcentaje) / 100;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cTarifaRecargoBL.PorcentajeRecargo.Exception", ex , "--Parámetros BimInicial:" + BimInicial + ", EjInicial:" + EjInicial + ", BimFinal:" + BimFinal + ", EjFinal:" + EjFinal);
                 return -1;
             }
             return porcentaje;
         }

         public decimal PorcentajeRecargoIsabi(int MesInicial, int EjInicial, int MesFinal, int EjFinal)
         {
             List<cTarifaRecargo> objList = null;
             List<TarifaRecargo> tarifa = new List<TarifaRecargo>();
             List<TarifaRecargo> t = new List<TarifaRecargo>();
             int periodoIni = 0;
             int periodoFin = 0;
             decimal porcentaje = 0;            

             //Sacando el perido Inicial
             periodoIni = Convert.ToInt32(MesInicial.ToString().Length == 1 ? EjInicial + "0" + MesInicial : EjInicial+MesInicial.ToString());
             periodoFin = Convert.ToInt32(MesFinal.ToString().Length == 1 ? EjFinal + "0" + MesFinal : EjFinal+MesFinal.ToString());
             
             try
             {
                 objList = Predial.cTarifaRecargo.Where(o => o.Ejercicio >= EjInicial && o.Ejercicio <= EjFinal && o.Activo == true).ToList();
                 foreach (var o in objList)
                 {
                     string bim = o.Bimestre.ToString().Length == 1 ? "0" + o.Bimestre : o.Bimestre.ToString();
                     tarifa.Add(new TarifaRecargo(o.Ejercicio, o.Bimestre, Convert.ToDecimal(o.Porcentaje), Convert.ToInt32(o.Ejercicio.ToString() + bim)));                          
                 }
                 t = tarifa.Where(o => o.periodo >= periodoIni && o.periodo <= periodoFin).ToList();
                 porcentaje =( t.Sum(o => o.porcentaje) > 200 ? 200 : t.Sum(o => o.porcentaje) )/ 100;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cTarifaRecargoBL.PorcentajeRecargoIsabi.Exception", ex , "--Parámetros BimInicial:" + MesInicial + ", EjInicial:" + EjInicial + ", BimFinal:" + MesFinal + ", EjFinal:" + EjFinal);
                 return -1;
             }
             return porcentaje;
         }

	 }

}
