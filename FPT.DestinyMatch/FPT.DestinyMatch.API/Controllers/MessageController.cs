using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace FPT.DestinyMatch.API.Controllers
{
    namespace DestinyMatch_API.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class MessageController : ControllerBase
        {
            private readonly ChatHub _chatHub;
            private readonly IMessageService _messageService;

            public MessageController(IMessageService messageService, ChatHub chatHub)
            {
                _messageService = messageService;
                _chatHub=chatHub;
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
                if (message == null)
                {
                    return BadRequest();
                }
                return Ok(message);
            }

            [HttpPost]
            public async Task<IActionResult> CreateMessage([FromBody] MessageRequest messageRequest)
            {
                var message = await _messageService.CreateMessage(messageRequest);
                await _chatHub.SendMessage(message.ConversationId, message.Content);
                return Ok(message);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateMessage(Guid id, [FromBody] MessageRequest messageRequest)
            {
                var message = await _messageService.UpdateMessage(id, messageRequest);
                if (message == null)
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

            [HttpGet("{conversationId}")]
            public async Task<IActionResult> GetMessagesByConversationId(Guid conversationId)
            {
                var messages = await _messageService.GetMessagesByConversationId(conversationId);
                if (messages == null)
                {
                    return NotFound();
                }
                await _chatHub.JoinConversation(conversationId.ToString());
                return Ok(messages);
            }

        }
    }
}
