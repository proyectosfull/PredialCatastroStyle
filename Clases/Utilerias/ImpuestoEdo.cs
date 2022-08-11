using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases.Utilerias
{
        [Serializable]
    public class ImpuestoEdo
    {
        public string PeriodoGral {get; set;}
        public decimal AntImpuesto {get; set;}
        public decimal DescAntImpuesto { get; set; }
        public decimal AntAdicional {get; set;}
        public decimal Impuesto {get; set;}
        public decimal DescImpuesto { get; set; }
        public decimal Diferencias {get; set;}
        public decimal DescDiferencias { get; set; }
        public decimal RecDiferencias {get; set;}
        public decimal DescRecDiferencias { get; set; }
        public decimal Adicional {get; set;}
        public decimal Rezagos {get; set;}
        public decimal DescRezagos { get; set; }
        public decimal Recargos {get; set;}
        public decimal DescRecargos { get; set; }
        public decimal Ejecucion {get; set;}
        public decimal DescEjecucion { get; set; }
        public decimal Honorarios { get; set; }
        public decimal DescHonorarios { get; set; }
        public decimal Multas {get; set;}
        public decimal DescMultas { get; set; }
        public decimal ActINP { get; set; }
        public decimal RezagoINP { get; set; }
        public decimal Descuentos {get; set;}
        public decimal ImporteNeto { get; set; }
        public decimal Importe{get; set;}

    }


}
