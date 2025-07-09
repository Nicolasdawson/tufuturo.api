using API.Implementations.Repository.Entities;

namespace API.Abstractions;

public interface ICareerInstitutionRepository : IRepository<CareerInstitution>
{
    List<CareerInstitution> GetByInstitution(int institutionId);

    List<CareerInstitution> GetByCareer(int careerId);
}