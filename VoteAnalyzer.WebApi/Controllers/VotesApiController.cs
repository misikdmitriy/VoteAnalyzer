using System.Threading.Tasks;
using System.Web.Http;

using MediatR;

using VoteAnalyzer.WebApi.MediatrRequests;

namespace VoteAnalyzer.WebApi.Controllers
{
    [RoutePrefix("api/votes")]
    public class VotesApiController : ApiController
    {
        private readonly IMediator _mediator;

        public VotesApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Votes()
        {
            var result = await _mediator.Send(new GetVotesRequest());
            return Ok(result);
        }
    }
}
