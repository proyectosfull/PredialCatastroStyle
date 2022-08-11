using Clases.Utilerias;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;
using System.Data;
using System.Configuration;

namespace Clases.BL
{
    public class vVistasBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public vVistasBL()
        {
            Predial = new PredialEntities();
        }

        public List<vPredios> GetFilterVPredio(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<vPredios> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vPredios.SqlQuery("Select Id,ClavePredial,NombreCalle,numero,IdColonia,NombreColonia,CP,localidad,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,FechaAlta,BaseTributacion,FechaAvaluo,FechaTraslado,Zona,MetrosFrente,IdUsoSuelo,IdExentoPago,IdStatusPredio,FechaBaja,IdTipoPredio,IdContribuyente,IdTipoFaseIp,Nivel,UbicacionExpediente,IdCartografia,BimestreFinIp,AaFinalIp,ReciboIp,IdReciboIp,FechaPagoIp,BimestreFinSm,AaFinalSm,ReciboSm,IdReciboSm,FechaPagoSm,IdTipoFaseSm,Activo,IdUsuario,FechaModificacion from vPredios where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.vPredios.SqlQuery("Select Id,ClavePredial,NombreCalle,numero,IdColonia,NombreColonia,CP,localidad,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,FechaAlta,BaseTributacion,FechaAvaluo,FechaTraslado,Zona,MetrosFrente,IdUsoSuelo,IdExentoPago,IdStatusPredio,FechaBaja,IdTipoPredio,IdContribuyente,IdTipoFaseIp,Nivel,UbicacionExpediente,IdCartografia,BimestreFinIp,AaFinalIp,ReciboIp,IdReciboIp,FechaPagoIp,BimestreFinSm,AaFinalSm,ReciboSm,IdReciboSm,FechaPagoSm,IdTipoFaseSm,Activo,IdUsuario,FechaModificacion from vPredios where activo=0 order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vPredios.SqlQuery("Select Id,ClavePredial,NombreCalle,numero,IdColonia,NombreColonia,CP,localidad,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,FechaAlta,BaseTributacion,FechaAvaluo,FechaTraslado,Zona,MetrosFrente,IdUsoSuelo,IdExentoPago,IdStatusPredio,FechaBaja,IdTipoPredio,IdContribuyente,IdTipoFaseIp,Nivel,UbicacionExpediente,IdCartografia,BimestreFinIp,AaFinalIp,ReciboIp,IdReciboIp,FechaPagoIp,BimestreFinSm,AaFinalSm,ReciboSm,IdReciboSm,FechaPagoSm,IdTipoFaseSm,Activo,IdUsuario,FechaModificacion from vPredios where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.vPredios.SqlQuery("Select Id,ClavePredial,NombreCalle,numero,IdColonia,NombreColonia,CP,localidad,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,FechaAlta,BaseTributacion,FechaAvaluo,FechaTraslado,Zona,MetrosFrente,IdUsoSuelo,IdExentoPago,IdStatusPredio,FechaBaja,IdTipoPredio,IdContribuyente,IdTipoFaseIp,Nivel,UbicacionExpediente,IdCartografia,BimestreFinIp,AaFinalIp,ReciboIp,IdReciboIp,FechaPagoIp,BimestreFinSm,AaFinalSm,ReciboSm,IdReciboSm,FechaPagoSm,IdTipoFaseSm,Activo,IdUsuario,FechaModificacion from vPredios where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.GetFilterVPredio.Exception", ex,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }
        public List<string> ListaCamposVPredios()
        {
            List<string> propertyList = new List<string>();
            try
            {
                vPredios pObject = new vPredios();
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
                new Utileria().logError("vVistasBL.ListaCamposVPredios.Exception", ex);
            }

            return propertyList;
        }

        public List<vVentanasNivel2> ObtieneNivel2Menu(int idUsario)
        {
            List<vVentanasNivel2> detalle = null;

            try
            {
                detalle = Predial.vVentanasNivel2.Where(r => r.IdUsuario == idUsario && r.url != null).OrderBy(o => o.orden).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtieneNivel2Menu.Exception", ex, "--Parámetros idUsario:" + idUsario);
            }
            return detalle;

        }
        public List<vVentanasNivel2> ObtieneNivel2MenuByNivel1(List<vVentanasNivel2> nivel2, int idVentana)
        {
            List<vVentanasNivel2> detalle = null;

            try
            {
                detalle = nivel2.Where(r => r.idPapa == idVentana).OrderBy(o => o.orden).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtieneNivel2Menu.Exception", ex);
            }
            return detalle;

        }

        public List<vVentanasNivel2Permisos> ObtieneNivel2Permisos(int idUsario)
        {
            List<vVentanasNivel2Permisos> detalle = null;

            try
            {
                detalle = Predial.vVentanasNivel2Permisos.Where(r => r.IdUsuario == idUsario && r.url != null).OrderBy(o => o.orden).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtieneNivel2Menu.Exception", ex, "--Parámetros idUsario:" + idUsario);
            }
            return detalle;

        }
        public List<vBaseGravable> GetFilterVBaseGravable(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<vBaseGravable> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vBaseGravable.SqlQuery("SELECT Nombre,Ejercicio,Bimestre,Valor,Id,IdPredio,ClavePredial,SuperficieTerreno,FechaAvaluo,ValorConstruccion,Activo,IdUsuario,FechaModificacion FROM vBaseGravable where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.vBaseGravable.SqlQuery("SELECT Nombre,Ejercicio,Bimestre,Valor,Id,IdPredio,ClavePredial,SuperficieTerreno,FechaAvaluo,ValorConstruccion,Activo,IdUsuario,FechaModificacion FROM vBaseGravable where activo=0 order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vBaseGravable.SqlQuery("SELECT Nombre,Ejercicio,Bimestre,Valor,Id,IdPredio,ClavePredial,SuperficieTerreno,FechaAvaluo,ValorConstruccion,Activo,IdUsuario,FechaModificacion FROM vBaseGravable where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.vBaseGravable.SqlQuery("SELECT Nombre,Ejercicio,Bimestre,Valor,Id,IdPredio,ClavePredial,SuperficieTerreno,FechaAvaluo,ValorConstruccion,Activo,IdUsuario,FechaModificacion FROM vBaseGravable where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.GetFiltervBaseGravable.Exception", ex,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }

        public List<vBaseGravable> GetVBaseGravableById(string valorFiltro, string campoSort, string tipoSort)
        {
            List<vBaseGravable> objList = null;
            try
            {
                objList = Predial.vBaseGravable.SqlQuery("SELECT Nombre,Ejercicio,Bimestre,Valor,Id,IdPredio,ClavePredial,SuperficieTerreno,FechaAvaluo,ValorTerreno,ValorConstruccion,Activo,IdUsuario,FechaModificacion FROM vBaseGravable where activo=1 AND IDPREDIO = @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();

            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.GetVBaseGravableById.Exception", ex,
                     "--Parámetros  valorFiltro:" + valorFiltro + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }
        public List<string> ListaCamposVBaseGravable()
        {
            List<string> propertyList = new List<string>();
            try
            {
                vBaseGravable pObject = new vBaseGravable();
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
                new Utileria().logError("vVistasBL.ListaCamposVBaseGravable.Exception", ex);
            }
            return propertyList;
        }
        public List<vPadronPredio> ObtienePadron(int status, int anio, int bimestre, int tipo, string clave, string listaclaves, string hdfIdCon, string hdfIdCondominio, string hdfIdColonia, string claveIni, string claveFin)
        {
            List<vPadronPredio> detalle = null;

            try
            {
                //detalle = Predial.vPadronPredio.ToList();
                if (status == 0)
                {
                    detalle = Predial.vPadronPredio.ToList();
                }
                if (status != 0  )
                {
                    if (status == 4)
                        detalle = Predial.vPadronPredio.Where(r => r.IdStatusPredio != 2).ToList();
                    else
                        detalle = Predial.vPadronPredio.Where(r => r.IdStatusPredio == status).ToList();
                }
                if (anio != 0)
                {
                    detalle = Predial.vPadronPredio.Where(r => r.AaFinalIp == anio).ToList();
                }
                if (bimestre != 0)
                {
                    detalle = Predial.vPadronPredio.Where(r => r.BimestreFinIp == bimestre).ToList();
                }
                if (tipo != 0)
                {
                    detalle = Predial.vPadronPredio.Where(r => r.IdTipoPredio == tipo).ToList();
                }
                if (clave != "")
                {
                    detalle = Predial.vPadronPredio.Where(r => r.ClavePredial.Contains(clave.Replace(" ", ""))).ToList();
                }
                if (listaclaves != "")
                {
                    String[] claves = listaclaves.Split(',');
                    detalle = Predial.vPadronPredio.Where(p => claves.Contains(p.ClavePredial.Replace(" ", ""))).ToList();
                }
                if (hdfIdCon.ToUpper() != "")
                {
                    detalle = Predial.vPadronPredio.Where(p => p.Contribuyente.Contains(hdfIdCon)).ToList();
                }
                if (hdfIdCondominio != "")
                {
                    List<cPredio> lPred = new cPredioBL().GetByCondominio(Convert.ToInt32(hdfIdCondominio));
                    string lcl = "";
                    foreach (cPredio p in lPred)
                    {
                        if (lcl == "")
                            lcl = p.ClavePredial;
                        else
                            lcl = lcl + "," + p.ClavePredial;
                    }
                    String[] claves = lcl.Split(',');
                    detalle = Predial.vPadronPredio.Where(o => claves.Contains(o.ClavePredial.Replace(" ", ""))).ToList();
                }
                if (hdfIdColonia != "")
                {
                    List<cPredio> lPred = new cPredioBL().GetByColonia(Convert.ToInt32(hdfIdColonia));
                    string lcl = "";
                    foreach (cPredio p in lPred)
                    {
                        if (lcl == "")
                            lcl = p.ClavePredial;
                        else
                            lcl = lcl + "," + p.ClavePredial;
                    }
                    String[] claves = lcl.Split(',');
                    detalle = Predial.vPadronPredio.Where(o => claves.Contains(o.ClavePredial.Replace(" ", ""))).ToList();
                }

                if (claveIni != "" && claveFin != "")
                {

                    Int64 ini, fin;
                    ini = Convert.ToInt64(claveIni);
                    fin = Convert.ToInt64(claveFin);
                    detalle = Predial.vPadronPredio.Where(r => r.ClaveNumerica >= ini && r.ClaveNumerica <= fin).ToList();
                }


            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtienePadron.Exception", ex,
                     "--Parámetros status:" + status + ", anio:" + anio + ", bimestre:" + bimestre + ", tipo:" + tipo);
            }
            return detalle;

        }
        public List<int?> ObtienePadronAnios(int status, int bimestre, int tipo)
        {
            List<vPadronPredioAnio> detalle = null;
            List<int?> resultado = null;
            try
            {
                //detalle = Predial.vPadronPredioAnio.ToList();
                if (status != 0)
                {
                    detalle = Predial.vPadronPredioAnio.Where(r => r.IdStatusPredio == status).ToList();
                }
                if (bimestre != 0)
                {
                    detalle = Predial.vPadronPredioAnio.Where(r => r.BimestreFinIp == bimestre).ToList();
                }
                if (tipo != 0)
                {
                    detalle = Predial.vPadronPredioAnio.Where(r => r.IdTipoPredio == tipo).ToList();
                }
                resultado = Predial.vPadronPredioAnio.Where(m => m.AaFinalIp > 1900).OrderByDescending(m => m.AaFinalIp).Select(m => m.AaFinalIp).Distinct().ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtienePadronAnios.Exception", ex, "--Parámetros status:" + status + ", bimestre:" + bimestre + ", tipo:" + tipo);
            }
            return resultado;
        }
        public List<vPadronPredio> ObtienePadronAllAnios()
        {
            List<vPadronPredio> detalle = null;

            try
            {
                detalle = Predial.vPadronPredio.ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtienePadronAllAnios.Exception", ex);
            }
            return detalle;

        }

        public List<vPadronPredio> ObtienePadronByFechaAlta(int mes, int anio)
        {
            List<vPadronPredio> detalle = null;

            try
            {
                detalle = Predial.vPadronPredio.ToList();
                if (mes != 0)
                {
                    detalle = detalle.Where(r => r.mes == mes).ToList();
                }
                if (anio != 0)
                {
                    detalle = detalle.Where(r => r.anoAlta == anio).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtienePadronByFechaAlta.Exception", ex, "--Parámetros mes:" + mes + ", anio:" + anio);
            }
            return detalle;

        }

        public List<vConfiguracionMesa> ObtieneMesaUsuario(int idUsuario)
        {
            List<vConfiguracionMesa> listDocumento = null;

            try
            {
                listDocumento = Predial.vConfiguracionMesa.Where(v => v.IdCajero == idUsuario).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistas.ObtieneMesaUsuario.Exception", ex, "--Parámetros idUsuario:" + idUsuario);
            }
            return listDocumento;
        }

        public List<vRecibo> ObtieneRecibo(int NoRecibo, String clavepredial, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<vRecibo> listRecibo = null;

            try
            {
                if (NoRecibo != 0)
                {
                    listRecibo = Predial.vRecibo.Where(v => v.Id == NoRecibo).ToList();
                }
                else if (fechaInicio != null)
                {
                    listRecibo = Predial.vRecibo.Where(v => v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                }
                else if (clavepredial != String.Empty)
                {
                    listRecibo = Predial.vRecibo.Where(v => v.ClavePredial == clavepredial).ToList();
                }
                else
                {
                    listRecibo = Predial.vRecibo.ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vRecibo.ObtieneRecibo.Exception", ex, "--Parámetros NoRecibo:" + NoRecibo + ", fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
            }
            return listRecibo;

        }

        public List<vRecibo> ObtieneRecibo(int NoRecibo, String clavepredial, DateTime? fechaInicio, DateTime? fechaFin, string estado)
        {
            List<vRecibo> listRecibo = null;

            try
            {
                if (NoRecibo != 0)
                {
                    listRecibo = Predial.vRecibo.Where(v => v.EstadoRecibo == estado && v.Id == NoRecibo).ToList();
                }
                else if (fechaInicio != null)
                {
                    listRecibo = Predial.vRecibo.Where(v => v.EstadoRecibo == estado && v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                }
                else if (clavepredial != String.Empty)
                {
                    listRecibo = Predial.vRecibo.Where(v => v.EstadoRecibo == estado && v.ClavePredial == clavepredial).ToList();
                }
                else
                {
                    listRecibo = Predial.vRecibo.Where(v => v.EstadoRecibo == estado).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vRecibo.ObtieneRecibo.Exception", ex, "--Parámetros NoRecibo:" + NoRecibo + ", fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin + ", estado:" + estado);
            }
            return listRecibo;
        }

        public List<vReciboDetalle> ObtieneReciboDetalle(int NoRecibo)
        {
            List<vReciboDetalle> listReciboDetalle = null;

            try
            {
                listReciboDetalle = Predial.vReciboDetalle.Where(v => v.IdRecibo == NoRecibo).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vRecibo.ObtieneReciboDetalle.Exception", ex, "--Parámetros NoRecibo:" + NoRecibo);
            }
            return listReciboDetalle;
        }
        public vRecibo ObtieneRecibo(int NoRecibo)
        {
            vRecibo Recibo = null;

            try
            {
                Recibo = Predial.vRecibo.FirstOrDefault(r => r.Id == NoRecibo);

            }
            catch (Exception ex)
            {
                new Utileria().logError("vRecibo.ObtieneRecibo.Exception", ex, "--Parámetros NoRecibo:" + NoRecibo);
            }
            return Recibo;

        }

        public List<vReportesIngresos> ObtieneReportesIngresos(DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<vReportesIngresos> objList = null;
            if (fechaFin != null)
                fechaFin = ((DateTime)fechaFin).AddDays(1);
            try
            {
                if (fechaInicio == null && fechaInicio == null)
                {
                    objList = Predial.vReportesIngresos.OrderByDescending(v => v.FechaPrePoliza).ToList();
                }
                else
                {
                    objList = Predial.vReportesIngresos.Where(o => o.FechaPrePoliza <= fechaFin && o.FechaPrePoliza >= fechaInicio).OrderByDescending(v => v.FechaPrePoliza).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistas.ObtieneReportesIngresos.Exception", ex, "--Parámetros fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
            }
            return objList;
        }

        public List<vConfiguracionMesa> ObtieneConfiguracionMesa(int idMesa)
        {
            List<vConfiguracionMesa> listDocumento = null;

            try
            {
                if (idMesa != 0)
                {
                    listDocumento = Predial.vConfiguracionMesa.Where(v => v.IdMesa == idMesa).ToList();
                }
                else
                {
                    listDocumento = Predial.vConfiguracionMesa.ToList();
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistas.ObtieneConfiguracionMesa.Exception", ex, "--Parámetros idMesa:" + idMesa);
            }
            return listDocumento;
        }

        public List<vPrediosCorto> GetFilterVPredioCorto(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<vPrediosCorto> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vPrediosCorto.SqlQuery("Select Id,ClavePredial,NombreCalle,Nombre,NombreColonia,CP,numero,localidad,Activo,IdUsuario,FechaModificacion from vPrediosCorto where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.vPrediosCorto.SqlQuery("Select Id,ClavePredial,NombreCalle,Nombre,NombreColonia,CP,numero,localidad,Activo,IdUsuario,FechaModificacion from vPrediosCorto where activo=0 order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "'%" + valorFiltro + "%'";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vPrediosCorto.SqlQuery("Select Id,ClavePredial,NombreCalle,Nombre,NombreColonia,CP,numero,localidad,Activo,IdUsuario,FechaModificacion from vPrediosCorto where activo=1 and " + campoFiltro + " like "+ valorFiltro + " order by " + campoSort + " " + tipoSort).ToList(); //, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.vPrediosCorto.SqlQuery("Select Id,ClavePredial,NombreCalle,Nombre,NombreColonia,CP,numero,localidad,Activo,IdUsuario,FechaModificacion from vPrediosCorto where activo=0 and " + campoFiltro + " like " + valorFiltro + " order by " + campoSort + " " + tipoSort).ToList(); //, ).ToList();new SqlParameter("@p", valorFiltro)
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBLCorto.GetFilterVPredioCorto.Exception", ex,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }
        public List<vBaseGravableCorto> GetFilterVBaseGravableCorto(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<vBaseGravableCorto> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vBaseGravableCorto.SqlQuery("SELECT Nombre,Ejercicio,Bimestre,Valor,Id,ClavePredial,FechaAvaluo,ValorConstruccion,Activo FROM vBaseGravableCorto where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.vBaseGravableCorto.SqlQuery("SELECT Nombre,Ejercicio,Bimestre,Valor,Id,ClavePredial,FechaAvaluo,ValorConstruccion,Activo FROM vBaseGravableCorto where activo=0 order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vBaseGravableCorto.SqlQuery("SELECT Nombre,Ejercicio,Bimestre,Valor,Id,ClavePredial,FechaAvaluo,ValorConstruccion,Activo FROM vBaseGravableCorto where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.vBaseGravableCorto.SqlQuery("SELECT Nombre,Ejercicio,Bimestre,Valor,Id,ClavePredial,FechaAvaluo,ValorConstruccion,Activo FROM vBaseGravableCorto where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vBaseGravableCorto.GetFiltervBaseGravableCorto.Exception", ex,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }

        public List<vPredios> GetFilterVPredioPropietario(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<vPredios> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vPredios.SqlQuery("Select Id,ClavePredial,NombreCalle,numero,IdColonia,NombreColonia,CP,localidad,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,FechaAlta,BaseTributacion,FechaAvaluo,FechaTraslado,Zona,MetrosFrente,IdUsoSuelo,IdExentoPago,IdStatusPredio,FechaBaja,IdTipoPredio,IdContribuyente,IdTipoFaseIp,Nivel,UbicacionExpediente,IdCartografia,BimestreFinIp,AaFinalIp,ReciboIp,IdReciboIp,FechaPagoIp,BimestreFinSm,AaFinalSm,ReciboSm,IdReciboSm,FechaPagoSm,IdTipoFaseSm,Activo,IdUsuario,FechaModificacion from vPredios where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.vPredios.SqlQuery("Select Id,ClavePredial,NombreCalle,numero,IdColonia,NombreColonia,CP,localidad,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,FechaAlta,BaseTributacion,FechaAvaluo,FechaTraslado,Zona,MetrosFrente,IdUsoSuelo,IdExentoPago,IdStatusPredio,FechaBaja,IdTipoPredio,IdContribuyente,IdTipoFaseIp,Nivel,UbicacionExpediente,IdCartografia,BimestreFinIp,AaFinalIp,ReciboIp,IdReciboIp,FechaPagoIp,BimestreFinSm,AaFinalSm,ReciboSm,IdReciboSm,FechaPagoSm,IdTipoFaseSm,Activo,IdUsuario,FechaModificacion from vPredios where activo=0 order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "'%" + valorFiltro + "%'";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vPredios.SqlQuery("Select vP.Id,ClavePredial,NombreCalle,vP.numero,IdColonia,NombreColonia,vP.CP,vP.localidad,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,FechaAlta,BaseTributacion,FechaAvaluo,FechaTraslado,Zona,MetrosFrente,IdUsoSuelo,IdExentoPago,IdStatusPredio,FechaBaja,IdTipoPredio,IdContribuyente,IdTipoFaseIp,Nivel,UbicacionExpediente,IdCartografia,BimestreFinIp,AaFinalIp,ReciboIp,IdReciboIp,FechaPagoIp,BimestreFinSm,AaFinalSm,ReciboSm,IdReciboSm,FechaPagoSm,IdTipoFaseSm,vP.Activo,vP.IdUsuario,vP.FechaModificacion  from vPredios vP join cContribuyente cC on cC.Id = vP.IdContribuyente where vP.activo=1 and " + campoFiltro + " like "+ valorFiltro + " order by " + campoSort + " " + tipoSort).ToList(); //, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.vPredios.SqlQuery("Select vP.Id,ClavePredial,NombreCalle,vP.numero,IdColonia,NombreColonia,vP.CP,vP.localidad,SuperficieTerreno,TerrenoPrivativo,TerrenoComun,ValorTerreno,SuperficieConstruccion,ConstruccionPrivativa,ConstruccionComun,ValorConstruccion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,FechaAlta,BaseTributacion,FechaAvaluo,FechaTraslado,Zona,MetrosFrente,IdUsoSuelo,IdExentoPago,IdStatusPredio,FechaBaja,IdTipoPredio,IdContribuyente,IdTipoFaseIp,Nivel,UbicacionExpediente,IdCartografia,BimestreFinIp,AaFinalIp,ReciboIp,IdReciboIp,FechaPagoIp,BimestreFinSm,AaFinalSm,ReciboSm,IdReciboSm,FechaPagoSm,IdTipoFaseSm,vP.Activo,vP.IdUsuario,vP.FechaModificacion  from vPredios vP join cContribuyente cC on cC.Id = vP.IdContribuyente where vP.activo=0 and " + campoFiltro + " like " + valorFiltro + " order by " + campoSort + " " + tipoSort).ToList(); //, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.GetFilterVPredioPropietario.Exception", ex,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }
        public vPredios GetByConstraint(int id)
        {
            vPredios obj = null;
            try
            {
                obj = Predial.vPredios.FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.GetByConstraint.Exception", ex, "--Parámetros id:" + id);
            }
            return obj;
        }
        public List<vPadronPredio> ObtienePadronByRangoFechaAlta(DateTime inicio, DateTime fin)
        {
            List<vPadronPredio> detalle = null;

            try
            {
                detalle = Predial.vPadronPredio.Where(r => r.FechaAlta >= inicio && r.FechaAlta <= fin).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtienePadronByRangoFechaAlta.Exception", ex, "--Parámetros inicio:" + inicio.ToString() + ", fin:" + fin.ToString());
            }
            return detalle;

        }

        public List<vPrediosRezago> ObtienePrediosEnRezago()
        {
            List<vPrediosRezago> rezago = null;
            try
            {
                rezago = Predial.vPrediosRezago.ToList();

            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtienePrediosEnRezago.Exception", ex, "--Parámetros mes:");
            }
            return rezago;

        }

        public List<vAntecedentePredio> ObtieneAntecedentePredio(string clave, int IdContribuyente, bool fechaTramite, DateTime inicio, DateTime fin)
        {
            List<vAntecedentePredio> detalle = null;

            try
            {
                if (clave != string.Empty)
                {
                    if (fechaTramite)
                        detalle = Predial.vAntecedentePredio.Where(r => r.ClavePredial == clave && r.FechaCompleta >= inicio && r.FechaCompleta <= fin).ToList();
                    else
                        detalle = Predial.vAntecedentePredio.Where(r => r.ClavePredial == clave && r.FechaPagoCompleta >= inicio && r.FechaPagoCompleta <= fin).ToList();
                }
                else
                {
                    if (fechaTramite)
                        detalle = Predial.vAntecedentePredio.Where(r => r.IdContribuyente == IdContribuyente && r.FechaCompleta >= inicio && r.FechaCompleta <= fin).ToList();
                    else
                        detalle = Predial.vAntecedentePredio.Where(r => r.IdContribuyente == IdContribuyente && r.FechaPagoCompleta >= inicio && r.FechaPagoCompleta <= fin).ToList();
                }


            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtieneAntecedentePredio.Exception", ex, "--Parámetros inicio:" + inicio.ToString() + ", fin:" + fin.ToString());
            }
            return detalle;

        }

        public List<vAntecedentePredio> ObtieneAntecedentePredio(string clave)
        {
            List<vAntecedentePredio> detalle = null;

            try
            {
                detalle = Predial.vAntecedentePredio.Where(r => r.ClavePredial == clave).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtieneAntecedentePredio.Exception", ex, "--Parámetros clave:" + clave);
            }
            return detalle;

        }

        public List<vBuscarPredioContDom> GetFiltervBuscarPredioContDom(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<vBuscarPredioContDom> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vBuscarPredioContDom.SqlQuery("SELECT IdPredio, ClavePredial, IdContribuyente, NombreCompleto, IdColonia, Domicilio FROM vBuscarPredioContDom order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.vBuscarPredioContDom.SqlQuery("SELECT IdPredio, ClavePredial, IdContribuyente, NombreCompleto, IdColonia, Domicilio FROM vBuscarPredioContDom order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vBuscarPredioContDom.SqlQuery("SELECT IdPredio, ClavePredial, IdContribuyente, NombreCompleto, IdColonia, Domicilio FROM vBuscarPredioContDom where " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.vBuscarPredioContDom.SqlQuery("SELECT IdPredio, ClavePredial, IdContribuyente, NombreCompleto, IdColonia, Domicilio FROM vBuscarPredioContDom where " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.vBuscarPredioContDom.Exception", ex,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }

        public DataTable ObtienePadronRezago(string[] strParam)
        {
            #region  Parametros y DataTable
            /// Obtiene la lista de claves a las cuales se le generará su estado de cuenta
            /// </summary>
            /// /// <param name="strParam">
            /// 0-Año Periodo, 
            /// 1-Bimestre Periodo, 
            /// 2-Ignorar Baldíos
            /// 3-Solo Exento
            /// 4-Ignorar Exento
            /// 5-Baldios
            /// 6-Rango Incial, 
            /// 7-Rango Final
            /// 8-IdContribuyente
            /// 9-Claves listado 
            /// 10-IdFase
            /// 11-IdCondominio
            /// 12-Idcolonia</param>
            /// <returns></returns>
            List<vPrediosRezago> rezago = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("IdPredio");
            dt.Columns.Add("ClavePredial");
            dt.Columns.Add("Domicilio");
            dt.Columns.Add("Contribuyente");
            dt.Columns.Add("Periodo");
            dt.Columns.Add("SuperTerreno");
            dt.Columns.Add("ValorTerreno");
            dt.Columns.Add("SuperContruccion");
            dt.Columns.Add("ValorConstruccion");
            dt.Columns.Add("BaseGravable");
            dt.Columns.Add("BimestreFinIp");
            dt.Columns.Add("AaFinalIp");
            dt.Columns.Add("IdCondominio");
            dt.Columns.Add("Condominio");
            dt.Columns.Add("Zona");
            dt.Columns.Add("MetrosFrente");
            dt.Columns.Add("ExentoPago");
            dt.Columns.Add("IdTipoPredio");
            dt.Columns.Add("TipoPredio");
            dt.Columns.Add("BimestreFinSm");
            dt.Columns.Add("AaFinalSm");
            dt.Columns.Add("IdDiferencia");
            dt.Columns.Add("ValorCatastral");
            #endregion 

            try
            {
                int anio = DateTime.Today.Year;
                Int32 bimestre = 0;

                bimestre = Convert.ToInt32(strParam[0]) * 10 + Convert.ToInt32(strParam[1]); //0-Año , 1-Bimestre,
                rezago = Predial.vPrediosRezago.ToList();
                //rezago = rezago.Where(r => r.Periodo < bimestre).ToList();

                if (strParam[6].ToString() != "" || strParam[7].ToString() != "") //6-Rango Incial, /// 7-Rango Final
                {
                    Int64 rangoini = Convert.ToInt64(strParam[6].ToString());
                    Int64 rangofin = Convert.ToInt64(strParam[7].ToString());
                    rezago = rezago.Where(r => r.Periodo < bimestre && r.ClaveNumeric >= rangoini && r.ClaveNumeric <= rangofin).ToList();
                }
                if (strParam[9].ToString().ToUpper() != "") //9-Claves listado
                {
                    String[] claves = strParam[9].Split(',');
                    rezago = rezago.Where(r => r.Periodo < bimestre).ToList();
                    rezago = rezago.Where(p => claves.Contains(p.ClavePredial.Replace(" ", ""))).ToList();
                }

                if (strParam[2].ToString().ToUpper() != "FALSE") //2-Baldios ignorar
                    rezago = rezago.Where(r => r.TipoPredio != "BALDIO").ToList();
                if (strParam[3].ToString().ToUpper() != "FALSE") //3-igual a exento
                    rezago = rezago.Where(r => r.ExentoPago == "EXENTO").ToList();
                if (strParam[4].ToString().ToUpper() != "FALSE") //4-diferente de exento
                    rezago = rezago.Where(p => p.ExentoPago != "EXENTO").ToList();
                if (strParam[5].ToString().ToUpper() != "FALSE") //5-igual a baldio
                    rezago = rezago.Where(p => p.TipoPredio == "BALDIO").ToList();
                if (strParam[8].ToString().ToUpper() != "")
                {
                    Int32 idContribuyente = Convert.ToInt32(strParam[8].ToString());
                    rezago = rezago.Where(p => p.IdContribuyente == idContribuyente).ToList();
                }
                if (strParam[10].ToString() != "0") //10-fase
                {
                    Int32 idDocumento = Convert.ToInt32(strParam[10].ToString());
                    rezago = rezago.Where(p => p.IdTipoFaseIp == idDocumento).ToList();
                }
                if (strParam[11].ToString() != "")//11-idCondominio
                {
                    Int32 idCondominio = Convert.ToInt32(strParam[11].ToString());
                    rezago = rezago.Where(p => p.IdCondominio == idCondominio).ToList();
                }
                if (strParam[12].ToString() != "")//12-idColonia
                {
                    Int32 idColonia = Convert.ToInt32(strParam[12].ToString());
                    rezago = rezago.Where(p => p.IdColonia == idColonia).ToList();
                }
                foreach (vPrediosRezago predio in rezago)
                {
                    DataRow row = dt.NewRow();
                    row["IdPredio"] = predio.Id.ToString();
                    row["ClavePredial"] = predio.ClavePredial;
                    row["Domicilio"] = predio.Domicilio;
                    row["Contribuyente"] = predio.Contribuyente;
                    row["SuperTerreno"] = predio.SuperficieTerreno;
                    row["ValorTerreno"] = predio.ValorTerreno;
                    row["SuperContruccion"] = predio.SuperficieConstruccion;
                    row["ValorConstruccion"] = predio.ValorConstruccion;
                    row["BaseGravable"] = predio.ValorCatastral;
                    row["BimestreFinIp"] = predio.BimestreFinIp;
                    row["AaFinalIp"] = predio.AaFinalIp;
                    row["IdCondominio"] = predio.IdCondominio;
                    row["Condominio"] = predio.Condominio;
                    row["Zona"] = predio.Zona;
                    row["MetrosFrente"] = predio.MetrosFrente;
                    row["ExentoPago"] = predio.Zona;
                    row["IdTipoPredio"] = predio.IdTipoPredio;
                    row["TipoPredio"] = predio.TipoPredio;
                    row["BimestreFinSm"] = predio.BimestreFinSm;
                    row["AaFinalSm"] = predio.AaFinalSm;
                    row["IdDiferencia"] = predio.IdDiferencia;
                    row["Periodo"] = predio.Periodo;
                    dt.Rows.Add(row);
                }
                if (dt.Rows.Count == 0)
                {
                    DataRow row = dt.NewRow();
                    row["IdPredio"] = "";
                    row["ClavePredial"] = "";
                    row["Domicilio"] = "";
                    row["Contribuyente"] = "";
                    row["Periodo"] = "";
                    row["SuperTerreno"] = "";
                    row["ValorTerreno"] = "";
                    row["SuperContruccion"] = "";
                    row["ValorConstruccion"] = "";
                    row["BaseGravable"] = "";
                    row["BimestreFinIp"] = "";
                    row["AaFinalIp"] = "";
                    row["Condominio"] = "";
                    row["Zona"] = "";
                    row["MetrosFrente"] = "";
                    row["ExentoPago"] = "";
                    row["IdTipoPredio"] = "";
                    row["TipoPredio"] = "";
                    row["BimestreFinSm"] = "";
                    row["AaFinalSm"] = "";
                    row["IdDiferencia"] = "";
                    dt.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.vPrediosRezago.Exception", ex, "--Parámetros status:");
            }
            return dt;
        }

        public List<vReciboReporteDet> ObtieneReciboReporteDet(int idCajero, string estado, int IdTipoTramite, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<vReciboReporteDet> listRecibo = null;

            try
            {

                if (estado == "P")
                {
                    if (IdTipoTramite == 0)
                    {
                        if (idCajero == 0)
                        {
                            listRecibo = Predial.vReciboReporteDet.Where(v => v.EstadoRecibo == estado && v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                        }
                        else
                        {
                            listRecibo = Predial.vReciboReporteDet.Where(v => v.IdUsuarioCobra == idCajero && v.EstadoRecibo == estado && v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                        }
                    }
                    else
                    {
                        if (idCajero == 0)
                        {
                            listRecibo = Predial.vReciboReporteDet.Where(v => v.EstadoRecibo == estado && v.IdTipoTramite == IdTipoTramite && v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                        }
                        else
                        {
                            listRecibo = Predial.vReciboReporteDet.Where(v => v.IdUsuarioCobra == idCajero && v.EstadoRecibo == estado && v.IdTipoTramite == IdTipoTramite && v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                        }
                    }
                }
                else if (estado == "C")
                {
                    if (IdTipoTramite == 0)
                    {
                        if (idCajero == 0)
                        {
                            listRecibo = Predial.vReciboReporteDet.Where(v => v.EstadoRecibo == estado && v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                        }
                        else
                        {
                            listRecibo = Predial.vReciboReporteDet.Where(v => v.IdUsuarioCobra == idCajero && v.EstadoRecibo == estado && v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                        }
                    }
                    else
                    {
                        if (idCajero == 0)
                        {
                            listRecibo = Predial.vReciboReporteDet.Where(v => v.EstadoRecibo == estado && v.IdTipoTramite == IdTipoTramite && v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                        }
                        else
                        {
                            listRecibo = Predial.vReciboReporteDet.Where(v => v.IdUsuarioCobra == idCajero && v.EstadoRecibo == estado && v.IdTipoTramite == IdTipoTramite && v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                        }
                    }
                }
                else
                {
                    if (IdTipoTramite == 0)
                    {
                        if (idCajero == 0)
                        {
                            listRecibo = Predial.vReciboReporteDet.Where(v => v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                        }
                        else
                        {
                            listRecibo = Predial.vReciboReporteDet.Where(v => v.IdUsuarioCobra == idCajero && v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                        }
                    }
                    else
                    {
                        if (idCajero == 0)
                        {
                            listRecibo = Predial.vReciboReporteDet.Where(v => v.IdTipoTramite == IdTipoTramite && v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                        }
                        else
                        {
                            listRecibo = Predial.vReciboReporteDet.Where(v => v.IdUsuarioCobra == idCajero && v.IdTipoTramite == IdTipoTramite && v.FechaPago >= fechaInicio && v.FechaPago <= fechaFin).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vRecibo.ObtieneRecibo.Exception", ex, "--Parámetros fechaInicio:" + fechaInicio + ", fechaFin:" + fechaFin);
            }
            return listRecibo;

        }

        public DataTable ObtienePadronRezagoBusq(string[] strParam)
        {
            /// Obtiene la lista de claves a las cuales se le generará su estado de cuenta
            /// </summary>
            /// /// <param name="strParam">
            /// 0-Año Periodo, 
            /// 1-Bimestre Periodo, 
            /// 2-Ignorar Baldíos
            /// 3-Solo Exento
            /// 4-Ignorar Exento
            /// 5-Baldios
            /// 6-Rango Incial, 
            /// 7-Rango Final
            /// 8-IdContribuyente
            /// 9-Claves listado 
            /// 10-IdFase
            /// 11-IdCondominio
            /// 12-Idcolonia</param>
            /// <returns></returns>
            List<vPrediosRezago> rezago = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("ClavePredial");
            dt.Columns.Add("Tipo");
            dt.Columns.Add("Periodo");
            dt.Columns.Add("IdPredio");
            dt.Columns.Add("IdTipoPredio");

            try
            {
                int anio = DateTime.Today.Year;
                Int32 bimestre = 0;

                bimestre = Convert.ToInt32(strParam[0]) * 10 + Convert.ToInt32(strParam[1]);
                rezago = Predial.vPrediosRezago.ToList();
                //rezago = rezago.Where(r => r.Periodo < bimestre).ToList();

                if (strParam[6].ToString() != "" || strParam[7].ToString() != "") //Baldios ignorar
                {
                    Int64 rangoini = Convert.ToInt64(strParam[6].ToString());
                    Int64 rangofin = Convert.ToInt64(strParam[7].ToString());
                    rezago = rezago.Where(r => r.Periodo < bimestre && r.ClaveNumeric >= rangoini && r.ClaveNumeric <= rangofin).ToList();
                }
                if (strParam[9].ToString().ToUpper() != "")
                {
                    String[] claves = strParam[9].Split(',');
                    rezago = rezago.Where(r => r.Periodo < bimestre).ToList();
                    rezago = rezago.Where(p => claves.Contains(p.ClavePredial.Replace(" ", ""))).ToList();
                }

                if (strParam[2].ToString().ToUpper() != "FALSE") //Baldios ignorar
                    rezago = rezago.Where(r => r.TipoPredio != "BALDIO").ToList();
                if (strParam[3].ToString().ToUpper() != "FALSE") //igual a exento
                    rezago = rezago.Where(r => r.ExentoPago == "EXENTO").ToList();
                if (strParam[4].ToString().ToUpper() != "FALSE") //diferente de exento
                    rezago = rezago.Where(p => p.ExentoPago != "EXENTO").ToList();
                if (strParam[5].ToString().ToUpper() != "FALSE") //igual a baldio
                    rezago = rezago.Where(p => p.TipoPredio == "BALDIO").ToList();
                if (strParam[8].ToString().ToUpper() != "")
                {
                    Int32 idContribuyente = Convert.ToInt32(strParam[8].ToString());
                    rezago = rezago.Where(p => p.IdContribuyente == idContribuyente).ToList();
                }
                if (strParam[10].ToString() != "0")
                {
                    Int32 idDocumento = Convert.ToInt32(strParam[10].ToString());
                    rezago = rezago.Where(p => p.IdTipoFaseIp == idDocumento).ToList();
                }
                if (strParam[11].ToString() != "")
                {
                    Int32 idCondominio = Convert.ToInt32(strParam[11].ToString());
                    rezago = rezago.Where(p => p.IdCondominio == idCondominio).ToList();
                }
                if (strParam[12].ToString() != "")
                {
                    Int32 idColonia = Convert.ToInt32(strParam[12].ToString());
                    rezago = rezago.Where(p => p.IdColonia == idColonia).ToList();
                }
                foreach (vPrediosRezago predio in rezago)
                {
                    DataRow row = dt.NewRow();
                    row["IdPredio"] = predio.Id.ToString();
                    row["IdTipoPredio"] = predio.IdTipoPredio.ToString();
                    row["ClavePredial"] = predio.ClavePredial;
                    row["Tipo"] = "Impuesto Predial";
                    row["Periodo"] = predio.AaFinalIp.ToString() == "0" ? predio.AaFinalSm.ToString() + " - " + predio.BimestreFinSm.ToString() : predio.AaFinalIp.ToString() + " - " + predio.BimestreFinIp.ToString();
                    dt.Rows.Add(row);
                }
                if (dt.Rows.Count == 0)
                {
                    DataRow row = dt.NewRow();
                    row["IdPredio"] = "";
                    row["IdTipoPredio"] = "";
                    row["ClavePredial"] = "";
                    row["Tipo"] = "";
                    row["Periodo"] = "";
                    dt.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.vPrediosRezago.Exception", ex, "--Parámetros status:");
            }
            return dt;
        }

        public List<vCajeros> cajeros()
        {
            List<vCajeros> listCajeros = null;

            try
            {
                listCajeros = Predial.vCajeros.ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vRecibo.ObtienePadronRezagoBusq.Exception", ex);
            }
            return listCajeros;

        }

        //public List<vBaseGravable> GetFilterVBaseGravableHist(string valorFiltro, string tipoSort)
        //{
        //    List<vBaseGravableCorto> objList = null;
        //    try
        //    {
        //        objList = Predial.vBaseGravableCorto.SqlQuery("SELECT Nombre,Ejercicio,Bimestre,Valor,Id,ClavePredial,FechaAvaluo,ValorConstruccion,Activo FROM vBaseGravableCorto where  ClavePredial = @p order by Ejercicio " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        new Utileria().logError("vBaseGravableCorto.GetFiltervBaseGravableCorto.Exception", ex ,
        //             "--Parámetros valorFiltro:" + valorFiltro + ", tipoSort:" + tipoSort);
        //    }
        //    return objList;
        //}

        public List<vBaseGravable> GetFilterVBaseGravableHist(string valorFiltro, string tipoSort)
        {
            List<vBaseGravable> objList = null;
            try
            {
                objList = Predial.vBaseGravable.SqlQuery("SELECT Nombre ,Ejercicio ,Bimestre ,Valor ,Id ,IdPredio ,ClavePredial ,SuperficieTerreno ,FechaAvaluo ,ValorTerreno ,ValorConstruccion ,Activo ,IdUsuario ,FechaModificacion FROM vBaseGravable where ClavePredial = @p order by Ejercicio " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vBaseGravable.GetFiltervBaseGravable.Exception", ex,
                     "--Parámetros valorFiltro:" + valorFiltro + ", tipoSort:" + tipoSort);
            }
            return objList;
        }

        public List<vRequerimientos> ObtieneRequerimientos(string estado, int idCondominio, String inicio, String fin)
        {
            List<vRequerimientos> requerimientos = null;
            StringBuilder where = new StringBuilder();
            where.AppendLine("FechaEmision >= '" + inicio + "' and FechaEmision <= '" + fin + "'");
            if (estado != "%")
                where.AppendLine(" and status='" + estado + "'");
            if (idCondominio != 0)
                where.AppendLine(" and idCondominio=" + idCondominio);

            try
            {
                requerimientos = Predial.vRequerimientos.SqlQuery(" SET DATEFORMAT dmy SELECT ClavePredial, FechaEmision, Folio, TipoImpuesto, Descripcion, BimestreInicial, EjercicioInicial, BimestreFinal, EjercicioFinal, Impuesto, Adicional, Recargo, Rezago, Diferencia, RecargoDiferencia, Ejecucion, Multa, Importe, Nombre, ApellidoPaterno, ApellidoMaterno, Recibo, FechaPago, Activo, Status, StatusEnvio, IdCondominio, DescCondominio, IdTipoFase, DescTipoFaceDoc FROM vRequerimientos where " + where + " ;").ToList();
                //requerimientos = Predial.vRequerimientos.Where(r => r.FechaEmision >= inicio && r.FechaEmision <= fin).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtieneRequerimientos.Exception", ex, "--Parámetros inicio:" + inicio.ToString() + ", fin:" + fin.ToString());
            }
            return requerimientos;

        }

        public List<vPermisoBoton> ObtienePermisoPorBoton(int idUsuario)
        {
            List<vPermisoBoton> detalle = null;

            try
            {
                detalle = Predial.vPermisoBoton.Where(r => r.id == idUsuario && r.url != null).OrderBy(o => o.ClaveBoton).ToList();

            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtienePermisoPorBoton.Exception", ex, "--Parámetros idUsuario:" + idUsuario);
            }
            return detalle;

        }

        public List<vRequerimientosCompleto> ObtieneRequerimientosCompleto(string estado, int idCondominio, String inicio, String fin, int idAgente)
        {
            List<vRequerimientosCompleto> requerimientos = null;
            StringBuilder where = new StringBuilder();
            where.AppendLine("FechaEmision >= '" + inicio + "' and FechaEmision <= '" + fin + "'");
            if (estado != "%")
                where.AppendLine(" and EstadoRecibo='" + estado + "'");
            if (idCondominio != 0)
                where.AppendLine(" and idCondominio=" + idCondominio);
            if (idAgente != 0)
                where.AppendLine(" and IdAgenteFiscal =" + idAgente);

            try
            {
                requerimientos = Predial.vRequerimientosCompleto.SqlQuery(" SET DATEFORMAT dmy SELECT ClavePredial,ClaveNumerica,Contribuyente,DireccionPredio,Asentamiento,Localidad,Fraccionamiento,TipoPredio,FechaAvaluo,SuperficieTerreno,ValorTerreno,SuperficieConstruccion,ValorConstruccion,BaseGravable,Id,ExentoPago,FolioConvenio,Fase,FechaEmision,EstadoRequerimiento,TipoFase,FechaLimite,EstadoEnvio,IdRequerimiento,Folio,TipoImpuesto,Periodo,IdDiferencias,Impuesto,Adicional,Recargo,Rezago,Diferencia,RecargoDiferencia,Recoleccion,Limpieza,Dap,Honorarios,Ejecucion,Multa,Importe,EstadoRecibo,Recibo,ImportePagado,FechaPago,FechaCancelacion,Activo,Documento,IdTipoFaseDoc,IdAgenteFiscal,FechaRecepcion,CaracterFirmante,AgenteFiscal,Oficio,FechaOficio,Observaciones FROM vRequerimientosCompleto where " + where + " ;").ToList();
                //requerimientos = Predial.vRequerimientos.Where(r => r.FechaEmision >= inicio && r.FechaEmision <= fin).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtieneRequerimientosCompleto.Exception", ex, "--Parámetros inicio:" + inicio.ToString() + ", fin:" + fin.ToString());
            }
            return requerimientos;

        }

        public List<vRequerimientosCompleto> ObtieneRequerimientosCompletoFecha(string estado, int idCondominio, String inicio, String fin, int idAgente, string tipo)
        {
            List<vRequerimientosCompleto> requerimientos = null;
            StringBuilder where = new StringBuilder();
            if ( tipo == "Emision")
                where.AppendLine("FechaEmision >= '" + inicio + "' and FechaEmision <= '" + fin + "'");
            else
                where.AppendLine("FechaPago >= '" + inicio + "' and FechaPago <= '" + fin + "'");

            if (estado != "%")
                where.AppendLine(" and EstadoRecibo='" + estado + "'");
            if (idCondominio != 0)
                where.AppendLine(" and idCondominio=" + idCondominio);
            if (idAgente != 0)
                where.AppendLine(" and IdAgenteFiscal =" + idAgente);

            try
            {
                requerimientos = Predial.vRequerimientosCompleto.SqlQuery(" SET DATEFORMAT dmy SELECT ClavePredial,ClaveNumerica,Contribuyente,DireccionPredio,Asentamiento,Localidad,Fraccionamiento,TipoPredio,FechaAvaluo,SuperficieTerreno,ValorTerreno,SuperficieConstruccion,ValorConstruccion,BaseGravable,Id,ExentoPago,FolioConvenio,Fase,FechaEmision,EstadoRequerimiento,TipoFase,FechaLimite,EstadoEnvio,IdRequerimiento,Folio,TipoImpuesto,Periodo,IdDiferencias,Impuesto,Adicional,Recargo,Rezago,Diferencia,RecargoDiferencia,Recoleccion,Limpieza,Dap,Honorarios,Ejecucion,Multa,Importe,EstadoRecibo,Recibo,ImportePagado,FechaPago,FechaCancelacion,Activo,Documento,IdTipoFaseDoc,IdAgenteFiscal,FechaRecepcion,CaracterFirmante,AgenteFiscal,Oficio,FechaOficio,Observaciones FROM vRequerimientosCompleto where " + where + " ;").ToList();
                //requerimientos = Predial.vRequerimientos.Where(r => r.FechaEmision >= inicio && r.FechaEmision <= fin).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.ObtieneRequerimientosCompleto.Exception", ex, "--Parámetros inicio:" + inicio.ToString() + ", fin:" + fin.ToString());
            }
            return requerimientos;

        }

        public List<vPadronPredio> CalculaEstadistica(int status, int anio, int bimestre, int tipo, string clave, string listaclaves, string hdfIdCon, string hdfIdCondominio, string hdfIdColonia, string claveIni, string claveFin)
        {
            List<vPadronPredio> detalle = null;

            try
            {
                detalle = Predial.vPadronPredio.ToList();
                if (status != 0)
                {
                    detalle = detalle.Where(r => r.IdStatusPredio == status).ToList();
                }
                if (anio != 0)
                {
                    detalle = detalle.Where(r => r.AaFinalIp == anio).ToList();
                }
                if (bimestre != 0)
                {
                    detalle = detalle.Where(r => r.BimestreFinIp == bimestre).ToList();
                }
                if (tipo != 0)
                {
                    detalle = detalle.Where(r => r.IdTipoPredio == tipo).ToList();
                }
                if (clave != "")
                {
                    detalle = detalle.Where(r => r.ClavePredial.Contains(clave.Replace(" ", ""))).ToList();
                }
                if (listaclaves != "")
                {
                    String[] claves = listaclaves.Split(',');
                    detalle = detalle.Where(p => claves.Contains(p.ClavePredial.Replace(" ", ""))).ToList();
                }
                if (hdfIdCon.ToUpper() != "")
                {
                    detalle = detalle.Where(p => p.Contribuyente.Contains(hdfIdCon)).ToList();
                }
                if (hdfIdCondominio != "")
                {
                    List<cPredio> lPred = new cPredioBL().GetByCondominio(Convert.ToInt32(hdfIdCondominio));
                    string lcl = "";
                    foreach (cPredio p in lPred)
                    {
                        if (lcl == "")
                            lcl = p.ClavePredial;
                        else
                            lcl = lcl + "," + p.ClavePredial;
                    }
                    String[] claves = lcl.Split(',');
                    detalle = detalle.Where(o => claves.Contains(o.ClavePredial.Replace(" ", ""))).ToList();
                }
                if (hdfIdColonia != "")
                {
                    List<cPredio> lPred = new cPredioBL().GetByColonia(Convert.ToInt32(hdfIdColonia));
                    string lcl = "";
                    foreach (cPredio p in lPred)
                    {
                        if (lcl == "")
                            lcl = p.ClavePredial;
                        else
                            lcl = lcl + "," + p.ClavePredial;
                    }
                    String[] claves = lcl.Split(',');
                    detalle = detalle.Where(o => claves.Contains(o.ClavePredial.Replace(" ", ""))).ToList();
                }

                if (claveIni != "" && claveFin != "")
                {

                    Int64 ini, fin;
                    ini = Convert.ToInt64(claveIni);
                    fin = Convert.ToInt64(claveFin);
                    detalle = detalle.Where(r => r.ClaveNumerica >= ini && r.ClaveNumerica <= fin).ToList();
                }


            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.CalculaEstadisticas.Exception", ex,
                     "--Parámetros status:" + status + ", anio:" + anio + ", bimestre:" + bimestre + ", tipo:" + tipo);
            }
            return detalle;

        }

        public List<vTabularIngresos> TabularIngresos(int mesa, int tipoTramite, int status, int tipo, int hdfIdCondominio, string Colonia, DateTime inicio, DateTime fin)
        {
            List<vTabularIngresos> detalle = null;

            try
            {
                detalle = Predial.vTabularIngresos.Where(r => r.FechaPago >= inicio && r.FechaPago <= fin).ToList();

                if (mesa != 0)
                {
                    detalle = detalle.Where(r => r.IdMesa == mesa).ToList();
                }
                if (tipoTramite != 0)
                {
                    detalle = detalle.Where(r => r.IdTipoTramite == tipoTramite).ToList();
                }
                if (status != 0)
                {
                    detalle = detalle.Where(r => r.IdStatusPredio == status).ToList();
                }

                if (tipo != 0)
                {
                    detalle = detalle.Where(r => r.IdTipoPredio == tipo).ToList();
                }

                if (hdfIdCondominio != 0)
                {
                    detalle = detalle.Where(p => p.IdCondominio == hdfIdCondominio).ToList();
                }

                if (Colonia != "")
                {
                    detalle = detalle.Where(r => r.NombreColonia.Contains(Colonia)).ToList();
                }


            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.TabularIngresos.Exception", ex,
                     "--Parámetros Fechas " + inicio.ToString() + " -" + fin.ToString() + " mesa " + mesa + " status:" + status + ", tipotrmite:" + tipoTramite + " tipopredio " + tipo + " Condominio " + hdfIdCondominio + " Colonia " + Colonia);

            }
            return detalle;

        }

        public List<vTabularIngresosGral> TabularIngresosGral(int mesa, int tipoTramite, int status, int tipo, int hdfIdCondominio, string Colonia, DateTime inicio, DateTime fin)
        {
            List<vTabularIngresosGral> detalle = null;

            try
            {
                detalle = Predial.vTabularIngresosGral.Where(r => r.FechaPago >= inicio && r.FechaPago <= fin).ToList();

                if (mesa != 0)
                {
                    detalle = detalle.Where(r => r.IdMesa == mesa).ToList();
                }
                if (tipoTramite != 0)
                {
                    detalle = detalle.Where(r => r.IdTipoTramite == tipoTramite).ToList();
                }
                if (status != 0)
                {
                    detalle = detalle.Where(r => r.IdStatusPredio == status).ToList();
                }

                if (tipo != 0)
                {
                    detalle = detalle.Where(r => r.IdTipoPredio == tipo).ToList();
                }

                if (hdfIdCondominio != 0)
                {
                    detalle = detalle.Where(p => p.IdCondominio == hdfIdCondominio).ToList();
                }

                if (Colonia != "")
                {
                    detalle = detalle.Where(r => r.NombreColonia.Contains(Colonia)).ToList();
                }


            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.TabularIngresosGral.Exception", ex,
                     "--Parámetros Fechas " + inicio.ToString() + " -" + fin.ToString() + " mesa " + mesa + " status:" + status + ", tipotrmite:" + tipoTramite + " tipopredio " + tipo + " Condominio " + hdfIdCondominio + " Colonia " + Colonia);

            }
            return detalle;

        }

        public List<vBitacora> bitacora(int idUsuario, string ventana, string claveClavePredial, DateTime fechaI, DateTime fechaF)
        {
            List<vBitacora> bit = null;
            try
            {

                if (idUsuario == 0 && ventana == string.Empty && claveClavePredial == string.Empty)
                {
                    bit = Predial.vBitacora.Where(b => b.FechaModificacion >= fechaI && b.FechaModificacion <= fechaF).ToList();
                }
                else if (idUsuario != 0 && ventana == string.Empty && claveClavePredial == string.Empty)
                {
                    bit = Predial.vBitacora.Where(b => b.IdUsuario == idUsuario && b.FechaModificacion >= fechaI && b.FechaModificacion <= fechaF).ToList();
                }
                else if (idUsuario != 0 && ventana != string.Empty && claveClavePredial == string.Empty)
                {
                    bit = Predial.vBitacora.Where(b => b.IdUsuario == idUsuario && b.ventana == ventana && b.FechaModificacion >= fechaI && b.FechaModificacion <= fechaF).ToList();
                }
                else if (idUsuario != 0 && ventana != string.Empty && claveClavePredial != string.Empty)
                {
                    bit = Predial.vBitacora.Where(b => b.IdUsuario == idUsuario && b.ventana == ventana && b.ClavePredial == claveClavePredial && b.FechaModificacion >= fechaI && b.FechaModificacion <= fechaF).ToList();
                }
                else if (idUsuario == 0 && ventana != string.Empty && claveClavePredial == string.Empty)
                {
                    bit = Predial.vBitacora.Where(b => b.ventana == ventana && b.FechaModificacion >= fechaI && b.FechaModificacion <= fechaF).ToList();
                }
                else if (idUsuario == 0 && ventana != string.Empty && claveClavePredial != string.Empty)
                {
                    bit = Predial.vBitacora.Where(b => b.ventana == ventana && b.ClavePredial == claveClavePredial && b.FechaModificacion >= fechaI && b.FechaModificacion <= fechaF).ToList();
                }
                else if (idUsuario == 0 && ventana == string.Empty && claveClavePredial != string.Empty)
                {
                    bit = Predial.vBitacora.Where(b => b.ClavePredial == claveClavePredial && b.FechaModificacion >= fechaI && b.FechaModificacion <= fechaF).ToList();
                }
                else if (idUsuario != 0 && ventana == string.Empty && claveClavePredial != string.Empty)
                {
                    bit = Predial.vBitacora.Where(b => b.IdUsuario == idUsuario && b.ClavePredial == claveClavePredial && b.FechaModificacion >= fechaI && b.FechaModificacion <= fechaF).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.bitacora.Exception", ex,
                     "--Parámetros idUsuario " + idUsuario + " claveClavePredial:" + claveClavePredial + ", ventana:" + ventana + " Fechas " + fechaI.ToString() + " - " + fechaF.ToString());
            }
            return bit;
        }

        public List<vVentanaFiltro> ventanaFiltro()
        {
            List<vVentanaFiltro> ventana = null;
            ventana = Predial.vVentanaFiltro.ToList();
            return ventana;
        }

        public List<vAnioPredialAnual> vAnioPredialAnual()
        {
            List<vAnioPredialAnual> anios = null;
            anios = Predial.vAnioPredialAnual.ToList();
            return anios;
        }

        public List<vPredioMovimiento> vPredioMovimiento(DateTime fechaIni, DateTime fechaFin, int tipo, string idCondominio, string idColonia, string claveIni, string claveFin)
        {
            List<vPredioMovimiento> detalle = null;

            try
            {
                detalle = Predial.vPredioMovimiento.Where(r => r.FechaMov >= fechaIni && r.FechaMov <= fechaFin).ToList();
              
                if (tipo != 0)
                {
                    detalle = detalle.Where(r => r.IdTipoPredio == tipo).ToList();
                }
               
                if (idCondominio != "")
                {
                    int idCond = Convert.ToInt32(idCondominio);
                    detalle = detalle.Where(o =>o.IdFraccionamiento == idCond).ToList();
                }
                if (idColonia != "")
                {
                    detalle = detalle.Where(r => r.Domicilio_Predio.Contains(idColonia)).ToList();
                }

                if (claveIni != "" && claveFin != "")
                {
                    Int64 ini, fin;
                    ini = Convert.ToInt64(claveIni);
                    fin = Convert.ToInt64(claveFin);
                    detalle = detalle.Where(r => r.ClaveNumerica >= ini && r.ClaveNumerica <= fin).ToList();
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.vPredioMovimiento.Exception", ex,
                     "--Parámetros tipo:" + tipo + "Rango: " + fechaIni + " - " + fechaFin + " idCond: " + idCondominio + " idCol: " + idColonia + " Claves: " + claveIni + " - " + claveFin);
            }
            return detalle;

        }

        public List<vTabularDetalleConta> TabularDetalleConta(DateTime inicio, DateTime fin)
        {
            List<vTabularDetalleConta> detalle = null;

            try
            {
                detalle = Predial.vTabularDetalleConta.Where(r => r.FechaPago >= inicio && r.FechaPago <= fin).ToList();

                //if (tipoTramite != 0)
                //{
                //    detalle = detalle.Where(r => r.IdTipoTramite == tipoTramite).ToList();
                //}
                ////if (status != 0)
                ////{
                ////    detalle = detalle.Where(r => r.EstatusPredio == status).ToList();
                ////}

                //if (tipo != "")
                //{
                //    detalle = detalle.Where(r => r.TipoPredio == tipo).ToList();
                //}
                //if (Colonia != "")
                //{
                //    detalle = detalle.Where(r => r.NombreColonia.Contains(Colonia)).ToList();
                //}
            }
            catch (Exception ex)
            {
                new Utileria().logError("vVistasBL.TabularIngresos.Exception", ex, "--Parámetros Fechas " + inicio.ToString() + " -" + fin.ToString());
                //"--Parámetros Fechas " + inicio.ToString() + " -" + fin.ToString()  + " status:" + status + ", tipotrmite:" + tipoTramite + " tipopredio " + tipo +  " Colonia " + Colonia);

            }
            return detalle; 

        }

        public List<vReciboMetodoPago> vReciboMetodoPago(DateTime inicio, DateTime fin, string claveMetodo, int idCajero)
        {
            List<vReciboMetodoPago> detalle = new List<vReciboMetodoPago>();
            vReciboMetodoPago det = new vReciboMetodoPago();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class

            try
            {                                
                cmd.Connection = con; //Pass the connection object to Command
                cmd.CommandTimeout = 300;

                string query = "SELECT id, FechaPago,EstadoRecibo, Tipo, Contribuyente, IdCajero, Cajero, CveMetodoPagoFinal, MetodoPagoFinal, ImporteMetodo  FROM vReciboMetodoPago where EstadoRecibo = 'PAGADO' and " +
                               " FechaPago >= @inicio  and fechaPago <= @final and idcajero = @idcajero ";
      
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@inicio", inicio);
                cmd.Parameters.AddWithValue("@final", fin);
                cmd.Parameters.AddWithValue("@idcajero", idCajero);
                SqlDataReader rdr = cmd.ExecuteReader(); 
                
                while (rdr.Read())
                {
                    det = new vReciboMetodoPago();
                    det.Id = Convert.ToInt32(rdr["Id"].ToString());   
                    det.FechaPago =  Convert.ToDateTime( rdr["FechaPago"].ToString());
                    det.EstadoRecibo = rdr["EstadoRecibo"].ToString();
                    det.Tipo =  rdr["Tipo"].ToString();
                    det.MetodoPagoFinal = rdr["MetodoPagoFinal"].ToString();
                    det.ImporteMetodo = Convert.ToDecimal(rdr["ImporteMetodo"].ToString());
                    det.CveMetodoPagoFinal = rdr["CveMetodoPagoFinal"].ToString();
                    detalle.Add(det);
                }
                cmd.Parameters.Clear();
                cmd.Dispose();
                con.Close();

            } 
            catch (Exception ex)
            {
                cmd.Dispose();
                new Utileria().logError("vVistasBL.vReciboMetodoPago.Exception", ex, "--Parámetros Fechas " + inicio.ToString() + " -" + fin.ToString());
                //"--Parámetros Fechas " + inicio.ToString() + " -" + fin.ToString()  + " status:" + status + ", tipotrmite:" + tipoTramite + " tipopredio " + tipo +  " Colonia " + Colonia);
                
            }
            
            return detalle; 

        }




    }
}
