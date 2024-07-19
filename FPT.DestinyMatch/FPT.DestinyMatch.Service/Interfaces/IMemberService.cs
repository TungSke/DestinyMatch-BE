using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Models.Response;
namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IMemberService
    {
        Task<(IEnumerable<MemberResponse> members, int totalCount)> GetMembers(string search, bool? gender, int? minAge, int? maxAge, int page, int pagesize);

        Task<MemberResponse?> GetMemberById(Guid id);
        Task<bool> DeleteMeber(Guid memberId);
        Task<Member> CreateMember(MemberRequest memberRequest);
        Task<Member> UpdateMember(Guid Id, MemberRequest memberRequest);
        Task<Member> GetMemberByAccountId(Guid id);
        Task<bool> CheckAccountExistsInMember(Guid accountId);
    }
}
