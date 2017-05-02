using Moq;
using NUnit.Framework;
using Shouldly;

using VoteAnalyzer.DataAccessLayer.Entities;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.Parser.Parsers;
using VoteAnalyzer.PdfIntegration.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;

namespace VoteAnalyzer.Parser.Tests
{
    public class VottingSessionParserTests
    {
        private VottingSessionParser _parser;
        private Mock<IPdfContainer> _pdfContainerMock;
        private Mock<IParser<ParseInfo, SessionParserModel>> _sessionParserMock;

        [SetUp]
        public void Setup()
        {
            _pdfContainerMock = new Mock<IPdfContainer>();
            _sessionParserMock = new Mock<IParser<ParseInfo, SessionParserModel>>();
            _parser = new VottingSessionParser(_pdfContainerMock.Object, 
                _sessionParserMock.Object);
        }

        [Test]
        public void ParseShouldReturnCorrectVottingSession1()
        {
            // Arrange
            _pdfContainerMock.Setup(converter => converter.GetSeparatedWords(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns(new[] { "Броварня", "тирипири", "№", "10", "За", "Основу", "№", "тарапапам" });

            var expected = new VottingSession
            {
                Number = 10,
                Subject = "За Основу"
            };

            // Act
            var result = _parser.Parse(new ParseInfo());

            // Assert
            result.Number.ShouldBe(expected.Number);
            result.Subject.ShouldBe(expected.Subject);
        }

        [Test]
        public void ParseShouldReturnCorrectVottingSession2()
        {
            // Arrange
            _pdfContainerMock.Setup(converter => converter.GetSeparatedWords(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns(new[] { "№", "бн", "Просто", "№" });

            var expected = new VottingSession
            {
                Number = null,
                Subject = "Просто"
            };

            // Act
            var result = _parser.Parse(new ParseInfo());

            // Assert
            result.Number.ShouldBe(expected.Number);
            result.Subject.ShouldBe(expected.Subject);
        }
    }
}
