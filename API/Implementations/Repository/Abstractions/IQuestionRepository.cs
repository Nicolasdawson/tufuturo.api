using API.Implementations.Repository.Entities;

namespace API.Abstractions;

public interface IQuestionRepository
{
    Task<List<Question>> GetAllQuestions();
}