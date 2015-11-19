using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WEBNotifiyBackend.Startup))]
namespace WEBNotifiyBackend
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
