using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Owin;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;


namespace UTAAA
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            var options_cookie = new CookieAuthenticationOptions
            {
                CookieManager = new SystemWebCookieManager()
            };

            app.UseCookieAuthentication(options_cookie);


            var options = new WsFederationAuthenticationOptions
            {
                Wtrealm = ConfigurationManager.AppSettings["ida:Wtrealm"],
                MetadataAddress = ConfigurationManager.AppSettings["ida:ADFSMetadata"],

                // optional for customization and adding AD groups to the token in case they are omitted by ADFS
                Notifications = new WsFederationAuthenticationNotifications()
                {
                    SecurityTokenValidated = async notification =>
                    {
                        var upn = notification.AuthenticationTicket.Identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Upn).Value;
                        using (var principalContext = new PrincipalContext(ContextType.Domain, "UTAD"))
                        {
                            using (var user = UserPrincipal.FindByIdentity(principalContext, upn))
                            {
                                var groups = user.GetGroups().Select(group => new Claim(ClaimTypes.Role, group.Name));

                                notification.AuthenticationTicket.Identity.AddClaims(groups);
                                //Removing the claim NAme which gives full name and adding UTAD for name
                                var claim = notification.AuthenticationTicket.Identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name);
                                if(claim != null)
                                {
                                    notification.AuthenticationTicket.Identity.RemoveClaim(claim);

                                }

                                notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Name, user.SamAccountName));
                            }
                        }

                        await Task.CompletedTask;
                    }
                }
            };

            app.UseWsFederationAuthentication(options);
        }

        //public static void ConfigureWebApi(IAppBuilder app)
        //{
        //    // Web API configuration and services
        //    var config = new HttpConfiguration();
        //    //config.Filters.Add(new AuthorizeAttribute() { Roles = ConfigurationManager.AppSettings["user:Admins"] });
        //    config.Formatters.Remove(config.Formatters.XmlFormatter);

        //    // Web API routes
        //    config.MapHttpAttributeRoutes();

        //    config.Routes.MapHttpRoute(
        //        name: "DefaultApi",
        //        routeTemplate: "api/{controller}/{id}",
        //        defaults: new { id = RouteParameter.Optional }
        //    );

        //   // app.UseWebApi(config);
        //}

        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            ConfigureAuth(app);

            //ConfigureWebApi(app);
        }
    }
}


//using System;
//using System.Threading.Tasks;
//using Microsoft.Owin;
//using Owin;

//namespace GAIMV3
//{
//    public partial class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            ConfigureAuth(app);
//        }
//    }
//}
