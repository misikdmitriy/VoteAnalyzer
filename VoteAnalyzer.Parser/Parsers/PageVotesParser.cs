using System.Collections.Generic;
using System.Linq;

using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;

namespace VoteAnalyzer.Parser.Parsers
{
    /// <summary>
    /// Search all votes on page uisng FirstVoteParser
    /// </summary>
    public class PageVotesParser : AbstractParser<ParseInfo, VoteParserModel[]>
    {
        private readonly IPdfContainer _pdfContainer;
        private readonly IParser<ParseInfo, DeputyParserModel[]> _deputiesParser;
        private readonly IParser<ParseInfo, VottingSessionParserModel> _vottingSessionParser;
        private readonly IParser<string[], FirstVoteParserModel> _firstVoteParser;

        private static readonly string[] TextBefore = { "п/п", "по-батькові", "депутата" };

        public PageVotesParser(IParser<ParseInfo, DeputyParserModel[]> deputiesParser,
            IPdfContainer pdfContainer,
            IParser<ParseInfo, VottingSessionParserModel> vottingSessionParser, 
            IParser<string[], FirstVoteParserModel> firstVoteParser)
        {
            _deputiesParser = deputiesParser;
            _pdfContainer = pdfContainer;
            _vottingSessionParser = vottingSessionParser;
            _firstVoteParser = firstVoteParser;
        }

        public override VoteParserModel[] Parse(ParseInfo argument)
        {
            var votes = new List<VoteParserModel>();

            var splitted = _pdfContainer.GetSeparatedWords(argument.FileInfo, argument.Page);

            var voteNumber = 0;

            var deputies = _deputiesParser.Parse(argument);

            var startIndex = splitted.IndexOfSubsequence(TextBefore) + 3;

            IEnumerable<string> cutted = splitted;

            var vottingSession = _vottingSessionParser.Parse(argument);

            while (voteNumber < deputies.Length)
            {
                cutted = cutted.Skip(startIndex).ToArray();

                var voteModel = _firstVoteParser.Parse(cutted.ToArray());
                var splittedVote = voteModel.Vote.Split(' ');

                startIndex = cutted.IndexOfSubsequence(splittedVote) + splittedVote.Length;

                votes.Add(new VoteParserModel
                {
                    Vote = voteModel.Vote,
                    VottingSessionParserModel = vottingSession,
                    DeputyParserModel = deputies[voteNumber++]
                });
            }

            return votes.ToArray();
        }
    }
}
