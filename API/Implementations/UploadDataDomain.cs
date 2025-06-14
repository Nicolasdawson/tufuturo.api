using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using API.Abstractions;
using API.Implementations.Repository.Entities;
using Ardalis.Result;
using ExcelDataReader;

namespace API.Implementations;

public class UploadDataDomain : IUploadDataDomain
{
    private readonly IRepository<Institution> _institutionRepository;
    private readonly IRepository<InstitutionType> _institutionTypeRepository;
    private readonly IRepository<AcreditationType> _acreditationTypeRepository;
    private readonly IRepository<Career> _careerRepository;
    private readonly IRepository<KnowledgeArea> _knowledgeAreaRepository;
    private readonly IRepository<CareerInstitution> _careerInstitutionRepository;
    private readonly IRepository<InstitutionCampus> _institutionCampusRepository;
    private readonly IRepository<CareerCampus> _careerCampusRepository;
    private readonly IRepository<Region> _regionRepository;
    private readonly IRepository<Schedule> _scheduleRepository;
    private const int YearOfData = 2025;

    public UploadDataDomain(IRepository<Institution> institutionRepository,
        IRepository<InstitutionType> institutionTypeRepository,
        IRepository<AcreditationType> acreditationTypeRepository,
        IRepository<Career> careerRepository,
        IRepository<KnowledgeArea> knowledgeAreaRepository,
        IRepository<CareerInstitution> careerInstitutionRepository,
        IRepository<InstitutionCampus> institutionCampusRepository,
        IRepository<CareerCampus> careerCampusRepository,
        IRepository<Region> regionRepository,
        IRepository<Schedule> scheduleRepository)
    {
        _institutionRepository = institutionRepository;
        _institutionTypeRepository = institutionTypeRepository;
        _acreditationTypeRepository = acreditationTypeRepository;
        _careerRepository = careerRepository;
        _knowledgeAreaRepository = knowledgeAreaRepository;
        _careerInstitutionRepository = careerInstitutionRepository;
        _institutionCampusRepository = institutionCampusRepository;
        _careerCampusRepository = careerCampusRepository;
        _regionRepository = regionRepository;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<Result> UploadInstitutions(FileStream file)
    {
        var institutionTypes = await _institutionTypeRepository.GetAllAsync();

        var institutions = ParseExcelToInstitutions(file, institutionTypes.ToList());

        await _institutionRepository.AddRangeAsync(institutions);

        return Result.Success();
    }

    public async Task<Result> UploadGenericsCareers(FileStream file)
    {
        var knowledgeAreas = await _knowledgeAreaRepository.GetAllAsync();

        var careers = ParseExcelToGenericCareer(file, knowledgeAreas.ToList());

        await _careerRepository.AddRangeAsync(careers);
        return Result.Success();
    }

    public async Task<Result> UploadCareersInstitution(FileStream file)
    {
        var careers = await _careerRepository.GetAllAsync();

        var institutions = await _institutionRepository.GetAllAsync();

        var careersInstitutions = ParseExcelToCareerInstitution(file, institutions.ToList(), careers.ToList());

        await _careerInstitutionRepository.AddRangeAsync(careersInstitutions);

        return Result.Success();
    }

    public async Task<Result> UploadInstitutionCampus(FileStream file)
    {
        // Buscardor_de_carreras

        var institutions = await _institutionRepository.GetAllAsync();

        var regions = await _regionRepository.GetAllAsync();

        var institutionsCampus = ParseExcelToInstitutionCampus(file, regions.ToList(), institutions.ToList());

        await _institutionCampusRepository.AddRangeAsync(institutionsCampus);

        return Result.Success();
    }

    public async Task<Result> UploadCareersCampus(FileStream file)
    {
        // Buscardor_de_carreras

        var careerInstitutions = await _careerInstitutionRepository.GetAllAsync();

        var campuses = await _institutionCampusRepository.GetAllAsync();

        var schedules = await _scheduleRepository.GetAllAsync();

        var careersCampus =
            ParseExcelToCareersCampus(file, careerInstitutions.ToList(), schedules.ToList(), campuses.ToList());

        await _careerCampusRepository.AddRangeAsync(careersCampus);

        return Result.Success();
    }

    private List<Institution> ParseExcelToInstitutions(FileStream file, List<InstitutionType> institutionTypes)
    {
        List<Institution> result = new List<Institution>();

        try
        {
            using var reader = ExcelReaderFactory.CreateReader(file);
            var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            DataTable table = dataSet.Tables[0];

            foreach (DataRow row in table.Rows)
            {
                var instituionName = row["Nombre institución"]?.ToString();
                var instituionCode = row["Código institución"]?.ToString();

                var institution = _institutionRepository.Get(x => !x.IsDeleted
                                                                  && x.Name == instituionName
                                                                  && x.Code == instituionCode)
                    .FirstOrDefault();

                if (institution == null)
                {
                    institution = new Institution()
                    {
                        Name = row["Nombre institución"]?.ToString(),
                        Code = row["Código institución"]?.ToString(),
                    };

                    var institutionTypeId = institutionTypes
                        .FirstOrDefault(x => x.Name == row["Tipo de institución"]?.ToString())?.Id;

                    if (institutionTypeId is null)
                        break;

                    institution.InstitutionTypeId = institutionTypeId.Value;
                }

                if (institution.InstitutionDetails.Any(x => x.YearOfData == YearOfData))
                    break;

                var instituionDetail = new InstitutionDetails();

                var acreditationType = _acreditationTypeRepository.Get(x => !x.IsDeleted
                                                                            && x.Name ==
                                                                            row["Acreditación (31 de octubre de 2024)"]
                                                                                .ToString())
                    .FirstOrDefault();

                if (acreditationType is null)
                    break;

                instituionDetail.AcreditationTypeId = acreditationType.Id;

                List<string> acreditationNames = new List<string>()
                {
                    "Acreditación Extendida",
                    "Acreditada"
                };

                if (acreditationNames.Contains(acreditationType.Name))
                {
                    instituionDetail.Acreditation =
                        int.Parse(row["Años acreditación (31 de octubre de 2024)"]?.ToString());

                    var expireAt = row["Vigencia acreditación (31 de octubre de 2024)"]?.ToString();
                    instituionDetail.AcreditationExpireAt = ExtractDate(expireAt);
                }

                instituionDetail.Builded = decimal.Parse(row["m² construidos"]?.ToString());

                instituionDetail.BuildedLibrary = decimal.Parse(row["m² construidos biblioteca"]?.ToString());

                instituionDetail.BuildedLabs = decimal.Parse(row["m² construidos laboratorios y talleres"]?.ToString());

                instituionDetail.Labs = int.Parse(row["N° de laboratorios y talleres"]?.ToString());

                instituionDetail.ComputersPerStudent = int.Parse(row["N° de computadores"]?.ToString());

                instituionDetail.GreenArea = int.Parse(row["m² áreas verdes y esparcimiento"]?.ToString());

                institution.InstitutionDetails.Add(instituionDetail);

                result.Add(institution);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return result;
    }

    private List<Career> ParseExcelToGenericCareer(FileStream file, List<KnowledgeArea> knowledgeAreas)
    {
        List<Career> result = new List<Career>();

        try
        {
            using var reader = ExcelReaderFactory.CreateReader(file);
            var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            DataTable table = dataSet.Tables[0];

            var distinctRows = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("Área Carrera Genérica"))
                .Select(group => group.First())
                .ToList();

            foreach (DataRow row in distinctRows)
            {
                var knowlageArea = row["Área del conocimiento"]?.ToString();
                var genericCareer = row["Área Carrera Genérica"]?.ToString();

                var career = _careerRepository.Get(x => !x.IsDeleted
                                                        && x.Name == genericCareer)
                    .FirstOrDefault();

                if (career == null)
                {
                    career = new Career()
                    {
                        Name = genericCareer
                    };

                    var knowlageAreaId = knowledgeAreas
                        .FirstOrDefault(x => x.Name == knowlageArea)?.Id;

                    if (knowlageAreaId is null)
                        break;

                    career.KnowledgeAreaId = knowlageAreaId.Value;
                }

                result.Add(career);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return result;
    }

    private List<CareerInstitution> ParseExcelToCareerInstitution(FileStream file, List<Institution> institutions,
        List<Career> careers)
    {
        List<CareerInstitution> result = new List<CareerInstitution>();

        try
        {
            using var reader = ExcelReaderFactory.CreateReader(file);
            var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            DataTable table = dataSet.Tables[0];

            foreach (DataRow row in table.Rows)
            {
                var instituionName = row["Nombre de institución"]?.ToString();
                var genericCareerName = row["Nombre carrera genérica"]?.ToString();
                var institutionCareerName = row["Nombre carrera (del título)"]?.ToString();

                var careerInstitution = _careerInstitutionRepository.Get(x => !x.IsDeleted
                                                                              && x.Name == instituionName)
                    .FirstOrDefault();

                if (careerInstitution == null)
                {
                    careerInstitution = new CareerInstitution()
                    {
                        Name = institutionCareerName,
                    };

                    var genericCareerId = careers
                        .FirstOrDefault(x => x.Name == genericCareerName)?.Id;

                    if (genericCareerId is null)
                        break;

                    careerInstitution.CarrerId = genericCareerId.Value;

                    var institutionId = institutions
                        .FirstOrDefault(x => x.Name == instituionName)?.Id;

                    if (institutionId is null)
                        break;

                    careerInstitution.InstitutionId = institutionId.Value;
                }

                if (careerInstitution.CareerInstitutionStats.Any(x => x.YearOfData == YearOfData))
                    break;

                var careerInstitutionStats = new CareerInstitutionStats();

                if (decimal.TryParse(row["% titulados con continuidad de estudios"]?.ToString(),
                        out var studyContinuity))
                    careerInstitutionStats.StudyContinuity = studyContinuity;

                if (decimal.TryParse(row["Retención 1er año"]?.ToString(), out var retentionFirstYear))
                    careerInstitutionStats.RetentionFirstYear = retentionFirstYear;

                if (decimal.TryParse(row["Duración Real (semestres)"]?.ToString(), out var realDuration))
                    careerInstitutionStats.RealDuration = realDuration;

                if (decimal.TryParse(row["Empleabilidad 1er año"]?.ToString(), out var employabilityFirstYear))
                    careerInstitutionStats.EmployabilityFirstYear = employabilityFirstYear;

                if (decimal.TryParse(row["Empleabilidad 2° año"]?.ToString(), out var employabilitySecondYear))
                    careerInstitutionStats.EmployabilitySecondYear = employabilitySecondYear;

                var avarageSalary = row["Ingreso Promedio al 4° año"]?.ToString();

                var regex = new Regex(@"\$?(?:(\d+)\s+millón\s*)?(\d+)\s+mil", RegexOptions.IgnoreCase);
                var matches = regex.Matches(avarageSalary);

                for (int i = 0; i < 1; i++)
                {
                    var avarageSalaryNumber = 0;

                    int millones = matches[i].Groups[1].Success ? int.Parse(matches[i].Groups[1].Value) : 0;
                    int miles = matches[i].Groups[2].Success ? int.Parse(matches[i].Groups[2].Value) : 0;

                    if (millones > 0)
                    {
                        avarageSalaryNumber = millones * 1_000_000;
                    }
                    else if (miles > 0)
                    {
                        avarageSalaryNumber = miles * 1_000;
                    }

                    if (i == 0)
                    {
                        careerInstitutionStats.AvarageSalaryFrom = avarageSalaryNumber;
                    }
                    else if (i == 1)
                    {
                        careerInstitutionStats.AvarageSalaryTo = avarageSalaryNumber;
                    }
                    else
                    {
                        break;
                    }
                }

                careerInstitutionStats.YearOfData = YearOfData;

                careerInstitution.CareerInstitutionStats.Add(careerInstitutionStats);

                result.Add(careerInstitution);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return result;
    }

    private List<InstitutionCampus> ParseExcelToInstitutionCampus(FileStream file, List<Region> regions,
        List<Institution> institutions)
    {
        List<InstitutionCampus> result = new List<InstitutionCampus>();

        try
        {
            using var reader = ExcelReaderFactory.CreateReader(file);
            var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            DataTable table = dataSet.Tables[0];

            var distinctRows = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("Sede"))
                .Select(group => group.First())
                .ToList();

            foreach (DataRow row in distinctRows)
            {
                var campusName = row["Sede"]?.ToString();

                var institutionName = row["Nombre institución"]?.ToString();

                var institutionId = institutions.FirstOrDefault(x => x.Name == institutionName)?.Id;

                if (institutionId is null)
                    break;

                var campus = _institutionCampusRepository.Get(x => !x.IsDeleted && x.Name == campusName)
                    .FirstOrDefault();

                if (campus is null)
                {
                    var regionId = regions.FirstOrDefault(x => x.Name == row["Región"]?.ToString() && !x.IsDeleted)?.Id;

                    if (regionId is null)
                        break;

                    campus = new InstitutionCampus()
                    {
                        Name = campusName,
                        RegionId = regionId.Value,
                        InstitutionId = institutionId.Value
                    };

                    result.Add(campus);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return result;
    }

    private List<CareerCampus> ParseExcelToCareersCampus(FileStream file, List<CareerInstitution> careerInstitutions,
        List<Schedule> schedules,
        List<InstitutionCampus> institutionCampus)
    {
        List<CareerCampus> result = new List<CareerCampus>();

        try
        {
            using var reader = ExcelReaderFactory.CreateReader(file);
            var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            DataTable table = dataSet.Tables[0];

            foreach (DataRow row in table.Rows)
            {
                var campusName = row["Sede"]?.ToString();

                var campusCareerName = row["Nombre carrera"]?.ToString();

                var scheduleName = row["Jornada"]?.ToString();

                var scheduleId = schedules.FirstOrDefault(x => x.Name == scheduleName)?.Id;

                if (scheduleId is null)
                    break;

                var campusInstitutionId = institutionCampus.FirstOrDefault(x => x.Name == campusName)?.Id;

                if (campusInstitutionId is null)
                    break;

                var campusCareer = _careerCampusRepository.Get(x => !x.IsDeleted
                                                                    && x.Name == campusCareerName
                                                                    && x.ScheduleId == scheduleId.Value
                                                                    && x.InstitutionCampusId == campusInstitutionId)
                    .FirstOrDefault();

                if (campusCareer is null)
                {
                    campusCareer = new CareerCampus()
                    {
                        Name = campusCareerName,
                        InstitutionCampusId = campusInstitutionId.Value,
                        ScheduleId = scheduleId.Value
                        // institutionCarreer revisar como linkealo
                    };
                }

                if (campusCareer.CareerCampusStats.All(x => x.YearOfData != YearOfData))
                {
                    var careerCampusStats = new CareerCampusStats();

                    if (int.TryParse(row["Arancel Anual 2024"]?.ToString(), out var anualTuition))
                        careerCampusStats.AnualTuition =
                            anualTuition; // $ 110.000 el parse talvez va tener que ser distinto

                    if (int.TryParse(row["Costo de titulación"]?.ToString(), out var graduationFee))
                        careerCampusStats.GraduationFee =
                            graduationFee; // $ 110.000 el parse talvez va tener que ser distinto

                    if (int.TryParse(row["Duración Formal (semestres)"]?.ToString(), out var duration))
                        careerCampusStats.Duration = duration;

                    if (int.TryParse(row["Matrícula Total Masculina 2023"]?.ToString(), out var maleEnrollment))
                        careerCampusStats.MaleEnrollment = maleEnrollment;

                    if (int.TryParse(row["Matrícula Total Femenina 2023"]?.ToString(), out var femaleEnrollment))
                        careerCampusStats.FemaleEnrollment = femaleEnrollment;

                    if (int.TryParse(row["Matrícula Total 2023"]?.ToString(), out var totalEnrollment))
                        careerCampusStats.TotalEnrollment = totalEnrollment;

                    if (decimal.TryParse(row["Municipal y Servicios Locales"]?.ToString(), out var publicSchoolRate))
                        careerCampusStats.PublicSchoolRate = publicSchoolRate;

                    if (decimal.TryParse(row["Particular Subvencionado"]?.ToString(), out var subsidizedSchoolRate))
                        careerCampusStats.SubsidizedSchoolRate = subsidizedSchoolRate;

                    if (decimal.TryParse(row["Particular Pagado"]?.ToString(), out var privateSchoolRate))
                        careerCampusStats.PrivateSchoolRate = privateSchoolRate;

                    if (int.TryParse(row["Titulación Femenina 2022"]?.ToString(), out var femaleDegrees))
                        careerCampusStats.FemaleDegrees = femaleDegrees;

                    if (int.TryParse(row["Titlación Masculina 2022"]?.ToString(), out var maleDegrees))
                        careerCampusStats.MaleDegrees = maleDegrees;

                    if (int.TryParse(row["Titulación Total 2022"]?.ToString(), out var totalDegrees))
                        careerCampusStats.TotalDegrees = totalDegrees;

                    var firstYearEntryRange = row["Rango ingreso a 1er año con PAES 2023"]?.ToString();
                    
                    var matchesRange = Regex.Matches(firstYearEntryRange, @"\d+");

                    if (matchesRange.Count >= 2)
                    {
                        careerCampusStats.FirstYearEntryFrom = int.Parse(matchesRange[0].Value);
                        careerCampusStats.FirstYearEntryTo = int.Parse(matchesRange[1].Value);
                    }
                    
                    if (decimal.TryParse(row["Promedio PAES 2023 de Matrícula 1er año 2023"]?.ToString(), out var avargePaes))
                        careerCampusStats.AvargePaes = avargePaes;
                    
                    if (decimal.TryParse(row["Promedio NEM 2023 de Matrícula 2023"]?.ToString(), out var avarageEnrollment))
                        careerCampusStats.AvarageEnrollment = avarageEnrollment;
                    
                    if (int.TryParse(row["Vacantes 1er semestre "]?.ToString(), out var vacanciesFirstSemester))
                        careerCampusStats.VacanciesFirstSemester = vacanciesFirstSemester;
                    
                    if (int.TryParse(row["NEM"]?.ToString(), out var nem))
                        careerCampusStats.Nem = nem;
                    
                    if (int.TryParse(row["Ranking"]?.ToString(), out var ranking))
                        careerCampusStats.Ranking = ranking;
                    
                    if (int.TryParse(row["PAES Lenguaje"]?.ToString(), out var paesLanguaje))
                        careerCampusStats.PaesLanguaje = paesLanguaje;
                    
                    if (int.TryParse(row["PAES Matemáticas"]?.ToString(), out var paesMaths))
                        careerCampusStats.PaesMaths = paesMaths;
                    
                    if (int.TryParse(row["PAES Matemáticas 2"]?.ToString(), out var paesMaths2))
                        careerCampusStats.PaesMaths2 = paesMaths2;
                    
                    if (int.TryParse(row["PAES Historia"]?.ToString(), out var paesHistory))
                        careerCampusStats.PaesHistory = paesHistory;
                    
                    if (int.TryParse(row["PAES Ciencias"]?.ToString(), out var paesSciences))
                        careerCampusStats.PaesSciences = paesSciences;
                    
                    if (int.TryParse(row["Otros"]?.ToString(), out var others)) // estas columnas tienen espacio, talve no las agarra
                        careerCampusStats.Others = others;
                    
                    careerCampusStats.YearOfData = YearOfData;

                    campusCareer.CareerCampusStats.Add(careerCampusStats);
                }

                result.Add(campusCareer);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return result;
    }

    private static DateTime? ExtractDate(string date)
    {
        int hastaIndex = date.IndexOf("hasta", StringComparison.Ordinal);

        if (hastaIndex != -1)
        {
            string datePart = date.Substring(hastaIndex, "hasta".Length).Trim(); // creo que deberia ocupar date

            var culture = new CultureInfo("es-ES");

            if (DateTime.TryParseExact(datePart, "d 'de' MMMM 'de' yyyy", culture, DateTimeStyles.None,
                    out DateTime parsedDate))
            {
                return parsedDate;
            }
        }

        return null;
    }
}