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
	 public class cDiferenciaBL
	 {
		 PredialEntities Predial;
		 /// <summary>
		 /// constructor
		 /// </summary>
		 public cDiferenciaBL()
		 { 
			 Predial = new PredialEntities();
		 } 
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Insert(cDiferencia obj)
		 {
			 MensajesInterfaz Insert;
			 try
			 {
				 Predial.cDiferencia.Add(obj); 
				 Predial.SaveChanges();
				 Insert = MensajesInterfaz.Ingreso;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cDiferenciaBL.Insert.DbUpdateException", ex);
				 Insert = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cDiferenciaBL.Insert.DataException", ex);
				 Insert = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cDiferenciaBL.Insert.Exception", ex);
				 Insert = MensajesInterfaz.ErrorGeneral;
			 }
			 return Insert;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Update(cDiferencia obj)
		 {
			 MensajesInterfaz Update;
            try
            {
                cDiferencia objOld = Predial.cDiferencia.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdPredio = obj.IdPredio;
                objOld.FechaAplicacion = obj.FechaAplicacion;
                objOld.Avaluo = obj.Avaluo;
                objOld.AvaluoBInicial = obj.AvaluoBInicial;
                objOld.AvaluoEjercicioInicial = obj.AvaluoEjercicioInicial;
                objOld.AvaluoBFinal = obj.AvaluoBFinal;
                objOld.AvaluoEjercicioFinal = obj.AvaluoEjercicioFinal;
                objOld.Construccion = obj.Construccion;
                objOld.ConstruccionBInicial = obj.ConstruccionBInicial;
                objOld.ConstruccionEjercicioInicial = obj.ConstruccionEjercicioInicial;
                objOld.ConstruccionBFinal = obj.ConstruccionBFinal;
                objOld.ConstruccionEjercicioFinal = obj.ConstruccionEjercicioFinal;
                objOld.Traslado = obj.Traslado;
                objOld.TrasladoBInicial = obj.TrasladoBInicial;
                objOld.TrasladoEjercicioInicial = obj.TrasladoEjercicioInicial;
                objOld.TrasladoBFinal = obj.TrasladoBFinal;
                objOld.TrasladoEjercicioFinal = obj.TrasladoEjercicioFinal;
                objOld.Status = obj.Status;
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Update = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("cDiferenciaBL.Update.DbUpdateException", ex);
                Update = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("cDiferenciaBL.Update.DataException", ex);
                Update = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("cDiferenciaBL.Update.Exception", ex);
                Update = MensajesInterfaz.ErrorGeneral;
            }
			 return Update;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="id"></param>
		 /// <returns></returns>
		 public cDiferencia GetByConstraint(int id)
		 {
			 cDiferencia obj = null;
			 try
			 {
				 obj = Predial.cDiferencia.FirstOrDefault(o => o.Id == id);
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cDescuento.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
			 }
			 return obj;
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="obj"></param>
		 /// <returns></returns>
		 public MensajesInterfaz Delete(cDiferencia obj)
		 {
			 MensajesInterfaz Delete;
			 try
			 {
				 cDiferencia objOld = Predial.cDiferencia.FirstOrDefault(c => c.Id == obj.Id);
				 objOld.Activo = obj.Activo;
				 objOld.IdUsuario = obj.IdUsuario;
				 objOld.FechaModificacion = obj.FechaModificacion;
				 Predial.SaveChanges();
				 Delete = MensajesInterfaz.Actualizacion;
			 }
			 catch (DbUpdateException ex)
			 {
				 new Utileria().logError("cDiferenciaBL.Delete.DbUpdateException", ex);
				 Delete = MensajesInterfaz.ErrorGuardar;
			 }
			 catch (DataException ex)
			 {
                 new Utileria().logError("cDiferenciaBL.Delete.DataException", ex);
				 Delete = MensajesInterfaz.ErrorDB;
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cDiferenciaBL.Delete.Exception", ex);
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
		 public List<cDiferencia> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
		 {
			 List<cDiferencia> objList = null;
			 try
			 {
				 if (campoFiltro == string.Empty)
				 {
					  if (activos.ToUpper()=="TRUE")
                          objList = Predial.cDiferencia.SqlQuery("SELECT Id,IdPredio,FechaAplicacion,Avaluo,AvaluoBInicial,AvaluoEjercicioInicial,AvaluoBFinal,AvaluoEjercicioFinal,Construccion,ConstruccionBInicial,ConstruccionEjercicioInicial,ConstruccionBFinal,ConstruccionEjercicioFinal,Traslado,TrasladoBInicial,TrasladoEjercicioInicial,TrasladoBFinal,TrasladoEjercicioFinal,Status,Activo,IdUsuario,FechaModificacion FROM cDiferencia where Activo=1 order by " + campoSort + " " + tipoSort).ToList();
					  else
                          objList = Predial.cDiferencia.SqlQuery("SELECT Id,IdPredio,FechaAplicacion,Avaluo,AvaluoBInicial,AvaluoEjercicioInicial,AvaluoBFinal,AvaluoEjercicioFinal,Construccion,ConstruccionBInicial,ConstruccionEjercicioInicial,ConstruccionBFinal,ConstruccionEjercicioFinal,Traslado,TrasladoBInicial,TrasladoEjercicioInicial,TrasladoBFinal,TrasladoEjercicioFinal,Status,Activo,IdUsuario,FechaModificacion FROM cDiferencia where Activo=0 order by " + campoSort + " " + tipoSort).ToList();
				 }
				 else
				 {
					  //valorFiltro = "%" + valorFiltro + "%";
                      campoFiltro = campoFiltro == "ClavePredial" ? "IdPredio" : campoFiltro;
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.cDiferencia.SqlQuery("Select Id,IdPredio,FechaAplicacion,Avaluo,AvaluoBInicial,AvaluoEjercicioInicial,AvaluoBFinal,AvaluoEjercicioFinal,Construccion,ConstruccionBInicial,ConstruccionEjercicioInicial,ConstruccionBFinal,ConstruccionEjercicioFinal,Traslado,TrasladoBInicial,TrasladoEjercicioInicial,TrasladoBFinal,TrasladoEjercicioFinal,Status,Activo,IdUsuario,FechaModificacion from cDiferencia where Activo=1 and " + campoFiltro + "  =  " + valorFiltro + "order by " + campoSort + " " + tipoSort).ToList(); //, new SqlParameter("@p", valorFiltro)).ToList();
					  else
                          objList = Predial.cDiferencia.SqlQuery("Select Id,IdPredio,FechaAplicacion,Avaluo,AvaluoBInicial,AvaluoEjercicioInicial,AvaluoBFinal,AvaluoEjercicioFinal,Construccion,ConstruccionBInicial,ConstruccionEjercicioInicial,ConstruccionBFinal,ConstruccionEjercicioFinal,Traslado,TrasladoBInicial,TrasladoEjercicioInicial,TrasladoBFinal,TrasladoEjercicioFinal,Status,Activo,IdUsuario,FechaModificacion from cDiferencia where Activo=0 and " + campoFiltro + " = " + valorFiltro + "order by " + campoSort + " " + tipoSort).ToList(); // " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
			 }
			 catch (Exception ex)
			 {
                 new Utileria().logError("cDiferenciaBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
			 }
			 return objList;
		 }

		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name=""></param>
		 /// <returns></returns>
		 public List<cDiferencia> GetAll()
		 {
			 List<cDiferencia> objList = null;
			 try
			 {
				 objList = Predial.cDiferencia.Where(o => o.Activo==true).ToList();
			 }
			 catch (Exception ex)
			 {
				 new Utileria().logError("cDiferenciaBL.GetAll.Exception", ex);
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
                 cDiferencia pObject = new cDiferencia();
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
                 new Utileria().logError("cDiferenciaBL.ListaCampos.Exception", ex);
             }
			
		 return propertyList;
		 }

         public cDiferencia GetByClaveCatastral(int IdPredio)
         {
             cDiferencia obj = null;
             try
             {
                 obj = Predial.cDiferencia.FirstOrDefault(o => o.IdPredio == IdPredio && o.Status == "A" && o.Activo == true);
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cDiferenciaBL.GetByClaveCatastral.Exception", ex , "--Parámetros IdPredio:" + IdPredio);
                 obj = null;
             }
             return obj;
         }

         public List<cDiferencia> GetListDiferenciaByPredio(int IdPredio)
         {
             List<cDiferencia> obj = new List<cDiferencia>();
             try
             {
                 obj = Predial.cDiferencia.Where(x => x.IdPredio == IdPredio && x.Activo == true).ToList();
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cDiferenciaBL.GetListDiferenciaByPredio.Exception", ex , "--Parámetros IdPredio:" + IdPredio);
             }
             return obj;
         }

        public string GetDiferenciaPeriodoByPredio(string clave)
        {
            string periodo = string.Empty;
            List<cDiferencia> obj = new List<cDiferencia>();
            try
            {
                cPredio p = new cPredioBL().GetByClavePredial(clave);
                periodo = "";
                obj = Predial.cDiferencia.Where(x => x.IdPredio == p.Id && x.Activo == true).ToList();
                if (obj != null)
                {

                    foreach (var v in obj)
                    {
                        if (v.Avaluo > 0)
                        {
                            periodo = "DIFERENCIAS POR AVALUO. PERIODO " + " " + v.AvaluoBInicial.ToString() + " " + v.AvaluoEjercicioInicial.ToString() + " al " + v.AvaluoBFinal.ToString() + " " + v.AvaluoEjercicioFinal.ToString();
                        }
                        if (v.Construccion > 0)
                        {
                            periodo = periodo + "DIFERENCIAS POR CONSTRUCCION. PERIODO " + " " + v.AvaluoBInicial.ToString() + " " + v.ConstruccionEjercicioInicial.ToString() + " al " + v.ConstruccionBFinal.ToString() + " " + v.ConstruccionEjercicioFinal.ToString();
                        }
                        if (v.Traslado > 0)
                        {
                            periodo = periodo + "DIFERENCIAS POR TRASLADO DE DOMINIO. PERIODO " + " " + v.TrasladoBInicial.ToString() + " " + v.TrasladoEjercicioInicial.ToString() + " al " + v.TrasladoBFinal.ToString() + " " + v.TrasladoEjercicioFinal.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("cDiferenciaBL.GetListDiferenciaByPredio.Exception", ex, "--Parámetros IdPredio:" + periodo);
            }
            return periodo;
        }
        public List<Diferencias> CalculaDiferencias(int idClavePredial,ref List<cTarifaRecargo> listRecargos)
         {
             cDiferencia dif = new cDiferencia();
             List<Diferencias> diferencias = new List<Diferencias>();
             Diferencias d = new Diferencias();
             decimal indiceAct = 0;
             decimal indiceAnt = 0;
             SaldosC s = new SaldosC();
             MensajesInterfaz msg = new MensajesInterfaz();
             string AplicaActualizacion = new cParametroSistemaBL().GetValorByClave("APLICARACTUALIZACIONINP").ToString();

            try
             {
                decimal porcAdicional = new cParametroCobroBL().GetByClave("PorcentajeAdicional");

                Periodo per = new Periodo();
                DateTime hoy = DateTime.Today;
                int bimActual = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(hoy.Month / 2)));
                int mesActual = DateTime.Today.Month;
                decimal bimestres;

                indiceAct = 0;
                if (AplicaActualizacion == "SI")
                {
                    indiceAct = new cIndicePrecioBL().ValorIndiceActual();
                    if (indiceAct == 0)
                    {
                        indiceAct = new cIndicePrecioBL().ValorIndiceActualSinCambio();
                        if (indiceAct == 0)
                        {
                            d.mensaje = MensajesInterfaz.INPactual;
                            diferencias.Add(d);
                            return diferencias;
                        }
                    }
                }

                 dif = new cDiferenciaBL().GetByClaveCatastral(idClavePredial);
                 int bim = 0, aIni = 0;
                 if (dif != null)
                 {
             
                    //Validar si es cobro antici
                     #region Avaluo
                     if (dif.Avaluo > 0)
                     {

                         bimestres = s.CuentaBimestre(dif.AvaluoBInicial.Value, dif.AvaluoEjercicioInicial.Value, dif.AvaluoBFinal.Value, dif.AvaluoEjercicioFinal.Value, ref msg);
                         d.Periodo = dif.AvaluoBInicial.Value.ToString() + " " + dif.AvaluoEjercicioInicial.Value.ToString() + " al  " + dif.AvaluoBFinal.Value.ToString() + " " + dif.AvaluoEjercicioFinal.Value;
                         if (msg > 0)
                         {
                             d.mensaje = MensajesInterfaz.PeriodoIncorrectoDif;
                             diferencias.Add(d);
                             return diferencias;
                         }
                         bim = dif.AvaluoBInicial.Value;
                         aIni = dif.AvaluoEjercicioInicial.Value;
                         for (int i = 1; i <= bimestres; i++)
                         {
                             d = new Diferencias();
                             if (bim == 7)
                             {
                                 bim = 1;
                                 aIni++;
                             }
                                        
                            d.Tipo = "AVALUO";
                            d.Ejercicio = aIni;
                            d.Bimestre = bim;
                            d.Diferencia = dif.Avaluo.Value;
                            d.ActualizacionINP = 0;
                            if (Convert.ToInt32(aIni.ToString() + bim.ToString()) <= Convert.ToInt32(DateTime.Today.Year.ToString() + bimActual.ToString()))
                            {
                                indiceAnt = new cIndicePrecioBL().ValorIndiceAnterior(aIni, bim);
                                if (indiceAnt == 0)//validando indice
                                {
                                    d.mensaje = MensajesInterfaz.INPanterior;
                                    diferencias.Add(d);
                                    return diferencias;
                                }
                            
                                 d.PorcentajeRecargo = new cTarifaRecargoBL().PorcentajeRecargo(bim, aIni, bimActual, hoy.Year,ref listRecargos);
                                 d.PorcentajeINP = (indiceAct / indiceAnt) - 1;
                                 if (indiceAnt > indiceAct) d.PorcentajeINP = 0;
                                 d.ActualizacionINP = dif.Avaluo.Value * d.PorcentajeINP;
                             }
                             d.Adicional = (d.Diferencia + d.ActualizacionINP) * (porcAdicional / 100);
                             d.Recargo = (d.Diferencia + d.ActualizacionINP + d.Adicional) * d.PorcentajeRecargo;
                             d.Descuento = 0;
                             d.Total = d.Diferencia + d.Recargo + d.ActualizacionINP + d.Adicional;
                             diferencias.Add(d);
                             bim++;
                         }
                     }
                     #endregion
                     #region Construccion
                     bim = 0; aIni = 0;
                     if (dif.Construccion > 0)
                     {
                         bimestres = s.CuentaBimestre(dif.ConstruccionBInicial.Value, dif.ConstruccionEjercicioInicial.Value, dif.ConstruccionBFinal.Value, dif.ConstruccionEjercicioFinal.Value, ref msg);
                         d.Periodo = dif.ConstruccionBInicial.Value.ToString() + " " + dif.ConstruccionEjercicioInicial.Value.ToString() + " al " + dif.ConstruccionBFinal.Value.ToString() + " " + dif.ConstruccionEjercicioFinal.Value.ToString();                         if (msg > 0)
                         {
                             d.mensaje = MensajesInterfaz.PeriodoIncorrectoDif;
                             diferencias.Add(d);
                             return diferencias;
                         }
                         bim = dif.ConstruccionBInicial.Value;
                         aIni = dif.ConstruccionEjercicioInicial.Value;
                         for (int i = 1; i <= bimestres; i++)
                         {
                             d = new Diferencias();
                             if (bim == 7)

                             {
                                 bim = 1;
                                 aIni++;
                             }
                             //indiceAnt = new cIndicePrecioBL().ValorIndiceAnterior(aIni, bim);
                             d.Tipo = "CONSTRUCCION";
                             d.Ejercicio = aIni;
                             d.Bimestre = bim;
                             d.Diferencia = dif.Construccion.Value;
                             d.ActualizacionINP = 0;
                             if (Convert.ToInt32(aIni.ToString() + bim.ToString()) <= Convert.ToInt32(DateTime.Today.Year.ToString() + bimActual.ToString()))
                             {
                                 indiceAnt = new cIndicePrecioBL().ValorIndiceAnterior(aIni, bim);
                                 if (indiceAnt == 0)//validando indice
                                 {
                                     d.mensaje = MensajesInterfaz.INPanterior;
                                     diferencias.Add(d);
                                     return diferencias;
                                 }
                                 d.PorcentajeRecargo = new cTarifaRecargoBL().PorcentajeRecargo(bim, aIni, bimActual, hoy.Year, ref listRecargos);
                                 d.Recargo = dif.Construccion.Value * d.PorcentajeRecargo;
                                 d.PorcentajeINP = (indiceAct / indiceAnt) - 1;
                                 if (indiceAnt > indiceAct) d.PorcentajeINP = 0;
                                 d.ActualizacionINP = dif.Construccion.Value * d.PorcentajeINP;
                             }
                             d.Adicional = (d.Diferencia + d.ActualizacionINP) * (porcAdicional / 100);
                             d.Recargo = (d.Diferencia + d.ActualizacionINP + d.Adicional) * d.PorcentajeRecargo;
                             d.Descuento = 0;
                             d.Total = d.Diferencia + d.Recargo + d.ActualizacionINP + d.Adicional;
                             diferencias.Add(d);
                             bim++;
                         }
                     }
                     #endregion
                     #region Traslado
                     bim = 0; aIni = 0;
                     if (dif.Traslado > 0)
                     {
                         bimestres = s.CuentaBimestre(dif.TrasladoBInicial.Value, dif.TrasladoEjercicioInicial.Value, dif.TrasladoBFinal.Value, dif.TrasladoEjercicioFinal.Value, ref msg);
                         d.Periodo = dif.TrasladoBInicial.Value.ToString() + " " + dif.TrasladoEjercicioInicial.Value.ToString() + " al " + dif.TrasladoBFinal.Value.ToString() + " " + dif.TrasladoEjercicioFinal.Value.ToString();
                         if (msg > 0 )
                         {
                             d.mensaje = MensajesInterfaz.PeriodoIncorrectoDif;
                             diferencias.Add(d);
                             return diferencias;
                         }

                         bim = dif.TrasladoBInicial.Value;
                         aIni = dif.TrasladoEjercicioInicial.Value;
                         for (int i = 1; i <= bimestres; i++)
                         {
                             d = new Diferencias();
                             if (bim == 7)
                             {
                                 bim = 1;
                                 aIni++;
                             }
                             //indiceAnt = new cIndicePrecioBL().ValorIndiceAnterior(aIni, bim);
                             d.Tipo = "TRASLADO";
                             d.Ejercicio = aIni;
                             d.Bimestre = bim;
                             d.Diferencia = dif.Traslado.Value;
                             d.ActualizacionINP = 0;
                             if (Convert.ToInt32(aIni.ToString() + bim.ToString()) <= Convert.ToInt32(DateTime.Today.Year.ToString() + bimActual.ToString()))
                             {
                                 indiceAnt = new cIndicePrecioBL().ValorIndiceAnterior(aIni, bim);
                                 if (indiceAnt == 0)//validando indice
                                 {
                                     d.mensaje = MensajesInterfaz.INPanterior;
                                     diferencias.Add(d);
                                     return diferencias;
                                 }
                                 d.PorcentajeRecargo = new cTarifaRecargoBL().PorcentajeRecargo(bim, aIni, bimActual, hoy.Year,ref listRecargos);
                                 d.Recargo = dif.Traslado.Value * d.PorcentajeRecargo;
                                 d.PorcentajeINP = (indiceAct / indiceAnt) - 1;
                                 if (indiceAnt > indiceAct) d.PorcentajeINP = 0;
                                 d.ActualizacionINP = dif.Traslado.Value * d.PorcentajeINP;
                             }
                             d.Adicional = (d.Diferencia + d.ActualizacionINP) * (porcAdicional / 100);
                             d.Recargo = (d.Diferencia + d.ActualizacionINP + d.Adicional) * d.PorcentajeRecargo;
                             d.Descuento = 0;
                             d.Total = d.Diferencia + d.Recargo + d.ActualizacionINP + d.Adicional;
                             diferencias.Add(d);
                             bim++;
                         }
                     }
                 }
                     #endregion
             }
             catch (Exception ex)
             {
                 new Utileria().logError("cDiferenciaBL.CalculaDiferencias.Exception", ex , "--Parámetros idClavePredial:" + idClavePredial);
             }

             return diferencias;
         } //Calcula diferencias


	 }

}
