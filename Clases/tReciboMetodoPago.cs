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
    
    public partial class tReciboMetodoPago
    {
        public int Id { get; set; }
        public int IdRecibo { get; set; }
        public string Banco { get; set; }
        public string NumeroCheque { get; set; }
        public string Cuenta { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public int IdTipopago { get; set; }
        public bool Activo { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    
        public virtual cTipoPago cTipoPago { get; set; }
        public virtual cUsuarios cUsuarios { get; set; }
        public virtual tRecibo tRecibo { get; set; }
    }
}