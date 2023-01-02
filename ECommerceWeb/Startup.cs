using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ECommerceWeb.Startup))]
namespace ECommerceWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
