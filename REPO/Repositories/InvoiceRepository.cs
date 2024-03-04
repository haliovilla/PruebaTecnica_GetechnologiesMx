using DAC;
using EDM.Entities;
using Microsoft.EntityFrameworkCore;
using REPO.Common;
using REPO.Interfaces;

namespace REPO.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Invoice>> GetByPersonIdAsync(int personId) =>
            await dbContext.Invoices
            .Where(x => x.PersonId == personId
                && !x.Deleted)
        .OrderByDescending(x=>x.Date)
        .ToListAsync();
    }
}
