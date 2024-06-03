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
    public class MajorRepository : GenericRepository<Major>, IMajorRepository
    {                               //inheritance               implement interface
        //************************[ DECLARATION ]************************
        public MajorRepository(DestinyMatchContext context) : base(context)
        {
        }

    }
}
