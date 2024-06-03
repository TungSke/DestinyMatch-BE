using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class MemberPackageRepository : GenericRepository<MemberPackage>, IMemberPackageRepository
    {
        public MemberPackageRepository(DestinyMatchContext _dbcontext) : base(_dbcontext)
        {
        }
    }
}
