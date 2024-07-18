using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Repository.Models;
namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IMemberService
    {
        Task<(IEnumerable<MemberResponse> members, int totalCount)> GetMembers(string search, int page, int pagesize);

        Task<MemberResponse?> GetMemberById(Guid id);
        Task<bool> DeleteMeber(Guid memberId);
        Task<Member> CreateMember(MemberRequest memberRequest);
        Task<Member> UpdateMember(Guid Id, MemberRequest memberRequest);
        Task<Member> GetMemberByAccountId(Guid id);
        Task<bool> CheckAccountExistsInMember(Guid accountId);
        Task<(IEnumerable<Member> ResultList, int TotalCount, int CurrentPage, int CurrentAmount)>
            SearchMember(int amount, int pageIndex, string? emailKeyword, string? nameKeyword, bool? genderType, string? statusType, string? universityKeyword, string? majorKeyword, List<string>? hobbyList, int? minAge, int? maxAge, bool orderByName_Descending);
    }
}
