using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheLineSportsOnline.Startup))]
namespace TheLineSportsOnline
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
