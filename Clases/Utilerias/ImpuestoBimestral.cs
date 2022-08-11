using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases.BL;

namespace Clases.Utilerias{
        [Serializable]
    
    public class ImpuestoBimestral
    {

        //public ImpuestoBimestral(int ejercicio, int bimestre, decimal impuesto, decimal impuestoINP, decimal porcentajeINP, decimal recargo, decimal porcentajeRecargos, decimal adicional, int bimestresCobrar)
        //    {
        //        this.Ejercicio =ejercicio;
        //        this.Bimestre = bimestre;
        //        this.Impuesto = impuesto;
        //        this.ImpuestoINP = impuestoINP;
        //        this.PorcentajeINP = porcentajeINP;
        //        this.Recargo = recargo;                
        //        this.PorcentajeRecargos = porcentajeRecargos;
        //        this.Adicional = adicional;
        //        this.BimestresCobrar = bimestresCobrar;
        //    }
        public int Ejercicio { get; set; }
        public int Bimestre { get; set; }
        public decimal BaseImpuesto { get; set; }
        public decimal BaseGravable {get; set; }
        public decimal Uma { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Adicional { get; set; }
        public decimal IndActual {get;set;}
        public decimal IndAnterior {get;set;}
        public decimal PorcentajeINP { get; set; }  
        public decimal ImpuestoINP { get; set; }             
        public decimal AdicionalINP { get; set; }
        public decimal Recargo { get; set; }
        public decimal PorcentajeRecargo { get; set; }           
            
        public string TextError { get; set; }  

        public MensajesInterfaz mensaje;
                                     

    }
}
