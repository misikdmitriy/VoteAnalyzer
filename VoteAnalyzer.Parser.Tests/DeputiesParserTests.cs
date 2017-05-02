using Moq;
using NUnit.Framework;
using Shouldly;

using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.Parser.Parsers;
using VoteAnalyzer.PdfIntegration.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;
using VoteAnalyzer.PdfIntegration.PdfServices;

namespace VoteAnalyzer.Parser.Tests
{
    [TestFixture]
    public class DeputiesParserTests
    {
        private Mock<IPdfContainer> _pdfContainerMock;
        private DeputiesParser _parser;

        [SetUp]
        public void Setup()
        {
            _pdfContainerMock = new Mock<IPdfContainer>();

            _parser = new DeputiesParser(_pdfContainerMock.Object);
        }

        [Test]
        public void ParseShouldReturnCorrectModel1()
        {
            // Arrange
            _pdfContainerMock.Setup(converter => converter.GetSeparatedWords(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns(new[] { "Результат", "голосування", "Результат", "голосування", "1", "Прізвище", "імя",
                    "по-батьков", "За", "2", "Прізвище", "імя",
                    "по-батькові", "не", "голосував", "Підсумки" });

            var expected = new[]
            {
                new DeputyParserModel {Name = "Прізвище імя по-батьков"},
                new DeputyParserModel {Name = "Прізвище імя по-батькові"}
            };

            // Act
            var result = _parser.Parse(new ParseInfo());

            // Assert
            result.Length.ShouldBe(2);
            result[0].Name.ShouldBe(expected[0].Name);
            result[1].Name.ShouldBe(expected[1].Name);
        }

        [Test]
        public void ParseShouldReturnCorrectModel2()
        {
            // Arrange
            _pdfContainerMock.Setup(converter => converter.GetSeparatedWords(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns(new[] { "Результат", "голосування", "Результат", "голосування", "1", "Прізвище", "імя",
                    "по-батьков", "За", "Підсумки" });

            var expected = new[]
            {
                new DeputyParserModel {Name = "Прізвище імя по-батьков"},
            };

            // Act
            var result = _parser.Parse(new ParseInfo());

            // Assert
            result.Length.ShouldBe(1);
            result[0].Name.ShouldBe(expected[0].Name);
        }
    }
}
