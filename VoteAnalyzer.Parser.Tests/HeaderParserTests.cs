using System;
using Moq;
using NUnit.Framework;
using Shouldly;
using VoteAnalyzer.Common;
using VoteAnalyzer.Common.Models;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.Parser.Tests
{
    [TestFixture]
    public class HeaderParserTests
    {
        private HeaderParser _parser;
        private Mock<IPdfConverter> _pdfConverterMock;

        [SetUp]
        public void Setup()
        {
            _pdfConverterMock = new Mock<IPdfConverter>();
            _parser = new HeaderParser(_pdfConverterMock.Object);
        }

        [Test]
        public void ParseShouldReturnCorrectSession1()
        {
            // Arrange
            _pdfConverterMock.Setup(converter => converter.ConvertToText(It.IsAny<ParseInfo>()))
                .Returns("Броварська міська рада 28 засідання 05.02.16");

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
            _pdfConverterMock.Setup(converter => converter.ConvertToText(It.IsAny<ParseInfo>()))
                .Returns("Броварська міська рада 31 засідання від 15.06.15 і таке інше");

            var expected = new Session
            {
                DateTime = new DateTime(2015, 6, 15),
                Name = "31 засідання від"
            };

            // Act
            var result = _parser.Parse(new ParseInfo());

            // Assert
            result.Name.ShouldBe(expected.Name);
            result.DateTime.ShouldBe(expected.DateTime);
        }
    }
}
