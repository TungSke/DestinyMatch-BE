using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<Member?> GetMemberById(Guid id);
        Task<bool> CheckAccountExistsInMember(Guid accountId);
        Task<(IEnumerable<Member>? ResultList, int TotalCount, int CurrentPage, int CurrentAmount)> GetListMember_Search(
            int amount, int pageIndex,
            string? emailKeyword, string? nameKeyword, bool? genderType, string? statusType,
            string? universityKeyword, string? majorKeyword, List<string>? hobbyList,
            int? minAge, int? maxAge, bool orderByName_Descending);
    }
}
