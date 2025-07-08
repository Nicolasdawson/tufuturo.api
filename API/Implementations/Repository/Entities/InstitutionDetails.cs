using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class InstitutionDetails : GenericEntity
{
    [Column("yearofdata")]
    public int YearOfData { get; set; }
    
    [Column("acreditation")]
    public int? Acreditation { get; set; }
    
    [Column("acreditationexpireat")]
    public DateTime? AcreditationExpireAt { get; set; }
    
    [Column("builded")]
    public decimal Builded { get; set; }
    
    [Column("buildedlibrary")]
    public decimal BuildedLibrary { get; set; }
    
    [Column("buildedlabs")]
    public decimal BuildedLabs { get; set; }
    
    [Column("labs")]
    public int Labs { get; set; }
    
    [Column("computersperstudent")]
    public decimal ComputersPerStudent { get; set; }
    
    [Column("greenarea")]
    public decimal GreenArea { get; set; }
    
    [Column("institutionid")]
    public int InstitutionId { get; set; }
    
    [ForeignKey("InstitutionId")]
    public Institution Institution { get; set; }
    
    [Column("acreditationtypeid")]
    public int AcreditationTypeId { get; set; }
    
    [ForeignKey("AcreditationTypeId")]
    public AcreditationType AcreditationType { get; set; }
}