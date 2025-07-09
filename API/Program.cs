using API.Abstractions;
using API.Implementations;
using API.Implementations.Repository;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null)
                    .MaxBatchSize(20);
            })
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        .EnableServiceProviderCaching(false));

builder.Services.AddScoped<IAssessmentDomain, AssessmentDomain>();
builder.Services.AddScoped<IStudentDomain, StudentDomain>();
builder.Services.AddScoped<IQuestionsDomain, QuestionsDomain>();
builder.Services.AddScoped<ICatalogsDomain, CatalogsDomain>();
builder.Services.AddScoped<IInstitutionDomain, InstitutionDomain>();
builder.Services.AddScoped<ICareerDomain, CareerDomain>();
builder.Services.AddTransient<IUploadDataDomain, UploadDataDomain>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IInstitutionRepository, InstitutionRepository>();
builder.Services.AddScoped<ICareerInstitutionRepository, CareerInstitutionRepository>();
builder.Services.AddScoped<ICareerCampusRepository, CareerCampusRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://tufuturo-frontend-gxgsfcbcapfrashs.canadacentral-01.azurewebsites.net")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .Select(e => new ValidationError()
            {
                Identifier = e.Key,
                ErrorMessage = string.Join(",", e.Value?.Errors.Select(x => x.ErrorMessage) ?? [])
            });

        return new BadRequestObjectResult(errors);
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();