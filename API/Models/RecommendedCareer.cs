
namespace API.Models;

public class RecommendedCareer
{
    public string CareerName { get; set; } = string.Empty;
    public string InstitutionName { get; set; } = string.Empty;
    public string CampusName { get; set; } = string.Empty;
    public decimal EmployabilityFirstYear { get; set; }
    public int AverageSalaryFrom { get; set; }
    public double MatchScore { get; set; }
}
