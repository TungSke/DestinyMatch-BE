using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.API.Models.ResponseModels;
using FPT.DestinyMatch.API.Models.RequestModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Google.Apis.Auth;

using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace FPT.DestinyMatch.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IAccountService _accountService;

        public AccountsController(IConfiguration config, IAccountService accountService)
        {
            _config = config;
            _accountService = accountService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> ViewAccount([FromRoute] Guid id)
        {
            return Ok(await _accountService.GetAccountByIdAsync(id));
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

                return Ok(new ClaimAccountInfo { Id = userId, Email = userEmail, Role = userRole });
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

            var CreateSucces = await _accountService.CreateAccountAsync(accCreate.Email, accCreate.Password);
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

            var acc = await _accountService.LoginByPasswordAsync(accLog.Email, accLog.Password);

            ClaimAccountInfo validAcc = new()
            {
                Id = acc.Id.ToString(),
                Email = acc.Email!,
                Role = acc.Role
            };

            var token = GenerateToken(validAcc);
            return Created(nameof(Login), new JwtToken
            {
                Token = token
            });
        }


        private string GenerateToken(ClaimAccountInfo account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),//Jwt standard claim
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role)//Jwt claim in .Net
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
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

        private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string token, string platform)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = platform.ToLower().Equals("web") ?
                new List<string> { _config["Google:web:client_id"]! } :
                new List<string> { _config["Google:mobile:client_id"]! }
            };

            return await GoogleJsonWebSignature.ValidateAsync(token, settings);
        }

        [HttpPost]
        [Route("google-authentication")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleResponse responseData)
        {

            var payload = await ValidateGoogleToken(responseData.Token, responseData.Platform);

            // Extract the email from the payload
            var email = payload.Email;
            //var fullname = payload.Name;
            //var pictureUrl = payload.Picture;

            var accInfo = await _accountService.HandleGoogleAsync(email);

            ClaimAccountInfo validAcc = new()
            {
                Id = accInfo.Id.ToString(),
                Email = accInfo.Email!,
                Role = accInfo.Role
            };

            var token = GenerateToken(validAcc);
            return Created(nameof(Login), new JwtToken
            {
                Token = token
            });
        }
    }
}
