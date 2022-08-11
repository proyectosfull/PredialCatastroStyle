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
	 public class cIndicePrecioBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cIndicePrecioBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cIndicePrecio obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cIndicePrecio.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cIndicePrecioBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cIndicePrecioBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cIndicePrecioBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cIndicePrecio obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cIndicePrecio objOld = Predial.cIndicePrecio.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Ejercicio = obj.Ejercicio;
				 objOld.Mes = obj.Mes;
				 objOld.Valor = obj.Valor;				 
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cIndicePrecioBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cIndicePrecioBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cIndicePrecioBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cIndicePrecio GetByConstraint(int id)
		 {
			 cIndicePrecio obj = null;
			 try
			 {
				 obj = Predial.cIndicePrecio.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cIndicePrecioBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cIndicePrecio obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cIndicePrecio objOld = Predial.cIndicePrecio.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cIndicePrecioBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
             {
                 new Utileria().logError("cIndicePrecioBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cIndicePrecioBL.Delete.Exception", ex);
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
		 public List<cIndicePrecio> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cIndicePrecio> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cIndicePrecio.SqlQuery("Select Id,Ejercicio,Mes,Valor,Activo,IdUsuario,FechaModificacion from cIndicePrecio where activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
						 objList = Predial.cIndicePrecio.SqlQuery("Select Id,Ejercicio,Mes,Valor,Diciembre,Activo,IdUsuario,FechaModificacion from cIndicePrecio where activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
						 objList = Predial.cIndicePrecio.SqlQuery("Select Id,Ejercicio,Mes,Valor,Activo,IdUsuario,FechaModificacion from cIndicePrecio where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
						 objList = Predial.cIndicePrecio.SqlQuery("Select Id,Ejercicio,Mes,Valor,Activo,IdUsuario,FechaModificacion from cIndicePrecio where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cIndicePrecioBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cIndicePrecio> GetAll()
		 {
			 List<cIndicePrecio> objList = null;
			 try
			 {
				 objList = Predial.cIndicePrecio.Where(o => o.Activo==true).OrderBy(o => o.Ejercicio).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cIndicePrecioBL.GetAll.Exception", ex);
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
                 cIndicePrecio pObject = new cIndicePrecio();
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
                 new Utileria().logError("cIndicePrecioBL.ListaCampos.Exception", ex);
             }
		 return propertyList;
		 }

         public decimal GetByMesEjercicio(int mes, int ejercicio)
         {
             cIndicePrecio obj = null;
             try
             {
                 obj = Predial.cIndicePrecio.FirstOrDefault(o => o.Ejercicio == ejercicio && o.Mes == mes &&  o.Activo==true);
                 if (obj == null) return 0;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cIndicePrecioBL.GetByMesEjercicio.Exception", ex , "--Parámetros mes:" + mes + ", ejercicio:" + ejercicio);
                 return 0;
             }
             return obj.Valor;
         }

         public Boolean getIsEjercicio( int ejercicio)
         {
             cIndicePrecio obj = null;
             try
             {
                 obj = Predial.cIndicePrecio.FirstOrDefault(o => o.Ejercicio == ejercicio );
                 if (obj == null) return false;
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cIndicePrecioBL.getIsEjercicio.Exception", ex, ", ejercicio:" + ejercicio);
                 return false;
             }
             return true;
         }

         public decimal ValorIndiceActual ()             
         {            
             cIndicePrecio indiceActual = new cIndicePrecio();
             decimal valor = 0;
             DateTime hoy = DateTime.Today;
             int mesActual = hoy.Month;

             try
             {              
                 if (hoy.Day >= 11)
                 {
                     if (mesActual == 1)
                     {
                        valor = new cIndicePrecioBL().GetByMesEjercicio(12, hoy.Year - 1);                       
                     }
                     else
                     {
                         valor = new cIndicePrecioBL().GetByMesEjercicio(mesActual - 1, hoy.Year);
                     }
                 }
                 else
                 {
                     if (mesActual == 1)
                     {
                         valor = new cIndicePrecioBL().GetByMesEjercicio(11, hoy.Year - 1);
                     }
                     else if  (mesActual == 2)
                     {
                         valor = new cIndicePrecioBL().GetByMesEjercicio(12, hoy.Year - 1);
                     }
                     else
                     {
                         valor = new cIndicePrecioBL().GetByMesEjercicio(mesActual - 2, hoy.Year);
                     }
                 }
                
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cIndicePrecioBL.ValorIndiceActual.Exception", ex);
             }
             return valor;
         }//valor indice

        public decimal ValorIndiceActualSinCambio()
        {
            cIndicePrecio indiceActual = new cIndicePrecio();
            decimal valor = 0;
            DateTime hoy = DateTime.Today;
            int mesActual = hoy.Month;

            try
            {               
                    if (mesActual == 1)
                    {
                        valor = new cIndicePrecioBL().GetByMesEjercicio(11, hoy.Year - 1);
                    }
                    else if (mesActual == 2)
                    {
                        valor = new cIndicePrecioBL().GetByMesEjercicio(12, hoy.Year - 1);
                    }
                    else
                    {
                        valor = new cIndicePrecioBL().GetByMesEjercicio(mesActual - 2, hoy.Year);
                    }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cIndicePrecioBL.ValorIndiceActual.Exception", ex);
            }
            return valor;
        }//valor indice



        public decimal ValorIndiceAnterior ( int ejercicio, int bimestre)
         {            
             cIndicePrecio indiceAnterior = new cIndicePrecio();
             decimal valor = 0;
             try
             { 
                 if (bimestre == 1)
                     valor = new cIndicePrecioBL().GetByMesEjercicio(12, ejercicio - 1);
                 else
                     valor = new cIndicePrecioBL().GetByMesEjercicio((bimestre * 2) - 2, ejercicio);

             }
             catch (Exception ex)
             {
                 new Utileria().logError("cIndicePrecioBL.ValorIndiceAnterior.Exception", ex , "--Parámetros ejercicio:" + ejercicio + ", bimestre:" + bimestre);
                 return  0;
             }
             return valor;
         }

        public decimal ValorIndiceAnterior(int ejercicio, int bimestre,ref List<cIndicePrecio> listIndice)
        {
            cIndicePrecio indiceAnterior = new cIndicePrecio();
            decimal valor = 0;
            try
            {
                if (bimestre == 1)
                    //valor = new cIndicePrecioBL().GetByMesEjercicio(12, ejercicio - 1);
                    valor = listIndice.FirstOrDefault(o => o.Ejercicio == (ejercicio - 1) && o.Mes == 12 && o.Activo == true).Valor;
                else
                    //valor = new cIndicePrecioBL().GetByMesEjercicio((bimestre * 2) - 2, ejercicio);
                    valor = listIndice.FirstOrDefault(o => o.Ejercicio == ejercicio && o.Mes == ((bimestre * 2) - 2) && o.Activo == true).Valor;

            }
            catch (Exception ex)
            {
                new Utileria().logError("cIndicePrecioBL.ValorIndiceAnterior.Exception", ex, "--Parámetros ejercicio:" + ejercicio + ", bimestre:" + bimestre);
                return 0;
            }
            return valor;
        }
        /// <summary>
        /// Obtiene los años existentes del catálogo
        /// </summary>
        /// <returns></returns>
        public DataTable obtenerAniosCatalgos(Boolean bActivo, String strAnio)
        {
            cIndicePrecio indiceAnterior = new cIndicePrecio();
            List<Int32> anios = new List<Int32>();
            DataTable dt = new DataTable();
            dt.Columns.Add("Ejercicio");
            dt.Columns.Add("Activo");
            try
            {                
                if (bActivo)                                     
                   anios = Predial.cIndicePrecio.Where(ip => ip.Activo==true && ip.Ejercicio.ToString().Contains(strAnio)).OrderByDescending(ip=>ip.Ejercicio).ToList().Select(ip => ip.Ejercicio).Distinct().ToList();                                    
                else
                    anios = Predial.cIndicePrecio.Where(ip => ip.Activo == false && ip.Ejercicio.ToString().Contains(strAnio)).OrderByDescending(ip => ip.Ejercicio).ToList().Select(ip => ip.Ejercicio).Distinct().ToList();

                foreach(Int32 anio in anios)
                {
                    DataRow row = dt.NewRow();
                    row["Ejercicio"] = anio;
                    row["Activo"] = bActivo;
                    dt.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cIndicePrecioBL.obtenerAniosCatalgos.Exception", ex , "--Parámetros bActivo:" + bActivo + ", strAnio:" + strAnio);
            }
            return dt;
        }
        /// <summary>
        /// Obtiene los años existentes del catálogo
        /// </summary>
        /// <returns></returns>
        public cIndicePrecio obtenerAniosMes(Int32 Anio, Int32 Mes)
        {
            cIndicePrecio indice = new cIndicePrecio();
            
            try
            {
                indice = Predial.cIndicePrecio.Where(ip => ip.Ejercicio==Anio && ip.Mes==Mes && ip.Activo == true).FirstOrDefault();

                
            }
            catch (Exception ex)
            {
                new Utileria().logError("cIndicePrecioBL.obtenerAniosMes.Exception", ex , "--Parámetros Anio:" + Anio + ", Mes:" + Mes);
            }
            return indice;
        }

        public void InactivaEjercicioMes(Int32 Anio, Int32 Mes)
        {
            List<cIndicePrecio> indice = new List<cIndicePrecio>();
            cIndicePrecioBL ibl = new cIndicePrecioBL();
            MensajesInterfaz msg = new MensajesInterfaz();
            try
            {
                indice = Predial.cIndicePrecio.Where(ip => ip.Ejercicio == Anio && ip.Mes == Mes && ip.Activo == true).ToList();
                foreach (cIndicePrecio i in indice)
                {
                    i.Activo = false;
                    msg = ibl.Update(i);
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cIndicePrecioBL.InactivaEjercicioMes.Exception", ex, "--Parámetros Anio:" + Anio + ", Mes:" + Mes);
            }           
        }


    }    
}
