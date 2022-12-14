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
    
    public partial class cPredio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cPredio()
        {
            this.cBaseGravable = new HashSet<cBaseGravable>();
            this.cConceptoOmisionPago = new HashSet<cConceptoOmisionPago>();
            this.cContratoAgua = new HashSet<cContratoAgua>();
            this.cDiferencia = new HashSet<cDiferencia>();
            this.cPredioObservacion = new HashSet<cPredioObservacion>();
            this.tConvenioEdoCta = new HashSet<tConvenioEdoCta>();
            this.tDescuentoAsignado = new HashSet<tDescuentoAsignado>();
            this.tInternet = new HashSet<tInternet>();
            this.tPrediosDescuento = new HashSet<tPrediosDescuento>();
            this.tPredioVerificacion = new HashSet<tPredioVerificacion>();
            this.tRequerimiento = new HashSet<tRequerimiento>();
            this.tTramite = new HashSet<tTramite>();
        }
    
        public int Id { get; set; }
        public string ClavePredial { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public int IdColonia { get; set; }
        public string CP { get; set; }
        public string Localidad { get; set; }
        public Nullable<double> SuperficieTerreno { get; set; }
        public Nullable<double> TerrenoPrivativo { get; set; }
        public Nullable<double> TerrenoComun { get; set; }
        public Nullable<double> ValorTerreno { get; set; }
        public Nullable<double> SuperficieConstruccion { get; set; }
        public Nullable<double> ConstruccionPrivativa { get; set; }
        public Nullable<double> ConstruccionComun { get; set; }
        public double ValorConstruccion { get; set; }
        public double ValorCatastral { get; set; }
        public double ValorComercial { get; set; }
        public double ValorFiscal { get; set; }
        public double ValorOperacion { get; set; }
        public System.DateTime FechaAlta { get; set; }
        public double BaseTributacion { get; set; }
        public System.DateTime FechaAvaluo { get; set; }
        public System.DateTime FechaTraslado { get; set; }
        public int Zona { get; set; }
        public double MetrosFrente { get; set; }
        public int IdUsoSuelo { get; set; }
        public Nullable<int> IdExentoPago { get; set; }
        public int IdStatusPredio { get; set; }
        public Nullable<System.DateTime> FechaBaja { get; set; }
        public int IdTipoPredio { get; set; }
        public int IdContribuyente { get; set; }
        public int IdTipoFaseIp { get; set; }
        public Nullable<int> Nivel { get; set; }
        public string UbicacionExpediente { get; set; }
        public string IdCartografia { get; set; }
        public int BimestreFinIp { get; set; }
        public int AaFinalIp { get; set; }
        public Nullable<int> ReciboIp { get; set; }
        public Nullable<int> IdReciboIp { get; set; }
        public Nullable<System.DateTime> FechaPagoIp { get; set; }
        public int BimestreFinSm { get; set; }
        public int AaFinalSm { get; set; }
        public Nullable<int> ReciboSm { get; set; }
        public Nullable<int> IdReciboSm { get; set; }
        public Nullable<System.DateTime> FechaPagoSm { get; set; }
        public int IdTipoFaseSm { get; set; }
        public int IdTipoMovAvaluo { get; set; }
        public Nullable<int> IdTipoInmueble { get; set; }
        public Nullable<int> IdCondominio { get; set; }
        public Nullable<int> IdTipoFaseDoc { get; set; }
        public bool Activo { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaModificacion { get; set; }
        public string ClaveAnterior { get; set; }
        public string Referencia { get; set; }
        public string Observacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cBaseGravable> cBaseGravable { get; set; }
        public virtual cColonia cColonia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cConceptoOmisionPago> cConceptoOmisionPago { get; set; }
        public virtual cCondominio cCondominio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cContratoAgua> cContratoAgua { get; set; }
        public virtual cContribuyente cContribuyente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cDiferencia> cDiferencia { get; set; }
        public virtual cExentoPago cExentoPago { get; set; }
        public virtual cStatusPredio cStatusPredio { get; set; }
        public virtual cTipoFase cTipoFase { get; set; }
        public virtual cTipoFase cTipoFase1 { get; set; }
        public virtual cTipoInmueble cTipoInmueble { get; set; }
        public virtual cTipoMovAvaluo cTipoMovAvaluo { get; set; }
        public virtual cTipoPredio cTipoPredio { get; set; }
        public virtual cUsoSuelo cUsoSuelo { get; set; }
        public virtual cUsuarios cUsuarios { get; set; }
        public virtual tRecibo tRecibo { get; set; }
        public virtual tRecibo tRecibo1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cPredioObservacion> cPredioObservacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tConvenioEdoCta> tConvenioEdoCta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tDescuentoAsignado> tDescuentoAsignado { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tInternet> tInternet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tPrediosDescuento> tPrediosDescuento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tPredioVerificacion> tPredioVerificacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tRequerimiento> tRequerimiento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tTramite> tTramite { get; set; }
    }
}
