using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases.Utilerias
{
        [Serializable]
    public class ServicioBimestral
    {
        public int Ejercicio { get; set; }
        public decimal NoBimestre { get; set; }
        public decimal Infraestructura { get; set; }
        public decimal Recoleccion { get; set; }
        public decimal Limpieza { get; set; }
        public decimal Dap { get; set; }
        public decimal Recargo { get; set; }
        public decimal PorcentajeRecargo { get; set; }
        public decimal Adicional { get; set; }
        public decimal IndActual { get; set; }
        public decimal IndAnterior { get; set; }
        public decimal PorcentajeINP { get; set; }  
        public decimal InfraestructuraIPN { get; set; }
        public decimal RecoleccionIPN { get; set; }
        public decimal LimpiezaIPN { get; set; }
        public decimal DapIPN { get; set; }
        public decimal AdicionalIPN { get; set; }
        public decimal InfraestructuraRec { get; set; }
        public decimal RecoleccionRec { get; set; }
        public decimal LimpiezaRec { get; set; }
        public decimal DapRec { get; set; }
        public string TextError { get; set; }
            
        public MensajesInterfaz mensaje;

    }
}
