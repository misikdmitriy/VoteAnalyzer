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
        private PageVotesParser _parser;

        [SetUp]
        public void Setup()
        {
            _pdfContainerMock = new Mock<IPdfContainer>();
            _deputyParserMock = new Mock<IParser<ParseInfo, DeputyParserModel[]>>();
            _vottingSessionParserMock = new Mock<IParser<ParseInfo, VottingSessionParserModel>>();

            _parser = new PageVotesParser(_deputyParserMock.Object,
                _pdfContainerMock.Object, _vottingSessionParserMock.Object);
        }

        [Test]
        public void ParseShouldReturnCorrectModel1()
        {
            // Arrange
            _pdfContainerMock.Setup(converter => converter.GetSeparatedWords(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns(new[] { "Результат", "голосування", "Результат", "голосування", "1", "Прізвище", "імя",
                    "по-батьков", "За", "2", "Прізвище", "імя",
                    "по-батькові", "не", "голосував", "Підсумки" });

            _deputyParserMock.Setup(d => d.Parse(It.IsAny<ParseInfo>()))
                .Returns(new[] {new DeputyParserModel(), new DeputyParserModel()});

            var expected = new[]
            {
                new VoteParserModel {Vote = "За"},
                new VoteParserModel {Vote = "не голосував"}
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
            _pdfContainerMock.Setup(converter => converter.GetSeparatedWords(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns(new[] { "Результат", "голосування", "Результат", "голосування", "1", "Прізвище", "імя",
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
