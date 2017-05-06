using System;
using System.Collections.Generic;
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

            var voteModels = await _voteRepository.GetAll();

            foreach (var voteModel in voteModels)
            {
                var deputyTask = _deputiesRepository.ReadAsync(voteModel.DeputyId);
                var vottingSessionTask = _vottingSessionsRepository.ReadAsync(voteModel.VottingSessionId);
                var knownVoteTask = _knownVotesRepository.ReadAsync(voteModel.KnownVoteId);

                await Task.WhenAll(deputyTask, vottingSessionTask, knownVoteTask);

                var session = await _sessionsRepository.ReadAsync(vottingSessionTask.Result.SessionId);

                votes.Add(new VoteDto
                {
                    Vote = knownVoteTask.Result.Vote,
                    DeputyName = deputyTask.Result.Name,
                    SessionName = session.Name,
                    VottingSessionSubject = vottingSessionTask.Result.Subject,
                    VottingSessionNumber = vottingSessionTask.Result.Number,
                    SessionDate = session.DateTime
                });
            }

            return votes.ToArray();
        }
    }
}