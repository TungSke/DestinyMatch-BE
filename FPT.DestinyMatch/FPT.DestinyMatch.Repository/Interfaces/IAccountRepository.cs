using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        public Task<Account?> GetValidAccountByEmail(string email);
        public Task<Account?> GetByIdIncludeMember(Guid accountId);
        public Task<IEnumerable<Account>> GetListAsync(int amountItem, int pageIndex,
            string? keyword, bool sortByDate, string? statusSearch, string? roleSearch, bool sortDescending);

    }
}