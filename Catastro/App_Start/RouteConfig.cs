using Microsoft.AspNet.FriendlyUrls;
using System.Web.Routing;

namespace Catastro
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode =  RedirectMode.Off ; // RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);
        }
    }
}
