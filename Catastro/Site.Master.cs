using Clases;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases.BL;
using System.Configuration;

namespace Catastro
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
            if (!IsPostBack)
            {
                //cUsuarios usuario = new cUsuarios();
                //usuario.Id = 1;
                //usuario.Usuario = "prueba";
                //Session["usuario"] = usuario;
                if (Session["usuario"] != null)
                    LlenaMenu();
                else
                    Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (Session["usuario"] == null)
                    Response.Redirect("~/Login.aspx");
            }
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    //cUsuarios usuario = new cUsuarios();
            //    //usuario.Id = 1;
            //    //usuario.Usuario = "prueba";
            //    //Session["usuario"] = usuario;
            //    if (Session["usuario"] != null)
            //        LlenaMenu();
            //    else
            //        Response.Redirect("~/Login.aspx");
            //}
            //else
            //{
            //    if (Session["usuario"] == null)
            //        Response.Redirect("~/Login.aspx");
            //}
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }

        protected void imbContrasenia_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Usuarios/Contrasenia.aspx");
        }

        protected void logOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            FormsAuthentication.RedirectToLoginPage();
        }

        protected void LlenaMenu()
        {
            List<pVentanasPrimerNivel_Result> nivel1 = new pProcedimientos().ObtieneVentanasPrimerNivel(((cUsuarios)Session["usuario"]).Id);
            List<vVentanasNivel2> nivel2 = new vVistasBL().ObtieneNivel2Menu(((cUsuarios)Session["usuario"]).Id);
            bool existePagina = false;
            //string nombreSitio = ConfigurationManager.AppSettings["NombreSitioWeb"];
            foreach (pVentanasPrimerNivel_Result v1 in nivel1)
            {
                MenuItem m = new MenuItem();
                m.Text = v1.Ventana;
                m.Value = v1.clave;
                List<vVentanasNivel2> nivel2n1 = new vVistasBL().ObtieneNivel2MenuByNivel1(nivel2, (int)v1.id);
                foreach (vVentanasNivel2 v2 in nivel2n1)
                {
                    MenuItem m2 = new MenuItem();
                    //if (Request.Url.AbsolutePath.ToString().ToLower().Replace(".aspx", "") == nombreSitio.ToLower() + v2.url.ToLower().Replace(".aspx", ""))
                    if ((Request.Url.ToString().ToLower() + ".aspx").Contains(v2.url.ToLower()))                        
                            existePagina = true;
                    m2.NavigateUrl = "~" + v2.url;
                    m2.Text = v2.Ventana;
                    m2.Value = v2.clave;
                    m.ChildItems.Add(m2);
                }
                M_Admo.Items.Add(m);
            }
            List<vVentanasNivel2Permisos> nivel2P = new vVistasBL().ObtieneNivel2Permisos(((cUsuarios)Session["usuario"]).Id);
            foreach (vVentanasNivel2Permisos v2 in nivel2P)
            {
                //if (Request.Url.AbsolutePath.ToString().ToLower().Replace(".aspx", "") == nombreSitio.ToLower() + v2.url.ToLower().Replace(".aspx", ""))
                if ((Request.Url.ToString().ToLower() + ".aspx").Contains(v2.url.ToLower().Replace(".aspx","")))
                {
                    existePagina = true;
                    break;
                }
            }
            if (!existePagina && !Request.Url.ToString().Contains("Default"))
                Response.Redirect("~/Default.aspx");
        }
    }

}