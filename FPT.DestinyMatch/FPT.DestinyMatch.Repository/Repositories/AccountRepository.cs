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
        public async Task<IEnumerable<Account>> GetAccountList(
            int amountItem, int pageIndex,
            string? emailSearch = null,
            string? roleSearch = null,
            string? statusSearch = null,
            string? sortField = nameof(Account.CreateAt),
            bool sortDescending = true)
        {
            var query = DMDB.Accounts.AsQueryable();

            // Apply search
            if (!string.IsNullOrEmpty(emailSearch))
            {
                query = query.Where(a => a.Email.Contains(emailSearch));
            }

            if (!string.IsNullOrEmpty(roleSearch))
            {
                query = query.Where(a => a.Role == roleSearch);
            }

            if (!string.IsNullOrEmpty(statusSearch))
            {
                query = query.Where(a => a.Status == statusSearch);
            }

            // Sort
            switch (sortField)
            {
                case nameof(Account.CreateAt):
                    query = sortDescending
                        ? query.OrderByDescending(a => a.CreateAt)
                        : query.OrderBy(a => a.CreateAt);
                    break;
                case nameof(Account.Email):
                    query = sortDescending
                        ? query.OrderByDescending(a => a.Email)
                        : query.OrderBy(a => a.Email);
                    break;
                case nameof(Account.Role):
                    query = sortDescending
                        ? query.OrderByDescending(a => a.Role)
                        : query.OrderBy(a => a.Role);
                    break;
                case nameof(Account.Status):
                    query = sortDescending
                        ? query.OrderByDescending(a => a.Status)
                        : query.OrderBy(a => a.Status);
                    break;
            }

            // Paging
            var totalCount = await query.CountAsync();
            var accounts = await query
                .Skip((pageIndex - 1) * amountItem)
                .Take(amountItem)
                .ToListAsync();

            return accounts;
        }
    }
}
