namespace API.Models;

public class Career
{
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string Categories { get; set; } = string.Empty;
    
    public string EducationRequirements { get; set; } = string.Empty;
    
    public decimal MedianSalary { get; set; }
}