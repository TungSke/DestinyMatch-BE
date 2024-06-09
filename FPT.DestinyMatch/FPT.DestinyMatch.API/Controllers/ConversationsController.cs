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
            Guid otherMemberId;
            if (currentMemberId == currentConversation.FirstMemberId)
            {
                otherMemberId = currentConversation.SecondMemberId;
                return Ok(new ConversationDetail
                {
                    Id = currentConversation.Id,
                    Name = currentConversation.SecondName,//Display the other member name -> Not the interacting member
                    CreateTime = currentConversation.CreatedAt,
                    ChattingMember = otherMemberId
                });
            }

            otherMemberId = currentConversation.FirstMemberId;

            return Ok(new ConversationDetail
            {
                Id = currentConversation.Id,
                Name = currentConversation.FirstName,
                CreateTime = currentConversation.CreatedAt,
                ChattingMember = otherMemberId
            });
        }

        [HttpGet]
        [Route("my-conversation-list")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> GetListByMemberId(Guid currentMemberId)
        {
            var defaultList = await _conversationService.GetConversationList(currentMemberId);
            var customList = defaultList.Select(c => new ConversationDetail
            {
                Id = c.Id,
                Name = (currentMemberId == c.FirstMemberId) ? c.SecondName : c.FirstName,
                CreateTime = c.CreatedAt,
                ChattingMember = (currentMemberId == c.FirstMemberId) ? c.SecondMemberId : c.FirstMemberId
            });
            return Ok(customList);
        }

        [HttpPost]
        [Route("create-new")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> NewConversation(Guid withMemberId)
        {
            Guid interactingMemberId;
            if (Guid.TryParse
                (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out interactingMemberId)
                == false)
            {
                return Unauthorized("Member Id is missing or not available");
            };

            var newConversation = await _conversationService.StartNewConversation(interactingMemberId, withMemberId);
            return Ok(new ConversationDetail
            {
                Id = newConversation.Id,
                Name = newConversation.SecondName,//Display the other member name -> Not the interacting member
                ChattingMember = newConversation.SecondMemberId,
                CreateTime = DateTime.Now
            });
        }

        [HttpPatch]
        [Route("rename-conversation")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> RenameConversation([FromBody] Guid conversationId, string newName)
        {
            // Declare current member using
            Guid interactingMemberId;
            if (Guid.TryParse
                (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out interactingMemberId)
                == false)
            {
                return Unauthorized("Member Id is missing or not available");
            };
            return await _conversationService.ChangeNameConversation(conversationId, interactingMemberId, newName) ?
                Ok("Rename Success") : BadRequest("Rename failed!"); ;
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> DeleteConveration([FromRoute] Guid id)
        {
            return await _conversationService.DeleteConversation(id) ?
                Ok("Delete Success") : BadRequest("Delete failed!");
        }
    }
}
