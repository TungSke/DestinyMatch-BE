using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {                               //inheritance               implement interface
        //************************[ DECLARATION ]************************
        public AccountRepository(DestinyMatchContext context) : base(context)
        {
        }

        //**************************[ METHODS ]**************************
        public async Task<Account?> GetByEmailAsync(string email)
        {
            var acc = await DMDB.Accounts.SingleOrDefaultAsync(a => a.Email == email);
            return  acc;
        }
    }
}
