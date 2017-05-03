using System;
using Autofac;
using Autofac.Core;

using VoteAnalyzer.DataAccessLayer.DbContexts;
using VoteAnalyzer.DataAccessLayer.Entities;
using VoteAnalyzer.DataAccessLayer.Repositories;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.Parser.Parsers;
using VoteAnalyzer.PdfIntegration.PdfContainers;
using VoteAnalyzer.PdfIntegration.PdfServices;

namespace VoteAnalyzer.WebApi.DependencyInjection
{
    public class DependencyRegistration
    {
        public static IContainer Register(ContainerBuilder builder)
        {
            builder.RegisterType<IRepository<Deputy, Guid>>()
                .As<Repository<Deputy>>();

            builder.RegisterType<IRepository<KnownVote, Guid>>()
                .As<Repository<KnownVote>>();

            builder.RegisterType<IRepository<Session, Guid>>()
                .As<Repository<Session>>();

            builder.RegisterType<IRepository<Vote, Guid>>()
                .As<Repository<Vote>>();

            builder.RegisterType<IRepository<VottingSession, Guid>>()
                .As<Repository<VottingSession>>();

            builder.RegisterType<MainDbContext>()
                .AsSelf();

            builder.RegisterType<IParser<ParseInfo, DeputyParserModel>>()
                .As<DeputiesParser>();

            builder.RegisterType<IParser<ParseInfo, SessionParserModel>>()
                .As<SessionParser>();

            builder.RegisterType<IParser<ParseInfo, VoteParserModel[]>>()
                .Named<VotesParser>("pageVoteParser");

            builder.RegisterType<IParser<ParseInfo, VoteParserModel[]>>()
                .As<VoteParserFacade>()
                .WithParameter(new ResolvedParameter(
                    (info, context) => info.Name == "parser",
                    (info, context) => context
                        .ResolveNamed<IParser<ParseInfo, VoteParserModel[]>>("pageVoteParsers")));

            builder.RegisterType<IParser<ParseInfo, VottingSessionParserModel>>()
                .As<VottingSessionParser>();

            builder.RegisterType<IPdfContainer>()
                .As<PdfContainer>();

            builder.RegisterType<IPdfService>()
                .As<PdfService>();

            return builder.Build();
        }
    }
}