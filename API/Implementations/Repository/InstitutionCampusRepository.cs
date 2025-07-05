using API.Abstractions;
using API.Implementations.Repository.Entities;

namespace API.Implementations.Repository;

public class InstitutionCampusRepository : Repository<InstitutionCampus>, IInstitutionCampusRepository
{
    private readonly ApplicationDbContext _context;
    
    public InstitutionCampusRepository(ApplicationDbContext context)  : base(context)
    {
        _context = context;
    }
}