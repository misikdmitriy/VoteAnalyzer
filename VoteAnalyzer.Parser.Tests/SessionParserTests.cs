using System;
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
    [TestFixture]
    public class SessionParserTests
    {
        private SessionParser _parser;
        private Mock<IPdfContainer> _pdfContainerMock;

        [SetUp]
        public void Setup()
        {
            _pdfContainerMock = new Mock<IPdfContainer>();
            _parser = new SessionParser(_pdfContainerMock.Object);
        }

        [Test]
        public void ParseShouldReturnCorrectSession1()
        {
            // Arrange
            _pdfContainerMock.Setup(converter => converter.GetSeparatedWords(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns(new[] { "Броварська", "міська", "рада", "28", "засідання", "від", "05", "02", "16" });

            var expected = new Session
            {
                DateTime = new DateTime(2016, 2, 5),
                Name = "28 засідання"
            };

            // Act
            var result = _parser.Parse(new ParseInfo());

            // Assert
            result.Name.ShouldBe(expected.Name);
            result.DateTime.ShouldBe(expected.DateTime);
        }

        [Test]
        public void ParseShouldReturnCorrectSession2()
        {
            // Arrange
            _pdfContainerMock.Setup(converter => converter.GetSeparatedWords(It.IsAny<PdfFileInfo>(), It.IsAny<int>()))
                .Returns(new[]
                {
                    "Броварська", "міська", "рада", "31", "засідання", "від", "15", "06", "15",
                    "і", "таке", "інше"
                });

            var expected = new Session
            {
                DateTime = new DateTime(2015, 6, 15),
                Name = "31 засідання"
            };

            // Act
            var result = _parser.Parse(new ParseInfo());

            // Assert
            result.Name.ShouldBe(expected.Name);
            result.DateTime.ShouldBe(expected.DateTime);
        }
    }
}
