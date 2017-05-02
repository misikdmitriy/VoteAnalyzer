using Moq;
using NUnit.Framework;
using VoteAnalyzer.PdfIntegration.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;
using VoteAnalyzer.PdfIntegration.PdfServices;

namespace VoteAnalyzer.PdfIntegration.Tests
{
    [TestFixture]
    public class PdfContainerTests
    {
        // TODO: FINISH TESTS
        private Mock<IPdfService> _pdfServiceMock;
        private IPdfContainer _pdfContainer;

        [SetUp]
        public void Setup()
        {
            _pdfServiceMock = new Mock<IPdfService>();
            _pdfContainer = new PdfContainer(_pdfServiceMock.Object);

            _pdfServiceMock.Setup(p => p.Exists(It.IsAny<PdfFileInfo>()))
                .Returns(true);

            _pdfServiceMock.Setup(p => p.GetNumberOfPages(It.IsAny<PdfFileInfo>()))
                .Returns(3);

            _pdfServiceMock.Setup(p => p.ConvertToText(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns("Words can be long");
        }

        [Test]
        public void FAIL()
        {
            Assert.Fail();
        }
    }
}
