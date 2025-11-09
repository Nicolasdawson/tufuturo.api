using API.Abstractions;
using API.Utils;
using Microsoft.Extensions.Logging;
using Models = API.Models;
using Entities = API.Implementations.Repository.Entities;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace API.Implementations;

public class CareerDomain : ICareerDomain
{
    private readonly IRepository<Entities.Career> _careerRepository;
    private readonly ICareerInstitutionRepository _careerInstitutionRepository;
    private readonly ICareerCampusRepository _careerCampusRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;

    public CareerDomain(IRepository<Entities.Career> careerRepository,
        ICareerInstitutionRepository careerInstitutionRepository,
        ICareerCampusRepository careerCampusRepository,
        ILogger<CareerDomain> logger,
        IMemoryCache memoryCache)
    {
        _careerRepository = careerRepository;
        _careerInstitutionRepository = careerInstitutionRepository;
        _careerCampusRepository = careerCampusRepository;
        
        _memoryCache = memoryCache;
        _memoryCacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(1))
            .SetPriority(CacheItemPriority.NeverRemove);
    }

    public async Task<PagedResult<List<Models.Career>>> GetParentCareers(Models.CareerParams queryParams)
    {
        var cacheKey = $"ParentCareers_{queryParams.InstitutionTypeId}_{queryParams.AcreditationTypeId}_" +
                       $"{queryParams.InstitutionId}_{queryParams.KnowledgeAreaId}_" +
                       $"{queryParams.ScheduleId}_{queryParams.RegionId}_" +
                       $"{queryParams.Skip}_{queryParams.Take}";
        
        if (_memoryCache.TryGetValue(cacheKey, out PagedResult<List<Models.Career>>? careersCache))
        {
            return careersCache!;
        }
        
        var careers = _careerRepository.Get(x => !x.IsDeleted);
        
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
            .Include(x => x.KnowledgeArea)
            .OrderByDescending(x => x.Id)
            .Skip(queryParams.Skip)
            .Take(queryParams.Take)
            .Select(x => x.ToModel())
            .ToList();

        var pageNumber = queryParams.Skip > 0 ? queryParams.Take / queryParams.Skip : 1;
        
        var totalPages = count / queryParams.Take;

        PagedInfo pagedInfo = new PagedInfo(pageNumber, queryParams.Take, totalPages, count);
        
        var result =  Result.Success(careersResult).ToPagedResult(pagedInfo);
        
        _memoryCache.Set(cacheKey, result, _memoryCacheEntryOptions);

        return result;
    }

    public async Task<Result<List<Models.CareerInstitution>>> GetCareersInstitution(int institutionId)
    {
        var careers = _careerInstitutionRepository.GetByInstitution(institutionId);

        if (!careers.IsAny())
        {
            return Result.NotFound();
        }

        var models = careers.Select(x => x.ToModel()).ToList();
        return Result.Success(models);
    }

    public async Task<Result<List<Models.CareerCampus>>> GetCareersCampus(int institutionId, int campusId)
    {
        var careers = await _careerCampusRepository.GetCareerCampus(institutionId, campusId);

        if (!careers.IsAny())
        {
            return Result.NotFound();
        }
        
        var models = careers.Select(x => x.ToModel()).ToList();
        return Result.Success(models);
    }
    
    public Result<List<Models.CareerInstitution>> GetCareerByGenericCareer(int careerId)
    {
        var careers = _careerInstitutionRepository.GetByCareer(careerId);

        if (!careers.IsAny())
        {
            return Result.NotFound();
        }
        
        var models = careers.Select(x => x.ToModel()).ToList();
        return Result.Success(models);
    }
}