using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebNotifyBackend.Startup))]
namespace WebNotifyBackend
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
