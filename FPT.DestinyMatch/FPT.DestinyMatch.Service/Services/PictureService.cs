using Microsoft.AspNetCore.Http;
using Firebase.Storage;
using System.IO;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Mapster;
using FPT.DestinyMatch.Service.Models.Response;
using FPT.DestinyMatch.Service.Models.Request;

namespace FPT.DestinyMatch.Service.Services
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

            var task = new FirebaseStorage(_bucket, new FirebaseStorageOptions
            {
                ThrowOnCancel = true
            })
            .Child("imgs")
            .Child(file.FileName)
            .PutAsync(file.OpenReadStream());

            Console.WriteLine($"task: {file.OpenReadStream()}");
            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");
            var downloadUrl = await task;

            PictureResponse picture = new PictureResponse
            {
                UrlPath = downloadUrl,
                MemberId = memberId
            };
            await AddPicture(picture);
            return downloadUrl;
        }


        private async Task AddPicture(PictureResponse picture)
        {
            picture.Id = Guid.NewGuid();
            var pic = picture.Adapt<Picture>();
            _pictureRepository.Add(pic);
            await _pictureRepository.SaveChangeAsync();
        }

        public async Task<IEnumerable<Picture>> getAllPicturfromusers(Guid userid)
        {
            return await _pictureRepository.GetAsync().Where(x => x.MemberId.Equals(userid)).ToListAsync();
        }

        public async Task<Picture> GetPictureById(Guid id)
        {
            return await _pictureRepository.GetByIdAsync(id);
        }

        public async Task UpdatePicture(IFormFile? file, PictureRequest picture)
        {
            string downloadUrl = null;

            if (file != null)
            {
                var task = new FirebaseStorage(_bucket, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true
                })
                .Child("imgs")
                .Child(file.FileName)
                .PutAsync(file.OpenReadStream());

                task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");
                downloadUrl = await task;
            }

            var pic = await GetPictureById(picture.Id);
            if (pic is null)
            {
                throw new KeyNotFoundException("Picture not found");
            }


            if (picture.IsAvatar == true)
            {
                var userPictures = await _pictureRepository.GetAsync()
                    .Where(x => x.MemberId == pic.MemberId && x.Id != picture.Id)
                    .ToListAsync();

                foreach (var userPicture in userPictures)
                {
                    if (userPicture.IsAvatar == true)
                    {
                        userPicture.IsAvatar = false;
                        _pictureRepository.Update(userPicture);
                    }
                }
            }

            picture.Adapt(pic);
            pic.IsAvatar = picture.IsAvatar;
            pic.UrlPath = downloadUrl ?? pic.UrlPath;

            await _pictureRepository.SaveChangeAsync();
        }



        public async Task DeletePicture(Guid id)
        {
            var picture = await _pictureRepository.GetByIdAsync(id);
            if (picture is not null)
            {
                await DeletePictureInFirebase(picture.UrlPath);
                _pictureRepository.Remove(picture);
                await _pictureRepository.SaveChangeAsync();
            }
            else throw new KeyNotFoundException("Picture not found");
        }

        private async Task DeletePictureInFirebase(string urlPictureOfUser)
        {
            Uri uri = new Uri(urlPictureOfUser);
            string filename = System.Web.HttpUtility.UrlDecode(Path.GetFileName(uri.LocalPath));
            var storageReference = new FirebaseStorage(_bucket, new FirebaseStorageOptions
            {
                ThrowOnCancel = true
            }).Child("imgs").Child(filename);
            Console.WriteLine($"Delete file: {filename}");
            await storageReference.DeleteAsync();

        }
    }
}
