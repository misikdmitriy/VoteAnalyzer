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

        [Test]
        public void IndexOfSubsequenceShouldReturnCorrectResult()
        {
            // Arrange
            var sequence = new[] {"a", "B", "c", "d"};
            var subsequence = new[] {"b", "C"};

            // Act
            var result = sequence.IndexOfSubsequence(subsequence);

            // Assert
            result.ShouldBe(1);
        }

        [Test]
        public void LastIndexOfSubsequenceShouldReturnCorrectResult()
        {
            // Arrange
            var sequence = new[] { "a", "B", "c", "d", "b", "c" };
            var subsequence = new[] { "b", "C" };

            // Act
            var result = sequence.LastIndexOfSubsequence(subsequence);

            // Assert
            result.ShouldBe(4);
        }
    }
}
