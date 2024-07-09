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

        public async Task<(IEnumerable<Member> members, int totalCount)> GetMembers(string? search, int page, int pagesize)
        {
            var members = DMDB.Members.Include(m => m.Pictures).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                members = members.Where(m => m.Fullname.ToLower().Contains(search) || m.Introduce.ToLower().Contains(search));
            }
            page = page == 0 ? 1 : page;
            pagesize = pagesize == 0 ? 5 : pagesize;
            var totalCount = await members.CountAsync();
            members = members.Skip((page - 1) * pagesize).Take(pagesize);

            return (members, totalCount);
        }

        public async Task<Member?> GetMemberById(Guid id)
        {
            return await DMDB.Members.Include(m => m.Pictures).Include(m => m.Hobbies)

                                     .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
