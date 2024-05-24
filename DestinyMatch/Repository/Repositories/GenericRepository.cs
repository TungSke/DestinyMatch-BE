using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DestinyMatchContext _context;

        public GenericRepository(DestinyMatchContext context)
        {
            _context = context;
        }
    }
}
