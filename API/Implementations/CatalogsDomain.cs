using API.Abstractions;
using API.Implementations.Repository.Entities;
using API.Models;
using API.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace API.Implementations;

public class CatalogsDomain : ICatalogsDomain
{
    private readonly IRepository<Region> _regionRepository;
    private readonly IRepository<AcreditationType> _acreditationTypeRepository;
    private readonly IRepository<InstitutionType> _institutionTypeRepository;
    private readonly IRepository<KnowledgeArea> _knowledgeAreaRepository;
    private readonly IRepository<Schedule> _scheduleRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;

    public CatalogsDomain(IRepository<Region> regionRepository,
        IRepository<AcreditationType> acreditationTypeRepository,
        IRepository<InstitutionType> institutionTypeRepository, 
        IRepository<KnowledgeArea> knowledgeAreaRepository,
        IRepository<Schedule> scheduleRepository, IMemoryCache memoryCache)
    {
        _regionRepository = regionRepository;
        _acreditationTypeRepository = acreditationTypeRepository;
        _institutionTypeRepository = institutionTypeRepository;
        _knowledgeAreaRepository = knowledgeAreaRepository;
        _scheduleRepository = scheduleRepository;
        
        _memoryCache = memoryCache;
        _memoryCacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(1))
            .SetPriority(CacheItemPriority.NeverRemove);
    }

    public async Task<List<Catalog>> GetRegions()
    {
        const string cacheKey = "CatalogRegions";
        
        if (_memoryCache.TryGetValue(cacheKey, out List<Catalog>? catalogs) && catalogs.IsAny())
        {
            return catalogs!;
        }
        
        var catalogEntities = await _regionRepository.GetAllAsync();

        catalogs = catalogEntities.Select(x => x.ToModel()).ToList();
        
        _memoryCache.Set(cacheKey, catalogs, _memoryCacheEntryOptions);
        
        return catalogs;
    }
    
    public async Task<List<Catalog>> GetAcreditationTypes()
    {
        const string cacheKey = "CatalogAcreditationTypes";
        
        if (_memoryCache.TryGetValue(cacheKey, out List<Catalog>? catalogs) && catalogs.IsAny())
        {
            return catalogs!;
        }
        
        var catalogsEntities = await _acreditationTypeRepository.GetAllAsync();

        catalogs = catalogsEntities.Select(x => x.ToModel()).ToList();
        
        _memoryCache.Set(cacheKey, catalogs, _memoryCacheEntryOptions);
        
        return catalogs;
    }
    
    public async Task<List<Catalog>> GetInstitutionTypes()
    {
        const string cacheKey = "CatalogInstitutionTypes";
        
        if (_memoryCache.TryGetValue(cacheKey, out List<Catalog>? catalogs) && catalogs.IsAny())
        {
            return catalogs!;
        }
        
        var catalogsEntities = await _institutionTypeRepository.GetAllAsync();

        catalogs = catalogsEntities.Select(x => x.ToModel()).ToList();
        
        _memoryCache.Set(cacheKey, catalogs, _memoryCacheEntryOptions);
        
        return catalogs;
    }
    
    public async Task<List<Catalog>> GetKnowledgeAreas()
    {
        const string cacheKey = "CatalogKnowledgeAreas";
        
        if (_memoryCache.TryGetValue(cacheKey, out List<Catalog>? catalogs) && catalogs.IsAny())
        {
            return catalogs!;
        }
        
        var catalogsEntities = await _knowledgeAreaRepository.GetAllAsync();

        catalogs = catalogsEntities.Select(x => x.ToModel()).ToList();
        
        _memoryCache.Set(cacheKey, catalogs, _memoryCacheEntryOptions);
        
        return catalogs;
    }
    
    public async Task<List<Catalog>> GetSchedules()
    {
        const string cacheKey = "CatalogSchedules";
        
        if (_memoryCache.TryGetValue(cacheKey, out List<Catalog>? catalogs) && catalogs.IsAny())
        {
            return catalogs!;
        }
        
        var catalogsEntities = await _scheduleRepository.GetAllAsync();

        catalogs = catalogsEntities.Select(x => x.ToModel()).ToList();
        
        _memoryCache.Set(cacheKey, catalogs, _memoryCacheEntryOptions);
        
        return catalogs;
    }
}