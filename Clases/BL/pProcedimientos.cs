using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;
using Clases.Utilerias;

namespace Clases.BL
{
    public class pProcedimientos
    {
        PredialEntities Predial;
        public pProcedimientos()
        {
            Predial = new PredialEntities();
        }
        public List<pVentanasPrimerNivel_Result> ObtieneVentanasPrimerNivel(int idUsuario)
        {
            List<pVentanasPrimerNivel_Result> listVentanas = null;

            try
            {
                listVentanas = Predial.pVentanasPrimerNivel(idUsuario).OrderBy(o => o.orden).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.ObtieneVentanasPrimerNivel.Exception", ex , "--Parámetros idUsuario:" + idUsuario);
            }
            return listVentanas;
        }

        public List<pReporteIngresosPorConcepto_Result> ObtieneReporteIngresoPorConcepto(DateTime fechaInicio, DateTime fechaFin)
        {
            List<pReporteIngresosPorConcepto_Result> listDocumento = null;
            try
            {
                listDocumento = Predial.pReporteIngresosPorConcepto(fechaInicio, fechaFin).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.ObtieneReporteIngresoPorConcepto.Exception", ex , "--Parámetros fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
            }
            return listDocumento;
        }
        public List<pReporteIngresosConsulta_Result> ObtieneReporteIngresosConsulta(int IdCajero, DateTime fechaInicio, DateTime fechaFin)
        {
            List<pReporteIngresosConsulta_Result> listDocumento = null;
            try
            {
                listDocumento = Predial.pReporteIngresosConsulta(IdCajero,fechaInicio, fechaFin).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.ObtieneReporteIngresosConsulta.Exception", ex , "--Parámetros fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
            }
            return listDocumento;
        }

        public List<pReporteIngresosConDescuento_Result> ObtieneReporteIngresosConDescuentos(int IdCajero, DateTime fechaInicio, DateTime fechaFin)
        {
            List<pReporteIngresosConDescuento_Result> listDocumento = null;
            try
            {
                listDocumento = Predial.pReporteIngresosConDescuento(IdCajero, fechaInicio, fechaFin).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.ObtieneReporteIngresosConDescuentos.Exception", ex , "--Parámetros fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
            }
            return listDocumento;
        }
        
        public List<pReporteIngresosPorConceptoAgrupado_Result> ObtieneReporteIngresoPorConceptoAgrupado(DateTime fechaInicio, DateTime fechaFin)
        {
            List<pReporteIngresosPorConceptoAgrupado_Result> listDocumento = null;
            try
            {
                listDocumento = Predial.pReporteIngresosPorConceptoAgrupado(fechaInicio, fechaFin).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.ObtieneReporteIngresoPorConcepto.Exception", ex , "--Parámetros fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
            }
            return listDocumento;
        }
        public List<pReporteIngresosPrepolizaConsulta_Result> ReporteIngresoPrepolizaConsulta(int idPrepoliza)
        {
            List<pReporteIngresosPrepolizaConsulta_Result> listaDetalle = null;
            try
            {
                listaDetalle = Predial.pReporteIngresosPrepolizaConsulta(idPrepoliza).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.ReporteIngresoPrepolizaConsulta.Exception", ex , "--Parámetros idPrepoliza:" + idPrepoliza);
            }

            return listaDetalle;
        }

        public List<int?> ObtieneReporteIngresoRecibos(DateTime fechaInicio, DateTime fechaFin)
        {
            List<int?> listDocumento = null;
            try
            {
                listDocumento = Predial.pReporteIngresosRecibos(fechaInicio, fechaFin).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.ObtieneReporteIngresoRecibos.Exception", ex , "--Parámetros fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
            }
            return listDocumento;
        }

        public List<pReporteIngresos_Result> ObtieneReporteIngreso(DateTime fechaInicio, DateTime fechaFin)
        {
            List<pReporteIngresos_Result> listDocumento = null;
            try
            {
                listDocumento = Predial.pReporteIngresos(fechaInicio, fechaFin).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.ObtieneReporteIngreso.Exception", ex , "--Parámetros fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
            }
            return listDocumento;
        }

        //public List<pReporteIngresos_Result> ObtieneReporteIngreso(int idPrepoliza)
        //{
        //    List<pReporteIngresos_Result> listaDetalle = new List<pReporteIngresos_Result>();
        //    tPrepoliza p = new tPrepolizaBL().GetByConstraint(idPrepoliza);
        //    try
        //    {                
        //        foreach (tPrepolizaDetalle pd in p.tPrepolizaDetalle)
        //        {
        //            pReporteIngresos_Result obj = new pReporteIngresos_Result();
        //            obj.Id = pd.IdConcepto;
        //            obj.Total = pd.Importe;
        //            obj.cri = pd.cConcepto.Cri;
        //            obj.Descripcion = pd.cConcepto.Descripcion;
        //            listaDetalle.Add(obj);
        //        }                
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("pProcedimientos.ObtieneReporteIngreso.Exception", ex , "--Parámetros idPrepoliza:" + idPrepoliza);
        //    }
        //    return listaDetalle;
        //}

