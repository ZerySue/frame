using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(frame0.Startup))]
namespace frame0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
