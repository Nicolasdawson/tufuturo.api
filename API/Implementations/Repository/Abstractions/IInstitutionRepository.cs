using API.Implementations.Repository.Entities;

namespace API.Abstractions;

public interface IInstitutionRepository : IRepository<Institution>
{
    Task<Institution?> InstitutionDetail(int id);
}