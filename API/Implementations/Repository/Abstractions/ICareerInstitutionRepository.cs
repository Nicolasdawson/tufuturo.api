using API.Implementations.Repository.Entities;

namespace API.Abstractions;

public interface ICareerInstitutionRepository : IRepository<CareerInstitution>
{
    List<CareerInstitution> GetCareersInstitution(int institutionId);
}