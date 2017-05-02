using System;
using System.Linq;
using VoteAnalyzer.Common;
using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.Common.Models;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.Parser
{
    public class HeaderParser : IParser<ParseInfo, Session>
    {
        private readonly IPdfConverter _pdfConverter;
        private IParser<string, string[]> _wordParser;

        private string[] _textBefore = { "Броварська", "міська", "рада" };

        public HeaderParser(IPdfConverter pdfConverter, IParser<string, string[]> wordParser)
        {
            _pdfConverter = pdfConverter;
            _wordParser = wordParser;
        }

        public Session Parse(ParseInfo argument)
        {
            var text = _pdfConverter.ConvertToText(argument);

            if (!string.IsNullOrEmpty(text))
            {
                var splitted = _wordParser.Parse(text);

                var index =
                    splitted.IndexOfByPredicate(
                        (s, i) => s.Equals(_textBefore[0], StringComparison.InvariantCultureIgnoreCase)
                            && splitted[i + 1].Equals(_textBefore[1], StringComparison.InvariantCultureIgnoreCase)
                            && splitted[i + 2].Equals(_textBefore[2], StringComparison.InvariantCultureIgnoreCase));

                if (index != -1)
                {
                    var sessionNameStartIndex = index + 3;
                }
            }

            throw new NotImplementedException();
        }
    }
}
