using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Clases.Utilerias
{
    [Serializable]
    public class Periodo    {
        
        public int bInicial { get; set; }      
        public int eInicial { get; set; }   
        public int bFinal { get; set; }     
        public int eFinal { get; set; }

        public MensajesInterfaz mensaje;
    }

}