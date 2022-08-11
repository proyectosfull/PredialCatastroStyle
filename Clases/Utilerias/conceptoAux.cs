using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clases.Utilerias
{
        [Serializable]
    public class conceptoAux
    {
            
        public Int32 Id { set; get; }
        public String Nombre { set; get; }
        public Int32 Cantidad { set; get; }
        public conceptoAux(Int32 Id, String Nombre,Int32 Cantidad)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Cantidad = Cantidad;
        }
    }
}