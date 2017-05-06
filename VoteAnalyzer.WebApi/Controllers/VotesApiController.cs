using System.Threading.Tasks;
using System.Web.Http;
using VoteAnalyzer.Services;
using VoteAnalyzer.WebApi.Models;

namespace VoteAnalyzer.WebApi.Controllers
{
    [RoutePrefix("api/votes")]
    public class VotesApiController : ApiController
    {
        private IVotingService _votingService;
        private ParseFilesInfo _info;

        public VotesApiController(IVotingService votingService, ParseFilesInfo info)
        {
            _votingService = votingService;

            _info = info;
        }

        [HttpGet]
        [Route("demo")]
        public async Task<IHttpActionResult> Temp()
        {
            foreach (var pdfFileInfo in _info.FilesInfo)
            {
                await _votingService.ParseDocumentAsync(pdfFileInfo);
            }
            return Ok("otsosi");
        }
    }
}
