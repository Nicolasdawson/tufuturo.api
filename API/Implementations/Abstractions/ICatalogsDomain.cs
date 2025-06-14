using API.Models;

namespace API.Abstractions;

public interface ICatalogsDomain
{
    Task<List<Catalog>> GetRegions();
    Task<List<Catalog>> GetAcreditationTypes();
    Task<List<Catalog>> GetInstitutionTypes();
    Task<List<Catalog>> GetKnowledgeAreas();
    Task<List<Catalog>> GetSchedules();
}