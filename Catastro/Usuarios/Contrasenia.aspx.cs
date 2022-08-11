using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases.BL;
using Catastro.Controles;
using Clases;
using Clases.Utilerias;
namespace Catastro.Configuracion
{
    public partial class Contrasenia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUsuario.Text =((cUsuarios)Session["usuario"]).Usuario;
            }
        }

        protected void btnCambiarContraseña_Click(object sender, EventArgs e)
        {
            cUsuarios usuario = new cUsuariosBL().GetByUsuarioContrasenia(txtUsuario.Text, new Utileria().GetSHA1(txtContraseniaViejaU.Text));
            if (usuario != null)
            {
                usuario.Contrasenia = new Utileria().GetSHA1(txtContraseniaNuevaU.Text);
                MensajesInterfaz msg = new cUsuariosBL().Update(usuario);
                mgs.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                mgs.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.UsuarioNoExiste), ModalPopupMensaje.TypeMesssage.Alert);
            }
        }

      
    }
}