using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs.Picture;
using System.Runtime.CompilerServices;

namespace DestinyMatch_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pictureController : ControllerBase
    {
        private readonly IPictureService _pictureService;

        public pictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file,Guid memberId)
        {
            if (file == null)
            {
                return BadRequest("No file was uploaded");
            }

            var downloadUrl = await _pictureService.UploadImage(file, memberId);
            return Ok(downloadUrl);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPictureById(Guid id)
        {
            var picture = await _pictureService.GetPictureById(id);
            return Ok(picture);
        }

        [HttpGet("user/{userid}")]
        public async Task<IActionResult> GetAllPicturesFromUser(Guid userid)
        {
            var pictures = await _pictureService.getAllPicturfromusers(userid);
            return Ok(pictures);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePicture(GetPicture picture)
        {
            await _pictureService.UpdatePicture(picture);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePicture(Guid id, string urlPictureOfUser)
        {
            await _pictureService.DeletePicture(id, urlPictureOfUser);
            return Ok();
        }
    }
}
