using BusinessLogic.Interfaces;
using DestinyMatch_API.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DestinyMatch_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class accountsController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IAccountService _accountService;

        public accountsController(IConfiguration config, IAccountService accountService)
        {
            _config = config;
            _accountService = accountService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "admin,moderator,member")]
        public async Task<IActionResult> ViewAccount([FromRoute] Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound("Not found that id account");
            }
            return Ok(account);
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccountAsync([FromBody] AccountAuthen accCreate)
        {
            var CreateSucces = await _accountService.CreateAccountAsync(accCreate.Email, accCreate.Password);
            if (CreateSucces == true)
            {
                return Ok("Create Success");
            }
            return BadRequest("Account existed!");
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AccountAuthen accLog)
        {
            var account = await _accountService.LoginByPassword(accLog.Email, accLog.Password);
            if (account != null)
            {
                var token = GenerateToken(account);
                return Ok(token);
            }
            return BadRequest("Login Failed!");
        }

        private string GenerateToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),//Jwt standard claim
                new Claim(JwtRegisteredClaimNames.Email, account.Email ?? ""),
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
        [Route("changRole")]
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
        [Route("changePassword")]
        [Authorize(Roles = "moderator,member")]
        public async Task<IActionResult> ChangePassword([FromBody] AccountNewPassword input)
        {
            bool result = await _accountService
                .ChangePasswordAccountAsync(input.Id, input.OldPassword, input.NewPassword);
            if (result == true)
            {
                return Ok("Update Success!");
            }
            return BadRequest("Update Failed!");
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "moderator,member")]
        public async Task<IActionResult> Delete([FromBody] AccountAuthen input)
        {
            bool result = await _accountService.DeleteAccountAsync(input.Email, input.Password);
            if (result == true)
            {
                return Ok("Delete Success!");
            }
            return BadRequest("Delete Failed!");
        }

        [HttpPatch]
        [Route("recoverAccount")]
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

        [HttpPatch]
        [Route("resetPassword")]
        [Authorize(Roles = "moderator,member")]
        public async Task<IActionResult> ResetPassword()
        {
            //Is doing with Google Cloud Api
            return Ok();
        }

        [HttpPost]
        [Route("login/google")]
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
