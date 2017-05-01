using NUnit.Framework;
using VoteAnalyzer.Common.Models;

namespace VoteAnalyzer.Common.Tests
{
    [TestFixture]
    public class PdfConverterTests
    {
        private PdfConverter _pdfConverter;

        [SetUp]
        public void Setup()
        {
            _pdfConverter = new PdfConverter();
        }

        [Test]
        public void ConvertToTextShouldReturnNotEmptyString()
        {
            // Arrange
            var parseInfo = new ParseInfo
            {
                Directory = @"C:\Users\mad_b\Documents\visual studio 2017\Projects\VoteAnalyzer\VoteAnalyzer.Files",
                FileName = @"Результат поіменного голосування_03.04.2017.pdf",
                Page = 1
            };

            // Act
            var result = _pdfConverter.ConvertToText(parseInfo);

            // Assert
        }
    }
}
