using System;
using NUnit.Framework;
using Shouldly;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.Common.Tests
{
    [TestFixture]
    public class TranslatorTests
    {
        [Test]
        public void ToKnownVoteActionShouldReturnAction()
        {
            // Arrange
            var action = "Відсутній";

            // Act
            var result = action.ToKnownVoteAction();

            // Assert
            result.ShouldBe(VoteAction.Absent);
        }

        [Test]
        public void ToKnownVoteActionShouldThrowException()
        {
            // Arrange
            var action = "afenkjwerhuwch43";

            // Act
            Should.Throw<ArgumentException>(() => action.ToKnownVoteAction());
        }
    }
}
