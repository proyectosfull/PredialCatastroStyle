using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catastro.ModelosFactura
{
    public class PagoBean
    {
        private string numRecibo;
        private string fechaRealizo;
        private string lectorActual;
        private string lectorAnterior;
        private string fechaCubre;
        private string mesAnio;
        private string meses;
        private string condicionPago;
        private string formaPago;
        private string metodoPago;
        private string expedicion;
        private string numCuentaPago;
        private string consumoMedidor;
        private string cantidadLetra;
        private string pagoTotal;
        private string descuento;
        private string subtotal;
        private string observaciones;
        private bool esCancelado;
        private bool facturar;
        private List<List<string>> conceptosPago;

        public PagoBean() { }

        public PagoBean(string numRecibo, string fechaRealizo, string lectorActual, string lectorAnterior, string fechaCubre, string mesAnio, string meses, string condicionPago, string formaPago, string metodoPago, string expedicion, string numCuentaPago, string consumoMedidor, string cantidadLetra, string pagoTotal, string observaciones, List<List<string>> conceptosPago, bool cancel, bool facturar)
        {
            this.numRecibo = numRecibo;
            this.fechaRealizo = fechaRealizo;
            this.lectorActual = lectorActual;
            this.lectorAnterior = lectorAnterior;
            this.fechaCubre = fechaCubre;
            this.mesAnio = mesAnio;
            this.meses = meses;
            this.condicionPago = condicionPago;
            this.formaPago = formaPago;
            this.metodoPago = metodoPago;
            this.expedicion = expedicion;
            this.numCuentaPago = numCuentaPago;
            this.consumoMedidor = consumoMedidor;
            this.cantidadLetra = cantidadLetra;
            this.pagoTotal = pagoTotal;
            this.observaciones = observaciones;
            this.conceptosPago = conceptosPago;
            this.esCancelado = cancel;
            this.facturar = facturar;
        }

        public string Subtotal
        {
            get
            {
                return subtotal;
            }

            set
            {
                subtotal = value;
            }
        }

        public string Descuento
        {
            get
            {
                return descuento;
            }

            set
            {
                descuento = value;
            }
        }

        public bool Facturar
        {
            get
            {
                return facturar;
            }

            set
            {
                facturar = value;
            }
        }

        public bool EsCancelado
        {
            get
            {
                return esCancelado;
            }

            set
            {
                esCancelado = value;
            }
        }

        public string NumRecibo
        {
            get
            {
                return numRecibo;
            }

            set
            {
                numRecibo = value;
            }
        }

        public string FechaRealizo
        {
            get
            {
                return fechaRealizo;
            }

            set
            {
                fechaRealizo = value;
            }
        }

        public string LectorActual
        {
            get
            {
                return lectorActual;
            }

            set
            {
                lectorActual = value;
            }
        }

        public string LectorAnterior
        {
            get
            {
                return lectorAnterior;
            }

            set
            {
                lectorAnterior = value;
            }
        }

        public string FechaCubre
        {
            get
            {
                return fechaCubre;
            }

            set
            {
                fechaCubre = value;
            }
        }

        public string MesAnio
        {
            get
            {
                return mesAnio;
            }

            set
            {
                mesAnio = value;
            }
        }

        public string Meses
        {
            get
            {
                return meses;
            }

            set
            {
                meses = value;
            }
        }

        public string CondicionPago
        {
            get
            {
                return condicionPago;
            }

            set
            {
                condicionPago = value;
            }
        }

        public string FormaPago
        {
            get
            {
                return formaPago;
            }

            set
            {
                formaPago = value;
            }
        }

        public string MetodoPago
        {
            get
            {
                return metodoPago;
            }

            set
            {
                metodoPago = value;
            }
        }

        public string Expedicion
        {
            get
            {
                return expedicion;
            }

            set
            {
                expedicion = value;
            }
        }

        public string NumCuentaPago
        {
            get
            {
                return numCuentaPago;
            }

            set
            {
                numCuentaPago = value;
            }
        }

        public string ConsumoMedidor
        {
            get
            {
                return consumoMedidor;
            }

            set
            {
                consumoMedidor = value;
            }
        }

        public string CantidadLetra
        {
            get
            {
                return cantidadLetra;
            }

            set
            {
                cantidadLetra = value;
            }
        }

        public string PagoTotal
        {
            get
            {
                return pagoTotal;
            }

            set
            {
                pagoTotal = value;
            }
        }

        public List<List<string>> ConceptosPago
        {
            get
            {
                return conceptosPago;
            }

            set
            {
                conceptosPago = value;
            }
        }

        public string Observaciones
        {
            get
            {
                return observaciones;
            }

            set
            {
                observaciones = value;
            }
        }
    }
}