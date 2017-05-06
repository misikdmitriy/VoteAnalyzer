using System;
using System.Threading.Tasks;
using VoteAnalyzer.DataAccessLayer.Entities;
using VoteAnalyzer.DataAccessLayer.Repositories;

namespace VoteAnalyzer.DataAccessLayer.Factories
{
    public class EntitiesFactory : IEntitiesFactory
    {
        private readonly IRepository<Deputy, Guid> _deputiesRepository;
        private readonly IRepository<KnownVote, Guid> _knownVoteRepository;
        private readonly IRepository<Session, Guid> _sessionRepository;
        private readonly IRepository<VottingSession, Guid> _vottingSessionRepository;

        public EntitiesFactory(IRepository<Deputy, Guid> deputiesRepository, 
            IRepository<KnownVote, Guid> knownVoteRepository, 
            IRepository<Session, Guid> sessionRepository, 
            IRepository<VottingSession, Guid> vottingSessionRepository)
        {
            _deputiesRepository = deputiesRepository;
            _knownVoteRepository = knownVoteRepository;
            _sessionRepository = sessionRepository;
            _vottingSessionRepository = vottingSessionRepository;
        }

        public async Task<Deputy> CreateDeputyAsync(string name)
        {
            var deputy = await _deputiesRepository.GetDeputyByNameAsync(name);

            if (deputy == null)
            {
                deputy = new Deputy
                {
                    Name = name
                };

                await _deputiesRepository.CreateAsync(deputy);
            }

            return deputy;
        }

        public async Task<Session> CreateSessionAsync(string name, DateTime dateTime = default(DateTime))
        {
            var session = await _sessionRepository.GetSessionByNameAsync(name);

            if (session == null)
            {
                session = new Session
                {
                    Name = name,
                    DateTime = dateTime
                };

                await _sessionRepository.CreateAsync(session);
            }

            return session;
        }

        public async Task<VottingSession> CreateVottingSessionAsync(Guid sessionId, int? number = null,
            string subject = null)
        {
            var vottingSession =
                    await _vottingSessionRepository.GetVottingSessionBySubjectAndNumberAsync(subject, number, sessionId);

            if (vottingSession == null)
            {
                vottingSession = new VottingSession
                {
                    Number = number,
                    Subject = subject,
                    SessionId = sessionId
                };

                await _vottingSessionRepository.CreateAsync(vottingSession);
            }

            return vottingSession;
        }

        public async Task<KnownVote> CreateKnownVoteAsync(string vote)
        {
            var knownVote = await _knownVoteRepository.GetKnownVoteByVoteAsync(vote);

            if (knownVote == null)
            {
                knownVote = new KnownVote
                {
                    Vote = vote
                };

                await _knownVoteRepository.CreateAsync(knownVote);
            }

            return knownVote;
        }
    }
}
