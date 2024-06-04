using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DestinyMatch_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;
        public authController(IAccountService accountService, IAuthService authService)
        {
            _accountService = accountService;
            _authService = authService;
        }
        [HttpGet("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var acc = await _accountService.LoginByPassword(email, password);
            if (acc == null)
            {
                return NotFound();
            }
            else
            {
                var jwt = await _authService.GenerateJSONWebToken(acc);
                return Ok(jwt);
            }
        }
    }
}
