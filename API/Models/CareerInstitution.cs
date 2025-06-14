namespace API.Models;

public class CareerInstitution
{
    public decimal StudyContinuity { get; set; }
    
    public decimal StudyContinuityDiff { get; set; }

    public decimal RetentionFirstYear { get; set; }
    
    public decimal RetentionFirstYearDiff { get; set; }

    public decimal RealDuration { get; set; }
    
    public decimal RealDurationDiff { get; set; }

    public decimal EmployabilityFirstYear { get; set; }
    
    public decimal EmployabilityFirstYearDiff { get; set; }

    public decimal EmployabilitySecondYear { get; set; }
    
    public decimal EmployabilitySecondYearDiff { get; set; }

    public decimal AvarageSalary { get; set; }
    
    public decimal AvarageSalaryDiff { get; set; }
    
    public required string Name { get; set; }
}