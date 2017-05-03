using System.Threading.Tasks;
using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.DataAccessLayer.Entities;
using VoteAnalyzer.DataAccessLayer.Repositories;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.Parser.Parsers;
using VoteAnalyzer.PdfIntegration.Models;

namespace VoteAnalyzer.Services
{
    public class VotingService
    {
        private IParser<ParseInfo, VoteParserModel[]> _parser;  

        private Repository<Deputy> _deputiesRepository;
        private Repository<KnownVote> _knownVoteRepository;
        private Repository<Session> _sessionRepository;
        private Repository<Vote> _voteRepository;
        private Repository<VottingSession> _vottingSessionRepository;

        public VotingService(IParser<ParseInfo, VoteParserModel[]> parser,
            Repository<Deputy> deputiesRepository,
            Repository<KnownVote> knownVoteRepository,
            Repository<Session> sessionRepository,
            Repository<Vote> voteRepository,
            Repository<VottingSession> vottingSessionRepository)
        {
            _deputiesRepository = deputiesRepository;
            _knownVoteRepository = knownVoteRepository;
            _parser = parser;
            _sessionRepository = sessionRepository;
            _voteRepository = voteRepository;
            _vottingSessionRepository = vottingSessionRepository;
        }

        public async Task ParseDocumentAsync(PdfFileInfo fileInfo)
        {
            var votes = _parser.Parse(new ParseInfo {FileInfo = fileInfo});

            foreach (var vote in votes)
            {
                var deputy = await GetDeputyAsync(vote);
                var session = await GetSessionAsync(vote);
                var vottingSession = await GetVottingSessionAsync(vote, session);

                if (await _voteRepository.IsVoteExistsAsync(deputy.Id, vottingSession.Id))
                {
                    continue;
                }

                var knownVote = await GetKnownVoteAsync(vote);

                var realVote = new Vote
                {
                    DeputyId = deputy.Id,
                    KnownVoteId = knownVote.Id,
                    VottingSessionId = vottingSession.Id
                };

                await _voteRepository.CreateAsync(realVote);
            }
        }

        private async Task<Deputy> GetDeputyAsync(VoteParserModel vote)
        {
            var deputy = await _deputiesRepository.GetDeputyByNameAsync(vote.DeputyParserModel.Name);

            if (deputy == null)
            {
                deputy = new Deputy
                {
                    Name = vote.DeputyParserModel.Name
                };

                await _deputiesRepository.CreateAsync(deputy);
            }

            return deputy;
        }

        private async Task<Session> GetSessionAsync(VoteParserModel vote)
        {
            var session = await _sessionRepository.GetSessionByNameAsync(vote.VottingSessionParserModel.SessionParserModel.Name);

            if (session == null)
            {
                session = new Session
                {
                    Name = vote.VottingSessionParserModel.SessionParserModel.Name,
                    DateTime = vote.VottingSessionParserModel.SessionParserModel.DateTime
                };

                await _sessionRepository.CreateAsync(session);
            }

            return session;
        }

        private async Task<VottingSession> GetVottingSessionAsync(VoteParserModel vote, Session session)
        {
            var vottingSession =
                    await _vottingSessionRepository.GetVottingSessionBySubjectAsync(
                        vote.VottingSessionParserModel.Subject, session.Id);

            if (vottingSession == null)
            {
                vottingSession = new VottingSession
                {
                    Number = vote.VottingSessionParserModel.Number,
                    Subject = vote.VottingSessionParserModel.Subject,
                    SessionId = session.Id
                };

                await _vottingSessionRepository.CreateAsync(vottingSession);
            }

            return vottingSession;
        }

        private async Task<KnownVote> GetKnownVoteAsync(VoteParserModel vote)
        {
            var knownVote = await _knownVoteRepository.GetKnownVoteByVoteAsync(vote.Vote);

            if (knownVote == null)
            {
                knownVote = new KnownVote
                {
                    Vote = vote.Vote
                };

                await _knownVoteRepository.CreateAsync(knownVote);
            }

            return knownVote;
        }
    }
}
