namespace API.Models;

public class CareerCampus
{
    public required string CareerName { get; set; }

    public required string Schedule { get; set; }

    public required string Campus { get; set; }

    public int AnualTuition { get; set; }
    
    public int AnualTuitionDiff { get; set; }

    public int GraduationFee { get; set; }
    
    public int GraduationFeeDiff { get; set; }

    public int Duration { get; set; }

    public int MaleEnrollment { get; set; }
    
    public int MaleEnrollmentDiff { get; set; }

    public int FemaleEnrollment { get; set; }
    
    public int FemaleEnrollmentDiff { get; set; }

    public int TotalEnrollment { get; set; }
    
    public int TotalEnrollmentDiff { get; set; }

    public decimal PublicSchoolRate { get; set; }

    public decimal SubsidizedSchoolRate { get; set; }

    public decimal PrivateSchoolRate { get; set; }

    public int FemaleDegrees { get; set; }
    
    public int FemaleDegreesDiff { get; set; }

    public int MaleDegrees { get; set; }
    
    public int MaleDegreesDiff { get; set; }

    public int TotalDegrees { get; set; }
    
    public int TotalDegreesDiff { get; set; }

    public decimal FirstYearEntryRange { get; set; }

    public decimal AvargePaes { get; set; }
    
    public decimal AvargePaesDiff { get; set; }

    public decimal AvarageEnrollment { get; set; }
    
    public decimal AvarageEnrollmentDiff { get; set; }

    public int VacanciesFirstSemester { get; set; }
    
    public int VacanciesFirstSemesterDiff { get; set; }

    public int Nem { get; set; }

    public int Ranking { get; set; }

    public int PaesLanguaje { get; set; }

    public int PaesMaths { get; set; }

    public int? PaesMaths2 { get; set; }

    public int? PaesHistory { get; set; }

    public int? PaesSciences { get; set; }

    public int? Others { get; set; }
}