using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        public Task<bool> CreateAccountAsync(string email, string password);
        public Task<bool> LoginByPassWord(string email, string password);
    }
}
