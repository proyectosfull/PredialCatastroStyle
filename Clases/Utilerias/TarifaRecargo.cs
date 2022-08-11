using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Clases.Utilerias
{   
    
    public class TarifaRecargo
    {        
        public TarifaRecargo(int aa, int bim, decimal porcentaje, int periodo)
        {
            this.aa = aa;
            this.bim = bim;
            this.porcentaje = porcentaje;
            this.periodo = periodo;
        }
        
        public int  aa { get; set; }
        public int bim { get; set; }
        public decimal porcentaje { get; set; }
        public int periodo { get; set; }
    }
}