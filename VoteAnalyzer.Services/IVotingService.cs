using System.Threading.Tasks;
using VoteAnalyzer.PdfIntegration.Models;

namespace VoteAnalyzer.Services
{
    public interface IVotingService
    {
        Task ParseDocumentAsync(PdfFileInfo fileInfo);
    }
}