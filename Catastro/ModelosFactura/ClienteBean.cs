using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catastro.ModelosFactura
{
    public class ClienteBean
    {
        private string numContrato;
        private string nombreCompleto;
        private string direccion;
        private string rfc;
        private string giro;
        private string claveGiro;
        private string correoElectronico;

        private string claveCatastral;  
        private string cuentaCatastral;
        private string tipoPredio;
        private string superficieTerreno;       
        private string areaComun;
        private string superficieAreaConstruccion;
        private string baseImpuesto;
        private string referencia;
        private string usuarioFacturo;
        private string direccionPredio;
        private string idPredio;

        public ClienteBean() { }

        public ClienteBean(string numContrato, string nombreCompleto, string direccion, string rfc, string giro, string claveGiro, string correo
            , string claveCatastral, string cuentaCatastral, string tipoPredio, string superficieTerreno, string areaComun, 
            string superficieAreaConstruccion, string baseImpuesto, string referencia, string usuarioFacturo, string direccionPredio)
        {
            this.numContrato = numContrato;
            this.nombreCompleto = nombreCompleto;
            this.direccion = direccion;
            this.rfc = rfc;
            this.giro = giro;
            this.claveGiro = claveGiro;
            this.correoElectronico = correo;
            this.claveCatastral = claveCatastral;
            this.cuentaCatastral = cuentaCatastral;
            this.tipoPredio = tipoPredio;
            this.superficieTerreno = superficieTerreno;
            this.areaComun = areaComun;
            this.superficieAreaConstruccion = superficieAreaConstruccion;
            this.baseImpuesto = baseImpuesto;
            this.referencia = referencia;
            this.usuarioFacturo = usuarioFacturo;
            this.direccionPredio = direccionPredio;
        }

        public string IdPredio
        {
            get
            {
                return idPredio;
            }
            set
            {
                idPredio = value;
            }
        }
        public string UsuarioFacturo
        {
            get
            {
                return usuarioFacturo;
            }
            set
            {
                usuarioFacturo = value;
            }
        }
        public string Referencia
        {
            get
            {
                return referencia;
            }
            set
            {
                referencia = value;
            }
        }

        public string DireccionPredio
        {
            get
            {
                return direccionPredio;
            }
            set
            {
                direccionPredio = value;
            }
        }
        public string BaseImpuesto
        {
            get
            {
                return baseImpuesto;
            }
            set
            {
                baseImpuesto = value;
            }
        }
        public string SuperficieAreaConstruccion
        {
            get
            {
                return superficieAreaConstruccion;
            }
            set
            {
                superficieAreaConstruccion = value;
            }
        }
        public string AreaComun
        {
            get
            {
                return areaComun;
            }
            set
            {
                areaComun = value;
            }
        }
        public string SuperficieTerreno
        {
            get
            {
                return superficieTerreno;
            }
            set
            {
                superficieTerreno = value;
            }
        }

        public string TipoPredio
        {
            get
            {
                return tipoPredio;
            }
            set
            {
                tipoPredio = value;
            }
        }
        public string CuentaCatastral
        {
            get
            {
                return cuentaCatastral;
            }
            set
            {
                cuentaCatastral = value;
            }
        }
        public string ClaveCatastral
        {
            get
            {
                return claveCatastral;
            }
            set
            {
                claveCatastral = value;
            }
         }

        public string CorreoElectronico
        {
            get
            {
                return correoElectronico;
            }

            set
            {
                correoElectronico = value;
            }
        }

        public string NumContrato
        {
            get
            {
                return numContrato;
            }

            set
            {
                numContrato = value;
            }
        }

        public string NombreCompleto
        {
            get
            {
                return nombreCompleto;
            }

            set
            {
                nombreCompleto = value;
            }
        }

        public string Direccion
        {
            get
            {
                return direccion;
            }

            set
            {
                direccion = value;
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

        public string Giro
        {
            get
            {
                return giro;
            }

            set
            {
                giro = value;
            }
        }

        public string ClaveGiro
        {
            get
            {
                return claveGiro;
            }

            set
            {
                claveGiro = value;
            }
        }
    }
}