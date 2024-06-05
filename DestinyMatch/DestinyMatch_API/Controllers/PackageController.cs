using BusinessLogic.Interfaces;
using BusinessLogic.Models.Request;
using BusinessLogic.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace DestinyMatch_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPackages()
        {
            var packages = await _packageService.GetPackages();
            return Ok(packages);
        }

        

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPackageById(Guid id)
        {
            var package = await _packageService.GetPackageById(id);
            return Ok(package);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackageAsync(PackageRequest package)
        {
            var res = await _packageService.CreatePackageAsync(package);
            if(res == false)
            {
                return BadRequest();
            }
            return Ok(package);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePackageAsync(PackageResponse package)
        {
            var res = await _packageService.UpdatePackageAsync(package);
            if (res == false)
            {
                return BadRequest();
            }
            return Ok(package);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackageAsync(Guid id)
        {
            var res = await _packageService.DeletePackageAsync(id);
            if (res == false)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
