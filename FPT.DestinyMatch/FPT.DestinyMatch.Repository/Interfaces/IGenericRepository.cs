using System.Linq.Expressions;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        IQueryable<TModel> GetAsync();
        public Task<int> CountAsync(Expression<Func<TModel, bool>> expression);
        public Task<bool> AnyAsync(Expression<Func<TModel, bool>> expression);
        public Task<TModel?> GetByIdAsync(Guid id);
        public Task<TModel?> GetByFilterAsync(Expression<Func<TModel, bool>> expression);
        public Task<List<TModel>> GetListByFilterAsync(Expression<Func<TModel, bool>> expression);
        public Task<TModel> Add(TModel obj);
        public Task AddRangeAsync(IEnumerable<TModel> tmodel);
        public Task Update(TModel obj);
        public Task Remove(TModel obj);
        public Task<bool> SaveChangeAsync();
    }
}
