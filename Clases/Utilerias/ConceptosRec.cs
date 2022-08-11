using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases.Utilerias
{

    public class ConceptosRec
    {
        public double Cantidad { set; get; }
        public string CuentaPredial { set; get; }
        public string Descripcion { set; get; } //Del concepto
        public string Id { set; get; } //Cri 1.2.3.1.1
        public double Importe { set; get; }
        public string Unidad { set; get; }  //N/A      
        public double ValorUnitario { set; get; }
        public int IdCri { set; get; }
        public double Descuento { set; get; }
        public double Adicional { set; get; }
        public int PorcentajeDescto { set; get; }
        public int IdConceptoRef { set; get; }
        public int IdControlSistema { set; get; }
        public string ClaveProdServ { set; get; }
        public string ClaveUnidadMedida { set; get; }
    }
}


