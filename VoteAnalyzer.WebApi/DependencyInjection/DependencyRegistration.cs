using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Features.Variance;
using MediatR;
using VoteAnalyzer.DataAccessLayer.DbContexts;
using VoteAnalyzer.DataAccessLayer.Entities;
using VoteAnalyzer.DataAccessLayer.Factories;
using VoteAnalyzer.DataAccessLayer.Repositories;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.Parser.Parsers;
using VoteAnalyzer.PdfIntegration.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;
using VoteAnalyzer.PdfIntegration.PdfServices;
using VoteAnalyzer.Services;
using VoteAnalyzer.WebApi.Models;

namespace VoteAnalyzer.WebApi.DependencyInjection
{
    public class DependencyRegistration
    {
        public static IContainer Register(ContainerBuilder builder)
        {
            builder
                .RegisterSource(new ContravariantRegistrationSource());

            builder
               .RegisterType<Mediator>()
               .As<IMediator>()
               .InstancePerLifetimeScope();

            builder
              .Register<SingleInstanceFactory>(ctx => {
                  var c = ctx.Resolve<IComponentContext>();
                  return t => { object o; return c.TryResolve(t, out o) ? o : null; };
              })
              .InstancePerLifetimeScope();

            builder
              .Register<MultiInstanceFactory>(ctx => {
                  var c = ctx.Resolve<IComponentContext>();
                  return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
              })
              .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(DependencyRegistration).GetTypeInfo().Assembly)
                .AsImplementedInterfaces(); 

            builder.RegisterType<Repository<Deputy>>()
                .As<IRepository<Deputy, Guid>>();

            builder.RegisterType<Repository<KnownVote>>()
                .As<IRepository<KnownVote, Guid>>();

            builder.RegisterType<Repository<Session>>()
                .As<IRepository<Session, Guid>>();

            builder.RegisterType<Repository<Vote>>()
                .As<IRepository<Vote, Guid>>();

            builder.RegisterType<Repository<VottingSession>>()
                .As<IRepository<VottingSession, Guid>>();

            builder.RegisterType<MainDbContext>()
                .AsSelf();

            builder.RegisterType<EntitiesFactory>()
                .As<IEntitiesFactory>();

            builder.RegisterType<DeputiesParser>()
                .As<IParser<ParseInfo, DeputyParserModel[]>>();

            builder.RegisterType<SessionParser>()
                .As<IParser<ParseInfo, SessionParserModel>>();

            builder.RegisterType<FirstVoteParser>()
                .As<IParser<string[], FirstVoteParserModel>>();

            builder.RegisterType<PageVotesParser>()
                .Named<IParser<ParseInfo, VoteParserModel[]>>("pageVotesParser");

            builder.RegisterType<DocumentVotesParser>()
                .Named<IParser<ParseInfo, VoteParserModel[]>>("documentVotesParser")
                .WithParameter(new ResolvedParameter(
                    (info, context) => info.Name == "parser",
                    (info, context) => context
                        .ResolveNamed<IParser<ParseInfo, VoteParserModel[]>>("pageVotesParser")));

            builder.RegisterType<VottingSessionParser>()
                .As<IParser<ParseInfo, VottingSessionParserModel>>();

            var path = new Uri(Assembly.GetAssembly(typeof(DependencyRegistration))
                .GetName().CodeBase).LocalPath;

            var configuration = ConfigurationManager.OpenExeConfiguration(path);

            var directory = configuration.AppSettings.Settings["Directory"].Value;
            var files = Directory.GetFiles(directory);

            var filesInfo = files
                .Select(f => new PdfFileInfo { Directory = directory, FileName = Path.GetFileName(f) })
                .ToArray();

            var parseFilesInfo = new ParseFilesInfo
            {
                FilesInfo = filesInfo
            };

            builder.RegisterInstance(parseFilesInfo)
                .As<ParseFilesInfo>();

            builder.RegisterType<PdfContainer>()
                .As<IPdfContainer>();

            builder.RegisterType<PdfService>()
                .As<IPdfService>();

            builder.RegisterType<VotingService>()
                .As<IVotingService>()
                .WithParameter(new ResolvedParameter(
                    (info, context) => info.Name == "parser",
                    (info, context) => context
                        .ResolveNamed<IParser<ParseInfo, VoteParserModel[]>>("documentVotesParser")));

            return builder.Build();
        }
    }
}