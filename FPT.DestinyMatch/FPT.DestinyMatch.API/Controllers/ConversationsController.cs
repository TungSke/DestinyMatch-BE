using FPT.DestinyMatch.API.Models.ResponseModels;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;

namespace FPT.DestinyMatch.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : Controller
    {
        private readonly IConversationService _conversationService;
        public ConversationsController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpGet]
        [Route("view-detail")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> GetConversationDetail(Guid id)
        {
            Guid currentMemberId;
            if (Guid.TryParse
                (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out currentMemberId)
                == false)
            {
                return BadRequest("Wrong conversation Id format");
            };
            var conversation = await _conversationService.GetConversationDetail(id, currentMemberId);
            if (conversation is null)
            {
                BadRequest("Not found this conversation");
            }

            return Ok(new ConversationDetail
            {
                Id = conversation.Id,
                Name = conversation.Name,
                Create_time = conversation.CreatedAt,

            });
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> GetListByMemberId()
        {
            return Ok();
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> NewConversation()
        {
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> DeleteConveration()
        {
            return Ok();
        }
    }
}
