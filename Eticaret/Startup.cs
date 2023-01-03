using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Eticaret.Startup))]
namespace Eticaret
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
