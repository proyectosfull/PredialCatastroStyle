using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Catastro
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           // System.IO.File.Delete(Server.MapPath("~") + "/Temporales/");
        }
        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            if (HttpContext.Current != null)
            {
                var url = HttpContext.Current.Request.Url;
                var page = HttpContext.Current.Handler as System.Web.UI.Page;
                new Clases.Utilerias.Utileria().logError(url.ToString() + "." + page.ToString() + ".", ex, " --- " + ex.StackTrace.ToString());
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("x-frame-options", "SAMEORIGIN");
            //protección clickjacking
            HttpContext.Current.Response.AddHeader("x-XSS-Protection", "1");
            //protección contra cross site scripting
        }
        void Application_End(object sender, EventArgs e)
        {           
           
            //System.IO.File.Delete(Server.MapPath("~") +"/Temporales/");
        }
        

    }
}