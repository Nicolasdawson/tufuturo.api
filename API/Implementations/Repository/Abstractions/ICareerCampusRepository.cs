using Entities = API.Implementations.Repository.Entities;

namespace API.Abstractions;

public interface ICareerCampusRepository : IRepository<Entities.CareerCampus>
{
    Task<List<Entities.CareerCampus>> GetCareerCampus(int institutionId, int campusId);
}