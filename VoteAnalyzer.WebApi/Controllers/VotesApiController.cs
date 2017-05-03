using System.Web.Http;
using VoteAnalyzer.Services;
using VoteAnalyzer.WebApi.Models;

namespace VoteAnalyzer.WebApi.Controllers
{
    [RoutePrefix("api/votes")]
    public class VotesApiController : ApiController
    {
        private IVotingService _votingService;

        public VotesApiController(IVotingService votingService, ParseFilesInfo info)
        {
            _votingService = votingService;

            foreach (var pdfFileInfo in info.FilesInfo)
            {
                _votingService.ParseDocumentAsync(pdfFileInfo);
            }
        }

        [HttpGet]
        [Route("demo")]
        public IHttpActionResult Temp()
        {
            return Ok();
        }
    }
}
