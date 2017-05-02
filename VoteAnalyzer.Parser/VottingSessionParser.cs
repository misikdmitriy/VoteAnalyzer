using VoteAnalyzer.Common;
using VoteAnalyzer.Common.Models;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.Parser
{
    public class VottingSessionParser : IParser<ParseInfo, VottingSession>
    {
        private readonly IPdfConverter _pdfConverter;
        private readonly IParser<string, string[]> _wordParser;

        public VottingSessionParser(IPdfConverter pdfConverter, IParser<string, string[]> wordParser)
        {
            _pdfConverter = pdfConverter;
            _wordParser = wordParser;
        }

        public VottingSession Parse(ParseInfo argument)
        {
            throw new System.NotImplementedException();
        }
    }
}
