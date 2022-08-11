using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Catastro.Startup))]
namespace Catastro
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
