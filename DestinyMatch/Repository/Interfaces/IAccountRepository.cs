using Repository.Models;
using Repository.Repositories;

namespace Repository.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        public Task<bool> ExistEmailAsync(string email);
    }
}