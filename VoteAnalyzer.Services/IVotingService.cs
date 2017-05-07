using System.Threading.Tasks;
using VoteAnalyzer.PdfIntegration.Models;

namespace VoteAnalyzer.Services
{
    public interface IVotesCounter
    {
        Task ParseDocumentAsync(PdfFileInfo fileInfo);
    }
}