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
    
    public partial class tConfiguracionMesa
    {
        public int Id { get; set; }
        public int IdMesa { get; set; }
        public int IdCajero { get; set; }
        public int IdCaja { get; set; }
        public string Turno { get; set; }
        public string Lugar { get; set; }
        public string Maquina { get; set; }
        public Nullable<System.DateTime> FechaApertura { get; set; }
        public Nullable<System.DateTime> FechaCierre { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<int> IdUsuario1 { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    
        public virtual cCaja cCaja { get; set; }
        public virtual cMesa cMesa { get; set; }
        public virtual cUsuarios cUsuarios { get; set; }
        public virtual cUsuarios cUsuarios1 { get; set; }
    }
}