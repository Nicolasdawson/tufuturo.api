using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class InstitutionDetails : GenericEntity
{
    [Column("yearOfData")]
    public int YearOfData { get; set; }
    
    [Column("acreditation")]
    public int? Acreditation { get; set; }
    
    [Column("acreditationExpireAt")]
    public DateTime? AcreditationExpireAt { get; set; }
    
    [Column("builded")]
    public decimal Builded { get; set; }
    
    [Column("buildedLibrary")]
    public decimal BuildedLibrary { get; set; }
    
    [Column("buildedLabs")]
    public decimal BuildedLabs { get; set; }
    
    [Column("labs")]
    public int Labs { get; set; }
    
    [Column("computersPerStudent")]
    public decimal ComputersPerStudent { get; set; }
    
    [Column("greenArea")]
    public decimal GreenArea { get; set; }
    
    [Column("institutionId")]
    public int InstitutionId { get; set; }
    
    [ForeignKey("institutionId")]
    public Institution Institution { get; set; }
    
    [Column("acreditationTypeId")]
    public int AcreditationTypeId { get; set; }
    
    [ForeignKey("acreditationTypeId")]
    public AcreditationType AcreditationType { get; set; }
}