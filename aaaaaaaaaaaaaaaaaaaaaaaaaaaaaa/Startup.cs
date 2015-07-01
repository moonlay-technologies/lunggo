using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.Startup))]
namespace aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
