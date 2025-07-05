using Ardalis.Result;

namespace API.Implementations;

public interface IUploadDataDomain
{
    Task<Result> UploadInstitutions(Stream file);

    Task<Result> UploadGenericsCareers(Stream file);

    Task<Result> UploadCareersInstitution(Stream file);

    Task<Result> UploadInstitutionCampus(Stream file);

    Task<Result> UploadCareersCampus(Stream file);
}