using DAC;
using EDM.Entities;
using Microsoft.EntityFrameworkCore;
using REPO.Common;
using REPO.Interfaces;

namespace REPO.Repositories
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Person> GetByIdentificationAsync(string identification) =>
            await dbContext.People
            .Include(x => x.Invoices)
                .FirstOrDefaultAsync(x => x.Identification == identification
                    && !x.Deleted);
    }
}
