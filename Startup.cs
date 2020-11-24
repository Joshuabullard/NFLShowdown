using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NFLShowdown.Startup))]
namespace NFLShowdown
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
