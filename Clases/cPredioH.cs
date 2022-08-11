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
    
    public partial class cPredioH
    {
        public int EjercicioH { get; set; }
        public int Id { get; set; }
        public string ClavePredial { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Colonia { get; set; }
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
        public string UsoSuelo { get; set; }
        public string ExentoPago { get; set; }
        public string StatusPredio { get; set; }
        public Nullable<System.DateTime> FechaBaja { get; set; }
        public int IdTipoPredio { get; set; }
        public string TipoPredio { get; set; }
        public string Contribuyente { get; set; }
        public string TipoFaseIp { get; set; }
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
        public string TipoFaseSm { get; set; }
        public string TipoMovAvaluo { get; set; }
        public string TipoInmueble { get; set; }
        public string Condominio { get; set; }
        public Nullable<int> IdTipoFaseDoc { get; set; }
        public bool Activo { get; set; }
        public string Usuario { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    }
}