using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FPT.DestinyMatch.API.Controllers
{
    namespace DestinyMatch_API.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class MessageController : ControllerBase
        {
            private readonly IHubContext<ChatHub> _hubContext;
            private readonly IMessageService _messageService;

            public MessageController(IMessageService messageService, IHubContext<ChatHub> hubContext)
            {
                _messageService = messageService;
                _hubContext = hubContext;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var message = await _messageService.GetMessages();
                return Ok(message);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetMessageById(Guid id)
            {
                var message = await _messageService.GetMessageById(id);
                if (message is null)
                {
                    return BadRequest();
                }
                return Ok(message);
            }
            [HttpPost]
            public async Task<IActionResult> CreateMessage([FromBody] MessageRequest messageRequest)
            {
                var message = await _messageService.CreateMessage(messageRequest);

                // Notify clients about the new message
                
                return Ok(message);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateMessage(Guid id, [FromBody] MessageRequest messageRequest)
            {
                var message = await _messageService.UpdateMessage(id, messageRequest);
                if (message is null)
                {
                    return NotFound();
                }
                return Ok(message);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteMessage(Guid id)
            {
                var result = await _messageService.DeleteMessage(id);
                if (!result)
                {
                    return NotFound();
                }
                return Ok(result);
            }

            [HttpGet("conversation/{conversationId}")]
            public async Task<IActionResult> GetMessagesByConversationId(Guid conversationId)
            {
                var messages = await _messageService.GetMessagesByConversationId(conversationId);
                if (messages is null)
                {
                    return NotFound();
                }

                // Join the conversation using SignalR hub
                await _hubContext.Clients.All.SendAsync("JoinConversation", conversationId.ToString());

                return Ok(messages);
            }

        }
    }
}
