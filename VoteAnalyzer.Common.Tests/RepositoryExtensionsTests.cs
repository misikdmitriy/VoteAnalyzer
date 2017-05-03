using System;
using Moq;
using NUnit.Framework;
using Shouldly;

using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.DataAccessLayer.Entities;
using VoteAnalyzer.DataAccessLayer.Repositories;

namespace VoteAnalyzer.Common.Tests
{
    [TestFixture]
    public class RepositoryExtensionsTests
    {
        [Test]
        public void GetDeputyByNameAsyncShouldReturnFirstDeputy()
        {
            // Arrange
            var deputies = new[]
            {
                new Deputy {Id = Guid.NewGuid(), Name = "N1"},
                new Deputy {Name = "N2"},
                new Deputy {Id = Guid.NewGuid(), Name = "N1"},
                new Deputy {Name = "N3"}
            };

            var deputyRepoMock = new Mock<IRepository<Deputy, Guid>>();
            deputyRepoMock.Setup(d => d.ReadAsync(It.IsAny<Func<Deputy, bool>>()))
                .ReturnsAsync(deputies);

            // Act
            var result = deputyRepoMock.Object.GetDeputyByNameAsync("n1").Result;

            // Assert
            result.Name.ShouldBe("N1");
            result.Id.ShouldBe(deputies[0].Id);
        }

        [Test]
        public void GetSessionByNameAsyncShouldReturnFirstSession()
        {
            // Arrange
            var sessions = new[]
            {
                new Session {Id = Guid.NewGuid(), Name = "N1"},
                new Session {Name = "N2"},
                new Session {Id = Guid.NewGuid(), Name = "N1"},
                new Session {Name = "N3"}
            };

            var sessionRepoMock = new Mock<IRepository<Session, Guid>>();
            sessionRepoMock.Setup(s => s.ReadAsync(It.IsAny<Func<Session, bool>>()))
                .ReturnsAsync(sessions);

            // Act
            var result = sessionRepoMock.Object.GetSessionByNameAsync("n1").Result;

            // Assert
            result.Name.ShouldBe("N1");
            result.Id.ShouldBe(sessions[0].Id);
        }

        [Test]
        public void GetVottingSessionBySubjectAsyncShouldReturnFirstVottingSession()
        {
            // Arrange
            var vottingSessions = new[]
            {
                new VottingSession {Id = Guid.NewGuid(), Subject = "N1", SessionId = Guid.NewGuid()},
                new VottingSession {Subject = "N2"},
                new VottingSession {Id = Guid.NewGuid(), Subject = "N1", SessionId = Guid.NewGuid()},
                new VottingSession {Subject = "N3"}
            };

            var vottingSessionRepoMock = new Mock<IRepository<VottingSession, Guid>>();
            vottingSessionRepoMock.Setup(v => v.ReadAsync(It.IsAny<Func<VottingSession, bool>>()))
                .ReturnsAsync(vottingSessions);

            // Act
            var result = vottingSessionRepoMock.Object.GetVottingSessionBySubjectAsync("n1",
                vottingSessions[0].SessionId).Result;

            // Assert
            result.Subject.ShouldBe(vottingSessions[0].Subject);
            result.Id.ShouldBe(vottingSessions[0].Id);
            result.SessionId.ShouldBe(vottingSessions[0].SessionId);
        }

        [Test]
        public void GetKnownVoteByVoteAsyncShouldReturnFirstKnownVote()
        {
            // Arrange
            var knownVotes = new[]
            {
                new KnownVote {Id = Guid.NewGuid(), Vote = "V1"},
                new KnownVote {Vote = "V2"},
                new KnownVote {Id = Guid.NewGuid(), Vote = "V1"},
                new KnownVote {Vote = "V3"},
            };

            var knownVoteRepoMock = new Mock<IRepository<KnownVote, Guid>>();
            knownVoteRepoMock.Setup(k => k.ReadAsync(It.IsAny<Func<KnownVote, bool>>()))
                .ReturnsAsync(knownVotes);

            // Act
            var result = knownVoteRepoMock.Object.GetKnownVoteByVoteAsync("v1").Result;

            // Assert
            result.Vote.ShouldBe(knownVotes[0].Vote);
            result.Id.ShouldBe(knownVotes[0].Id);
        }

        [Test]
        public void IsVoteExistsAsyncShouldReturnTrue()
        {
            // Arrange
            var votes = new[]
            {
                new Vote {DeputyId = Guid.NewGuid(), VottingSessionId = Guid.NewGuid()}
            };

            var votesRepoMock = new Mock<IRepository<Vote, Guid>>();
            votesRepoMock.Setup(r => r.ReadAsync(It.IsAny<Func<Vote, bool>>()))
                .ReturnsAsync(votes);

            // Act
            var result = votesRepoMock.Object.IsVoteExistsAsync(votes[0].DeputyId, votes[0].VottingSessionId).Result;

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void IsVoteExistsAsyncShouldReturnFalse()
        {
            // Arrange
            var votesRepoMock = new Mock<IRepository<Vote, Guid>>();
            votesRepoMock.Setup(r => r.ReadAsync(It.IsAny<Func<Vote, bool>>()))
                .ReturnsAsync(new Vote[0]);

            // Act
            var result = votesRepoMock.Object.IsVoteExistsAsync(Guid.NewGuid(), Guid.NewGuid()).Result;

            // Assert
            result.ShouldBeFalse();
        }
    }
}
