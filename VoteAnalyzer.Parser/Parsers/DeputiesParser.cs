using System.Collections.Generic;
using System.Linq;

using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;

namespace VoteAnalyzer.Parser.Parsers
{
    public class DeputiesParser : AbstractParser<ParseInfo, DeputyParserModel[]>
    {
        private readonly IPdfContainer _pdfContainer;
        private readonly IParser<string[], FirstVoteParserModel> _firstVoteParser;

        private static readonly string[] TextBefore = { "по-батькові", "депутата" };
        private static readonly string[] TextAfter = { "ПІДСУМКИ" };

        public DeputiesParser(IPdfContainer pdfContainer, 
            IParser<string[], FirstVoteParserModel> firstVoteParser)
        {
            _pdfContainer = pdfContainer;
            _firstVoteParser = firstVoteParser;
        }

        public override DeputyParserModel[] Parse(ParseInfo argument)
        {
            var deputies = new List<DeputyParserModel>();

            var splitted = _pdfContainer.GetSeparatedWords(argument.FileInfo, argument.Page);

            var startIndex = splitted.LastIndexOfSubsequence(TextBefore) + TextBefore.Length;
            var finishIndex = splitted.IndexOfSubsequence(TextAfter);

            var cutted = splitted
                .Skip(startIndex)
                .Take(finishIndex - startIndex)
                .ToList();

            RemoveVotes(cutted);
            RemoveNumbers(cutted);

            for (var i = 0; i < cutted.Count; i += 3)
            {
                deputies.Add(new DeputyParserModel
                {
                    Name = $"{cutted[i]} {cutted[i + 1]} {cutted[i + 2]}"
                });
            }

            return deputies.ToArray();
        }

        private static void RemoveNumbers(IList<string> cutted)
        {
            while (true)
            {
                var startIndex = cutted.IndexOfByPredicate((s, i) => int.TryParse(s, out int _));
                if (startIndex == -1)
                {
                    break;
                }
                cutted.RemoveAt(startIndex);
            }
        }

        private void RemoveVotes(List<string> cutted)
        {
            while (true)
            {
                if (_firstVoteParser.TryParse(cutted.ToArray(), out var voteModel))
                {
                    var splittedVote = voteModel.Vote.Split(' ');

                    var startIndex = cutted.IndexOfSubsequence(splittedVote);

                    cutted.RemoveRange(startIndex, splittedVote.Length);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
