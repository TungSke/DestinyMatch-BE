using FPT.DestinyMatch.API.Models.RequestModels;
using FPT.DestinyMatch.API.Models.RequestModels.Paging;
using FPT.DestinyMatch.API.Models.ResponseModels;
using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FPT.DestinyMatch.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class PrivilegedsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IVerificationService _verificationService;
        public PrivilegedsController(
            IAccountService accountService,
            IVerificationService verificationService)
        {
            _accountService = accountService;
            _verificationService = verificationService;
        }

        //=====================[ ACCOUNT ]=====================
        [HttpGet]
        [Route("account/{id}")]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> ViewAccount([FromRoute] Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account is null)
            {
                return NotFound("Not found that id account");
            }
            return Ok(account);
        }

        [HttpPost]
        [Route("account/list")]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> GetListAccount([FromBody] AccountPaging inputData)
        {
            var accList = await _accountService.GetAccountsListAsync
                (inputData.Amount,
                inputData.Page,
                inputData.EmailKeyword,
                inputData.ByDate,
                inputData.Status,
                inputData.Role,
                inputData.OrderByDescending);
            return Ok(accList);
        }

        [HttpPatch]
        [Route("account/role-management")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeRole([FromBody] AccountNewRole input)
        {
            bool result = await _accountService.ChangeRoleAccountAsync(input.Id, input.NewRole);
            if (result == true)
            {
                return Ok("Update Success!");
            }
            return BadRequest("Update Failed!");
        }

        [HttpPatch]
        [Route("account/new-password")]
        [Authorize(Roles = "moderator")]
        public async Task<IActionResult> ChangePassword([FromBody] AccountNewPassword input)
        {
            bool result = await _accountService
                .ChangePasswordAccountAsync(input.Id, string.Empty, input.NewPassword, true);
            if (result == true)
            {
                return Ok("Update Success!");
            }
            return BadRequest("Update Failed!");
        }

        [HttpPatch]
        [Route("account/recovery")]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Recover([FromBody] AccountRecover input)
        {
            bool result = await _accountService
                .RecoverAccountAsync(input.Email, input.Status);
            if (result == true)
            {
                return Ok("Recover Success!");
            }
            return BadRequest("Recover Failed!");
        }

        [HttpDelete]
        [Route("account/{id}")]
        [Authorize(Roles = "moderator")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            bool result = await _accountService.DeleteAccountAsync(id);
            if (result == true)
            {
                return Ok("Delete Success!");
            }
            return BadRequest("Delete Failed!");
        }

        //=====================[ VERIFICATION ]=====================
        [HttpGet]
        [Route("verification/{id}")]
        [Authorize(Roles = "moderator")]
        public async Task<IActionResult> ViewDetail([FromRoute] Guid id)
        {
            return Ok(await _verificationService.GetVerificationDetailAsync(id));
        }

        [HttpPost]
        [Route("verification/list")]
        [Authorize(Roles = "moderator")]
        public async Task<IActionResult> ViewListVerification([FromBody] VerificationPaging inputData)
        {
            var verList = await _verificationService.GetListVerificationAsync(
                inputData.Amount,
                inputData.Page,
                inputData.MemberId,
                inputData.Status,
                inputData.OrderByAscending);
            return Ok(verList);
        }

        [HttpPatch]
        [Route("verification/new-status")]
        [Authorize(Roles = "moderator")]
        public async Task<IActionResult> UpdateStatusVerification([FromBody] VerificationUpdate inputData)
        {
            bool result = await _verificationService.UpdateStatusVerificationAsync(inputData.Id, inputData.Status);
            return result ? Ok("Update Success") : BadRequest("Update Failed");
        }

        [HttpDelete]
        [Route("verification/{id}")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> CancelVerification([FromRoute] Guid id)
        {
            bool result = await _verificationService.DeleteVerificationAsync(id);
            return result ? Ok("Delete Success") : BadRequest("Delete Failed");
        }
    }
}
