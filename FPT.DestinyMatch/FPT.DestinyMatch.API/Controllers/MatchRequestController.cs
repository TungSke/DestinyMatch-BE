using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace FPT.DestinyMatch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchRequestController : Controller
    {
        private readonly IMatchRequestService _matchRequestService;

        public MatchRequestController(IMatchRequestService matchRequestService)
        {
            _matchRequestService = matchRequestService;
        }

        [HttpGet("match")]
        public async Task<IActionResult> GetMatchRequest(Guid matchRequestId)
        {
            var matchRequest = await _matchRequestService.GetMatchRequestById(matchRequestId);
            if (matchRequest is null)
            {
                return NotFound();
            }
            return Ok(matchRequest);
        }

        [HttpGet("match-requests")]
        public async Task<IActionResult> GetAllMatchRequest()
        {
            var matchRequest = await _matchRequestService.GetMatchRequests();
            return Ok(matchRequest);
        }

        [HttpPost("matching")]
        public async Task<IActionResult> CreateMatchRequest([FromBody] MatchRequestToAdd matchRequestToAdd)
        {
            var matchRequest = await _matchRequestService.Matching(matchRequestToAdd);
            return Ok(matchRequest);
        }

        [HttpGet("match-by-me/{memberId}")]
        public async Task<IActionResult> GetMatchRequestsByMe(Guid memberId)
        {
            var matchRequests = await _matchRequestService.MatchRequestOfMe(memberId);
            return Ok(matchRequests);
        }

        [HttpGet("match-to-me/{memberId}")]
        public async Task<IActionResult> GetMatchRequestsToMe(Guid memberId)
        {
            var matchRequests = await _matchRequestService.MatchRequestToMe(memberId);
            return Ok(matchRequests);
        }

        [HttpPatch("match-response")]
        public async Task<IActionResult> MatchResponse(Guid matchRequestId,string response)
        {
            var matchRequests = await _matchRequestService.MatchResponse(matchRequestId, response);
            return Ok(matchRequests);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveMatchRequest(Guid matchRequestId)
        {
            var isRemove = await _matchRequestService.RemoveMatchRequest(matchRequestId);
            if(isRemove == false)
            {
                return NotFound("Can't find Match Request");
            }
            return NoContent();
        }
    }
}
