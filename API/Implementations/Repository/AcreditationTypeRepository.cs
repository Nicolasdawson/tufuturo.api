using API.Abstractions;
using API.Implementations.Repository.Entities;

namespace API.Implementations.Repository;

public class AcreditationTypeRepository : Repository<AcreditationType>, IAcreditationTypeRepository
{
    private readonly ApplicationDbContext _context;
    
    public AcreditationTypeRepository(ApplicationDbContext context)  : base(context)
    {
        _context = context;
    }
}