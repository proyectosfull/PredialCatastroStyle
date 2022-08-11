using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CatastroPago.Startup))]
namespace CatastroPago
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
