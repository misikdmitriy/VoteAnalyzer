using System;
using System.Threading.Tasks;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.DataAccessLayer.Factories
{
    public interface IEntitiesFactory
    {
        Task<Deputy> CreateDeputyAsync(string name);
        Task<KnownVote> CreateKnownVoteAsync(string vote);
        Task<Session> CreateSessionAsync(string name, DateTime dateTime = default(DateTime));
        Task<VottingSession> CreateVottingSessionAsync(Guid sessionId, int? number = default(int?), string subject = null);
    }
}