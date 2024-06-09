using FPT.DestinyMatch.API.Models.ResponseModels;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
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
            // Declare current member using
            Guid currentMemberId;
            if (Guid.TryParse
                (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out currentMemberId)
                == false)
            {
                return Unauthorized("Member Id is missing or not available");
            };
            var currentConversation = await _conversationService.GetConversationDetail(id, currentMemberId);
            if (currentConversation is null)
            {
                BadRequest("Not found this conversation");
            }

            // Declare target other member base on who is requesting
            Guid otherMemberId =
                (currentMemberId == currentConversation.FirstMemberId) ?
                currentConversation.SecondMemberId : currentMemberId;
            return Ok(new ConversationDetail
            {
                Id = currentConversation.Id,
                Name = currentConversation.Name,
                CreateTime = currentConversation.CreatedAt,
                ChattingMember = otherMemberId
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

        [HttpPatch]
        [Route("rename-conversation")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> RenameConversation([FromBody] Guid conversationId, string newName)
        {
            // Declare current member using
            Guid interactingMemberId;
            if (Guid.TryParse
                (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out currentMemberId)
                == false)
            {
                return Unauthorized("Member Id is missing or not available");
            };
            return await _conversationService.ChangeNameConversation(conversationId, newName)? Ok("Rename Success") : BadRequest("Rename failed!"); ;
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> DeleteConveration([FromRoute] Guid id)
        {
            return await _conversationService.DeleteConversation(id)? Ok("Delete Success"): BadRequest("Delete failed!");
        }
    }
}
