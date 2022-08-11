using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases.Utilerias
{
[Serializable]
    public class Descuento
    {
        public int id { get; set; }
        public string Clave { get; set; }
        public int Ejercicio { get; set; }        
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal AnticipadoImpuesto { get; set; }
        public decimal AnticipadoInfraestructura { get; set; }
        public decimal Infraestructura { get; set; }
        public decimal AnticipadoAdicional { get; set; }
        public decimal AnticipadoLimpieza { get; set; }
        public decimal AnticipadoRecoleccion { get; set; }
        public decimal AnticipadoDap { get; set; }
        public decimal ActualImpuesto { get; set; }
        public decimal ActualAdicional { get; set; }
        public decimal ActualRecargo { get; set; }
        public decimal ActualLImpieza { get; set; }
        public decimal ActualRecoleccion { get; set; }
        public decimal ActualDap { get; set; }
        public decimal Diferencia { get; set; }
        public decimal DiferenciaRecargo { get; set; }
        public decimal Rezago { get; set; }
        public decimal RezagoRecargo { get; set; }
        public decimal RezagoAdicional { get; set; }
        public decimal Basegravable { get; set; }
        public decimal Importe { get; set; }
        public decimal Multas { get; set; }
        public decimal Honorarios { get; set; }
        public decimal Ejecucion { get; set; }
        

    }
}
