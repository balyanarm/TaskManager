using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repos
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);

        IQueryable<TEntity> Query { get; }
        
        Task RemoveAsync(TEntity entity);
    }
}
