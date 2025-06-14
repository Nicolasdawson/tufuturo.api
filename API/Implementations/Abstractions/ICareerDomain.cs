using Ardalis.Result;

namespace API.Implementations;

public interface ICareerDomain
{
    Task<Result<List<Models.Career>>> GetCareers(Models.CareerParams queryParams);
    Task<Result<List<Models.CareerInstitution>>> GetCareersInstitution(int institutionId);
    Task<Result<List<Models.CareerCampus>>> GetCareersCampus(int institutionId, int campusId);
}