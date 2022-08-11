using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases.Utilerias
{
    [Serializable]     
    class ListaBase
    {           
        public ListaBase(string clave,string valor)
        {
                this.clave = clave;
                this.valor = valor;              
        }
            /// Obtiene Clave, Valor 
            /// </summary>
            /// Clase para sacar un listado base de cualquier tabla
            public string clave { get; set; }
            public string valor { get; set; }
           

        


    }
}
