using VoteAnalyzer.Common.Models;

namespace VoteAnalyzer.Common
{
    public interface IPdfConverter
    {
        string ConvertToText(ParseInfo info);
    }
}
