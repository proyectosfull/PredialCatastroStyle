using Clases;
using Clases.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Catastro
{
    /// <summary>
    /// Summary description for Autocomplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Autocomplete : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] GetCompletionList(string prefixText, int count)
        {
            if (count < 5)
            {
                return new string[0];
            }

            List<cColonia> lista = new cColoniaBL().GetAutoCompleteByName(prefixText);
            var items = new List<string>(lista.Count());
            foreach (cColonia c  in lista)
            {
                items.Add(c.NombreColonia);
            }
            return items.ToArray();
        }
        [WebMethod]
        public string[] GetCompletionListCon(string prefixText, int count)
        {
            if (count < 5)
            {
                return new string[0];
            }

            List<cContribuyente> lista = new cContribuyenteBL().GetAutoCompleteByName(prefixText);
            if (lista == null)
                return new string[0];
            if (lista.Count()> 100)
                return new string[0];
            var items = new List<string>(lista.Count());
            foreach (cContribuyente c in lista)
            {
                items.Add( c.ApellidoPaterno + " " + c.ApellidoMaterno + " " + c.Nombre.Trim() );
            }
            return items.ToArray();
        }

        [WebMethod]
        public string[] GetCompletionListProdServ(string prefixText, int count)
        {
            if (count < 3)
            {
                return new string[0];
            }

            List<cProdServ> lista = new cProdServBL().GetAutoCompleteByName(prefixText);
            var items = new List<string>(lista.Count());          
            foreach (cProdServ c in lista)
            {
                string str = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.ClaveProdServ + " " + c.Descripcion, Convert.ToString(c.Id));
                items.Add(str);
                //items.Add(c.ClaveProdServ + " " + c.Descripcion);
            }
            return items.ToArray();
        }

        [WebMethod]
        public string[] GetCompletionListUnidadMedida(string prefixText, int count)
        {
            if (count < 3)
            {
                return new string[0];
            }

            List<cUnidadMedida> lista = new cUnidadMedidaBL().GetAutoCompleteByName(prefixText);
            var items = new List<string>(lista.Count());
            foreach (cUnidadMedida c in lista)
            {
                string str = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.ClaveUnidad + " " + c.Nombre, Convert.ToString(c.Id));
                items.Add(str);
                //items.Add(c.ClaveUnidad + " " + c.Nombre);
            }
            return items.ToArray();
        }
    }
}
