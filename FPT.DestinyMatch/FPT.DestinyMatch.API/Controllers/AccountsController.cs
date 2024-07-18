using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.API.Models.ResponseModels;
using FPT.DestinyMatch.API.Models.RequestModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace FPT.DestinyMatch.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> ViewAccount([FromRoute] Guid id)
        {
            return Ok(await _accountService.GetMemberByAccountId(id));
        }

        [HttpGet]
        [Route("me")]
        [Authorize]//Must login to use
        public IActionResult CheckCurrentSession()
        {
            var userClaims = User.Claims.ToList();

            if (userClaims.Any())
            {
                string? userId = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                string? userEmail = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                string? userRole = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                string? memberid = userClaims.FirstOrDefault(c => c.Type == "memberid")?.Value;

                return Ok(new ClaimAccountInfo { Id = userId, Email = userEmail, Role = userRole, MemberId = memberid });
            }
            return Unauthorized();//401: User haven't authorized yet or don't have access permission
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAccount([FromBody] AccountAuthen accCreate)
        {
            // Validate the email address
            if (!AccountAuthen.IsValidEmail(accCreate.Email))
            {
                return BadRequest("Invalid email address!");
            }

            var CreateSucces = await _accountService.CreateAccountAsync(accCreate.Email, accCreate.Password, accCreate.ReceiveNotifiEmail);
            return CreateSucces ? Created(nameof(RegisterAccount), "Create Success") : BadRequest("Create Failed");
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AccountAuthen accLog)
        {
            // Validate the email address
            if (!AccountAuthen.IsValidEmail(accLog.Email))
            {
                return BadRequest("Invalid email address!");
            }

            var jwtToken = await _accountService.LoginByPasswordAsync(accLog.Email, accLog.Password);

            return jwtToken.IsNullOrEmpty() ?
                BadRequest("There is an error during generate JWT Token!") :
                Created(nameof(Login), new JwtToken
                {
                    Token = jwtToken
                });
        }

        [HttpPatch]
        [Route("new-password")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> ChangePassword([FromBody] AccountNewPassword input)
        {
            if (input.OldPassword.IsNullOrEmpty())
            {
                return BadRequest("Old password required for confirmation!");
            }
            bool result = await _accountService.ChangePasswordAccountAsync(input.Id, input.OldPassword, input.NewPassword, false);
            return result ? Ok("Update Success!") : BadRequest("Update Failed!");
        }

        [HttpDelete]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> Delete([FromBody] AccountConfirm acc)
        {
            bool result = await _accountService.DeleteAccountAsync(acc.Id, acc.Password);
            return result ? Ok("Delete Success!") : BadRequest("Delete Failed!");
        }

        [HttpPatch]
        [Route("password-recovery")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> ResetPassword()
        {
            //Is doing with Google Cloud Api
            return Ok();
        }

        [HttpPost]
        [Route("google-authentication")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleResponse responseData)
        {
            string jwtToken = await _accountService.HandleGoogleAsync(responseData.Token, responseData.Platform);

            return jwtToken.IsNullOrEmpty() ?
                BadRequest("There is an error during generate JWT Token!") :
                Created(nameof(Login), new JwtToken
                {
                    Token = jwtToken
                });
        }

        [HttpPut]
        [Route("update-fcmtoken")]
        [Authorize]
        public async Task<IActionResult> UpdateFcmToken(string fcmtoken)
        {
            var context = HttpContext;
            await _accountService.UpdateFcmToken(context, fcmtoken);
            return Ok("Update Success!");
        }
    }
}
