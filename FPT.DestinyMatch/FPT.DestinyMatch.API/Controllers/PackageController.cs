using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Service.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace FPT.DestinyMatch.API.Controllers
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
        public async Task<IActionResult> GetPackages(int pageIndex=1, int PageSize=10, string? searchString=null)
        {
            var packages = await _packageService.GetPackages(pageIndex, PageSize, searchString);
            return Ok(packages);
        }



        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPackageById(Guid id)
        {
            var package = await _packageService.GetPackageById(id);
            if(package == null) return NotFound("Package not found");
            return Ok(package);
        }

        [HttpPost]
        //[Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> CreatePackageAsync(PackageRequest package)
        {
            var res = await _packageService.CreatePackageAsync(package);
            if (res == false)
            {
                return BadRequest();
            }
            return Ok(package);
        }

        [HttpPut]
        //[Authorize(Roles = "admin,moderator")]
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
        /*[Authorize(Roles = "admin,moderator")]*/
        public async Task<IActionResult> DeletePackageAsync(Guid id)
        {
            var res = await _packageService.DeletePackageAsync(id);
            if (res == false)
            {
                return NotFound();
            }
            return Ok("delete success");
        }
    }
}
