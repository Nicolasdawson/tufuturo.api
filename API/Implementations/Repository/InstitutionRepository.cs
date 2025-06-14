using API.Abstractions;
using API.Implementations.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Implementations.Repository;

public class InstitutionRepository : Repository<Institution>, IInstitutionRepository
{
    private readonly ApplicationDbContext _context;
    
    public InstitutionRepository(ApplicationDbContext context)  : base(context)
    {
        _context = context;
    }

    public async Task<Institution?> InstitutionDetail(int id)
    {
        return await _context.Institutions.Where(x => !x.IsDeleted
                                                      && x.Id == id)
            .Include(x => x.InstitutionType)
            .Include(x => x.InstitutionDetails
                .OrderBy(d => d.YearOfData)
                .Take(2))
            .FirstOrDefaultAsync();
    }
}