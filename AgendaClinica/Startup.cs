using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(AgendaClinica.Startup))]

namespace AgendaClinica
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {   
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = System.TimeSpan.FromHours(1)
                //CookieName = "AgendaFacil",
                //CookiePath = "/"
            });
        }
    }
}
