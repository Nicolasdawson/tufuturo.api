using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class CareerCampusStats : GenericEntity
{
    [Required]
    [Column("anualTuition")]
    public int AnualTuition { get; set; }
    
    [Required]
    [Column("graduationFee")]
    public int GraduationFee { get; set; }
    
    [Required]
    [Column("duration")]
    public int Duration { get; set; }
    
    [Required]
    [Column("maleEnrollment")]
    public int MaleEnrollment { get; set; }
    
    [Required]
    [Column("femaleEnrollment")]
    public int FemaleEnrollment { get; set; }
    
    [Required]
    [Column("totalEnrollment")]
    public int TotalEnrollment { get; set; }
    
    [Required]
    [Column("publicSchoolRate")]
    public decimal PublicSchoolRate { get; set; }
    
    [Required]
    [Column("subsidizedSchoolRate")]
    public decimal SubsidizedSchoolRate { get; set; }
    
    [Required]
    [Column("privateSchoolRate")]
    public decimal PrivateSchoolRate { get; set; }
    
    [Required]
    [Column("femaleDegrees")]
    public int FemaleDegrees { get; set; }
    
    [Required]
    [Column("maleDegrees")]
    public int MaleDegrees { get; set; }
    
    [Required]
    [Column("totalDegrees")]
    public int TotalDegrees { get; set; }
    
    [Required]
    [Column("firstYearEntryFrom")]
    public decimal FirstYearEntryFrom { get; set; }
    
    [Required]
    [Column("firstYearEntryTo")]
    public decimal FirstYearEntryTo { get; set; }
    
    [Required]
    [Column("avargePaes")]
    public decimal AvargePaes { get; set; }
    
    [Required]
    [Column("avarageEnrollment")]
    public decimal AvarageEnrollment { get; set; }
    
    [Required]
    [Column("vacanciesFirstSemester")]
    public int VacanciesFirstSemester { get; set; }
    
    [Required]
    [Column("nem")]
    public int Nem { get; set; }
    
    [Required]
    [Column("ranking")]
    public int Ranking { get; set; }
    
    [Required]
    [Column("paesLanguaje")]
    public int PaesLanguaje { get; set; }
    
    [Required]
    [Column("paesMaths")]
    public int PaesMaths { get; set; }
    
    [Column("paesMaths2")]
    public int? PaesMaths2 { get; set; }
    
    [Column("paesHistory")]
    public int? PaesHistory { get; set; }
    
    [Column("paesSciences")]
    public int? PaesSciences { get; set; }
    
    [Column("others")]
    public int? Others { get; set; }
    
    [Required]
    [Column("yearOfData")]
    public int YearOfData { get; set; }
    
    [Column("careerCampusId")]
    public int CareerCampusId { get; set; }
    
    [ForeignKey("careerCampusId")]
    public CareerCampus CareerCampus { get; set; }
}