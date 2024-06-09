using System.Linq.Expressions;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        IQueryable<TModel> GetAllAsync();
        public Task<int> CountAsync(Expression<Func<TModel, bool>> expression);
        public Task<bool> AnyAsync(Expression<Func<TModel, bool>> expression);
        public Task<TModel?> GetByIdAsync(Guid id);
        public Task<List<TModel>> GetByFilterAsync(Expression<Func<TModel, bool>> expression);
        public void Add(TModel obj);
        public Task AddRangeAsync(IEnumerable<TModel> tmodel);
        public void Update(TModel obj);
        public void Remove(TModel obj);
        public Task<bool> SaveChangeAsync();
    }
}
