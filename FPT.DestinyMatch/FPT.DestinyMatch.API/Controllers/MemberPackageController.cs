using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace FPT.DestinyMatch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberPackageController : ControllerBase
    {
        private readonly IMemberPackageService _memberPackage;

        public MemberPackageController(IMemberPackageService memberPackage)
        {
            _memberPackage = memberPackage;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var memberPackage = await _memberPackage.GetMemberPackages();
            return Ok(memberPackage);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberPackageById(Guid id)
        {
            var member = await _memberPackage.GetMemberPackageById(id);
            if (member is null)
            {
                return BadRequest();
            }
            return Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMemberPackage([FromBody] MemberPackageRequest memberPackage)
        {
            var member = await _memberPackage.CreateMemberPackage(memberPackage);
            return Ok(member);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMemberPackage(Guid id, [FromBody] MemberPackageRequest memberPackage)
        {
            var member = await _memberPackage.UpdateMemberPackage(id, memberPackage);
            if (member is null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemberPackage(Guid id)
        {
            var result = await _memberPackage.DeleteMeberPackage(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
