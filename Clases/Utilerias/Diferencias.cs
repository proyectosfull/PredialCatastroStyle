using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace Clases.Utilerias
{
    [Serializable]
    public class Diferencias
    {     
        /// importes y descuentos
        /// </summary>
        /// 
        public string Tipo { get; set; }
        public int Ejercicio { get; set; }
        public int Bimestre { get; set; }
        public decimal Diferencia { get; set; }
        public decimal Recargo { get; set; }
        public decimal PorcentajeRecargo { get; set; }
        public decimal ActualizacionINP { get; set; }
        public decimal PorcentajeINP { get; set; }
        public decimal Adicional { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public string Periodo { get; set; }
        public MensajesInterfaz mensaje { get; set; }

    }
}