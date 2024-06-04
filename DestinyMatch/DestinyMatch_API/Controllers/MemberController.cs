using BusinessLogic.Interfaces;
using BusinessLogic.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DestinyMatch_API.Controllers
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
        public async Task<IActionResult> GetAllMember()
        {
            var members = await _memberService.GetMembers();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(Guid id)
        {
            var member = await _memberService.GetMemberById(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
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
            if (member == null)
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
