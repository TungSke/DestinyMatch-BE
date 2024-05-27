using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAccounts();
        public Task<Account> GetByEmailAsync(string email);
        public Task<bool> CreateAccountAsync(string email, string password);
        public Task<Account> LoginByPassWord(string email, string password);
    }
}
