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
    
    public partial class tPredioVerificacion
    {
        public int Id { get; set; }
        public int IdPredio { get; set; }
        public Nullable<decimal> SuperficieTerrenoEscritura { get; set; }
        public Nullable<int> IdTipoLote { get; set; }
        public int IdTipoTenencia { get; set; }
        public string Observaciones { get; set; }
        public Nullable<int> IdMotivoAvaluo { get; set; }
        public Nullable<int> IdCatTipoPredio { get; set; }
        public Nullable<int> IdUbicacionPredio { get; set; }
        public Nullable<int> IdEtapaConstruccion { get; set; }
        public Nullable<int> IdCalidadConstruccion { get; set; }
        public Nullable<int> IdConservacion { get; set; }
        public Nullable<int> IdTipoSueloZHV { get; set; }
        public Nullable<int> IdTipoResguardoLimite { get; set; }
        public Nullable<int> IdTipoUsuSuelo { get; set; }
        public Nullable<bool> ServicioAgua { get; set; }
        public string NoContratoAgua { get; set; }
        public Nullable<bool> ServicioDrenaje { get; set; }
        public Nullable<bool> ServicioElectrico { get; set; }
        public Nullable<bool> ServicioCable { get; set; }
        public Nullable<bool> ServicioAlumbrado { get; set; }
        public Nullable<bool> ServicioTelefono { get; set; }
        public Nullable<bool> ServicioVigilancia { get; set; }
        public Nullable<bool> ServicioLimpia { get; set; }
        public Nullable<bool> ServicioEmpedrado { get; set; }
        public Nullable<bool> ServicioAsfalto { get; set; }
        public Nullable<bool> ServicioAdocreto { get; set; }
        public Nullable<bool> ServicioBanqueta { get; set; }
        public Nullable<bool> ServicioGuarniciones { get; set; }
        public string ServicioPermisoCons { get; set; }
        public Nullable<int> ServicioVialidad { get; set; }
        public Nullable<int> IdTipoVialidad { get; set; }
        public Nullable<int> IdViasAccesso { get; set; }
        public Nullable<int> IdTopografia { get; set; }
        public Nullable<int> IdAntiguedadConstruccion { get; set; }
        public string ObservacionConstruccion { get; set; }
        public Nullable<int> IdUsuarioValuador { get; set; }
        public string FirmaElaboro { get; set; }
        public string CaracterInformante { get; set; }
        public Nullable<int> IdVistoBuenoUsuario { get; set; }
        public string FirmaVoBo { get; set; }
        public System.DateTime FechaElaboracion { get; set; }
        public Nullable<double> Latitud { get; set; }
        public Nullable<double> Longitud { get; set; }
        public bool Activo { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    
        public virtual cPredio cPredio { get; set; }
        public virtual cUsuarios cUsuarios { get; set; }
    }
}
