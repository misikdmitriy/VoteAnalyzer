using System.Collections.Generic;
using VoteAnalyzer.Parser.Models;

namespace VoteAnalyzer.Parser.Parsers
{
    /// <summary>
    /// Parse whole document
    /// </summary>
    public class VoteParserFacade : AbstractParser<ParseInfo, VoteParserModel[]>
    {
        private readonly IParser<ParseInfo, VoteParserModel[]> _parser;

        public VoteParserFacade(IParser<ParseInfo, VoteParserModel[]> parser)
        {
            _parser = parser;
        }

        public override VoteParserModel[] Parse(ParseInfo argument)
        {
            var votes = new List<VoteParserModel>();

            bool isSuccess;

            argument.Page = 0;

            do
            {
                argument.Page++;
                isSuccess = _parser.TryParse(argument, out VoteParserModel[] voteParserModels);

                if (isSuccess)
                {
                    votes.AddRange(voteParserModels);
                }

            } while (isSuccess);

            return votes.ToArray();
        }
    }
}
