using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace FPT.DestinyMatch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HobbyController : Controller
    {
        private readonly IHobbyService _hobbyService;

        public HobbyController(IHobbyService hobbyService)
        {
            _hobbyService = hobbyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHobbies()
        {
            var hobbies = await _hobbyService.GetHobbies();
            return Ok(hobbies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHobbyById(Guid id)
        {
            var hobby = await _hobbyService.GetHobbyById(id);
            if (hobby is null)
            {
                return NotFound();
            }
            return Ok(hobby);
        }



        [HttpPost]
        public async Task<IActionResult> CreateHobby([FromBody] HobbyRequest hobbyRequest)
        {
            var hobby = await _hobbyService.CreateHobby(hobbyRequest);
            return Ok(hobby);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHobby(Guid id, [FromBody] HobbyRequest hobbyRequest)
        {
            var hobby = await _hobbyService.EditHobby(id, hobbyRequest);
            if (hobby is null)
            {
                return NotFound();
            }
            return Ok(hobby);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHobby(Guid id)
        {
            var hobby = await _hobbyService.DeleteHobby(id);
            if (!hobby)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
