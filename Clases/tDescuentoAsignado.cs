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
    
    public partial class tDescuentoAsignado
    {
        public int Id { get; set; }
        public int IdPredio { get; set; }
        public int IdDescuento { get; set; }
        public System.DateTime Vigencia { get; set; }
        public string Estado { get; set; }
        public bool Activo { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    
        public virtual cDescuento cDescuento { get; set; }
        public virtual cPredio cPredio { get; set; }
        public virtual cUsuarios cUsuarios { get; set; }
    }
}