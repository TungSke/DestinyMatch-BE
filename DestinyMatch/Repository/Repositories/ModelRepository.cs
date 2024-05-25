using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ModelRepository : GenericRepository<Account>//inheritance
    {
        public ModelRepository(DestinyMatchContext context) : base(context)
        {
        }
        //Other method of this class
        //So Business Layer can call both generic methods and these other method by inject this class

        //If you want to change the way of generic method work:
        /*
        public override Task<Model> GetByIdAsync(Guid id)
        {
            return DMDB.Model
                .include(m => m.Object)
                .SingleOrDefaultAsync(m => m.Id == id);
        }
         */
    }
}
