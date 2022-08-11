using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases.Models
{ 
    [Serializable]
    public class ContratoAgua 
    {
        public int Id { get; set; }
        public int IdPredio { get; set; }
        public string NoContrato { get; set; }
        public bool Activo { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    }
}
