using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        public Task<Account?> GetByEmailAsync(string email);
        public Task<IEnumerable<Account>> GetListAsync(int amountItem, int pageIndex,
            string? keyword, bool sortByDate, string? statusSearch, string? roleSearch, bool sortDescending);
    }
}