using Moq;
using NUnit.Framework;
using Shouldly;

using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.Parser.Parsers;
using VoteAnalyzer.PdfIntegration.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;
using VoteAnalyzer.Tests.Common;

namespace VoteAnalyzer.Parser.Tests
{
    [TestFixture]
    public class DeputiesParserTests
    {
        private Mock<IPdfContainer> _pdfContainerMock;
        private Mock<IParser<string[], FirstVoteParserModel>> _firstVoteParserMock;
        private DeputiesParser _parser;

        [SetUp]
        public void Setup()
        {
            _pdfContainerMock = new Mock<IPdfContainer>();
            _firstVoteParserMock = new Mock<IParser<string[], FirstVoteParserModel>>();

            _parser = new DeputiesParser(_pdfContainerMock.Object, _firstVoteParserMock.Object);
        }

        [Test]
        public void ParseShouldReturnCorrectModel1()
        {
            // Arrange
            var firstVoteParserResults = new[] { "За", "Не голосував" };
            var index = 0;
            var outModel = It.IsAny<FirstVoteParserModel>();

            _firstVoteParserMock.Setup(p => p.TryParse(It.IsAny<string[]>(), out outModel))
                .OutCallback((string[] strs, out FirstVoteParserModel model) =>
                {
                    model = index < 2 
                        ? new FirstVoteParserModel {Vote = firstVoteParserResults[index]} 
                        : null;
                    index++;
                })
                .Returns(() => index < 3);

            _pdfContainerMock.Setup(converter => converter.GetSeparatedWords(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns(new[] {  "п/п", "по-батькові", "депутата", "1", "Прізвище", "імя",
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
            FirstVoteParserModel outModel;
            var index = 0;
            _firstVoteParserMock.Setup(p => p.TryParse(It.IsAny<string[]>(), out outModel))
                .OutCallback((string[] strs, out FirstVoteParserModel model) =>
                {
                    model = index < 1 
                        ? new FirstVoteParserModel {Vote = "За"} 
                        : null;
                    index++;
                })
                .Returns(() => index < 2);

            _firstVoteParserMock.Setup(p => p.Parse(It.IsAny<string[]>()))
                .Returns(new FirstVoteParserModel { Vote = "За" });

            _pdfContainerMock.Setup(converter => converter.GetSeparatedWords(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns(new[] {  "п/п", "по-батькові", "депутата", "1", "Прізвище", "імя",
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
