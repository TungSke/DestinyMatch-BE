using Repository.Models.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();
        public Task<TModel?> GetByIdAsync(Guid id);
        public void Add(TModel obj);
        public void Update(TModel obj);
        public void Remove(TModel obj);
        public Task SaveChangeAsync();
    }
}
