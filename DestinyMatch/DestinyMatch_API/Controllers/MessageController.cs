using BusinessLogic.Interfaces;
using BusinessLogic.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;

namespace DestinyMatch_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
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
    }
}
