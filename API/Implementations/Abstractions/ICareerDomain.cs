using Ardalis.Result;

namespace API.Implementations;

public interface ICareerDomain
{
    Task<PagedResult<List<Models.Career>>> GetParentCareers(Models.CareerParams queryParams);
    Task<Result<List<Models.CareerInstitution>>> GetCareersInstitution(int institutionId);
    Task<Result<List<Models.CareerCampus>>> GetCareersCampus(int institutionId, int campusId);
    Result<List<Models.CareerInstitution>> GetCareerByGenericCareer(int careerId);
}