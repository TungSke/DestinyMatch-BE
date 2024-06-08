namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        IQueryable<TModel> GetAsync();
        public Task<TModel?> GetByIdAsync(Guid id);
        public void Add(TModel obj);
        public void Update(TModel obj);
        public void Remove(TModel obj);
        public Task<bool> SaveChangeAsync();
    }
}
