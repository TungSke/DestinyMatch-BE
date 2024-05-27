using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Models;
using Repository.Models.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class, GenericModel<Guid> //must be a class and must implement GenericModel 
    {
        //************************[ DECLARATION ]************************

        protected readonly DestinyMatchContext DMDB;

        public GenericRepository(DestinyMatchContext _dbcontext)
        {
            DMDB = _dbcontext;
        }

        //**************************[ METHODS ]**************************

        public virtual async Task<TModel?> GetByIdAsync(Guid id)
        {
            return await DMDB.Set<TModel>().SingleOrDefaultAsync(model => model.Id == id);
        }
        public void Add(TModel obj)
        {
            DMDB.Set<TModel>().Add(obj);
        }
        public void Update(TModel obj)
        {
            DMDB.Set<TModel>().Update(obj);
        }
        public void Remove(TModel obj)
        {
            DMDB.Set<TModel>().Remove(obj);
        }

        public Task SaveChangeAsync()
        {
            return DMDB.SaveChangesAsync();
        }
    }
}
