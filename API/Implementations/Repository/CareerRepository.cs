using API.Abstractions;
using API.Implementations.Repository.Entities;

namespace API.Implementations.Repository;

public class CareerRepository : Repository<Career>, ICareerRepository
{
    private readonly ApplicationDbContext _context;
    
    public CareerRepository(ApplicationDbContext context)  : base(context)
    {
        _context = context;
    }
}