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

        /// <summary>
        /// Get all votes
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all votes</response>
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Votes()
        {
            var result = await _mediator.Send(new GetVotesRequest());
            return Ok(result);
        }

        /// <summary>
        /// Get influences zones near the deputy 
        /// if influence == 1 than this deputies votes always the same way
        /// if influence == 0 than this deputies always votes differently
        /// </summary>
        /// <param name="deputyName"></param>
        /// <returns></returns>
        /// <response code="200">Returns all influence zones</response>
        [HttpGet]
        [Route("{deputyName}")]
        public async Task<IHttpActionResult> InfluenceZone(string deputyName)
        {
            var result = await _mediator.Send(new GetInfluenceZoneRequest(deputyName));
            return Ok(result);
        }
    }
}
