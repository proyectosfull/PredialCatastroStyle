using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Catastro.Controles
{
    public partial class ModalPopupMensaje : System.Web.UI.UserControl
    {
        #region miembros
        //bool displayAceptar = true;
        //bool displayCancelar = true;

        /// <summary>
        /// Define enumeracion para el tipo de mensaje dentro de la ventana modal
        /// ya sea si presenta solo informacion, o presenta solo mensaje de Error
        /// </summary>
        public enum TypeMesssage
        {
            Confirm = 0,
            Alert = 1,
            Error = 1
        }

        #endregion miembros


        #region Event Handler
        public event EventHandler eventoAceptar;
        #endregion Event Handler


        #region propiedades

        /// <summary>
        /// Propiedad que establece la creación de un evento al control
        /// </summary>
        public event EventHandler EventoAceptar
        {
            add { eventoAceptar += value; }
            remove { eventoAceptar -= value; }
        }

        /// <summary>
        /// Crea una propiedad que permite especificar el si el control tendrá visible 
        /// el boton Aceptar
        /// </summary>
        public bool DysplayAceptar
        {
            set { ViewState["displayAceptar"] = value; }
            get
            {
                if ((object)ViewState["displayAceptar"] == null)
                    return true;
                else
                    return (bool)ViewState["displayAceptar"];
            }
            //set { this.displayAceptar = value; }
            //get { return this.displayAceptar; }
        }

        /// <summary>
        /// Crea una propiedad que permite especificar el si el control tendrá visible 
        /// el boton Cancelar
        /// </summary>
        public bool DysplayCancelar
        {
            set { ViewState["displayCancelar"] = value; }
            get
            {
                if ((object)ViewState["displayCancelar"] == null)
                    return true;
                else
                    return (bool)ViewState["displayCancelar"];
            }
            //set { this.displayCancelar = value; }
            //get { return this.displayCancelar; }
        }

        /// <summary>
        /// Crea una propiedad del tipo TypeMessage
        /// la cual guardará el tipo de mensaje en el ViewState
        /// </summary>
        public TypeMesssage TipoMensaje
        {
            get { return (TypeMesssage)ViewState["TipoMensaje"]; }
            set { ViewState["TipoMensaje"] = value; }
        }

        /// <summary>
        /// Crea una propiedad del MENSAJE
        /// la cual guardará el  mensaje 
        /// </summary>
        public string Mensaje
        {
            get { return lblMensaje.Text; }
        }

        #endregion propiedades
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //this.btnAceptarMensaje.Visible = displayAceptar;
            //this.btnCancelarMensaje.Visible = displayCancelar;
            //}
        }

        /// <summary>
        /// Metodo que permite mandar el mensaje en la ventana modal
        /// </summary>
        /// <param name="mensaje">Recibe una cadena la cual se presenta en la ventana modal</param>
        /// <param name="tipoMensaje">Recibe una valor enumeracion con el tipo de mensaje</param>
        public void ShowPopup(string mensaje, TypeMesssage tipoMensaje)
        {
            this.btnAceptarMensaje.Visible = DysplayAceptar;
            this.btnCancelarMensaje.Visible = DysplayCancelar;
            lblMensaje.Text = mensaje;
            this.TipoMensaje = tipoMensaje;
            if (this.TipoMensaje == TypeMesssage.Alert)
            {
                btnCancelarMensaje.Visible = false;
                this.btnAceptarMensaje.Focus();
            }
            //else
            //    btnCancelarMensaje.Visible = true;

            mpeMensaje.Show();
            //if (this.btnAceptarMensaje.Visible && this.btnCancelarMensaje.Visible == false)
            //    this.btnAceptarMensaje.Focus();

            btnAceptarMensaje.Text = "Aceptar";
            btnCancelarMensaje.Text = "Cancelar";
        }
        public void ShowPopup(string mensaje, TypeMesssage tipoMensaje, string textAceptar, string textCancelar)
        {
            this.btnAceptarMensaje.Visible = DysplayAceptar;
            this.btnCancelarMensaje.Visible = DysplayCancelar;
            lblMensaje.Text = mensaje;
            this.TipoMensaje = tipoMensaje;
            mpeMensaje.Show();
            if (this.TipoMensaje == TypeMesssage.Alert)
            {
                btnCancelarMensaje.Visible = false;
                this.btnAceptarMensaje.Focus();
            }
            //if (this.btnAceptarMensaje.Visible && this.btnCancelarMensaje.Visible == false)
            //    this.btnAceptarMensaje.Focus();
            //else
            //    btnCancelarMensaje.Visible = true;

            btnAceptarMensaje.Text = textAceptar;
            btnCancelarMensaje.Text = textCancelar;

        }


        /// <summary>
        /// Metodo que permite la ejecucion del evento creado en la pagina principal
        /// </summary>
        /// <param name="e"></param>
        protected virtual void lanzaEvento(EventArgs e)
        {
            if (eventoAceptar != null)
            {
                eventoAceptar(this, e);
            }
        }



        /// <summary>
        /// Evento Click del boton Aceptar el cual permite cerrar la ventana modal
        /// y apartir del tipo de mensaje definido lanzar un evento que realize otra operacion o no.
        /// Ejemplo: TypeMesssage tipo = TypeMesssage.Informacion; lanzará el evento.
        ///          TypeMesssage tipo = TypeMesssage.Error;  solo cierra la ventana sin hacer otra operacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarMensaje_Click(object sender, EventArgs e)
        {
            mpeMensaje.Hide();

            if (this.TipoMensaje == TypeMesssage.Confirm)
            {
                lanzaEvento(e);
            }
        }

        /// <summary>
        /// Evento Click del boton Cancelar el cual solo cierra la ventnana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarMensaje_Click(object sender, EventArgs e)
        {
            mpeMensaje.Hide();
        }
    }
}