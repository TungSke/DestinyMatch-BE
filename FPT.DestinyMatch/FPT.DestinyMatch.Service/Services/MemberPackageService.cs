using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace FPT.DestinyMatch.Service.Services
{
    public class MemberPackageService : IMemberPackageService
    {
        private readonly IMemberPackageRepository _memberPackageRepository;

        public MemberPackageService(IMemberPackageRepository memberPackageRepository)
        {
            _memberPackageRepository = memberPackageRepository;
        }
        public async Task<MemberPackage> CreateMemberPackage(MemberPackageRequest memberRequest)
        {
            var memberPackage = new MemberPackage
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.Now,
                EndDate = memberRequest.EndDate,
                MemberId = memberRequest.MemberId,
                PackageId = memberRequest.PackageId
            };
            _memberPackageRepository.Add(memberPackage);
            await _memberPackageRepository.SaveChangeAsync();
            return memberPackage;
        }

        public async Task<bool> DeleteMeberPackage(Guid Id)
        {
            var memberPackage = await _memberPackageRepository.GetByIdAsync(Id);
            if (memberPackage is null)
            {
                return false;
            }
            _memberPackageRepository.Remove(memberPackage);
            await _memberPackageRepository.SaveChangeAsync();
            return true;
        }

        public async Task<MemberPackage?> GetMemberPackageById(Guid id)
        {
            return await _memberPackageRepository.GetByIdAsync(id);

        }

        public async Task<IEnumerable<MemberPackage>> GetMemberPackages()
        {
            return await _memberPackageRepository.GetAsync().ToListAsync();
        }

        public async Task<MemberPackage> UpdateMemberPackage(Guid Id, MemberPackageRequest memberRequest)
        {
            var memberPackage = await _memberPackageRepository.GetByIdAsync(Id);
            if (memberPackage is null)
            {
                return null;
            }

            memberPackage.EndDate = memberRequest.EndDate;
            memberPackage.MemberId = memberRequest.MemberId;
            memberPackage.PackageId = memberRequest.PackageId;
            _memberPackageRepository.Update(memberPackage);
            await _memberPackageRepository.SaveChangeAsync();
            return memberPackage;
        }
    }
}
