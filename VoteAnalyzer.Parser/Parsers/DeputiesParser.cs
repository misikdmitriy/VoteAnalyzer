﻿using System;
using System.Collections.Generic;
using System.Linq;

using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.Parser.Models;
using VoteAnalyzer.PdfIntegration.PdfContainers;

namespace VoteAnalyzer.Parser.Parsers
{
    public class DeputiesParser : IParser<ParseInfo, DeputyParserModel[]>
    {
        private readonly IPdfContainer _pdfContainer;

        private static readonly string[] TextBefore = { "Результат", "голосування" };
        private static readonly string TextAfter = "Підсумки";

        public DeputiesParser(IPdfContainer pdfContainer)
        {
            _pdfContainer = pdfContainer;
        }

        public DeputyParserModel[] Parse(ParseInfo argument)
        {
            var deputies = new List<DeputyParserModel>();

            var splitted = _pdfContainer.GetSeparatedWords(argument.FileInfo, argument.Page);

            var startIndex =
                splitted.LastIndexOfByPredicate(
                    (s, i) => s.Equals(TextBefore[0], StringComparison.InvariantCultureIgnoreCase)
                              && splitted[i + 1].Equals(TextBefore[1], StringComparison.InvariantCultureIgnoreCase)) + 2;

            IEnumerable<string> cutted = splitted;

            while (startIndex != -1)
            {
                cutted = cutted.Skip(startIndex + 1).ToArray();

                deputies.Add(new DeputyParserModel
                {
                    Name = $"{cutted.ElementAt(0)} {cutted.ElementAt(1)} {cutted.ElementAt(2)}"
                });

                startIndex = cutted.IndexOfByPredicate((s, i) => int.TryParse(s, out int _));
            }

            return deputies.ToArray();
        }
    }
}