using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using API.Abstractions;
using API.Implementations.Repository.Entities;
using Ardalis.Result;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

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
    private readonly ICareerCampusRepository _careerCampusRepository;
    private readonly IRepository<Region> _regionRepository;
    private readonly IRepository<Schedule> _scheduleRepository;
    private readonly ILogger<UploadDataDomain> _logger;
    private const int YearOfData = 2025;

    public UploadDataDomain(
        IRepository<Institution> institutionRepository,
        IRepository<InstitutionType> institutionTypeRepository,
        IRepository<AcreditationType> acreditationTypeRepository,
        IRepository<Career> careerRepository,
        IRepository<KnowledgeArea> knowledgeAreaRepository,
        IRepository<CareerInstitution> careerInstitutionRepository,
        IRepository<InstitutionCampus> institutionCampusRepository,
        ICareerCampusRepository careerCampusRepository,
        IRepository<Region> regionRepository,
        IRepository<Schedule> scheduleRepository, ILogger<UploadDataDomain> logger)
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
        _logger = logger;

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public async Task<Result> UploadInstitutions(Stream file)
    {
        _logger.LogInformation("Uploading institutions");
        var acreditationTypes = await _acreditationTypeRepository.GetAllAsync();

        var institutionTypes = await _institutionTypeRepository.GetAllAsync();

        var institutions = ParseExcelToInstitutions(file, institutionTypes.ToList(), acreditationTypes.ToList());

        _logger.LogInformation("Found {Count} institutions to upload", institutions.Count);

        await _institutionRepository.AddRangeAsync(institutions);

        _logger.LogInformation("Institutions uploaded successfully");
        return Result.Success();
    }

    public async Task<Result> UploadGenericsCareers(Stream file)
    {
        _logger.LogInformation("Uploading generic careers");
        var knowledgeAreas = await _knowledgeAreaRepository.GetAllAsync();

        var careers = ParseExcelToGenericCareer(file, knowledgeAreas.ToList());

        _logger.LogInformation("Found {Count} generic careers to upload", careers.Count);

        await _careerRepository.AddRangeAsync(careers);

        _logger.LogInformation("Generic careers uploaded successfully");
        return Result.Success();
    }

    public async Task<Result> UploadCareersInstitution(Stream file)
    {
        _logger.LogInformation("Uploading careers institution");
        var careers = await _careerRepository.GetAllAsync();

        var institutions = await _institutionRepository.GetAllAsync();

        var careersInstitutions = ParseExcelToCareerInstitution(file, institutions.ToList(), careers.ToList());

        _logger.LogInformation("Found {Count} careers institution to upload", careersInstitutions.Count);

        await _careerInstitutionRepository.AddRangeAsync(careersInstitutions);

        _logger.LogInformation("Careers institution uploaded successfully");
        return Result.Success();
    }

    public async Task<Result> UploadInstitutionCampus(Stream file)
    {
        _logger.LogInformation("Uploading institution campus");
        var institutions = await _institutionRepository.GetAllAsync();

        var regions = await _regionRepository.GetAllAsync();

        var institutionsCampus = ParseExcelToInstitutionCampus(file, regions.ToList(), institutions.ToList());

        _logger.LogInformation("Found {Count} institution campus to upload", institutionsCampus.Count);

        await _institutionCampusRepository.AddRangeAsync(institutionsCampus);

        _logger.LogInformation("Institution campus uploaded successfully");
        return Result.Success();
    }

    public async Task<Result> UploadCareersCampus(Stream file)
    {
        _logger.LogInformation("Uploading careers campus");
        var careerInstitutions = await _careerInstitutionRepository.GetAllAsync();

        var campuses = await _institutionCampusRepository.GetAllAsync();

        var schedules = await _scheduleRepository.GetAllAsync();

        var careersCampus =
            ParseExcelToCareersCampus(file, careerInstitutions.ToList(), schedules.ToList(), campuses.ToList());

        _logger.LogInformation("Found {Count} careers campus to upload", careersCampus.Count);

        await _careerCampusRepository.AddRangeAsync(careersCampus);

        _logger.LogInformation("Careers campus uploaded successfully");
        return Result.Success();
    }

    private List<Institution> ParseExcelToInstitutions(Stream file, List<InstitutionType> institutionTypes,
        List<AcreditationType> acreditationTypes)
    {
        _logger.LogInformation("Parsing excel to institutions");
        List<Institution> result = new List<Institution>();

        using var reader = ExcelReaderFactory.CreateReader(file, new ExcelReaderConfiguration()
        {
            FallbackEncoding = Encoding.UTF8
        });

        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
        {
            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
            {
                UseHeaderRow = true
            }
        });

        DataTable table = dataSet.Tables[0];
        _logger.LogInformation("Found {Count} rows in the excel file", table.Rows.Count);

        foreach (DataRow row in table.Rows)
        {
            var instituionName = row["Nombre institución"]?.ToString();
            var instituionCode = row["Código institución"]?.ToString();
            var institutionType = row["Tipo de institución"]?.ToString();

            // var institution = _institutionRepository.Get(x => !x.IsDeleted
            //                                                   && x.Name == instituionName
            //                                                   && x.Code == instituionCode)
            //     .FirstOrDefault();

            Institution? institution = null;

            if (institution == null)
            {
                institution = new Institution()
                {
                    Name = instituionName,
                    Code = instituionCode,
                };

                var institutionTypeId = institutionTypes
                    .FirstOrDefault(x => x.Name == institutionType)?.Id;

                if (institutionTypeId is null)
                {
                    _logger.LogWarning("Institution type not found for {InstitutionType}", institutionType);
                    continue;
                }

                institution.InstitutionTypeId = institutionTypeId.Value;
            }

            if (institution.InstitutionDetails.Any(x => x.YearOfData == YearOfData))
            {
                _logger.LogWarning("Institution detail already exists for {InstitutionName} and year {YearOfData}",
                    institution.Name, YearOfData);
                continue;
            }

            var instituionDetail = new InstitutionDetails();

            var acreditationType = acreditationTypes.FirstOrDefault(x =>
                x.Name == row["Acreditación (31 de octubre de 2024)"]?.ToString());

            if (acreditationType is null)
            {
                _logger.LogWarning("Acreditation type not found for {AcreditationType}",
                    row["Acreditación (31 de octubre de 2024)"]?.ToString());
                continue;
            }

            instituionDetail.AcreditationTypeId = acreditationType.Id;

            List<string> acreditationNames = new List<string>()
            {
                "Acreditación Extendida",
                "Acreditada"
            };

            if (acreditationNames.Contains(acreditationType.Name))
            {
                if (int.TryParse(row["Años acreditación (31 de octubre de 2024)"]?.ToString(),
                        out int acreditation))
                {
                    instituionDetail.Acreditation = acreditation;
                }

                var expireAt = row["Vigencia acreditación (31 de octubre de 2024)"]?.ToString();

                instituionDetail.AcreditationExpireAt = ExtractDate(expireAt);
            }

            if (decimal.TryParse(row["m² construidos"]?.ToString(), out decimal builded))
            {
                instituionDetail.Builded = builded;
            }

            if (decimal.TryParse(row["m² construidos biblioteca"]?.ToString(), out decimal buildedLibrary))
            {
                instituionDetail.BuildedLibrary = buildedLibrary;
            }

            if (decimal.TryParse(row["m² construidos laboratorios y talleres"]?.ToString(),
                    out decimal buildedLabs))
            {
                instituionDetail.BuildedLabs = buildedLabs;
            }

            if (int.TryParse(row["N° de laboratorios y talleres"]?.ToString(), out int labs))
            {
                instituionDetail.Labs = labs;
            }

            if (int.TryParse(row["N° de computadores"]?.ToString(), out int computersPerStudent))
            {
                instituionDetail.ComputersPerStudent = computersPerStudent;
            }

            if (int.TryParse(row["m² áreas verdes y esparcimiento"]?.ToString(), out int greenArea))
            {
                instituionDetail.GreenArea = greenArea;
            }

            instituionDetail.YearOfData = YearOfData;

            institution.InstitutionDetails.Add(instituionDetail);

            result.Add(institution);
        }


        return result;
    }

    private List<Career> ParseExcelToGenericCareer(Stream file, List<KnowledgeArea> knowledgeAreas)
    {
        _logger.LogInformation("Parsing excel to generic careers");
        List<Career> result = new List<Career>();

        using var reader = ExcelReaderFactory.CreateReader(file);
        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
        {
            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
            {
                UseHeaderRow = true
            }
        });

        DataTable table = dataSet.Tables[0];
        _logger.LogInformation("Found {Count} rows in the excel file", table.Rows.Count);

        var distinctRows = table.AsEnumerable()
            .GroupBy(row => row["Área Carrera Genérica"]?.ToString())
            .Select(group => group.First())
            .ToList();

        _logger.LogInformation("Found {Count} distinct generic careers", distinctRows.Count);

        foreach (DataRow row in distinctRows)
        {
            var knowlageArea = row["Área del conocimiento"]?.ToString();
            var genericCareer = row["Área Carrera Genérica"]?.ToString();

            // var career = _careerRepository.Get(x => !x.IsDeleted
            //                                         && x.Name == genericCareer)
            //     .FirstOrDefault();

            Career? career = null;

            if (career == null)
            {
                career = new Career()
                {
                    Name = genericCareer
                };

                var knowlageAreaId = knowledgeAreas
                    .FirstOrDefault(x => x.Name == knowlageArea)?.Id;

                if (knowlageAreaId is null)
                {
                    _logger.LogWarning("Knowledge area not found for {KnowledgeArea}", knowlageArea);
                    continue;
                }

                career.KnowledgeAreaId = knowlageAreaId.Value;
            }

            result.Add(career);
        }


        return result;
    }

    private List<CareerInstitution> ParseExcelToCareerInstitution(Stream file, List<Institution> institutions,
        List<Career> careers)
    {
        _logger.LogInformation("Parsing excel to career institution");
        List<CareerInstitution> result = new List<CareerInstitution>();

        using var reader = ExcelReaderFactory.CreateReader(file);
        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
        {
            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
            {
                UseHeaderRow = true
            }
        });

        DataTable table = dataSet.Tables[0];
        _logger.LogInformation("Found {Count} rows in the excel file", table.Rows.Count);

        foreach (DataRow row in table.Rows)
        {
            dynamic rowData = new ExpandoObject();
            rowData.GenericCareerName = row["Nombre carrera genérica"]?.ToString();
            rowData.InstitutionCareerName = row["Nombre carrera (del título)"]?.ToString();
            rowData.InstitutionCode = row["Código"]?.ToString();

            CareerInstitution? careerInstitution = null;

            if (careerInstitution == null)
            {
                careerInstitution = new CareerInstitution()
                {
                    Name = rowData.InstitutionCareerName
                };

                var genericCareerId = careers
                    .FirstOrDefault(x => x.Name == rowData.GenericCareerName)?.Id;

                if (genericCareerId is null)
                {
                    _logger.LogWarning("Generic career not found for {GenericCareerName}",
                        row["Nombre carrera genérica"]?.ToString());
                    continue;
                }

                careerInstitution.CarrerId = genericCareerId.Value;

                var institutionId = institutions
                    .FirstOrDefault(x => x.Code == rowData.InstitutionCode)?.Id;

                if (institutionId is null)
                {
                    _logger.LogWarning("Institution not found for {InstitutionCode}", row["Código"]?.ToString());
                    continue;
                }

                careerInstitution.InstitutionId = institutionId.Value;
            }

            if (careerInstitution.CareerInstitutionStats.Any(x => x.YearOfData == YearOfData))
            {
                _logger.LogWarning(
                    "Career institution stats already exists for {CareerInstitutionName} and year {YearOfData}",
                    careerInstitution.Name, YearOfData);
                continue;
            }

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

            if (matches.Count > 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    var avarageSalaryNumber = 0;

                    int millones = matches[i].Groups[1].Success ? int.Parse(matches[i].Groups[1].Value) : 0;
                    int miles = matches[i].Groups[2].Success ? int.Parse(matches[i].Groups[2].Value) : 0;

                    if (millones > 0)
                    {
                        avarageSalaryNumber += millones * 1_000_000;
                    }

                    if (miles > 0)
                    {
                        avarageSalaryNumber += miles * 1_000;
                    }

                    if (i == 0)
                    {
                        careerInstitutionStats.AvarageSalaryFrom = avarageSalaryNumber;
                        avarageSalaryNumber = 0;
                    }
                    else if (i == 1)
                    {
                        careerInstitutionStats.AvarageSalaryTo = avarageSalaryNumber;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            careerInstitutionStats.YearOfData = YearOfData;

            // careerInstitution.CareerInstitutionStats.Add(careerInstitutionStats);

            result.Add(careerInstitution);
        }

        return result;
    }

    private List<InstitutionCampus> ParseExcelToInstitutionCampus(Stream file, List<Region> regions,
        List<Institution> institutions)
    {
        _logger.LogInformation("Parsing excel to institution campus");
        List<InstitutionCampus> result = new List<InstitutionCampus>();

        using var reader = ExcelReaderFactory.CreateReader(file);
        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
        {
            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
            {
                UseHeaderRow = true
            }
        });

        DataTable table = dataSet.Tables[0];
        _logger.LogInformation("Found {Count} rows in the excel file", table.Rows.Count);

        var distinctRows = table.AsEnumerable()
            .GroupBy(row => row.Field<string>("Sede"))
            .Select(group => group.First())
            .ToList();

        _logger.LogInformation("Found {Count} distinct institution campus", distinctRows.Count);

        foreach (DataRow row in distinctRows)
        {
            var campusName = row["Sede"]?.ToString();

            var institutionCode = row["Código institución "]?.ToString();

            var institutionId = institutions.FirstOrDefault(x => x.Code == institutionCode)?.Id;

            if (institutionId is null)
            {
                _logger.LogWarning("Institution not found for {InstitutionCode}", institutionCode);
                continue;
            }

            // var campus = _institutionCampusRepository.Get(x => !x.IsDeleted && x.Name == campusName)
            //     .FirstOrDefault();

            InstitutionCampus? campus = null;

            if (campus is null)
            {
                var regionId = regions.FirstOrDefault(x => x.Name == row["Región"]?.ToString() && !x.IsDeleted)?.Id;

                if (regionId is null)
                {
                    _logger.LogWarning("Region not found for {Region}", row["Región"]?.ToString());
                    continue;
                }

                campus = new InstitutionCampus()
                {
                    Name = campusName,
                    RegionId = regionId.Value,
                    InstitutionId = institutionId.Value
                };

                result.Add(campus);
            }
        }


        return result;
    }

    private List<CareerCampus> ParseExcelToCareersCampus(Stream file, List<CareerInstitution> careerInstitutions,
        List<Schedule> schedules,
        List<InstitutionCampus> institutionCampus)
    {
        _logger.LogInformation("Parsing excel to careers campus");
        List<CareerCampus> result = new List<CareerCampus>();

        using var reader = ExcelReaderFactory.CreateReader(file);
        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
        {
            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
            {
                UseHeaderRow = true
            }
        });

        DataTable table = dataSet.Tables[0];
        _logger.LogInformation("Found {Count} rows in the excel file", table.Rows.Count);

        foreach (DataRow row in table.Rows)
        {
            var campusName = row["Sede"]?.ToString();

            var campusCareerName = row["Nombre carrera"]?.ToString();

            if (string.IsNullOrWhiteSpace(campusName))
            {
                _logger.LogWarning("Campus name is empty");
                continue;
            }

            var scheduleName = row["Jornada"]?.ToString();

            var genericCareerName = row["Área Carrera Genérica"]?.ToString();

            var scheduleId = schedules.FirstOrDefault(x => x.Name == scheduleName)?.Id;

            if (scheduleId is null)
            {
                _logger.LogWarning("Schedule not found for {ScheduleName}", scheduleName);
                continue;
            }

            var campusInstitution = institutionCampus.FirstOrDefault(x => x.Name == campusName);

            if (campusInstitution is null)
            {
                _logger.LogWarning("Campus institution not found for {CampusName}", campusName);
                continue;
            }

            var careerInstitutionId = careerInstitutions
                .FirstOrDefault(x =>
                    x.Name == genericCareerName && x.InstitutionId == campusInstitution.InstitutionId)?.Id;

            if (careerInstitutionId is null)
            {
                _logger.LogWarning(
                    "Career institution not found for {GenericCareerName} and institution {InstitutionId}",
                    genericCareerName, campusInstitution.InstitutionId);
                continue;
            }

            // var campusCareer = _careerCampusRepository.Get(x => !x.IsDeleted
            //                                                     && x.Name == campusCareerName
            //                                                     && x.ScheduleId == scheduleId.Value
            //                                                     && x.InstitutionCampusId == campusInstitutionId)
            //     .FirstOrDefault();

            CareerCampus? campusCareer = null;

            if (campusCareer is null)
            {
                campusCareer = new CareerCampus()
                {
                    Name = campusCareerName!,
                    InstitutionCampusId = campusInstitution.Id,
                    ScheduleId = scheduleId.Value,
                    CareerInstitutionId = careerInstitutionId.Value,
                    IsDeleted = false
                };
            }

            if (campusCareer.CareerCampusStats.All(x => x.YearOfData != YearOfData))
            {
                var careerCampusStats = new CareerCampusStats();

                if (int.TryParse(row["Arancel Anual 2024"]?.ToString(), out var anualTuition))
                    careerCampusStats.AnualTuition =
                        anualTuition;

                if (int.TryParse(row["Costo de titulación"]?.ToString(), out var graduationFee))
                    careerCampusStats.GraduationFee =
                        graduationFee;

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

                if (decimal.TryParse(row["Particular Subvencionado "]?.ToString(), out var subsidizedSchoolRate))
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

                if (decimal.TryParse(row["Promedio PAES 2023 de Matrícula 1er año 2023"]?.ToString(),
                        out var avargePaes))
                    careerCampusStats.AvargePaes = avargePaes;

                if (decimal.TryParse(row["Promedio NEM 2023 de Matrícula 2023"]?.ToString(),
                        out var avarageEnrollment))
                    careerCampusStats.AvarageEnrollment = avarageEnrollment;

                if (int.TryParse(row["Vacantes 1er semestre "]?.ToString(), out var vacanciesFirstSemester))
                    careerCampusStats.VacanciesFirstSemester = vacanciesFirstSemester;

                if (int.TryParse(row["NEM"]?.ToString(), out var nem))
                    careerCampusStats.Nem = nem;

                if (int.TryParse(row["Ranking "]?.ToString(), out var ranking))
                    careerCampusStats.Ranking = ranking;

                if (int.TryParse(row["PAES Lenguaje"]?.ToString(), out var paesLanguaje))
                    careerCampusStats.PaesLanguaje = paesLanguaje;

                if (int.TryParse(row["PAES Matemáticas "]?.ToString(), out var paesMaths))
                    careerCampusStats.PaesMaths = paesMaths;

                if (int.TryParse(row["PAES Matemáticas 2"]?.ToString(), out var paesMaths2))
                    careerCampusStats.PaesMaths2 = paesMaths2;

                if (int.TryParse(row["PAES Historia "]?.ToString(), out var paesHistory))
                    careerCampusStats.PaesHistory = paesHistory;

                if (int.TryParse(row["PAES Ciencias "]?.ToString(), out var paesSciences))
                    careerCampusStats.PaesSciences = paesSciences;

                if (int.TryParse(row["Otros "]?.ToString(),
                        out var others))
                    careerCampusStats.Others = others;

                careerCampusStats.YearOfData = YearOfData;

                // campusCareer.CareerCampusStats.Add(careerCampusStats);
            }

            result.Add(campusCareer);
        }


        return result;
    }

    private static DateTime? ExtractDate(string date)
    {
        int hastaIndex = date.IndexOf("hasta", StringComparison.Ordinal);

        if (hastaIndex != -1)
        {
            string datePart = date.Substring(hastaIndex + 6).Trim();

            var culture = new CultureInfo("es-ES");

            if (DateTime.TryParseExact(datePart, "d 'de' MMMM 'de' yyyy", culture, DateTimeStyles.None,
                    out DateTime parsedDate))
            {
                return DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
            }
        }

        return null;
    }
}