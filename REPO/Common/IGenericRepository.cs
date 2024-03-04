using EDM.Common;
using System.Linq.Expressions;

namespace REPO.Common
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        IEnumerable<T> FindAsync(Expression<Func<T, bool>> expression);

        Task SaveChangesAsync();
    }
}
