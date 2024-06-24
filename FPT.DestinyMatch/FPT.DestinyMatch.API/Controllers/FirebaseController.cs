using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;

namespace FPT.DestinyMatch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]  
    public class FirebaseController : ControllerBase
    {
        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification(string fcmToken, string? title,string body)
        {
            var message = new Message()
            {
                Token = fcmToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };
            try
            {
                Console.WriteLine("Sending notification to: " + fcmToken);
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return Ok("Notification sent successfully: " + fcmToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending notification: {ex.Message}");
            }
        }
    }
}
