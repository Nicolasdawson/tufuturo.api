using API.Abstractions;
using API.Implementations.Repository.Entities;

namespace API.Implementations.Repository;

public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
{
    private readonly ApplicationDbContext _context;
    
    public ScheduleRepository(ApplicationDbContext context)  : base(context)
    {
        _context = context;
    }
}