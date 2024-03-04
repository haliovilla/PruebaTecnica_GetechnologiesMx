using EDM.Entities;
using REPO.Common;

namespace REPO.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        Task<Person> GetByIdentificationAsync(string identification);
    }
}
