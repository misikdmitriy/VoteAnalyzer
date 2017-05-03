using System;
using System.Collections.Generic;
using System.Linq;

using VoteAnalyzer.Common.Constants;
using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;

namespace VoteAnalyzer.Parser.Parsers
{
    public class PageVotesParser : AbstractParser<ParseInfo, VoteParserModel[]>
    {
        private readonly IPdfContainer _pdfContainer;
        private readonly IParser<ParseInfo, DeputyParserModel[]> _deputiesParser;
        private readonly IParser<ParseInfo, VottingSessionParserModel> _vottingSessionParser;

        private static readonly string[] TextBefore = { "Результат", "голосування" };
        private static readonly string TextAfter = "Підсумки";

        public PageVotesParser(IParser<ParseInfo, DeputyParserModel[]> deputiesParser,
            IPdfContainer pdfContainer,
            IParser<ParseInfo, VottingSessionParserModel> vottingSessionParser)
        {
            _deputiesParser = deputiesParser;
            _pdfContainer = pdfContainer;
            _vottingSessionParser = vottingSessionParser;
        }

        public override VoteParserModel[] Parse(ParseInfo argument)
        {
            var votes = new List<VoteParserModel>();

            var splitted = _pdfContainer.GetSeparatedWords(argument.FileInfo, argument.Page);

            var voteNumber = 0;

            var deputies = _deputiesParser.Parse(argument);

            var startIndex =
                splitted.LastIndexOfByPredicate(
                    (s, i) => s.Equals(TextBefore[0], StringComparison.InvariantCultureIgnoreCase)
                              && splitted[i + 1].Equals(TextBefore[1], StringComparison.InvariantCultureIgnoreCase)) + 2;

            IEnumerable<string> cutted = splitted;

            var lastIteration = false;

            while (!lastIteration)
            {
                cutted = cutted.Skip(startIndex + 1).ToArray();

                startIndex = cutted.IndexOfByPredicate((s, i) => int.TryParse(s, out int _));

                if (startIndex == -1)
                {
                    startIndex =
                        cutted.IndexOfByPredicate(
                            (s, i) => s.Equals(TextAfter, StringComparison.InvariantCultureIgnoreCase));

                    lastIteration = true;
                }

                var taken = 1;

                var vote = cutted.ElementAt(startIndex - taken);

                while (!Constants.ExistingVotes
                    .Any(s => s.Equals(vote, StringComparison.InvariantCultureIgnoreCase)))
                {
                    vote = $"{cutted.ElementAt(startIndex - ++taken) } {vote}";
                }

                var vottingSession = _vottingSessionParser.Parse(argument);

                votes.Add(new VoteParserModel
                {
                    Vote = vote,
                    VottingSessionParserModel = vottingSession,
                    DeputyParserModel = deputies[voteNumber++]
                });
            }

            return votes.ToArray();
        }
    }
}
