using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetMembers();
        
        Task<Member?> GetMemberById(Guid id);
        Task<bool> DeleteMeber(Guid memberId);
         Task<Member> CreateMember(MemberRequest memberRequest);
         Task<Member> UpdateMember(Guid Id, MemberRequest memberRequest);
        
    }
}
