using Clases.Utilerias;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace Clases.BL
{
    public class vTramiteBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public vTramiteBL()
        {
            Predial = new PredialEntities();
        }

        public List<vTramite> GetFilterVTramite(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort, int idTipoTramite)
        {
            List<vTramite> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vTramite.SqlQuery("Select ClavePredial,NombreAdquiriente,TipoAvaluo,NombreValuador,NumeroEscritura,FechaOperacion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,Notaria,Id,IdPredrio,Activo,Status,Periodo,Adeudo,IdTipoTramite from vTramite where activo=1 and IdTipoTramite = " + idTipoTramite + " order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.vTramite.SqlQuery("Select ClavePredial,NombreAdquiriente,TipoAvaluo,NombreValuador,NumeroEscritura,FechaOperacion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,Notaria,Id,IdPredrio,Activo,Status,Periodo,Adeudo,IdTipoTramite from vTramite where activo=0 and IdTipoTramite = " + idTipoTramite + " order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.vTramite.SqlQuery("Select ClavePredial,NombreAdquiriente,TipoAvaluo,NombreValuador,NumeroEscritura,FechaOperacion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,Notaria,Id,IdPredrio,Activo,Status,Periodo,Adeudo,IdTipoTramite from vTramite where activo=1 and IdTipoTramite = " + idTipoTramite + " and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.vTramite.SqlQuery("Select ClavePredial,NombreAdquiriente,TipoAvaluo,NombreValuador,NumeroEscritura,FechaOperacion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,Notaria,Id,IdPredrio,Activo,Status,Periodo,Adeudo,IdTipoTramite from vTramite where activo=0 and IdTipoTramite = " + idTipoTramite +  " and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vTramiteBL.GetFilterVTramite.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", activos:" + activos + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }
        public List<string> ListaCamposVTramite()
        {
            List<string> propertyList = new List<string>();
            try
            {
                vTramite pObject = new vTramite();
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
                new Utileria().logError("vTramiteBL.ListaCamposVTramite.Exception", ex);
            }
            return propertyList;
        }

        public List<vTramite> GetFilterVTramiteCertificado(string claveCatastral, int idTipoTramite, string status, string campoSort, string tipoSort)
        {
            List<vTramite> objList = null;
            try
            {
                objList = Predial.vTramite.SqlQuery("Select ClavePredial,NombreAdquiriente,TipoAvaluo,NombreValuador,NumeroEscritura,FechaOperacion,ValorCatastral,ValorComercial,ValorFiscal,ValorOperacion,Notaria,Id,IdPredrio,Activo,Status,Periodo,Adeudo,IdTipoTramite from vTramite where activo=1 and IdTipoTramite = " + idTipoTramite + " and Status = " + status +
                    " and ClavePredial like '" + claveCatastral + "' order by " + campoSort + " " + tipoSort).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("vTramiteBL.GetFilterVTramiteCertificado.Exception", ex ,
                     "--Parámetros claveCatastral:" + claveCatastral + ", idTipoTramite:" + idTipoTramite + ", status:" + status + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }
    }
}
