using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using VoteAnalyzer.DataAccessLayer.Entities;
using VoteAnalyzer.DataAccessLayer.Factories;
using VoteAnalyzer.DataAccessLayer.Repositories;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.Parser.Parsers;
using VoteAnalyzer.PdfIntegration.Models;

namespace VoteAnalyzer.Services
{
    public class VotesCounter : IVotesCounter
    {
        private readonly IParser<ParseInfo, VoteParserModel[]> _parser;
        private readonly IRepository<Vote, Guid> _voteRepository;
        private readonly IRepository<ParsedFile, Guid> _parsedFilesRepository;
        private readonly IEntitiesFactory _entitiesFactory;

        public VotesCounter(IEntitiesFactory entitiesFactory,
            IParser<ParseInfo, VoteParserModel[]> parser,
            IRepository<Vote, Guid> voteRepository, 
            IRepository<ParsedFile, Guid> parsedFilesRepository)
        {
            _entitiesFactory = entitiesFactory;
            _parser = parser;
            _voteRepository = voteRepository;
            _parsedFilesRepository = parsedFilesRepository;
        }

        public async Task ParseDocumentAsync(PdfFileInfo fileInfo)
        {
            var path = Path.Combine(fileInfo.Directory, fileInfo.FileName);

            if ((await _parsedFilesRepository.ReadAsync(p => path.Equals(p.Path, 
                    StringComparison.InvariantCultureIgnoreCase))).Any())
            {
                return;
            }
            var parseInfo = new ParseInfo { FileInfo = fileInfo, Page = 0 };

            while (true)
            {
                parseInfo.Page++;

                try
                {
                    var votes = _parser.Parse(parseInfo);

                    await AddVotes(votes);
                }
                catch (Exception)
                {
                    break;
                }
            }

            await _parsedFilesRepository.CreateAsync(new ParsedFile {Path = path});
        }

        private async Task AddVotes(IEnumerable<VoteParserModel> votes)
        {
            foreach (var vote in votes)
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
        }
    }
}
