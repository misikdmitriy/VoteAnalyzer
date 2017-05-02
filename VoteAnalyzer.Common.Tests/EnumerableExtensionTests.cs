using NUnit.Framework;
using Shouldly;
using VoteAnalyzer.Common.Extensions;

namespace VoteAnalyzer.Common.Tests
{
    [TestFixture]
    public class EnumerableExtensionTests
    {
        [Test]
        public void IndexOfByPredicateShouldReturnCorrectIndex()
        {
            // Arrange
            var array = new[] {1, 2, 3, 4, 5, 4};

            // Act
            var result = array.IndexOfByPredicate((i, i1) => i == 4);

            // Assert
            result.ShouldBe(3);
        }

        [Test]
        public void LastIndexOfByPredicateShouldReturnCorrectIndex()
        {
            // Arrange
            var array = new[] { 1, 2, 3, 4, 5, 4 };

            // Act
            var result = array.LastIndexOfByPredicate((i, i1) => i == 4);

            // Assert
            result.ShouldBe(5);
        }
    }
}
