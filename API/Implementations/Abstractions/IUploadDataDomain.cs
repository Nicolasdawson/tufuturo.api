using Ardalis.Result;

namespace API.Implementations;

public interface IUploadDataDomain
{
    Task<Result> UploadInstitutions(FileStream file);

    Task<Result> UploadGenericsCareers(FileStream file);

    Task<Result> UploadCareersInstitution(FileStream file);

    Task<Result> UploadInstitutionCampus(FileStream file);

    Task<Result> UploadCareersCampus(FileStream file);
}