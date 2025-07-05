using API.Abstractions;
using API.Implementations.Repository.Entities;

namespace API.Implementations.Repository;

public class InstitutionTypeRepository : Repository<InstitutionType>, IInstitutionTypeRepository
{
    private readonly ApplicationDbContext _context;
    
    public InstitutionTypeRepository(ApplicationDbContext context)  : base(context)
    {
        _context = context;
    }
}