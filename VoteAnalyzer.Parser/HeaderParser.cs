using System;
using System.Linq;
using VoteAnalyzer.Common;
using VoteAnalyzer.Common.Models;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.Parser
{
    public class HeaderParser : IParser<ParseInfo, Session>
    {
        private IPdfConverter _pdfConverter;

        private string _textBefore = "Броварська міська рада";

        public HeaderParser(IPdfConverter pdfConverter)
        {
            _pdfConverter = pdfConverter;
        }

        public Session Parse(ParseInfo argument)
        {
            var text = _pdfConverter.ConvertToText(argument);

            if (!string.IsNullOrEmpty(text))
            {
                // Session name start after 'Броварська міська рада'
                var startIndex = text.IndexOf(_textBefore, StringComparison.InvariantCultureIgnoreCase) 
                    + _textBefore.Length;

                // Session name ends with date
                var readTo = text.Substring(startIndex)
                    .IndexOf(".", StringComparison.InvariantCultureIgnoreCase) + 6;

                var result = text.Substring(startIndex, readTo);

                var numbers = Enumerable.Range(0, 10).Select(n => n.ToString()[0]).ToArray();

                var indexOfDate = result.Substring(4)
                    .IndexOfAny(numbers) + 4;

                var yearMonthDay = result.Substring(indexOfDate)
                    .Split('.')
                    .Select(int.Parse)
                    .ToArray();

                var date = new DateTime(2000 + yearMonthDay[2], yearMonthDay[1], yearMonthDay[0]);

                return new Session
                {
                    DateTime = date,
                    Name = result.Substring(1, indexOfDate - 2)
                };
            }

            throw new NotImplementedException();
        }
    }
}
