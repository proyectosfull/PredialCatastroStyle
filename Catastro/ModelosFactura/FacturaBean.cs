using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catastro.ModelosFactura
{
    public class FacturaBean
    {
        private string folioFiscal;
        private string certificadoEmisor;
        private string certificadoSAT;
        private string certificadoCSD;
        private string horaFecha;
        private string selloDigitalCFDI;
        private string selloSAT;
        private string cadenaOriginal;
        private string selloComprobante;
        private string rfcEmisor;
        private string rfcReceptor;
        private string total;
        private string nombreFiscal;
        private string rfc;
        private string direccionFiscal;
        private string usoCfdi;

        public FacturaBean() { }

        public string UsoCfdi
        {
            get
            {
                return usoCfdi;
            }

            set
            {
                usoCfdi = value;
            }
        }

        public string Total
        {
            get
            {
                return total;
            }

            set
            {
                total = value;
            }
        }

        public string RFCReceptor
        {
            get
            {
                return rfcReceptor;
            }

            set
            {
                rfcReceptor = value;
            }
        }

        public string RFCEmisor
        {
            get
            {
                return rfcEmisor;
            }

            set
            {
                rfcEmisor = value;
            }
        }

        public string SelloComprobante
        {
            get
            {
                return selloComprobante;
            }

            set
            {
                selloComprobante = value;
            }
        }

        public string FolioFiscal
        {
            get
            {
                return folioFiscal;
            }

            set
            {
                folioFiscal = value;
            }
        }

        public string CertificadoSAT
        {
            get
            {
                return certificadoSAT;
            }

            set
            {
                certificadoSAT = value;
            }
        }

        public string CertificadoCSD
        {
            get
            {
                return certificadoCSD;
            }

            set
            {
                certificadoCSD = value;
            }
        }

        public string HoraFecha
        {
            get
            {
                return horaFecha;
            }

            set
            {
                horaFecha = value;
            }
        }

        public string SelloDigitalCFDI
        {
            get
            {
                return selloDigitalCFDI;
            }

            set
            {
                selloDigitalCFDI = value;
            }
        }

        public string SelloSAT
        {
            get
            {
                return selloSAT;
            }

            set
            {
                selloSAT = value;
            }
        }

        public string CadenaOriginal
        {
            get
            {
                return cadenaOriginal;
            }

            set
            {
                cadenaOriginal = value;
            }
        }

        public string CertificadoEmisor
        {
            get
            {
                return certificadoEmisor;
            }

            set
            {
                certificadoEmisor = value;
            }
        }

        public string NombreFiscal
        {
            get
            {
                return nombreFiscal;
            }

            set
            {
                nombreFiscal = value;
            }
        }

        public string Rfc
        {
            get
            {
                return rfc;
            }

            set
            {
                rfc = value;
            }
        }

        public string DireccionFiscal
        {
            get
            {
                return direccionFiscal;
            }

            set
            {
                direccionFiscal = value;
            }
        }
    }
}