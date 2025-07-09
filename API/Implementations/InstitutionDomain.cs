using API.Abstractions;
using API.Implementations.Repository;
using API.Implementations.Repository.Entities;
using Ardalis.Result;

namespace API.Implementations;

public class InstitutionDomain : IInstitutionDomain
{
    private readonly IInstitutionRepository _institutionRepository;
    public InstitutionDomain(IInstitutionRepository institutionRepository)
    {
        _institutionRepository = institutionRepository;
    }
    
    public async Task<Result<Institution>> GetDetail(int institutionId)
    {
        var institution = await _institutionRepository.InstitutionDetail(institutionId);

        if (institution is null)
            return Result<Institution>.NotFound("Institution not found");
        
        return Result<Institution>.Success(institution);
    }
}