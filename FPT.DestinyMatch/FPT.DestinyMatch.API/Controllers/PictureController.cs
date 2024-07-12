using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Service.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;


namespace FPT.DestinyMatch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IPictureService _pictureService;

        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([Required] IFormFile file, Guid memberId)
        {
            if (file is null)
            {
                return BadRequest("No file was uploaded");
            }
            Image image = Image.FromStream(file.OpenReadStream(), true, true);
            var newImage = new Bitmap(1980, 1080);
            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, 1980, 1080);
            }
            using (var outputStream = new MemoryStream())
            {
                newImage.Save(outputStream, ImageFormat.Jpeg);
                outputStream.Position = 0;

                var resizedFile = new FormFile(outputStream, 0, outputStream.Length, file.Name, file.FileName)
                {
                    Headers = file.Headers,
                    ContentType = file.ContentType
                };

                var downloadUrl = await _pictureService.UploadImage(resizedFile, memberId);
                return Ok(downloadUrl);
            }
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "member")]
        public async Task<IActionResult> GetPictureById(Guid id)
        {
            var picture = await _pictureService.GetPictureById(id);
            return Ok(picture);
        }

        [HttpGet("user/{userid}")]
        //[Authorize(Roles = "member")]
        public async Task<IActionResult> GetAllPicturesFromUser(Guid userid)
        {
            var pictures = await _pictureService.getAllPicturfromusers(userid);
            return Ok(pictures);
        }

        [HttpPut]
        //[Authorize(Roles = "member")]
        public async Task<IActionResult> UpdatePicture(IFormFile? file,[FromQuery] PictureRequest picture)
        {
            await _pictureService.UpdatePicture(file,picture);
            return Ok(picture);
        }

        [HttpDelete]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> DeletePicture(Guid pictureId)
        {
            await _pictureService.DeletePicture(pictureId);
            return Ok("delete success");
        }
    }
}
