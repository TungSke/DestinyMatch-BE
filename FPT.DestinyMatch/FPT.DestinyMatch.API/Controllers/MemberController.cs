using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

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

        [HttpGet]
        public async Task<IActionResult> GetMembers(string? search, bool? gender, int? minAge, int? maxAge, int page, int pagesize)
        {
            try
            {
                var (data, count) = await _memberService.GetMembers(search, gender, minAge, maxAge, page, pagesize);
                return Ok(new { data, count });
            }
            catch
            {
                return BadRequest();
            }
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
        public async Task<ActionResult<bool>> CheckAccountExistsInMember(Guid accountId)
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
    }
}
