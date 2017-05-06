using VoteAnalyzer.PdfIntegration.Models;

namespace VoteAnalyzer.PdfIntegration.PdfServices
{
    public interface IPdfService
    {
        string GetContent(PdfFileInfo info, int page);
        int GetNumberOfPages(PdfFileInfo info);
        bool Exists(PdfFileInfo info);
    }
}
