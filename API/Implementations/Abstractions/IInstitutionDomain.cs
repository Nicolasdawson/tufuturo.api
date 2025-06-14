using API.Implementations.Repository.Entities;
using Ardalis.Result;

namespace API.Abstractions;

public interface IInstitutionDomain
{
    Task<Result<Institution>> GetDetail(int institutionId);
}