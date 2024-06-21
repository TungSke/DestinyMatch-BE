using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Extensions.Exceptions;
using FPT.DestinyMatch.Service.Interfaces;
using System.Drawing;
using System.Globalization;

namespace FPT.DestinyMatch.Service.Services
{
    public class VerificationService : IVerificationService
    {
        private readonly IVerificationRepository _verificationRepository;
        public VerificationService(IVerificationRepository verificationRepository)
        {
            _verificationRepository = verificationRepository;
        }

        //--------------------------[ IMPLEMENT ]--------------------------
        public async Task<Verification> GetVerificationDetailAsync(Guid verificationId)
        {
            var verification = await _verificationRepository.GetDetailAsync(verificationId) ?? throw new NotFoundException("Not found any verification with that id");
            return verification;
        }

        public async Task<bool> CreateVerificationAsync(string? submittedPicture, Guid memberId)
        {
            var existRequest = await _verificationRepository.GetByFilterAsync(
                ver => ver.MemberId == memberId && ver.Status!.Equals("Chưa Duyệt"));

            if (existRequest is not null)
            {
                throw new ConflictException("Cannot request another verification due to your last request has not approved yet");
            }

            await _verificationRepository.Add(new Verification
            {
                SubmittedPicture = submittedPicture,
                MemberId = memberId
            });
            return await _verificationRepository.SaveChangeAsync();
        }

        public async Task<IEnumerable<Verification>> GetListVerificationAsync(
            int amount, int page,
            Guid memberId,
            string? status,
            bool orderByAscending)
        {
            amount = amount == 0 ? 5 : amount;
            page = page == 0 ? 1 : page;

            var verificationList = await _verificationRepository.GetListVerificationAsync(amount, page, memberId, status, orderByAscending);
            if (verificationList.Any() == false)
            {
                throw new NotFoundException("Not found any account");
            }
            return verificationList;
        }

        public async Task<bool> UpdateStatusVerificationAsync(Guid verificationId, string newStatus)
        {
            if (newStatus.ToLower().Equals("đã duyệt") ||
                newStatus.ToLower().Equals("từ chối duyệt"))
            {
                var currentVerification = await _verificationRepository.GetByIdAsync(verificationId) ?? throw new NotFoundException("Not found any verification with that id");

                currentVerification.Status = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newStatus.ToLower());
                currentVerification.TimeStamp = DateTime.Now;
                return await _verificationRepository.SaveChangeAsync();
            }
            throw new BadRequestException("Wrong status format! Only accept one of these [đã duyệt , từ chối duyệt]");
        }

        public async Task<bool> DeleteVerificationAsync(Guid verificationId)
        {
            var currentVerification = await _verificationRepository.GetByIdAsync(verificationId) ?? throw new NotFoundException("Not found any verification with that id");

            currentVerification.Status = "Đã Xóa";
            currentVerification.TimeStamp = DateTime.Now;
            return await _verificationRepository.SaveChangeAsync();
        }
    }
}
