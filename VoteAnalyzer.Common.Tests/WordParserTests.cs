using NUnit.Framework;
using Shouldly;

namespace VoteAnalyzer.Common.Tests
{
    [TestFixture]
    public class WordParserTests
    {
        [Test]
        public void ParseShouldReturnSplittedStrings()
        {
            // Arrange
            var input = "E, f, ffff. joke   alala";

            // Act
            var result = WordSeparator.Split(input);

            // Assert
            result[0].ShouldBe("E");
            result[1].ShouldBe("f");
            result[2].ShouldBe("ffff");
            result[3].ShouldBe("joke");
            result[4].ShouldBe("alala");
        }

        [Test]
        public void ParseShouldReturnEmptyArray()
        {
            // Arrange
            // Act
            var result = WordSeparator.Split(null);

            // Arrange
            result.Length.ShouldBe(0);
        }
    }
}
