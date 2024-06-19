using FPT.DestinyMatch.API.Models.ResponseModels;
using FPT.DestinyMatch.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FPT.DestinyMatch.API.Models.RequestModels;
using FPT.DestinyMatch.API.Models.RequestModels.Paging;
using FPT.DestinyMatch.Service.Services;

namespace FPT.DestinyMatch.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : Controller
    {
        private readonly ChatHub chatHub;
        private readonly IConversationService _conversationService;
        public ConversationsController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> GetConversationDetail([FromRoute] Guid id)
        {
            // Declare current member using
            Guid currentMemberId;
            if (Guid.TryParse
                (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out currentMemberId)
                == false)
            {
                return Unauthorized("Member Id is missing or not available for validate permission");
            };
            var currentConversation = await _conversationService.GetConversationDetailAsync(id, currentMemberId);

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
        [Route("recently-list/{id}&{page}")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> GetListRecentlyConversation([FromRoute] Guid id, int page)
        {
            // Declare current member using
            Guid currentMemberId;
            if (Guid.TryParse
                (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out currentMemberId)
                == false)
            {
                return Unauthorized("Member Id is missing or not available");
            };
            if (id != currentMemberId)
            {
                return Unauthorized("You don't have permission to view this member's conversations");
            }
            var conversationList = await _conversationService.GetRecentlyConversationListAsync(id, page);
            return Ok(conversationList);
        }

        [HttpPost]
        [Route("list")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> SearchConversation([FromBody] ConversationPaging inputData)
        {
            Guid currentMemberId;
            if (Guid.TryParse
                (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out currentMemberId)
                == false)
            {
                return Unauthorized("Member Id is missing or not available");
            };
            if (inputData.CurrentUsingMemberId != currentMemberId)
            {
                return Unauthorized("You don't have permission to view this member's conversations");
            }
            var conversationList = await _conversationService.SearchConversationsListAsync
                (inputData.Amount,
                inputData.Page,
                inputData.CurrentUsingMemberId,
                inputData.NameKeyword,
                inputData.Status,
                inputData.OrderByDescending);
            return Ok(conversationList);
        }

        [HttpPost]
        [Route("new")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> NewConversation([FromBody] GuidRequestor withMember)
        {
            Guid interactingMemberId;
            if (Guid.TryParse
                (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out interactingMemberId)
                == false)
            {
                return Unauthorized("Member Id is missing or not available");
            };

            var newConversation = await _conversationService.StartNewConversationAsync(interactingMemberId, withMember.Id);
            return Ok(new ConversationDetail
            {
                Id = newConversation.Id,
                Name = newConversation.SecondName,//Display the other member name -> Not the interacting member
                ChattingMemberId = newConversation.SecondMemberId,
                RecentlyTime = DateTime.Now,
                CreateTime = DateTime.Now
            });
        }

        [HttpPatch]
        [Route("new-name")]
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
            return await _conversationService.ChangeNameConversationAsync(request.ConversationId, interactingMemberId, request.NewName) ?
                Ok("Rename Success") : BadRequest("Rename failed!"); ;
        }

        [HttpDelete]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> DeleteConveration([FromBody] GuidRequestor request)
        {
            return await _conversationService.DeleteConversationAsync(request.Id) ?
                Ok("Delete Success") : BadRequest("Delete failed!");
        }
    }
}
