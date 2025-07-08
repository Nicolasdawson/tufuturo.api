using API.Abstractions;
using API.Implementations.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Implementations.Repository;

public class CareerCampusRepository : Repository<Entities.CareerCampus>, ICareerCampusRepository
{
    private readonly ApplicationDbContext _context;

    public CareerCampusRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Entities.CareerCampus>> GetCareerCampus(int institutionId, int campusId)
    {
        return await _context.CareerCampuses.Where(x => x.InstitutionCampusId == campusId
                                                        && x.CareerInstitution.InstitutionId == institutionId)
            .Include(x => x.CareerCampusStats
                .OrderBy(d => d.YearOfData)
                .Take(2))
            .ToListAsync();
    }

    public new async Task AddRangeAsync(IEnumerable<CareerCampus> entities)
    {
        await _context.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }
}