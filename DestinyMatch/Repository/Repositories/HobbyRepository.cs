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
    public class HobbyRepository : GenericRepository<Hobby>, IHobbyReposiroty
    {                               //inheritance               implement interface
        //************************[ DECLARATION ]************************
        public HobbyRepository(DestinyMatchContext context) : base(context)
        {
        }

    }
}