        public List<pReporteIngresosXcajero_Result> ObtieneReporteIngresoXcajero(DateTime fechaInicio, DateTime fechaFin, Boolean Concetrado)
        {
            List<pReporteIngresosXcajero_Result> listDocumento = null;
            try
            {
                listDocumento = Predial.pReporteIngresosXcajero(fechaInicio, fechaFin, Concetrado).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.pReporteIngresosXcajero.Exception", ex , "--Parámetros fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
            }
            return listDocumento;
        }

        //public List<pConsultaBitacora_Result> ObtieneConsultaBitacora(string user, string ventana, string clavePredial, DateTime fechaInicio, DateTime fechaFin)
        //{
        //    List<pConsultaBitacora_Result> listDocumento = null;
        //    //int re
        //    //try
        //    //{
        //    //    listDocumento = Predial.pConsultaBitacora(user, ventana, clavePredial, null, null); //.  .ToList();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    new Utileria().logError("pProcedimientos.pConsultaBitacora.Exception", ex , "--Parámetros user:"+ user + ", ventana:" + ventana + ", clavePredial:" + clavePredial + " fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
        //    //}
        //    return listDocumento;
        //}

        public List<pReporteIngresosTabular_Result> ObtieneReporteIngresoTabular(DateTime fechaInicio, DateTime fechaFin)
        {
            List<pReporteIngresosTabular_Result> list = null;
            try
            {
                list = Predial.pReporteIngresosTabular(fechaInicio, fechaFin).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.pReporteIngresosTabular_Result.Exception", ex, "--Parámetros fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
            }
            return list;
        }

        public int actulizaMasivo(string tabla,string IdForaneo, string claves)
        {
           int result= Predial.pActualizaMasivo(tabla, IdForaneo, claves);
            return result;
        }

        public List<pAnualPredialP1_Result> pAnualPredialP1 (int anio)
        {
            List<pAnualPredialP1_Result> list = null;
            try
            {
                list = Predial.pAnualPredialP1(anio).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.pAnualPredialP1.Exception", ex, "--Parámetros anio:" + anio);
            }
            return list;
        }
        public List<pAnualPredialP2_Result> pAnualPredialP2(int anio)
        {
            List<pAnualPredialP2_Result> list = null;
            try
            {
                list = Predial.pAnualPredialP2(anio).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.pAnualPredialP2.Exception", ex, "--Parámetros anio:" + anio);
            }
            return list;
        }
        public List<pAnualPredialP3_Result> pAnualPredialP3(int anio)
        {
            List<pAnualPredialP3_Result> list = null;
            try
            {
                list = Predial.pAnualPredialP3(anio).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.pAnualPredialP3.Exception", ex, "--Parámetros anio:" + anio);
            }
            return list;
        }
        public List<pAnualPredialP4_Result> pAnualPredialP4(int anio)
        {
            List<pAnualPredialP4_Result> list = null;
            try
            {
                list = Predial.pAnualPredialP4(anio).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.pAnualPredialP4.Exception", ex, "--Parámetros anio:" + anio);
            }
            return list;
        }
        public List<pAnualPredialP5_Result> pAnualPredialP5(int anio)
        {
            List<pAnualPredialP5_Result> list = null;
            try
            {
                list = Predial.pAnualPredialP5(anio).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.pAnualPredialP5.Exception", ex, "--Parámetros anio:" + anio);
            }
            return list;
        }
        public List<spCalculaImpuestoHactual_Result> spCalculaImpuestoHactual(int anio)
        {
            List<spCalculaImpuestoHactual_Result> list = null;
            try
            {
                list = Predial.spCalculaImpuestoHactual(anio,false).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.spCalculaImpuestoHactual.Exception", ex, "--Parámetros anio:" + anio);
            }
            return list;
        }
        public List<spCalculaImpuestoH_Result> spCalculaImpuestoH(int anio)
        {
            List<spCalculaImpuestoH_Result> list = null;
            try
            {
                list = Predial.spCalculaImpuestoH(6,anio, false,false).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("pProcedimientos.spCalculaImpuestoHactual.Exception", ex, "--Parámetros anio:" + anio);
            }
            return list;
        }

    }
}
