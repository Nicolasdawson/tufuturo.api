using API.Models;

namespace API.Abstractions;

public interface IQuestionsDomain
{
    Task<List<Question>> GetAllQuestions();
}