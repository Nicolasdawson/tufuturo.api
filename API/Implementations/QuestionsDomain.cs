using API.Abstractions;
using API.Models;
using API.Utils;
using Microsoft.Extensions.Caching.Memory;
using Entities = API.Implementations.Repository.Entities;

namespace API.Implementations;

public class QuestionsDomain : IQuestionsDomain
{
    private readonly IRepository<Entities.Question> _questionRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly string _cacheKey = "QuestionsEs";
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public QuestionsDomain(IRepository<Entities.Question> questionRepository, IMemoryCache memoryCache)
    {
        _questionRepository = questionRepository;
        _memoryCache = memoryCache;
        
        _cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(1))
            .SetPriority(CacheItemPriority.NeverRemove);
    }

    public async Task<List<Question>> GetAllQuestions()
    {
        if (_memoryCache.TryGetValue(_cacheKey, out List<Question>? questions) && questions.IsAny())
        {
            return questions!;
        }
        
        var questionsEntities = await _questionRepository.GetAllAsync();

        questions = questionsEntities.Select(x => x.ToModel()).ToList();
        
        _memoryCache.Set(_cacheKey, questions, _cacheOptions);
        
        return questions;
    }
}