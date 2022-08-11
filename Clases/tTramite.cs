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
    
    public partial class tTramite
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tTramite()
        {
            this.tPrediosDescuento = new HashSet<tPrediosDescuento>();
            this.tRecibo = new HashSet<tRecibo>();
            this.tTramiteDetalle = new HashSet<tTramiteDetalle>();
        }
    
        public int Id { get; set; }
        public Nullable<int> IdPredio { get; set; }
        public Nullable<int> IdConvenio { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Status { get; set; }
        public Nullable<double> BaseGravable { get; set; }
        public Nullable<int> BimestreInicial { get; set; }
        public Nullable<int> EjercicioInicial { get; set; }
        public Nullable<int> BimestreFinal { get; set; }
        public Nullable<int> EjercicioFinal { get; set; }
        public string Tipo { get; set; }
        public Nullable<decimal> Adeudo { get; set; }
        public string Periodo { get; set; }
        public Nullable<double> SuperficieTerreno { get; set; }
        public Nullable<double> TerrenoPrivativo { get; set; }
        public Nullable<double> TerrenoComun { get; set; }
        public Nullable<double> SuperficieConstruccion { get; set; }
        public Nullable<double> ConstruccionPrivativa { get; set; }
        public Nullable<double> ConstruccionComun { get; set; }
        public Nullable<System.DateTime> FechaEstimado { get; set; }
        public string NombreAdquiriente { get; set; }
        public Nullable<int> IdTipoAvaluo { get; set; }
        public Nullable<System.DateTime> FechaOperacion { get; set; }
        public string NumeroEscritura { get; set; }
        public Nullable<double> ValorMasAlto { get; set; }
        public Nullable<bool> PorMinimos { get; set; }
        public Nullable<int> NoSalarioMinimo { get; set; }
        public Nullable<int> IdValuador { get; set; }
        public string Notaria { get; set; }
        public int IdTipoTramite { get; set; }
        public string Observacion { get; set; }
        public Nullable<int> IdDiferencia { get; set; }
        public Nullable<int> IdRequerimiento { get; set; }
        public Nullable<int> NoParcialidad { get; set; }
        public Nullable<decimal> Mensualidad { get; set; }
        public Nullable<decimal> Interes { get; set; }
        public bool IsabiForaneo { get; set; }
        public Nullable<double> valorComercial { get; set; }
        public Nullable<double> valorOperacion { get; set; }
        public bool Activo { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaModificacion { get; set; }
        public int IdTramiteRef { get; set; }
        public int IdRfcIsabis { get; set; }
    
        public virtual cDiferencia cDiferencia { get; set; }
        public virtual cPredio cPredio { get; set; }
        public virtual cTipoAvaluo cTipoAvaluo { get; set; }
        public virtual cTipoTramite cTipoTramite { get; set; }
        public virtual cUsuarios cUsuarios { get; set; }
        public virtual cValuador cValuador { get; set; }
        public virtual tConvenio tConvenio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tPrediosDescuento> tPrediosDescuento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tRecibo> tRecibo { get; set; }
        public virtual tRequerimiento tRequerimiento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tTramiteDetalle> tTramiteDetalle { get; set; }
    }
}
