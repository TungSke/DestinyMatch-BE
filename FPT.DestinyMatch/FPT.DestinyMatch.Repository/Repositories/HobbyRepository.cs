using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class HobbyRepository : GenericRepository<Hobby>, IHobbyReposiroty
    {                               //inheritance               implement interface
        //************************[ DECLARATION ]************************
        public HobbyRepository(DestinyMatchContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Hobby> hobbies, int totalCount)> GetHobbies(string? search, int page, int pagesize)
        {
            var hobbies = DMDB.Hobbies.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                hobbies = hobbies.Where(h => h.Name.Contains(search) || h.Description.Contains(search));
            }

            var totalCount = await hobbies.CountAsync();
            hobbies = hobbies.Skip((page - 1) * pagesize).Take(pagesize);

            return (hobbies, totalCount);
        }




    }
}
