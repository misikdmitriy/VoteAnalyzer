using Moq;
using NUnit.Framework;
using Shouldly;

using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.Parser.Parsers;
using VoteAnalyzer.PdfIntegration.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;

namespace VoteAnalyzer.Parser.Tests
{
    [TestFixture]
    public class PageVotesParserTests
    {
        private Mock<IPdfContainer> _pdfContainerMock;
        private Mock<IParser<ParseInfo, DeputyParserModel[]>> _deputyParserMock;
        private Mock<IParser<ParseInfo, VottingSessionParserModel>> _vottingSessionParserMock;
        private Mock<IParser<string[], FirstVoteParserModel>> _firstVoteParserMock;
        private PageVotesParser _parser;

        [SetUp]
        public void Setup()
        {
            _pdfContainerMock = new Mock<IPdfContainer>();
            _deputyParserMock = new Mock<IParser<ParseInfo, DeputyParserModel[]>>();
            _vottingSessionParserMock = new Mock<IParser<ParseInfo, VottingSessionParserModel>>();
            _firstVoteParserMock = new Mock<IParser<string[], FirstVoteParserModel>>();

            _parser = new PageVotesParser(_deputyParserMock.Object,
                _pdfContainerMock.Object, _vottingSessionParserMock.Object,
                _firstVoteParserMock.Object);
        }

        [Test]
        public void ParseShouldReturnCorrectModel1()
        {
            // Arrange
            var firstVoteParserResults = new[] { "За", "Не голосував" };
            var index = 0;

            _firstVoteParserMock.Setup(p => p.Parse(It.IsAny<string[]>()))
                .Returns(() => new FirstVoteParserModel { Vote = firstVoteParserResults[index++] });

            _pdfContainerMock.Setup(converter => converter.GetSeparatedWords(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns(new[] { "п/п", "по-батькові", "депутата", "1", "Прізвище", "імя",
                    "по-батьков", "За", "2", "Прізвище", "імя",
                    "по-батькові", "не", "голосував", "Підсумки" });

            _deputyParserMock.Setup(d => d.Parse(It.IsAny<ParseInfo>()))
                .Returns(new[] {new DeputyParserModel(), new DeputyParserModel()});

            var expected = new[]
            {
                new VoteParserModel {Vote = "За"},
                new VoteParserModel {Vote = "Не голосував"}
            };

            // Act
            var result = _parser.Parse(new ParseInfo());

            // Assert
            result.Length.ShouldBe(2);
            result[0].Vote.ShouldBe(expected[0].Vote);
            result[1].Vote.ShouldBe(expected[1].Vote);
        }

        [Test]
        public void ParseShouldReturnCorrectModel2()
        {
            // Arrange
            _firstVoteParserMock.Setup(p => p.Parse(It.IsAny<string[]>()))
                .Returns(new FirstVoteParserModel { Vote = "За" });

            _pdfContainerMock.Setup(converter => converter.GetSeparatedWords(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns(new[] { "п/п", "по-батькові", "депутата" , "1", "Прізвище", "імя",
                    "по-батьков", "За", "Підсумки" });

            _deputyParserMock.Setup(d => d.Parse(It.IsAny<ParseInfo>()))
                .Returns(new[] { new DeputyParserModel() });

            var expected = new[]
            {
                new VoteParserModel {Vote = "За"}
            };

            // Act
            var result = _parser.Parse(new ParseInfo());

            // Assert
            result.Length.ShouldBe(1);
            result[0].Vote.ShouldBe(expected[0].Vote);
        }
    }
}
