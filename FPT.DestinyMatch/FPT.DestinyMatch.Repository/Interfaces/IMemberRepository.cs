using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        public Task<(IEnumerable<Member> members, int totalCount)> GetMembers(string? search, int page, int pagesize);
        Task<Member?> GetMemberById(Guid id);
        Task<bool> CheckAccountExistsInMember(Guid accountId);
    }
}
