//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Clases
{
    using System;
    using System.Collections.Generic;
    
    public partial class tCoPropietario
    {
        public int Id { get; set; }
        public Nullable<int> IdPredio { get; set; }
        public Nullable<int> IdContribuyente { get; set; }
        public Nullable<decimal> Porcentaje { get; set; }
        public Nullable<System.DateTime> FechaInicial { get; set; }
        public bool Titular { get; set; }
        public Nullable<bool> Activo { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    
        public virtual cUsuarios cUsuarios { get; set; }
    }
}
