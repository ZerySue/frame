using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyFrame.Startup))]
namespace MyFrame
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
