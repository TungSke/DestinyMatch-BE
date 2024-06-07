using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace FPT.DestinyMatch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorController : Controller
    {
        private readonly IMajorService _majorService;

        public MajorController(IMajorService majorService)
        {
            _majorService = majorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMajor()
        {
            var majors = await _majorService.GetAllMajor();
            return Ok(majors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMajorById(Guid id)
        {
            var major = await _majorService.GetMajorById(id);
            if (major == null)
            {
                return NotFound();
            }
            return Ok(major);
        }



        [HttpPost]
        public async Task<IActionResult> CreateMajor([FromBody] MajorRequest majorRequest)
        {
            var major = await _majorService.CreateMajor(majorRequest);
            return Ok(major);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMajor(Guid id, [FromBody] MajorRequest majorRequest)
        {
            var hobby = await _majorService.EditMajor(id, majorRequest);
            if (hobby == null)
            {
                return NotFound();
            }
            return Ok(hobby);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMajor(Guid id)
        {
            var major = await _majorService.DeleteMajor(id);
            if (!major)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
