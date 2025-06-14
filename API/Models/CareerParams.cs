namespace API.Models;

public class CareerParams : GenericParams
{
    public int? InstitutionTypeId { get; set; }
    
    public int? AcreditationTypeId { get; set; }
    
    public int? InstitutionId { get; set; }
    
    public int? KnowledgeAreaId { get; set; }
    
    public int? ScheduleId { get; set; }
    
    public int? RegionId { get; set; }
}