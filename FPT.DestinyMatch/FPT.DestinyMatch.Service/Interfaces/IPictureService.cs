using Microsoft.AspNetCore.Http;
using Repository.DTOs.Picture;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IPictureService
    {
        Task<string> UploadImage(IFormFile file, Guid memberId);
        Task<IEnumerable<Picture>> getAllPicturfromusers(Guid userid);
        Task<Picture> GetPictureById(Guid id);
        Task UpdatePicture(GetPicture picture);
        Task DeletePicture(Guid id, string urlPictureOfUser);
    }
}
