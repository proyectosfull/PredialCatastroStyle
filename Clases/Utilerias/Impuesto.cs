using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Clases.Utilerias
{
    [Serializable]
    public class Impuesto
    {
        //Anticipado            
        public Periodo PerAnticipado { get; set; }
        public decimal AntImpuesto { get; set; }
        public decimal AntImpuestoPri { get; set; }
        public decimal AntImpuestoExc { get; set; }
        public decimal AntDescImpuesto { get; set; }
        public decimal AntAdicional { get; set; }
        public decimal AntDescAdicional { get; set; }
        public decimal AntBimestre { get; set; }

        //Actual             
        public Periodo PerActual { get; set; }
        public decimal ActBimestre { get; set; }
        public decimal ActBimestreRez { get; set; }
        public decimal ActImpuesto { get; set; }
        public decimal ActImpuestoPri { get; set; }
        public decimal ActImpuestoExc { get; set; }
        public decimal ActDescImpuesto { get; set; }
        public decimal ActRezago { get; set; }
        public decimal ActDescRezago { get; set; }
        public decimal ActRecargo { get; set; }
        public decimal ActDescRecargo { get; set; }
        public decimal ActAdicional { get; set; }
        public decimal ActDescAdicional { get; set; }
        public decimal ActDiferencias { get; set; }
        public decimal ActDescDiferencias { get; set; }
        public decimal ActRecDiferencias { get; set; }
        public decimal ActDescRecDiferencias { get; set; }
        public decimal ActAdicDiferencia { get; set; }
        public decimal ActDescAdicDiferencia { get; set; }
        public decimal ActImpuestoINP { get; set; }
        public decimal ActDiferenciasINP { get; set; }

        //rezago         
        public Periodo PerRezago { get; set; }
        public decimal RezBimestre { get; set; }
        public decimal Rezagos { get; set; }
        public decimal RezDescRezagos { get; set; }
        public decimal RezRecargo { get; set; }
        public decimal RezDescRecargos { get; set; }
        public decimal RezAdicional { get; set; }
        public decimal RezDescAdicional { get; set; }
        public decimal RezDiferencias { get; set; }
        public decimal RezDescDiferencias { get; set; }
        public decimal RezRecDiferencias { get; set; }
        public decimal RezDescRecDiferencias { get; set; }
        public decimal RezAdicDiferencias { get; set; }
        public decimal RezDescAdicDiferencias { get; set; }
        public decimal RezagoINP { get; set; }   
        public decimal RezDiferenciasINP { get; set; }

        //Generales
        public decimal Multa { get; set; }
        public decimal MultaDesc { get; set; }
        public decimal Ejecucion { get; set; }
        public decimal EjecucionDesc { get; set; }
        public decimal Honorarios { get; set; }
        public decimal HonorariosDesc { get; set; }
        //sumatorias         
        public decimal DescuentoGral { get; set; }
        public decimal Importe { get; set; }  
              
        public int IdRequerimiento  { get; set; }
        public int IdDiferencia { get; set; }
        public int idDescuentoRez { get; set; }
        public int idDescuentoJYP { get; set; }
        public int idDescuentoGral { get; set; }

        public ImpuestoEdo Estado { get; set; }
        public List<ImpuestoEdo> Anual { get; set; }

        //public Concepto Coneptos { get; set; }
        public string TextError { get; set; }        

        public MensajesInterfaz mensaje { get; set; }

    }

}