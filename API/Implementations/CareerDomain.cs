using API.Abstractions;
using API.Utils;
using Models = API.Models;
using Entities = API.Implementations.Repository.Entities;
using Ardalis.Result;

namespace API.Implementations;

public class CareerDomain : ICareerDomain
{
    private readonly IRepository<Entities.Career> _careerRepository;
    private readonly ICareerInstitutionRepository _careerInstitutionRepository;
    private readonly ICareerCampusRepository _careerCampusRepository;

    public CareerDomain(IRepository<Entities.Career> careerRepository,
        ICareerInstitutionRepository careerInstitutionRepository,
        ICareerCampusRepository careerCampusRepository)
    {
        _careerRepository = careerRepository;
        _careerInstitutionRepository = careerInstitutionRepository;
        _careerCampusRepository = careerCampusRepository;
    }

    public async Task<PagedResult<List<Models.Career>>> GetParentCareers(Models.CareerParams queryParams)
    {
        var careers = _careerRepository.Get(x => !x.IsDeleted, "KnowledgeArea"
            // "Institution,KnowledgeArea,CareerInstitutions, CareerInstitutions.CareerCampuses,CareerInstitutions.CareerCampuses.InstitutionCampus" // necesito esto? 
            );
        
        var count = careers.Count();

        if (queryParams.InstitutionTypeId != null)
        {
            careers = careers.Where(x =>
                x.CareerInstitutions.Any(ci => ci.Institution.InstitutionTypeId == queryParams.InstitutionTypeId));
        }

        if (queryParams.AcreditationTypeId != null)
        {
            careers = careers.Where(x => x.CareerInstitutions.Any(ci => ci.InstitutionId == queryParams.InstitutionId));
        }

        if (queryParams.InstitutionId != null)
        {
            careers = careers.Where(x => x.CareerInstitutions.Any(ci => ci.InstitutionId == queryParams.InstitutionId));
        }

        if (queryParams.KnowledgeAreaId != null)
        {
            careers = careers.Where(x => x.KnowledgeAreaId == queryParams.KnowledgeAreaId);
        }

        if (queryParams.ScheduleId != null)
        {
            careers = careers.Where(x => x.CareerInstitutions.Any(ci =>
                ci.CareerCampuses.Any(cc => cc.ScheduleId == queryParams.ScheduleId)));
        }

        if (queryParams.RegionId != null)
        {
            careers = careers.Where(x => x.CareerInstitutions.Any(ci =>
                ci.CareerCampuses.Any(cc => cc.InstitutionCampus.RegionId == queryParams.RegionId)));
        }

        var careersResult = careers
            .OrderByDescending(x => x.Id)
            .Skip(queryParams.Skip)
            .Take(queryParams.Take)
            .Select(x => x.ToModel())
            .ToList();

        var pageNumber = queryParams.Skip > 0 ? queryParams.Take / queryParams.Skip : 1;
        
        var totalPages = count / queryParams.Take;

        PagedInfo pagedInfo = new PagedInfo(pageNumber, queryParams.Take, totalPages, count);
        
        return Result.Success(careersResult).ToPagedResult(pagedInfo);
    }

    public async Task<Result<List<Models.CareerInstitution>>> GetCareersInstitution(int institutionId)
    {
        var careers = _careerInstitutionRepository.GetByInstitution(institutionId);

        if (!careers.IsAny())
            return Result.NotFound();

        var models = careers.Select(x => x.ToModel()).ToList();

        return Result.Success(models);
    }

    public async Task<Result<List<Models.CareerCampus>>> GetCareersCampus(int institutionId, int campusId)
    {
        var careers = await _careerCampusRepository.GetCareerCampus(institutionId, campusId);

        if (!careers.IsAny())
            return Result.NotFound();
        
        var models = careers.Select(x => x.ToModel()).ToList();

        return Result.Success(models);
    }
    
    public Result<List<Models.CareerInstitution>> GetCareerByGenericCareer(int careerId)
    {
        var careers = _careerInstitutionRepository.GetByCareer(careerId);

        if (!careers.IsAny())
            return Result.NotFound();
        
        var models = careers.Select(x => x.ToModel()).ToList();

        return Result.Success(models);
    }
}