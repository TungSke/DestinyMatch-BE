using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPT.DestinyMatch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]  
    public class FirebaseController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendMessageAsync()
        {
            // Implementation will be added here to handle sending push notifications
            return Ok("Push notification sent successfully!");
        }
    }
}
