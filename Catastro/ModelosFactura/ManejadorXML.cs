using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Catastro.ModelosFactura
{
    public class ManejadorXML
    {
        public static string obtenerInnerXML(string nombreNodo, string xmlPath)
        {
            string retoraValor = "";
            XmlReader reader = XmlReader.Create(xmlPath);
            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == nombreNodo))
                {
                    retoraValor = reader.ReadInnerXml();
                }
            }
            return retoraValor;
        }

        public static string obtenerValorXML(string nombreNodo, string valor, string xmlPath)
        {
            string retoraValor = "";
            XmlReader reader = XmlReader.Create(xmlPath);
            while (reader.Read())
            {
                /*Buscamos en el archivo un elemento de tipo nodo que tenga el nombre "tfd:TimbreFiscalDigital" el cual es el
                 complemento del SAT.*/
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == nombreNodo))
                {
                    //Si el nodo contiene los atributos que se requieren los guardamos en una variable para despues imprimirlos
                    if (reader.HasAttributes)
                    {
                        retoraValor = reader.GetAttribute(valor);

                    }
                }
            }
            return retoraValor;
        }


        public static Dictionary<string, string> obtenerFacturaTimbradaXML(string xmlPath)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            XmlReader reader = XmlReader.Create(xmlPath);
            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "tfd:TimbreFiscalDigital"))
                {
                    if (reader.HasAttributes)
                    {
                        dic.Add("UUID", reader.GetAttribute("UUID"));

                    }
                }

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Comprobante"))
                {
                    if (reader.HasAttributes)
                    {
                        dic.Add("NoCertificado", reader.GetAttribute("NoCertificado"));
                    }
                }

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "tfd:TimbreFiscalDigital"))
                {
                    if (reader.HasAttributes)
                    {
                        dic.Add("NoCertificadoSAT", reader.GetAttribute("NoCertificadoSAT"));
                    }
                }

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "tfd:TimbreFiscalDigital"))
                {
                    if (reader.HasAttributes)
                    {
                        dic.Add("FechaTimbrado", reader.GetAttribute("FechaTimbrado"));
                    }
                }

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "tfd:TimbreFiscalDigital"))
                {
                    if (reader.HasAttributes)
                    {
                        dic.Add("SelloCFD", reader.GetAttribute("SelloCFD"));
                    }
                }

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "tfd:TimbreFiscalDigital"))
                {
                    if (reader.HasAttributes)
                    {
                        dic.Add("SelloSAT", reader.GetAttribute("SelloSAT"));
                    }
                }

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Comprobante"))
                {
                    if (reader.HasAttributes)
                    {
                        dic.Add("Sello", reader.GetAttribute("Sello"));
                    }
                }
            }
            return dic;
        }

    }
}
