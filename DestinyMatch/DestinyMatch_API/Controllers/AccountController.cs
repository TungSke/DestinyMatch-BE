using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DestinyMatch_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet]
        [Route("api/Account/{email}")]
        public async Task<ActionResult> GetAccountByEmail([FromRoute] string email)
        {
            var account = await _accountService.GetByEmailAsync(email);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        
        [HttpGet("getallaccounts")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> getAllAccount()
        {
            var list = await _accountService.GetAccounts();
            return Ok(list); 
        }
    }
}
