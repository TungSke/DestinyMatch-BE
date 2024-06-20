using FPT.DestinyMatch.API.Models.RequestModels;
using FPT.DestinyMatch.API.Models.RequestModels.Paging;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FPT.DestinyMatch.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationsController : Controller
    {
        private readonly IVerificationService _verificationService;
        public VerificationsController(IVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> ViewDetail([FromRoute] Guid id)
        {
            return Ok(await _verificationService.GetVerificationDetailAsync(id));
        }

        [HttpGet]
        [Route("history")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> ViewHistory([FromBody] VerificationPaging inputData)
        {
            if (inputData.MemberId == Guid.Empty)
            {
                return BadRequest("View History failed! No specific members yet.");
            }
            var verList = await _verificationService.GetListVerificationAsync(
                inputData.Amount,
                inputData.Page,
                inputData.MemberId,
                inputData.Status,
                inputData.OrderByAscending);
            return Ok(verList);
        }

        [HttpPost]
        [Route("new-verification")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> RequestVerification([FromBody] VerificationSubmit submittedData)
        {
            bool result = await _verificationService.CreateVerificationAsync(submittedData.SubmittedPicture, submittedData.MemberId);
            return result ? Created(nameof(RequestVerification), "Submit Success") : BadRequest("Submit Failed");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> CancelVerification([FromRoute] Guid id)
        {
            bool result = await _verificationService.DeleteVerificationAsync(id);
            return result ? Ok("Cancel Success") : BadRequest("Cancel Failed");
        }
    }
}
