using System;
using System.Linq;
using System.Threading.Tasks;

using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.DataAccessLayer.Repositories
{
    public static class RepositoryExtensions
    {
        public static async Task<Deputy> GetDeputyByNameAsync(this IRepository<Deputy, Guid> repository, string name)
        {
            return (await repository.ReadAsync(d => d.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                .FirstOrDefault();
        }

        public static async Task<Session> GetSessionByNameAsync(this IRepository<Session, Guid> repository, string name)
        {
            return (await repository.ReadAsync(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                .FirstOrDefault();
        }

        public static async Task<VottingSession> GetVottingSessionBySubjectAsync(
            this IRepository<VottingSession, Guid> repository, string subject, Guid sessionId)
        {
            return (await repository.ReadAsync(s => s.SessionId == sessionId && s.Subject.Equals(subject, StringComparison.InvariantCultureIgnoreCase)))
                .FirstOrDefault();
        }

        public static async Task<KnownVote> GetKnownVoteByVoteAsync(this IRepository<KnownVote, Guid> repository,
            string vote)
        {
            return (await repository.ReadAsync(s => s.Vote.Equals(vote, StringComparison.InvariantCultureIgnoreCase)))
                .FirstOrDefault();
        }

        public static async Task<bool> IsVoteExistsAsync(this IRepository<Vote, Guid> repository, Guid deputyId,
            Guid vottingSessionId)
        {
            return (await repository.ReadAsync(v => v.DeputyId == deputyId && v.VottingSessionId == vottingSessionId))
                .Any();
        }
    }
}
