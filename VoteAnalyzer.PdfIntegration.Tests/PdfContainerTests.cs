using Moq;
using NUnit.Framework;
using Shouldly;
using VoteAnalyzer.PdfIntegration.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;
using VoteAnalyzer.PdfIntegration.PdfServices;

namespace VoteAnalyzer.PdfIntegration.Tests
{
    [TestFixture]
    public class PdfContainerTests
    {
        private Mock<IPdfService> _pdfServiceMock;
        private IPdfContainer _pdfContainer;

        private PdfFileInfo _pdfFileInfo = new PdfFileInfo
        {
            Directory = "Directory",
            FileName = "Filename"
        };

        [SetUp]
        public void Setup()
        {
            _pdfServiceMock = new Mock<IPdfService>();
            _pdfContainer = new PdfContainer(_pdfServiceMock.Object);

            _pdfServiceMock.Setup(p => p.Exists(It.IsAny<PdfFileInfo>()))
                .Returns(true);

            _pdfServiceMock.Setup(p => p.GetNumberOfPages(It.IsAny<PdfFileInfo>()))
                .Returns(3);

            _pdfServiceMock.Setup(p => p.GetContent(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns("Words can be long");
        }

        [Test]
        public void GetSeparatedWordsShouldReturnSplittedArray()
        {
            // Arrange
            var expected = new[] {"Words", "can", "be", "long"};

            // Act
            var result = _pdfContainer.GetSeparatedWords(_pdfFileInfo, 1);

            // Assert
            result.Length.ShouldBe(expected.Length);
            result[0].ShouldBe(expected[0]);
            result[1].ShouldBe(expected[1]);
            result[2].ShouldBe(expected[2]);
        }

        [Test]
        public void GetContentShouldDoIt()
        {
            // Arrange
            var expected = "Words can be long";

            // Act
            var result = _pdfContainer.GetContent(_pdfFileInfo, 1);

            // Assert
            result.ShouldBe(expected);
        }
    }
}
