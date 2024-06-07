using Microsoft.AspNetCore.Http;
using Firebase.Storage;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using Repository.Interfaces;
using BusinessLogic.Interfaces;
using Repository.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DTOs.Picture;
using Mapster;

namespace BusinessLogic.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly string _bucket = "destinymatch-70b72.appspot.com";

        public PictureService(IPictureRepository pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        public async Task<string> UploadImage(IFormFile file, Guid memberId)
        {
            var cancellationToken = new CancellationToken();

            var task = new FirebaseStorage(_bucket, new FirebaseStorageOptions
            {
                ThrowOnCancel = true
            })
            .Child("imgs")
            .Child(file.FileName)
            .PutAsync(file.OpenReadStream(), cancellationToken);
            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");
            var downloadUrl = await task;

            GetPicture picture = new GetPicture
            {
                UrlPath = downloadUrl,
                MemberId = memberId
            };
            await AddPicture(picture);
            return downloadUrl;
        }


        private async Task AddPicture(GetPicture picture)
        {
            picture.Id = Guid.NewGuid();
            var pic = picture.Adapt<Picture>();
            _pictureRepository.Add(pic);
            await _pictureRepository.SaveChangeAsync();
        }

        public async Task<IEnumerable<Picture>> getAllPicturfromusers(Guid userid)
        {
            return await _pictureRepository.GetAllAsync().Where(x => x.MemberId.Equals(userid)).ToListAsync();
        }

        public async Task<Picture> GetPictureById(Guid id)
        {
            return await _pictureRepository.GetByIdAsync(id);
        }

        public async Task UpdatePicture(GetPicture picture)
        {
            var pic = picture.Adapt<Picture>();
             _pictureRepository.Update(pic);
            await _pictureRepository.SaveChangeAsync();
        }

        public async Task DeletePicture(Guid id, string urlPictureOfUser)
        {
            var picture = await _pictureRepository.GetByIdAsync(id);
            _pictureRepository.Remove(picture);
            await _pictureRepository.SaveChangeAsync();
            await DeletePictureinFirebase(urlPictureOfUser);
        }

        private async Task DeletePictureinFirebase(string urlPictureOfUser)
        {
            var task = new FirebaseStorage(_bucket, new FirebaseStorageOptions
            {
                ThrowOnCancel = true
            })
            .Child("imgs")
            .Child(urlPictureOfUser)
            .DeleteAsync();
            await task;
        }

    }
}
