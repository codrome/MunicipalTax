using Owin;
using Swashbuckle.Application;
using System.Web.Http;

namespace MunicipalTax.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);

            config
                 .EnableSwagger((c) =>
                    {
                        c.SingleApiVersion("v1", "Municipal Tax Rate API");
                        //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    })
                 .EnableSwaggerUi();
        }
    }
}
