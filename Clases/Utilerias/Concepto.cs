using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases.Utilerias
{
    public class Concepto
    {
        public string Id {get; set;}
        public int Cantidad {get; set;}
        public string Unidad {get; set;}
        public string Descripcion {get; set;}
        public decimal ValorUnitario   {get; set;}
        public decimal Importe  {get; set;}
        public string CuentaPredial { get; set; }
      
    }
}
