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
    
    public partial class cTipoTramite
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cTipoTramite()
        {
            this.tTramite = new HashSet<tTramite>();
        }
    
        public int Id { get; set; }
        public string Tramite { get; set; }
        public Nullable<int> IdMesa { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime Fecha { get; set; }
        public bool ConDescuento { get; set; }
        public bool Activo { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tTramite> tTramite { get; set; }
    }
}
