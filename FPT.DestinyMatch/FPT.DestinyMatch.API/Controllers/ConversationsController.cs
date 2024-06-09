using FPT.DestinyMatch.API.Models.ResponseModels;
using FPT.DestinyMatch.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FPT.DestinyMatch.API.Models.RequestModels;

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
        [Route("view-detail{id}")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> GetConversationDetail([FromRoute] Guid id)
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

            // Declare target other member base on who is requesting
            Guid otherMemberId;
            if (currentMemberId == currentConversation.FirstMemberId)
            {
                otherMemberId = currentConversation.SecondMemberId;
                return Ok(new ConversationDetail
                {
                    Id = currentConversation.Id,
                    Name = currentConversation.SecondName,//Display the other member name -> Not the interacting member
                    RecentlyTime = currentConversation.RecentlyActivity,
                    CreateTime = currentConversation.CreatedAt,
                    ChattingMemberId = otherMemberId
                });
            }

            otherMemberId = currentConversation.FirstMemberId;

            return Ok(new ConversationDetail
            {
                Id = currentConversation.Id,
                Name = currentConversation.FirstName,
                RecentlyTime = currentConversation.RecentlyActivity,
                CreateTime = currentConversation.CreatedAt,
                ChattingMemberId = otherMemberId
            });
        }

        [HttpGet]
        [Route("my-conversation-list")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> GetListByMemberId([FromBody] GuidRequest currentMember)
        {
            var defaultList = await _conversationService.GetConversationList(currentMember.Id);
            var customList = defaultList.Select(c => new ConversationDetail
            {
                Id = c.Id,
                Name = (currentMember.Id == c.FirstMemberId) ? c.SecondName : c.FirstName,
                RecentlyTime = c.RecentlyActivity,
                CreateTime = c.CreatedAt,
                ChattingMemberId = (currentMember.Id == c.FirstMemberId) ? c.SecondMemberId : c.FirstMemberId
            }).OrderByDescending(c => c.RecentlyTime);
            return Ok(customList);
        }

        [HttpPost]
        [Route("create-new")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> NewConversation([FromBody] GuidRequest withMember)
        {
            Guid interactingMemberId;
            if (Guid.TryParse
                (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out interactingMemberId)
                == false)
            {
                return Unauthorized("Member Id is missing or not available");
            };

            var newConversation = await _conversationService.StartNewConversation(interactingMemberId, withMember.Id);
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
        public async Task<IActionResult> RenameConversation([FromBody] RenamingConversationRequest request)
        {
            // Declare current member using
            Guid interactingMemberId;
            if (Guid.TryParse
                (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out interactingMemberId)
                == false)
            {
                return Unauthorized("Member Id is missing or not available");
            };
            return await _conversationService.ChangeNameConversation(request.ConversationId, interactingMemberId, request.NewName) ?
                Ok("Rename Success") : BadRequest("Rename failed!"); ;
        }

        [HttpDelete]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> DeleteConveration([FromBody] GuidRequest request)
        {
            return await _conversationService.DeleteConversation(request.Id) ?
                Ok("Delete Success") : BadRequest("Delete failed!");
        }
    }
}
