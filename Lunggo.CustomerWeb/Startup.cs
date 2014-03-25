using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lunggo.CustomerWeb.Startup))]
namespace Lunggo.CustomerWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
