using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Catastro.Controles;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using System.Web.Security;

namespace Catastro
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtUsuario.Text = Request.Url.ToString();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            cUsuarios usuario = new cUsuariosBL().GetByUsuarioContrasenia(txtUsuario.Text, new Utileria().GetSHA1(txtContrasenia.Text));
            if (usuario != null)
            {
                if (usuario.Activo)
                {
                    FormsAuthentication.SetAuthCookie(txtUsuario.Text, false);
                    Session["usuario"] = usuario;
                    Response.Redirect("~/Default.aspx", false);
                }
                else
                {
                    mgs.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.UsuarioInactivo), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            else
            {
                mgs.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.UsuarioNoExiste), ModalPopupMensaje.TypeMesssage.Alert);
            }
        }

      
    }
}