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
	 public class cConceptoBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// 
		 /// </summary>
		 public cConceptoBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cConcepto obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cConcepto.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cConceptoBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cConceptoBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cConceptoBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cConcepto obj)
		 {
			 MensajesInterfaz Update;
			 try
			 {
				 cConcepto objOld = Predial.cConcepto.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.Cri = obj.Cri;
				 objOld.Nombre = obj.Nombre;
				 objOld.Descripcion = obj.Descripcion;
				 objOld.SalarioMin = obj.SalarioMin;
                 objOld.SalarioMax = obj.SalarioMax;
				 objOld.Importe = obj.Importe;				 
				 objOld.IdMesa = obj.IdMesa;
				 objOld.TipoCobro = obj.TipoCobro;
				 objOld.EsAdicional= obj.EsAdicional;
				 objOld.Adicional = obj.Adicional;
				 objOld.SinDescuento = obj.SinDescuento;
				 objOld.Ejercicio = obj.Ejercicio;
				 objOld.IdTipoTramite = obj.IdTipoTramite;
                objOld.IdProdServ = obj.IdProdServ;
                objOld.IdUnidadMedida = obj.IdUnidadMedida;
                objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Update = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cConceptoBL.Update.DbUpdateException", ex);
				 Update = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cConceptoBL.Update.DataException", ex);
				 Update = MensajesInterfaz.ErrorDB;                 
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cConceptoBL.Update.Exception", ex);
				 Update = MensajesInterfaz.ErrorGeneral;
			 }
			 return Update;
		 }        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public cConcepto GetByConstraint(int id)
		 {
			 cConcepto obj = null;
			 try
			 {
				 obj = Predial.cConcepto.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cConceptoBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cConcepto obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cConcepto objOld = Predial.cConcepto.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
                 new Utileria().logError("cConceptoBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cConceptoBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;

			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cConceptoBL.Delete.Exception", ex);
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
		 public List<cConcepto> GetFilter(string campoFiltro, string valorFiltro, string activos,string ejercicio, string campoSort, string tipoSort)
		 {
			 List<cConcepto> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cConcepto.SqlQuery("SELECT Id,IdTipoTramite,IdMesa,IdUnidadMedida,IdProdServ,Ejercicio,Cri,Nombre,Descripcion,SalarioMin,SalarioMax,Importe,TipoCobro,Adicional,EsAdicional,SinDescuento,Activo,IdUsuario,FechaModificacion, IdGrupo FROM cConcepto where ejercicio= " + ejercicio+" and activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.cConcepto.SqlQuery("SELECT Id,IdTipoTramite,IdMesa,IdUnidadMedida,IdProdServ,Ejercicio,Cri,Nombre,Descripcion,SalarioMin,SalarioMax,Importe,TipoCobro,Adicional,EsAdicional,SinDescuento,Activo,IdUsuario,FechaModificacion, IdGrupo FROM cConcepto where ejercicio= " + ejercicio + " and activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  valorFiltro = "%" + valorFiltro + "%";
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cConcepto.SqlQuery("SELECT Id,IdTipoTramite,IdMesa,IdUnidadMedida,IdProdServ,Ejercicio,Cri,Nombre,Descripcion,SalarioMin,SalarioMax,Importe,TipoCobro,Adicional,EsAdicional,SinDescuento,Activo,IdUsuario,FechaModificacion,IdGrupo FROM cConcepto where ejercicio= " + ejercicio + " and activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
					  else
                          objList = Predial.cConcepto.SqlQuery("SELECT Id,IdTipoTramite,IdMesa,IdUnidadMedida,IdProdServ,Ejercicio,Cri,Nombre,Descripcion,SalarioMin,SalarioMax,Importe,TipoCobro,Adicional,EsAdicional,SinDescuento,Activo,IdUsuario,FechaModificacion,IdGrupo FROM cConcepto where ejercicio= " + ejercicio + " and activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
				 }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cConceptoBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cConcepto> GetAll()
		 {
			 List<cConcepto> objList = null;
			 try
			 {
				 objList = Predial.cConcepto.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cConceptoBL.GetAll.Exception", ex);
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
                 cConcepto pObject = new cConcepto();
                 if (pObject != null)
                 {
                     foreach (var prop in pObject.GetType().GetProperties())
                     {
                         if (prop.Name.ToUpper() != "ID" && prop.Name.ToUpper() != "IDUSUARIO" && prop.Name.ToUpper() != "EJERCICIO" && prop.Name.ToUpper() != "SALARIOMIN" && prop.Name.ToUpper() != "SALARIOMAX" && prop.Name.ToUpper() != "TIPOCOBRO" && prop.Name.ToUpper() != "ADICIONAL" && prop.Name.ToUpper() != "ESADICIONAL" && prop.Name.ToUpper() != "SINDESCUENTO" && prop.Name.ToUpper() != "ACTIVO" && prop.Name.ToUpper() != "FECHAMODIFICACION" && prop.Name.Substring(0, 1) != "c" && prop.Name.Substring(0, 1) != "t" && prop.Name.Substring(0, 1) != "m" && prop.Name.Substring(0, 1) != "I")
                             propertyList.Add(prop.Name);
                     }
                 }
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cConceptoBL.ListaCampos.Exception", ex);
             }
			 
		 return propertyList;
		 }

         public string GetByCri(string cri, int ejercicio, int mesa)
         {
             cConcepto obj = null;
             try
             {
                 obj = Predial.cConcepto.FirstOrDefault(o => o.IdMesa == mesa && o.Cri == cri && o.Ejercicio == ejercicio && o.Activo == true);
                 if (obj == null) return "";
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cConceptoBL.GetByCri.Exception", ex , "--Parámetros cri:" + cri + 
                     ", ejercicio:" + ejercicio + ", mesa:" + mesa);
                 return "CRI no definido para el ejercicio actual:" + cri;
             }
             return obj.Descripcion;
         }

         public cConcepto RegistroByCri(string cri, int ejercicio, int mesa)
         {
             cConcepto obj = null;
             try
             {
                 obj = Predial.cConcepto.FirstOrDefault(o => o.IdMesa == mesa && o.Cri == cri && o.Ejercicio == ejercicio && o.Activo == true);                
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cConceptoBL.RegistroByCri.Exception", ex , "--Parámetros cri:" + cri +
                     ", ejercicio:" + ejercicio + ", mesa:" + mesa);
                 return obj = null;
             }
             return obj;
         }

         public cConcepto GetByCampo(string campoFiltro, string valorFiltro, int idmesa, string activos, string campoSort, string tipoSort)
         {
             cConcepto concepto = null;
             try
             {
                 if (campoFiltro != string.Empty)
                 {

                     if (activos.ToUpper() == "TRUE")
                         concepto = Predial.cConcepto.SqlQuery("Select Id,Ejercicio,Cri,Nombre,Descripcion,SalarioMin,Importe,IdMesa,TipoCobro,Agrava,Adicional,SinDescuento,Concesion,Activo,IdUsuario,FechaModificacion from cConcepto where activo=1 and Ejercicio = (SELECT YEAR(GETDATE()))  and IdMesa=" + idmesa + " and " + campoFiltro + " =  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).Single();
                     else
                         concepto = Predial.cConcepto.SqlQuery("Select Id,Ejercicio,Cri,Nombre,Descripcion,SalarioMin,Importe,IdMesa,TipoCobro,Agrava,Adicional,SinDescuento,Concesion,Activo,IdUsuario,FechaModificacion from cConcepto where activo=0  and Ejercicio = (SELECT YEAR(GETDATE()))  and IdMesa=" + idmesa + " and " + campoFiltro + " =  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).Single();
                 }

             }
             catch (Exception ex)
             {
                 new Utileria().logError("cConceptoBL.GetByCampo.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", idmesa:" + idmesa + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
             }
             return concepto;

         }

         public List<cConcepto> GetListByConstraint(int id, int Ejercicio,int idMesa)
         {
             List<cConcepto> obj = null;
             try
             {
                 if (idMesa == 0)
                 {
                     obj = Predial.cConcepto.Where(o => o.IdTipoTramite == id && o.Activo == true && o.Ejercicio == Ejercicio)
                      .OrderByDescending(u => u.Adicional).ToList();
                 }
                 else
                 {
                     obj = Predial.cConcepto.Where(o => o.IdTipoTramite == id && o.Activo == true && o.Ejercicio == Ejercicio && o.IdMesa == idMesa)
                     .OrderByDescending(u => u.Adicional).ToList();
                 }
                 
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cConceptoBL.GetListByConstraint.Exception", ex ,
                     "--Parámetros id:" + id + ", Ejercicio:" + Ejercicio + ", idMesa: " + idMesa);
             }
             return obj;
         }

         public List<cConcepto> GetFilterCobroCatastro(string campoFiltro, Int32 valorFiltro, int activo, string campoSort, string tipoSort)
         {
             List<cConcepto> objList = null;
             try
             {
                 objList = Predial.cConcepto.SqlQuery("Select Id,IdTipoTramite,IdMesa,Ejercicio,Cri,Nombre,Descripcion,Importe,TipoCobro,Adicional,SinDescuento,Activo,IdUsuario,FechaModificacion,SalarioMin,EsAdicional,SalarioMax from cConcepto where IdTipoTramite is null and activo= " +
                     activo + " and Ejercicio = (SELECT YEAR(GETDATE())) and EsAdicional=0 and " + campoFiltro + " =  " + valorFiltro + " order by " + campoSort + " " + tipoSort).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cConceptoBL.GetFilterCobroCatastro.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activo:" + activo + ", campoSort:" + campoSort + ", tipoSort:"+tipoSort);
             }
             return objList;
         }

         public cConcepto GetListByConstraintEjercicio(int id, int Ejercicio)
         {
             cConcepto obj = null;
             try
             {
                 obj = Predial.cConcepto.Where(o => o.Id == id && o.Activo == true && o.Ejercicio == Ejercicio).FirstOrDefault();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cConceptoBL.GetListByConstraintEjercicio.Exception", ex ,
                     "--Parámetros id:" + id + ", Ejercicio:" + Ejercicio);
             }
             return obj;
         }

         public cConcepto getConceptoByCriEjercicio(string cri, int ejercicio, int idTipoTramite)
         {
             cConcepto obj = null;
             try
             {
                 obj = Predial.cConcepto.FirstOrDefault(o => o.Cri == cri && o.Ejercicio == ejercicio && o.Activo == true && o.IdTipoTramite==idTipoTramite);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cConceptoBL.getConceptoByCriEjercicio.Exception", ex ,
                     "--Parámetros cri:" + cri + ", ejercicio:" + ejercicio);
                 return obj = null;
             }
             return obj;
         }

        public List<cConcepto> GetListForDescuentos(int id, int Ejercicio)
        {
            List<cConcepto> obj = null;
            try
            {
                obj = Predial.cConcepto.Where(o => o.IdTipoTramite == id && o.Activo == true && o.Ejercicio == Ejercicio && o.SinDescuento==true)
                    .OrderByDescending(u => u.Adicional).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cConceptoBL.GetListForDescuentos.Exception", ex ,
                     "--Parámetros id:" + id + ", Ejercicio:" + Ejercicio);
            }
            return obj;
        }
         
        public cConcepto GetRegistroIdDescuento(int id, int Ejercicio, int idMesa)
        {
            cConcepto obj = null;
            try
            {
                obj = Predial.cConcepto.Where(o => o.Id == id && o.Ejercicio == DateTime.Today.Year && o.IdMesa == idMesa && o.Activo == true && o.Ejercicio == Ejercicio).FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cConceptoBL.GetRegistroIdDescuento.Exception", ex ,
                     "--Parámetros id:" + id + ", Ejercicio:" + Ejercicio + ", idMesa: " + idMesa);
            }
            return obj;
        }

        public cConcepto AdicionalMesaCatastro(int ejercicio, int mesa)
        {
            cConcepto obj = null;
            try
            {
                obj = Predial.cConcepto.FirstOrDefault(o => o.IdMesa == mesa && o.EsAdicional == true && o.Ejercicio == ejercicio && o.Activo == true && o.IdTipoTramite == 4);
            }
            catch (Exception ex)
            {
                new Utileria().logError("cConceptoBL.AdicionalMesaCatastro.Exception", ex ,
                    "--Parámetros Ejercicio:" + ejercicio + ", mesa: " + mesa);
                return obj = null;
            }
            return obj;
        }

        public int GetIdConceptoByCriEjercicio(string cri, int ejercicio)
        {
            cConcepto obj = null;
            try
            {
                obj = Predial.cConcepto.FirstOrDefault(o => o.Cri == cri && o.Ejercicio == ejercicio && o.Activo == true);
                if (obj == null)
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cConceptoBL.getConceptoByCriEjercicio.Exception", ex ,
                    "--Parámetros cri:" + cri + ", ejercicio:" + ejercicio);
                return 0;
            }
            return obj.Id;
        }

        public List<cConcepto> listConceptosCatastro(int ejercicio, int mesa)
        {
            List<cConcepto> obj = null;
            try
            {
                obj = Predial.cConcepto.Where(o => o.IdMesa == mesa && o.Ejercicio == ejercicio && o.Activo == true && o.EsAdicional==false).OrderBy(o => o.Descripcion).ToList();
                //obj = Predial.cConcepto.Where(o => o.IdMesa == mesa && o.Ejercicio == ejercicio && o.Activo == true && o.IdTipoTramite == null).OrderBy(o=> o.Descripcion).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cConceptoBL.listConceptosCatastro.Exception", ex ,
                    "--Parámetros Ejercicio:" + ejercicio + ", mesa: " + mesa);
                return obj = null;
            }
            return obj;
        }

        public cConcepto RegistroByCriIdTramite(string cri, int ejercicio, int mesa, int idTipoTramite)
        {
            cConcepto obj = null;
            try
            {
                obj = Predial.cConcepto.FirstOrDefault(o => o.IdMesa == mesa && o.Cri == cri && o.Ejercicio == ejercicio && o.IdTipoTramite == idTipoTramite &&  o.Activo == true);
            }
            catch (Exception ex)
            {
                new Utileria().logError("cConceptoBL.RegistroByCri.Exception", ex , "--Parámetros cri:" + cri +
                    ", ejercicio:" + ejercicio + ", mesa:" + mesa);
                return obj = null;
            }
            return obj;
        }

        public List<cConcepto> ListConceptobyDescuentoTramiteISABI(int ejercicio)
        {
            List<cConcepto> obj = null;
            try
            {
                int idMesa = new cMesaBL().GetByNombre("ISABIS");
                idMesa = idMesa;
                obj = Predial.cConcepto.Where(o => o.IdMesa == idMesa && o.Ejercicio == ejercicio && o.Activo == true && o.SinDescuento==true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("cConceptoBL.RegistroByCri.Exception", ex, ", ejercicio:" + ejercicio + ", mesa:" + 4);
                return obj = null;
            }
            return obj;
        }

        public cConcepto GetForDescuentos(int id, int Ejercicio)
        {
            cConcepto obj = null;
            try
            {
                obj = Predial.cConcepto.FirstOrDefault(o => o.Id == id && o.Activo == true && o.Ejercicio == Ejercicio && o.SinDescuento == true);
                
            }
            catch (Exception ex)
            {
                new Utileria().logError("cConceptoBL.GetListForDescuentos.Exception", ex,
                     "--Parámetros id:" + id + ", Ejercicio:" + Ejercicio);
            }
           
            return obj;
        }


    }


}
