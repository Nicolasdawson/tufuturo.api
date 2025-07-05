using API.Abstractions;
using API.Implementations.Repository.Entities;

namespace API.Implementations.Repository;

public class KnowledgeAreaRepository : Repository<KnowledgeArea>, IKnowledgeAreaRepository
{
    private readonly ApplicationDbContext _context;
    
    public KnowledgeAreaRepository(ApplicationDbContext context)  : base(context)
    {
        _context = context;
    }
}