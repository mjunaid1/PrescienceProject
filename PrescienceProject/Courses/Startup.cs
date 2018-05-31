using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Courses.Startup))]
namespace Courses
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
