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
        private readonly IParser<string, string[]> _wordParser;

        private readonly string[] _textBefore = { "Броварська", "міська", "рада" };
        private const string NameDateSeparator = "від";

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

                    var cutted = splitted.Skip(sessionNameStartIndex).ToArray();

                    var indexOfDate =
                        cutted.IndexOfByPredicate(
                            (s, i) => s.Equals(NameDateSeparator, StringComparison.InvariantCultureIgnoreCase)) + 1;

                    var name = cutted.Take(indexOfDate - 1)
                        .Aggregate((current, next) => current + " " + next);

                    var date = new DateTime(2000 + int.Parse(cutted[indexOfDate + 2]),
                        int.Parse(cutted[indexOfDate + 1]),
                        int.Parse(cutted[indexOfDate]));

                    return new Session
                    {
                        DateTime = date,
                        Name = name
                    };
                }
            }

            return null;
        }
    }
}
