using System;
using System.Threading.Tasks;

using VoteAnalyzer.DataAccessLayer.Entities;
using VoteAnalyzer.DataAccessLayer.Factories;
using VoteAnalyzer.DataAccessLayer.Repositories;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.Parser.Parsers;
using VoteAnalyzer.PdfIntegration.Models;

namespace VoteAnalyzer.Services
{
    public class VotingService : IVotingService
    {
        private readonly IParser<ParseInfo, VoteParserModel[]> _parser;
        private readonly IRepository<Vote, Guid> _voteRepository;
        private readonly IEntitiesFactory _entitiesFactory;

        public VotingService(IEntitiesFactory entitiesFactory,
            IParser<ParseInfo, VoteParserModel[]> parser,
            IRepository<Vote, Guid> voteRepository)
        {
            _entitiesFactory = entitiesFactory;
            _parser = parser;
            _voteRepository = voteRepository;
        }

        public async Task ParseDocumentAsync(PdfFileInfo fileInfo)
        {
            var votes = _parser.Parse(new ParseInfo { FileInfo = fileInfo });

            foreach (var vote in votes)
            {
                try
                {
                    var deputyTask = _entitiesFactory.CreateDeputyAsync(vote.DeputyParserModel.Name);

                    var sessionTask = _entitiesFactory.CreateSessionAsync(
                        vote.VottingSessionParserModel.SessionParserModel.Name,
                        vote.VottingSessionParserModel.SessionParserModel.DateTime);

                    await Task.WhenAll(deputyTask, sessionTask);

                    var deputy = deputyTask.Result;
                    var session = sessionTask.Result;

                    var vottingSession = await _entitiesFactory.CreateVottingSessionAsync(session.Id,
                        vote.VottingSessionParserModel.Number, vote.VottingSessionParserModel.Subject);

                    if (await _voteRepository.IsVoteExistsAsync(deputy.Id, vottingSession.Id))
                    {
                        continue;
                    }

                    var knownVote = await _entitiesFactory.CreateKnownVoteAsync(vote.Vote);

                    var realVote = new Vote
                    {
                        DeputyId = deputy.Id,
                        KnownVoteId = knownVote.Id,
                        VottingSessionId = vottingSession.Id
                    };


                    await _voteRepository.CreateAsync(realVote);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}
