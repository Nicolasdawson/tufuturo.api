using API.Abstractions;
using API.Models;
using API.Utils;
using Microsoft.Extensions.Caching.Memory;

namespace API.Implementations;

public class QuestionsDomain : IQuestionsDomain
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly string _cacheKey = "QuestionsEs";
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public QuestionsDomain(IQuestionRepository questionRepository, IMemoryCache memoryCache)
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
        
        var questionsEntities = await _questionRepository.GetAllQuestions();

        questions = questionsEntities.Select(x => x.ToModel()).ToList();
        
        _memoryCache.Set(_cacheKey, questions, _cacheOptions);
        
        return questions;
    }
}