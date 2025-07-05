using API.Abstractions;
using API.Implementations.Repository.Entities;

namespace API.Implementations.Repository;

public class RegionRepository : Repository<Region>, IRegionRepository
{
    private readonly ApplicationDbContext _context;
    
    public RegionRepository(ApplicationDbContext context)  : base(context)
    {
        _context = context;
    }
}