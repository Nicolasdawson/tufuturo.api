using API.Implementations.Repository.Entities;

namespace API.Abstractions;

public interface IInstitutionRepository
{
    Task<Institution?> InstitutionDetail(int id);
}