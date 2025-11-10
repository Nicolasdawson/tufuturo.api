
namespace API.Models;

public class RecommendedCareer
{
    public string CareerName { get; set; } = string.Empty;
    public string InstitutionName { get; set; } = string.Empty;
    public string CampusName { get; set; } = string.Empty;
    public double MatchScore { get; set; }
}
