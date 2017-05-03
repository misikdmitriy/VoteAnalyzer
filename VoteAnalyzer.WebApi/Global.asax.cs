using System.Reflection;
using System.Web.Http;

using Autofac;
using Autofac.Integration.WebApi;

using VoteAnalyzer.WebApi.DependencyInjection;

namespace VoteAnalyzer.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // Set the dependency resolver to be Autofac.
            var container = DependencyRegistration.Register(builder);

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
