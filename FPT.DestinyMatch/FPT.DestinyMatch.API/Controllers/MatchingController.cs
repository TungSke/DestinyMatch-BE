using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace FPT.DestinyMatch.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchingController : Controller
    {
        private readonly IMatchingService _matchingService;
        private string thisUsermemId = "";
        public MatchingController(IMatchingService matchingService)
        {
            this._matchingService = matchingService;
        }

        [HttpGet]
        [Route("get-current-user-conversation")]
        [Authorize]
        public async Task<IActionResult> GetMatchings(int pageIndex = 1, int pageSize = 10, string? search = null, string status = "success : pending" )
        {
            thisUsermemId = User.Claims.FirstOrDefault(c => c.Type == "memberid")?.Value;
            var list = await _matchingService.GetMatchings(Guid.Parse(thisUsermemId), pageIndex, pageSize, search, status);
            return Ok(list);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMatching([FromBody] MatchingRequest matchingrequest)
        {
            thisUsermemId = User.Claims.FirstOrDefault(c => c.Type == "memberid")?.Value;
            matchingrequest.thisMemberId = Guid.Parse(thisUsermemId);
            await _matchingService.CreateMatching(matchingrequest);
            return Created();
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> ChangeStatus(Guid matchid, string status)
        {
            await _matchingService.ChangeStatusMatching(matchid, status);
            return Ok("change success");
        }
    }
}
