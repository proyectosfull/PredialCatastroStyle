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
    
    public partial class vTabularDetalleConta
    {
        public int Recibo { get; set; }
        public string ClavePredial { get; set; }
        public string ClaveAnterior { get; set; }
        public string Contribuyente { get; set; }
        public string NombreColonia { get; set; }
        public string TipoPredio { get; set; }
        public string EstatusPredio { get; set; }
        public string EstatusRecibo { get; set; }
        public System.DateTime FechaPago { get; set; }
        public Nullable<System.DateTime> FechaCancelacion { get; set; }
        public string Periodo { get; set; }
        public int IdTipoTramite { get; set; }
        public string TipoTramite { get; set; }
        public Nullable<int> IdDescuento { get; set; }
        public string clave_descto { get; set; }
        public string descuento { get; set; }
        public decimal ImporteNeto { get; set; }
        public decimal ImporteDescuento { get; set; }
        public decimal ImportePagado { get; set; }
    }
}
