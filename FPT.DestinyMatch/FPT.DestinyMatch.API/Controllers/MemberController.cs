using FPT.DestinyMatch.API.Models.RequestModels.Paging;
using FPT.DestinyMatch.API.Models.ResponseModels;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace FPT.DestinyMatch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(Guid id)
        {
            var member = await _memberService.GetMemberById(id);
            if (member is null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpGet("accountid")]
        public async Task<IActionResult> GetMemberByAccountId(Guid id)
        {
            var member = await _memberService.GetMemberByAccountId(id);
            if (member is null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpGet("exists")]
        public async Task<ActionResult<bool>> CheckAccountExistsInMember( Guid accountId)
        {
            var exists = await _memberService.CheckAccountExistsInMember(accountId);
            return Ok(exists);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMember([FromBody] MemberRequest memberRequest)
        {
            var member = await _memberService.CreateMember(memberRequest);
            return Ok(member);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(Guid id, [FromBody] MemberRequest memberRequest)
        {
            var member = await _memberService.UpdateMember(id, memberRequest);
            if (member is null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var result = await _memberService.DeleteMeber(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("list")]
        public async Task<IActionResult> FilterMember([FromBody] MemberPaging filter)
        {
            var mixObj = await _memberService.SearchMember(
                filter.Amount,filter.Page,
                filter.EmailKeyword, filter.NameKeyword, filter.Gender, filter.Status,
                filter.UniversityKeyword, filter.MajorKeyword, filter.HobbyList,
                filter.MinAge, filter.MaxAge, filter.OrderByName_Descending);

            return Ok(new PagedResultResponse<Member>
            {
                PageIndex = mixObj.CurrentPage,
                PageSize = mixObj.CurrentAmount,
                TotalCount = mixObj.TotalCount,
                ResultsList = mixObj.ResultList
            });
        }
    }
}
