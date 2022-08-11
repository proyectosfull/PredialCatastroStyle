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
    public class vBuscarPredioContDomBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public vBuscarPredioContDomBL()
        {
            Predial = new PredialEntities();
        }

        public List<vBuscarPredioContDom> GetFilter(string campoFiltro, string valorFiltro, string campoSort, string tipoSort)
        {
            List<vBuscarPredioContDom> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                        objList = Predial.vBuscarPredioContDom.SqlQuery("Select IdPredio,ClavePredial,IdContribuyente,NombreCompleto,IdColonia,Domicilio from vBuscarPredioContDom order by " + campoSort + " " + tipoSort).ToList();
                }
                else
                {
                    valorFiltro = "%" + valorFiltro + "%";
                    objList = Predial.vBuscarPredioContDom.SqlQuery("Select IdPredio,ClavePredial,IdContribuyente,NombreCompleto,IdColonia,Domicilio from vBuscarPredioContDom where " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("vBuscarPredioContDomBL.GetFilter.Exception", ex ,
                     "--Parámetros campoFiltro:" + campoFiltro + ", valorFiltro:" + valorFiltro + ", campoSort:" + campoSort + ", tipoSort:" + tipoSort);
            }
            return objList;
        }

        public vBuscarPredioContDom GetByConstraint(int id)
        {
            vBuscarPredioContDom obj = null;
            try
            {
                obj = Predial.vBuscarPredioContDom.FirstOrDefault(o => o.IdPredio == id);
            }
            catch (Exception ex)
            {
                new Utileria().logError("vBuscarPredioContDomBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
            }
            return obj;
        }
        public List<string> ListaCampos()
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
                new Utileria().logError("vBuscarPredioContDomBL.ListaCampos.Exception", ex);
            }
           
            return propertyList;
        }

    }
}
