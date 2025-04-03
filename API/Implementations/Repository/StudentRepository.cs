using API.Abstractions;
using API.Implementations.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Implementations.Repository;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    private readonly ApplicationDbContext _context;
    
    public StudentRepository(ApplicationDbContext context)  : base(context)
    {
        _context = context;
    }
    
    public async Task<bool> CheckEmail(string email)
    {
        return await _context.Set<Student>()
            .AnyAsync(s => s.Email == email);
    }
}