using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using VoteAnalyzer.DataAccessLayer.Entities;
using VoteAnalyzer.DataAccessLayer.Repositories;
using VoteAnalyzer.WebApi.Models;

namespace VoteAnalyzer.WebApi.MediatrRequests
{
    public class GetVotesRequest : IRequest<VoteDto[]>
    {
    }

    public class GetVotesHandler : IAsyncRequestHandler<GetVotesRequest, VoteDto[]>
    {
        private readonly IRepository<Deputy, Guid> _deputiesRepository;
        private readonly IRepository<VottingSession, Guid> _vottingSessionsRepository;
        private readonly IRepository<Session, Guid> _sessionsRepository;
        private readonly IRepository<Vote, Guid> _voteRepository;
        private readonly IRepository<KnownVote, Guid> _knownVotesRepository;

        private readonly IList<Deputy> _deputiesCache = new List<Deputy>();
        private readonly IList<VottingSession> _vottingSessionsCache = new List<VottingSession>();
        private readonly IList<Session> _sessionsCache = new List<Session>();
        private readonly IList<KnownVote> _knownVotesCache = new List<KnownVote>();

        public GetVotesHandler(IRepository<Deputy, Guid> deputiesRepository,
            IRepository<KnownVote, Guid> knownVotesRepository,
            IRepository<Session, Guid> sessionsRepository,
            IRepository<Vote, Guid> voteRepository,
            IRepository<VottingSession, Guid> vottingSessionsRepository)
        {
            _deputiesRepository = deputiesRepository;
            _knownVotesRepository = knownVotesRepository;
            _sessionsRepository = sessionsRepository;
            _voteRepository = voteRepository;
            _vottingSessionsRepository = vottingSessionsRepository;
        }

        public async Task<VoteDto[]> Handle(GetVotesRequest message)
        {
            var votes = new List<VoteDto>();

            var voteModels = await _voteRepository.ReadAllAsync();

            foreach (var voteModel in voteModels)
            {
                var deputyTask = _deputiesCache.Any(d => d.Id == voteModel.DeputyId) 
                    ? Task.FromResult(_deputiesCache.First(d => d.Id == voteModel.DeputyId)) 
                    : _deputiesRepository.ReadAsync(voteModel.DeputyId);

                var vottingSessionTask = _vottingSessionsCache.Any(v => v.Id == voteModel.VottingSessionId)
                    ? Task.FromResult(_vottingSessionsCache.First(v => v.Id == voteModel.VottingSessionId))
                    : _vottingSessionsRepository.ReadAsync(voteModel.VottingSessionId);

                var knownVoteTask = _knownVotesCache.Any(v => v.Id == voteModel.KnownVoteId)
                    ? Task.FromResult(_knownVotesCache.First(v => v.Id == voteModel.KnownVoteId))
                    : _knownVotesRepository.ReadAsync(voteModel.KnownVoteId);

                await Task.WhenAll(deputyTask, vottingSessionTask, knownVoteTask);

                var session = _sessionsCache.FirstOrDefault(s => s.Id == vottingSessionTask.Result.SessionId) 
                    ?? await _sessionsRepository.ReadAsync(vottingSessionTask.Result.SessionId);

                votes.Add(new VoteDto
                {
                    Vote = knownVoteTask.Result.Vote,
                    DeputyName = deputyTask.Result.Name,
                    SessionName = session.Name,
                    VottingSessionSubject = vottingSessionTask.Result.Subject,
                    VottingSessionNumber = vottingSessionTask.Result.Number,
                    SessionDate = session.DateTime
                });

                AddToCache(deputyTask.Result, vottingSessionTask.Result, knownVoteTask.Result, session);
            }

            ClearCache();

            return votes.ToArray();
        }

        private void ClearCache()
        {
            _deputiesCache.Clear();
            _vottingSessionsCache.Clear();
            _knownVotesCache.Clear();
            _sessionsCache.Clear();
        }

        private void AddToCache(Deputy deputy, VottingSession vottingSession, 
            KnownVote knownVote, Session session)
        {
            if (_deputiesCache.All(d => d.Id != deputy.Id))
            {
                _deputiesCache.Add(deputy);
            }
            if (_vottingSessionsCache.All(v => v.Id != vottingSession.Id))
            {
                _vottingSessionsCache.Add(vottingSession);
            }
            if (_knownVotesCache.All(k => k.Id != knownVote.Id))
            {
                _knownVotesCache.Add(knownVote);
            }
            if (_sessionsCache.All(s => s.Id != session.Id))
            {
                _sessionsCache.Add(session);
            }
        }
    }
}