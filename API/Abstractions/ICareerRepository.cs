using API.Models;

namespace API.Abstractions;

public interface ICareerRepository
{
    Task<List<CareerSuggestion>> GetCareersByInterestsAsync(List<RiasecCategory> interests);
}