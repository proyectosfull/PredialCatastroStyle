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
    
    public partial class vReciboReporteDet
    {
        public int Id { get; set; }
        public string ClavePredial { get; set; }
        public int IdTipoTramite { get; set; }
        public string Tramite { get; set; }
        public int IdUsuarioCobra { get; set; }
        public string Cajero { get; set; }
        public System.DateTime FechaPago { get; set; }
        public string EstadoRecibo { get; set; }
        public bool Facturado { get; set; }
        public string Nombre { get; set; }
        public Nullable<decimal> ImporteNeto { get; set; }
        public Nullable<decimal> ImporteDescuento { get; set; }
        public Nullable<decimal> ImporteTotal { get; set; }
        public string Periodo { get; set; }
        public int IdDetalle { get; set; }
        public string FormaPago { get; set; }
    }
}