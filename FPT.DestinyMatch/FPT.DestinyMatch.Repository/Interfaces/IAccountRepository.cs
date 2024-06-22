using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        public Task<Account?> GetByEmailAsync(string email);
    }
}