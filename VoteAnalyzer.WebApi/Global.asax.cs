using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;

using Autofac;
using Autofac.Integration.WebApi;
using VoteAnalyzer.Services;
using VoteAnalyzer.WebApi.DependencyInjection;
using VoteAnalyzer.WebApi.Models;

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

            var voteService = container.Resolve<IVotesCounter>();
            var fileInfo = container.Resolve<ParseFilesInfo>();

            Task.Run(async () =>
            {
                foreach (var pdfFileInfo in fileInfo.FilesInfo)
                {
                    await voteService.ParseDocumentAsync(pdfFileInfo);
                }
            });
        }
    }
}
