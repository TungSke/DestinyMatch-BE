using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using FPT.DestinyMatch.Service.Interfaces;

namespace FPT.DestinyMatch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;

        public FirebaseController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }
        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification(string fcmToken, string? title, string body)
        {
            await _firebaseService.SendNotification(fcmToken, title, body);
            return Ok("Notification sent successfully: " + fcmToken);
        }
    }
}
