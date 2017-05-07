using System.IO;
using NUnit.Framework;
using Shouldly;
using VoteAnalyzer.PdfIntegration.Models;
using VoteAnalyzer.PdfIntegration.PdfServices;

namespace VoteAnalyzer.PdfIntegration.Tests
{
    [TestFixture]
    public class PdfConverterTests
    {
        private IPdfService _pdfService;
        private readonly PdfFileInfo _parseInfo = new PdfFileInfo
        {
            Directory = @"C:\Path\VoteAnalyzer.Files",
            FileName = @"Результат поіменного голосування_03.04.2017.pdf",
        };

        private readonly PdfFileInfo _uncorrectFileInfo = new PdfFileInfo
        {
            Directory = @"C:\",
            FileName = "123.pdf"
        };

        [SetUp]
        public void Setup()
        {
            _pdfService = new PdfService();
        }

        [Test]
        public void ConvertToTextShouldReturnNotEmptyString()
        {
            // Arrange
            // Act
            var result = _pdfService.GetContent(_parseInfo, 2);

            // Assert
            result.Length.ShouldBePositive();
        }

        [Test]
        public void ConvertToTextShouldThrowException()
        {
            // Arrange
            // Act
            // Assert
            Should.Throw<FileNotFoundException>(() => _pdfService.GetContent(_uncorrectFileInfo, 0));
        }

        [Test]
        public void GetNumberOfPagesShouldReturnCorrectResult()
        {
            // Arrange
            // Act
            var result = _pdfService.GetNumberOfPages(_parseInfo);

            // Assert
            result.ShouldBe(9);
        }

        [Test]
        public void GetNumberOfPagesShouldThrowException()
        {
            // Arrange
            // Act
            // Assert
            Should.Throw<FileNotFoundException>(() => _pdfService.GetNumberOfPages(_uncorrectFileInfo));
        }

        [Test]
        public void ExistsShouldReturnTrueIfFileExists()
        {
            // Arrange
            // Act
            var result = _pdfService.Exists(_parseInfo);

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void ExistsShouldReturnFalseIfFileNotExists()
        {
            // Arrange
            // Act
            var result = _pdfService.Exists(_uncorrectFileInfo);

            // Assert
            result.ShouldBeFalse();
        }
    }
}
