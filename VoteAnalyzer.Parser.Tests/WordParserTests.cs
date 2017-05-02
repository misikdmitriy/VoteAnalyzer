using NUnit.Framework;
using Shouldly;

namespace VoteAnalyzer.Parser.Tests
{
    [TestFixture]
    public class WordParserTests
    {
        private IParser<string, string[]> _wordParser;

        [SetUp]
        public void Setup()
        {
            _wordParser = new WordParser();
        }

        [Test]
        public void ParseShouldReturnSplittedStrings()
        {
            // Arrange
            var input = "E, f, ffff. joke   alala";

            // Act
            var result = _wordParser.Parse(input);

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
            var result = _wordParser.Parse(null);

            // Arrange
            result.Length.ShouldBe(0);
        }
    }
}
