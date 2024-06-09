using FPT.DestinyMatch.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FPT.DestinyMatch.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationController : Controller
    {
        private readonly IVerificationService _verificationService;
        public VerificationController(IVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles ="moderator")]
        public async Task<IActionResult> ViewDetail([FromBody] Guid s) {
            return Ok();
        }
    }
}
