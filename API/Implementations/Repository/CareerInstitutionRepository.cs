using API.Abstractions;
using API.Implementations.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Implementations.Repository;

public class CareerInstitutionRepository : Repository<Entities.CareerInstitution>, ICareerInstitutionRepository
{
    private readonly ApplicationDbContext _context;
    
    public CareerInstitutionRepository(ApplicationDbContext context)  : base(context)
    {
        _context = context;
    }

    public List<CareerInstitution> GetCareersInstitution(int institutionId)
    {
        return _context.CareerInstitutions.Where(x => !x.IsDeleted 
                                                        && x.InstitutionId == institutionId)
            .Include(x => x.CareerInstitutionStats
                .OrderBy(d => d.YearOfData)
                .Take(2))
            .ToList();
    }
}