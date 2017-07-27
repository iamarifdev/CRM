using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(App.Web.Startup))]
namespace App.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
