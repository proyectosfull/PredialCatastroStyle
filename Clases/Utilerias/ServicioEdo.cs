using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases.Utilerias
{
        [Serializable]
    public class ServicioEdo
    {
        public string PeriodoGral { get; set; }
        public decimal AntInfraestructura { get; set; }
        public decimal AntAdicional { get; set; }        
        public decimal Infraestructura { get; set; }
        public decimal Recoleccion { get; set; }
        public decimal Limpieza { get; set; }
        public decimal Dap { get; set; }
        public decimal Recargo { get; set; }
        public decimal Rezagos { get; set; }
        public decimal Adicional { get; set; }
        public decimal Multa { get; set; }        
        public decimal Ejecucion { get; set; }
        public decimal Honorarios { get; set; }
        public decimal Descuentos { get; set; }
        public int Idrequerimiento { get; set; }
        public decimal ActualINP { get; set; }
        public decimal RezagoINP { get; set; }
        public decimal Importe { get; set; }
        

    }
}
