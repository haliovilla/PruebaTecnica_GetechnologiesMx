using EDM.Entities;
using REPO.Common;

namespace REPO.Interfaces
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<List<Invoice>> GetByPersonIdAsync(int personId);
    }
}
