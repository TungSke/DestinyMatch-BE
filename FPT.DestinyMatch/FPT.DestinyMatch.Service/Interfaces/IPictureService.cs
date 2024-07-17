using Microsoft.AspNetCore.Http;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Models.Request;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IPictureService
    {
        Task<string> UploadImage(IFormFile file, Guid memberId);
        Task<IEnumerable<Picture>> getAllPicturfromusers(Guid userid);
        Task<Picture> GetPictureById(Guid id);
        Task UpdatePicture(IFormFile file, PictureRequest picture);
        Task DeletePicture(Guid id);
    }
}
