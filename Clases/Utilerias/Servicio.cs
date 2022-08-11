using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Clases.Utilerias{
  
    [Serializable]
    public class Servicio
    {
        //Anticipado            
        public Periodo PerAnticipado { get; set; }
        public int AntBimestre { get; set; }

        public decimal AntInfraestructura { get; set; }
        public decimal AntDescInfraestructura { get; set; }
        public decimal AntAdicInfraestructura { get; set; }
        public decimal AntDescAdicInfraestructura { get; set; }

        public decimal AntRecoleccion { get; set; }
        public decimal AntDescRecoleccion { get; set; }
        public decimal AntAdicRecoleccion { get; set; }
        public decimal AntDescAdicRecoleccion { get; set; }

        public decimal AntLimpieza { get; set; }
        public decimal AntDescLimpieza { get; set; }
        public decimal AntAdicLimpieza { get; set; }
        public decimal AntDescAdicLimpieza { get; set; }

        public decimal AntDap { get; set; }
        public decimal AntDescDap { get; set; }
        public decimal AntAdicDap { get; set; }
        public decimal AntDescAdicDap { get; set; }

        //public decimal AntAdicional { get; set; }
        //public decimal AntDescAdicional { get; set; }
 
        //Actual             
        public Periodo PerActual { get; set; }
        public int ActBimestre { get; set; }

        public decimal ActInfraestructura { get; set; }       
        public decimal ActDescInfraestructura { get; set; }
        public decimal ActAdicInfraestructura { get; set; }
        public decimal ActDescAdicInfraestructura { get; set; }
        public decimal ActRecInfraestructura { get; set; }
        public decimal ActDescRecInfraestructura { get; set; }

        public decimal ActRecoleccion { get; set; }
        public decimal ActDescRecoleccion { get; set; }
        public decimal ActAdicRecoleccion { get; set; }
        public decimal ActDescAdicRecoleccion { get; set; }
        public decimal ActRecRecoleccion { get; set; }
        public decimal ActDescRecRecoleccion { get; set; }

        public decimal ActLimpieza { get; set; }
        public decimal ActDescLimpieza { get; set; }
        public decimal ActAdicLimpieza { get; set; }
        public decimal ActDescAdicLimpieza { get; set; }
        public decimal ActRecLimpieza { get; set; }
        public decimal ActDescRecLimpieza { get; set; }

        public decimal ActDap { get; set; }
        public decimal ActDescDap { get; set; }
        public decimal ActAdicDap { get; set; }
        public decimal ActDescAdicDap { get; set; }
        public decimal ActRecDap { get; set; }
        public decimal ActDescRecDap { get; set; }

        //public decimal ActRecargo { get; set; }
        //public decimal AcDescRecargo { get; set; }
        //public decimal ActAdicional { get; set; }
        //public decimal ActDescAdicional { get; set; }
        public decimal ActINP { get; set; }  

        //Rezago  
        public Periodo PerRezago { get; set; }
        public int RezBimestre { get; set; }
        public decimal Rezagos { get; set; }
        public decimal RezDescRezagos { get; set; }
        public decimal RezRecargo { get; set; }
        public decimal RezDescRecargos { get; set; }
        public decimal RezAdicional { get; set; }
        public decimal RezDescAdicional { get; set; }
        public decimal RezINP { get; set; }
               
        //Generales  
        public decimal Multa { get; set; }
        public decimal MultaDesc { get; set; }
        public decimal Ejecucion { get; set; }
        public decimal EjecucionDesc { get; set; }
        public int IdRequerimiento { get; set; }
        public decimal Honorarios { get; set; }
        public decimal HonorariosDesc { get; set; }

        //sumatorias         
        public decimal DescuentoGral { get; set; }
        public decimal Importe { get; set; }
        public string PeriodoGral { get; set; }

        public ServicioEdo Estado { get; set; }

        //public Conceptos Concepto { get; set; }
        public string TextError { get; set; }

        public MensajesInterfaz mensaje;
    }
}