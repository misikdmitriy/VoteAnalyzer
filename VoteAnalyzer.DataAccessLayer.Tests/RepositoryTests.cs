using System;

using NUnit.Framework;

using Shouldly;

using VoteAnalyzer.DataAccessLayer.DbContexts;
using VoteAnalyzer.DataAccessLayer.Entities;
using VoteAnalyzer.DataAccessLayer.Repositories;

namespace VoteAnalyzer.DataAccessLayer.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        private VottingSession _vottingSession;
        private Session _session;
        private IRepository<VottingSession, Guid> Repository { get; set; }

        [SetUp]
        public void Setup()
        {
            Repository = new Repository<VottingSession>();

            _session = new Session
            {
                DateTime = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = "Name"
            };

            _vottingSession = new VottingSession
            {
                SessionId = _session.Id,
                Id = Guid.NewGuid(),
                Subject = "Subject",
                Number = 42
            };

            using (var context = new MainDbContext("MainDbContext"))
            {
                context.Set<Session>().Add(_session);
                context.SaveChanges();
            }
        }

        [Test]
        public void CreateAsyncShouldCreateRecordInDb()
        {
            // Arrange
            // Act
            Repository.CreateAsync(_vottingSession).Wait();

            var result = Repository.ReadAsync(_vottingSession.Id).Result;

            // Assert
            result.Id.ShouldBe(_vottingSession.Id);
            result.Number.ShouldBe(_vottingSession.Number);
            result.Subject.ShouldBe(_vottingSession.Subject);
            result.SessionId.ShouldBe(_vottingSession.SessionId);
        }

        [Test]
        public void CreateAsyncShouldThrowExceptionIfSessionNotExists()
        {
            // Arrange
            _vottingSession.SessionId = Guid.NewGuid();

            // Act
            // Assert
            Should.Throw<Exception>(() => Repository.CreateAsync(_vottingSession).Wait());
        }

        [Test]
        public void ReadAsyncShouldReturnNull()
        {
            // Arrange
            // Act
            var result = Repository.ReadAsync(Guid.NewGuid()).Result;

            // Assert
            result.ShouldBeNull();
        }

        [Test]
        public void ReadAsyncShouldReturnCorrectArray()
        {
            // Arrange
            Repository.CreateAsync(_vottingSession);

            // Act
            var result = Repository.ReadAsync(m => m.Id == _vottingSession.Id).Result;

            // Assert
            result.Length.ShouldBe(1);
            result[0].Number.ShouldBe(_vottingSession.Number);
            result[0].Subject.ShouldBe(_vottingSession.Subject);
        }

        [Test]
        public void UpdateAsyncShouldUpdateModel()
        {
            // Arrange
            const int number = 8;
            const string subject = "real";

            Repository.CreateAsync(_vottingSession).Wait();

            _vottingSession.Number = number;
            _vottingSession.Subject = subject;

            // Act
            Repository.UpdateAsync(_vottingSession).Wait();
            var result = Repository.ReadAsync(_vottingSession.Id).Result;

            // Assert
            result.Id.ShouldBe(_vottingSession.Id);
            result.SessionId.ShouldBe(_vottingSession.SessionId);
            result.Number.ShouldBe(number);
            result.Subject.ShouldBe(subject);
        }

        [Test]
        public void UpdateAsyncShouldThrowExceptionIfEntityNotExist()
        {
            // Arrange
            // Act
            // Assert
            Should.Throw<Exception>(() => Repository.UpdateAsync(new VottingSession()).Wait());
        }

        [Test]
        public void DeleteAsyncShouldDeleteModel()
        {
            // Arrange
            Repository.CreateAsync(_vottingSession).Wait();

            // Act
            Repository.DeleteAsync(_vottingSession.Id).Wait();
            var result = Repository.ReadAsync(_vottingSession.Id).Result;

            // Assert
            result.ShouldBeNull();
        }

        [Test]
        public void DeleteAsyncShouldDoNothingIfEntityNotExists()
        {
            // Arrange
            // Act
            // Assert
            Repository.DeleteAsync(Guid.NewGuid()).Wait();
        }

        [Test]
        public void ReadAllAsyncShouldReturnCorrectArrayOfModels()
        {
            // Arrange
            var initialLength = Repository.ReadAllAsync().Result.Length;
            Repository.CreateAsync(_vottingSession);

            // Act
            var resultLength = Repository.ReadAllAsync().Result.Length;

            // Assert
            resultLength.ShouldBe(initialLength + 1);
        }
    }
}
