using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs.University;

namespace DestinyMatch_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversitityService _universityService;

        public UniversityController(IUniversitityService universityService)
        {
            _universityService = universityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUniversities()
        {
            var universities = await _universityService.GetUniversities();
            return Ok(universities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUniversityById(Guid id)
        {
            var university = await _universityService.GetUniversityById(id);
            return Ok(university);
        }

        [HttpPost]
        public async Task<IActionResult> AddUniversity(GetUniversity university)
        {
            var u = await _universityService.AddUniversity(university);
            return Ok(u);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUniversity(UpdateUni university)
        {
            var u = await _universityService.UpdateUniversity(university);
            return Ok(u);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUniversity(Guid id)
        {
            await _universityService.DeleteUniversity(id);
            return Ok();
        }
    }
}
