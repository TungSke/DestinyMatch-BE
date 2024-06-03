using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs.Package;

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
        public async Task<IActionResult> CreatePackageAsync(CreatePackage package)
        {
            var res = await _packageService.CreatePackageAsync(package);
            if(res == false)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePackageAsync(UpdatePackage package)
        {
            var res = await _packageService.UpdatePackageAsync(package);
            if (res == false)
            {
                return NotFound();
            }
            return Ok();
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
