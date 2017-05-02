using VoteAnalyzer.PdfIntegration.Models;

namespace VoteAnalyzer.PdfIntegration.PdfContainers
{
    public interface IPdfContainer
    {
        string[] GetSeparatedWords(PdfFileInfo fileInfo, int page);
        string GetContent(PdfFileInfo fileInfo, int page);
        void Clear();
    }
}
