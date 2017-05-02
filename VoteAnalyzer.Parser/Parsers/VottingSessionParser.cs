using System.Linq;

using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;

namespace VoteAnalyzer.Parser.Parsers
{
    /// <summary>
    /// Parse votting session
    /// Rule: votting session start with symbol '№': [NUMBER] [SESSION SUBJECT] '№'
    /// </summary>
    public class VottingSessionParser : IParser<ParseInfo, VottingSessionParserModel>
    {
        private readonly IPdfContainer _pdfContainer;
        private readonly IParser<ParseInfo, SessionParserModel> _sessionParser;

        public VottingSessionParser(IPdfContainer pdfContainer,
            IParser<ParseInfo, SessionParserModel> sessionParser)
        {
            _pdfContainer = pdfContainer;
            _sessionParser = sessionParser;
        }

        public VottingSessionParserModel Parse(ParseInfo argument)
        {
            var splitted = _pdfContainer.GetSeparatedWords(argument.FileInfo, argument.Page);

            var startIndex = splitted.IndexOfByPredicate((s, i) => s == "№") + 1;

            var finishIndex = splitted.IndexOfByPredicate((s, i) => i > startIndex && s == "№") - 1;

            int? number = null;

            if (int.TryParse(splitted[startIndex], out int n))
            {
                number = n;
            }

            var subject = splitted.Skip(startIndex + 1)
                    .Take(finishIndex - startIndex)
                    .Aggregate((current, next) => $"{current} {next}");

            var session = _sessionParser.Parse(argument);

            return new VottingSessionParserModel
            {
                Number = number,
                Subject = subject,
                SessionParserModel = session
            };
        }
    }
}
