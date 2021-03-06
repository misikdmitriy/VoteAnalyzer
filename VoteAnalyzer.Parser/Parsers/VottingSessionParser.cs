﻿using System;
using System.Linq;

using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;

namespace VoteAnalyzer.Parser.Parsers
{
    /// <summary>
    /// Parse votting session
    /// Rule: votting session start with symbol '№': [NUMBER] [SESSION SUBJECT] ['№', 'Прізвище']
    /// </summary>
    public class VottingSessionParser : AbstractParser<ParseInfo, VottingSessionParserModel>
    {
        private readonly IPdfContainer _pdfContainer;
        private readonly IParser<ParseInfo, SessionParserModel> _sessionParser;

        private readonly string[] _textBefore = { "№" };

        public VottingSessionParser(IPdfContainer pdfContainer,
            IParser<ParseInfo, SessionParserModel> sessionParser)
        {
            _pdfContainer = pdfContainer;
            _sessionParser = sessionParser;
        }

        public override VottingSessionParserModel Parse(ParseInfo argument)
        {
            var splitted = _pdfContainer.GetSeparatedWords(argument.FileInfo, argument.Page);

            var startIndex = splitted.IndexOfSubsequence(_textBefore) + 1;

            var finishIndex = splitted.IndexOfByPredicate((s, i) => i > startIndex &&
                (s == "№" || s.Equals("Прізвище", StringComparison.InvariantCultureIgnoreCase))) - 1;

            int? number = null;

            if (int.TryParse(splitted[startIndex], out int n))
            {
                number = n;
            }

            string subject;

            if (finishIndex - startIndex <= 0)
            {
                subject = "";
            }
            else
            {
                subject = splitted.Skip(startIndex + 1)
                    .Take(finishIndex - startIndex)
                    .Aggregate((current, next) => $"{current} {next}");
            }

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
