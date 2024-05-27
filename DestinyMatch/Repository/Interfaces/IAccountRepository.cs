using Repository.Models;
using Repository.Repositories;

namespace Repository.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        public Task<Account?> GetByEmailAsync(string email);
    }
}