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

    public List<CareerInstitution> GetByInstitution(int institutionId)
    {
        return _context.CareerInstitutions.Where(x => !x.IsDeleted 
                                                        && x.InstitutionId == institutionId)
            .Include(x => x.Institution)
            .Include(x => x.CareerInstitutionStats
                .OrderBy(d => d.YearOfData)
                .Take(2))
            .ToList();
    }
    
    public List<CareerInstitution> GetByCareer(int careerId)
    {
        return _context.CareerInstitutions.Where(x => !x.IsDeleted 
                                                      && x.CarrerId == careerId)
            .Include(x => x.Institution)
            .Include(x => x.CareerInstitutionStats
                .OrderBy(d => d.YearOfData)
                .Take(2))
            .ToList();
    }
}