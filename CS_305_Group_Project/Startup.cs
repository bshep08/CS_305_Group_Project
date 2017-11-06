using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CS_305_Group_Project.Startup))]
namespace CS_305_Group_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
