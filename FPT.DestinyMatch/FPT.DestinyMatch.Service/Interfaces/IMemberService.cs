using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Models.Response;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IMemberService
    {
        Task<(IEnumerable<Member> members, int totalCount)> GetMembers(string search, int page, int pagesize);

        Task<Member?> GetMemberById(Guid id);
        Task<bool> DeleteMeber(Guid memberId);
        Task<Member> CreateMember(MemberRequest memberRequest);
        Task<Member> UpdateMember(Guid Id, MemberRequest memberRequest);
        Task<Member> GetMemberByAccountId(Guid id);
    }
}
