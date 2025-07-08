using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class CareerCampusStats : GenericEntity
{
    [Required]
    [Column("anualtuition")]
    public int AnualTuition { get; set; }
    
    [Required]
    [Column("graduationfee")]
    public int GraduationFee { get; set; }
    
    [Required]
    [Column("duration")]
    public int Duration { get; set; }
    
    [Required]
    [Column("maleenrollment")]
    public int MaleEnrollment { get; set; }
    
    [Required]
    [Column("femaleenrollment")]
    public int FemaleEnrollment { get; set; }
    
    [Required]
    [Column("totalenrollment")]
    public int TotalEnrollment { get; set; }
    
    [Required]
    [Column("publicschoolrate")]
    public decimal PublicSchoolRate { get; set; }
    
    [Required]
    [Column("subsidizedschoolrate")]
    public decimal SubsidizedSchoolRate { get; set; }
    
    [Required]
    [Column("privateschoolrate")]
    public decimal PrivateSchoolRate { get; set; }
    
    [Required]
    [Column("femaledegrees")]
    public int FemaleDegrees { get; set; }
    
    [Required]
    [Column("maledegrees")]
    public int MaleDegrees { get; set; }
    
    [Required]
    [Column("totaldegrees")]
    public int TotalDegrees { get; set; }
    
    [Required]
    [Column("firstyearentryfrom")]
    public decimal FirstYearEntryFrom { get; set; }
    
    [Required]
    [Column("firstyearentryto")]
    public decimal FirstYearEntryTo { get; set; }
    
    [Required]
    [Column("avargepaes")]
    public decimal AvargePaes { get; set; }
    
    [Required]
    [Column("avarageenrollment")]
    public decimal AvarageEnrollment { get; set; }
    
    [Required]
    [Column("vacanciesfirstsemester")]
    public int VacanciesFirstSemester { get; set; }
    
    [Required]
    [Column("nem")]
    public int Nem { get; set; }
    
    [Required]
    [Column("ranking")]
    public int Ranking { get; set; }
    
    [Required]
    [Column("paeslanguaje")]
    public int PaesLanguaje { get; set; }
    
    [Required]
    [Column("paesmaths")]
    public int PaesMaths { get; set; }
    
    [Column("paesmaths2")]
    public int? PaesMaths2 { get; set; }
    
    [Column("paeshistory")]
    public int? PaesHistory { get; set; }
    
    [Column("paessciences")]
    public int? PaesSciences { get; set; }
    
    [Column("others")]
    public int? Others { get; set; }
    
    [Required]
    [Column("yearofdata")]
    public int YearOfData { get; set; }
    
    [Column("careercampusid")]
    public int CareerCampusId { get; set; }
    
    [ForeignKey("careerCampusId")]
    public CareerCampus CareerCampus { get; set; }
}