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
    
    public partial class tPredioLocalizacion
    {
        public int Id { get; set; }
        public int IdLocalidad { get; set; }
        public string NombreLocalidad { get; set; }
        public int IdColonia { get; set; }
        public string NombreColonia { get; set; }
        public int IdCalle { get; set; }
        public string NombreCalle { get; set; }
        public int IdFraccionamiento { get; set; }
        public string Fraccionamiento { get; set; }
        public string Cp { get; set; }
        public string ClaveCatastral { get; set; }
        public string Referencia { get; set; }
        public int IdPredio { get; set; }
        public int IdTerreno { get; set; }
        public bool Activo { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    }
}
