using System;
using System.Linq;

using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;

namespace VoteAnalyzer.Parser.Parsers
{
    /// <summary>
    /// Parse session name
    /// Rule: 'Броварська міська рада' [NAME] 'від' [DATE]
    /// </summary>
    public class SessionParser : AbstractParser<ParseInfo, SessionParserModel>
    {
        private readonly IPdfContainer _pdfContainer;

        private readonly string[] _textBefore = { "Броварська", "міська", "рада" };
        private readonly string[] _nameDateSeparator = { "від" };

        public SessionParser(IPdfContainer pdfContainer)
        {
            _pdfContainer = pdfContainer;
        }

        public override SessionParserModel Parse(ParseInfo argument)
        {
            var splitted = _pdfContainer.GetSeparatedWords(argument.FileInfo, argument.Page);

            var index =
                splitted.IndexOfSubsequence(_textBefore);

            var sessionNameStartIndex = index + 3;

            var cutted = splitted.Skip(sessionNameStartIndex).ToArray();

            var indexOfDate =
                cutted.IndexOfSubsequence(_nameDateSeparator) + 1;

            var name = cutted.Take(indexOfDate - 1)
                .Aggregate((current, next) => $"{current} {next}");

            var date = new DateTime(2000 + int.Parse(cutted[indexOfDate + 2]),
                int.Parse(cutted[indexOfDate + 1]),
                int.Parse(cutted[indexOfDate]));

            return new SessionParserModel
            {
                DateTime = date,
                Name = name
            };
        }
    }
}
