using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Models.Request;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IMemberPackageService
    {
        Task<IEnumerable<MemberPackage>> GetMemberPackages();

        Task<MemberPackage?> GetMemberPackageById(Guid id);
        Task<bool> DeleteMeberPackage(Guid Id);
        Task<MemberPackage> CreateMemberPackage(MemberPackageRequest memberRequest);
        Task<MemberPackage> UpdateMemberPackage(Guid Id, MemberPackageRequest memberRequest);
    }
}
