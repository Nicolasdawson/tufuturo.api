using API.Abstractions;
using API.Models;
using Ardalis.Result;
using Entities = API.Implementations.Repository.Entities;

namespace API.Implementations;

public class AssessmentDomain : IAssessmentDomain
{
    private readonly IRepository<Entities.Student> _studentRepository;
    private readonly IRepository<Entities.Question> _questionRepository;

    public AssessmentDomain(IRepository<Entities.Student> studentRepository,
        IRepository<Entities.Question> questionRepository, ILogger<AssessmentDomain> logger)
    {
        _studentRepository = studentRepository;
        _questionRepository = questionRepository;
    }

    public async Task<Result> CreateAssessment(AssessmentRequest request)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId);

        if (student is null)
        {
            return Result.NotFound("Student not found");
        }

        var questions = await _questionRepository.GetAllAsync();

        return Result.Success();
    }


}