using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(DestinyMatchContext _dbcontext) : base(_dbcontext)
        {
        }

        public async Task<Member?> GetMemberById(Guid id)
        {
            return await DMDB.Members.Include(m => m.Pictures).Include(m => m.Hobbies)

                                     .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> CheckAccountExistsInMember(Guid accountId)
        {
            return await DMDB.Members.AnyAsync(m => m.AccountId == accountId);
        }

        public async Task<(IEnumerable<Member>? ResultList, int TotalCount, int CurrentPage, int CurrentAmount)> GetListMember_Search(
            int amount, int pageIndex,
            string? emailKeyword, string? nameKeyword, bool? genderType, string? statusType,
            string? universityKeyword, string? majorKeyword, List<string>? hobbyList,
            int? minAge, int? maxAge, bool orderByName_Descending)
        {
            var query = DMDB.Members
                .Include(m => m.Account)
                .Include(m => m.University)
                .Include(m => m.Major)
                .Include(m => m.Hobbies)
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();

            // Apply search
            if (!string.IsNullOrEmpty(emailKeyword))
            {
                query = query.Where(mem => mem.Account.Email.ToLower().Contains(emailKeyword.ToLower()));
            }

            if (!string.IsNullOrEmpty(nameKeyword))
            {
                query = query.Where(mem => mem.Fullname.ToLower().Contains(nameKeyword.ToLower()));
            }

            if (genderType is not null)
            {
                query = query.Where(mem => mem.Gender == genderType);
            }

            if (!string.IsNullOrEmpty(statusType))
            {
                query = query.Where(mem => mem.Status.ToLower().Equals(statusType.ToLower()));
            }

            if (!string.IsNullOrEmpty(universityKeyword))
            {
                query = query.Where(mem => mem.University.Code.ToLower().Contains(universityKeyword.ToLower()));
                query = query.Where(mem => mem.University.Name.ToLower().Contains(universityKeyword.ToLower()));
            }

            if (!string.IsNullOrEmpty(majorKeyword))
            {
                query = query.Where(mem => mem.Major.Code.ToLower().Contains(majorKeyword.ToLower()));
                query = query.Where(mem => mem.Major.Name.ToLower().Contains(majorKeyword.ToLower()));
            }

            if (hobbyList is not null || hobbyList.Any())
            {
                var lowerHobbiesList = hobbyList.Select(h => h.ToLower()).ToList();
                query = query.Where(mem => mem.Hobbies.Any(input => lowerHobbiesList.Contains(input.Name.ToLower())));
            }

            if (minAge.HasValue && maxAge.HasValue)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var minDob = today.AddYears(-maxAge.Value);
                var maxDob = today.AddYears(-minAge.Value);

                query = query.Where(mem => mem.Dob >= minDob && mem.Dob <= maxDob);
            }

            // Sort by Name
            query = orderByName_Descending ?
                query.OrderByDescending(mem => mem.Fullname) : query.OrderBy(mem => mem.Fullname);


            int totalCount = query.Count();
            // Apply paging
            var pagedAccounts = await query
                .Skip((pageIndex - 1) * amount)
                .Take(amount)
                .ToListAsync();

            return (pagedAccounts, totalCount, pageIndex, amount);
        }
    }
}
