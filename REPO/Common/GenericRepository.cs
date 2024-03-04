using DAC;
using EDM.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace REPO.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase
    {
        protected readonly AppDbContext dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual void Add(T entity) => 
            dbContext.Set<T>().Add(entity);

        public virtual void AddRange(IEnumerable<T> entities) => 
            dbContext.Set<T>().AddRange(entities);

        public virtual void Delete(T entity)
        {
            entity.Deleted = true;
            Update(entity);
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                entity.Deleted = true;
                Update(entity);
            }
        }

        public virtual void Remove(T entity) => 
            dbContext.Set<T>().Remove(entity);

        public virtual void RemoveRange(IEnumerable<T> entities) => 
            dbContext.Set<T>().RemoveRange(entities);

        public virtual void Update(T entity) => 
            dbContext.Set<T>().Update(entity);

        public virtual IEnumerable<T> FindAsync(Expression<Func<T, bool>> expression) =>
            dbContext.Set<T>().Where(expression);

        public virtual async Task<IEnumerable<T>> GetAllAsync() =>
            await dbContext.Set<T>()
                .Where(x => !x.Deleted)
                .ToListAsync();

        public virtual async Task<T> GetByIdAsync(int id) =>
            await dbContext.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id
                    && !x.Deleted);

        public async Task SaveChangesAsync() =>
            await dbContext.SaveChangesAsync();


    }
}
