using BusinessLogic.Models.Response;
using Microsoft.AspNetCore.Http;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPictureService
    {
        Task<string> UploadImage(IFormFile file, Guid memberId);
        Task<IEnumerable<Picture>> getAllPicturfromusers(Guid userid);
        Task<Picture> GetPictureById(Guid id);
        Task UpdatePicture(PictureResponse picture);
        Task DeletePicture(Guid id, string urlPictureOfUser);
    }
}
