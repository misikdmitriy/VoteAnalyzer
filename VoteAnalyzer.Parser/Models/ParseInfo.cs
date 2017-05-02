using VoteAnalyzer.PdfIntegration.Models;

namespace VoteAnalyzer.Parser.Models
{
    public class ParseInfo
    {
        public PdfFileInfo FileInfo { get; set; }
        public int Page { get; set; }
    }
}
