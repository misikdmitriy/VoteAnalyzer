using System;
using System.Collections.Generic;
using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;

namespace VoteAnalyzer.Parser.Parsers
{
    /// <summary>
    /// Parse whole document
    /// </summary>
    public class DocumentVotesParser : AbstractParser<ParseInfo, VoteParserModel[]>
    {
        private readonly IParser<ParseInfo, VoteParserModel[]> _parser;
        private readonly IPdfContainer _pdfContainer;

        private readonly string[] _startText = { "Система", "поіменного", "голосування" };

        public DocumentVotesParser(IParser<ParseInfo, VoteParserModel[]> parser, IPdfContainer pdfContainer)
        {
            _parser = parser;
            _pdfContainer = pdfContainer;
        }

        public override VoteParserModel[] Parse(ParseInfo argument)
        {
            var votes = new List<VoteParserModel>();

            argument.Page = 0;

            while (true)
            {
                argument.Page++;

                try
                {
                    var splitted = _pdfContainer.GetSeparatedWords(argument.FileInfo, argument.Page);

                    if (splitted.IndexOfSubsequence(_startText) == -1)
                    {
                        continue;
                    }
                }
                catch (Exception)
                {
                    break;
                }

                votes.AddRange(_parser.Parse(argument));
            }

            return votes.ToArray();
        }
    }
}
