using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repos
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _entitySet;
        private readonly TaskManagerDbContext _context;

        public IQueryable<TEntity> Query => _entitySet;

        public EntityRepository(TaskManagerDbContext dbContext)
        {
            _entitySet = dbContext.Set<TEntity>();
            _context = dbContext;

        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _entitySet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        
        public async Task RemoveAsync(TEntity entity)
        {
            _entitySet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
