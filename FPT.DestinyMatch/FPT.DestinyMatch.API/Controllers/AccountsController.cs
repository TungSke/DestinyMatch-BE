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
            var account = await _accountService.GetAccountByIdAsync(id);
            return account is null ? NotFound("Not found that id account") : Ok(account);
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

                return Ok(new AuthenticationAccount { Id = userId, Email = userEmail, Role = userRole });
            }
            return Unauthorized();//401: User haven't authorized yet or don't have access permission
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAccount([FromBody] AccountLogin accCreate)
        {
            // Validate the email address
            if (!AccountLogin.IsValidEmail(accCreate.Email))
            {
                return BadRequest("Invalid email address!");
            }

            var CreateSucces = await _accountService.CreateAccountAsync(accCreate.Email, accCreate.Password);
            return CreateSucces ? Created(nameof(RegisterAccount), "Create Success") : BadRequest("Create Failed");
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AccountLogin accLog)
        {
            var acc = await _accountService.LoginByPasswordAsync(accLog.Email, accLog.Password);

            AuthenticationAccount validAcc = new()
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


        private string GenerateToken(AuthenticationAccount account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),//Jwt standard claim
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role)//Jwt claim in .Net
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

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
        public async Task<IActionResult> Delete([FromBody] AccountLogin input)
        {
            bool result = await _accountService.DeleteAccountAsync(input.Email, input.Password, false);
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
        [Route("google-login")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        {
            /*
             var payload = await GoogleJsonWebSignature.ValidateAsync(idToken,
            new GoogleJsonWebSignature.ValidationSettings());

            // Extract the email from the payload
            var email = payload.Email;

            string username = "", password = "";
            bool needSignup = true, existEmail = false;
            User user;
            Teacher teacher;
            Staff staff;
            Admin admin;
            for (int role = 1; role <= 4; role++)
            {
            switch (role)
            {
               case 1:
                   user = await _context.Users
                   .Include(u => u.Account)
                   .SingleOrDefaultAsync(usr => usr.Email.Equals(email));
                   existEmail = (user is not null);
                   username = (user is not null) ? user.Account.Username : string.Empty;
                   password = (user is not null) ? user.Account.Password : string.Empty;
                   break;
               case 2:
                   teacher = await _context.Teachers
                   .Include(tch => tch.Account)
                   .SingleOrDefaultAsync(tch => tch.Email.Equals(email));
                   existEmail = (teacher is not null);
                   username = (teacher is not null) ? teacher.Account.Username : string.Empty;
                   password = (teacher is not null) ? teacher.Account.Password : string.Empty;
                   break;
               case 3:
                   staff = await _context.Staff
                   .Include(stf => stf.Account)
                   .SingleOrDefaultAsync(stf => stf.Email.Equals(email));
                   existEmail = (staff is not null);
                   username = (staff is not null) ? staff.Account.Username : string.Empty;
                   password = (staff is not null) ? staff.Account.Password : string.Empty;
                   break;
               case 4:
                   admin = await _context.Admins
                   .Include(adm => adm.Account)
                   .SingleOrDefaultAsync(adm => adm.Email.Equals(email));
                   existEmail = (admin is not null);
                   username = (admin is not null) ? admin.Account.Username : string.Empty;
                   password = (admin is not null) ? admin.Account.Password : string.Empty;
                   break;
            }
            //If exist then stop checking
            if (existEmail)
            {
               needSignup = false;
               break;
            }

            }//End loop

            //If not exist then create account
            if (needSignup)
            {
            username = email;
            password = Guid.NewGuid().ToString();
            await _context.Database.ExecuteSqlRawAsync("exec dbo.proc_signUpAccount @username = @p0, @password = @p1, @email = @p2", email, password, email);
            }

            //Then login
            var account = await _context.Accounts
            .AsNoTracking()
            .SingleOrDefaultAsync(acc => acc.Username.Equals(username) && acc.Password.Equals(password));

            HttpContext.Session.SetString("usersession", JsonSerializer.Serialize(account));
            await HttpContext.Session.CommitAsync();
            return RedirectToAction("Index", "Home");
            }
            */
            return Ok();
        }
    }
}
