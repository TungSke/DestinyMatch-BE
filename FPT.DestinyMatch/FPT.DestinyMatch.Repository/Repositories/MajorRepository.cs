using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class MajorRepository : GenericRepository<Major>, IMajorRepository
    {                               //inheritance               implement interface
        //************************[ DECLARATION ]************************
        public MajorRepository(DestinyMatchContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Major> majors, int totalCount)> GetMajors(string? search, int page, int pagesize)
        {
            var majors = DMDB.Majors.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                majors = majors.Where(m => m.Name.Contains(search) || m.Code.Contains(search));
            }
            page = page == 0 ? 1 : page;
            pagesize = pagesize == 0 ? 5 : pagesize;
            var totalCount = await majors.CountAsync();
            majors = majors.Skip((page - 1) * pagesize).Take(pagesize);

            return (majors, totalCount);
        }
    }
}
