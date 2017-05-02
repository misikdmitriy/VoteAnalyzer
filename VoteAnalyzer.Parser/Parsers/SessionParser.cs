using System;
using System.Linq;

using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;

namespace VoteAnalyzer.Parser.Parsers
{
    public class SessionParser : IParser<ParseInfo, SessionParserModel>
    {
        private readonly IPdfContainer _pdfContainer;

        private readonly string[] _textBefore = { "Броварська", "міська", "рада" };
        private const string NameDateSeparator = "від";

        public SessionParser(IPdfContainer pdfContainer)
        {
            _pdfContainer = pdfContainer;
        }

        public SessionParserModel Parse(ParseInfo argument)
        {
            var splitted = _pdfContainer.GetSeparatedWords(argument.FileInfo, argument.Page);

            var index =
                splitted.IndexOfByPredicate(
                    (s, i) => s.Equals(_textBefore[0], StringComparison.InvariantCultureIgnoreCase)
                        && splitted[i + 1].Equals(_textBefore[1], StringComparison.InvariantCultureIgnoreCase)
                        && splitted[i + 2].Equals(_textBefore[2], StringComparison.InvariantCultureIgnoreCase));

            var sessionNameStartIndex = index + 3;

            var cutted = splitted.Skip(sessionNameStartIndex).ToArray();

            var indexOfDate =
                cutted.IndexOfByPredicate(
                    (s, i) => s.Equals(NameDateSeparator, StringComparison.InvariantCultureIgnoreCase)) + 1;

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
