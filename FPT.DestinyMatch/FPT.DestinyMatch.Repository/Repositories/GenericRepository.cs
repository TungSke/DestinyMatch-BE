using FPT.DestinyMatch.Repository;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Repository.Repositories
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
        public IQueryable<TModel> GetAsync()
        {
            return DMDB.Set<TModel>().AsQueryable();
        }

        public async Task<int> CountAsync(Expression<Func<TModel, bool>> expression)
        {
            return await DMDB.Set<TModel>().CountAsync(expression);
        }

        public async Task<bool> AnyAsync(Expression<Func<TModel, bool>> expression)
        {
            return await DMDB.Set<TModel>().AnyAsync(expression);
        }

        public virtual async Task<TModel?> GetByIdAsync(Guid id)
        {
            return await DMDB.Set<TModel>().FindAsync(id);
        }
        public async Task<TModel?> GetByFilterAsync(Expression<Func<TModel, bool>> expression)
        {
            return await DMDB.Set<TModel>().FirstOrDefaultAsync(expression);
        }
        public async Task<List<TModel>> GetListByFilterAsync(Expression<Func<TModel, bool>> expression)
        {
            return await DMDB.Set<TModel>().Where(expression).ToListAsync();
        }

        public Task<TModel> Add(TModel obj)
        {
            DMDB.Set<TModel>().Add(obj);
            return Task.FromResult(obj);
        }

        public async Task AddRangeAsync(IEnumerable<TModel> tmodel)
        {
            await DMDB.Set<TModel>().AddRangeAsync(tmodel);
        }

        public Task Update(TModel obj)
        {
            DMDB.Set<TModel>().Update(obj);
            return Task.CompletedTask;
        }

        public Task Remove(TModel obj)
        {
            DMDB.Set<TModel>().Remove(obj);
            return Task.CompletedTask;
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await DMDB.SaveChangesAsync() > 0;
        }
    }
}
