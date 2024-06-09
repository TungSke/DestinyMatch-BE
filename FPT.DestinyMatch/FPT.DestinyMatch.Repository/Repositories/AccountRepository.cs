using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT.DestinyMatch.Repository.Repositories
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
            return acc;
        }
    }
}
