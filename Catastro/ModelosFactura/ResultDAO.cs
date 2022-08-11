using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catastro.ModelosFactura
{
    public class ResultDAO
    {
        private bool success;
        private string message;
        private string paramAux;

        public ResultDAO()
        {
            this.success = false;
            this.message = "";
            this.paramAux = "";
        }

        public ResultDAO(bool ok, string msg)
        {
            this.success = ok;
            this.message = msg;
            this.paramAux = "";
        }

        public bool SUCCESS
        {
            get
            {
                return success;
            }

            set
            {
                success = value;
            }
        }

        public string MESSAGE
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }

        public string PARAM_AUX
        {
            get
            {
                return paramAux;
            }

            set
            {
                paramAux = value;
            }
        }
    }
}