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
    public class GetInfluenceZoneRequest : IRequest<DeputyInfluenceDto[]>
    {
        public string CenterDeputyName { get; set; }

        public GetInfluenceZoneRequest(string centerDeputyName)
        {
            CenterDeputyName = centerDeputyName;
        }
    }

    public class GetInfluenceZoneRequestHandler : IAsyncRequestHandler<GetInfluenceZoneRequest, DeputyInfluenceDto[]>
    {
        private readonly IRepository<Deputy, Guid> _deputiesRepository;
        private readonly IRepository<Vote, Guid> _votesRepository;
        private readonly IRepository<KnownVote, Guid> _knowVotesRepository;

        private readonly string[] _votes = new[] { "За", "Проти" };

        public GetInfluenceZoneRequestHandler(IRepository<Deputy, Guid> deputiesRepository,
            IRepository<Vote, Guid> votesRepository, IRepository<KnownVote, Guid> knowVotesRepository)
        {
            _deputiesRepository = deputiesRepository;
            _votesRepository = votesRepository;
            _knowVotesRepository = knowVotesRepository;
        }

        public async Task<DeputyInfluenceDto[]> Handle(GetInfluenceZoneRequest message)
        {
            var result = new List<DeputyInfluenceDto>();

            var centerDeputy = await _deputiesRepository.GetDeputyByNameAsync(message.CenterDeputyName);

            var deputies = await _deputiesRepository.ReadAllAsync();
            var votesToCount = await Task.WhenAll(_votes
                .Select(v => _knowVotesRepository.GetKnownVoteByVoteAsync(v)));

            votesToCount = votesToCount.Where(r => r != null).ToArray();

            var centerDeputyVotes = await _votesRepository.ReadAsync(v => v.DeputyId == centerDeputy.Id
                                                               && votesToCount.Any(v1 => v1.Id == v.KnownVoteId));

            foreach (var deputy in deputies)
            {
                if (deputy.Id == centerDeputy.Id)
                {
                    continue;
                }

                var allVotes = await _votesRepository.ReadAsync(v => v.DeputyId == deputy.Id
                                                               && votesToCount.Any(v1 => v1.Id == v.KnownVoteId));

                if (allVotes.Length == 0)
                {
                    result.Add(new DeputyInfluenceDto
                    {
                        DeputyName = deputy.Name,
                        Influence = 0
                    });

                    continue;
                }

                var equalVotesCount = allVotes.Count(v => centerDeputyVotes.Any(
                                            v1 => v1.KnownVoteId == v.KnownVoteId
                                                    && v1.VottingSessionId == v.VottingSessionId));

                result.Add(new DeputyInfluenceDto
                {
                    DeputyName = deputy.Name,
                    Influence = (double)equalVotesCount / allVotes.Length
                });
            }

            return result.ToArray();
        }
    }
}