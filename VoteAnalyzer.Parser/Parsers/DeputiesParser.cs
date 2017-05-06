using System;
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

        private static readonly string[] TextBefore = { "п/п", "по-батькові", "депутата" };
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

            var startIndex =
                splitted.LastIndexOfByPredicate(
                    (s, i) => s.Equals(TextBefore[0], StringComparison.InvariantCultureIgnoreCase)
                              && splitted[i + 1].Equals(TextBefore[1], StringComparison.InvariantCultureIgnoreCase)
                              && splitted[i + 2].Equals(TextBefore[2], StringComparison.InvariantCultureIgnoreCase)) + 3;

            IEnumerable<string> cutted = splitted;

            while (startIndex != -1)
            {
                cutted = cutted.Skip(startIndex).ToArray();

                if (cutted.ElementAt(0).Equals(TextAfter[0], StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }

                if (int.TryParse(cutted.ElementAt(0), out var _))
                {
                    deputies.Add(new DeputyParserModel
                    {
                        Name = $"{cutted.ElementAt(1)} {cutted.ElementAt(2)} {cutted.ElementAt(3)}"
                    });
                }
                else
                {
                    deputies.Add(new DeputyParserModel
                    {
                        Name = $"{cutted.ElementAt(0)} {cutted.ElementAt(1)} {cutted.ElementAt(3)}"
                    });
                }

                var voteModel = _firstVoteParser.Parse(cutted.ToArray());
                var splittedVote = voteModel.Vote.Split(' ');

                startIndex = cutted.IndexOfByPredicate((s, i) =>
                {
                    for (var j = 0; j < splittedVote.Length; j++)
                    {
                        if (!splittedVote[j].Equals(cutted.ElementAt(i + j),
                            StringComparison.InvariantCultureIgnoreCase))
                        {
                            return false;
                        }
                    }

                    return true;
                }) + splittedVote.Length;
            }

            return deputies.ToArray();
        }
    }
}
