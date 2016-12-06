using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HandsomeWeb.Startup))]
namespace HandsomeWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
