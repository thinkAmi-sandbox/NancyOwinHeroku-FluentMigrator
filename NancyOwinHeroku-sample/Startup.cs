using Owin;

namespace NancyOwinHeroku_sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}
