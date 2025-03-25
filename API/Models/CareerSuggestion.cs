namespace API.Models;

public class CareerSuggestion
{
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public List<RiasecCategory> RelatedCategories { get; set; } = new();
    
    public string EducationRequirements { get; set; } = string.Empty;
    
    public decimal MedianSalary { get; set; }
}