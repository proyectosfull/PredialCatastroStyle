using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Catastro.ModelosFactura
{
    public partial class Factura
    {
        [JsonProperty("Comprobante")]
        public Comprobante Comprobante { get; set; }

        //[JsonProperty("CamposPDF")]
        //public CamposPdf CamposPdf { get; set; }
    }

    //public partial class CamposPdf
    //{
    //    [JsonProperty("tipoComprobante")]
    //    public string TipoComprobante { get; set; }

    //    [JsonProperty("Comentarios")]
    //    public string Comentarios { get; set; }
    //}

    public partial class Comprobante
    {
        [JsonProperty("Serie")]
        public string Serie { get; set; }

        [JsonProperty("Folio")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Folio { get; set; }

        [JsonProperty("Fecha")]
        public string Fecha { get; set; }

        [JsonProperty("FormaPago")]
        public string FormaPago { get; set; }

        [JsonProperty("NoCertificado")]
        public string NoCertificado { get; set; }

        [JsonProperty("CondicionesDePago")]
        public string CondicionesDePago { get; set; }

        [JsonProperty("SubTotal")]
        public string SubTotal { get; set; }

        [JsonProperty("Descuento")]
        public string Descuento { get; set; }

        [JsonProperty("Moneda")]
        public string Moneda { get; set; }

        [JsonProperty("TipoCambio")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long TipoCambio { get; set; }

        [JsonProperty("Total")]
        public string Total { get; set; }

        [JsonProperty("TipoDeComprobante")]
        public string TipoDeComprobante { get; set; }

        [JsonProperty("MetodoPago")]
        public string MetodoPago { get; set; }

        [JsonProperty("LugarExpedicion")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long LugarExpedicion { get; set; }

        [JsonProperty("Emisor")]
        public Emisor Emisor { get; set; }

        [JsonProperty("Receptor")]
        public Receptor Receptor { get; set; }

        [JsonProperty("Conceptos")]
        public List<Concepto> Conceptos { get; set; }

        [JsonProperty("Impuestos")]
        public ComprobanteImpuestos Impuestos { get; set; }
    }

    public partial class Concepto
    {
        [JsonProperty("ClaveProdServ")]
        public string ClaveProdServ { get; set; }

        [JsonProperty("NoIdentificacion")]
        public string NoIdentificacion { get; set; }

        [JsonProperty("Cantidad")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Cantidad { get; set; }

        [JsonProperty("ClaveUnidad")]
        public string ClaveUnidad { get; set; }

        [JsonProperty("Unidad")]
        public string Unidad { get; set; }

        [JsonProperty("Descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("ValorUnitario")]
        public string ValorUnitario { get; set; }

        [JsonProperty("Importe")]
        public string Importe { get; set; }

        [JsonProperty("Descuento")]
        public string Descuento { get; set; }

        [JsonProperty("Impuestos")]
        public ConceptoImpuestos Impuestos { get; set; }
    }

    public partial class ConceptoImpuestos
    {
        [JsonProperty("Traslados")]
        public List<Traslado> Traslados { get; set; }
    }

    public partial class Traslado
    {
        [JsonProperty("Base", NullValueHandling = NullValueHandling.Ignore)]
        public string Base { get; set; }

        [JsonProperty("Impuesto")]
        public string Impuesto { get; set; }

        [JsonProperty("TipoFactor")]
        public string TipoFactor { get; set; }

        [JsonProperty("TasaOCuota")]
        public string TasaOCuota { get; set; }

        [JsonProperty("Importe")]
        public string Importe { get; set; }
    }

    public partial class Emisor
    {
        [JsonProperty("Rfc")]
        public string Rfc { get; set; }

        [JsonProperty("Nombre")]
        public string Nombre { get; set; }

        [JsonProperty("RegimenFiscal")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long RegimenFiscal { get; set; }
    }

    public partial class ComprobanteImpuestos
    {
        [JsonProperty("TotalImpuestosTrasladados")]
        public string TotalImpuestosTrasladados { get; set; }

        [JsonProperty("Traslados")]
        public List<Traslado> Traslados { get; set; }
    }

    public partial class Receptor
    {
        [JsonProperty("Rfc")]
        public string Rfc { get; set; }

        [JsonProperty("Nombre")]
        public string Nombre { get; set; }

        [JsonProperty("UsoCFDI")]
        public string UsoCfdi { get; set; }
    }

    public partial class Factura
    {
        public static Factura FromJson(string json) => JsonConvert.DeserializeObject<Factura>(json, Catastro.ModelosFactura.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Factura self) => JsonConvert.SerializeObject(self, Catastro.ModelosFactura.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
