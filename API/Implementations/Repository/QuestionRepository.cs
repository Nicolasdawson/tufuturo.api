using API.Abstractions;
using API.Implementations.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Implementations.Repository;

public class QuestionRepository : Repository<Question>, IQuestionRepository
{
    private readonly ApplicationDbContext _context;
    
    public QuestionRepository(ApplicationDbContext context)  : base(context)
    {
        _context = context;
    }

    public async Task<List<Question>> GetAllQuestions()
    {
        return await _context.Questions.Where(x => !x.IsDeleted).ToListAsync();
    }
}