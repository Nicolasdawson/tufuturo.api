using Models = API.Models;
using Entities = API.Implementations.Repository.Entities;

namespace API.Utils;

public static class Extensions
{
    public static Models.Question ToModel(this Entities.Question question)
    {
        return new Models.Question
        {
            Id = question.Id,
            Text = question.Text,
            Category = Enum.Parse<Models.RiasecCategory>(question.Category)
        };
    }

    public static Models.Catalog ToModel(this Entities.GenericCatalog catalog)
    {
        return new Models.Catalog
        {
            Id = catalog.Id,
            Name = catalog.Name
        };
    }

    public static Models.Career ToModel(this Entities.Career career)
    {
        return new Models.Career
        {
            Name = career.Name,
            KnowledgeArea = career.KnowledgeArea.Name,
            Id = career.Id
        };
    }

    public static Models.CareerInstitution ToModel(this Entities.CareerInstitution entity)
    {
        var lastStats = entity.CareerInstitutionStats.Last();
        var firstStats = entity.CareerInstitutionStats.First();
        
        return new Models.CareerInstitution
        {
            Name = entity.Name,
            StudyContinuity = lastStats.StudyContinuity,
            StudyContinuityDiff = lastStats.StudyContinuity - firstStats.StudyContinuity,
            RetentionFirstYear = lastStats.RetentionFirstYear,
            RetentionFirstYearDiff = lastStats.RetentionFirstYear - firstStats.RetentionFirstYear,
            RealDuration = lastStats.RealDuration,
            RealDurationDiff = lastStats.RealDuration - firstStats.RealDuration,
            EmployabilityFirstYear = lastStats.EmployabilityFirstYear,
            EmployabilityFirstYearDiff = lastStats.EmployabilityFirstYear - firstStats.EmployabilityFirstYear,
            EmployabilitySecondYear = lastStats.EmployabilitySecondYear,
            EmployabilitySecondYearDiff = lastStats.EmployabilitySecondYear - firstStats.EmployabilitySecondYear,
            InstitutionName = entity.Institution.Name,
            CareerId = entity.CarrerId,
            InstitutionId = entity.InstitutionId,
            Id = entity.Id
        };
    }

    public static Models.CareerCampus ToModel(this Entities.CareerCampus entity)
    {
        var lastStats = entity.CareerCampusStats.Last();
        var firstStats = entity.CareerCampusStats.First();

        return new Models.CareerCampus
        {
            CareerName = entity.Name,
            Schedule = entity.Schedule.Name,
            Campus = entity.InstitutionCampus.Name,
            AnualTuition = lastStats.AnualTuition,
            AnualTuitionDiff = lastStats.AnualTuition - firstStats.AnualTuition,
            GraduationFee = lastStats.GraduationFee,
            GraduationFeeDiff = lastStats.GraduationFee - firstStats.GraduationFee,
            Duration = lastStats.Duration,
            MaleEnrollment = lastStats.MaleEnrollment,
            MaleEnrollmentDiff = lastStats.MaleEnrollment - firstStats.MaleEnrollment,
            FemaleEnrollment = lastStats.FemaleEnrollment,
            FemaleEnrollmentDiff = lastStats.FemaleEnrollment - firstStats.FemaleEnrollment,
            TotalEnrollment = lastStats.TotalEnrollment,
            TotalEnrollmentDiff = lastStats.TotalEnrollment - firstStats.TotalEnrollment,
            PublicSchoolRate = lastStats.PublicSchoolRate,
            SubsidizedSchoolRate = lastStats.SubsidizedSchoolRate,
            PrivateSchoolRate = lastStats.PrivateSchoolRate,
            FemaleDegrees = lastStats.FemaleDegrees,
            FemaleDegreesDiff = lastStats.FemaleDegrees - firstStats.FemaleDegrees,
            MaleDegrees = lastStats.MaleDegrees,
            MaleDegreesDiff = lastStats.MaleDegrees - firstStats.MaleDegrees,
            TotalDegrees = lastStats.TotalDegrees,
            TotalDegreesDiff = lastStats.TotalDegrees - firstStats.TotalDegrees,
            // FirstYearEntryRange = lastStats.FirstYearEntryRange,
            AvargePaes = lastStats.AvargePaes,
            AvargePaesDiff = lastStats.AvargePaes - firstStats.AvargePaes,
            AvarageEnrollment = lastStats.AvarageEnrollment,
            AvarageEnrollmentDiff = lastStats.AvarageEnrollment - firstStats.AvarageEnrollment,
            VacanciesFirstSemester = lastStats.VacanciesFirstSemester,
            VacanciesFirstSemesterDiff = lastStats.VacanciesFirstSemester - firstStats.VacanciesFirstSemester,
            Nem = lastStats.Nem,
            Ranking = lastStats.Ranking,
            PaesLanguaje = lastStats.PaesLanguaje,
            PaesMaths = lastStats.PaesMaths,
            PaesMaths2 = lastStats.PaesMaths2,
            PaesHistory = lastStats.PaesHistory,
            PaesSciences = lastStats.PaesSciences,
            Others = lastStats.Others
        };
    }
    
    public static bool IsAny<T>(this List<T>? list)
    {
        return list != null && list.Count != 0;
    }
}